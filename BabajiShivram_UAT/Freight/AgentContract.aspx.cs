using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
public partial class Freight_AgentContract : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Agent Contract";

        if (!IsPostBack)
        {
           // fill Agent Deatil
            DBOperations.FillCompanyByCategory(ddAgent, Convert.ToInt32(EnumCompanyType.Agent));
        }
    }

    #region Notes
    protected void btnAgentContractAdd_Click(object sender, EventArgs e)
    {
        int AgentId = Convert.ToInt32(ddAgent.SelectedValue);
        string cNotes = txtNotes.Text.Trim();
        string strFilePath = "";
        string strFileFolder = hdnCustFilePath.Value.Trim();

        int NoteType = 2; // 1 for document, 2 for Agent Contract

        DateTime StartDate = DateTime.MinValue;
        DateTime ValidTillDate = DateTime.MinValue;

        if (txtStartDate.Text.Trim() != "")
        {
            StartDate = Commonfunctions.CDateTime(txtStartDate.Text.Trim());
        }
        if (txtValidDate.Text.Trim() != "")
        {
            ValidTillDate = Commonfunctions.CDateTime(txtValidDate.Text.Trim());
        }

        if (fuNotesDoc.HasFile)
        {
            // Upload Customer Notes File To Customer Folder

            strFileFolder = strFileFolder + "\\NoteFiles\\";

            strFilePath = UploadNotesFiles(fuNotesDoc, strFileFolder);
        }

        int outVal = DBOperations.AddCustomerNotes(AgentId, cNotes, strFilePath, NoteType, StartDate, ValidTillDate, LoggedInUser.glUserId);

        if (outVal == 0)
        {
            lberror.Text = "Agent Contract Added Successfully !";
            lberror.CssClass = "success";
            gvAgentContract.DataSourceID = "DataSourceAgentContract";
            gvAgentContract.DataBind();
            txtNotes.Text = string.Empty;
            ddAgent.SelectedIndex = 0;
            txtStartDate.Text = string.Empty;
            txtValidDate.Text = string.Empty;

        }
        else if (outVal == 1)
        {
            lberror.Text = "System Error! Please try after sometime";
            lberror.CssClass = "errorMsg";
        }
        else if (outVal == 2)
        {
            lberror.Text = "Agent Contract Already Exist!";
            lberror.CssClass = "errorMsg";
        }
    }

    protected void gvAgentContract_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
      
    }
    protected void gvAgentContract_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }


    }
    public string UploadNotesFiles(FileUpload fuDocument, string FilePath)
    {
        string FileName = fuDocument.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        if (FilePath == "")
            FilePath = "NoteFiles\\";

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("UploadFiles\\" + FilePath);
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

    private void DownloadDocument(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\" + DocumentPath);
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
    
    protected void gvAgentContract_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int ContractID = Convert.ToInt32(e.Keys[0].ToString());
        string strNotes = "";

        TextBox txtNotes = (TextBox)gvAgentContract.Rows[e.RowIndex].FindControl("txtNotes");
        TextBox txtEdtStartDate = (TextBox)gvAgentContract.Rows[e.RowIndex].FindControl("txtEdtStartDate");
        TextBox txtEdtValidTill = (TextBox)gvAgentContract.Rows[e.RowIndex].FindControl("txtEdtValidTill");
     
        if(txtNotes.Text.Trim() == "")
        {
            lblError.Text = "Please Enter Contract Detail!";
            lblError.CssClass = "errorMsg";
            e.Cancel = true;
        }
        else
        {
            strNotes = txtNotes.Text.Trim();

            DateTime StartDate = DateTime.MinValue;
            DateTime ValidTillDate = DateTime.MinValue;

            if (txtEdtStartDate.Text.Trim() != "")
            {
                StartDate = Commonfunctions.CDateTime(txtEdtStartDate.Text.Trim());
            }
            if (txtEdtValidTill.Text.Trim() != "")
            {
                ValidTillDate = Commonfunctions.CDateTime(txtEdtValidTill.Text.Trim());
            }

            int result = DBOperations.UpdateCustomerNotes(ContractID, strNotes, StartDate, ValidTillDate, LoggedInUser.glUserId);

            if(result == 0)
            {
                lblError.Text = "Contract details updated successfully!";
                lblError.CssClass = "success";
                gvAgentContract.EditIndex = -1;
                e.Cancel = true;
               
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please try after sometime";
                lblError.CssClass = "errorMsg";
                gvAgentContract.EditIndex = -1;
                e.Cancel = true;
            }
            else if (result == 1)
            {
                lblError.Text = "Contract Details Not Found!";
                lblError.CssClass = "errorMsg";
                gvAgentContract.EditIndex = -1;
                e.Cancel = true;
            }
        }
    }
}