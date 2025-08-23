<%@ Page Title="Applicable Fields" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="QuoteApplicableFieldsMS.aspx.cs"
    Inherits="QuoteApplicableFieldsMS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>

    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upQuoteAppFields" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upQuoteAppFields" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblresult" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <fieldset>
                <legend>Quotation Applicable Fields Setup</legend>
                <div>
                    <asp:GridView ID="gvQuotationAppFields" runat="server" CssClass="table" ShowFooter="true" PagerStyle-CssClass="pgr"
                        AllowSorting="true" AutoGenerateColumns="false" AllowPaging="true"
                        OnPageIndexChanging="gvQuotationAppFields_PageIndexChanging" PageSize="20" PagerSettings-Position="TopAndBottom"
                        DataKeyNames="lid" OnRowCommand="gvQuotationAppFields_RowCommand" OnRowUpdating="gvQuotationAppFields_RowUpdating"
                        OnRowDeleting="gvQuotationAppFields_RowDeleting" OnRowEditing="gvQuotationAppFields_RowEditing" Width="100%"
                        OnRowCancelingEdit="gvQuotationAppFields_RowCancelingEdit">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Category Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblQuotationAppField" runat="server" Text='<%#Eval("sName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtQuotationAppField" runat="server" Text='<%#Eval("sName") %>' Width="90%" TabIndex="1"></asp:TextBox>
                                    <span style="color: Red">*</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtQuotationAppFieldfooter" runat="server" Text='<%#Eval("sName") %>' Width="90%" TabIndex="1"></asp:TextBox>
                                    <span style="color: Red">*</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit/Delete">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server" Text="Edit" Font-Underline="true"></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="lnkDelete" CommandName="Delete" ToolTip="Delete" Width="22" OnClientClick="return confirm('Sure to delete?');" runat="server" Text="Delete" Font-Underline="true"></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" Width="39" runat="server" Text="Update" TabIndex="2" Font-Underline="true"></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" Width="22" runat="server" TabIndex="3" Text="Cancel" Font-Underline="true"></asp:LinkButton>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:LinkButton ID="lnkAdd" CommandName="Insert" ToolTip="Add" Width="22" runat="server" Text="Add" Font-Underline="true" TabIndex="2"></asp:LinkButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

