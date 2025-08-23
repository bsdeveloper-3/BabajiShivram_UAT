using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data;

public partial class FAQ_FAQ : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!IsPostBack)
        {

            string UserId = LoggedInUser.glUserId.ToString();
            Session["FaqId"] = null;
        }

    }
    protected void GridFaqDetails_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;

        }
    }
    
    protected void GridFaqDetails_RowCommand(Object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName.ToLower() == "select")
        {
            string strFaqService = (string)e.CommandArgument;
            Session["FaqId"] = strFaqService;

            Response.Redirect("~/FAQ/FaqServiceDetails.aspx");
        }
    }

    protected void GrvFAQAllList_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;

        }
    }

    protected void GrvFAQAllList_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            string strFaqService = (string)e.CommandArgument;
            Session["FaqId"] = strFaqService;

            Response.Redirect("~/FAQ/FaqServiceDetails.aspx");
        }
    }



    protected void btnSearch_Click(object sender, EventArgs e)
    {

        if (hdnServiceId.Value != "0")
        {

            int serviceid = int.Parse(hdnServiceId.Value);//Convert.ToInt32(ddServicess.SelectedValue);

            DataSet dsServiceID = DBOperations.GetServiceByAllFaqList(Convert.ToInt32(serviceid));
            PanAllList.Visible = false;

            if (dsServiceID.Tables[0].Rows.Count > 0)
            {
                lblservice.Text = dsServiceID.Tables[0].Rows[0]["sName"].ToString();
                GridFaqDetails.DataSource = dsServiceID;
                GridFaqDetails.DataBind();
                panback.Visible = true;
                PanelService.Visible = true;
                panback.Visible = true;
                Panservice.Visible = true;
            }
            else
            {
                lblError.Text = "Service Record Not Found";
                //Response.Redirect("ErrorMessage.aspx");
                PanelService.Visible = false;
                panback.Visible = true;
                PanAllList.Visible = false;
                Panservice.Visible = false;
                Response.Redirect("ErrorMessage.aspx");
            }
            
        }

        else

        {
            Response.Redirect("ErrorMessage.aspx");
            lblError.Text = "Record Not Found";
            panback.Visible = true;
        }

       
    }

   
}
