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

public partial class PCDBillingDetail : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(gvPCDDocument);

        if (!IsPostBack)
        {
            Session["PreviousRowIndex"] = null;
            if (Session["JobId"] == null)
            {
                Response.Redirect("PendingPCDBilling.aspx");
            }
            else
            {
                Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
                lblTitle.Text = "Billing Detail";

                GetJobDetail(Convert.ToInt32(Session["JobId"]));

                if (gvPCDInvoice.Rows.Count > 0)
                {
                    gvPCDInvoice.SelectedIndex = 1;

                    gvPCDInvoice.SelectedIndexChanged += new EventHandler(gvPCDInvoice_SelectedIndexChanged);
                }
                else
                {
                    fvInvoice.ChangeMode(FormViewMode.Insert);
                }
              //  DBOperations.FillPCDDocument(ddPCDDocument);
            }
        }
    }

    private void GetJobDetail(int JobId)
    {
        DataView dvDetail = DBOperations.GetJobDetailForPCDBilling(JobId);

        if (dvDetail.Table.Rows.Count > 0)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
         //   lblTitle.Text += " - " + dvDetail.Table.Rows[0]["JobRefNo"].ToString();

            lblJobRefNo.Text = dvDetail.Table.Rows[0]["JobRefNo"].ToString();
            
            hdnCustomerId.Value = dvDetail.Table.Rows[0]["CustomerId"].ToString();
            lblCustName.Text = dvDetail.Table.Rows[0]["Customer"].ToString();
            lblScrutinyBy.Text = dvDetail.Table.Rows[0]["ScrutinyBy"].ToString();
            lblScrutinyDate.Text = dvDetail.Table.Rows[0]["ScrutinyDate"].ToString();

      //      lblPersonName.Text = dvDetail.Table.Rows[0]["TransportPersonName"].ToString();
      //      lblTransportDate.Text = Convert.ToDateTime(dvDetail.Table.Rows[0]["TransportDate"]).ToString("dd/MM/yyyy");;
        }
    }
    
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("PendingPCDBilling.aspx");
    }
    
    protected void gvPCDDocument_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string strDocPath = (string)DataBinder.Eval(e.Row.DataItem, "DocPath").ToString();

            if (strDocPath == "")
            {
                LinkButton lnkDownload = (LinkButton)e.Row.FindControl("lnkDownload");
                lnkDownload.Visible = false;
            }
        }
    }

    protected void gvPCDDocument_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
    }

    #region PCD Invoice

    protected void btnNew_Click(object sender, EventArgs e)
    {
        fvInvoice.ChangeMode(FormViewMode.Insert);
    }
    
    protected void gvPCDInvoice_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["PreviousRowIndex"] != null)
        {
            var previousRowIndex = (int)Session["PreviousRowIndex"];
            GridViewRow PreviousRow = gvPCDInvoice.Rows[previousRowIndex];
            PreviousRow.BackColor = System.Drawing.Color.White;
        }
        
        GridViewRow row = (GridViewRow)(gvPCDInvoice.Rows[gvPCDInvoice.SelectedIndex]);
        row.BackColor = System.Drawing.Color.Red;
        Session["PreviousRowIndex"] = row.RowIndex;

        fvInvoice.ChangeMode(FormViewMode.ReadOnly);
       // gvPCDInvoice.Rows[gvPCDInvoice.SelectedIndex].BackColor = System.Drawing.Color.Red;
    }
        
    protected void gvPCDInvoice_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            // Hiding the Select Button Cell in Header Row.
            e.Row.Cells[0].Style.Add(HtmlTextWriterStyle.Display, "none");
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Hiding the Select Button Cells showing for each Data Row. 
            e.Row.Cells[0].Style.Add(HtmlTextWriterStyle.Display, "none");

            // Attaching one onclick event for the entire row, so that it will
            // fire SelectedIndexChanged, while we click anywhere on the row.
            e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.textDecoration='underline';";
            e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";
            e.Row.ToolTip = "Click to select Invoice";
            e.Row.Attributes["onclick"] = this.Page.ClientScript.GetPostBackClientHyperlink(this.gvPCDInvoice, "Select$" + e.Row.RowIndex);
        }
    }
    
    #region FormView Event

    protected void fvInvoice_DataBound(object sender, EventArgs e)
    {
        Page.Validate("Required");
    }

    protected void fvInvoice_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        if (e.Exception != null || e.AffectedRows == -1)
        {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
        }
    }

    protected void fvInvoice_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
    {
        if (e.Exception != null || e.AffectedRows == -1)
        {
            e.ExceptionHandled = true;
            e.KeepInEditMode = true;
        }
    }

    protected void fvInvoice_ItemDeleted(object sender, FormViewDeletedEventArgs e)
    {
        if (e.Exception != null || e.AffectedRows == -1)
        {
            e.ExceptionHandled = true;
        }
    }

    protected void DataSourceFormView_Inserted(object sender, SqlDataSourceStatusEventArgs e)
    {
        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);
        
        if (Result > 0)
        {
            lblError.Text = "Invoice Detail Added Successfully";
            lblError.CssClass = "success";
            DataSourcePCDInvoice.SelectParameters[0].DefaultValue = Result.ToString();
          //  gvPCDInvoice.SelectedIndex = -1;
            gvPCDInvoice.DataBind();
        }
        else if (Result == 0)
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";
        }
        else if (Result == -1)
        {
            lblError.Text = "Invoice Details Already Exists";
            lblError.CssClass = "warning";

        }
    }

    protected void DataSourceFormView_Updated(object sender, SqlDataSourceStatusEventArgs e)
    {
        string Result = e.Command.Parameters["@OutPut"].Value.ToString();
                
        if (Result == "0")
        {
            lblError.Text = "Invoice Detail Updated Successfully";
            lblError.CssClass = "success";
            gvPCDInvoice.DataBind();
        }
        else if (Result == "1")
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";
        }
        else if (Result == "2")
        {
            lblError.Text = "Invoice Details Already Exists";
            lblError.CssClass = "warning";
        }
    }

    protected void DataSourceFormView_Deleted(object sender, SqlDataSourceStatusEventArgs e)
    {
        string Result = e.Command.Parameters["@OutPut"].Value.ToString();

        
        if (Result == "0")
        {
            lblError.Text = "Invoice Detail Deleted Successfully";
            lblError.CssClass = "success";
            gvPCDInvoice.DataBind();
           // gvPCDInvoice.SelectedIndex = -1;
        }
        else if (Result == "1")
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";
        }
        else if (Result == "2")
        {
            lblError.Text = "Invoice Not Exists";
            lblError.CssClass = "info";

        }
    }

    #endregion
    #endregion

    #region Documnet Download

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
        
    #endregion
}
