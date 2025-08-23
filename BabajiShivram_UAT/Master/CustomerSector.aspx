<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CustomerSector.aspx.cs" Inherits="CustomerSector" Title="Customer Sector Setup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
            <legend>Customer Sector Setup</legend>   
                <div>
                    <asp:GridView ID="gvCustomerSector" runat="server" CssClass="table" ShowFooter="true"
                        PagerStyle-CssClass="pgr" AllowSorting="true" AutoGenerateColumns="false" AllowPaging="true"
                        OnPageIndexChanging="gvCustomerSector_PageIndexChanging" PageSize="20" PagerSettings-Position="TopAndBottom"
                        DataKeyNames="lid" OnRowCommand="gvCustomerSector_RowCommand" OnRowUpdating="gvCustomerSector_RowUpdating"
                        OnRowDeleting="gvCustomerSector_RowDeleting" OnRowEditing="gvCustomerSector_RowEditing"
                        Width="100%" OnRowCancelingEdit="gvCustomerSector_RowCancelingEdit">
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
                            <asp:TemplateField HeaderText="Sector Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblCustomerSector_Name" runat="server" Text='<%#Eval("sName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtCustomerSector_Name" runat="server" Text='<%#Eval("sName") %>'
                                        MaxLength="50"></asp:TextBox>
                                    <span style="color: Red">*</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtCustomerSector_Namefooter" runat="server" Text='<%#Eval("sName") %>'
                                        MaxLength="50" TabIndex="1"></asp:TextBox>
                                    <span style="color: Red">*</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sector Code">
                                <ItemTemplate>
                                    <asp:Label ID="lblCustomerSector_Code" runat="server" Text='<%#Eval("sCode") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtCustomerSector_Code" runat="server" Text='<%#Eval("sCode") %>'
                                        MaxLength="50"></asp:TextBox>
                                    <span style="color: Red">*</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtCustomerSector_Codefooter" runat="server" Text='<%#Eval("sCode") %>'
                                        MaxLength="50" TabIndex="2"></asp:TextBox>
                                    <span style="color: Red">*</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Remark">
                                <ItemTemplate>
                                    <asp:Label ID="lblRemark" runat="server" Text='<%#Eval("sRemarks") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtRemark" runat="server" Text='<%#Eval("sRemarks") %>' MaxLength="200"></asp:TextBox>
                                    <span style="color: Red">*</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtRemarkfooter" runat="server" Text='<%#Eval("sRemarks") %>' MaxLength="200" TabIndex="3"></asp:TextBox>
                                    <span style="color: Red">*</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit/Delete">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server"
                                        Text="Edit" Font-Underline="true"></asp:LinkButton>
                                 &nbsp;   <asp:LinkButton ID="lnkDelete" CommandName="Delete" ToolTip="Delete" Width="22" OnClientClick="return confirm('Sure to delete?');"
                                        runat="server" Text="Delete" Font-Underline="true"></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" Width="39" runat="server"
                                        Text="Update" Font-Underline="true"></asp:LinkButton>
                                  &nbsp;  <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" Width="22" runat="server"
                                        Text="Cancel" Font-Underline="true"></asp:LinkButton>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:LinkButton ID="lnkAdd" CommandName="Insert" ToolTip="Add" Width="22" runat="server"
                                        Text="Add" Font-Underline="true" TabIndex="4"></asp:LinkButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    
</asp:Content>
