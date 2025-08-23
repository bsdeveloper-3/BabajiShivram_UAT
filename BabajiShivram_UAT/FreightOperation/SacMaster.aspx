<%@ Page Title="HSN/SAC Master" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SacMaster.aspx.cs"
    Inherits="FreightOperation_SacMaster" Culture="en-GB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <asp:UpdatePanel ID="upSacMaster" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblresult" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div class="clear"></div>
            <fieldset>
                <legend>HSN/SAC Setup</legend>
                <asp:GridView ID="gvSacDetail" runat="server" CssClass="table" ShowFooter="true" PagerStyle-CssClass="pgr"
                    AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" PageSize="20" PagerSettings-Position="TopAndBottom"
                    OnPageIndexChanging="gvSacDetail_PageIndexChanging" DataKeyNames="lId" OnRowCommand="gvSacDetail_RowCommand"
                    OnRowUpdating="gvSacDetail_RowUpdating" OnRowDeleting="gvSacDetail_RowDeleting" OnRowEditing="gvSacDetail_RowEditing"
                    Width="100%" OnRowCancelingEdit="gvSacDetail_RowCancelingEdit">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="HSN/SAC">
                            <ItemTemplate>
                                <asp:Label ID="lblSAC" runat="server" Text='<%#Eval("SacNo") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSAC" TabIndex="1" MaxLength="20" runat="server" Text='<%#Eval("SacNo") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtSACFooter" TabIndex="1" runat="server" MaxLength="20" Text='<%#Eval("SacNo") %>'></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <%--            <asp:TemplateField HeaderText="CGST (%)" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblCGST" runat="server" Text='<%#Eval("CGST") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtCGST" TabIndex="2" runat="server" Text='<%#Eval("CGST") %>' MaxLength="25"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revCGST" runat="server" ControlToValidate="txtCGST" ForeColor="Red" Display="Dynamic"
                                    SetFocusOnError="true" ValidationExpression="^\-{0,1}\d+(.\d+){0,1}$" ErrorMessage="Only numbers allowed"></asp:RegularExpressionValidator>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtCGSTFooter" TabIndex="2" runat="server" Text='<%#Eval("CGST") %>' MaxLength="25"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revCGSTFooter" runat="server" ControlToValidate="txtCGSTFooter" ForeColor="Red" Display="Dynamic"
                                    SetFocusOnError="true" ValidationExpression="^\-{0,1}\d+(.\d+){0,1}$" ErrorMessage="Only numbers allowed"></asp:RegularExpressionValidator>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SGST (%)" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblSGST" runat="server" Text='<%#Eval("SGST") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSGST" TabIndex="3" runat="server" Text='<%#Eval("SGST") %>' MaxLength="25"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revSGSTFooter" runat="server" ControlToValidate="txtSGST" ForeColor="Red" Display="Dynamic"
                                    SetFocusOnError="true" ValidationExpression="^\-{0,1}\d+(.\d+){0,1}$" ErrorMessage="Only numbers allowed"></asp:RegularExpressionValidator>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtSGSTFooter" TabIndex="3" runat="server" Text='<%#Eval("SGST") %>' MaxLength="25"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revSGSTFooter" runat="server" ControlToValidate="txtSGSTFooter" ForeColor="Red" Display="Dynamic"
                                    SetFocusOnError="true" ValidationExpression="^\-{0,1}\d+(.\d+){0,1}$" ErrorMessage="Only numbers allowed"></asp:RegularExpressionValidator>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="IGST (%)" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblIGST" runat="server" Text='<%#Eval("IGST") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtIGST" TabIndex="4" runat="server" Text='<%#Eval("IGST") %>' MaxLength="25"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revIGSTFooter" runat="server" ControlToValidate="txtIGST" ForeColor="Red" Display="Dynamic"
                                    SetFocusOnError="true" ValidationExpression="^\-{0,1}\d+(.\d+){0,1}$" ErrorMessage="Only numbers allowed"></asp:RegularExpressionValidator>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtIGSTFooter" TabIndex="4" runat="server" Text='<%#Eval("IGST") %>' MaxLength="25"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revIGSTFooter" runat="server" ControlToValidate="txtIGSTFooter" ForeColor="Red" Display="Dynamic"
                                    SetFocusOnError="true" ValidationExpression="^\-{0,1}\d+(.\d+){0,1}$" ErrorMessage="Only numbers allowed"></asp:RegularExpressionValidator>
                            </FooterTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Edit">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server" TabIndex="5"
                                    Text="Edit" Font-Underline="true"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" runat="server" TabIndex="6"
                                    Text="Update" Font-Underline="true"></asp:LinkButton>
                                &nbsp;
                                <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" runat="server" TabIndex="7"
                                    Text="Cancel" Font-Underline="true"></asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lnkAdd" CommandName="Insert" ToolTip="Add" Width="22" runat="server"
                                    Text="Add" Font-Underline="true"></asp:LinkButton>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

