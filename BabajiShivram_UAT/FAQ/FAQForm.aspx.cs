using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;

public partial class FAQ_FAQForm : System.Web.UI.Page
{
    private static Random _random = new Random();
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Frequently Asked Question";
      
        if (!IsPostBack)
        {
            DBOperations.FillServicess(ddServicess);
           
            SetInitialRowFAQContact();

        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        int RESULT = 0;
        int ServiceId = Convert.ToInt32(ddServicess.SelectedValue);
        string Title = TxtTitle.Text.Trim();
       // string Subject = TxtSubject.Text.Trim();
        string Discription = mytextarea.Text.Trim();

        string strfuDoc1Path = "";
        if (fuDoc1.FileName.Trim() != "")
        {
            strfuDoc1Path = UploadDocument(fuDoc1);
        }

        string strfuDocForm1Path = "";
        if (fuDocForm1.FileName.Trim() != "")
        {
          strfuDocForm1Path = UploadDocument(fuDocForm1);
       }

        //string strfuDoc2Path = "";
        //if (fuDoc2.FileName.Trim() != "")
        //{
        //    strfuDoc2Path = UploadDocument(fuDoc2);
        //}
        //string strfuDoc3Path = "";
        //if (fuDoc3.FileName.Trim() != "")
        //{
        //    strfuDoc3Path = UploadDocument(fuDoc3);
        //}
        //string strfuDocForm1Path = "";
        //if (fuDocForm1.FileName.Trim() != "")
        //{
        //    strfuDocForm1Path = UploadDocument(fuDocForm1);
        //}
        //string strfuDocForm2Path = "";
        //if (fuDocForm2.FileName.Trim() != "")
        //{
        //    strfuDocForm2Path = UploadDocument(fuDocForm2);
        //}
        //string strfuDocForm3Path = "";
        //if (fuDocForm3.FileName.Trim() != "")
        //{
        //    strfuDocForm3Path = UploadDocument(fuDocForm3);
        //}
        RESULT = DBOperations.AddFAQDescription(ServiceId, Title, Discription, strfuDoc1Path, strfuDocForm1Path, LoggedInUser.glUserId);

        if (RESULT > 0)
        {
            int gridcnt = GrvContactPerson.Rows.Count;

            if (gridcnt > 0)
            {
                for (int i = 0; i < gridcnt; i++)
                {

                    TextBox TxtName = (TextBox)GrvContactPerson.Rows[i].Cells[1].FindControl("TxtName");
                    DropDownList ddBranch = (DropDownList)GrvContactPerson.Rows[i].Cells[2].FindControl("ddBranch");
               
                    TextBox TxtPhoneNo = (TextBox)GrvContactPerson.Rows[i].Cells[3].FindControl("txtPhoneNo");
                    TextBox TxtEmailID = (TextBox)GrvContactPerson.Rows[i].Cells[4].FindControl("txtEmailID");


                    int ContactType = i + 1;
                    string strName = TxtName.Text.Trim();
                    int Branch = Convert.ToInt32(ddBranch.SelectedValue);
                  
                    string strPhonNo = TxtPhoneNo.Text.Trim();
                    string strEmailid = TxtEmailID.Text.Trim();

                    if (strName != "")
                    {
                        int FAQContact = DBOperations.AddFAQContactPersonDeatil(RESULT, strName, Branch, strPhonNo, strEmailid, ContactType, LoggedInUser.glUserId);
                        TxtName.Text = "";
                        TxtPhoneNo.Text = "";
                        ddBranch.SelectedValue = "0";
                        TxtEmailID.Text = "";

                    }

                }
            }

        }

            if (RESULT > 0)

            {
                Lblerror.Text = "FAQ Submited  Successfully!";
                TxtTitle.Text = "";
                ddServicess.SelectedIndex = 0;
                mytextarea.Text = "";
                Lblerror.CssClass = "Success";

            }
            else if (RESULT == 0)
            {
                Lblerror.Text = "System Error! ";

            }
            else if (RESULT == -1)
            {
                Lblerror.Text = "Service Title Already Exists! ";

            }

        }

    private string UploadDocument(FileUpload fileUpload)
    {
        string FileName = fileUpload.FileName;

        FileName = FileServer.ValidateFileName(FileName);

        string FilePath2 = "FAQDoc\\";

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\" + FilePath2);

        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + FilePath2;

        }

        if (!System.IO.Directory.Exists(ServerFilePath))
        {
            System.IO.Directory.CreateDirectory(ServerFilePath);
        }

        if (fileUpload.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {

                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            fileUpload.SaveAs(ServerFilePath + FileName);

            return FilePath2 + FileName;
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

    protected void btnContactAdd_Click(object sender, EventArgs e)
    {
        AddNewRowToFAQContactGrid();

    }

    private void SetInitialRowFAQContact()
    {

        DataTable dt = new DataTable();

        DataRow dr = null;

        dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));

        dt.Columns.Add(new DataColumn("ColName", typeof(string)));

        dt.Columns.Add(new DataColumn("ColBranch", typeof(string)));

        dt.Columns.Add(new DataColumn ("ColPhonNo",typeof(string)));

        dt.Columns.Add(new DataColumn("ColEmailid", typeof(string)));

        dr = dt.NewRow();

        dr["RowNumber"] = 1;

        dr["ColName"] = string.Empty;

        dr["ColBranch"] = string.Empty;

        dr["ColPhonNo"] = string.Empty;

        dr["ColEmailid"] = string.Empty;

        dt.Rows.Add(dr);


        ViewState["CurrentTable1"] = dt;

        GrvContactPerson.DataSource = dt;

        GrvContactPerson.DataBind();

    }

