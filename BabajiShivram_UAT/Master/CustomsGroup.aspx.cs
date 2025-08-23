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
using System.Collections.Generic;
using System.Data.SqlClient;

public partial class Master_CustomsGroup : System.Web.UI.Page
{

    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            btnNewCustomGroup.Visible = true;
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Custom Group Setup";

        }

    }

    protected void gvCustomGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvCustomGroup.Visible = false;
        fsMainBorder.Visible = false;
        btnNewCustomGroup.Visible = false;
        if (gvCustomGroup.SelectedIndex == -1)
        {
            //  FormView1.ChangeMode(FormView1.DefaultMode);
        }
        else
        {
            FormView1.ChangeMode(FormViewMode.ReadOnly);
        }
    }

    protected void gvCustomGroup_PreRender(object sender, EventArgs e)
    {

        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    protected void btnNewCustomGroup_Click(object sender, EventArgs e)
    {
        FormView1.ChangeMode(FormViewMode.Insert);
        fsMainBorder.Visible = false;
        gvCustomGroup.Visible = false;
        btnNewCustomGroup.Visible = false;
    }

    protected void btnAddCustomGroup_Click(object sender, EventArgs e)
    {
        string CustomGroupId = "";
        ListBox listGroup = (ListBox)this.FormView1.FindControl("listGroup");
        RadioButtonList RDLStatus = (RadioButtonList)this.FormView1.FindControl("rdlStatus");
        Boolean Status = Convert.ToBoolean(RDLStatus.SelectedValue);
        Button btnAddCustomGroup = (Button)this.FormView1.FindControl("btnAddCustomGroup");
        if (listGroup.Items.Count == 0)
        {
            lberror.Text = "Please Add Custom Group!";
            lberror.CssClass = "errorMsg";
            fsMainBorder.Visible = false;
            gvCustomGroup.Visible = false;
            btnNewCustomGroup.Visible = false;

            return;

        }
        else
        {
            foreach (ListItem Item in listGroup.Items)
            {

                CustomGroupId += Item.Value + ",";
            }
            HiddenField hdnPortId = (HiddenField)this.FormView1.FindControl("hdnPortId");
            TextBox PortName = (TextBox)this.FormView1.FindControl("txtPortName");
            int PortId = Convert.ToInt32(hdnPortId.Value);
            TextBox Person = (TextBox)this.FormView1.FindControl("txtPersonName");
            string PersonName = Person.Text;
            TextBox Mobile = (TextBox)this.FormView1.FindControl("txtMobile");
            string MobileNo = Mobile.Text;

            int Result = DBOperations.AddCustomGroup(PortId, CustomGroupId, PersonName, MobileNo, LoggedInUser.glUserId, Status);

            if (Result == 0)
            {
                lberror.Text = "Custom Group Details Added Successfully";
                lberror.CssClass = "success";

                PortName.Text = String.Empty;
                hdnPortId.Value = "0";
                Mobile.Text = "";
                Panel pnladdDetails = (Panel)this.FormView1.FindControl("pnlAddCustomGroups");
                pnladdDetails.Visible = false;
                gvCustomGroup.SelectedIndex = -1;
                gvCustomGroup.DataBind();
                gvCustomGroup.Visible = true;
                fsMainBorder.Visible = true;
                btnNewCustomGroup.Visible = true;

            }
            else if (Result == 1)
            {
                lberror.Text = "System Error! Please Try After Sometime";
                lberror.CssClass = "errorMsg";
            }
            else if (Result == 3)
            {
                lberror.Text = "Custom Group already exists";
                lberror.CssClass = "errorMsg";
                btnAddCustomGroup.CommandName = "other";
            }
        }
    }

    protected void btnUpdateCustomGroup_Click(object sender, EventArgs e)
    {
        string CustomGroupId = "";
        DataKey key = FormView1.DataKey;
        int lId = Convert.ToInt32(key.Value.ToString());
        ListBox listGroup = (ListBox)this.FormView1.FindControl("listupdGroup");
        if (listGroup.Items.Count == 0)
        {
            lberror.Text = "Please Add Custom Group!";
            lberror.CssClass = "errorMsg";
        }
        else
        {
            foreach (ListItem Item in listGroup.Items)
            {

                CustomGroupId += Item.Value + ",";
            }
            RadioButtonList RDLStatus = (RadioButtonList)this.FormView1.FindControl("rdlCustomGroupStatus");
            Boolean Status = Convert.ToBoolean(RDLStatus.SelectedValue);
            HiddenField hdnPortId = (HiddenField)this.FormView1.FindControl("hiddenPortId");
            TextBox PortName = (TextBox)this.FormView1.FindControl("txtPortName");
            int PortId = Convert.ToInt32(hdnPortId.Value);
            TextBox Person = (TextBox)this.FormView1.FindControl("txtName");
            string PersonName = Person.Text;
            TextBox Mobile = (TextBox)this.FormView1.FindControl("txtTelephone");
            string MobileNo = Mobile.Text;

            int Result = DBOperations.UpdateCustomeGroup(lId, PortId, CustomGroupId, PersonName, MobileNo, LoggedInUser.glUserId, Status);

            if (Result == 0)
            {
                lberror.Text = "Custom Group Updated Successfully";
                lberror.CssClass = "success";
                PortName.Text = String.Empty;
                hdnPortId.Value = "0";
                Panel pnlupdDetails = (Panel)this.FormView1.FindControl("pnlUpdateCustomGroup");
                pnlupdDetails.Visible = false;
                gvCustomGroup.DataBind();
                gvCustomGroup.Visible = true;
                gvCustomGroup.SelectedIndex = -1;
                FormView1.DataBind();
                fsMainBorder.Visible = true;
                btnNewCustomGroup.Visible = true;
            
            }
            else if (Result == 1)
            {
                lberror.Text = "System Error! Please Try After Sometime";
                lberror.CssClass = "errorMsg";
            }
            else if (Result == -1)
            {
                lberror.Text = " Custom Group Details Already Exists!";
                lberror.CssClass = "errorMsg";
            }

        }
    }

    #region FormView Command
    protected void FormView1_ItemCommand(Object sender, FormViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancel")
        {
            gvCustomGroup.Visible = true;
            gvCustomGroup.SelectedIndex = -1;
            fsMainBorder.Visible = true;
            btnNewCustomGroup.Visible = true;
        }
        else if (e.CommandName == "")
        {
            //btnNewCustomGroup.Visible = true;
            //gvCustomGroup.Visible = true;
            //fsMainBorder.Visible = true;
        }
        else if (e.CommandName == "Delete")
        {
            btnNewCustomGroup.Visible = true;
            gvCustomGroup.Visible = true;
            fsMainBorder.Visible = true;
        }

    }

    protected void FormView1_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        if (e.Exception != null | e.AffectedRows == -1)
        {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
        }
    }

    protected void FormView1_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
    {
        if (e.Exception != null | e.AffectedRows == -1)
        {
            e.ExceptionHandled = true;
            e.KeepInEditMode = true;
        }
    }


    protected void FormView1_ItemDeleted(object sender, FormViewDeletedEventArgs e)
    {
        if (e.Exception != null | e.AffectedRows == -1)
        {
            e.ExceptionHandled = true;
        }
    }


    protected void FormviewSqlDataSource_Deleted(object sender, SqlDataSourceStatusEventArgs e)
    {
        lberror.Visible = true;

        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        if (Result == 0)
        {
            lberror.Text = "Custom Group Deleted Successfully !";
            lberror.CssClass = "success";
            gvCustomGroup.SelectedIndex = -1;
            gvCustomGroup.DataBind();
            gvCustomGroup.Visible = true;
            fsMainBorder.Visible = true;
        }
        else if (Result == 1)
        {
            lberror.Text = "System Error! Please try after sometime";
            lberror.CssClass = "errorMsg";
        }
    }
    #endregion

    #region ListBox Event when Inserting

    protected void btnInsert_Click(object sender, EventArgs e)
    {
        btnNewCustomGroup.Visible = false;
        fsMainBorder.Visible = false;
        
        ListBox listCustomGroup = (ListBox)this.FormView1.FindControl("listCustomGroup");
        ListBox listGroup = (ListBox)this.FormView1.FindControl("listGroup");
        
        List<ListItem> itemsToAdd = new List<ListItem>();

        foreach (ListItem listItem in listCustomGroup.Items)
        {
            if (listItem.Selected)
                itemsToAdd.Add(listItem);
        }

        foreach (ListItem listItem in itemsToAdd)
        {
            ListItem lstExists = listGroup.Items.FindByText(listItem.Text);
            if (lstExists == null)
            {
                // Add item to Alloted Group
                listGroup.Items.Add(new ListItem(listItem.Text, listItem.Value));
            }

            // Remove item from Available Group

            listCustomGroup.Items.Remove(listItem);
        }

    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        fsMainBorder.Visible = false;

        ListBox listCustomGroup = (ListBox)this.FormView1.FindControl("listCustomGroup");
        ListBox listGroup = (ListBox)this.FormView1.FindControl("listGroup");
        
        List<ListItem> itemsToRemove = new List<ListItem>();
        
        foreach (ListItem listItem in listGroup.Items)
        {
            if (listItem.Selected)
                itemsToRemove.Add(listItem);
        }

        foreach (ListItem listItem in itemsToRemove)
        {
            listGroup.Items.Remove(listItem);

            ListItem lstExists = listCustomGroup.Items.FindByText(listItem.Text);
            if (lstExists == null)
            {
                // Add item to Available Group
                listCustomGroup.Items.Add(new ListItem(listItem.Text, listItem.Value));
            }
        }
    }
       
    #endregion

    #region ListBox Event when Updating

    protected void btnupdInsert_Click(object sender, EventArgs e)
    {
        btnNewCustomGroup.Visible = false;
        fsMainBorder.Visible = false;
        ListBox listCustomGroup = (ListBox)this.FormView1.FindControl("listupdCustomGroup");
        ListBox listGroup = (ListBox)this.FormView1.FindControl("listupdGroup");

        List<ListItem> itemsToAdd = new List<ListItem>();

        foreach (ListItem listItem in listCustomGroup.Items)
        {
            if (listItem.Selected)
                itemsToAdd.Add(listItem);
        }

        foreach (ListItem listItem in itemsToAdd)
        {
            ListItem lstExists = listGroup.Items.FindByText(listItem.Text);
            if (lstExists == null)
            {
                // Add item to Alloted Group
                listGroup.Items.Add(new ListItem(listItem.Text, listItem.Value));
            }

            // Remove item from Available Group

            listCustomGroup.Items.Remove(listItem);
        }

    }

    protected void btnupdRemove_Click(object sender, EventArgs e)
    {
        fsMainBorder.Visible = false;

        ListBox listCustomGroup = (ListBox)this.FormView1.FindControl("listupdCustomGroup");
        ListBox listGroup = (ListBox)this.FormView1.FindControl("listupdGroup");
        
        List<ListItem> itemsToRemove = new List<ListItem>();
                

        foreach (ListItem listItem in listGroup.Items)
        {
            if (listItem.Selected)
                itemsToRemove.Add(listItem);
        }

        foreach (ListItem listItem in itemsToRemove)
        {
            listGroup.Items.Remove(listItem);

            ListItem lstExists = listCustomGroup.Items.FindByText(listItem.Text);
            if (lstExists == null)
            {
                // Add item to Available Group
                listCustomGroup.Items.Add(new ListItem(listItem.Text, listItem.Value));
            }
        }
    }

    #endregion

    #region for gvCustomGroup binding multiple Custom Group Id column in one row

    protected void gvCustomGroup_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "lId") != DBNull.Value)
            {
                LinkButton lnkGroupName = (LinkButton)e.Row.FindControl("lnkGroupName");
                
                if (lnkGroupName != null)
                {
                    int GroupId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "lId"));
                    string strGroupName = "";

                    DataSet dsGroupName = DBOperations.GetAllotedCustomsGroupById(GroupId);

                    int intRowCount = dsGroupName.Tables[0].Rows.Count;

                    if (intRowCount > 0)
                    {
                        lnkGroupName.Text = dsGroupName.Tables[0].Rows[0]["GroupName"].ToString() + " ...";

                        foreach (DataRow dtRow in dsGroupName.Tables[0].Rows)
                        {
                            strGroupName = strGroupName + dtRow["GroupName"].ToString() + ",";
                        }
                        
                        // Show Group Name In LinkButton Tooltip
                    
                        strGroupName = strGroupName.Substring(0, strGroupName.LastIndexOf(","));
                        lnkGroupName.Text = strGroupName;
                        lnkGroupName.ToolTip = strGroupName;
                    }
                }
            }
        }
    }

    #endregion


}

