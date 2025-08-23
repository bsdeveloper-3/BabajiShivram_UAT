<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SchemeType.aspx.cs" Inherits="SchemeType"
    MasterPageFile="~/MasterPage.master" Title="Scheme/Licence Type Setup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist"
            runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    
        <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional"
            RenderMode="Inline">
            <ContentTemplate>
                <div align="center">
                    <asp:Label ID="lblresult" runat="server" EnableViewState="false"></asp:Label>
                </div>
                <div class="clear"></div>
                <fieldset>
                <legend>Scheme Type Setup</legend>
                <asp:GridView ID="gvSchemeType" runat="server" AllowPaging="true" OnPageIndexChanging="gvSchemeType_PageIndexChanging"
                    AllowSorting="true" CssClass="table" PagerStyle-CssClass="pgr" PageSize="20"
                    PagerSettings-Position="TopAndBottom" ShowFooter="true" AutoGenerateColumns="false"
                    DataKeyNames="lid" OnRowCommand="gvSchemeType_RowCommand" OnRowUpdating="gvSchemeType_RowUpdating"
                    OnRowDeleting="gvSchemeType_RowDeleting" Width="100%" OnRowEditing="gvSchemeType_RowEditing"
                    OnRowCancelingEdit="gvSchemeType_RowCancelingEdit">
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
                        <asp:TemplateField HeaderText="Scheme/Licence Type">
                            <ItemTemplate>
                                <asp:Label ID="lblSchemeType" runat="server" Text='<%#Eval("SchemeType") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSchemeType" runat="server" Text='<%#Eval("SchemeType") %>'></asp:TextBox>
                                <span style="color: Red;">*</span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtSchemeTypefooter" runat="server" Text='<%#Eval("SchemeType") %>' TabIndex="1"></asp:TextBox>
                                <span style="color: Red;">*</span>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remark">
                            <ItemTemplate>
                                <asp:Label ID="lblsRemarks" runat="server" Text='<%#Eval("sRemarks") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtsRemarks" runat="server" Text='<%#Eval("sRemarks") %>'></asp:TextBox>
                                <span style="color: Red;">*</span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtsRemarksfooter" runat="server" Text='<%#Eval("sRemarks") %>' TabIndex="2"></asp:TextBox>
                                <span style="color: Red;">*</span>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit/Delete">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server"
                                    Text="Edit" Font-Underline="true"></asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="lnkDelete" CommandName="Delete" ToolTip="Delete" Width="22" OnClientClick="return confirm('Sure to delete?');"
                                    runat="server" Text="Delete" Font-Underline="true"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" Width="39" runat="server"
                                    Text="Update" Font-Underline="true"></asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" Width="22" runat="server"
                                    Text="Cancel" Font-Underline="true"></asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lnkAdd" CommandName="Insert" ToolTip="Add" Width="22" runat="server"
                                    Text="Add" Font-Underline="true" TabIndex="3"></asp:LinkButton>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    
</asp:Content>
