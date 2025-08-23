<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Department.aspx.cs" MasterPageFile="~/MasterPage.master"
    Inherits="Department" Title="Department Setup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist" runat="server">
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
        <fieldset>
    <legend>Department Setup</legend>
        <div>          
            <asp:GridView ID="gvDepartment" runat="server" CssClass="table" ShowFooter="true" PagerStyle-CssClass="pgr" 
                AllowSorting="true" AutoGenerateColumns="false" AllowPaging="true"
                OnPageIndexChanging="gvDepartment_PageIndexChanging" PageSize="20" PagerSettings-Position="TopAndBottom"
                DataKeyNames="lid" OnRowCommand="gvDepartment_RowCommand" OnRowUpdating="gvDepartment_RowUpdating"
                OnRowDeleting="gvDepartment_RowDeleting" OnRowEditing="gvDepartment_RowEditing" Width="100%"
                OnRowCancelingEdit="gvDepartment_RowCancelingEdit">
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
                    <asp:TemplateField HeaderText="Department Name">
                        <ItemTemplate>
                            <asp:Label ID="lblDepartment_Name" runat="server" Text='<%#Eval("DeptName") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtDepartment_Name" runat="server" Text='<%#Eval("DeptName") %>' MaxLength="50"></asp:TextBox>
                            <span style="color:Red">*</span>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtDepartment_Namefooter" runat="server" Text='<%#Eval("DeptName") %>' MaxLength="50" TabIndex="1"></asp:TextBox>
                            <span style="color:Red">*</span>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Department Code">
                        <ItemTemplate>
                            <asp:Label ID="lblDepartment_Code" runat="server" Text='<%#Eval("DeptCode") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtDepartment_Code" runat="server" Text='<%#Eval("DeptCode") %>' MaxLength="50"></asp:TextBox>
                            <span style="color:Red">*</span>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtDepartment_Codefooter" runat="server" Text='<%#Eval("DeptCode") %>' MaxLength="50" TabIndex="2"></asp:TextBox>
                            <span style="color:Red">*</span>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Remark">
                        <ItemTemplate>
                            <asp:Label ID="lblRemark" runat="server" Text='<%#Eval("sRemarks") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtRemark" runat="server" Text='<%#Eval("sRemarks") %>' MaxLength="200"></asp:TextBox>
                            <span style="color:Red">*</span>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtRemarkfooter" runat="server" Text='<%#Eval("sRemarks") %>' MaxLength="200" TabIndex="3"></asp:TextBox>
                            <span style="color:Red">*</span>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Edit/Delete">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server" Text="Edit" Font-Underline="true"></asp:LinkButton>
                            &nbsp;<asp:LinkButton ID="lnkDelete" CommandName="Delete" ToolTip="Delete" Width="22" OnClientClick="return confirm('Sure to delete?');" runat="server" Text="Delete" Font-Underline="true"></asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                             <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" Width="39" runat="server" Text="Update" Font-Underline="true"></asp:LinkButton>
                             &nbsp;<asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" Width="22" runat="server" Text="Cancel" Font-Underline="true"></asp:LinkButton>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:LinkButton ID="lnkAdd" CommandName="Insert" ToolTip="Add" Width="22" runat="server" Text="Add" Font-Underline="true" TabIndex="4"></asp:LinkButton>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
     </fieldset>   
    </ContentTemplate> 
    </asp:UpdatePanel> 
    
</asp:Content>
