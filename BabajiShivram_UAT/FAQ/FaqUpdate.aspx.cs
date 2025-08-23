using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;
public partial class FAQ_FaqUpdate : System.Web.UI.Page
{
    private static Random _random = new Random();
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Update FAQ";

        if (!IsPostBack)
        {
            string strFaqId = "0";

            if (Session["FaqId"] != null)
            {
                strFaqId = Session["FaqId"].ToString();

                FAQServiceDetail(Convert.ToInt32(strFaqId));
              //  DBOperations.FillBranch(ddBranch1);
             
            }

        }
       // GrvContactPerson.DataBind();
    }

    protected void btnBack_Click(object sender ,EventArgs e)
    {
        Response.Redirect("FaqDetails.aspx");
    }
   


    protected void GrvContactPerson_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GrvContactPerson.EditIndex = -1;

    }

    protected void GrvContactPerson_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GrvContactPerson.EditIndex = e.NewEditIndex;

    }

    protected void GrvContactPerson_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int lid = Convert.ToInt32(GrvContactPerson.DataKeys[e.RowIndex].Value.ToString());
        int FaqID = Convert.ToInt32(Session["FaqId"]);

        TextBox TxtName = (TextBox)GrvContactPerson.Rows[e.RowIndex].FindControl("TxtName");
        DropDownList ddBranch = (DropDownList)GrvContactPerson.Rows[e.RowIndex].FindControl("ddBranch");
        TextBox TxtPhonNo = (TextBox)GrvContactPerson.Rows[e.RowIndex].FindControl("txtPhoneNo");
        TextBox TxtEmailId = (TextBox)GrvContactPerson.Rows[e.RowIndex].FindControl("txtEmailID");
        TextBox TxtContacttype = (TextBox)GrvContactPerson.Rows[e.RowIndex].FindControl("txtContacttype");

        string strName = TxtName.Text.Trim();
        int ddbranch = Convert.ToInt32(ddBranch.SelectedValue);
        string strPhonNo = TxtPhonNo.Text.Trim();
        string strEmailId = TxtEmailId.Text.Trim();
        string ContactType= TxtContacttype.Text.Trim();
        int ContactType1 = Convert.ToInt32(TxtContacttype.Text);


        if (TxtName.Text.Trim() != "")
        {


            int result = DBOperations.UpdateFAQContactDetail(lid, FaqID, strName, ddbranch, strPhonNo, ContactType1, strEmailId, LoggedInUser.glUserId);

            if (result == 0)
            {

                Lblerror.Text = TxtName.Text.Trim() + " Contact PersonDetail  Updated Successfully.";
                Lblerror.CssClass = "success";

                GrvContactPerson.EditIndex = -1;
                e.Cancel = true;
               

            }
            else if (result == 1)
            {
                Lblerror.Text = "System Error! Please Try After Sometime.";
                Lblerror.CssClass = "errorMsg";
                e.Cancel = true;
            }
            else if (result == 2)
            {
                Lblerror.Text = "Division Name Already Added!";
                Lblerror.CssClass = "errorMsg";
                e.Cancel = true;
            }
        }//END_IF
        else
        {
            Lblerror.CssClass = "errorMsg";
            Lblerror.Text = " Please Enter Division Name!";
        }
    }

    private void FAQServiceDetail(int FaqId)
    {
        DataSet dsServiceID = DBOperations.GetServiceByAllFaqDetails(Convert.ToInt32(FaqId));
        //DataSet dsServiceID1 = DBOperations.ServiceContactDeatil(Convert.ToInt32(FaqId));

        if (dsServiceID.Tables[0].Rows.Count > 0)
        {
            lblServicess.Text = dsServiceID.Tables[0].Rows[0]["sName"].ToString();
            txtTital.Text = dsServiceID.Tables[0].Rows[0]["Title"].ToString();
            mytextarea.Text = dsServiceID.Tables[0].Rows[0]["Description"].ToString();
            HFfuDoc1.Value =  dsServiceID.Tables[0].Rows[0]["DocPath"].ToString();
            //HFfuDoc2.Value = dsServiceID.Tables[0].Rows[0]["DocPath2"].ToString();
            //HFfuDoc3.Value = dsServiceID.Tables[0].Rows[0]["DocPath3"].ToString();
            HFfuDocForm1.Value = dsServiceID.Tables[0].Rows[0]["DocFormPath"].ToString();
            //HFfuDocForm2.Value = dsServiceID.Tables[0].Rows[0]["DocFormPath2"].ToString();
            //HFfuDocForm3.Value = dsServiceID.Tables[0].Rows[0]["DocFormPath3"].ToString();
        }

    }


    protected void DataSourceContactPerson_Deleted(object sender, SqlDataSourceStatusEventArgs e)
    {
        int Result = Convert.ToInt32(e.Command.Parameters["@outPut"].Value);

        if (Result == 0)
        {
            Lblerror.Text = "FAQ Contact  Successfully Removed!";
            Lblerror.CssClass = "success";

        }
        else if (Result == 1)
        {
            Lblerror.Text = "System Error! Please try after sometime";
            Lblerror.CssClass = "errorMsg";
        }
        else if (Result == 2)
        {
            Lblerror.Text = "FAQ Contact Not Found!";
            Lblerror.CssClass = "errorMsg";
        }
    }
  


    protected void btnUpdate_Click(object sender,EventArgs e)
    {
       
        int Faqid = Convert.ToInt32(Session["FaqId"]);
        string Title = txtTital.Text.Trim();
        string Discrtiption = mytextarea.Text;
        string strfuDoc1Path ="";
        if (fuDoc1.FileName.Trim() != "")
        {
            strfuDoc1Path = UploadDocument(fuDoc1);
         
        }
        else
        {
            strfuDoc1Path = Convert.ToString(HFfuDoc1.Value);
        }
        //string strfuDoc2Path = "";
        //if (fuDoc2.FileName.Trim() != "")
        //{
        //    strfuDoc2Path = UploadDocument(fuDoc2);
        //}
        //else
        //{
        //    strfuDoc2Path = Convert.ToString(HFfuDoc2.Value);
        //}
        //string strfuDoc3Path = "";
        //if (fuDoc3.FileName.Trim() != "")
        //{
        //    strfuDoc3Path = UploadDocument(fuDoc3);
        //}
        //else
        //{
        //    strfuDoc3Path = Convert.ToString(HFfuDoc3.Value);
        //}
        string strfuDocForm1Path = "";
        if (fuDocForm1.FileName.Trim() != "")
        {
            strfuDocForm1Path = UploadDocument(fuDocForm1);
        }
        else
        {
            strfuDocForm1Path = Convert.ToString(HFfuDocForm1.Value);

        }
        //string strfuDocForm2Path = "";
        //if (fuDocForm2.FileName.Trim() != "")
        //{
        //    strfuDocForm2Path = UploadDocument(fuDocForm2);
        //}
        //else
        //{
        //    strfuDocForm2Path = Convert.ToString(HFfuDocForm2.Value);
        //}
        //string strfuDocForm3Path = "";
        //if (fuDocForm3.FileName.Trim() != "")
        //{
        //    strfuDocForm3Path = UploadDocument(fuDocForm3);
        //}
        //else
        //{
        //    strfuDocForm3Path = Convert.ToString(HFfuDocForm3.Value);
        //}

       int   RESULT = DBOperations.UpdateFAQDetail(Faqid, Title, Discrtiption, strfuDoc1Path,
            strfuDocForm1Path, LoggedInUser.glUserId);

       

        if (RESULT > 0)

        {
            Lblerror.Text = "FAQ Update Successfully..!";
          
            txtTital.Text = "";
            mytextarea.Text = "";
           

        }
        else if (RESULT == 0)
        {
            Lblerror.Text = "System Error! ";

        }
        else if (RESULT == -1)
        {
            Lblerror.Text = "FAQ Detaile Already Exists! ";

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



}