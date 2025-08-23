<%@ Page Title="Vendor Batch Payment" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="PendingInvoiceBatchPay.aspx.cs" Inherits="AccountExpense_PendingInvoiceBatchPay" EnableEventValidation="false"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upFillDetails" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        </div>
        <asp:UpdatePanel ID="upFillDetails" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                
            <div align="center">
                <asp:Label ID="lblMessage" runat="server" CssClass="errorMsg" Font-Size="Large"></asp:Label>
            </div>
            <fieldset><legend>Vendor Batch Payment</legend>
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
            <asp:GridView ID="gvDetail" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="lid"
                AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataSourceID="InvoiceSqlDataSource" 
                AllowPaging="True" AllowSorting="True" PageSize="40" PagerSettings-Position="TopAndBottom"
                Width="100%" OnRowCommand="gvDetail_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Job No">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkJobNo" CommandName="select" runat="server" Text='<%#Eval("FARefNo") %>'
                            CommandArgument='<%#Bind("InvoiceMemoId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>                
                <asp:BoundField DataField="FARefNo" HeaderText="Job No" SortExpression="FARefNo" Visible="false"/>
                <asp:BoundField DataField="InvoiceMemoRefNo" HeaderText="Memo Ref No" SortExpression="InvoiceMemoRefNo" />
                <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" SortExpression="InvoiceNo" />
                <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" SortExpression="InvoiceDate" DataFormatString="{0:dd/MM/yyyy}"/>
                <%--<asp:BoundField DataField="ExpenseTypeName" HeaderText="Expense Type" SortExpression="ExpenseTypeName" />--%>
                <asp:BoundField DataField="BJVJBNumber" HeaderText="JB" SortExpression="BJVJBNumber" />
                <asp:BoundField DataField="PaidAmount" HeaderText="Net Payable" SortExpression="PaidAmount" />
                <asp:BoundField DataField="VendorName" HeaderText="Vendor" SortExpression="VendorName" />
                <asp:BoundField DataField="InvoiceAmount" HeaderText="Total Value" SortExpression="InvoiceAmount" />
                <asp:BoundField DataField="TDSTotalAmount" HeaderText="TDS" SortExpression="TDSTotalAmount" />
                <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" />
                <asp:BoundField DataField="PaymentDate" HeaderText="Batch Date" SortExpression="PaymentDate" DataFormatString="{0:dd/MM/yyyy hh:mm tt}"/>
                <asp:TemplateField HeaderText="Cancel">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkCancel" CommandName="cancelpay" runat="server" Text="Cancel Pay"
                           OnClientClick="return confirm('Are you sure to Cancel Payment?')" CommandArgument='<%#Bind("InvoiceMemoId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>                
            </Columns>
            <PagerTemplate>
                <asp:GridViewPager runat="server" />
            </PagerTemplate>
        </asp:GridView>
        </fieldset>
        <div>
            <asp:SqlDataSource ID="InvoiceSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="INV_GetMemoPaymentBatch" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="lUser" SessionField="UserId" />
                    <asp:Parameter Name="lType" DefaultValue="1" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

