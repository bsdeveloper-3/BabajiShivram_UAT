<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CFSMaster.aspx.cs" MasterPageFile="~/MasterPage.master"
    Inherits="CFSMaster" Title="CFS Setup" %>

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
        <legend>CFS Detail</legend>
                <asp:GridView ID="gvCFSMaster" runat="server" CssClass="table" AutoGenerateColumns="false"
                    AllowPaging="true" OnPageIndexChanging="gvCFSMaster_PageIndexChanging" PageSize="20"
                    PagerSettings-Position="TopAndBottom" PagerStyle-CssClass="pgr" ShowFooter="true" AllowSorting="true"
                    OnRowDeleting="gvCFSMaster_RowDeleting" OnRowEditing="gvCFSMaster_RowEditing" OnRowCancelingEdit="gvCFSMaster_RowCancelingEdit"
                    OnRowCommand="gvCFSMaster_RowCommand" OnRowUpdating="gvCFSMaster_RowUpdating" OnRowDataBound="gvCFSMaster_RowDataBound" >
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
                        <asp:TemplateField HeaderText="CFS Name">
                            <ItemTemplate>
                                <asp:Label ID="lblCFSMName" runat="server" Text='<%# Eval("sName") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txteCFSName" runat="server" Text='<%# Eval("sName") %>' MaxLength="100" />
                                <span style="color: Red;">*</span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtCFSName" runat="server" Text='<%#Eval("sName") %>' MaxLength="100" />
                                <span style="color: Red;">*</span>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CFS User">
                            <ItemTemplate>
                                <asp:Label ID="lblCFSUser" runat="server" Text='<%# Eval("CFSUserName") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddCFSUser" runat="server">
                                </asp:DropDownList>
                                <span style="color: Red;">*</span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddUserFooter" runat="server">
                                </asp:DropDownList>
                                <span style="color: Red;">*</span>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remark">
                            <ItemTemplate>
                                <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                    <asp:Label ID="lblRemark" runat="server" Text='<%#Eval("Remark") %>' />
                                </div>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txteRemark" runat="server" Text='<%# Eval("Remark") %>' MaxLength="200"
                                    TextMode="MultiLine" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtRemark" runat="server" Text='<%#Eval("Remark") %>' MaxLength="200"
                                    TextMode="MultiLine" TabIndex="2" />
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
                                    CommandName="Insert" TabIndex="3"/>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                    
                </asp:GridView>
            </fieldset>    
            </ContentTemplate>
        </asp:UpdatePanel>
    
</asp:Content>
