<%@ Page Title="Add Shipping Letter" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ShippingLetter.aspx.cs"
    Inherits="ContMovement_ShippingLetter" ValidateRequest="false" Culture="en-GB" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
    </asp:ScriptManager>

    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlAddShippingLetter" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../Images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upnlAddShippingLetter" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <fieldset class="fieldset-AutoWidth">
                <legend>Add Letters</legend>

                <div align="center">
                    <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                    <asp:ValidationSummary ID="vsShippingLetter" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="vgRequired" CssClass="errorMsg" />
                </div>
                <div class="clear">
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="vgRequired" TabIndex="3" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" TabIndex="4" />
                </div>
                <table class="table" border="0" style="width: 100%">
                    <tr>
                        <td>Shipping Master</td>
                        <td>
                            <asp:DropDownList ID="ddlShippingMS" runat="server" AppendDataBoundItems="true" DataSourceID="DataSourceShippingMaster"
                                DataTextField="CompName" DataValueField="lid" TabIndex="1" Width="360px">
                                <asp:ListItem Selected="True" Value="0" Text="-Select-"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Letter Name</td>
                        <td>
                            <asp:TextBox ID="txtLetterName" runat="server" TabIndex="2" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <div>
                    <asp:SqlDataSource ID="DataSourceShippingMaster" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="GetShippingMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                </div>
            </fieldset>
            <fieldset class="fieldset-AutoWidth">
                <legend>Shipping Letters</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                        </div>
                        <div class="fleft" style="margin-left: 2px; padding-top: 3px;">
                            <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                                <asp:Image ID="imgExportToExcel" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
                <div class="clear"></div>
                <asp:GridView ID="gvLetters" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr"
                    DataKeyNames="lid" OnRowCommand="gvLetters_RowCommand" AllowPaging="True" AllowSorting="True" CssClass="table" PageSize="20"
                    PagerSettings-Position="TopAndBottom" DataSourceID="SqlDataSourceLetters" OnPreRender="gvLetters_PreRender">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ShippingName" HeaderText="Shipping Name" SortExpression="ShippingName" />
                        <asp:BoundField DataField="LetterName" HeaderText="Letter Name" SortExpression="LetterName" />
                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="CreatedDate" />
                        <asp:TemplateField HeaderText="Delete">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnDeleteField" runat="server" Text="Delete" OnClientClick="return confirm('Are you sure to delete the letter?');" CommandName="Delete"
                                    CommandArgument='<%#Eval("lid") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <div>
                    <asp:SqlDataSource ID="SqlDataSourceLetters" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="CM_GetShippingLetters" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

