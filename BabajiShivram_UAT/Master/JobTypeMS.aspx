<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JobTypeMS.aspx.cs" Inherits="JobTypeMS"
    MasterPageFile="~/MasterPage.master" Title="Job Type Setup" %>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="content1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upJobType" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    
        <asp:UpdatePanel ID="upJobType" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <div align="center">
                    <asp:Label ID="lblresult" runat="server" EnableViewState="false"></asp:Label>
                </div>
                <div class="clear"></div>
                <fieldset>
                <legend>Job Type Detail</legend>
                <asp:GridView ID="gvJobType" runat="server" AllowPaging="true" OnPageIndexChanging="gvJobType_PageIndexChanging"
                    CssClass="table" PagerStyle-CssClass="pgr" Width="100%" PagerSettings-Position="TopAndBottom"
                    PageSize="20" AllowSorting="true" ShowFooter="true" AutoGenerateColumns="false"
                    DataKeyNames="lid" OnRowCommand="gvJobType_RowCommand" OnRowUpdating="gvJobType_RowUpdating"
                    OnRowDeleting="gvJobType_RowDeleting" OnRowEditing="gvJobType_RowEditing" OnRowCancelingEdit="gvJobType_RowCancelingEdit">
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
                        <asp:TemplateField HeaderText="Job Type">
                            <ItemTemplate>
                                <asp:Label ID="lblJobType" runat="server" Text='<%#Eval("sName") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtJobType" runat="server" Text='<%#Eval("sName") %>'></asp:TextBox>
                                <span style="color: Red">*</span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtJobTypefooter" runat="server" Text='<%#Eval("sName") %>' TabIndex="1"></asp:TextBox>
                                <span style="color: Red">*</span>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit/Delete">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server"
                                    Text="Edit" Font-Underline="true"></asp:LinkButton>
                               &nbsp; <asp:LinkButton ID="lnkDelete" CommandName="Delete" ToolTip="Delete" Width="22" runat="server"
                                    Text="Delete" Font-Underline="true" OnClientClick="return confirm('Sure to delete?');"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" Width="39" runat="server"
                                    Text="Update" Font-Underline="true"></asp:LinkButton>
                               &nbsp; <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" Width="22" runat="server"
                                    Text="Cancel" Font-Underline="true"></asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lnkAdd" CommandName="Insert" ToolTip="Add" Width="22" runat="server"
                                    Text="Add" Font-Underline="true" TabIndex="2"></asp:LinkButton>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    
</asp:Content>
