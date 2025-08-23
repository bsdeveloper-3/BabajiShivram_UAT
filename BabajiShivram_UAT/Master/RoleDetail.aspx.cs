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

public partial class RoleDetail : System.Web.UI.Page
{
    DataTable dtgrdUserRights = new DataTable();
    LoginClass logobj = new LoginClass();
    clsRoleDetail objRoleDetail = new clsRoleDetail(); //use to fill Roledetail Grid
    //CommonFillControls comcrt = new CommonFillControls();

    CDatabase comcrt = new CDatabase();

    int lRoleId;
    int lModuleIdl;
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        if (Session["AccessRoleId"] == null)
        {
            Response.Redirect("RoleMaster.aspx");
        }
        if (!IsPostBack)
        {
            //
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Access Role Detail";
            //
            lRoleId = Convert.ToInt32(Session["AccessRoleId"]);
            lModuleIdl = Convert.ToInt32(ddModule.SelectedValue);

            hidRoleId.Value = lRoleId.ToString();//lRoleId.ToString();  //Request.QueryString["id"].ToString();
            lblRoleName.Text = Request.QueryString["Name"].ToString();//lRoleId.ToString(); //Request.QueryString["rcode"].ToString();
            
            lblMsg.Text = "";
                        
            //
            grdUserRight.Visible = true;
            
            btnSave.Visible = true;
            btnSave2.Visible = true;
            btnCancel2.Visible = true;

            grdUserRight.DataSource = objRoleDetail.getDataTableForRole(lRoleId, 0, lModuleIdl);
            grdUserRight.DataBind();

            //
        }
    }//end of function

    protected void btnCancel_Click(Object sender, EventArgs e)
    {
        Response.Redirect("RoleMaster.aspx");
    }

    protected void ddModule_SelectedIndexChanged(object sender, EventArgs e)
    {
        lModuleIdl = Convert.ToInt32(ddModule.SelectedValue);
        lRoleId = Convert.ToInt32(Session["AccessRoleId"]);

        grdUserRight.Visible = true;

        btnSave.Visible = true;
        btnSave2.Visible = true;
        btnCancel2.Visible = true;
        grdUserRight.DataSource = objRoleDetail.getDataTableForRole(lRoleId, 0, lModuleIdl);
        grdUserRight.DataBind();
    }

    //All/Rights column checkbox event
    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        int i = -1;
        try
        {
            i = Int32.Parse(hidRowIndex.Value);
        }
        catch
        {
        }

        if (grdUserRight.Visible == true && i > -1)
        {
            if (((CheckBox)grdUserRight.Rows[i].FindControl("chkAll")).Checked == true)
            {
                ((CheckBox)grdUserRight.Rows[i].FindControl("chkAll")).Checked = true;
                
                if (((Label)grdUserRight.Rows[i].FindControl("lblcGsGpFlag")).Text == "G")
                {
                    if (((CheckBox)grdUserRight.Rows[i].FindControl("chkAll")).Checked == true)
                        checkAllCheckboxOnG(true, Int32.Parse(((Label)grdUserRight.Rows[i].FindControl("lbllId")).Text));

                    checkUncheck(((CheckBox)grdUserRight.Rows[i].FindControl("chkAll")).Checked, Int32.Parse(((Label)grdUserRight.Rows[i].FindControl("lbllId")).Text), "chkAll");

                    checkRoot(true, Int32.Parse(((Label)grdUserRight.Rows[i].FindControl("lbllPrevId")).Text), "chkAll");
                }
                else
                {
                    unCheckRoot(false, Int32.Parse(((Label)grdUserRight.Rows[i].FindControl("lbllPrevId")).Text), "chkAll");

                    checkRoot(true, Int32.Parse(((Label)grdUserRight.Rows[i].FindControl("lbllPrevId")).Text), "chkAll");
                }
            }
            else
            {
                ((CheckBox)grdUserRight.Rows[i].FindControl("chkAll")).Checked = false;
                
                if (((Label)grdUserRight.Rows[i].FindControl("lblcGsGpFlag")).Text == "G")
                {
                    checkUncheck(((CheckBox)grdUserRight.Rows[i].FindControl("chkAll")).Checked, Int32.Parse(((Label)grdUserRight.Rows[i].FindControl("lbllId")).Text), "chkAll");
                }
                else
                {
                    unCheckRoot(false, Int32.Parse(((Label)grdUserRight.Rows[i].FindControl("lbllPrevId")).Text), "chkAll");
                }
            }
        }
    }//end of function

    //Grid view rowDataBound Event for change color of text filter on cGsGpFlag
    //This event is comman for both grid view
        
    //this function take 3 arg. bool flag, lId, Control id
    public void checkUncheck(bool bflag, int lId, string controlId)
    {
        if (grdUserRight.Visible == true)
        {
            foreach (GridViewRow row in grdUserRight.Rows)
            {
                if (((Label)row.FindControl("lblcGsGpFlag")).Text == "G" && Int32.Parse(((Label)row.FindControl("lbllPrevId")).Text) == lId)
                {
                    if (controlId == "chkAll")
                    {
                        ((CheckBox)(row.FindControl(controlId))).Checked = bflag;
                        
                        checkUncheck(bflag, Int32.Parse(((Label)row.FindControl("lbllId")).Text), controlId);
                    }
                    else
                    {
                        ((CheckBox)(row.FindControl(controlId))).Checked = bflag;

                        checkUncheck(bflag, Int32.Parse(((Label)row.FindControl("lbllId")).Text), controlId);
                    }
                }
                else if (Int32.Parse(((Label)row.FindControl("lbllPrevId")).Text) == lId)
                {
                    if (controlId == "chkAll")
                    {
                        ((CheckBox)(row.FindControl(controlId))).Checked = bflag;
                    }
                    else
                    {
                        ((CheckBox)(row.FindControl(controlId))).Checked = bflag;
                    }
                }
            }
        }
    }//end of function

    //this funtion is use to uncheck root element if child element check box is in uncheck state
    public void unCheckRoot(bool bflag, int iPrevId, string controlId)
    {
        if (grdUserRight.Visible == true)
        {
            foreach (GridViewRow row in grdUserRight.Rows)
            {
                if (((Label)row.FindControl("lblcGsGpFlag")).Text == "G" && Int32.Parse(((Label)row.FindControl("lbllId")).Text) == iPrevId)
                {
                    if (controlId == "chkAll")
                    {
                        ((CheckBox)(row.FindControl(controlId))).Checked = bflag;

                        //((CheckBox)(row.FindControl("chkAdd"))).Checked = bflag;
                        //((CheckBox)(row.FindControl("chkEdit"))).Checked = bflag;
                        //((CheckBox)(row.FindControl("chkDel"))).Checked = bflag;
                        //((CheckBox)(row.FindControl("chkPrev"))).Checked = bflag;

                        unCheckRoot(bflag, Int32.Parse(((Label)row.FindControl("lbllPrevId")).Text), controlId);
                    }
                    else
                    {
                        ((CheckBox)(row.FindControl(controlId))).Checked = bflag;
                        ((CheckBox)(row.FindControl("chkAll"))).Checked = bflag;

                        unCheckRoot(bflag, Int32.Parse(((Label)row.FindControl("lbllPrevId")).Text), controlId);
                    }
                }
            }
        }
    }//end of function

    //This function is use to check or uncheck checkbox depent on parent element
    public void checkAllCheckboxOnG(bool bflag, int lId)
    {
       if (grdUserRight.Visible == true)
        {
            foreach (GridViewRow row in grdUserRight.Rows)
            {
                if (Int32.Parse(((Label)row.FindControl("lbllPrevId")).Text) == lId)
                {
                    if (((Label)row.FindControl("lblcGsGpFlag")).Text == "G")
                    {
                        ((CheckBox)(row.FindControl("chkAll"))).Checked = bflag;

                        checkAllCheckboxOnG(bflag, Int32.Parse(((Label)row.FindControl("lbllId")).Text));
                    }
                    else
                    {
                        ((CheckBox)(row.FindControl("chkAll"))).Checked = bflag;
                    }
                }
            }
        }
    }//end of function

    //this funtion is use to checked root element if child element check box is in checked state is true
    public void checkRoot(bool bflag, int iPrevId, string controlId)
    {
        bool btestFlag = false;
       if (grdUserRight.Visible == true)
        {
            foreach (GridViewRow row in grdUserRight.Rows)
            {
                if (Int32.Parse(((Label)row.FindControl("lbllPrevId")).Text) == iPrevId)
                {
                    if (((CheckBox)row.FindControl(controlId)).Checked == true)

                        btestFlag = true;
                    else
                    {
                        btestFlag = false;
                        break;
                    }
                }
            }

            if (btestFlag == true)
            {
                foreach (GridViewRow row in grdUserRight.Rows)
                {
                    if (((Label)row.FindControl("lblcGsGpFlag")).Text == "G" && Int32.Parse(((Label)row.FindControl("lbllId")).Text) == iPrevId)
                    {
                        if (controlId == "chkAll")
                        {
                            ((CheckBox)row.FindControl(controlId)).Checked = true;

                            //((CheckBox)row.FindControl("chkAdd")).Checked = true;
                            //((CheckBox)row.FindControl("chkEdit")).Checked = true;
                            //((CheckBox)row.FindControl("chkDel")).Checked = true;
                            //((CheckBox)row.FindControl("chkPrev")).Checked = true;

                            checkRoot(bflag, Int32.Parse(((Label)row.FindControl("lbllPrevId")).Text), controlId);
                        }
                        else
                        {
                            ((CheckBox)row.FindControl(controlId)).Checked = true;

                            //this is use to check Root chkAll when all other child checkbox check status are checked

                            if (((CheckBox)row.FindControl("chkAll")).Checked == true)

                                ((CheckBox)(row.FindControl("chkAll"))).Checked = true;

                            checkRoot(bflag, Int32.Parse(((Label)row.FindControl("lbllPrevId")).Text), controlId);
                        }
                    }
                }
            }

        }
    }//end of function

    protected void btnSave_Click(object sender, EventArgs e)
    {
        int i = 0;
        clsRoleDetail objRoleDetail = new clsRoleDetail();

        objRoleDetail.lRoleId = Int32.Parse(hidRoleId.Value);
        objRoleDetail.lCompId = 0; //Int32.Parse(ddlCompanies.SelectedValue);
        objRoleDetail.lModuleId = Int32.Parse(ddModule.SelectedValue);
        objRoleDetail.bDel = 0;
        objRoleDetail.lUserId = Convert.ToInt32(logobj.glUserId);

        if (grdUserRight.Visible == true)
        {
            i = objRoleDetail.saveSingleMode(grdUserRight);
        }
        if (i == 0)
        {
            lblMsg.Text = "Error Role Details are not saved"; //Response.Write("false");
            lblMsg.CssClass = "errorMsg";
            
            grdUserRight.Visible = false;
            btnSave.Visible = false;
            btnSave2.Visible = false;
            btnCancel2.Visible = false;
        }

        else
        {
            lblMsg.Text = "Role details saved successfully";  //Response.Write("true");
            lblMsg.CssClass = "success";

            btnCancel.Text = "Back To Role Setup";
            grdUserRight.Visible = false;
            btnSave.Visible = false;
            btnSave2.Visible = false;
            btnCancel2.Visible = false;
        }
    }//end of function

    protected void grdUserRight_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string lId = "", cGsGpFlag = "", lPrevId = "";
            lId = ((Label)e.Row.FindControl("lbllId")).Text; // e.Row.Cells[0].Text;

            cGsGpFlag = ((Label)e.Row.FindControl("lblcGsGpFlag")).Text;  //e.Row.Cells[1].Text;

            lPrevId = ((Label)e.Row.FindControl("lbllPrevId")).Text; ;

            if (cGsGpFlag == "G")
            {
                e.Row.CssClass = "statuspanel";
                e.Row.ForeColor = System.Drawing.Color.Green;
            }

            ((HiddenField)e.Row.FindControl("hidcontrol")).Value = lId + ":" + cGsGpFlag + ":" + lPrevId;
        }
    }//end of function
    
    protected void grdUserRight_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
}
