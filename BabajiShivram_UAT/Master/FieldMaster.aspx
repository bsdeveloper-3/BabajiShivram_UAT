<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FieldMaster.aspx.cs" MasterPageFile="~/MasterPage.master"
    Inherits="FieldMaster" Title="Additional Field Setup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />

    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional"
        RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblresult" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div class="clear"></div>
            <fieldset>
                <legend>Additional Field Setup</legend>
                <asp:GridView ID="gvField" runat="server" CssClass="table" ShowFooter="true" PagerStyle-CssClass="pgr"
                    AutoGenerateColumns="false" AllowPaging="true" PageSize="20" PagerSettings-Position="TopAndBottom"
                    OnPageIndexChanging="gvField_PageIndexChanging" DataKeyNames="FieldId" OnRowCommand="gvField_RowCommand"
                    OnRowUpdating="gvField_RowUpdating" OnRowDeleting="gvField_RowDeleting" OnRowEditing="gvField_RowEditing"
                    Width="100%" OnRowCancelingEdit="gvField_RowCancelingEdit">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="FieldId" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblFieldId" runat="server" Text='<%#Eval("FieldId") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtFieldId" runat="server" Text='<%#Eval("FieldId") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtFieldIdFooter" runat="server" Text='<%#Eval("FieldId") %>'></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Field Name">
                            <ItemTemplate>
                                <asp:Label ID="lblField_Name" runat="server" Text='<%#Eval("FieldName") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtField_Name" runat="server" Text='<%#Eval("FieldName") %>' MaxLength="50"></asp:TextBox>
                                <span style="color: Red">*</span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtField_Namefooter" runat="server" Text='<%#Eval("FieldName") %>'
                                    MaxLength="50" TabIndex="1"></asp:TextBox>
                                <span style="color: Red">*</span>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Field Type">
                            <ItemTemplate>
                                <asp:Label ID="lblFieldType" runat="server" Text='<%#Eval("FieldTypeName") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddFieldType" runat="server">
                                    <asp:ListItem Text="Alphanumeric" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Numeric" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Date" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="% Percent" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="Currency" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="CheckBox" Value="6"></asp:ListItem>
                                </asp:DropDownList>
                                <span style="color: Red">*</span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddFieldTypeFooter" runat="server" TabIndex="2">
                                    <asp:ListItem Text="Alphanumeric" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Numeric" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Date" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="% Percent" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="Currency" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="CheckBox" Value="6"></asp:ListItem>
                                </asp:DropDownList>
                                <span style="color: Red">*</span>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Module">
                            <ItemTemplate>
                                <asp:Label ID="lblModuleId" runat="server" Text='<%#Eval("Module") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddModule" runat="server" SelectedValue='<%#Eval("ModuleId") %>'>
                                    <asp:ListItem Text="Import" Value="1" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Export" Value="5"></asp:ListItem>
                                </asp:DropDownList>
                                <span style="color: Red">*</span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddModuleFooter" runat="server" TabIndex="2">
                                    <asp:ListItem Text="Import" Value="1" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Export" Value="5"></asp:ListItem>
                                </asp:DropDownList>
                                <span style="color: Red">*</span>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit/Delete">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server"
                                    Text="Edit" Font-Underline="true"></asp:LinkButton>
                                &nbsp;
                                <asp:LinkButton ID="lnkDelete" CommandName="Delete" ToolTip="Delete" OnClientClick="return confirm('Sure to delete?');"
                                    runat="server" Text="Delete" Font-Underline="true"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" runat="server"
                                    Text="Update" Font-Underline="true"></asp:LinkButton>
                                &nbsp;
                                <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" runat="server"
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
