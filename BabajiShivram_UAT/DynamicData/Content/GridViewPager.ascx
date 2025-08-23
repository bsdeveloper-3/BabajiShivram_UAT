<%@ Control Language="C#" CodeFile="GridViewPager.ascx.cs" Inherits="GridViewPager" %>

<div class="pager">
    <span class="results1">
        <asp:ImageButton AlternateText="First page" ToolTip="First page" ID="ImageButtonFirst" runat="server" ImageUrl="Images/PgFirst.gif" Width="8" Height="9" CommandName="Page" CommandArgument="First" CausesValidation="false"/>
        &nbsp;
        <asp:ImageButton AlternateText="Previous page" ToolTip="Previous page" ID="ImageButtonPrev" runat="server" ImageUrl="Images/PgPrev.gif" Width="5" Height="9" CommandName="Page" CommandArgument="Prev" CausesValidation="false"/>
        &nbsp;
        <asp:Label ID="LabelPage" runat="server" Text="Page " AssociatedControlID="TextBoxPage" />
        <asp:TextBox ID="TextBoxPage" runat="server" Columns="5" AutoPostBack="true" ontextchanged="TextBoxPage_TextChanged" Width="20px" CssClass="droplist" />
        of
        <asp:Label ID="LabelNumberOfPages" runat="server" />
        &nbsp;
        <asp:ImageButton AlternateText="Next page" ToolTip="Next page" ID="ImageButtonNext" runat="server" ImageUrl="Images/PgNext.gif" Width="5" Height="9" CommandName="Page" CommandArgument="Next" CausesValidation="false" />
        &nbsp;
        <asp:ImageButton AlternateText="Last page" ToolTip="Last page" ID="ImageButtonLast" runat="server" ImageUrl="Images/PgLast.gif" Width="8" Height="9" CommandName="Page" CommandArgument="Last" CausesValidation="false" />
    </span>
    <span class="results2">
        <asp:Label ID="LabelRows" runat="server" Text="Results per page:" AssociatedControlID="DropDownListPageSize" />
        <asp:DropDownList ID="DropDownListPageSize" runat="server" AutoPostBack="true" CssClass="droplist" onselectedindexchanged="DropDownListPageSize_SelectedIndexChanged">
            <asp:ListItem Text="10" Value="10" />
            <asp:ListItem Text="20" Value="20" />
            <asp:ListItem Text="40" Value="40" />
            <asp:ListItem Text="80" Value="80" />
            <asp:ListItem Text="Show All" Value="2000" />
        </asp:DropDownList>
    </span>
</div>
