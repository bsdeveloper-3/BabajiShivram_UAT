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
public partial class DynamicData_Content_LeftNavigation : System.Web.UI.UserControl
{
    private LoginClass LoggedInUser = new LoginClass();

    #region Events


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }

    protected void trMktMenu_TreeNodePopulate(object sender, TreeNodeEventArgs e)
    {
        if (e.Node.ChildNodes.Count == 0)
        {
            switch (e.Node.Depth)
            {
                case 0:
                    GetParentNodeItem(e.Node);
                    break;
                    //case 1:
                    //    GetChildNodeItem(e.Node);
                    //    break;
            }
        }
        //END_IF

    }

    #endregion

    #region Functions

    // added by amit
    public void FillTree()
    {
        GetParentNodeItem((TreeNode)trMktMenu.FindNode("Menu"));
    }
    //End

    void GetParentNodeItem(TreeNode node)
    {
        MnuItemList mlist = new MnuItemList();

        string strRootURL = "~/Dashboard.aspx";
        string strRootName = "Dashboard";

        //----------Start Billing            
        int rolename = BillingOperation.GetRole(LoggedInUser.glUserId, LoggedInUser.glRoleId);

        if (rolename == 0) //Billing
        {
            strRootURL = "~/DashboardBillingUser.aspx";
            //strRootURL = "~/Dashboard_Billing.aspx";
            strRootName = "Dashboard";
        }
        //else if (rolename == 1) //HOD
        //{
        //    strRootURL = "~/Dashboard_Billing.aspx";
        //    strRootName = "Dashboard";
        //}
        else
        {
            strRootURL = "~/Dashboard.aspx";
            strRootName = "Dashboard";
        }
        //----------end Billing

        if (LoggedInUser.glModuleId == 1)
        {
            //----------Start Billing            
            if (rolename == 0) //Billing
            {
                strRootURL = "~/DashboardBillingUser.aspx";
                strRootName = "Dashboard";
            }
            //else if (rolename == 1) //HOD
            //{
            //    strRootURL = "~/Dashboard_Billing.aspx";
            //    strRootName = "Dashboard";
            //}
            else
            {
                strRootURL = "~/Dashboard.aspx";
                strRootName = "Dashboard";
            }
            //strRootURL = "~/Dashboard.aspx";
            //----------end Billing
        }
        else if (LoggedInUser.glModuleId == 2)
        {
            strRootURL = "~/Freight/FreightDashboard.aspx";
            strRootName = "Freight Dashboard";
            trMktMenu.ExpandDepth = 2;
        }
        else if (LoggedInUser.glModuleId == 3)
        {
            strRootURL = "~/Transport/TransDashboard.aspx";
            strRootName = "Dashboard";
        }
        else if (LoggedInUser.glModuleId == 4)
        {
            strRootURL = "~/Service/ServiceDashboard.aspx";
            strRootName = "Dashboard";
        }
        else if (LoggedInUser.glModuleId == 5)
        {
            strRootURL = "~/ExportCHA/ExportDashboard.aspx";
            strRootName = "Dashboard";
        }
        else if (LoggedInUser.glModuleId == 6)
        {
            strRootURL = "~/Quotation/QuoteDashboard.aspx";
            strRootName = "Dashboard";
        }
        else if (LoggedInUser.glModuleId == 8)
        {
            strRootURL = "~/SEZ/SEZDashboard.aspx";
            strRootName = "Dashboard";
        }
        else if (LoggedInUser.glModuleId == 9) // Account Expense
        {
            strRootURL = "~/AccountExpense/Dashboard.aspx";
            strRootName = "Dashboard";
        }
        else if (LoggedInUser.glModuleId == 10) // Container Movement
        {
            strRootURL = "~/ContMovement/CDashboard.aspx";
            strRootName = "Dashboard";
        }
        else if (LoggedInUser.glModuleId == 11) // CRM
        {
            strRootURL = "~/CRM/Dashboard.aspx";
            strRootName = "Dashboard";
        }
        else if (LoggedInUser.glModuleId == 15) // Reporting
        {
            strRootURL = "~/ReportDashboard.aspx";
            strRootName = "Dashboard";
        }

        TreeNode RootNode = (TreeNode)trMktMenu.FindNode("Menu");

        RootNode.NavigateUrl = strRootURL;

        //  mlist = TreeMenuClass.GetParentItem(LoggedInUser.glModuleId, LoggedInUser.glCompanyId, LoggedInUser.glRoleId, LoggedInUser.glSystemUser);

        if (Session["mlist"] == null)
        {
            mlist = TreeMenuClass.GetParentItem(LoggedInUser.glModuleId, LoggedInUser.glRoleId, LoggedInUser.glType);

            Session["mlist"] = mlist;
        }
        else
        {
            mlist = (MnuItemList)Session["mlist"];
        }

        if (mlist.Count > 0)
        {
            RootNode.Text = strRootName;
            RootNode.NavigateUrl = strRootURL;

            Dictionary<string, TreeNode> dictChildNode = new Dictionary<string, TreeNode>();

            int i = 1;
            if (Session["ChildNodes"] != null)
            {
                dictChildNode = (Dictionary<string, TreeNode>)Session["ChildNodes"];

                foreach (MnuItem m in mlist)
                {
                    if (dictChildNode.ContainsKey(m.Id.ToString()))
                    {
                        TreeNode newNode = new TreeNode(m.Name, m.Id);
                        newNode.PopulateOnDemand = false;
                        newNode.SelectAction = TreeNodeSelectAction.Expand;
                        // newNode.NavigateUrl = m.PLink;

                        if (i == 1)
                        {
                            newNode.ImageUrl = "~/Images/bs-user.png";
                        }
                        if (i == 2)
                        {
                            newNode.ImageUrl = "~/Images/settings.png";
                        }
                        if (i == 3)
                        {
                            newNode.ImageUrl = "~/Images/report.png";
                        }

                        newNode = (TreeNode)dictChildNode[newNode.Value];
                        newNode.Collapse();

                        node.ChildNodes.Add(newNode);
                    }
                }
            }
            else
            {
                foreach (MnuItem m in mlist)
                {
                    TreeNode newNode = new TreeNode(m.Name, m.Id);
                    newNode.PopulateOnDemand = false;
                    newNode.SelectAction = TreeNodeSelectAction.Expand;
                    // newNode.NavigateUrl = m.PLink;
                    if (i == 1)
                    {
                        newNode.ImageUrl = "~/Images/bs-user.png";
                    }
                    if (i == 2)
                    {
                        newNode.ImageUrl = "~/Images/settings.png";
                    }
                    if (i == 3)
                    {
                        newNode.ImageUrl = "~/Images/report.png";
                    }

                    if (GetChildNodeItem(newNode, LoggedInUser.glModuleId.ToString()) == true)
                    {
                        node.ChildNodes.Add(newNode);
                        dictChildNode[newNode.Value] = newNode;
                        Session["ChildNodes"] = dictChildNode;
                    }

                    i = i + 1;
                }
            }
        }

        if (HttpContext.Current.Session["NodeSelected"] != null)
        {
            TreeNode ExpandedNode = trMktMenu.FindNode(HttpContext.Current.Session["NodeSelected"].ToString());
            ExpandedNode.Expand();
        }

        // get the saved state of all nodes.
        //  new TreeViewState().RestoreTreeView(trMktMenu, this.GetType().ToString());
    }

    bool GetChildNodeItem(TreeNode node, string ModuleId)
    {
        bool a = false;

        SubMnuItemList subLlist = TreeMenuClass.GetsubMenuItem(node.Value, LoggedInUser.glModuleId, LoggedInUser.glRoleId, LoggedInUser.glType);

        if (subLlist.Count > 0)
        {
            foreach (SubMnuItem s in subLlist)
            {
                TreeNode newNode = new TreeNode(s.Name, s.Id);
                newNode.PopulateOnDemand = false;
                newNode.Value = s.PLink;
                node.ChildNodes.Add(newNode);
            }
            a = true;
        }
        else
            a = false;

        //called in Parent Node
        //trMktMenu.CollapseAll();
        //// get the saved state of all nodes.
        //  new TreeViewState().RestoreTreeView(trMktMenu, this.GetType().ToString());

        return a;
    }

    protected void trMktMenu_SelectedNodeChanged(object sender, EventArgs e)
    {
        if (trMktMenu.SelectedNode.Value != string.Empty)
        {
            HttpContext.Current.Session["NodeSelected"] = trMktMenu.SelectedNode.Parent.ValuePath;

            // Session["SelectedNodeValuePath"] = trMktMenu.SelectedNode.ValuePath;
            Response.Redirect(trMktMenu.SelectedNode.Value);
        }
    }

    /********** Code Required- Used for Save TreeView State for Expand/Collapse Node
    protected void trMktMenu_Unload(object sender, EventArgs e)
    {
        // save the state of all nodes.
     //   new TreeViewState().SaveTreeView(trMktMenu, this.GetType().ToString());
    }
    ***********************************************/

    //Commented by amit
    /*************************************************
    void GetParentNodeItem(TreeNode node)
    {
       MnuItemList mlist = TreeMenuClass.GetParentItem();
       if (mlist.Count > 0)
       {
           foreach (MnuItem m in mlist)
           {
               TreeNode newNode = new TreeNode(m.Name, m.Id);
               newNode.PopulateOnDemand = false;
               newNode.SelectAction = TreeNodeSelectAction.Expand;
               //newNode.NavigateUrl = m.PLink;
               if (GetChildNodeItem(newNode) == true)
                   node.ChildNodes.Add(newNode);
           }
       }
       //else
       //{
       //    trMktMenu.Nodes.Remove(node);
   
       //}
    }

    
     bool GetChildNodeItem(TreeNode node)
    {
        Int16 intItem = Convert.ToInt16(node.Value);
        //  SubMnuItemList subLlist = TreeMenuClass.GetsubMenuItem(intItem);
        // TO DO:: change the thrid parameter module id
        // Right now kept 1 default for Marketing module.
        bool a = false;
        int UserId = Convert.ToInt16(Session["UserId"]);
        EmployeeProperties oEmpProp = new EmployeeProperties();
        oEmpProp = UserMethods.GetEmployeeDetailsByUserId(UserId);
        int AccessId = oEmpProp.AccessId;

        SubMnuItemList subLlist = TreeMenuClass.GetsubMenuItem(node.Value.ToString(),AccessId,1);
        if (subLlist.Count > 0)
        {
            foreach (SubMnuItem s in subLlist)
            {
                TreeNode newNode = new TreeNode(s.Name, s.Id);
                newNode.PopulateOnDemand = false;
                newNode.SelectAction = TreeNodeSelectAction.Expand;
                newNode.NavigateUrl = s.PLink;
                // newNode.Text = "<a href='../../" + s.PLink + "' class='ChildNodeStyle'>" + s.Name + "</a>";
                node.ChildNodes.Add(newNode);
                GetChildNodeItem(newNode);
            }

            a= true;
        }
        else
        {
            //trMktMenu.Nodes.Remove(node);
            //RemoveNode(node);
            a= false;
        }
        return a;
    }

    **********************************************************/
    // Comments End
    //private void RemoveNode(TreeNode node)
    //{
    //    //throw new Exception("The method or operation is not implemented.");
    //    trMktMenu.Nodes.Remove(node);
    //}    
    #endregion
}
