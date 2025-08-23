<%@ Page Title="Audit L2 - Credit Vendor" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PendingInvoiceAuditCreditL2.aspx.cs" 
        Inherits="AccountExpense_PendingInvoiceAuditCreditL2" EnableEventValidation="false" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <meta http-equiv="refresh" content="120">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <asp:UpdatePanel ID="upFillDetails" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
    <fieldset><legend>Invoice Audit - L2</legend>
        <div align="left">
            <asp:Label ID="lblMessage" runat="server" CssClass="errorMsg" EnableViewState="false" ></asp:Label>
            <asp:HiddenField ID="hdnInvoiceId" runat="server" />
            <asp:HiddenField ID="hdnOperationType" runat="server" />
            <asp:HiddenField ID="hdnBillType" runat="server" Value="0" />
            <asp:HiddenField ID="hdnStatusId" runat="server" Value="0" />
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
                <asp:TemplateField HeaderText="" ItemStyle-Width="8%">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnInvoiceApprove" runat="server" ImageUrl="~/Images/success.png" OnClientClick="return confirm('Are you sure to Complete Audit L2?');"
                            CommandArgument='<%#Eval("InvoiceId") %>' CommandName="ApproveInvoice" Width="17px" Height="17px" ToolTip="Invoice Audit L2."
                            Style="padding-right: 1px; margin-right: 0px; padding-left: 1px"></asp:ImageButton>
                        <asp:ImageButton ID="btnInvoiceReject" runat="server" ImageUrl="~/Images/Reject.jpg" OnClientClick="return confirm('Are you sure to reject Invoice?');"
                            CommandArgument='<%#Eval("InvoiceId") %>' CommandName="RejectInvoice" Width="18px" Height="18px" ToolTip="Reject Invoice."
                            Style="padding-right: 1px; margin-right: 0px; padding-left: 1px"></asp:ImageButton>
                        <asp:ImageButton ID="btnPaymentHold" runat="server" ImageUrl="~/Images/hold.png" OnClientClick="return confirm('Are you sure to Hold Payment?');"
                            CommandArgument='<%#Eval("InvoiceId") %>' CommandName="PaymentHold" Width="18px" Height="18px" ToolTip="Hold Payment"
                            Style="padding-right: 1px; margin-right: 0px; padding-left: 1px"></asp:ImageButton>
                        <asp:ImageButton ID="imgbtnJobDetail" runat="server" ImageUrl="~/Images/history.png"
                            CommandArgument='<%#Eval("InvoiceId") %>' CommandName="History" Width="18px" Height="18px" ToolTip="Payment History."
                                Style="padding-right: 1px; margin-right: 0px; padding-left: 1px"></asp:ImageButton>       
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
                <asp:BoundField DataField="ExpenseTypeName" HeaderText="Type" />
                <asp:BoundField DataField="PaymentType" HeaderText="Payment Type" SortExpression="PaymentType" />
                <asp:BoundField DataField="BankName" HeaderText="Bank Name" SortExpression="BankName" />
                <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" />
                <asp:BoundField DataField="StatusName" HeaderText="Status" />
                <asp:BoundField DataField="ImmediatePayment" HeaderText="Immediate Payment" SortExpression="ImmediatePayment"/>
                <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                <%--<asp:BoundField DataField="Consignee" HeaderText="Consignee" SortExpression="Consignee" />--%>
                <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" SortExpression="VendorName" />
                <asp:BoundField DataField="PaymentTerms" HeaderText="Vendor Pay Terms Days" SortExpression="PaymentTerms" />
                <asp:BoundField DataField="ImmediatePayment" HeaderText="Immediate Payment" SortExpression="ImmediatePayment"/>
                <asp:BoundField DataField="PaymentRequiredDate" HeaderText="Payment Required Date" SortExpression="PaymentRequiredDate" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField DataField="InvoiceAmount" HeaderText="Total Value" SortExpression="InvoiceAmount" />
                <asp:BoundField DataField="CurrencyName" HeaderText="Currency" SortExpression="CurrencyName" />
                <asp:BoundField DataField="InvoiceCurrencyExchangeRate" HeaderText="Ex Rate" SortExpression="InvoiceCurrencyExchangeRate" />
                <asp:BoundField DataField="InvoiceCurrencyAmt" HeaderText="Total Value (INR)" SortExpression="InvoiceCurrencyAmt" />
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

        <%--  START : MODAL POP-UP FOR Approve Invoice- 1/ Reject Invoice for Audit - 2/Hold Payment -3/ Approve Payment -4/   --%>

            <div>
                <asp:LinkButton ID="lnkRejectExp" runat="server"></asp:LinkButton>
            </div>

            <div id="divOpExpense">
                <cc1:ModalPopupExtender ID="RejectModalPopupExtender" runat="server" CacheDynamicResults="false" PopupControlID="panRejectJob" 
                    CancelControlID="imgbtnRejectExp" TargetControlID="lnkRejectExp" BackgroundCssClass="modalBackground" DropShadow="true">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="panRejectJob" runat="server" CssClass="ModalPopupPanel" Width="600px">
                    <div class="header">
                        <div class="fleft">
                            <asp:Label ID="lblPopupName" runat="server"></asp:Label>

                        </div>
                        <div class="fright">
                            <asp:ImageButton ID="imgbtnRejectExp" ImageUrl="../Images/delete.gif" runat="server"
                                ToolTip="Close" />
                        </div>
                    </div>
                    <div id="Div2" runat="server" style="max-height: 250px; overflow: auto; padding: 5px">
                        <div>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <div class="m">
                                    <center>
                                        <asp:Label ID="lblError_RejectExp" runat="server"></asp:Label>
                                    </center>
                                </div>
                                <tr>
                                    <td>BS Job Number</td>
                                    <td>
                                        <asp:Label ID="lblJobNumber" runat="server"></asp:Label>
                                    </td>
                                    <td>Branch</td>
                                    <td>
                                        <asp:Label ID="lblBranch1" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Payment Type</td>
                                    <td>
                                        <asp:Label ID="lblPaymentType1" runat="server"></asp:Label>
                                    </td>
                                    <td>Expense Type</td>
                                    <td>
                                        <asp:Label ID="lblexpenseType1" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Amount</td>
                                    <td>
                                        <asp:Label ID="lblAmount1" runat="server"></asp:Label>
                                    </td>
                                    <td>Paid To</td>
                                    <td>
                                        <asp:Label ID="lblPaidTo1" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Remark</td>
                                    <td>
                                        <asp:Label ID="lblRemark1" runat="server"></asp:Label>
                                    </td>
                                    <td>Created By</td>
                                    <td>
                                        <asp:Label ID="lblCreatedBy1" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <asp:RequiredFieldValidator ID="RFVReason" runat="server" ControlToValidate="txtReason" SetFocusOnError="true" Display="Dynamic"
                            ForeColor="Red" ErrorMessage="Enter Remark" ValidationGroup="ValidateExpense" Font-Bold="true"></asp:RequiredFieldValidator>
                        <div>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>Remark                                      
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" Rows="4" MaxLength="200" Width="300px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button ID="btnApproveInvoice" runat="server" Text="Approve Audit L2" OnClick="btnApproveInvoice_OnClick" Visible="false" ValidationGroup="ValidateExpense" />
                                        <asp:Button ID="btnRejectInvoice" runat="server" Text="Reject Invoice" OnClick="btnRejectInvoice_OnClick" Visible="false" ValidationGroup="ValidateExpense" />
                                        <asp:Button ID="btnHoldPayment" runat="server" Text="Hold Payment" OnClick="btnHoldPayment_OnClick" Visible="false" ValidationGroup="ValidateExpense" />
                                        <asp:Button ID="btnApprovePayment" runat="server" Text="Approve Payment" OnClick="btnApprovePayment_OnClick" Visible="false" ValidationGroup="ValidateExpense" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </asp:Panel>
            </div>
         <%--  START : MODAL POP-UP FOR Details/History Expense  --%>
            <div>
                <asp:LinkButton ID="lnkHistory" runat="server" Enabled="false"></asp:LinkButton>
            </div>

            <div id="divDetailsHistory">
                <cc1:ModalPopupExtender ID="ModalPopupDetailHistory" runat="server" CacheDynamicResults="false" PopupControlID="panJobDetails" 
                    CancelControlID="imgbtnJobDetails" TargetControlID="lnkHistory" BackgroundCssClass="modalBackground" DropShadow="true">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="panJobDetails" runat="server" CssClass="ModalPopupPanel" Width="900px">
                    <div class="header">
                        <div class="fleft">
                            Job Payment History
                        </div>
                        <div class="fright">
                            <asp:ImageButton ID="imgbtnJobDetails" ImageUrl="../Images/delete.gif" runat="server"
                                ToolTip="Close" />
                        </div>
                    </div>          
                    <div id="Div4" runat="server" style="max-height: 250px; overflow: auto; padding: 5px">
                        <div>
                            <asp:GridView ID="grdJobHistory" runat="server" CssClass="table" AutoGenerateColumns="false"
                                PagerStyle-CssClass="pgr" DataKeyNames="InvoiceID" AllowPaging="True" AllowSorting="True" PageSize="20"
                                PagerSettings-Position="TopAndBottom" Width="100%">
                                <%--DataSourceID="DataSourceDocuments">--%>
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                 
                                    <asp:BoundField DataField="FARefNo" HeaderText="BS Job No" SortExpression="FARefNo"/>
                                    <asp:BoundField DataField="ConsigneeName" HeaderText="Consignee" SortExpression="Consignee"/>
                                    <%--<asp:BoundField DataField="BranchName" HeaderText="Branch" SortExpression="BranchName"/>--%>
                                    <asp:BoundField DataField="PaymentTypeName" HeaderText="Payment Type" SortExpression="PaymentTypeName"/>
                                    <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName"/>
                                    <asp:BoundField DataField="ExpenseTypeName" HeaderText="Expense Type" SortExpression="ExpenseTypeName"/>
                                    <asp:BoundField DataField="TotalAmount" HeaderText="Total Amt" SortExpression="TotalAmount" />
                                    <asp:BoundField DataField="VendorName" HeaderText="Paid To" SortExpression="VendorName" ReadOnly="true"/>
                                     <%--<asp:TemplateField HeaderText="Remark">
                                        <ItemTemplate>
                                            <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                            <asp:Label ID="lblRemarkView" runat="server" Text='<%#Eval("StatusRemark") %>'></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy"/>
                                    <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss tt}" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>
            </div>

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
                                   OnPreRender="gvBJVDetail_PreRender" >
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
                SelectCommand="AC_GetInvoiceForAuditL2" SelectCommandType="StoredProcedure">
            </asp:SqlDataSource>
        </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>



