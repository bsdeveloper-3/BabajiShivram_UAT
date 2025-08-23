<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DocumentMaster.aspx.cs" EnableEventValidation="false"
     Inherits="Master_DocumentMaster"  Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    
    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional" 
        RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblresult" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div class="clear"></div>
            <fieldset>
                <legend>Document Master</legend>

                <div class="fright">
                    <asp:LinkButton ID="lnkexport" runat="server"  OnClick="lnkexport_Click" > 
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                </div>
                <div class="clear"></div>
                <div>
                <asp:GridView ID="gvDocumentMaster" runat="server" CssClass="table" ShowFooter="true" PagerStyle-CssClass="pgr"
                    AutoGenerateColumns="false" AllowPaging="true" PageSize="20" PagerSettings-Position="TopAndBottom"
                    OnPageIndexChanging="gvDocumentMaster_PageIndexChanging" DataKeyNames="lid" OnRowCommand="gvDocumentMaster_RowCommand"
                    OnRowUpdating="gvDocumentMaster_RowUpdating" OnRowDeleting="gvDocumentMaster_RowDeleting" OnRowEditing="gvDocumentMaster_RowEditing"
                    Width="100%" OnRowCancelingEdit="gvDocumentMaster_RowCancelingEdit">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="FieldId" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblId" runat="server" Text='<%#Eval("lid") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtId" runat="server" Text='<%#Eval("lid") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtIdFooter" runat="server" Text='<%#Eval("lid") %>'></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Document Name">
                            <ItemTemplate>
                                <asp:Label ID="lblDoc_Name" runat="server" Text='<%#Eval("SName") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDoc_Name" runat="server" Text='<%#Eval("SName") %>' MaxLength="50"></asp:TextBox>
                                <span style="color: Red">*</span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtDoc_Namefooter" runat="server" Text='<%#Eval("SName") %>'
                                    MaxLength="50" TabIndex="1"></asp:TextBox>
                                <span style="color: Red">*</span>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Operation Type">
                            <ItemTemplate>
                                <asp:Label ID="lblFieldType" runat="server" Text='<%#Eval("OperationType") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddFieldType" runat="server" >
                                    <asp:ListItem Text="Import" Value="1" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Export" Value="5"></asp:ListItem>
                                </asp:DropDownList>
                                <span style="color: Red">*</span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddFieldTypeFooter" runat="server" TabIndex="2">
                                    <asp:ListItem Text="--Select--" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Import" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Export" Value="5"></asp:ListItem>
                                </asp:DropDownList>
                                <span style="color: Red">*</span>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Operation Mode">
                            <ItemTemplate>
                                <asp:Label ID="lblOperationMode" runat="server" Text='<%#Eval("OperationMode") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlMode" runat="server" >
                                    <%--<asp:ListItem Text="--Select--" Value="0" Selected="True"></asp:ListItem>--%>
                                    <asp:ListItem Text="Sea" Value="1" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Air" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Both" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                                <%--<span style="color: Red">*</span>--%>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddModeFooter" runat="server" TabIndex="2">
                                    <asp:ListItem Text="--Select--" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Sea" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Air" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Both" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                                <span style="color: Red">*</span>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Compulsary">
                            <ItemTemplate>
                                <asp:Label ID="lblcOMP" runat="server" Text='<%#Eval("Compulsary") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddComp" runat="server" SelectedValue='<%#Eval("Comp") %>'>
                                    <asp:ListItem Text="Yes" Value="1" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <span style="color: Red">*</span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddCompFooter" runat="server" TabIndex="2">
                                    <asp:ListItem Text="--Select--" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
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
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

