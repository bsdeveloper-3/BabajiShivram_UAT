<%@ Page Title="Invoice Audit - Proforma to Final" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="PendingInvoicePostingFinal.aspx.cs" Inherits="AccountExpense_PendingInvoicePostingFinal" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <fieldset><legend>Invoice Audit - Proforma to Final</legend>
        <div align="center">
            <asp:Label ID="lblMessage" runat="server" CssClass="errorMsg" EnableViewState="false" ></asp:Label>
        </div>
        
        <div class="clear">
            <asp:Panel ID="pnlFilter" runat="server">
                <div class="fleft">
                    <uc1:DataFilter ID="DataFilter1" runat="server" />
                </div>
                <div class="fleft" style="margin-left:30px; padding-top:3px;">
                    <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                        <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                </div>
            </asp:Panel>
        </div>
        <div class="m clear"></div>
        <asp:GridView ID="gvDetail" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="InvoiceId"
            AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataSourceID="InvoiceSqlDataSource" 
            AllowPaging="True" AllowSorting="True" PageSize="40" PagerSettings-Position="TopAndBottom"
            Width="100%" OnRowCommand="gvDetail_RowCommand" OnRowDataBound="gvDetail_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Job No" SortExpression="FARefNo">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkJobRefNo" CommandName="select" runat="server" Text='<%#Eval("FARefNo") %>'
                            CommandArgument='<%#Eval("InvoiceId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="FARefNo" HeaderText="Job No" Visible="False" />
                <asp:BoundField DataField="ExpenseTypeName" HeaderText="Exp Type" SortExpression="ExpenseTypeName" />
                <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" SortExpression="InvoiceNo" />
                <asp:BoundField DataField="InvoiceTypeName" HeaderText="Invoice Type" SortExpression="InvoiceTypeName" />
                <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName" />
                <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                <%--<asp:BoundField DataField="Consignee" HeaderText="Consignee" SortExpression="Consignee" />--%>
                <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" SortExpression="VendorName" />
                <%--<asp:BoundField DataField="VendorGSTNo" HeaderText="Supplier GSTIN No" SortExpression="VendorGSTNo" />--%>
                <asp:BoundField DataField="PaymentTerms" HeaderText="Vendor Pay Terms Days" SortExpression="PaymentTerms" />
                <asp:BoundField DataField="PaymentRequiredDate" HeaderText="Payment Required Date" SortExpression="PaymentRequiredDate" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField DataField="TotalAmount" HeaderText="Total Value" SortExpression="TotalAmount" />
                <asp:BoundField DataField="AdvanceReceived" HeaderText="Adv Rcvd" SortExpression="AdvanceReceived" />
                <%--<asp:BoundField DataField="AdvanceAmount" HeaderText="Advance Amount" SortExpression="AdvanceAmount" />--%>
                <asp:BoundField DataField="Aging" HeaderText="Aging" SortExpression="Aging" />
                <asp:BoundField DataField="InvoiceUser" HeaderText="User" SortExpression="InvoiceUser" />
                <asp:BoundField DataField="CreatedDate" HeaderText="Date" SortExpression="CreatedDate" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" />
            </Columns>
            <PagerTemplate>
                <asp:GridViewPager runat="server" />
            </PagerTemplate>
        </asp:GridView>
        </fieldset>
        <div>
            <asp:SqlDataSource ID="InvoiceSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="AC_GetPendingInvoiceForPostingFinal" SelectCommandType="StoredProcedure">
            </asp:SqlDataSource>
        </div>
</asp:Content>


