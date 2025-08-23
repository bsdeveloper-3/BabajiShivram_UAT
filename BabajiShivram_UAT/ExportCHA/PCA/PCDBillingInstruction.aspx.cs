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

public partial class PCA_PCDBillingInstruction : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnSubmit);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int result = 0;
        int JobId, WithoutLRStatus = 0;
        string AlliedAgencyRemark = "", Instruction = "", Instruction1 = "", Instruction2 = "", Instruction3 = "", FilePath = "";
        string AlliedAgencyService = "", OtherService = "", OtherServiceRemark = "", InstructionCopy = "",
            InstructionCopy1 = "", InstructionCopy2 = "", InstructionCopy3 = "";
        //int AlliedAgencyRemarkApply, DeliveyRelatedApply, VASApply, CECertificateApply;

        //JobId = Convert.ToInt32(Session["JobId"]);
        JobId = Convert.ToInt32(hdnJobId.Value);

        for (int i = 0; i < chkAgencyService.Items.Count; i++)
        {
            if (chkAgencyService.Items[i].Selected)
            {
                AlliedAgencyService += chkAgencyService.Items[i].Text + ";";
            }
        }
        AlliedAgencyRemark = txtAgencyServiceRemark.Text;

        for (int i = 0; i < chkOtherService.Items.Count; i++)
        {
            if (chkOtherService.Items[i].Selected)
            {
                OtherService += chkOtherService.Items[i].Text + ";";
            }
        }
        OtherServiceRemark = txtOtherServiceRemark.Text;

        Instruction = txtOtherRemark.Text;
        Instruction1 = txtOtherRemark1.Text;
        Instruction2 = txtOtherRemark2.Text;
        Instruction3 = txtOtherRemark3.Text;

        if (FuInstructionCopy.HasFile)
        {
            FilePath = "BillingInstructionCopy" + "\\" + Session["JobId"] + "\\";
            InstructionCopy = UploadDoc(FuInstructionCopy, FilePath);
        }

        if (FuInstructionCopy1.HasFile)
        {
            FilePath = "BillingInstructionCopy" + "\\" + Session["JobId"] + "\\";
            InstructionCopy1 = UploadDoc(FuInstructionCopy1, FilePath);
        }

        if (FuInstructionCopy2.HasFile)
        {
            FilePath = "BillingInstructionCopy" + "\\" + Session["JobId"] + "\\";
            InstructionCopy2 = UploadDoc(FuInstructionCopy2, FilePath);
        }

        if (FuInstructionCopy3.HasFile)
        {
            FilePath = "BillingInstructionCopy" + "\\" + Session["JobId"] + "\\";
            InstructionCopy3 = UploadDoc(FuInstructionCopy3, FilePath);
        }

        if(hdnModuleId.Value=="1")
        {
            result = DBOperations.BS_AddBillingInstruction(JobId, 0, AlliedAgencyService, AlliedAgencyRemark, 0, "",
            0, "", "", 0, "", "", WithoutLRStatus,
            "", "", Instruction, Instruction1, Instruction2, Instruction3,
            InstructionCopy, InstructionCopy1, InstructionCopy2, InstructionCopy3, OtherService, OtherServiceRemark, LoggedInUser.glUserId);
        }
        else if(hdnModuleId.Value=="5")
        {
            result = DBOperations.EX_AddBillingInstruction(JobId, AlliedAgencyService, AlliedAgencyRemark, OtherService, OtherServiceRemark, Instruction, Instruction1, Instruction2, Instruction3,
                InstructionCopy, InstructionCopy1, InstructionCopy2, InstructionCopy3,LoggedInUser.glUserId);
        }

        if (result == 0)
        {
            ClearFields();
            lblMessage.Text = "Billing Instruction Added Successfully.";
            lblMessage.CssClass = "success";
        }
        else
        {
            lblMessage.Text = "System Error! Please try after sometime!";
            lblMessage.CssClass = "errorMsg";
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }

    protected void txtJobNumber_TextChanged(object sender, EventArgs e)
    {
        if (txtJobNumber.Text.Trim() != "")
        {
            string ModuleId = hdnModuleId.Value;
            if (hdnJobId.Value != "0")
            {

                DataSet dsGetJobDetail = AccountExpense.GetJobdetailById(Convert.ToInt32(hdnJobId.Value), Convert.ToInt32(hdnModuleId.Value));
                if (dsGetJobDetail != null)
                {
                    //Block Fund Request for Job - Flag - IsFundAllowed 1 = Alloswd, ) -Block
                    lblShipmentType.Text = dsGetJobDetail.Tables[0].Rows[0]["ShipmentType"].ToString();
                    lblShipmentCate.Text = dsGetJobDetail.Tables[0].Rows[0]["ShipmentCategory"].ToString();
                    if(ModuleId=="1")
                    {
                        lblConsignee.Text = dsGetJobDetail.Tables[0].Rows[0]["Consignee"].ToString();
                        lblConsigShipper.Text = "Consignee";
                    }
                    else if (ModuleId=="5")
                    {
                        lblConsignee.Text = dsGetJobDetail.Tables[0].Rows[0]["Shipper"].ToString();
                        lblConsigShipper.Text = "Shipper";
                    }
                    lblCust.Text = dsGetJobDetail.Tables[0].Rows[0]["Customer"].ToString();
                }


                DataTable dtBillInstruction = new DataTable();
                dtBillInstruction = DBOperations.Get_BillingInstructionDetail((Convert.ToInt32(hdnJobId.Value)));

                if (dtBillInstruction.Rows.Count > 0)
                {
                    foreach (DataRow rw in dtBillInstruction.Rows)
                    {
                        //lblRefNo.Text = rw["JobRefNo"].ToString();
                        if (rw["AlliedAgencyService"].ToString() == "") { }
                        else
                        {
                            string args = rw["AlliedAgencyService"].ToString();
                            string[] arg = args.Split(';');
                            for (int i = 0; arg.Length - 2 >= i; i++)
                            {
                                string value = arg[i];

                                foreach (ListItem li1 in chkAgencyService.Items)
                                {
                                    if (li1.Text.ToString() == value)
                                    {
                                        chkAgencyService.Items[i].Selected = true;
                                        chkAgencyService.Items[i].Enabled = false;
                                    }

                                }
                            }
                        }


                        if (rw["OtherService"].ToString() == "") { }
                        else
                        {
                            string args = rw["OtherService"].ToString();
                            string[] arg = args.Split(';');
                            for (int i = 0; arg.Length - 2 >= i; i++)
                            {
                                string value = arg[i];

                                foreach (ListItem li1 in chkOtherService.Items)
                                {
                                    if (li1.Text.ToString() == value)
                                    {
                                        chkOtherService.Items[i].Selected = true;
                                        chkOtherService.Items[i].Enabled = false;
                                    }

                                }
                            }
                        }
                        if (rw["AlliedAgencyRemark"].ToString() == "") { txtAgencyServiceRemark.Text = ""; }
                        else
                        {
                            txtAgencyServiceRemark.Enabled = false;
                            txtAgencyServiceRemark.Text = rw["AlliedAgencyRemark"].ToString();
                        };
                        if (rw["OtherServiceRemark"].ToString() == "") { txtOtherServiceRemark.Text = ""; }
                        else
                        {
                            txtOtherServiceRemark.Enabled = false;
                            txtOtherServiceRemark.Text = rw["OtherServiceRemark"].ToString();
                        };
                        if (rw["Instruction"].ToString() == "") { txtOtherRemark.Text = ""; }
                        else
                        {
                            txtOtherRemark.Enabled = false;
                            txtOtherRemark.Text = rw["Instruction"].ToString();
                        };
                        if (rw["Instruction1"].ToString() == "") { txtOtherRemark1.Text = ""; }
                        else
                        {
                            txtOtherRemark1.Enabled = false;
                            txtOtherRemark1.Text = rw["Instruction1"].ToString();
                        };
                        if (rw["Instruction2"].ToString() == "") { txtOtherRemark2.Text = ""; }
                        else
                        {
                            txtOtherRemark2.Enabled = false;
                            txtOtherRemark2.Text = rw["Instruction2"].ToString();
                        };
                        if (rw["Instruction3"].ToString() == "") { txtOtherRemark3.Text = ""; }
                        else
                        {
                            txtOtherRemark3.Enabled = false;
                            txtOtherRemark3.Text = rw["Instruction3"].ToString();
                        };
                        if (rw["InstructionCopy"].ToString() == "")
                        { //txtOtherRemark.Text = ""; 
                        }
                        else
                        {
                            FuInstructionCopy.Enabled = false;
                            //txtOtherRemark.Text = rw["InstructionCopy"].ToString();
                        };
                        if (rw["InstructionCopy1"].ToString() == "") { //txtOtherRemark1.Text = ""; 
                        }
                        else
                        {
                            FuInstructionCopy1.Enabled = false;
                            //txtOtherRemark1.Text = rw["InstructionCopy1"].ToString();
                        };
                        if (rw["InstructionCopy2"].ToString() == "") { //txtOtherRemark2.Text = "";
                        }
                        else
                        {
                            FuInstructionCopy2.Enabled = false;
                           // txtOtherRemark2.Text = rw["InstructionCopy2"].ToString();
                        };
                        if (rw["InstructionCopy3"].ToString() == "") { //txtOtherRemark3.Text = ""; 
                        }
                        else
                        {
                            FuInstructionCopy3.Enabled = false;
                            //txtOtherRemark3.Text = rw["InstructionCopy3"].ToString();
                        };
                    }
                }

               

            }
        }
        else
        {
            lblConsignee.Text = "";
        }
    }

    private string UploadDoc(FileUpload fuDocument, string Filepath)
    {
        string FileName = "";
        FileName = fuDocument.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        string ServerFilePath = FileServer.GetFileServerDir();

        if (Filepath == "")
            Filepath = "BillingInstruction" + Session["JobId"] + "\\";

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\" + Filepath);
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + Filepath;
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

            return Filepath + FileName;
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

    protected void ClearFields()
    {
        chkAgencyService.ClearSelection();
        chkOtherService.ClearSelection();
        txtOtherRemark.Text = "";
        txtOtherRemark1.Text = "";
        txtOtherRemark2.Text = "";
        txtOtherRemark3.Text = "";
        txtOtherServiceRemark.Text = "";
        txtAgencyServiceRemark.Text = "";
        txtJobNumber.Text = "";
        lblConsignee.Text = "";
        lblCust.Text = "";
        lblShipmentCate.Text = "";
        lblShipmentType.Text = "";
        chkAgencyService.Enabled = true;
        chkOtherService.Enabled = true;
        txtOtherServiceRemark.Enabled = true;
        txtOtherRemark.Enabled = true;
        txtOtherRemark1.Enabled = true;
        txtOtherRemark2.Enabled = true;
        txtOtherRemark3.Enabled = true;
        FuInstructionCopy.Enabled = true;
        FuInstructionCopy1.Enabled = true;
        FuInstructionCopy2.Enabled = true;
        FuInstructionCopy3.Enabled = true;
    }

    

}