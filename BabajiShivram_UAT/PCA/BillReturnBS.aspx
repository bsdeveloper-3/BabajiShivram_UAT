<%@ Page Title="Bill Return" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="BillReturnBS.aspx.cs" Inherits="PCA_BillReturnBS" EnableEventValidation="false" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upBillReturn" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upBillReturn" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </div>
            <div class="clear"></div>
            <div>
                <fieldset>
                    <legend>Bill Return Detail</legend>
                    <div class="m clear">
                        <asp:Panel ID="pnlFilter" runat="server">
                            <div class="fleft">
                                <uc1:DataFilter ID="DataFilter1" runat="server" />
                            </div>
                            <div class="fleft" style="margin-left: 40px;">

                                <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                            <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                                </asp:LinkButton>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="clear">
                    </div>

                    <asp:GridView ID="gridBillReturn" runat="server" AutoGenerateColumns="False" CssClass="table"
                        Width="100%" PagerStyle-CssClass="pgr" CellPadding="4" AllowPaging="True" DataSourceID="BillReturnSqlDataSource"
                        AllowSorting="True" PageSize="20" OnRowDataBound="gridBillReturn_RowDataBound" OnRowCommand="gridBillReturn_RowCommand"
                        PagerSettings-Position="TopAndBottom" OnPreRender="gridBillReturn_PreRender" >
                        <%--OnRowCommand="GridViewDocument_RowCommand">--%>
                        <Columns>
                            <asp:TemplateField HeaderText="SI">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Job Ref No">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblJobRefNo" CommandName="select" runat="server" Text='<%#Bind("JobRefNo") %>'
                                        CommandArgument='<%#Eval("JobId") + ";" + Eval("lid") + ";" + Eval("BillRetLid")%>' />
                                    <asp:Label ID="lblJobId" runat="server" Text='<%#Bind("JobId") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" Visible="false" />
                            <asp:BoundField DataField="CustName" HeaderText="Customer" SortExpression="Customer" Visible="false" />
                            <asp:BoundField DataField="CustRefNo" HeaderText="Customer Ref No" SortExpression="CustRefNo" Visible="false" />
                            <asp:BoundField DataField="INVNO" HeaderText="Bill No" SortExpression="INVNO" Visible="false" />
                            <asp:BoundField DataField="BillReturnReason" HeaderText="Reason" SortExpression="BillReturnReason" visible="false"/>
                            <asp:BoundField DataField="INVAMOUNT" HeaderText="Amount No" SortExpression="INVAMOUNT" Visible="false" />
                            <asp:BoundField DataField="BillReturnRemark" HeaderText="Return Remark / Error" SortExpression="BillReturnRemark" Visible="false"/>
                            <asp:BoundField DataField="BillRetDate" HeaderText="Return Date" SortExpression="BillRetDate" Visible="false"/>
                            <asp:BoundField DataField="CustUserName" HeaderText="Bill Return By" SortExpression="CustUserName" Visible="false"/>
                            <asp:BoundField DataField="Aging" HeaderText="Aging" SortExpression="Aging" Visible="false"/>

                            <asp:TemplateField HeaderText="Customer">
                                <ItemTemplate>
                                    <asp:Label ID="lblcustName" runat="server" Text='<%#Bind("CustName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Customer Ref No">
                                <ItemTemplate>
                                    <asp:Label ID="lbllid" runat="server" Text='<%#Bind("lid") %>'></asp:Label>
                                    <asp:Label ID="lblCustRfNo" runat="server" Text='<%#Bind("CustRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Bill No" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblBillNo" runat="server" Text='<%#Bind("INVNO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount">
                                <ItemTemplate>
                                    <asp:Label ID="lblINVAMOUNT" runat="server" Text='<%#Bind("INVAMOUNT") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Reason">
                                <ItemTemplate>
                                    <asp:Label ID="lblRetutnReason" runat="server" Text='<%#Bind("BillReturnReason") %>' Visible="false"></asp:Label>
                                    <asp:DropDownList ID="ddlReason" runat="server" Enabled="false" ForeColor="Black" Visible="false">
                                        <asp:ListItem Value="0" Text="-Select-"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="GST Invoice Related"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Teriff/Contract"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="SOP Contract"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Dispatch/Wrong Attension"></asp:ListItem>
                                        <asp:ListItem Value="5" Text="Warehouse"></asp:ListItem>
                                        <asp:ListItem Value="6" Text="Shipping Line"></asp:ListItem>
                                        <asp:ListItem Value="7" Text="CFS"></asp:ListItem>
                                        <asp:ListItem Value="8" Text="Other"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label ID="lblReason" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Return Remark / Error">
                                <ItemTemplate>
                                    <asp:Label ID="lblRemark" runat="server" Text='<%#Bind("BillReturnRemark") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Return Date" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblReturnDate" runat="server" Text='<%#Bind("BillRetDate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Bill Return By">
                                <ItemTemplate>
                                    <asp:Label ID="lblCustUserName" runat="server" Text='<%#Bind("CustUserName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Aging">
                                <ItemTemplate>
                                    <asp:Label ID="lblAging" runat="server" Text='<%#Bind("Aging") %>' ToolTip="Today – Bill Reurn Date"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerTemplate>
                            <asp:GridViewPager ID="GridViewPager1" runat="server" />
                        </PagerTemplate>
                    </asp:GridView>
                </fieldset>
            </div>

            <div>
                <asp:SqlDataSource ID="BillReturnSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetPendingBillReturnBS" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
            <%--</fieldset>--%>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

