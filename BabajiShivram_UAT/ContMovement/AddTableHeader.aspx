<%@ Page Title="Add Table Columns" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddTableHeader.aspx.cs"
    Inherits="ContMovement_AddTableHeader" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
    </asp:ScriptManager>

    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlAddShippingField" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../Images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upnlAddShippingField" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="vsRequired" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="vgRequired" CssClass="errorMsg" />
            </div>
            <div class="clear">
            </div>
            <fieldset class="fieldset-AutoWidth">
                <legend>Letter Tables</legend>
                <div class="clear">
                </div>
                <table class="table" border="0" style="width: 100%">
                    <tr>
                        <td>Shipping Master</td>
                        <td>
                            <asp:Label ID="lblShippingName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Letter Name</td>
                        <td>
                            <asp:Label ID="lblLetterName" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Field Name</td>
                        <td>
                            <asp:Label ID="lblFieldName" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset>
                <legend>Table Column Detail</legend>
                <table class="table" border="0" style="width: 100%">
                    <tr>
                        <td>Table Header
                            <asp:RequiredFieldValidator ID="rfvHeader" runat="server" ControlToValidate="txtTableHeader" Display="Dynamic" SetFocusOnError="true"
                                ErrorMessage="Please enter table header." Text="*" ForeColor="Red" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTableHeader" runat="server" Width="350px" TabIndex="1"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Include any Babaji Column?
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBSColumn" runat="server" DataSourceID="DataSourceBSFieldMaster" DataTextField="FieldName" DataValueField="lid"
                                Width="350px" AppendDataBoundItems="true" TabIndex="2">
                                <asp:ListItem Value="0" Text="-Select-" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Data Type</td>
                        <td>
                            <asp:DropDownList ID="ddlDataType" runat="server" Width="350px" TabIndex="3">
                                <asp:ListItem Value="1" Selected="True" Text="Alphanumeric"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Date"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" TabIndex="4" ValidationGroup="vgRequired" />
                            &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" TabIndex="5" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:GridView ID="gvHeader" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr"
                    DataKeyNames="lid" OnRowCommand="gvHeader_RowCommand" AllowPaging="True" AllowSorting="True" CssClass="table" PageSize="20"
                    PagerSettings-Position="Bottom" DataSourceID="SqlDataSourceHeader">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="HeaderTitle" HeaderText="Table Header" />
                        <asp:BoundField DataField="BSFieldName" HeaderText="Babaji Column" />
                        <asp:BoundField DataField="DataTypeName" HeaderText="Data Type" />
                        <asp:TemplateField HeaderText="Delete">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnDeleteField" runat="server" Text="Delete" OnClientClick="return confirm('Are you sure to delete the header?');" CommandName="Delete"
                                    CommandArgument='<%#Eval("lid") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div>
                    <asp:SqlDataSource ID="DataSourceShippingMaster" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="GetShippingMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="DataSourceBSFieldMaster" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="CM_GetFieldMaster" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlDataSourceHeader" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="CM_GetLetterTables_FieldId" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:SessionParameter Name="FieldId" SessionField="FieldId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

