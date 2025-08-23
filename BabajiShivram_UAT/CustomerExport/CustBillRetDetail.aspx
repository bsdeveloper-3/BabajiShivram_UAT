<%@ Page Title="Bill Return Detail" Language="C#" MasterPageFile="~/ExportCustomerMaster.master" AutoEventWireup="true"
    CodeFile="CustBillRetDetail.aspx.cs" Inherits="CustBillRetDetail" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />

    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
   DisplayMode ="List" ShowSummary = "true" ValidationGroup="ReqSummary" />

    <asp:UpdatePanel ID="upBillReturn" runat="server" UpdateMode="Conditional"
        RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div class="clear"></div>
            <fieldset>
                <legend>Bill Return Detail</legend>
                <div class="m clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:datafilter id="DataFilter1" runat="server" />
                        </div>
                    </asp:Panel>
                </div>
                <div class="m clear"></div>
                <asp:GridView ID="gvBillReturnDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                    PagerStyle-CssClass="pgr" DataKeyNames="JobId" DataSourceID="BillReturnSqlDataSource"
                    AllowPaging="True" AllowSorting="True" PageSize="20" PagerSettings-Position="TopAndBottom"
                    OnPreRender="gvBillReturnDetaill_PreRender" OnRowDataBound="gvBillReturnDetail_RowDataBound">
                    <%-- OnRowCommand="gvJobDetail_RowCommand">--%>
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="BS Job No" SortExpression="JobRefNo" Visible="false">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkJobRefNo" CommandName="select" runat="server" Text='<%#Eval("JobRefNo") %>'
                                    CommandArgument='<%#Bind("JobId") + Bind("BJVlid") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" SortExpression="JobRefNo" />
                        <asp:BoundField DataField="CustName" HeaderText="Customer Name" SortExpression="CustName" />
                        <asp:BoundField DataField="INVNO" HeaderText="INVoice NO" SortExpression="INVNO" />
                        <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />
                        <asp:TemplateField HeaderText="Reason">
                            <ItemTemplate>
                                 <asp:Label ID="lblReturnReason" runat="server" Text='<%#Bind("Reason") %>' Visible="false"></asp:Label>
                                <asp:DropDownList ID="ddlBillReason" runat="server" Enabled="false" ForeColor="Black" Visible="false">
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
                                <asp:Label ID="lblBillReason" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="Reason" HeaderText="Reason" SortExpression="Reason"  Visible="false"/>
                        <asp:BoundField DataField="RemarkCust" HeaderText="Customer Remark" SortExpression="RemarkCust" />
                        <asp:BoundField DataField="BillReturnBy" HeaderText="Bill Return By" SortExpression="BillReturnBy" />
                        <asp:BoundField DataField="BillReturnDate" HeaderText="Bill Return Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="BillReturnDate" />
                        <asp:BoundField DataField="RemarkBSEnd" HeaderText="Remark BSEnd" SortExpression="RemarkBSEnd" />
                        <asp:BoundField DataField="ChangesDate" HeaderText="Changes Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ChangesDate" />
                        <asp:BoundField DataField="NewDispatchDate" HeaderText="New Dispatch Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="NewDispatchDate" />
                        <asp:BoundField DataField="updDate" HeaderText="Bill Update Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="updDate" />
                        <asp:BoundField DataField="BillCompleteBy" HeaderText="Bill Completed By" SortExpression="BillCompleteBy" />

                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="BillReturnSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetAllBillReturnDetail" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="CustUserId" SessionField="CustUserId" />
                        <%--<asp:SessionParameter Name="UserId" SessionField="UserId" DefaultValue="0" />--%>
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

