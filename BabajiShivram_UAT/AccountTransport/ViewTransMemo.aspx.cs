using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;

public partial class AccountTransport_ViewTransMemo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(Session["MemoViewId"] == null)
        {
            if (Request.UrlReferrer != null)
            {
                int startIndex = Request.UrlReferrer.AbsolutePath.LastIndexOf("/");

                if (startIndex > 0)
                {
                    string strReturnURL = Request.UrlReferrer.AbsolutePath.Remove(0, startIndex + 1);

                    Response.Redirect(strReturnURL);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "<script>alert('Session Expired! Please try again');</script>", false);
            }
        }
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Memo Detail";

            GetMemoDetail();
        }
    }

    private void GetMemoDetail()
    {
        if (Session["MemoViewId"] != null)
        {
            int MemoId = Convert.ToInt32(Session["MemoViewId"].ToString());

            DataSet dsDetail = DBOperations.GetTransMemoDetail(MemoId);

            if (dsDetail.Tables[0].Rows.Count > 0)
            {
                lblMemoRefNo.Text = dsDetail.Tables[0].Rows[0]["PayMemoRefNo"].ToString();
                lblTransporterName.Text = dsDetail.Tables[0].Rows[0]["VendorName"].ToString();
                lblTotalAmount.Text = dsDetail.Tables[0].Rows[0]["MemoAmount"].ToString();
            }
        }
    }

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "Memo_Detail_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm") + ".xls";

        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExportFunction(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvDetail.AllowPaging = false;
        gvDetail.AllowSorting = false;

        gvDetail.DataBind();

        gvDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}