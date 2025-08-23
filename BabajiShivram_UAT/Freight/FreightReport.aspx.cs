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
using AjaxControlToolkit;
using System.Data.SqlClient;

public partial class Freight_FreightReport : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Freight Report";

            if (Session["Lid"] != null)
            {
                ModifyAdhocReport(Convert.ToInt32(Session["Lid"]));
            }

            Fill_ReportTree();
            Fill_ConditionalTree();
        }
    }
        
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strReportName = txtReportName.Text.Trim();
        string strReportFieldId = "", strConditionColumnId = "";
        
        if (strReportName == "")
        {
            lblError.Text = "Please Enter Report Name!";
            lblError.CssClass = "errorMsg";
            return;
        }

        if (listReport.Items.Count == 0)
        {
            lblError.Text = "Please Add Report Field!";
            lblError.CssClass = "errorMsg";
            return;
        }
        else
        {
            foreach (ListItem lstItem in listReport.Items)
            {
                strReportFieldId += lstItem.Value + ",";
            }

            foreach (ListItem lstItem in listCondition.Items)
            {
                strConditionColumnId += lstItem.Value + ",";
            }

            int Result = DBOperations.AddFreightAdhocReport(strReportName, strReportFieldId, strConditionColumnId, LoggedInUser.glUserId);

            if (Result == 0)
            {
                txtReportName.Text = "";
                
                Session["lid"]      = null;
                
                lblError.Text       = "Report Created Successfully!";
                lblError.CssClass   = "success";
                
                listReport.Items.Clear();
                listCondition.Items.Clear();

                Fill_ReportTree(); // Fill Tree View For babaji Report
                Fill_ConditionalTree();

            }
            else if (Result == 1)
            {
                lblError.Text       = "System Error! Please try after sometime.";
                lblError.CssClass   = "errorMsg";
            }
            else if (Result == 2)
            {
                lblError.Text       = "Please Add Report Columns!";
                lblError.CssClass   = "errorMsg";
            }
            else if (Result == 3)
            {
                lblError.Text       = "Report Name Already Exist!";
                lblError.CssClass   = "errorMsg";
            }
        }
    }

    private void ModifyAdhocReport(int ReportId)
    {
        btnSave.Visible         = false;
        btnUpdate.Visible       = true;
        btnCancel.Visible       = true;
        txtReportName.Enabled   = false;

        DataSet dsReport = DBOperations.GetFreightAdHocReportDetail(ReportId);

        txtReportName.Text  = dsReport.Tables[0].Rows[0]["ReportName"].ToString();
      //  string ReportTypeId = dsReport.Tables[0].Rows[0]["ReportType"].ToString();
      //  string CustomerId   = dsReport.Tables[0].Rows[0]["CustomerId"].ToString();

        DataSet dsColumnField = DBOperations.GeFreighttReportColumnNamebyId(ReportId);

        // Fill Existing Report Field In listBox
        int ItemCount = dsColumnField.Tables[0].Rows.Count;
        
        //add Columnlist Item into Report list
        for (int i = 0; i < ItemCount; i++)
        {
            string field = dsColumnField.Tables[0].Rows[i]["FieldName"].ToString();
            string value = dsColumnField.Tables[0].Rows[i]["ColumnId"].ToString();
            listReport.Items.Add(new ListItem(field, value));
        }
        //Get ConditionColumn List into Filter list
        DataSet dsConditionFields = DBOperations.GetFreightReportConditionFieldsbyId(ReportId);
        int RowCount = dsConditionFields.Tables[0].Rows.Count;
        for (int i = 0; i < RowCount; i++)
        {
            string field = dsConditionFields.Tables[0].Rows[i]["ConditionFieldName"].ToString();
            string Value = dsConditionFields.Tables[0].Rows[i]["FieldId"].ToString();
            listCondition.Items.Add(new ListItem(field, Value));
        }

        // Fill Available Report Field In TreeView
        
        Fill_ReportTree();
        Fill_ConditionalTree();
        
        btnSave.Visible     = false;
        btnUpdate.Visible   = true;
        btnCancel.Visible   = true;

    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        int Lid = Convert.ToInt32(Session["Lid"]);
        string ReportName       = txtReportName.Text.Trim();
        string ReportFieldId    = "", ConditionColumnId = "";
        
        if (ReportName == "")
        {
            lblError.Text = "Please Enter Report Name!";
            lblError.CssClass = "errorMsg";
            return;
        }
                
        if (listReport.Items.Count == 0)
        {
            lblError.Text = "Please Add Report Field!";
            lblError.CssClass = "errorMsg";
            return;
        }
        else
        {
            foreach (ListItem lstItem in listReport.Items)
            {
                ReportFieldId += lstItem.Value + ",";
            }

            foreach (ListItem lstItem in listCondition.Items)
            {
                ConditionColumnId += lstItem.Value + ",";
            }

            int Result = DBOperations.UpdateFreightAdhocReport(Lid, ReportName, ReportFieldId, ConditionColumnId, LoggedInUser.glUserId);

            if (Result == 0)
            {
                txtReportName.Text = "";
                txtReportName.Enabled = true;
                listReport.Items.Clear();
                listCondition.Items.Clear();
                lblError.Text = "Report Updated Successfully!";
                lblError.CssClass = "success";
                btnSave.Visible = true;
                btnUpdate.Visible = false;
                btnCancel.Visible = false;
                Session["Lid"] = null;
                TreeViewField.Nodes.Clear(); // Clear Report Field
                TreeViewforConditon.Nodes.Clear(); // Clear Conditional Field
            }
            else if (Result == 1)
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
            }
            else if (Result == 2)
            {
                lblError.Text = "Report Name Already Exist!";
                lblError.CssClass = "warning";
            }


        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        btnSave.Visible = true;
        btnUpdate.Visible = false;
        txtReportName.Text = "";
        txtReportName.Enabled = true;
        listReport.Items.Clear();
        listCondition.Items.Clear();
        btnCancel.Visible = false;
        Session["Lid"] = null;
     
        TreeViewField.Nodes.Clear(); // Clear Report Field
        TreeViewforConditon.Nodes.Clear(); // Clear Conditional Field

    }

    #region ListBox Event

    protected void btnInsert_Click(object sender, EventArgs e)
    {
        int ReportType = 1;

        if (ReportType > 0)
        {
            foreach (TreeNode node in TreeViewField.CheckedNodes)
            {
                ListItem lstExists = listReport.Items.FindByText(node.Text);
                if (lstExists == null)
                {
                    listReport.Items.Add(new ListItem(node.Text, node.Value));
                }
            }

            Fill_ReportTree();
        }
    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        int ReportType = 1;// Convert.ToInt32(ddReportType.SelectedValue);

        if (ReportType > 0)
        {
            List<ListItem> itemsToRemove = new List<ListItem>();

            foreach (ListItem listItem in listReport.Items)
            {
                if (listItem.Selected)
                    itemsToRemove.Add(listItem);
            }

            foreach (ListItem listItem in itemsToRemove)
            {
                listReport.Items.Remove(listItem);
            }

            Fill_ReportTree();
        }
    }

    protected void btnAddCondition_Click(object sender, EventArgs e)
    {
        int ReportType = 1;// Convert.ToInt32(ddReportType.SelectedValue);

        if (ReportType > 0)
        {
            foreach (TreeNode node in TreeViewforConditon.CheckedNodes)
            {
                ListItem lstExists = listCondition.Items.FindByText(node.Text);
                if (lstExists == null)
                {
                    listCondition.Items.Add(new ListItem(node.Text, node.Value));
                }
            }

            Fill_ConditionalTree();
        }
    }

    protected void btnRemoveCondition_Click(object sender, EventArgs e)
    {
        int ReportType = 1;// Convert.ToInt32(ddReportType.SelectedValue);
        if (ReportType > 0)
        {
            List<ListItem> itemsToRemove = new List<ListItem>();

            foreach (ListItem Items in listCondition.Items)
            {
                if (Items.Selected)
                    itemsToRemove.Add(Items);
            }
            foreach (ListItem listItems in itemsToRemove)
            {
                listCondition.Items.Remove(listItems);
            }

            Fill_ConditionalTree();
        }
    }

    protected void MoveUp_Click(object sender, EventArgs e)
    {
        // Multiple Item Moving Up 
        MoveListboxItems(-1, listReport);
        #region Old Code

        //****************** OLD Code for single item selection Moving UP *********************//
        /*
        //this.Options is ListBox
        if (listReport.SelectedIndex == -1)
            return;
        if (listReport.SelectedIndex == 0)
        {
            int MaxIndex = listReport.Items.Count -1;
            ListItem listZero = listReport.SelectedItem;
            listReport.Items.RemoveAt(0);
            listReport.Items.Insert(MaxIndex, listZero);
        }
        else
        {
            ListItem item, aboveItem;
            int itemIndex, aboveItemIndex;
            itemIndex = listReport.SelectedIndex;
            aboveItemIndex = listReport.SelectedIndex - 1;
            item = (ListItem)listReport.Items[itemIndex];
            aboveItem = (ListItem)listReport.Items[aboveItemIndex];

            listReport.Items.RemoveAt(aboveItemIndex);
            listReport.Items.Insert(itemIndex, aboveItem);
        }

        */
        #endregion
    }

    protected void MoveDown_Click(object sender, EventArgs e)
    {
        // Multiple Item Moving Down 
        MoveListboxItems(1, listReport);
        #region old Code
        //****************** OLD Code for single item selection Moving Down *********************//
        /*
        //this.Options is ListBox
        if (listReport.SelectedIndex == -1 || listReport.SelectedIndex >= listReport.Items.Count)
            return;

        ListItem item, belowItem;
        int itemIndex, belowItemIndex;
        itemIndex = listReport.SelectedIndex;
        belowItemIndex = listReport.SelectedIndex + 1;
        if (belowItemIndex >= listReport.Items.Count)
            return;
        item = (ListItem)listReport.Items[itemIndex];
        belowItem = (ListItem)listReport.Items[belowItemIndex];

        listReport.Items.RemoveAt(itemIndex);
        listReport.Items.Insert(belowItemIndex, item);
        listReport.SelectedIndex = belowItemIndex;
        
        **************************************************************/
        #endregion
    }

    private void MoveListboxItems(int step, ListBox lb)
    {
        /* 'step' should be:
         *   -1 for moving selected items up
         *    1 for moving selected items down
         * 'lb' is your ListBox
         *   see examples how to call below this function
        */
        try
        {
            // do only something when really an item is selected
            if (lb.SelectedIndex > -1)
            {
                // get some needed values - they change while we manipulate the listbox
                // but we need them as they was original

                List<ListItem> SelectedItems = new List<ListItem>();
                List<int> SelectedIndices = new List<int>();

                int count = listReport.Items.Count;

                for (int i = 0; i < count; i++)
                {
                    if (listReport.Items[i].Selected == true)
                    {
                        ListItem llitem = listReport.Items[i];
                        SelectedItems.Add(llitem);
                        SelectedIndices.Add(i);
                    }

                }

                // set some default values
                int selIndex = -1;
                int newIndex = -1;
                int selCount = SelectedItems.Count;
                int lc = 0;
                int mc = 0;
                string moveOldText = string.Empty;
                string moveOldValue = string.Empty;
                string selectedItemText = string.Empty;
                string selectedItemValue = string.Empty;

                if (step == 1)
                {
                    mc = selCount - 1;
                }
                else
                {
                    mc = lc;
                }
                // enter the loop through the selected items
                while (lc < selCount)
                {
                    selectedItemText = string.Empty;
                    selectedItemValue = string.Empty;
                    moveOldText = string.Empty;
                    moveOldValue = string.Empty;

                    try
                    {
                        // get the item that should get moved
                        selectedItemText = SelectedItems[mc].Text;
                        selectedItemValue = SelectedItems[mc].Value;
                        selIndex = Convert.ToInt32(SelectedIndices[mc]);
                    }
                    catch
                    {
                        selIndex = -1;
                    }
                    // gen index for new place
                    newIndex = selIndex + step;
                    try
                    {
                        // get the old value from the place where the item get moved
                        moveOldText = lb.Items[newIndex].Text;
                        moveOldValue = lb.Items[newIndex].Value;
                    }
                    catch { /* do nothing */ }
                    try
                    {
                        if (!String.IsNullOrEmpty(selectedItemValue) && !String.IsNullOrEmpty(moveOldValue) && selIndex != -1 && newIndex != -1)
                        {
                            // move selected item
                            ListItem listNewItem = new ListItem(selectedItemText, selectedItemValue);
                            // hold the moved item selected
                            listNewItem.Selected = true;

                            lb.Items.RemoveAt(newIndex);
                            lb.Items.Insert(newIndex, listNewItem);
                            // write old value back to the old place from selected item
                            lb.Items.RemoveAt(selIndex);

                            ListItem listOldItem = new ListItem(moveOldText, moveOldValue);
                            lb.Items.Insert(selIndex, listOldItem);
                        }
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = ex.Message;
                        lblError.CssClass = "errorMsg";
                    }
                    lc++;
                    if (step == 1)
                    {
                        mc -= step;
                    }
                    else
                    {
                        mc = lc;
                    }

                }// END_While

            }
        }
        catch (Exception Ex)
        {
            lblError.Text = Ex.Message;
            lblError.CssClass = "errorMsg";
        };
    }
    #endregion

    #region TreeView
    
    void Fill_ReportTree()
    {
        /*********************************************/

        List<ListItem> itemsInListBox = new List<ListItem>();

        foreach (ListItem listItem in listReport.Items)
        {
            itemsInListBox.Add(listItem);
        }

        /********************************************/

        TreeViewField.Nodes.Clear();

        string[,] ParentNode = new string[100, 2];

        int count = 0;

        DataSet dsFieldGroupName = DBOperations.GetFreightReportFieldGroup();
        
        if (dsFieldGroupName.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in dsFieldGroupName.Tables[0].Rows)
            {
                ParentNode[count, 0] = dr["lid"].ToString();// Parent ID
                ParentNode[count++, 1] = dr["sName"].ToString();//Parent Name
            }
        }
        
        for (int loop = 0; loop < count; loop++)
        {

            TreeNode root = new TreeNode();

            root.Text = ParentNode[loop, 1];
            string strParantNode = ParentNode[loop, 0];
            root.Target = "_blank";

            DataSet dsChildNode = DBOperations.GetFreightReportChildNode(ParentNode[loop, 0]);

            if (dsChildNode.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drChildNode in dsChildNode.Tables[0].Rows)
                {
                    bool bChildExists = false;

                    TreeNode child = new TreeNode();

                    child.ShowCheckBox = true;
                    child.Value = drChildNode["FieldId"].ToString();//Child Id;
                    child.Text = drChildNode["FieldName"].ToString();//Child Name
                    child.Target = "_blank";

                    foreach (ListItem listItem in itemsInListBox)
                    {
                        if (listItem.Text == child.Text)
                        {
                            bChildExists = true;
                            break;
                        }
                    }
                    if (bChildExists == false)
                    {
                        root.ChildNodes.Add(child);
                    }
                }//END_IF

                /******END New Test Code **************/

                TreeViewField.Nodes.Add(root);

            }
        }

        TreeViewField.CollapseAll();
    }

    void Fill_ConditionalTree()
    {
        /*********************************************/

        List<ListItem> itemsInConditionalList = new List<ListItem>();

        foreach (ListItem listItemCondition in listCondition.Items)
        {
            itemsInConditionalList.Add(listItemCondition);
        }

        /********************************************/

        TreeViewforConditon.Nodes.Clear();

        string[,] ParentNode = new string[100, 2];

        int count = 0;

        DataSet dsFieldGroupName = DBOperations.GetFreightReportFieldGroup();
        if (dsFieldGroupName.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in dsFieldGroupName.Tables[0].Rows)
            {
                ParentNode[count, 0] = dr["lid"].ToString();// Parent ID
                ParentNode[count++, 1] = dr["sName"].ToString();//Parent Name
            }
        }

        for (int loop = 0; loop < count; loop++)
        {
            TreeNode rootCondition  =   new TreeNode();

            string strParantNode    =   ParentNode[loop, 0];

            rootCondition.Text      =   ParentNode[loop, 1];
            rootCondition.Target    =   "_blank";

            DataSet dsChildNode = DBOperations.GetFreightReportChildNode(ParentNode[loop, 0]);

            if (dsChildNode.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drChildNode in dsChildNode.Tables[0].Rows)
                {
                    bool bChildConditionExists = false;

                    TreeNode childCondition = new TreeNode();

                    childCondition.ShowCheckBox = true;
                    childCondition.Value    =   drChildNode["FieldId"].ToString();  //Child Id;
                    childCondition.Text     =   drChildNode["FieldName"].ToString();//Child Name
                    childCondition.Target   =   "_blank";

                    foreach (ListItem listItem in itemsInConditionalList)
                    {
                        if (listItem.Text == childCondition.Text)
                        {
                            bChildConditionExists = true;
                            break;
                        }
                    }
                    if (bChildConditionExists == false)
                    {
                        rootCondition.ChildNodes.Add(childCondition);
                    }
                }//END_IF

                TreeViewforConditon.Nodes.Add(rootCondition);
            }
        }

        TreeViewforConditon.CollapseAll();

    }

    #endregion

}