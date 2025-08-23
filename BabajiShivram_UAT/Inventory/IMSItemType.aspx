<%@ Page Title="Inventory Category" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="IMSItemType.aspx.cs" Inherits="Inventory_IMSItemType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset>
        <legend>Item Category</legend>
        <div class="clear" style="text-align: center;">
            <h2>
                <asp:Label ID="lblerror" runat="server"  ForeColor="green"></asp:Label></h2>
        </div>
        <asp:Button ID="btnAddCategory" runat="server" Text="Add Category" OnClick="btnAddCategory_Click"
            ValidationGroup="Required" />
        <br />
        <br />
        <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
            <tr>
                <td>
                    Category
                    <asp:RequiredFieldValidator ID="RequiredFielCategory" runat="server" ControlToValidate="txtCategory"
                        Text="*" ErrorMessage="*" ValidationGroup="Required" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtCategory" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <legend>Item Category Details</legend>
        <asp:GridView ID="GrvIMSCategory" runat="server" AutoGenerateColumns="False"
            DataSourceID="DataSorceCategory" DataKeyNames="lid" Width="50%" PagerStyle-CssClass="pgr"
            CssClass="table" CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="sName" HeaderText="Category Name" SortExpression="sName" />
                <asp:TemplateField HeaderText="Remove" Visible="false">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgDeleteConsignee" runat="server" ImageUrl="~/images/delete-icon.png"
                            ToolTip="Remove Item" CommandName="Delete" OnClientClick="return confirm('Sure to Remove Item?');" />
                        <%--<asp:LinkButton ID="lnkDelete" Text="Remove" runat="server" CommandName="Delete"
                                                    OnClientClick="return confirm('Sure to Remove Consignee?');"></asp:LinkButton>--%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </fieldset>
    <asp:SqlDataSource ID="DataSorceCategory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
        SelectCommand="IMS_GetCategoryMS" SelectCommandType="StoredProcedure">
    </asp:SqlDataSource>
</asp:Content>
