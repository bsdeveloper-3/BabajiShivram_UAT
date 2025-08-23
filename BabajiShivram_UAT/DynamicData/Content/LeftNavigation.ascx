<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LeftNavigation.ascx.cs"
    Inherits="DynamicData_Content_LeftNavigation" %>
<asp:TreeView ID="trMktMenu" runat="server" ShowLines="false" NodeWrap="true" PathSeparator=":"
    NodeIndent="3" OnSelectedNodeChanged="trMktMenu_SelectedNodeChanged" ExpandDepth="1" 
    ShowExpandCollapse="true" CssClass="dashboard" TabIndex="50">
    <%--OnUnload="trMktMenu_Unload" --%>
    <%--OnTreeNodePopulate="trMktMenu_TreeNodePopulate"--%>
    <Nodes>
        <asp:TreeNode Text="Dashboard" Value="Menu" NavigateUrl="~/Dashboard.aspx" ImageUrl="~/images/dashboard.png">
        </asp:TreeNode>
        <%--PopulateOnDemand="True"--%>
    </Nodes>
    <ParentNodeStyle />
    <%--<RootNodeStyle Font-Bold="True" /> --%>
    <SelectedNodeStyle CssClass="selected" />
</asp:TreeView>
