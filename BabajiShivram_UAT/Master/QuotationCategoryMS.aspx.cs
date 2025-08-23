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
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

public partial class Master_QuotationCategoryMS : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(gvQuotationCatg);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Quotation Category Setup";

            lblresult.Visible = false;
            FillQuoteCatg();
        }
    }

    protected void gvQuotationCatg_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblresult.Visible = true;
        if (e.CommandName == "Insert")
        {
            TextBox txtQuotationCatg = gvQuotationCatg.FooterRow.FindControl("txtQuotationCatgfooter") as TextBox;
            DropDownList ddlServiceType = gvQuotationCatg.FooterRow.FindControl("ddlServiceTypeFooter") as DropDownList;

            if (txtQuotationCatg.Text.Trim() != "")
            {
                int result = QuotationOperations.AddQuotationCatgMS(txtQuotationCatg.Text.Trim(), Convert.ToInt32(ddlServiceType.SelectedValue), LoggedInUser.glUserId);
                if (result == 1)
                {
                    lblresult.Text = txtQuotationCatg.Text.Trim() + " Category Added Successfully..!!";
                    lblresult.CssClass = "success";
                    FillQuoteCatg();
                }
                else if (result == 2)
                {
                    lblresult.Text = "Quotation Category Already Exist!";
                    lblresult.CssClass = "warning";
                }
                else
                {
                    lblresult.Text = "System Error! Please Try After Sometime!";
                    lblresult.CssClass = "errorMsg";
                }


            }//END_IF
            else
            {
                lblresult.CssClass = "errorMsg";
                lblresult.Text = " Please Enter Quotation Category Name.";
                txtQuotationCatg.Focus();
            }
        }
        else if (e.CommandName.ToLower().ToString() == "downloaddoc")
        {
            string path = e.CommandArgument.ToString();
            DownloadDocument(path);
        }
    }

    protected void gvQuotationCatg_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int Lid = Convert.ToInt32(gvQuotationCatg.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtQuotationCatg = (TextBox)gvQuotationCatg.Rows[e.RowIndex].FindControl("txtQuotationCatg");
        DropDownList ddlServiceType = (DropDownList)gvQuotationCatg.Rows[e.RowIndex].FindControl("ddlServiceType");

        if (txtQuotationCatg.Text.Trim() != "" && txtQuotationCatg.Text.Trim() != "")
        {
            int result = QuotationOperations.UpdateQuotationCatgMS(Lid, txtQuotationCatg.Text.Trim(), Convert.ToInt32(ddlServiceType.SelectedValue), LoggedInUser.glUserId);
            if (result == 1)
            {
                lblresult.CssClass = "success";
                lblresult.Text = txtQuotationCatg.Text.Trim() + " Category Updated Successfully..!!";
                gvQuotationCatg.EditIndex = -1;
                FillQuoteCatg();
            }
            else if (result == 2)
            {
                lblresult.Text = "Quotation Category Does Not Exists..!!";
                lblresult.CssClass = "warning";
            }
            else
            {
                lblresult.Text = "System Error! Please Try After Sometime.";
                lblresult.CssClass = "errorMsg";
            }

        }//END_IF
        else
        {
            lblresult.CssClass = "errorMsg";
            lblresult.Text = " Please Enter Quotation Category.";
        }
    }

    protected void gvQuotationCatg_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblresult.Visible = true;
        int lid = Convert.ToInt32(gvQuotationCatg.DataKeys[e.RowIndex].Values["lid"].ToString());
        int result = QuotationOperations.DeleteQuotationCatg(lid, LoggedInUser.glUserId);
        if (result == 2)
        {
            lblresult.Text = "Quotation Category Deleted Successfully!";
            lblresult.CssClass = "success";
            FillQuoteCatg();
        }
        else if (result == 1)
        {
            lblresult.Text = "System Error! Please Try After Sometime.";
            lblresult.CssClass = "errorMsg";
        }
        else if (result == 3)
        {
            lblresult.Text = "Quotation Category Does Not Exists..!!";
            lblresult.CssClass = "warning";
        }
    }

    protected void FillQuoteCatg()
    {
        DataSet ds = new DataSet();
        ds = QuotationOperations.GetQuotationCatg();

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvQuotationCatg.DataSource = ds;
            gvQuotationCatg.DataBind();
        }
        else
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            gvQuotationCatg.DataSource = ds;
            gvQuotationCatg.DataBind();
            int columncount = gvQuotationCatg.Rows[0].Cells.Count;
            gvQuotationCatg.Rows[0].Cells.Clear();
            gvQuotationCatg.Rows[0].Cells.Add(new TableCell());
            gvQuotationCatg.Rows[0].Cells[0].ColumnSpan = columncount;
            gvQuotationCatg.Rows[0].Cells[0].Text = "No Records Found!";
        }
    }

    protected void gvQuotationCatg_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvQuotationCatg.EditIndex = e.NewEditIndex;
        FillQuoteCatg();
        lblresult.Text = "";
        lblresult.Visible = false;
    }

    protected void gvQuotationCatg_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvQuotationCatg.EditIndex = -1;
        FillQuoteCatg();
        lblresult.Text = "";
        lblresult.Visible = false;
    }

    protected void gvQuotationCatg_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvQuotationCatg.PageIndex = e.NewPageIndex;
        gvQuotationCatg.DataBind();
        FillQuoteCatg();
    }

    protected void gvQuotationCatg_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    #region Documnet Upload/Download/Delete

    public string UploadFiles(FileUpload FU, string FilePath)
    {
        string FileName = FU.FileName;

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\Quotation\\" + FilePath);
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

    protected void DownloadDocument(string DocumentPath)
    {
        //DocumentPath =  DBOperations.GetDocumentPath(Convert.ToInt32(DocumentId));
        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\Quotation\\" + DocumentPath);
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

    protected string RandomString(int size)
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