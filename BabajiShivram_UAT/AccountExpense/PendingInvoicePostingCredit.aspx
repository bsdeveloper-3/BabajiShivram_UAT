<%@ Page Title="Audit - Credit Payment" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="PendingInvoicePostingCredit.aspx.cs" Inherits="AccountExpense_PendingInvoicePostingCredit" EnableEventValidation="false" Culture="en-GB"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <meta http-equiv="refresh" content="180">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <fieldset><legend>Audit - Credit Payment</legend>
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
                <asp:TemplateField HeaderText="View BJV" SortExpression="FARefNo">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBJVNo" CommandName="ViewBJV" runat="server" Text="View BJV"
                            CommandArgument='<%#Eval("FARefNo") %>' ToolTip="Check BJV Detail" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="BilledStatus" HeaderText="Billed ?" SortExpression="BilledStatus" />
                <asp:BoundField DataField="ExpenseTypeName" HeaderText="Expense Type" SortExpression="ExpenseTypeName"/>
                <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" SortExpression="InvoiceNo"/>
                <asp:BoundField DataField="InvoiceModeName" HeaderText="Type" SortExpression="InvoiceModeName" />
                <asp:BoundField DataField="InvoiceTypeName" HeaderText="Invoice Type" SortExpression="InvoiceTypeName" />
                <asp:BoundField DataField="StatusName" HeaderText="Status"  SortExpression="StatusName"/>
                <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                <%--<asp:BoundField DataField="Consignee" HeaderText="Consignee" SortExpression="Consignee" />--%>
                <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" SortExpression="VendorName" />
                <%--<asp:BoundField DataField="VendorGSTNo" HeaderText="Supplier GSTIN No" SortExpression="VendorGSTNo" />--%>
                <asp:BoundField DataField="PaymentTerms" HeaderText="Pay Terms Days" SortExpression="PaymentTerms" />
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
        <div id="BJV_Popup">
            <asp:LinkButton ID="lnkModelPopup21" runat="server" />
                <cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="lnkModelPopup21"
                    PopupControlID="Panel21">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="Panel21" Style="display: none" runat="server">
                    <fieldset>
                            <legend>BJV DETAIL</legend>
                            <div class="header">
                                <div class="fright">
                                    <asp:ImageButton ID="ImageButton1" ImageUrl="~/Images/delete.gif" runat="server"
                                        OnClick="btnCancelBJVPopup_Click" ToolTip="Close" />
                                </div>
                            </div>
                            <div class="clear"></div>
                            <div id="Div3" runat="server" style="max-height: 560px; overflow: auto;">
                                <asp:GridView ID="gvBJVDetail" runat="server" CssClass="table" AutoGenerateColumns="false" AllowPaging="false"
                                    OnPreRender="gvBJVDetail_PreRender">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex +1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Ref" HeaderText="Job No" />
                                        <asp:BoundField DataField="Type" HeaderText="Type" />
                                        <asp:BoundField DataField="BookName" HeaderText="Book Name" />
                                        <asp:BoundField DataField="billno" HeaderText="Bill No" />
                                        <asp:BoundField DataField="billdate" HeaderText="Bill Date" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="par_code" HeaderText="Party Code" />
                                        <asp:BoundField DataField="Debit" HeaderText="Debit" />
                                        <asp:BoundField DataField="Credit" HeaderText="Credit" />
                                        <asp:TemplateField HeaderText="Narration">
                                        <ItemTemplate>
                                            <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                            <asp:Label ID="lblNarration" runat="server" 
                                               Text='<%# Eval("narration")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                </asp:Panel>
        </div>
        <div>
            <asp:SqlDataSource ID="InvoiceSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="AC_GetPendingInvoiceForPosting" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Name="IsImmediatePayment" DefaultValue="false" DbType="Boolean" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
</asp:Content>

