<%@ Page Title="Audit L1" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
        CodeFile="PendingTransL1.aspx.cs" Inherits="BSCCPLTransport_PendingTransL1" %>

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
        <asp:Label ID="lblMessage" runat="server" CssClass="errorMsg" EnableViewState="false"></asp:Label>
            <asp:HiddenField ID="hdnInvoiceId" runat="server" />
            <asp:HiddenField ID="hdnOperationType" runat="server" />
    </div>
        
        <fieldset><legend>Audit L1</legend>
        
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
        <asp:GridView ID="gvDetail" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="RequestId"
            AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataSourceID="InvoiceSqlDataSource" 
            AllowPaging="True" AllowSorting="True" PageSize="40" PagerSettings-Position="TopAndBottom"
            Width="100%" OnRowCommand="gvDetail_RowCommand" >
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Job No" SortExpression="FARefNo">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkJobRefNo" CommandName="select" runat="server" Text='<%#Eval("JobRefNo") %>'
                            CommandArgument='<%#Eval("RequestId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="JobRefNo" HeaderText="Job No" Visible="False" />
                <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" SortExpression="InvoiceNo" />
                <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" SortExpression="InvoiceDate" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName"/>
                <asp:BoundField DataField="ExpenseTypeName" HeaderText="Expense Type" />
                <asp:BoundField DataField="CustName" HeaderText="Customer" SortExpression="Customer" />
                <asp:BoundField DataField="PaidTo" HeaderText="Transporter" SortExpression="PaidTo" />
                <asp:BoundField DataField="Amount" HeaderText="Total Value" SortExpression="Amount" />
                <asp:BoundField DataField="ProfitLossPercent" HeaderText="P&L %" SortExpression="ProfitLossPercent" />
                <asp:BoundField DataField="PROFIT" HeaderText="Is Profit" SortExpression="PROFIT" />
                <asp:BoundField DataField="SellValue" HeaderText="Sell Value" SortExpression="SellValue" />
                <asp:BoundField DataField="AdvanceAmt" HeaderText="AdvAmt" SortExpression="AdvanceAmt" />
                <asp:BoundField DataField="PaymentRequiredDate" HeaderText="Pay Reqrd Date" SortExpression="PaymentRequiredDate" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="UserRemark" HeaderText="Remark" SortExpression="UserRemark" />
                <asp:BoundField DataField="RequestBy" HeaderText="User" SortExpression="RequestBy" />
                <asp:BoundField DataField="dtDate" HeaderText="Date" SortExpression="Date" DataFormatString="{0:dd/MM/yyyy}"/>
            </Columns>
            <PagerTemplate>
                <asp:GridViewPager runat="server" />
            </PagerTemplate>
        </asp:GridView>
        </fieldset>
           
        <div>
            <asp:SqlDataSource ID="InvoiceSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="TRS_GetPendingInvoiceForL1" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="UserId" SessionField="UserId" />
                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    <asp:Parameter Name="CompanyID" DefaultValue="12" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

