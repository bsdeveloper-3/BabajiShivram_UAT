<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IncoTerms.aspx.cs" Inherits="IncoTerms"
    MasterPageFile="~/MasterPage.master" Title="Incoterms of Shipment Setup" %>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="content1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upDivision" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    
        <asp:UpdatePanel ID="upDivision" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <div align="center">
                    <asp:Label ID="lblresult" runat="server"></asp:Label>
                </div>
                <div class="clear"></div>
                <fieldset>
            <legend>Inco Terms Setup</legend>
                <asp:GridView ID="gvInco" runat="server" AllowPaging="true" OnPageIndexChanging="gvInco_PageIndexChanging"
                    CssClass="table" PagerStyle-CssClass="pgr" PageSize="20" AllowSorting="true"
                    PagerSettings-Position="TopAndBottom" AutoGenerateColumns="false" DataKeyNames="lid"
                    OnRowCommand="gvInco_RowCommand" OnRowUpdating="gvInco_RowUpdating" OnRowDeleting="gvInco_RowDeleting"
                    OnRowEditing="gvInco_RowEditing" OnRowCancelingEdit="gvInco_RowCancelingEdit"
                    ShowFooter="true" Width="100%">
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
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:Label ID="lblsName" runat="server" Text='<%#Eval("sName") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtsName" runat="server" Text='<%#Eval("sName") %>'></asp:TextBox>
                                <span style="color: Red;">*</span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtsNamefooter" runat="server" Text='<%#Eval("sName") %>' TabIndex="1"></asp:TextBox>
                                <span style="color: Red;">*</span>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit/Delete">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server"
                                    Text="Edit" Font-Underline="true"></asp:LinkButton>&nbsp;
                                &nbsp;<asp:LinkButton ID="lnkDelete" CommandName="Delete" ToolTip="Delete" Width="22" runat="server"
                                    Text="Delete" Font-Underline="true" OnClientClick="return confirm('Sure to delete?');"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" Width="39" runat="server"
                                    Text="Update" Font-Underline="true"></asp:LinkButton>&nbsp;
                                &nbsp;<asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" Width="22" runat="server"
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
