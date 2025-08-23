<%@ Page Title="BSCCPL Mgmt Approval" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PendingTransMgmt.aspx.cs" 
   Inherits="BSCCPLTransport_PendingTransMgmt" EnableEventValidation="false" Culture="en-GB" %>
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
        <fieldset><legend>BSCCPL - Transporter Payment Approval</legend>
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
            Width="100%" OnRowCommand="gvDetail_RowCommand">
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
                <asp:TemplateField HeaderText="View BJV" SortExpression="FARefNo">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBJVNo" CommandName="ViewBJV" runat="server" Text="View BJV"
                            CommandArgument='<%#Eval("JobRefNo") %>' ToolTip="Check BJV Detail" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName"/> 
                <asp:BoundField DataField="CompanyName" HeaderText="Bill To" SortExpression="CompanyName" />
                <asp:BoundField DataField="ExpenseTypeName" HeaderText="Expense Type" SortExpression="ExpenseTypeName"/>
                <asp:BoundField DataField="CustName" HeaderText="Customer" SortExpression="Customer" />
                <asp:BoundField DataField="PaidTo" HeaderText="Transporter" SortExpression="PaidTo" />
                <asp:BoundField DataField="PaymentRequiredDate" HeaderText="Payment Required Date" SortExpression="PaymentRequiredDate" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="Amount" HeaderText="Total Value" SortExpression="TotalAmount" />
                <asp:BoundField DataField="ProfitLossPercent" HeaderText="P&L %" SortExpression="ProfitLossPercent" />
                <asp:BoundField DataField="PROFIT" HeaderText="Is Profit" SortExpression="PROFIT" />
                <asp:BoundField DataField="SellValue" HeaderText="Sell Value" SortExpression="SellValue" />
                <%--<asp:BoundField DataField="AdvanceAmt" HeaderText="Advance Amount" SortExpression="AdvanceAmt" />--%>
                <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" />
                <asp:BoundField DataField="RequestBy" HeaderText="User" SortExpression="RequestBy" />
                <asp:BoundField DataField="updDate" HeaderText="Date" SortExpression="updDate" DataFormatString="{0:dd/MM/yyyy}"/>
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
                    <legend>NBCPL BJV DETAIL</legend>
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
                SelectCommand="TRS_GetPendingInvoiceForApproval" SelectCommandType="StoredProcedure">
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


