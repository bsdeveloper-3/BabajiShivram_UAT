<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FABookMaster.aspx.cs" Inherits="Master_FABookMaster" %>


<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <AjaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div align="center">
        <asp:Label ID="lberror" runat="server" Text="" CssClass="errorMsg" EnableViewState="false"></asp:Label>
    </div>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
        ShowSummary="False" ValidationGroup="Required" />

    <asp:FormView ID="FormView1" runat="server" DataSourceID="FormViewDataSource" DataKeyNames="lid"
        Width="100%" OnDataBound="FormView1_DataBound"
        OnItemInserted="FormView1_ItemInserted" OnItemUpdated="FormView1_ItemUpdated"
        OnItemDeleted="FormView1_ItemDeleted" OnItemCommand="FormView1_ItemCommand">

        <InsertItemTemplate>
            <fieldset>
                <legend>Add New FA Book</legend>
                <div class="m clear">
                    <asp:Button ID="btnInsertButton" runat="server" CommandName="Insert" ValidationGroup="Required"
                        Text="Save" TabIndex="11" />
                    <asp:Button ID="btnUpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                        Text="Cancel" TabIndex="12" />
                </div>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <tr>
                        <td>FA Book Master
                        <asp:RequiredFieldValidator ID="RFV01" runat="server" ControlToValidate="txtBookName" SetFocusOnError="true"
                            Display="Dynamic" Text="*" ErrorMessage="Please Enter Book Name" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBookName" runat="server" Text='<%# Bind("BookName") %>' TabIndex="1"></asp:TextBox>
                        </td>
                        <td>Book Code
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBookCode" SetFocusOnError="true"
                            Text="*" Display="Dynamic" ErrorMessage="Please Enter Book Code" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBookCode" runat="server" TabIndex="4" Text='<%# Bind("BookCode") %>' />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </InsertItemTemplate>
        <EditItemTemplate>
            <fieldset>
                <legend>Update FA Book Detail</legend>
                <div class="m clear">
                    <asp:Button ID="btnUpdateButton" runat="server" CommandName="Update" TabIndex="11"
                        Text="Update" ValidationGroup="Required" />
                    <asp:Button ID="btnUpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                        TabIndex="12" Text="Cancel" />
                </div>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <tr>
                        <td>FA Book Master
                        <asp:RequiredFieldValidator ID="RFV01" runat="server" ControlToValidate="txtBookName" SetFocusOnError="true"
                            Display="Dynamic" Text="*" ErrorMessage="Please Enter Book Name" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBookName" runat="server" Text='<%# Bind("BookName") %>' TabIndex="1"></asp:TextBox>
                        </td>
                        <td>Book Code
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBookCode" SetFocusOnError="true"
                            Text="*" Display="Dynamic" ErrorMessage="Please Enter Book Code" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBookCode" runat="server" TabIndex="4" Text='<%# Bind("BookCode") %>' />
                        </td>
                    </tr>
                    <tr>

                        <td>Is Active?
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblActive" runat="server" SelectedValue='<%#Bind("IsActive") %>'
                                RepeatDirection="Horizontal">
                                <asp:ListItem Text="Yes" Value="True" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="No" Value="False"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
                <div class="m clear">
                </div>

            </fieldset>
        </EditItemTemplate>
        <ItemTemplate>
            <fieldset>
                <legend>FA book Detail</legend>
                <div class="m clear">
                    <asp:Button ID="btnEditButton" runat="server" CommandName="Edit" Text="Edit" />
                    <asp:Button ID="btnDeleteButton" runat="server" CommandName="Delete" OnClientClick="return confirm('Sure to delete?');" Text="Delete" />
                    <asp:Button ID="btnCancelButton" Text="Cancel" CommandName="Cancel" runat="server" />
                </div>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <tr>
                        <td>Book Name
                        </td>
                        <td>
                            <asp:Label ID="lblBookName" runat="server" Text='<%# Bind("BookName") %>'></asp:Label>
                        </td>
                        <td>Book Code</td>
                        <td>
                            <asp:Label ID="lblbookcode" runat="server" Text='<%# Bind("BookCode") %>'></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Is Active?
                        </td>
                        <td>
                            <%# (Boolean.Parse(Eval("IsActive").ToString())) ? "Yes" : "No"%>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
                <asp:HiddenField ID="hdnUserId" runat="server" Value='<%# Bind("lId") %>' Visible="true" />
            </fieldset>


        </ItemTemplate>
        <EmptyDataTemplate>
            &nbsp;&nbsp;<asp:Button ID="btnNew" runat="server" Text="Add New FA Book" CommandName="New" />

        </EmptyDataTemplate>
    </asp:FormView>

    <fieldset id="fsMainBorder" runat="server">
        <legend>FA Book Detail</legend>
        <!-- Filter Content Start-->
        <div class="m clear">
            <div class="fleft">
                <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>&nbsp;&nbsp;
            </div>
            <div class="fleft">
                <uc1:DataFilter ID="DataFilter1" runat="server" />
            </div>
        </div>
        <div class="clear">
        </div>
        <!-- Filter Content END-->
        <asp:GridView ID="gvBook" runat="server" AutoGenerateColumns="False" DataSourceID="GridViewSqlDataSource"
            AllowSorting="true" AllowPaging="True" CssClass="table" AlternatingRowStyle-CssClass="alt"
            DataKeyNames="lId" PageSize="20" PagerStyle-CssClass="pgr" OnSelectedIndexChanged="gvBook_SelectedIndexChanged"
            OnPreRender="gvBook_PreRender" PagerSettings-Position="TopAndBottom">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%# Container.DataItemIndex +1%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Book Name" SortExpression="BookName">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkempname" runat="Server" Text='<%#Eval("BookName") %>' CommandName="select"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="BookName" HeaderText="Book Name" Visible="false" />

                <asp:BoundField DataField="BookCode" HeaderText="BookCode" SortExpression="BookCode" />
                <asp:BoundField DataField="ActiveStatus" HeaderText="ActiveStatus" SortExpression="ActiveStatus" />
                <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" SortExpression="CreatedDate" />
            </Columns>
            <PagerTemplate>
                <asp:GridViewPager runat="server" />
            </PagerTemplate>
        </asp:GridView>
    </fieldset>
    <div id="divDataSource">
        <asp:SqlDataSource ID="FormViewDataSource" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
            runat="server" SelectCommand="USP_CB_GetFABookDetail" SelectCommandType="StoredProcedure"
            InsertCommand="USP_CB_insertFABook" InsertCommandType="StoredProcedure" UpdateCommand="USP_CB_UpdateFABookDetail"
            UpdateCommandType="StoredProcedure" DeleteCommand="USP_CB_DeleteBookDetail" DeleteCommandType="StoredProcedure"
            OnInserted="FormviewSqlDataSource_Inserted" OnUpdated="FormviewSqlDataSource_Updated"
            OnDeleted="FormviewSqlDataSource_Deleted">
            <SelectParameters>
                <asp:ControlParameter ControlID="gvBook" Name="lId" PropertyName="SelectedValue" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="BookName" Type="string" />
                <asp:Parameter Name="BookCode" Type="string" />
                <asp:SessionParameter Name="lUser" SessionField="UserId" />
                <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="lId" />
                <asp:Parameter Name="BookName" Type="string" />
                <asp:Parameter Name="BookCode" Type="string" />
                <asp:SessionParameter Name="lUser" SessionField="UserId" />
                <asp:ControlParameter ControlID="ctl00$ContentPlaceHolder1$FormView1$rblActive" Name="IsActive"
                    PropertyName="SelectedValue" Type="string" />
                <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
            </UpdateParameters>
            <DeleteParameters>
                <asp:Parameter Name="lId" />
                <asp:SessionParameter Name="lUser" SessionField="UserId" />
                <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
            </DeleteParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="GridviewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="USP_CB_GetFABookDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
            </SelectParameters>

        </asp:SqlDataSource>

    </div>

</asp:Content>

