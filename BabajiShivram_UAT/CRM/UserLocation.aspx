<%@ Page Title="Location Wise Heads" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="UserLocation.aspx.cs" Inherits="CRM_UserLocation" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
    </asp:ScriptManager>

    <asp:UpdatePanel ID="upJobDetail" runat="server">
        <ContentTemplate>
            <div>
                <div align="center">
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                    <asp:ValidationSummary ID="vsRequired" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="vgRequired" CssClass="errorMsg" EnableViewState="false" />
                </div>
                <fieldset>
                    <legend>Add Location Head</legend>
                    <div class="m clear">
                        <asp:Button ID="btnSubmit" Text="Save" OnClick="btnSubmit_Click" runat="server" ValidationGroup="vgRequired" TabIndex="5" />
                        <asp:Button ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" runat="server" CausesValidation="false" TabIndex="6" />
                    </div>
                    <table border="0" cellpadding="0" cellspacing="0" width="70%" bgcolor="white">
                        <tr>
                            <td>User Name
                                <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ControlToValidate="ddlService" Display="Dynamic" SetFocusOnError="true"
                                    Text="*" ErrorMessage="Please select user name" ValidationGroup="vgRequired" InitialValue="0"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSalesPerson" runat="server" DataSourceID="DataSourceSalesPerson" DataTextField="sName"
                                    DataValueField="lid" AppendDataBoundItems="true" TabIndex="1" Width="250px">
                                    <asp:ListItem Text="- Select User Name -" Value="0" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>Service                                       
                                <asp:RequiredFieldValidator ID="rfvService" runat="server" ControlToValidate="ddlService" Display="Dynamic" SetFocusOnError="true"
                                    Text="*" ErrorMessage="Please select service" ValidationGroup="vgRequired" InitialValue="0"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlService" runat="server" DataSourceID="DataSourceService" DataTextField="sName" DataValueField="ServicesId"
                                    AppendDataBoundItems="true" TabIndex="2" Width="200px">
                                    <asp:ListItem Value="0" Text="-Select-" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Service Location                                       
                                <asp:RequiredFieldValidator ID="rfvLocation" runat="server" ControlToValidate="ddlLocation" Display="Dynamic" SetFocusOnError="true"
                                    Text="*" ErrorMessage="Please select service location" ValidationGroup="vgRequired" InitialValue="0"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlLocation" runat="server" DataSourceID="DataSourceServiceLocation" DataTextField="BranchName" DataValueField="lid"
                                    AppendDataBoundItems="true" TabIndex="3" Width="50%">
                                    <asp:ListItem Value="0" Text="-Select-" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>Location Wise Head &nbsp;
                        <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
                            <asp:Image ID="imgExcel" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                        </asp:LinkButton>
                    </legend>
                    <div class="clear">
                        <asp:Panel ID="pnlFilter" runat="server">
                            <div class="fleft">
                                <uc1:DataFilter ID="DataFilter1" runat="server" />
                            </div>
                        </asp:Panel>
                    </div>
                    <asp:GridView ID="gvLocationHeads" runat="server" DataSourceID="DataSourceHeadLocation" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr"
                        DataKeyNames="lid" OnRowCommand="gvLocationHeads_RowCommand" AllowPaging="True" AllowSorting="True" CssClass="table" PageSize="20"
                        PagerSettings-Position="TopAndBottom">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Service" HeaderText="Service" ReadOnly="true" />
                            <asp:BoundField DataField="ServiceLocation" HeaderText="Service Location" ReadOnly="true" />
                            <asp:BoundField DataField="LocationHead" HeaderText="Head" SortExpression="LocationHead" ReadOnly="true" />
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDelete" runat="server" ToolTip="Delete location head." Text="Delete"
                                        CommandName="deleterow" CommandArgument='<%#Eval("lid")%>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerTemplate>
                            <asp:GridViewPager runat="server" />
                        </PagerTemplate>
                    </asp:GridView>
                </fieldset>
                <div id="SqlDataSources">
                    <asp:SqlDataSource ID="DataSourceSalesPerson" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                        SelectCommand="CRM_GetAllUsers" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="DataSourceService" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="BS_GetServicesMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="DataSourceServiceLocation" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="GetAllBranch" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="DataSourceHeadLocation" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="CRM_GetUserLocation" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
