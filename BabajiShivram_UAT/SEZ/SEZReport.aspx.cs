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
public partial class SEZ_SEZReport : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Fill_ReportTree(1);
            Fill_ConditionalTree(1);     
            listReport.Items.Clear();
            listCondition.Items.Clear();
            ListItem listSelect = new ListItem("- Select - ", "0");
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "User Report";         
            if (Session["Lid"] != null)
            {
                ModifyAdhocReport(Convert.ToInt32(Session["Lid"]));
            }
        }
    }


    bool block = false;
    bool block1 = false;
    int CustomerId;
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


        DataSet dsFieldGroupName = SEZOperation.GetSEZReportFieldGroupTest();
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


            DataSet dsChildNode = SEZOperation.GetSEZReportChildNodeTest(ParentNode[loop, 0], ReportType);

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
        List<ListItem> itemsInConditionalList = new List<ListItem>();
        foreach (ListItem listItemCondition in listCondition.Items)
        {
            itemsInConditionalList.Add(listItemCondition);
        }

        TreeViewforConditon.Nodes.Clear();
        //CustomerId = Convert.ToInt32(ddCustomer.SelectedValue);//Fill TreeView for Customer 
        string[,] ParentNode = new string[100, 2];
        int count = 0;

        DataSet dsFieldGroupName = SEZOperation.GetSEZReportFieldGroupTest();
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
            TreeNode rootCondition = new TreeNode();
            string strParantNode = ParentNode[loop, 0];
            rootCondition.Text = ParentNode[loop, 1];
            rootCondition.Target = "_blank";
            
            DataSet dsChildNode = SEZOperation.GetSEZReportChildNodeTest(ParentNode[loop, 0], ReportType);

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

                if (strParantNode != "8") // Additional Field Not Applicable For Conditional Filter
                {
                    TreeViewforConditon.Nodes.Add(rootCondition);
                }
            }
        }
        TreeViewforConditon.CollapseAll();
    }

    private void ModifyAdhocReport(int ReportId)
    {
        btnSave.Visible = false;
        btnUpdate.Visible = true;
        btnCancel.Visible = true;
        txtReportName.Enabled = false;

       // ddReportType.Enabled = false;
       // ddCustomer.Enabled = false;
       // DBOperations.FillBabajiUserCustomer(ddCustomer, LoggedInUser.glUserId, true);

        DataSet dsReport = DBOperations.GetAdHocReportDetail(ReportId);

        txtReportName.Text = dsReport.Tables[0].Rows[0]["ReportName"].ToString();
        string ReportTypeId = dsReport.Tables[0].Rows[0]["ReportType"].ToString();
        string CustomerId = dsReport.Tables[0].Rows[0]["CustomerId"].ToString();

       // ddReportType.Items.FindByValue(ReportTypeId).Selected = true;
       // ddCustomer.SelectedValue = CustomerId;

        DataSet dsColumnField = DBOperations.GetReportColumnNamebyId(ReportId);

        //if (ReportTypeId == "2")
        //{
        //    if (ddCustomer.SelectedValue == "0")
        //    {
        //        btnUpdate.Visible = false;
        //        Session["Lid"] = null;
        //        lblError.Text = "Report Modification Not Allowed For Selected Customer!";
        //        lblError.CssClass = "errorMsg";
        //        return;
        //    }
        //}

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
        DataSet dsConditionFields = DBOperations.GetReportConditionFieldsbyId(ReportId);
        int RowCount = dsConditionFields.Tables[0].Rows.Count;
        for (int i = 0; i < RowCount; i++)
        {
            string field = dsConditionFields.Tables[0].Rows[i]["ConditionFieldName"].ToString();
            string Value = dsConditionFields.Tables[0].Rows[i]["FieldId"].ToString();
            listCondition.Items.Add(new ListItem(field, Value));
        }

        // Fill Available Report Field In TreeView
        //if (ReportTypeId == "2")
        //{
        //    ListItem lstItem = ddCustomer.Items.FindByValue(CustomerId);
        //    if (lstItem != null)
        //    {
        //        Session["ReportCustomerId"] = CustomerId;
        //        Fill_ReportTree(2);
        //       // Fill_ConditionalTree(2);
        //    }
        //    else
        //    {
        //        btnUpdate.Visible = false;
        //        Session["Lid"] = null;
        //        lblError.Text = "Report Modification Not Allowed For Selected Customer!";
        //        lblError.CssClass = "errorMsg";
        //        return;
        //    }

        //}
        //else 
        if (ReportTypeId == "1")
        {
            Fill_ReportTree(1);
            Fill_ConditionalTree(1);
        }

        btnSave.Visible = false;
        btnUpdate.Visible = true;
        btnCancel.Visible = true;

    }

    //private void ModifyAdhocReport(int ReportId)
    //{
    //    btnSave.Visible = false;
    //    txtReportName.Enabled = false;     

    //    DataSet dsReport = DBOperations.GetAdHocReportDetail(ReportId);

    //    txtReportName.Text = dsReport.Tables[0].Rows[0]["ReportName"].ToString();
    //    string ReportTypeId = dsReport.Tables[0].Rows[0]["ReportType"].ToString();
    //    string CustomerId = dsReport.Tables[0].Rows[0]["CustomerId"].ToString();      

    //    DataSet dsColumnField = DBOperations.GetReportColumnNamebyId(ReportId);

    //    // Fill Existing Report Field In listBox
    //    int ItemCount = dsColumnField.Tables[0].Rows.Count;
    //    //add Columnlist Item into Report list
    //    for (int i = 0; i < ItemCount; i++)
    //    {
    //        string field = dsColumnField.Tables[0].Rows[i]["FieldName"].ToString();
    //        string value = dsColumnField.Tables[0].Rows[i]["ColumnId"].ToString();
    //        listReport.Items.Add(new ListItem(field, value));
    //    }
    //    //Get ConditionColumn List into Filter list
    //    DataSet dsConditionFields = DBOperations.GetReportConditionFieldsbyId(ReportId);
    //    int RowCount = dsConditionFields.Tables[0].Rows.Count;
    //    for (int i = 0; i < RowCount; i++)
    //    {
    //        string field = dsConditionFields.Tables[0].Rows[i]["ConditionFieldName"].ToString();
    //        string Value = dsConditionFields.Tables[0].Rows[i]["FieldId"].ToString();
    //        //listCondition.Items.Add(new ListItem(field, Value));
    //    }
    //            //btnUpdate.Visible = false;
    //            Session["Lid"] = null;
    //            lblError.Text = "Report Modification Not Allowed For Selected Customer!";
    //            lblError.CssClass = "errorMsg";
    //            return;         
    //    }
    //Fill_ReportTree(1);
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        int ReportType = 0;
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
        int ReportType = 0;
        
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
        int ReportType = 1;

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

            Fill_ConditionalTree(ReportType);
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

            Fill_ConditionalTree(ReportType);
        }
    }
    protected void MoveUp_Click(object sender, EventArgs e)
    {
        // Multiple Item Moving Up 
        MoveListboxItems(-1, listReport);
    }
    protected void MoveDown_Click(object sender, EventArgs e)
    {
        // Multiple Item Moving Down 
        MoveListboxItems(1, listReport);       
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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strReportName = txtReportName.Text.Trim();
        string strReportFieldId = "", strConditionColumnId = "";
        int ReportType = 1, CustomerId = 0;
       // ReportType = Convert.ToInt32(ddReportType.SelectedValue);
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

            // int Result = DBOperations.AddAdhocReport(strReportName, strReportFieldId, strConditionColumnId, ReportType, CustomerId, LoggedInUser.glUserId);

            int Result = SEZOperation.AddSEZReport(strReportName, strReportFieldId, strConditionColumnId, ReportType, CustomerId, 0, LoggedInUser.glUserId, LoggedInUser.glModuleId);


            if (Result == 0)
            {
                txtReportName.Text = "";              
                Session["ReportCustomerId"] = null;
                Session["lid"] = null;
                listReport.Items.Clear();
                listCondition.Items.Clear();
                lblError.Text = "Report Created Successfully!";
                lblError.CssClass = "success";

                Fill_ReportTree(1); // Fill Tree View For babaji Report
                Fill_ConditionalTree(1);  

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
        int ReportType = 1, CustomerId = 0;

        if (ReportName == "")
        {
            lblError.Text = "Please Enter Report Name!";
            lblError.CssClass = "errorMsg";
            return;
        }      
        
        //if (ddReportType.SelectedValue == "0") // No Report Type Selected
        //{
        //    lblError.Text = "Please Select Report Type!";
        //    lblError.CssClass = "errorMsg";
        //    return;
        //}

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

            int Result = DBOperations.UpdateAdhocReport(Lid, ReportName, ReportFieldId, ReportType, CustomerId, ConditionColumnId, LoggedInUser.glUserId);

            if (Result == 0)
            {
                txtReportName.Text = "";
                txtReportName.Enabled = true;
                listReport.Items.Clear();
                listCondition.Items.Clear();
                //ddReportType.Enabled = true;
                //ddCustomer.Enabled = true;
                //ddReportType.SelectedValue = "0";
                //ddCustomer.SelectedValue = "0";
                lblError.Text = "Report Updated Successfully!";
                lblError.CssClass = "success";
                btnSave.Visible = true;
                btnUpdate.Visible = false;
                btnCancel.Visible = false;
                Session["Lid"] = null;
                Session["ReportCustomerId"] = null;
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
        //ddReportType.Enabled = true;
        //ddCustomer.Enabled = true;
        //ddReportType.SelectedValue = "0";
        //ddCustomer.SelectedValue = "0";
        listReport.Items.Clear();
        //listCondition.Items.Clear();
        btnCancel.Visible = false;
        Session["Lid"] = null;
        Session["ReportCustomerId"] = null;

        TreeViewField.Nodes.Clear(); // Clear Report Field
        //TreeViewforConditon.Nodes.Clear(); // Clear Conditional Field

    }
}