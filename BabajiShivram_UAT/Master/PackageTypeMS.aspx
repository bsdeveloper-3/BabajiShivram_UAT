<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PackageTypeMS.aspx.cs" Inherits="PackageTypeMS"
    MasterPageFile="~/MasterPage.master" Title="Package Type Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <div align="center">
                    <asp:Label ID="lblResult" runat="server" EnableViewState="false"></asp:Label>
                </div>
                <div class="clear"></div>
                <fieldset>
                <legend>Package Type Detail</legend>
                <asp:GridView ID="gvPackageMaster" runat="server" CssClass="table" AutoGenerateColumns="false"
                    AllowPaging="true" OnPageIndexChanging="gvPackageMaster_PageIndexChanging" PageSize="20"
                    PagerSettings-Position="TopAndBottom" OnRowDeleting="gvPackageMaster_RowDeleting"
                    OnRowEditing="gvPackageMaster_RowEditing" OnRowCancelingEdit="gvPackageMaster_RowCancelingEdit"
                    OnRowCommand="gvPackageMaster_RowCommand" OnRowUpdating="gvPackageMaster_RowUpdating"
                    ShowFooter="true" AllowSorting="true">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField AccessibleHeaderText="lid" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblLid" runat="server" Text='<%#Eval("lid") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Package Type Name">
                            <ItemTemplate>
                                <asp:Label ID="lblPackageName" runat="server" Text='<%# Eval("sName") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEdtPackageName" runat="server" Text='<%# Eval("sName") %>' MaxLength="50" />
                                <span style="color: Red;">*</span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtPackageName" runat="server" Text='<%#Eval("sName") %>' MaxLength="50" TabIndex="1" />
                                <span style="color: Red;">*</span>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Package Code">
                            <ItemTemplate>
                                <asp:Label ID="lblPackageCode" runat="server" Text='<%# Eval("sCode") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEdtPackageCode" runat="server" Text='<%# Eval("sCode") %>' MaxLength="50" />
                                <span style="color: Red;">*</span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtPackageCode" runat="server" Text='<%#Eval("sCode") %>' MaxLength="50" TabIndex="1" />
                                <span style="color: Red;">*</span>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit/Delete">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnEdit" runat="server" Text="Edit" CommandName="Edit" ToolTip="Edit"
                                    Font-Underline="true" />&nbsp;
                                <asp:LinkButton ID="lbtnDelete" runat="server" Text="Delete" CommandName="Delete"
                                    ToolTip="Delete" OnClientClick="return confirm('Sure to delete?');" Font-Underline="true" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" runat="server"
                                    Text="Update" Font-Underline="true"></asp:LinkButton>&nbsp;
                               <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" runat="server"
                                    Text="Cancel" Font-Underline="true"></asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="btnAdd" runat="server" Text="Add" ToolTip="Add" Font-Underline="true"
                                    CommandName="Insert" TabIndex="2" />
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    
</asp:Content>
