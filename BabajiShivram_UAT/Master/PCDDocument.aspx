<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PCDDocument.aspx.cs" Inherits="PCDDocument" Title="PCA Document Setup" %>

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
    
    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblresult" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div class="clear"></div>
            <fieldset><legend>PCA Document Setup</legend>
                <asp:GridView ID="gvPCDDocument" runat="server" AllowPaging="true" CssClass="table" PagerStyle-CssClass="pgr" 
                    DataKeyNames="lid" Width="100%" PageSize="20" PagerSettings-Position="TopAndBottom" ShowFooter="true" AutoGenerateColumns="false" 
                    OnPageIndexChanging="gvPCDDocument_PageIndexChanging" AllowSorting="true" OnRowCommand="gvPCDDocument_RowCommand" 
                    OnRowUpdating="gvPCDDocument_RowUpdating" OnRowDeleting="gvPCDDocument_RowDeleting" OnRowEditing="gvPCDDocument_RowEditing"
                    OnRowCancelingEdit="gvPCDDocument_RowCancelingEdit">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="lid" Visible="False">
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
                        <asp:TemplateField HeaderText="PCA Document Name">
                            <ItemTemplate>
                                <asp:Label ID="lblPCDDocumentName" runat="server" Text='<%#Eval("sName") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPCDDocumentName"  runat="server" Width="65%" Text='<%#Eval("sName") %>'></asp:TextBox>
                                <span style="color: Red;">*</span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtPCDDocumentNamefooter" runat="server" Text='<%#Eval("sName") %>' TabIndex="1" Width="65%"></asp:TextBox>
                                <span style="color: Red;">*</span>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remark">
                            <ItemTemplate>
                                <asp:Label ID="lblsRemarks" runat="server" Text='<%#Eval("sRemark") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtsRemarks" runat="server" TextMode="MultiLine" Text='<%#Eval("sRemark") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtsRemarksfooter" runat="server" TextMode="MultiLine" Text='<%#Eval("sRemark") %>' TabIndex="2"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit/Delete">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server"
                                    Text="Edit" Font-Underline="true"></asp:LinkButton>
                              &nbsp;  <asp:LinkButton ID="lnkDelete" CommandName="Delete" ToolTip="Delete" Width="22" runat="server"
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
                                    Text="Add" Font-Underline="true" TabIndex="3"></asp:LinkButton>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>
