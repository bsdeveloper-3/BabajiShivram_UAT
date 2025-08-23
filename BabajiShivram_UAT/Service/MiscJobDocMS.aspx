<%@ Page Title="Misc Doc Master" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="MiscJobDocMS.aspx.cs" Inherits="Service_MiscJobDocMS" %>

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
            <legend>Misc Document Master</legend>
                <asp:GridView ID="gvchecklistDocument" runat="server" AllowPaging="true" CssClass="table"
                    PagerStyle-CssClass="pgr" OnPageIndexChanging="gvchecklistDocument_PageIndexChanging"
                    AllowSorting="false" ShowFooter="true" AutoGenerateColumns="false" DataKeyNames="lid"
                    OnRowCommand="gvchecklistDocument_RowCommand" OnRowUpdating="gvchecklistDocument_RowUpdating"
                    OnRowDeleting="gvchecklistDocument_RowDeleting" OnRowEditing="gvchecklistDocument_RowEditing"
                    OnRowCancelingEdit="gvchecklistDocument_RowCancelingEdit" Width="100%" PageSize="20"
                    PagerSettings-Position="TopAndBottom">
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
                        <asp:TemplateField HeaderText="Document Name" SortExpression="DocumentName">
                            <ItemTemplate>
                                <asp:Label ID="lblDocumentName" runat="server" Text='<%#Eval("DocumentName") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDocumentName" runat="server" Text='<%#Eval("DocumentName") %>'></asp:TextBox>
                                <span style="color: Red;">*</span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtDocumentNamefooter" runat="server" Text='<%#Eval("DocumentName") %>' TabIndex="1"></asp:TextBox>
                                <span style="color: Red;">*</span>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Module">
                            <ItemTemplate>
                                <asp:Label ID="lblModuleName" runat="server" Text='<%#Eval("ModuleName") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblEdtModuleName"  runat="server" Text='<%#Eval("ModuleName") %>'></asp:Label>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddlModule" runat="server" TabIndex="4">
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Marine" Value="30"></asp:ListItem>
                                    <asp:ListItem Text="Warehouse" Value="40"></asp:ListItem>
                                    <asp:ListItem Text="Essential Certificate" Value="35"></asp:ListItem>
                                    <asp:ListItem Text="Equipment Hire" Value="45"></asp:ListItem>
                                    <asp:ListItem Text="Public Notice" Value="50"></asp:ListItem>
                                    <asp:ListItem Text="Project" Value="55"></asp:ListItem>
                                </asp:DropDownList>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remark">
                            <ItemTemplate>
                                <asp:Label ID="lblsRemarks" runat="server" Text='<%#Eval("sRemarks") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtsRemarks"  runat="server" Text='<%#Eval("sRemarks") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtsRemarksfooter" runat="server" Text='<%#Eval("sRemarks") %>' TabIndex="3"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Edit/Delete">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server"
                                    Text="Edit" Font-Underline="true"></asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="lnkDelete" CommandName="Delete" ToolTip="Delete" Width="22" runat="server"
                                    Text="Delete" Font-Underline="true" OnClientClick="return confirm('Sure to delete?');"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" Width="39" runat="server"
                                    Text="Update" Font-Underline="true"></asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" Width="22" runat="server"
                                    Text="Cancel" Font-Underline="true"></asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lnkAdd" CommandName="Insert" ToolTip="Add" Width="22" runat="server"
                                    Text="Add" Font-Underline="true" TabIndex="4"></asp:LinkButton>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    
</asp:Content>
