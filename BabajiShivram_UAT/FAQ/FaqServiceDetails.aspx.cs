using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BSImport.UserManager.DAL;
using System.Data;

public partial class FAQ_FaqServiceDetails : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
       
        //Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        //lblTitle.Text = "FAQ :-";

        string strFaqId = "0";

        if (Session["FaqId"] != null)
        {
            strFaqId = Session["FaqId"].ToString();

            FAQDetail(Convert.ToInt32(strFaqId));
           

        }

    }

    protected void GridFaqDocDownload_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;

        }
    }

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

    //protected void GrvDocumentForm_RowCommand(Object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName.ToLower() == "download")
    //    {
    //        string DocPathForm1 = e.CommandArgument.ToString();
    //        DownloadDocument(DocPathForm1);
    //        //GridFaqDocDownload.DataBind();

    //    }


    //    //if (e.CommandName.ToLower() == "download1")
    //    //{
    //    //    string DocPathForm2 = e.CommandArgument.ToString();
    //    //    DownloadDocument(DocPathForm2);
    //    //    // GridPendingCustomerRegistration.DataBind();
    //    //}

    //    //if (e.CommandName.ToLower() == "downloadexcise")
    //    //{
    //    //    string DocPathForm3 = e.CommandArgument.ToString();
    //    //    DownloadDocument(DocPathForm3);
    //    //    // GridPendingCustomerRegistration.DataBind();
    //    //}


    //}


    protected void GridFaqDocDownload_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath1 = e.CommandArgument.ToString();
            DownloadDocument(DocPath1);
            //GridFaqDocDownload.DataBind();

        }
        if (e.CommandName.ToLower() == "download1")
        {
            string DocFormPath1 = e.CommandArgument.ToString();
            DownloadDocument(DocFormPath1);
            // GridPendingCustomerRegistration.DataBind();
        }

        //if (e.CommandName.ToLower() == "downloadexcise")
        //{
        //    string DocPath3 = e.CommandArgument.ToString();
        //    DownloadDocument(DocPath3);
        //    // GridPendingCustomerRegistration.DataBind();
        //}

    }

    protected void GridFaqDocDownload_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string strFilePath = "";

            if (DataBinder.Eval(e.Row.DataItem, "DocPath") != DBNull.Value)
                strFilePath = (string)DataBinder.Eval(e.Row.DataItem, "DocPath");


            if (strFilePath == "")
            {
                LinkButton lnkDownload = (LinkButton)e.Row.FindControl("lnkDownload");

                lnkDownload.Visible = false;

            }

            string strFilePath1 = "";

            if (DataBinder.Eval(e.Row.DataItem, "DocFormPath") != DBNull.Value)

                strFilePath1 = (string)DataBinder.Eval(e.Row.DataItem, "DocFormPath");

            if (strFilePath1 == "")
            {
                LinkButton lnkDownload1 = (LinkButton)e.Row.FindControl("lnkDownload");

                lnkDownload1.Visible = false;

            }
          

        }

    }

    
    //protected void GrvDocumentForm_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        string strFilePath = "";

    //        if (DataBinder.Eval(e.Row.DataItem, "DocFormPath1") != DBNull.Value)
    //            strFilePath = (string)DataBinder.Eval(e.Row.DataItem, "DocFormPath1");


    //        if (strFilePath == "")
    //        {
    //            LinkButton lnkDownload = (LinkButton)e.Row.FindControl("lnkDownload");

    //            lnkDownload.Visible = false;

    //        }

    //        //string strFilePath1 = "";

    //        //if (DataBinder.Eval(e.Row.DataItem, "DocFormPath2") != DBNull.Value)

    //        //    strFilePath1 = (string)DataBinder.Eval(e.Row.DataItem, "DocFormPath2");

    //        //if (strFilePath1 == "")
    //        //{
    //        //    LinkButton lnkDownload1 = (LinkButton)e.Row.FindControl("lnkDownload1");

    //        //    lnkDownload1.Visible = false;

    //        //}
    //        //string strFilePath2 = "";

    //        //if (DataBinder.Eval(e.Row.DataItem, "DocFormPath3") != DBNull.Value)

    //        //    strFilePath2 = (string)DataBinder.Eval(e.Row.DataItem, "DocFormPath3");

    //        //if (strFilePath2 == "")
    //        //{
    //        //    LinkButton lnkDownload2 = (LinkButton)e.Row.FindControl("lnkDownload2");

    //        //    lnkDownload2.Visible = false;

    //        //}

    //    }
    //}
    

    private void FAQDetail(int FaqId)
    {

        DataSet dsServiceID = DBOperations.GetServiceByAllFaqDetails(Convert.ToInt32(FaqId));
        DataSet dsServiceConID = DBOperations.GetServiceContactDeatil(Convert.ToInt32(FaqId));
        GrvfaqContactPerson.DataSource = dsServiceConID;
        GrvfaqContactPerson.DataBind();


        if (dsServiceID.Tables[0].Rows.Count > 0)
        {
            lblservice.Text = dsServiceID.Tables[0].Rows[0]["sName"].ToString();
            lblTitle.Text = dsServiceID.Tables[0].Rows[0]["Title"].ToString();
            lblserviceId.Text = dsServiceID.Tables[0].Rows[0]["lid"].ToString();
           
           
            GrvDiscriptiopn.DataSource = dsServiceID;
            GrvDiscriptiopn.DataBind();


            GridFaqDocDownload.DataSource = dsServiceID;
            GridFaqDocDownload.DataBind();
            //GrvDocumentForm.DataSource = dsServiceID;
            //GrvDocumentForm.DataBind();

        }

    }

}






