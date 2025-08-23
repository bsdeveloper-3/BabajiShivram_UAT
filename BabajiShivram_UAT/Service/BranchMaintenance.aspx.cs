using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;
public partial class Service_BranchMaintenance : System.Web.UI.Page
{
    private static Random _random = new Random();
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Branch Maintenance";

        if (!IsPostBack)
        {
            MskEdtValWorkDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");

            calWorkDate.StartDate = DateTime.Now.AddMonths(-2);
            calWorkDate.EndDate = DateTime.Now;

            string NewRefNo = DBOperations.GetNewAdminExpenseRefNo(); // Maintenance Ref Only- ET

            legRefNo.InnerText = legRefNo.InnerText + " - " + NewRefNo;

            //MskEdtValWorkDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");
            //MskEdtValWorkEndDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");

            DBOperations.FillBranch(ddBranch);
            DBOperations.FillBranchMaintenanceCategory(lbCategory); // Maintenance Category

            // Additional Branch Only For Admin Expenses
            ListItem listitem1 = new ListItem("MUMBAI - NHAVA SHEVA", "4");
            ListItem listitem2 = new ListItem("MUMBAI - REX", "9");
            ListItem listitem3 = new ListItem("DRONAGIRI WH", "10");
            ListItem listitem4 = new ListItem("AAMBY VALLEY", "32");
            ListItem listitem5 = new ListItem("ROYAL PALMS", "33");

            ddBranch.Items.Add(listitem1);
            ddBranch.Items.Add(listitem2);
            ddBranch.Items.Add(listitem3);
            ddBranch.Items.Add(listitem4);
            ddBranch.Items.Add(listitem5);

            ListItem lstCategory = new ListItem("Maintenance Category", "0");
            lstCategory.Attributes.Add("style", "font-weight:bold");
            lbCategory.Items.Insert(0, lstCategory);

        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int MaintainID = -123;
        int lUserId = LoggedInUser.glUserId;

        string strWorkDesc = "", strCategoryList = "";
        int BranchID = 0;

        DateTime WorkDateStart = DateTime.MinValue; 

        BranchID = Convert.ToInt32(ddBranch.SelectedValue);

        strWorkDesc = txtWorkDesc.Text.Trim();
        
        try
        {
            if (txtWorkDate.Text.Trim() != "")
            {
                WorkDateStart = Commonfunctions.CDateTime(txtWorkDate.Text.Trim());
            }

        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.CssClass = "errorMsg";
            return;
        }

        // Maintenance Category 

        foreach (ListItem item in lbCategory.Items)
        {
            if (item.Selected)
            {
                if (item.Value != "0")
                    strCategoryList += item.Value + ",";
            }
        }


        if (strCategoryList != "")
        {
            MaintainID = DBOperations.AddMaintenanceWorkBranch(WorkDateStart, BranchID, strWorkDesc, strCategoryList, lUserId);

            if (MaintainID > 0)
            {
                lblError.Text = "Details Updated Successfully!";
                lblError.CssClass = "success";

                DataSet dsDetail = DBOperations.GetMaintenanceWorkBranch(MaintainID);

                string TransRefNo = dsDetail.Tables[0].Rows[0]["RefNo"].ToString();
                string strDocFolder = "AdminDoc\\" + TransRefNo + "\\";

                string strDocumentName = txtAttachDocName.Text.Trim();
                string strDocumentName1 = txtAttachDocName1.Text.Trim();
                string strDocumentName2 = txtAttachDocName2.Text.Trim();

                if (fuAttachment.HasFile) // Add Maintenance Document
                {
                    string strFilePath = UploadDocument(strDocFolder, fuAttachment);

                    if (strFilePath != "")
                    {
                        if (strDocumentName == "")
                        {
                            strDocumentName = Path.GetFileNameWithoutExtension(fuAttachment.FileName);
                        }

                        int DocResult = DBOperations.AddMaintenanceDocumentBranch(MaintainID, strDocumentName, strFilePath, LoggedInUser.glUserId);
                    }
                }
                if (fuAttachment1.HasFile) // Add Document 1
                {
                    string strFilePath1 = UploadDocument(strDocFolder, fuAttachment1);

                    if (strFilePath1 != "")
                    {
                        if (strDocumentName1 == "")
                        {
                            strDocumentName1 = Path.GetFileNameWithoutExtension(fuAttachment1.FileName);
                        }

                        int DocResult = DBOperations.AddMaintenanceDocumentBranch(MaintainID, strDocumentName1, strFilePath1, LoggedInUser.glUserId);
                    }
                }
                if (fuAttachment2.HasFile) // Add Document 2
                {
                    string strFilePath2 = UploadDocument(strDocFolder, fuAttachment2);

                    if (strFilePath2 != "")
                    {
                        if (strDocumentName2 == "")
                        {
                            strDocumentName2 = Path.GetFileNameWithoutExtension(fuAttachment2.FileName);
                        }

                        int DocResult = DBOperations.AddMaintenanceDocumentBranch(MaintainID, strDocumentName2, strFilePath2, LoggedInUser.glUserId);
                    }
                }

                ClearField();

                //Response.Redirect("SuccessPage.aspx?Expense=201");
            }
            else if (MaintainID == 0)
            {
                lblError.Text = "Please Select Maintenence Category!";
                lblError.CssClass = "errorMsg";
            }
            else if (MaintainID == -1)
            {
                lblError.Text = "System Error! Please check required fields!";
                lblError.CssClass = "errorMsg";
            }
            else if (MaintainID == -2)
            {
                lblError.Text = "Maintenance detail already created!";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "System Error! Please check the required field";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            lblError.Text = "Please select atleast one Maintenance Category!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnCancel_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("BranchExpense.aspx");
    }

    public string UploadDocument(string FilePath, FileUpload Attachment)
    {
        if (FilePath == "")
        {
            FilePath = "AdminDoc\\";
        }

        string FileName = Attachment.FileName;
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

        if (Attachment.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);
                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            Attachment.SaveAs(ServerFilePath + FileName);
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

    private void ClearField()
    {
        txtWorkDate.Text = "";
        txtWorkDesc.Text = "";
        lbCategory.SelectedIndex = -1;
        txtAttachDocName.Text = "";
        txtAttachDocName1.Text = "";
        txtAttachDocName2.Text = "";
    }
}