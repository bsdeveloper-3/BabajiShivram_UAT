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

public partial class Reports_AdHocCustomerReport : System.Web.UI.Page
{
    LoginClass LoggedInCustomer = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Customer Report";
            
            Session["ReportId"] = null;

            if (Session["Lid"] != null)
            {
                ModifyAdhocReport(Convert.ToInt32(Session["Lid"]));
            }
            else
            {
                Fill_ReportTree(2);
                Fill_ConditionalTree(2);
            }
        }
    }

    private void ModifyAdhocReport(int ReportId)
    {
        txtReportName.Enabled = false;
        btnSave.Visible = false;
        btnUpdate.Visible = true;
        btnCancel.Visible = true;
        listReport.Items.Clear();
        listCondition.Items.Clear();

        DataSet dsReport = DBOperations.GetAdHocReportDetail(ReportId);
        txtReportName.Text = dsReport.Tables[0].Rows[0]["ReportName"].ToString();

        DataSet dsColumnField = DBOperations.GetReportColumnNamebyId(ReportId);
        
        int ItemCount = dsColumnField.Tables[0].Rows.Count;
        
        //add Columnlist Item into Report list
        for (int i = 0; i < ItemCount; i++)
        {
            string items = dsColumnField.Tables[0].Rows[i]["FieldName"].ToString();
            string value = dsColumnField.Tables[0].Rows[i]["ColumnId"].ToString();
            listReport.Items.Add(new ListItem(items, value));

        }
        
        //Get ConditionColumn List into Filter list
        DataSet dsconditionFields = DBOperations.GetReportConditionFieldsbyId(ReportId);
        int Count = dsconditionFields.Tables[0].Rows.Count;
        for (int i = 0; i < Count; i++)
        {
            string fields = dsconditionFields.Tables[0].Rows[i]["ConditionFieldName"].ToString();
            string Value = dsconditionFields.Tables[0].Rows[i]["FieldId"].ToString();
            listCondition.Items.Add(new ListItem(fields, Value));
        }

        Fill_ReportTree(2);
        Fill_ConditionalTree(2);

        btnSave.Visible = false;
        btnUpdate.Visible = true;
        btnCancel.Visible = true;

    }
    
    protected void btnSave_Click(object sender, EventArgs e)
    {

        string strReportName = txtReportName.Text.Trim();
        string strReportFieldId = "", strConditionColumnId = "";
        int ReportType = 2; // Customer Report Type Id
        int CustomerId = LoggedInCustomer.glCustId;
        int UserId = LoggedInCustomer.glCustUserId;

        if (listReport.Items.Count == 0)
        {
            lblError.Text = "Please Add Report Field!";
            lblError.CssClass = "errorMsg";
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

            //int Result = DBOperations.AddAdhocReport(strReportName, strReportFieldId, strConditionColumnId, ReportType, CustomerId, UserId);

            int Result = DBOperations.AddAdhocReport(strReportName, strReportFieldId, strConditionColumnId, ReportType, 0, CustomerId, UserId);


            if (Result == 0)
            {
                txtReportName.Text = "";
                listReport.Items.Clear();
                listCondition.Items.Clear();
                TreeViewField.CollapseAll();
                TreeViewforConditon.CollapseAll();
                lblError.Text = "Report Created Successfully!";
                lblError.CssClass = "success";
            }
            else if (Result == 1)
            {
                lblError.Text = "System Error! Please try after sometime.";
                lblError.CssClass = "errorMsg";
            }
            else if (Result == 2)
            {
                lblError.Text = "Please Add Report Columns!";
                lblError.CssClass = "errorMsg";
            }
            else if (Result == 3)
            {
                lblError.Text = "Report Name Already Exist!";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        int Lid = Convert.ToInt32(Session["Lid"]);
        string ReportName = txtReportName.Text.Trim();
        string ReportFieldId = "", ConditionColumnId = "";
        int ReportType = 2;
        int CustomerId = LoggedInCustomer.glCustId;
        int UserId = LoggedInCustomer.glCustUserId;
        if (listReport.Items.Count == 0)
        {
            lblError.Text = "Please Add Report Field!";
            lblError.CssClass = "errorMsg";
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

            int Result = DBOperations.UpdateAdhocReport(Lid, ReportName, ReportFieldId,ReportType,CustomerId,ConditionColumnId,UserId);

            if (Result == 0)
            {
                txtReportName.Text = "";
                txtReportName.Enabled = true;
                listReport.Items.Clear();
                listCondition.Items.Clear();
                Fill_ReportTree(2);
                Fill_ConditionalTree(2);
                lblError.Text = "Report Updated Successfully!";
                lblError.CssClass = "success";
                btnSave.Visible = true;
                btnUpdate.Visible = false;
                btnCancel.Visible = false;
                Session["Lid"] = null;

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
        txtReportName.Text = "";
        txtReportName.Enabled = true;
        btnSave.Visible = true;
        btnUpdate.Visible = false;
        
        listReport.Items.Clear();
        listCondition.Items.Clear();
        
        Fill_ReportTree(2);
        Fill_ConditionalTree(2);

        btnCancel.Visible = false;
        Session["Lid"] = null;

    }

    #region ListBox Event

    protected void btnInsert_Click(object sender, EventArgs e)
    {
        int ReportType = 2;

        List<ListItem> itemsToRemove = new List<ListItem>();

        foreach (TreeNode node in TreeViewField.CheckedNodes)
        {
            ListItem lstExists = listReport.Items.FindByText(node.Text);
            if (lstExists == null)
            {
                listReport.Items.Add(new ListItem(node.Text, node.Value));
            }
        }

        Fill_ReportTree(ReportType);
    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        int ReportType = 2;
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

        Fill_ReportTree(ReportType);
    }

    protected void btnAddCondition_Click(object sender, EventArgs e)
    {
        int ReportType = 2;

        foreach (TreeNode node in TreeViewforConditon.CheckedNodes)
        {
            ListItem lstExists = listCondition.Items.FindByText(node.Text);
            if (lstExists == null)
            {
                listCondition.Items.Add(new ListItem(node.Text, node.Value));
            }
        }

        Fill_ConditionalTree(ReportType);
    }

    protected void btnRemoveCondition_Click(object sender, EventArgs e)
    {
        int ReportType = 2;

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

        Fill_ConditionalTree(ReportType);
    }

    protected void MoveUp_Click(object sender, EventArgs e)
    {
        // Multiple Item Moving Up 
        MoveListboxItems(-1, listReport);

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
    }

    protected void MoveDown_Click(object sender, EventArgs e)
    {
        // Multiple Item Moving Down 
        MoveListboxItems(1, listReport);

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
                        lblError.CssClass = "ErrorMsg";
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
            lblError.CssClass = "ErrorMsg";
        };
    }

    #endregion

    #region TreeView
    bool block = false;
    bool block1 = false;
    void Fill_ReportTree(int ReportType)
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
        
        /******Test Comment*********
        SqlDataReader drFieldGroupName = DBOperations.GetAdhocReportFieldGroup();
        if (drFieldGroupName.HasRows)
        {
            while (drFieldGroupName.Read())
            {
                ParentNode[count, 0] = drFieldGroupName.GetValue(drFieldGroupName.GetOrdinal("lid")).ToString();// Parent ID
                ParentNode[count++, 1] = drFieldGroupName.GetValue(drFieldGroupName.GetOrdinal("sName")).ToString();//Parent Name
            }
        }
        
        drFieldGroupName.Close();
        drFieldGroupName.Dispose();
        
        ******END Test Comment**************/
        //

        /******New Test Code*********/

        DataSet dsFieldGroupName = DBOperations.GetAdhocReportFieldGroupTest();
        if (dsFieldGroupName.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in dsFieldGroupName.Tables[0].Rows)
            {
                ParentNode[count, 0] = dr["lid"].ToString();// Parent ID
                ParentNode[count++, 1] = dr["sName"].ToString();//Parent Name
            }
        }
        /******END New Test Code **************/

        for (int loop = 0; loop < count; loop++)
        {

            TreeNode root = new TreeNode();
            
            root.Text = ParentNode[loop, 1];
            string strParantNode = ParentNode[loop, 0];
            root.Target = "_blank";
            
            /******Test Comment*********
            
             SqlDataReader drChildNode = DBOperations.GetAdhocReportChildNode(ParentNode[loop, 0], ReportType, LoggedInCustomer.glCustId);

            if (drChildNode.HasRows)
            {
                while (drChildNode.Read())
                {
                    bool bChildExists = false;

                    TreeNode child = new TreeNode();

                    child.ShowCheckBox = true;
                                        
                    child.Value = drChildNode.GetValue(drChildNode.GetOrdinal("FieldId")).ToString();//Child Id;
                    child.Text = drChildNode.GetValue(drChildNode.GetOrdinal("FieldName")).ToString();//Child Name
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
                }
            }
                drChildNode.Close();
                drChildNode.Dispose();
             
             *******END Test Comment**************/
            /****** New Test Code **************/
            DataSet dsChildNode = DBOperations.GetAdhocReportChildNodeTest(ParentNode[loop, 0], ReportType, LoggedInCustomer.glCustId);

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

    void Fill_ConditionalTree(int ReportType)
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

        /******Test Comment*********
        SqlDataReader drFieldGroupName = DBOperations.GetAdhocReportFieldGroup();
        if (drFieldGroupName.HasRows)
        {
            while (drFieldGroupName.Read())
            {
                ParentNode[count, 0] = drFieldGroupName.GetValue(drFieldGroupName.GetOrdinal("lid")).ToString();// Parent ID
                ParentNode[count++, 1] = drFieldGroupName.GetValue(drFieldGroupName.GetOrdinal("sName")).ToString();//Parent Name
            }
        }
        drFieldGroupName.Close();
        drFieldGroupName.Dispose();
        
        /******END Test Comment*********/

        /****New Test Code*************/

        DataSet dsFieldGroupName = DBOperations.GetAdhocReportFieldGroupTest();
        if (dsFieldGroupName.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in dsFieldGroupName.Tables[0].Rows)
            {
                ParentNode[count, 0] = dr["lid"].ToString();// Parent ID
                ParentNode[count++, 1] = dr["sName"].ToString();//Parent Name
            }
        }
        /******END New Test Code **************/
        for (int loop = 0; loop < count; loop++)
        {
            TreeNode rootCondition = new TreeNode();
            
            string strParantNode = ParentNode[loop, 0];
            
            rootCondition.Text = ParentNode[loop, 1];
            rootCondition.Target = "_blank";

            /******Test Comment*********
            SqlDataReader drChildNode = DBOperations.GetAdhocReportChildNode(ParentNode[loop, 0], ReportType, LoggedInCustomer.glCustId);

            if (drChildNode.HasRows)
            {
                while (drChildNode.Read())
                {
                    bool bChildConditionExists = false;
                    
                    TreeNode childCondition = new TreeNode();

                    childCondition.ShowCheckBox = true;
                    childCondition.Value = drChildNode.GetValue(drChildNode.GetOrdinal("FieldId")).ToString();
                    childCondition.Text = drChildNode.GetValue(drChildNode.GetOrdinal("FieldName")).ToString();
                    childCondition.Target = "_blank";

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
                }
                drChildNode.Close();
                drChildNode.Dispose();
             ******Test Comment*********/

            /****** New Test Code **************/
            
            DataSet dsChildNode = DBOperations.GetAdhocReportChildNodeTest(ParentNode[loop, 0], ReportType, LoggedInCustomer.glCustId);

            if (dsChildNode.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drChildNode in dsChildNode.Tables[0].Rows)
                {
                    bool bChildConditionExists = false;

                    TreeNode childCondition = new TreeNode();

                    childCondition.ShowCheckBox = true;
                    childCondition.Value = drChildNode["FieldId"].ToString();//Child Id;
                    childCondition.Text = drChildNode["FieldName"].ToString();//Child Name
                    childCondition.Target = "_blank";

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

                /******END New Test Code **************/
                if (strParantNode != "8") // Additional Field Nt Applicable For Conditional Filter
                {
                    TreeViewforConditon.Nodes.Add(rootCondition);
                }
            }
        }

        TreeViewforConditon.CollapseAll();
    }

    protected void TreeViewField_OnTreeNodeExpanded(object sender, TreeNodeEventArgs e)
    {
        if (block == true)
            return;
        block = true;
        TreeViewField.CollapseAll();
        e.Node.Expand();
        block = false;

    }
    
    protected void TreeViewforConditon_OnTreeNodeExpanded(object sender, TreeNodeEventArgs e)
    {
        if (block1 == true)
            return;
        block1 = true;
        TreeViewforConditon.CollapseAll();
        e.Node.Expand();
        block1 = false;
    }
        
    #endregion
}
