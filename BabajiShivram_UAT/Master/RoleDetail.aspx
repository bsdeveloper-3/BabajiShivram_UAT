<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RoleDetail.aspx.cs" MasterPageFile="~/MasterPage.master"
    Inherits="RoleDetail" Title="Role Detail" %>

<asp:Content ID="Content1" runat="Server" ContentPlaceHolderID="ContentPlaceHolder1">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <script language="javascript" type="text/javascript" src="../Js/RoleDetail.js"></script>
    <div>

        <fieldset>
            <legend>Access Role Detail</legend>
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                    <asp:Label ID="lblMsg" runat="server" Style="text-align: center;"></asp:Label>
                    <div class="clear"></div>
                    <table class="table" border="0" style="width: 100%">
                        <tr>
                            <td valign="top">Role Name:
                            <asp:Label ID="lblRoleName" runat="server"></asp:Label>
                                <asp:HiddenField ID="hidRoleId" runat="server" />
                            </td>
                            <td>Module
                            </td>
                            <td>
                                <asp:DropDownList ID="ddModule" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddModule_SelectedIndexChanged">
                                    <asp:ListItem Text="Import Tracking" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Freight Tracking" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="NBCPL Transport" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Babaji Transport" Value="12"></asp:ListItem>
                                    <asp:ListItem Text="Company Services" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="Export CHA" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="Quotation" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="Special Economic Zone" Value="8"></asp:ListItem>
                                    <asp:ListItem Text="Account Expense" Value="9"></asp:ListItem>
                                    <asp:ListItem Text="Container Movement" Value="10"></asp:ListItem>
                                    <asp:ListItem Text="CRM" Value="11"></asp:ListItem>
                                    <asp:ListItem Text="MIS" Value="15"></asp:ListItem>
                                </asp:DropDownList>

                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>

            <asp:Label ID="Label1" runat="server" Text="lblId" Visible="False"></asp:Label>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Button ID="btnSave2" Visible="false" runat="server" OnClick="btnSave_Click"
                        Text="Save Role Detail" />
                    <asp:Button ID="btnCancel2" Visible="false" runat="server" OnClick="btnCancel_Click"
                        Text="Cancel" />
                    <asp:GridView ID="grdUserRight" runat="server" AutoGenerateColumns="False" CssClass="table"
                        Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" GridLines="Both"
                        CellPadding="5" BorderWidth="1px" BorderStyle="Solid" BorderColor="#336699" OnRowCommand="grdUserRight_RowCommand"
                        OnRowDataBound="grdUserRight_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="lId" Visible="false">
                                <%--Visible="False">--%>
                                <ItemTemplate>
                                    <asp:Label ID="lbllId" runat="server" Text='<%# Bind("lId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="cGsGpFlag" Visible="false">
                                <HeaderStyle HorizontalAlign="Center" />
                                <%--Visible="False">--%>
                                <ItemTemplate>
                                    <asp:Label ID="lblcGsGpFlag" runat="server" Text='<%# Bind("cGsGpFlag") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblsName" runat="server" Text='<%# Bind("sName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" Width="300px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rights">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkAll" name="chkAll" runat="server" AutoPostBack="true" Checked='<%# Bind("0") %>'
                                        OnCheckedChanged="chkAll_CheckedChanged" onclick="setRowNum1();" />
                                    <asp:HiddenField ID="hidcontrol" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="lPrevId" Visible="false">
                                <HeaderStyle HorizontalAlign="Center" />
                                <%--Visible="False">--%>
                                <ItemTemplate>
                                    <asp:Label ID="lbllPrevId" runat="server" Text='<%# Bind("lPrevId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="lIndexId" Visible="false">
                                <HeaderStyle HorizontalAlign="Center" />
                                <%--Visible="False">--%>
                                <ItemTemplate>
                                    <asp:Label ID="lbllIndexId" runat="server" Text='<%# Bind("lIndexId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="cTyp" Visible="false">
                                <HeaderStyle HorizontalAlign="Center" />
                                <%--Visible="False">--%>
                                <ItemTemplate>
                                    <asp:Label ID="lblcTyp" runat="server" Text='<%# Bind("cTyp") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:HiddenField ID="hidRowIndex" runat="server" EnableViewState="true" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <asp:Button ID="btnSave" Visible="false" runat="server" OnClick="btnSave_Click"
                        Text="Save Role Detail" />
                    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click"
                        Text="Cancel" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
    </div>

</asp:Content>
