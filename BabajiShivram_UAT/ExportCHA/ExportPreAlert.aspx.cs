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
using BSImport;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;


public partial class ExportCHA_ExportPreAlert : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(GridViewPreAlertDoc);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Pre-Alert Details";
        }

        if (LoggedInUser.glUserId == 1)
        {
           // gvPreAlert.Columns[12].Visible = true;
        }
        //
        DataFilter1.DataSource = GridviewSqlDataSource;
        DataFilter1.DataColumns = gvPreAlert.Columns;
        DataFilter1.FilterSessionID = "ExportPreAlert.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
        //
    }

    protected void gvPreAlert_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            string commandName = e.CommandName;
            //Get the Row Index.
            int EnqId = Convert.ToInt32(e.CommandArgument);

            //GridViewRow gvr = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            //int RowIndex = gvr.RowIndex;
            //int AlertId = Convert.ToInt32(gvPreAlert.DataKeys[gvr.RowIndex].Value);

            DataView dvCustomerRequest = DBOperations.GetDocumentByEnqId(EnqId);

            if (dvCustomerRequest.Table.Rows.Count > 0)
            {
                if (dvCustomerRequest.Table.Rows[0]["DocPath"] != DBNull.Value)
                {
                    string strPreAlertDocPath = dvCustomerRequest.Table.Rows[0]["DocPath"].ToString();

                    PreAlertDocument(strPreAlertDocPath);
                    ModalPopupDocument.Show();
                }
            }
        }
        else if (e.CommandName.ToLower() == "deleteprealert")
        {
            GridViewRow gvr1 = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;

            int RequestID = Convert.ToInt32(gvPreAlert.DataKeys[gvr1.RowIndex].Value);

            if (LoggedInUser.glUserId == 1)
            {
                int result = DBOperations.DelCustomerRequest(RequestID, LoggedInUser.glUserId);

                if (result == 0)
                {
                    lblError.Text = "PreAlert Removed Successfully!";
                    lblError.CssClass = "success";

                    gvPreAlert.DataBind();
                }
                else
                {
                    lblError.Text = "System Error! Please Try After Sometime.";
                    lblError.CssClass = "errorMsg";
                }

            }
            else
            {
                lblError.Text = "Delete Operation Not Allowed!";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected void gvPrealert_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    private void PreAlertDocument(string DirPath)
    {
        if (DirPath == "")
        {
            GridViewPreAlertDoc.DataSource = null;
            GridViewPreAlertDoc.DataBind();
        }
        else
        {
            String ServerPath = FileServer.GetFileServerDir();

            if (ServerPath == "")
            {
                ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\" + DirPath);
            }
            else
            {
                ServerPath = ServerPath + DirPath;
            }

            string DirectoryName = Path.GetDirectoryName(ServerPath);
            string[] filePaths = Directory.GetFiles(DirectoryName);

            List<ListItem> files = new List<ListItem>();
            foreach (string filePath in filePaths)
            {
                files.Add(new ListItem(Path.GetFileName(filePath), filePath));
            }


            GridViewPreAlertDoc.DataSource = files;
            GridViewPreAlertDoc.DataBind();
        }
    }

    protected void gvJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string strCustInstruction = "";

            if ((DataBinder.Eval(e.Row.DataItem, "CustInstruction")) != DBNull.Value)
            {
                strCustInstruction = (String)(DataBinder.Eval(e.Row.DataItem, "CustInstruction"));
                e.Row.ToolTip = strCustInstruction;
            }
        }
    }
    protected void DownloadPreAlertFile(object sender, EventArgs e)
    {
        string filePath = (sender as LinkButton).CommandArgument;
        Response.ContentType = ContentType;
        Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + Path.GetFileName(filePath) + "\"");
        Response.WriteFile(filePath);
        Response.End();
    }

    protected void btnCancelPopup_Click(object sender, EventArgs e)
    {
        ModalPopupDocument.Hide();
    }

    #region Data Filter

    void DataFilter1_OnDataBound()
    {
        try
        {
            DataFilter1.FilterSessionID = "ExportPreAlert.aspx";
            DataFilter1.FilterDataSource();
            gvPreAlert.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }
    #endregion
}