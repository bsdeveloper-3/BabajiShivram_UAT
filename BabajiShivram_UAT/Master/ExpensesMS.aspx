<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExpensesMS.aspx.cs" MasterPageFile="~/MasterPage.master"
    Inherits="Expenses" Title="Expense Type Setup" %>

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
            <legend>Expense Type Detail</legend>
                <asp:GridView ID="gvExpensesMaster" runat="server" CssClass="table" AutoGenerateColumns="false"
                    PagerSettings-Position="TopAndBottom" AllowPaging="true" PagerStyle-CssClass="pgr"
                    PageSize="20" OnPageIndexChanging="gvDivision_PageIndexChanging" OnRowDeleting="gvExpensesMaster_RowDeleting"
                    OnRowEditing="gvExpensesMaster_RowEditing" OnRowCancelingEdit="gvExpensesMaster_RowCancelingEdit"
                    OnRowCommand="gvExpensesMaster_RowCommand" OnRowUpdating="gvExpensesMaster_RowUpdating"
                    ShowFooter="true">
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
                        <asp:TemplateField HeaderText="Expense Type">
                            <ItemTemplate>
                                <asp:Label ID="lblExpenseMName" runat="server" Text='<%# Eval("expenseName") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txteExpenseMName" runat="server" Text='<%# Eval("expenseName") %>'
                                    MaxLength="50" />
                                <span style="color: Red;">*</span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtExpenseMName" runat="server" Text='<%#Eval("expenseName") %>'
                                    MaxLength="50" TabIndex="1" />
                                <span style="color: Red;">*</span>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remark">
                            <ItemTemplate>
                                <asp:Label ID="lblRemark" runat="server" Text='<%#Eval("Remark") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txteRemark" runat="server" Text='<%# Eval("Remark") %>' MaxLength="200"
                                    TextMode="MultiLine"/>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtRemark" runat="server" Text='<%#Eval("Remark") %>' MaxLength="200"
                                    TextMode="MultiLine" TabIndex="2"/>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit/Delete">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnEdit" runat="server" Text="Edit" CommandName="Edit" ToolTip="Edit"
                                    Font-Underline="true" />
                               &nbsp; <asp:LinkButton ID="lbtnDelete" runat="server" Text="Delete" CommandName="Delete"
                                    ToolTip="Delete" OnClientClick="return confirm('Sure to delete?');" Font-Underline="true" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" runat="server"
                                    Text="Update" Font-Underline="true"></asp:LinkButton>
                               &nbsp; <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" runat="server"
                                    Text="Cancel" Font-Underline="true"></asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="btnAdd" runat="server" Text="Add" ToolTip="Add" Font-Underline="true"
                                    CommandName="Insert" TabIndex="3" />
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    
</asp:Content>
<%--
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
--%>
