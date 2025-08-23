<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="WareHouse.aspx.cs" Inherits="WareHouse" Title="Warehouse Setup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
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
                <legend>Warehouse Setup</legend>
                <div>
                    <asp:GridView ID="gvWareHouse" runat="server" CssClass="table" ShowFooter="true"
                        PagerStyle-CssClass="pgr" AllowSorting="true" AutoGenerateColumns="false" AllowPaging="true"
                        OnPageIndexChanging="gvWareHouse_PageIndexChanging" PageSize="20" PagerSettings-Position="TopAndBottom"
                        DataKeyNames="lid" OnRowCommand="gvWareHouse_RowCommand" OnRowUpdating="gvWareHouse_RowUpdating"
                        OnRowDeleting="gvWareHouse_RowDeleting" OnRowEditing="gvWareHouse_RowEditing"
                        Width="99%" OnRowCancelingEdit="gvWareHouse_RowCancelingEdit">
                                                <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit/Delete">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" runat="server" Text="Edit"></asp:LinkButton>
                                  &nbsp; <asp:LinkButton ID="lnkDelete" CommandName="Delete" ToolTip="Delete" OnClientClick="return confirm('Sure to delete?');"
                                        runat="server" Text="Delete"></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" runat="server"
                                        Text="Update"></asp:LinkButton>
                                  &nbsp;  <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" runat="server"
                                        Text="Cancel"></asp:LinkButton>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:LinkButton ID="lnkAdd" CommandName="Insert" ToolTip="Add" runat="server"
                                        Text="Add" Font-Underline="true" TabIndex="10"></asp:LinkButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="lid" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbllid" runat="server" Text='<%#Eval("lid") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtlid" runat="server" Text='<%#Eval("lid") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtlidFooter" runat="server"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Warehouse Code">
                                <ItemTemplate>
                                    <asp:Label ID="lblWareHouse_Code" runat="server" Text='<%#Eval("Code") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtWareHouse_Code" runat="server" Text='<%#Eval("Code") %>' MaxLength="50"></asp:TextBox>
                                    <span style="color: Red">*</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtWareHouse_CodeFooter" runat="server" MaxLength="50" TabIndex="1"></asp:TextBox>
                                    <span style="color: Red">*</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblWareHouse_Name" runat="server" Text='<%#Eval("sName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtWareHouse_Name" runat="server" Text='<%#Eval("sName") %>' MaxLength="100"></asp:TextBox>
                                    <span style="color: Red">*</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtWareHouse_NameFooter" runat="server" MaxLength="100" TabIndex="2"></asp:TextBox>
                                    <span style="color: Red">*</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Address">
                                <ItemTemplate>
                                    <asp:Label ID="lblWareHouse_Address" runat="server" Text='<%#Eval("sAddress") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtWareHouse_Address" runat="server" Text='<%#Eval("sAddress") %>'
                                        MaxLength="200" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                    <span style="color: Red">*</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtWareHouse_AddressFooter" runat="server" MaxLength="200" 
                                        TextMode="MultiLine" Rows="2" TabIndex="3"></asp:TextBox>
                                    <span style="color: Red">*</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Type">
                                <ItemTemplate>
                                    <asp:Label ID="lblType" runat="server" Text='<%#Eval("TypeName")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddWarehouseType" runat="server" SelectedValue='<%#Eval("lType") %>' Width="100px" TabIndex="4">
                                        <asp:ListItem Text="General" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Bonded" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="SEZ" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                    <span style="color: Red">*</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddWarehouseType_Footer" runat="server" Width="100px" TabIndex="5">
                                        <asp:ListItem Text="General" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Bonded" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="SEZ" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                    <span style="color: Red">*</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Contact Person Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblWareHouse_ContactName" runat="server" Text='<%#Eval("ContactName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtWareHouse_ContactName" runat="server" Text='<%#Eval("ContactName") %>'
                                        MaxLength="100"></asp:TextBox>
                                    <span style="color: Red">*</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtWareHouse_ContactNameFooter" runat="server" MaxLength="100" TabIndex="6"></asp:TextBox>
                                    <span style="color: Red">*</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Contact Number">
                                <ItemTemplate>
                                    <asp:Label ID="lblWareHouse_ContactNumber" runat="server" Text='<%#Eval("ContactNumber") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtWareHouse_ContactNumber" runat="server" Text='<%#Eval("ContactNumber") %>'
                                        MaxLength="100"></asp:TextBox>
                                    <span style="color: Red">*</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtWareHouse_ContactNumberFooter" runat="server" MaxLength="100" TabIndex="7"></asp:TextBox>
                                    <span style="color: Red">*</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email id">
                                <ItemTemplate>
                                    <asp:Label ID="lblWareHouse_EmailId" runat="server" Text='<%#Eval("EmailId") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtWareHouse_EmailId" runat="server" Text='<%#Eval("EmailId") %>'
                                        MaxLength="100"></asp:TextBox>
                                    <span style="color: Red">*</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtWareHouse_EmailIdFooter" runat="server" MaxLength="100" TabIndex="8"></asp:TextBox>
                                    <span style="color: Red">*</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:Label ID="lblWareHouse_Status" runat="server" Text='<%#Eval("StatusName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddWareHouse_Status" runat="server" SelectedValue='<%#Eval("bStatus") %>'>
                                        <asp:ListItem Text="Active" Value="True"></asp:ListItem>
                                        <asp:ListItem Text="In Active" Value="False"></asp:ListItem>
                                    </asp:DropDownList>
                                    <span style="color: Red">*</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddWareHouse_StatusFooter" runat="server" TabIndex="9" Width="100px">
                                        <asp:ListItem Text="Active" Value="True"></asp:ListItem>
                                        <asp:ListItem Text="In Active" Value="False"></asp:ListItem>
                                    </asp:DropDownList>
                                    <span style="color: Red">*</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>

                    </asp:GridView>
                </div>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
