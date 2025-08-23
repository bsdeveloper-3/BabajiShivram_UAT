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

public partial class Master_TermsConditionMS : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(gvTermsCondition);
        ScriptManager1.RegisterPostBackControl(gvConditionDetail);
        ScriptManager1.RegisterPostBackControl(lnkExport);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Terms & Condition Setup";

            lblresult.Visible = false;
            FillTermsCondition();
        }
    }

    protected void gvTermsCondition_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblresult.Visible = true;
        if (e.CommandName == "Insert")
        {
            string DocPath = "";
            TextBox txtCatgFor_footer = gvTermsCondition.FooterRow.FindControl("txtCatgFor_footer") as TextBox;
            FileUpload fuDoc = gvTermsCondition.FooterRow.FindControl("fuTermsDoc_footer") as FileUpload;

            if (fuDoc != null && fuDoc.HasFile)
            {
                DocPath = UploadFiles(fuDoc, "");
            }

            if (txtCatgFor_footer.Text.Trim() != "" && txtCatgFor_footer.Text.Trim() != "")
            {
                int result = QuotationOperations.AddTermConditionMS(txtCatgFor_footer.Text.Trim(), DocPath, LoggedInUser.glUserId);
                if (result == 1)
                {
                    lblresult.Text = txtCatgFor_footer.Text.Trim() + " Terms & Condition Added Successfully..!!";
                    lblresult.CssClass = "success";
                    FillTermsCondition();
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
                lblresult.Text = " Please Enter Category Name.";
                txtCatgFor_footer.Focus();
                FillTermsCondition();
            }
        }
        else if (e.CommandName.ToLower().ToString() == "getterms")
        {
            int lid = 0;
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });

            if (commandArgs[0].ToString() != "")
                 lid = Convert.ToInt32(commandArgs[0].ToString());
            if (commandArgs[1].ToString() != "")
                lblTermName.Text = commandArgs[1].ToString();

            ViewState["TermsId"] = lid.ToString();
            GetTermsCondition(lid);
            mpeTermConditions.Show();
        }
    }

    protected void gvTermsCondition_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string DocPath = "";
        int Lid = Convert.ToInt32(gvTermsCondition.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtCatgFor = (TextBox)gvTermsCondition.Rows[e.RowIndex].FindControl("txtCatgFor");
        FileUpload fuDoc = (FileUpload)gvTermsCondition.Rows[e.RowIndex].FindControl("fuTermsDoc");

        if (fuDoc != null && fuDoc.HasFile)
        {
            DocPath = UploadFiles(fuDoc, "");
        }

        if (txtCatgFor.Text.Trim() != "" && txtCatgFor.Text.Trim() != "")
        {
            int result = QuotationOperations.UpdateTermConditionMS(Lid, DocPath, LoggedInUser.glUserId);
            if (result == 1)
            {
                lblresult.CssClass = "success";
                lblresult.Text = txtCatgFor.Text.Trim() + " Terms & Condition Updated Successfully..!!";
                gvTermsCondition.EditIndex = -1;
                FillTermsCondition();
            }
            else if (result == 2)
            {
                lblresult.Text = "Category Does Not Exists..!!";
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
            lblresult.Text = " Please Enter Category Name.";
        }
    }

    protected void gvTermsCondition_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblresult.Visible = true;
        int lid = Convert.ToInt32(gvTermsCondition.DataKeys[e.RowIndex].Values["lid"].ToString());
        int result = QuotationOperations.DeleteTermConditionMS(lid, LoggedInUser.glUserId);
        if (result == 2)
        {
            lblresult.Text = "Terms & Condition Deleted Successfully!";
            lblresult.CssClass = "success";
            FillTermsCondition();
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

    protected void FillTermsCondition()
    {
        DataSet ds = new DataSet();
        ds = QuotationOperations.GetTermConditionMS();

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvTermsCondition.DataSource = ds;
            gvTermsCondition.DataBind();
        }
        else
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            gvTermsCondition.DataSource = ds;
            gvTermsCondition.DataBind();
            int columncount = gvTermsCondition.Rows[0].Cells.Count;
            gvTermsCondition.Rows[0].Cells.Clear();
            gvTermsCondition.Rows[0].Cells.Add(new TableCell());
            gvTermsCondition.Rows[0].Cells[0].ColumnSpan = columncount;
            gvTermsCondition.Rows[0].Cells[0].Text = "No Records Found!";
        }
    }

    protected void gvTermsCondition_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvTermsCondition.EditIndex = e.NewEditIndex;
        FillTermsCondition();
        lblresult.Text = "";
        lblresult.Visible = false;
    }

    protected void gvTermsCondition_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvTermsCondition.EditIndex = -1;
        FillTermsCondition();
        lblresult.Text = "";
        lblresult.Visible = false;
    }

    protected void gvTermsCondition_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvTermsCondition.PageIndex = e.NewPageIndex;
        gvTermsCondition.DataBind();
        FillTermsCondition();
    }

    protected void gvTermsCondition_RowDataBound(object sender, GridViewRowEventArgs e)
    {
       
    }

    #region TERMS & CONDITION POPUP EVENTS

    protected void GetTermsCondition(int lid)
    {
        if (lid != 0)
        {
            DataSet ds = new DataSet();
            ds = QuotationOperations.GetTermConditionDetails(lid);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvConditionDetail.DataSource = ds;
                gvConditionDetail.DataBind();
            }
            else
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                gvConditionDetail.DataSource = ds;
                gvConditionDetail.DataBind();
                int columncount = gvConditionDetail.Rows[0].Cells.Count;
                gvConditionDetail.Rows[0].Cells.Clear();
                gvConditionDetail.Rows[0].Cells.Add(new TableCell());
                gvConditionDetail.Rows[0].Cells[0].ColumnSpan = columncount;
                gvConditionDetail.Rows[0].Cells[0].Text = "No Records Found!";
            }
        }
    }

    protected void gvConditionDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblError_Popup.Visible = true;
        if (e.CommandName == "Insert")
        {
            int TermsId = 0;
            TermsId = Convert.ToInt32(ViewState["TermsId"].ToString());
            TextBox txtTerms_Footer = gvConditionDetail.FooterRow.FindControl("txtTerms_Footer") as TextBox;

            int result = QuotationOperations.AddTermConditionDetails(TermsId, txtTerms_Footer.Text.Trim(), LoggedInUser.glUserId);
            if (result == 1)
            {
                lblError_Popup.Text = "Terms & Condition Added Successfully..!!";
                lblError_Popup.CssClass = "success";
                GetTermsCondition(Convert.ToInt32(ViewState["TermsId"]));
                mpeTermConditions.Show();
            }
            else
            {
                lblresult.Text = "Terms & Condition already exist!";
                lblresult.CssClass = "warning";
            }
        }
    }

    protected void gvConditionDetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvConditionDetail.EditIndex = e.NewEditIndex;
        GetTermsCondition(Convert.ToInt32(ViewState["TermsId"]));
        lblError_Popup.Text = "";
        lblError_Popup.Visible = false;
        mpeTermConditions.Show();
    }

    protected void gvConditionDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvConditionDetail.EditIndex = -1;
        GetTermsCondition(Convert.ToInt32(ViewState["TermsId"]));
        lblError_Popup.Text = "";
        lblError_Popup.Visible = false;
        mpeTermConditions.Show();
    }

    protected void gvConditionDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int Lid = Convert.ToInt32(gvConditionDetail.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtTerms = (TextBox)gvConditionDetail.Rows[e.RowIndex].FindControl("txtTerms");

        int result = QuotationOperations.UpdateTermConditionDetails(Lid, txtTerms.Text.Trim(), LoggedInUser.glUserId);
        if (result == 1)
        {
            lblError_Popup.Text = "Terms & Condition Updated Successfully..!!";
            lblError_Popup.CssClass = "success";
            gvConditionDetail.EditIndex = -1;
            GetTermsCondition(Convert.ToInt32(ViewState["TermsId"]));
            mpeTermConditions.Show();
        }
        else
        {
            lblError_Popup.Text = "Terms & Condition does not exist!";
            lblError_Popup.CssClass = "warning";
            mpeTermConditions.Show();
        }
    }

    protected void gvConditionDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblresult.Visible = true;
        int lid = Convert.ToInt32(gvConditionDetail.DataKeys[e.RowIndex].Values["lid"].ToString());
        int result = QuotationOperations.DeleteTermConditionDetails(lid, LoggedInUser.glUserId);
        if (result == 1)
        {
            lblError_Popup.Text = "Terms & Condition Deleted Successfully!";
            lblError_Popup.CssClass = "success";
            GetTermsCondition(Convert.ToInt32(ViewState["TermsId"]));
            mpeTermConditions.Show();
        }
        else
        {
            lblError_Popup.Text = "Terms & Condition does not exist!";
            lblError_Popup.CssClass = "warning";
            mpeTermConditions.Show();
        }
    }

    protected void gvConditionDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvConditionDetail.PageIndex = e.NewPageIndex;
        GetTermsCondition(Convert.ToInt32(ViewState["TermsId"]));
        mpeTermConditions.Show();
    }

    protected void imgdelRange_Click(object sender, EventArgs e)
    {
        mpeTermConditions.Hide();
    }

    #endregion

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

    #region Export Data For Popup

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        string strFileName = "TermsCondition_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    protected void ExportFunction(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvConditionDetail.AllowPaging = false;
        gvConditionDetail.AllowSorting = false;
        gvConditionDetail.Columns[1].Visible = false;
        gvConditionDetail.Columns[3].Visible = false;
        gvConditionDetail.FooterRow.Visible = false;
        gvConditionDetail.Caption = "Terms & Condition Details On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
        gvConditionDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    #endregion
}