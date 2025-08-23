<%@ Page Title="Inventory Item" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="IMSItemDetails.aspx.cs" Inherits="Inventory_IMSItemDetails" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <div id="HdnCodeForDateControl">
        <asp:TextBox ID="txtNewDate" runat="server" Visible="false"></asp:TextBox>
        <AjaxToolkit:CalendarExtender ID="CalendarExtender11" runat="server" 
            TargetControlID="txtNewDate">
        </AjaxToolkit:CalendarExtender>
    </div>
    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional"
        RenderMode="Inline">
        <ContentTemplate>
        <fieldset>
        <legend>Item Details</legend>
        <div class="clear" style="text-align: center;">
            <h2>
                <asp:Label ID="lblerror" runat="server" ForeColor="green" EnableViewState="false"></asp:Label></h2>
        </div>
        <asp:Button ID="btnSubmit" runat="server" Text="Add Item Details" OnClick="btnSubmit_Click"
            ValidationGroup="Required" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
            <tr>
                <td>
                    Category Name
                    <asp:RequiredFieldValidator ID="RFVCategory" runat="server" ControlToValidate="ddlCategory"
                        Text="*" ErrorMessage="*" ValidationGroup="Required"
                        SetFocusOnError="true" Display="Dynamic" InitialValue="0"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    Item Type
                    <asp:RequiredFieldValidator ID="RFVItem" runat="server" ControlToValidate="txtItemType"
                        Text="*" ErrorMessage="*" ValidationGroup="Required"
                        SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtItemType" runat="server"></asp:TextBox>
                </td>
            </tr>

        </table>
    </fieldset>

        <fieldset>
        <legend>Item Details </legend>
        <asp:GridView ID="GrvIMSItemDetails" runat="server" AutoGenerateColumns="False"
            DataKeyNames="ItemID" PagerStyle-CssClass="pgr" CssClass="table" PagerSettings-Position="TopAndBottom"
            CellPadding="4" AllowPaging="True" PageSize="20" AlternatingRowStyle-CssClass="alt"
            OnRowEditing="GrvIMSItemDetails_RowEditing" OnRowCancelingEdit="GrvIMSItemDetails_RowCancelingEdit"
            OnRowUpdating="GrvIMSItemDetails_RowUpdating" OnPageIndexChanging="GrvIMSItemDetails_PageIndexChanging"
            OnPreRender="GrvIMSItemDetails_PreRender" >
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="CategoryName" HeaderText="Category" SortExpression="CategoryName"
                    ReadOnly="true" />
                <asp:BoundField DataField="ItemName" HeaderText="Item Type" SortExpression="ItemName"
                    ReadOnly="true" />
                <asp:TemplateField HeaderText="BAL Quantity">
                    <ItemTemplate>
                        <asp:Label ID="lblQuantity" runat="server" Text='<%#Eval("BalQuntity") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="New Quantity">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtNewQuantity" runat="server" MaxLength="10" Width="50px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldNewQuantity" runat="server" ControlToValidate="txtNewQuantity"
                            Text="*" ErrorMessage="*" ValidationGroup="Required1"
                            SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>

                        <asp:RegularExpressionValidator ID="revNewQuantity" runat="server"
                            ErrorMessage="Invalid Input" ControlToValidate="txtNewQuantity" ValidationExpression="^[-+]?\d*$"
                            SetFocusOnError="true" ValidationGroup="Required1"></asp:RegularExpressionValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Vendor Name">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtVendorName" runat="server" Width="120px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RrvVendorName" runat="server" ControlToValidate="txtVendorName"
                            Text="*" ErrorMessage="*" ValidationGroup="Required1"
                            SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Bill No">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtBillNo" runat="server" Width="60px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RrvBillNo" runat="server" ControlToValidate="txtBillNo"
                            Text="*" ErrorMessage="*" ValidationGroup="Required1"
                            SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Bill Date">
                    <EditItemTemplate>
                    <asp:TextBox ID="txtBillDate" runat="server" CssClass="InputTextBox" Width="65px"></asp:TextBox>

                    <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" EnableViewState="False"
                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtBillDate"></AjaxToolkit:CalendarExtender>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Bill Amount">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtBillAmount" runat="server" Width="50px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RrvBillAmount" runat="server" ControlToValidate="txtBillAmount"
                            Text="*" ErrorMessage="*" ValidationGroup="Required1"
                            SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ADD Quantity">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="ADD" Width="22" runat="server"
                            Text="ADD" Font-Underline="true"></asp:LinkButton>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" Width="45" runat="server"
                            Text="Update" Font-Underline="true" ValidationGroup="Required1"></asp:LinkButton>
                        <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" Width="39" CausesValidation="false"
                            runat="server" Text="Cancel" Font-Underline="true"></asp:LinkButton>
                    </EditItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerTemplate>
                <asp:GridViewPager runat="server" />
            </PagerTemplate>
        </asp:GridView>

        <%--<asp:SqlDataSource ID="DataSorceItemDetails" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="IMS_getItemDetails" SelectCommandType="StoredProcedure"></asp:SqlDataSource>--%>
    </fieldset>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
