<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DataFilter.ascx.cs" Inherits="DynamicData_Content_DataFilter" %>
<%--<asp:UpdatePanel ID="updatePanel" runat="server">
    <ContentTemplate>--%>
    <div>
        <asp:Panel ID="pnlTools" runat="server">
        </asp:Panel>
        <asp:Panel ID="pnlNewFilter" runat="server">
        </asp:Panel>
        <asp:Panel ID="pnlToolbar" runat="server">
            <asp:Button ID="btnAddNewFilter" runat="server" Text="Add Filter" CssClass="buttons"
                OnClick="btnAddNewFilter_Click" />
            <asp:Button ID="btnAndNewFilter" runat="server" CssClass="buttons" Text="AND" Visible="False"
                OnClick="btnAndNewFilter_Click" />
            <asp:Button ID="btnOrNewFilter" runat="server" CssClass="buttons" Text="OR" Visible="False"
                OnClick="btnOrNewFilter_Click" /></asp:Panel>
        <asp:Panel ID="pnlInfo" runat="server">
            <asp:Label ID="lblInfo" runat="server" CssClass="errorMsg" EnableViewState="false"></asp:Label>
        </asp:Panel>
    </div>        
    <%--</ContentTemplate>
</asp:UpdatePanel>--%>
