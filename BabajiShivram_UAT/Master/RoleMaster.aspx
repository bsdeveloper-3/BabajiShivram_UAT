<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RoleMaster.aspx.cs" MasterPageFile="~/MasterPage.master"
    Inherits="RoleMaster" Title="Roles And Access Rights"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="Server" ContentPlaceHolderID="ContentPlaceHolder1">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional"
        RenderMode="Inline">
        <ContentTemplate>
            <script language="javascript" src="../Js/RoleMaster.js" type="text/javascript"></script>
            <div align="center">
                <asp:Label ID="lberror" Text="" runat="server" EnableViewState="false"></asp:Label>
                 <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                ShowSummary="False" ValidationGroup="Required" />
            </div>
            <div class="clear"></div>
                <asp:FormView ID="FormView1" runat="server" DataSourceID="FormViewSqlDataSource" OnDataBound="FormView1_DataBound"
                    Width="70%" OnItemInserted="FormView1_ItemInserted" OnItemDeleted="FormView1_ItemDeleted"
                    DataKeyNames="RoleId" OnItemUpdated="FormView1_ItemUpdated" OnItemCommand="FormView1_ItemCommand">
                    <EditItemTemplate>
                    <fieldset>
                    <legend>Update Access Role</legend>
                        <asp:Button ID="btnUpdateButton" runat="server" CommandName="Update"
                            Text="Update" ValidationGroup="Required" TabIndex="3" />
                        <asp:Button ID="btnUpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                            Text="Cancel" TabIndex="4" />
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>
                                    Role Name
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtEdRoleName"
                                        Text="*" ErrorMessage="Please Enter Role Name" ValidationGroup="Required" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEdRoleName" Text='<%# Bind("RoleName") %>' MaxLength="100" 
                                        runat="server" TabIndex="1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Remark
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEdRemark"
                                        Text="*" ErrorMessage="Please Enter Role Remark" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEdRemark" Text='<%# Bind("Remark") %>' TextMode="MultiLine" MaxLength="200"
                                        Width="70%" runat="server" TabIndex="2"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                    <fieldset>
                    <legend>Add Access Role</legend>
                        <asp:Button ID="btnInsertButton" runat="server" ValidationGroup="Required"
                            Text="Save" CommandName="Insert" TabIndex="3" />
                        <asp:Button ID="btnInsertCancelButton" runat="server" CommandName="Cancel"
                            CausesValidation="false" Text="Cancel" TabIndex="4" />
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>
                                    Role Name
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Text="*" ErrorMessage="Please Enter Role Name"
                                       InitialValue="" Display="Dynamic" ValidationGroup="Required" ControlToValidate="txtName"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtName" runat="server" MaxLength="100" ValidationGroup="Required" 
                                        Text='<%# Bind("RoleName") %>' TabIndex="1" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Role Remark
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtRemarks"
                                        InitialValue="" Text="*" ErrorMessage="Please Enter Role Remark" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRemarks" runat="server"  MaxLength="200" 
                                    TextMode="MultiLine" Width="200px" Text='<%# Bind("Remark") %>' TabIndex="2"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                       </fieldset> 
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <fieldset>
                    <legend>Access Role Detail</legend>
                        <asp:Button ID="btnEditButton" CssClass="edit" Text="Edit" CommandName="Edit" runat="server" />
                        <asp:Button ID="btnDeleteButton" Text="Delete" OnClientClick="return confirm('Sure to delete?');"
                            runat="server" CssClass="delete" CommandName="Delete" />
                        <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="cancel" CommandName="Cancel" />
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>
                                    Role Name</td>
                                <td>
                                    <asp:Label ID="lblRoleName" runat="server" Text='<%# Bind("RoleName") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Remark
                                </td>
                                <td>
                                    <asp:Label ID="lblRemark" runat="server" Text='<%# Eval("Remark") %>'></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        &nbsp;&nbsp;<asp:Button ID="btnNewButton" Text="Add New Role" runat="server" OnClick="btnNewButton_Click"
                            CommandName="New" />
                    </EmptyDataTemplate>
                </asp:FormView>
            
            <div>
            <fieldset id="fsMainBorder" runat="server">
            <legend>Access Role</legend>   
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table"
                    PagerStyle-CssClass="pgr" DataKeyNames="RoleId" DataSourceID="GridViewSqlDataSource"
                    Width="100%" CellPadding="4" PageSize="20" PagerSettings-Position="TopAndBottom"
                    AllowPaging="true" AllowSorting="true" OnRowCommand="GridView1_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%# Container.DataItemIndex +1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:ButtonField DataTextField="RoleName" HeaderText="Role Name" CommandName="Select"
                            SortExpression="RoleName" CausesValidation="false" />
                        <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" />
                        <asp:TemplateField HeaderText="Set Access Rights">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkRoleDetail" runat="server" CommandArgument='<%#String.Format("{0},{1}",Eval("RoleID"),Eval("RoleName"))%>'
                                    CommandName="RoleDetail" CausesValidation="False">Role Detail</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </fieldset>    
                <asp:SqlDataSource ID="GridViewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetRoleById" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="FormViewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetRoleById" SelectCommandType="StoredProcedure" InsertCommand="insRoleMaster"
                    InsertCommandType="StoredProcedure" UpdateCommand="updRoleMaster" UpdateCommandType="StoredProcedure"
                    DeleteCommand="delRoleMaster" DeleteCommandType="StoredProcedure"
                      OnInserted="FormviewSqlDataSource_Inserted" OnUpdated="FormviewSqlDataSource_Updated"
                      OnDeleted="FormviewSqlDataSource_Deleted" >
                    <SelectParameters>
                        <asp:ControlParameter ControlID="GridView1" Name="RoleId" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                    <InsertParameters>
                        <asp:Parameter Name="RoleName" Type="String" />
                        <asp:Parameter Name="Remark" Type="String" />
                        <asp:SessionParameter Name="lUser" SessionField="UserId" />
                        <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
                    </InsertParameters>
                    <DeleteParameters>
                        <asp:ControlParameter ControlID="GridView1" Name="RoleId" PropertyName="SelectedValue" />
                        <asp:SessionParameter Name="lUser" SessionField="UserId" />
                        <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:ControlParameter ControlID="GridView1" Name="RoleId" PropertyName="SelectedValue" />
                        <asp:Parameter Name="RoleName" Type="String" />
                        <asp:Parameter Name="Remark" Type="String" />
                        <asp:SessionParameter Name="lUser" SessionField="UserId" />
                        <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
                    </UpdateParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
