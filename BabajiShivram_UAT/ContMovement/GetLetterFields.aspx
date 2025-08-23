<%@ Page Title="Letter Fields" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="GetLetterFields.aspx.cs" Inherits="ContMovement_GetLetterFields" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
    </asp:ScriptManager>

    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlShippingFields" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../Images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upnlShippingFields" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div class="clear">
            </div>
            <fieldset class="fieldset-AutoWidth">
                <legend>Letter Fields</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                        </div>
                        <div class="fleft" style="margin-left: 2px; padding-top: 3px;">
                            <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                                <asp:Image ID="imgExportToExcel" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>
                            &nbsp;&nbsp;
                        </div>
                        <asp:Button ID="btnAddNewField" runat="server" OnClick="btnAddNewField_Click" Text="Add New Field" Style="float: left" />
                    </asp:Panel>
                </div>
                <div class="clear">
                </div>
                <asp:GridView ID="gvFields" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr"
                    DataKeyNames="lid" OnRowCommand="gvFields_RowCommand" AllowPaging="True" AllowSorting="True" CssClass="table" PageSize="20"
                    PagerSettings-Position="TopAndBottom" DataSourceID="SqlDataSourceFields" OnPreRender="gvFields_PreRender" OnRowDataBound="gvFields_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Table Headers">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnAddColumns" runat="server" CommandName="AddColumn" Text="Add" ToolTip="Add table headers"
                                    CommandArgument='<%#Eval("lid") %>' Font-Underline="true"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ShippingName" HeaderText="Shipping Name" />
                        <asp:BoundField DataField="LetterName" HeaderText="Letter Name" />
                        <asp:BoundField DataField="FieldName" HeaderText="Field Name" />
                        <asp:BoundField DataField="lTypeName" HeaderText="Data Type" />
                        <asp:BoundField DataField="Structure" HeaderText="Structure" />
                        <asp:TemplateField HeaderText="Delete">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnDeleteField" runat="server" Text="Delete" OnClientClick="return confirm('Are you sure to delete the field?');" CommandName="Delete"
                                    CommandArgument='<%#Eval("lid") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="SqlDataSourceFields" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CM_GetLetterFields" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>


