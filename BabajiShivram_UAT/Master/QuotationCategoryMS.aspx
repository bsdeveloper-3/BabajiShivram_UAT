<%@ Page Title="Quotation Category" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="QuotationCategoryMS.aspx.cs"
    Inherits="Master_QuotationCategoryMS" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />

    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upQuoteCatg" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upQuoteCatg" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblresult" runat="server" EnableViewState="false"></asp:Label>
                  <asp:ValidationSummary ID="vsAddDetail" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="AddDetail" CssClass="errorMsg" />
                  <asp:ValidationSummary ID="vsEditDetail" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="EditDetail" CssClass="errorMsg" />
            </div>
            <fieldset>
                <legend>Quotation Category Setup</legend>
                <div>
                    <asp:GridView ID="gvQuotationCatg" runat="server" CssClass="table" ShowFooter="true" PagerStyle-CssClass="pgr"
                        AllowSorting="true" AutoGenerateColumns="false" AllowPaging="true"
                        OnPageIndexChanging="gvQuotationCatg_PageIndexChanging" PageSize="20" PagerSettings-Position="TopAndBottom"
                        DataKeyNames="lid" OnRowCommand="gvQuotationCatg_RowCommand" OnRowUpdating="gvQuotationCatg_RowUpdating"
                        OnRowDeleting="gvQuotationCatg_RowDeleting" OnRowEditing="gvQuotationCatg_RowEditing" Width="100%"
                        OnRowCancelingEdit="gvQuotationCatg_RowCancelingEdit" OnRowDataBound="gvQuotationCatg_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="lid" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbllid" runat="server" Text='<%#Eval("lid") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtlid" runat="server" Text='<%#Eval("lid") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtlidfooter" runat="server" Text='<%#Eval("lid") %>'></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Category Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblQuotationCatg" runat="server" Text='<%#Eval("sName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtQuotationCatg" runat="server" Text='<%#Eval("sName") %>' Width="90%" TabIndex="1"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvname" runat="server" ControlToValidate="txtQuotationCatg" SetFocusOnError="true" Display="Dynamic"
                                        ErrorMessage="Please Enter Category Name." Text="*" ValidationGroup="EditDetail"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtQuotationCatgfooter" runat="server" Text='<%#Eval("sName") %>' Width="90%" TabIndex="1"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvnamefooter" runat="server" ControlToValidate="txtQuotationCatgfooter" SetFocusOnError="true" Display="Dynamic"
                                        ErrorMessage="Please Enter Category Name." Text="*" ValidationGroup="AddDetail"></asp:RequiredFieldValidator>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Service Type">
                                <ItemTemplate>
                                    <asp:Label ID="lblServiceType" runat="server" Text='<%#Eval("ServicesName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlServiceType" runat="server" AppendDataBoundItems="true" DataSourceID="DataSourceService"
                                        DataTextField="sName" DataValueField="ServicesId" SelectedValue='<%#Eval("ServicesId") %>'>
                                        <asp:ListItem Selected="True" Value="0" Text="-Select-"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvServType" runat="server" ControlToValidate="ddlServiceType" SetFocusOnError="true" Display="Dynamic"
                                        ErrorMessage="Please Select Service Type." Text="*" ValidationGroup="EditDetail" InitialValue="0"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlServiceTypeFooter" runat="server" AppendDataBoundItems="true" DataSourceID="DataSourceService"
                                        DataTextField="sName" DataValueField="ServicesId">
                                        <asp:ListItem Selected="True" Value="0" Text="-Select-"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvServTypefooter" runat="server" ControlToValidate="ddlServiceTypeFooter" SetFocusOnError="true" Display="Dynamic"
                                        ErrorMessage="Please Select Service Type." Text="*" ValidationGroup="AddDetail" InitialValue="0"></asp:RequiredFieldValidator>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit/Delete">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server" Text="Edit" Font-Underline="true"></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="lnkDelete" CommandName="Delete" ToolTip="Delete" Width="22" OnClientClick="return confirm('Sure to delete?');" runat="server" Text="Delete" Font-Underline="true"></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="lnkUpdate" ValidationGroup="EditDetail" CommandName="Update" ToolTip="Update" Width="39" runat="server" Text="Update" TabIndex="2" Font-Underline="true"></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" Width="22" runat="server" TabIndex="3" Text="Cancel" Font-Underline="true"></asp:LinkButton>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:LinkButton ID="lnkAdd" ValidationGroup="AddDetail" CommandName="Insert" ToolTip="Add" Width="22" runat="server" Text="Add" Font-Underline="true" TabIndex="2"></asp:LinkButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div>
                    <asp:SqlDataSource ID="DataSourceService" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="BS_GetServicesMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

