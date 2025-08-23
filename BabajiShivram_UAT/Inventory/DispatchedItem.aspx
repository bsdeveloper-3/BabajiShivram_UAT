<%@ Page Title="Inventory Dispatch" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DispatchedItem.aspx.cs" Inherits="Inventory_DispatchedItem" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upJobDetail" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upJobDetail" runat="server">
        <ContentTemplate>
    <fieldset>
        <legend>Dispatch Item </legend>
        <div class="clear" style="text-align: center;">
            <h2>
                <asp:Label ID="lblError" runat="server" ForeColor="green" EnableViewState="false"></asp:Label></h2>
        </div>

        <asp:Button ID="btnSubmitDispached" runat="server" Text="Submit" OnClick="btnSubmitDispached_Click" ValidationGroup="Required" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
            <tr>
                <td>
                    Employee Name
                    <asp:RequiredFieldValidator ID="RFVEditEmpName" runat="server" ControlToValidate="txtempName"
                        Text="*" ErrorMessage="*" ValidationGroup="Required"
                        SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtempName" runat="server"></asp:TextBox>
                    <div id="divwidthUser" runat="server"></div>
                    <AjaxToolkit:AutoCompleteExtender ID="UserExtender" runat="server" BehaviorID="divwidthUser"
                        CompletionListCssClass="AutoExtender" CompletionListElementID="divwidthUser"
                        CompletionListHighlightedItemCssClass="AutoExtenderHighlight" CompletionListItemCssClass="AutoExtenderList"
                        ContextKey="6109" FirstRowSelected="true" MinimumPrefixLength="2" 
                        ServiceMethod="GetUserCompletionList" ServicePath="~/WebService/UserAutoComplete.asmx"
                        TargetControlID="txtempName" UseContextKey="True">
                    </AjaxToolkit:AutoCompleteExtender>
                </td>
                <td>
                    Category
                    <asp:RequiredFieldValidator ID="RefCategory" runat="server" ControlToValidate="ddlCategory"
                        Text="*" Display="Dynamic" ErrorMessage="*" ValidationGroup="Required" SetFocusOnError="true"
                        InitialValue="0" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Item
                    <asp:RequiredFieldValidator ID="RequiredFieldItem" runat="server" ControlToValidate="ddlItem"
                        Text="*" Display="Dynamic" ErrorMessage="*" ValidationGroup="Required" SetFocusOnError="true"
                        InitialValue="0" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:DropDownList ID="ddlItem" runat="server" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlItem_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Label ID="lblremanihg" runat="server" ForeColor="Red">
                    </asp:Label>
                </td>
                <td>
                    Quantity
                    <asp:RequiredFieldValidator ID="RequiredFieldQuantity" runat="server" ControlToValidate="txtQuantity"
                        Text="*" ErrorMessage="*" ValidationGroup="Required"
                        SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="cv" runat="server" ControlToValidate="txtQuantity" Type="Integer"
                        Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="Required" />
                </td>
                <td>
                    <asp:TextBox ID="txtQuantity" runat="server" type="integer"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Department
                    <asp:RequiredFieldValidator ID="RequiredFielddepT" runat="server" ControlToValidate="ddldept"
                        Text="*" Display="Dynamic" ErrorMessage="*" ValidationGroup="Required" SetFocusOnError="true"
                        ForeColor="Red" InitialValue="0"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:DropDownList ID="ddldept" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    Branch
                    <asp:RequiredFieldValidator ID="RequiredFieldBranch" runat="server" ControlToValidate="ddlBranch"
                        Text="*" Display="Dynamic" ErrorMessage="*" InitialValue="0" ValidationGroup="Required"
                        SetFocusOnError="true"
                        ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:DropDownList ID="ddlBranch" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Dispatched ON
                    <AjaxToolkit:CalendarExtender ID="calDispDate" runat="server" Enabled="True" EnableViewState="False"
                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDispatchDate" PopupPosition="BottomRight"
                        TargetControlID="txtDispatchDate">
                    </AjaxToolkit:CalendarExtender>
                </td>
                <td>
                    <asp:TextBox ID="txtDispatchDate" Width="100px" runat="server" placeholder="dd/mm/yyyy" />
                    <asp:Image ID="imgDispatchDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                </td>
                <td></td>
                <td></td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <legend>Dispatch Item Details </legend>
        <div>
            <div class="fleft">
                <uc1:DataFilter ID="DataFilter1" runat="server" />
            </div>
            <div class="fleft">
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:LinkButton ID="lnkIMSXls" runat="server" OnClick="lnkIMSXls_Click">
                    <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" ImageAlign="AbsBottom" />
                </asp:LinkButton>
            </div>
        </div>
        <div class="m clear"></div>
        <asp:GridView ID="GrvDispatched" runat="server" AutoGenerateColumns="False"
            DataSourceID="DataSorceDispatchedDetails" PagerStyle-CssClass="pgr" CssClass="table"
            CellPadding="4" AllowPaging="True" AllowSorting="true" PageSize="10" AlternatingRowStyle-CssClass="alt">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="EmpName" HeaderText="Employee" SortExpression="EmpName" />
                <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category" />
                <asp:BoundField DataField="Item" HeaderText="Item" SortExpression="Item" />
                <asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity" />
                <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="Department" />
                <asp:BoundField DataField="Branch" HeaderText="Branch" SortExpression="Branch" />
                <asp:BoundField DataField="DispatchDate" HeaderText="Dispatch Date" SortExpression="dtdate" DataFormatString="{0: dd/MM/yyyy}"  />
            </Columns>
            <PagerTemplate>
                <asp:GridViewPager runat="server" />
            </PagerTemplate>
        </asp:GridView>

        <asp:SqlDataSource ID="DataSorceDispatchedDetails" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
            SelectCommand="IMS_GetDispatchedDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlCategory" Name="CategoryID" PropertyName="SelectedValue" DefaultValue="0" />
                <asp:ControlParameter ControlID="ddlItem" Name="ItemID" PropertyName="SelectedValue" DefaultValue="0"/>
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
            </SelectParameters>
        </asp:SqlDataSource>

    </fieldset>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