    private void AddNewRowToFAQContactGrid()
    {

        int rowIndex = 0;


        if (ViewState["CurrentTable1"] != null)
        {

            DataTable dtCurrentTable1 = (DataTable)ViewState["CurrentTable1"];

            DataRow drCurrentRow1 = null;

            if (dtCurrentTable1.Rows.Count > 0)
            {

                for (int i = 1; i <= dtCurrentTable1.Rows.Count; i++)
                {

                    TextBox TxtName = (TextBox)GrvContactPerson.Rows[rowIndex].Cells[1].FindControl("TxtName");
                    DropDownList ddBranch = (DropDownList)GrvContactPerson.Rows[rowIndex].Cells[2].FindControl("ddBranch");
                    TextBox TxtPhonNo = (TextBox)GrvContactPerson.Rows[rowIndex].Cells[3].FindControl("txtPhoneNo");
                    TextBox TxtEmailID = (TextBox)GrvContactPerson.Rows[rowIndex].Cells[4].FindControl("txtEmailID");

                    drCurrentRow1 = dtCurrentTable1.NewRow();

                    drCurrentRow1["RowNumber"] = i + 1;

                    dtCurrentTable1.Rows[i - 1]["ColName"] = TxtName.Text;

                    dtCurrentTable1.Rows[i - 1]["ColBranch"] = ddBranch.SelectedValue;

                    dtCurrentTable1.Rows[i - 1]["ColPhonNo"] = TxtPhonNo.Text;

                    dtCurrentTable1.Rows[i - 1]["ColEmailid"] = TxtEmailID.Text;

                    rowIndex++;

                }

                dtCurrentTable1.Rows.Add(drCurrentRow1);
                ViewState["CurrentTable1"] = dtCurrentTable1;
                GrvContactPerson.DataSource = dtCurrentTable1;
                GrvContactPerson.DataBind();

            }

        }

        else
        {

            Response.Write("ViewState is null");

        }

        SetPreviousDataFAQContact();

    }

    private void SetPreviousDataFAQContact()
    {

        int rowIndex = 0;

        if (ViewState["CurrentTable1"] != null)
        {

            DataTable dt = (DataTable)ViewState["CurrentTable1"];

            if (dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                   

                    TextBox TxtName = (TextBox)GrvContactPerson.Rows[rowIndex].Cells[1].FindControl("TxtName");
                    DropDownList ddBranch = (DropDownList)GrvContactPerson.Rows[rowIndex].Cells[2].FindControl("ddBranch");
                    TextBox TxtPhoneNo = (TextBox)GrvContactPerson.Rows[rowIndex].Cells[3].FindControl("txtPhoneNo");
                    TextBox TxtEmailID = (TextBox)GrvContactPerson.Rows[rowIndex].Cells[4].FindControl("txtEmailID");

                    TxtName.Text = dt.Rows[i]["ColName"].ToString();

                    ddBranch.SelectedValue  = dt.Rows[i]["ColBranch"].ToString();

                    TxtPhoneNo.Text = dt.Rows[i]["ColPhonNo"].ToString();

                    TxtEmailID.Text = dt.Rows[i]["ColEmailid"].ToString();

                    rowIndex++;


                }


            }

        }
    }

    protected void GrvContactPerson_RowCreated(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable1"];
            LinkButton lb = (LinkButton)e.Row.FindControl("LinkbtnFaqRemove");
            if (lb != null)
            {
                if (dt.Rows.Count > 0)
                {
                    if (e.Row.RowIndex == dt.Rows.Count - 1)
                    {
                        lb.Visible = false;
                    }
                }
                else
                {
                    lb.Visible = false;
                }
            }
        }
    }

    private void ResetRowID(DataTable dt)
    {
        //DataRow drCurrentRow1 = null;
        int rowIndex = 1;
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow drCurrentRow1 in dt.Rows)
            {
                drCurrentRow1[0] = rowIndex;
                rowIndex++;
            }
        }
    }

    protected void LinkbtnFaqRemove_Click(object sender, EventArgs e)
    {

        LinkButton lb = (LinkButton)sender;
        GridViewRow gvRow = (GridViewRow)lb.NamingContainer;
        int rowIndex = gvRow.RowIndex;
        if (ViewState["CurrentTable1"] != null)
        {

            DataTable dt = (DataTable)ViewState["CurrentTable1"];
            if (dt.Rows.Count > 1)
            {
                if (gvRow.RowIndex < dt.Rows.Count - 1)
                {
                    //Remove the Selected Row data and reset row number  
                    dt.Rows.Remove(dt.Rows[rowIndex]);
                    ResetRowID(dt);
                }
            }

            //Store the current data in ViewState for future reference  
            ViewState["CurrentTable1"] = dt;

            //Re bind the GridView for the updated data  
            GrvContactPerson.DataSource = dt;
            GrvContactPerson.DataBind();
        }

        //Set Previous Data on Postbacks  
        SetPreviousDataFAQContact();
    }

    

}