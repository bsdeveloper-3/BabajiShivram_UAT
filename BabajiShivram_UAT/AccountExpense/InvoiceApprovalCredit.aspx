<%@ Page Title="Approval Credit Vendor" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="InvoiceApprovalCredit.aspx.cs" Inherits="AccountExpense_InvoiceApprovalCredit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Scriptmanager id="ScriptManager1" runat="server" scriptmode="Release"> </asp:Scriptmanager>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upFillDetails" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
        <asp:UpdatePanel ID="upFillDetails" runat="server">
        <ContentTemplate>
            <div>
                <div align="center">
                    <asp:HiddenField ID="hdnJobId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnBranchId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnModuleId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnNewPaymentLid" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnBillType" runat="server" Value="0" />
                    <asp:Label ID="lblError" runat="server" Font-Bold="true" Font-Size="Medium" EnableViewState="false"></asp:Label>
                    <asp:ValidationSummary ID="vsFillDetails" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
                </div>

                <fieldset>
                    <legend>Invoice Payment Approval</legend>

                    <fieldset>
                    <legend>Job Detail</legend>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td colspan="4">
                                <asp:RadioButtonList ID="rblInvoiceMode" runat="server" RepeatDirection="Horizontal" Enabled="false">
                                    <asp:ListItem Text="Tax Invoice" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Credit Note" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Debit Note" Value="3"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>Job Number
                            </td>
                            <td>
                                <asp:Label ID="lblJobNumber" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>LT Ref No
                            </td>
                            <td>
                                <asp:Label ID="lblLTRefNo" Width="160px" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Customer
                            </td>
                            <td>
                                <asp:Label ID="lblCustomer" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                Consignee
                            </td>
                            <td>
                                <asp:Label ID="lblConsignee" runat="server"></asp:Label>
                            </td>                         
                        </tr>
                        <tr>
                            <td>BE No
                            </td>
                            <td>
                                <asp:Label ID="lblBENo" runat="server"></asp:Label>
                            </td>
                            <td>
                                BL No
                            </td>
                            <td>
                                <asp:Label ID="lblBLNo" runat="server"></asp:Label>
                            </td>                         
                        </tr>
                        <tr>
                            <td>Weight
                            </td>
                            <td>
                                <asp:Label ID="lblWeight" runat="server"></asp:Label>
                            </td>
                            <td>
                                Container Count
                            </td>
                            <td>
                                <asp:Label ID="lblContainerCount" runat="server"></asp:Label>
                            </td>                         
                        </tr>
                        <tr>
                            <td>
                                Assessable Value
                            </td>
                            <td>
                                <asp:Label ID="lblAssessableValue" runat="server"></asp:Label>
                            </td>
                            <td>
                                IGST Amount
                            </td>
                            <td>
                                <asp:Label ID="lblIGSTAmount" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Expense Type
                            </td>
                            <td>
                                <asp:Label ID="lblExpenseType" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                Duty Amount
                            </td>
                            <td>
                                <asp:Label ID="lblDutyAmount" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr id="tdInterestAmt" runat="server" visible="false">
                            <td>Interest</td>
                            <td>
                                <asp:Label ID="lblInterestAmount" runat="server"></asp:Label>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                    </fieldset>
                    <fieldset runat="server" id="fldVendor">
                    <legend>Vendor Detail</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>
                                Vendor Name
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lblVendorName" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Vendor GSTIN
                            </td>
                            <td>
                                <asp:Label ID="lblVendorGSTIN" runat="server"></asp:Label>
                            </td>
                            <td>
                                PAN
                            </td>
                            <td>
                                <asp:Label ID="lblPAN" runat="server"></asp:Label>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                Vendor Type
                            </td>
                            <td>
                                <asp:Label ID="lblVendorType" runat="server"></asp:Label>
                            </td>
                            <td>
                                Credit Terms (Days)
                            </td>
                            <td>
                                <asp:Label ID="lblCreditTerms" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                             <td>
                                Payment Due Date
                            </td>
                            <td>
                                <asp:Label ID="lblPaymentDueDate" runat="server"></asp:Label>
                            </td>
                            <td>
                                Payment Request Date
                            </td>
                            <td>
                                <asp:Label ID="lblPatymentRequestDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    </fieldset>
                    <fieldset id="fldVendorBuySell" runat="server" visible="false">
                        <legend>Buy Sell Detail</legend>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>
                                    Vendor Buy Value
                                </td>
                                <td>
                                    <asp:Label ID="txtVendorBuyValue" runat="server"></asp:Label>
                                </td>
                                <td>
                                    Vendor Sell Value
                                </td>
                                <td>
                                    <asp:Label ID="txtVendorSellValue" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Job Buy Value
                                </td>
                                <td>
                                    <asp:Label ID="txtCustomerBuyValue" runat="server"></asp:Label>
                                </td>
                                <td>
                                    Job Sell Value
                                </td>
                                <td>
                                    <asp:Label ID="txtCustomerSellValue" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Remark (Job Profit)
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="txtJobProfitRemark" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        
                    </fieldset>
                    <fieldset id="fldVendorJobMargin" runat="server" visible="false">
                        <legend>Margin against Vendor Invoice</legend>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>
                                    Invoice Taxable Value (INR)
                                </td>
                                <td>
                                    <asp:Label ID="lblInvoiceValue1" runat="server"></asp:Label>
                                </td>
                                <td>
                                    Selling Amount
                                </td>
                                <td>
                                    <asp:Label ID="lblSellingAmount1" runat="server" ></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Margin
                                </td>
                                <td>
                                    <asp:Label ID="lblMargin" runat="server"></asp:Label>
                                </td>
                                <td>
                                    % Margin
                                </td>
                                <td>
                                    <asp:Label ID="lblPercentMargin" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset id="fldOverAllJobMargin" runat="server" visible="false">
                        <legend>Overall Job Margins</legend>
                        <asp:LinkButton ID="lnkBJVNo" runat="server" Text="View BJV" OnClick="lnkBJVNo_Click" ToolTip="Check BJV Detail" />
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" style="font-size:14px" bgcolor="white">
                            <tr>
                                <td>
                                    Total Cost
                                </td>
                                <td>
                                    <asp:Label ID="lblTotalCost" runat="server"></asp:Label>
                                </td>
                                <td>
                                    Total Selling (Billed Amt)
                                </td>
                                <td>
                                    <asp:Label ID="lblBilledAmount" runat="server" ></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Margin
                                </td>
                                <td>
                                    <asp:Label ID="lblBillMargin" runat="server"></asp:Label>
                                </td>
                                <td>
                                    % Margin
                                </td>
                                <td>
                                    <asp:Label ID="lblBillMarginPercent" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    
                    <fieldset id="fldInvoiceitem" runat="server">
                    <div class="clear">
                    </div>
                        <legend>Charge Detail</legend>
                        Transaction Type <asp:Label ID="lblTransactionTypeName" runat="server"></asp:Label><br />
                        <asp:CheckBox ID="chkNoITC" runat="server" Text="No ITC" Enabled="false" />
                        <div class="clear"></div>
                            <div id="Div1" runat="server" style="max-height: 550px; overflow: auto;">
                                <asp:GridView ID="gvCharges" runat="server" CssClass="table" AutoGenerateColumns="false"
                                    AllowPaging="false" DataKeyNames="lid" DataSourceID="InvoiceDataSource" >
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex +1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="HSN" HeaderText="HSN" />
                                        <asp:BoundField DataField="ChargeName" HeaderText="Charge Name" />
                                        <asp:BoundField DataField="ChargeCode" HeaderText="Charge Code" />
                                        <asp:BoundField DataField="TaxAmount" HeaderText="Taxable Value" />
                                        <asp:BoundField DataField="TaxAmountINR" HeaderText="Taxable Value (INR)" DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="CurrencyName" HeaderText="Currency" />
                                        <asp:BoundField DataField="InvoiceCurrencyExchangeRate" HeaderText="Rate" />
                                        <asp:BoundField DataField="IGSTRate" HeaderText="IGST Rate" />
                                        <asp:BoundField DataField="IGSTAmount" HeaderText="IGST Amt" />
                                        <asp:BoundField DataField="CGSTRate" HeaderText="CGST Rate" />
                                        <asp:BoundField DataField="CGSTAmount" HeaderText="CGST Amt" />
                                        <asp:BoundField DataField="SGSTRate" HeaderText="SGST Rate" />
                                        <asp:BoundField DataField="SGSTAmount" HeaderText="SGST Amt" />
                                        <asp:BoundField DataField="Amount" HeaderText="Total Value" />
                                        <%--<asp:BoundField DataField="Remark" HeaderText="Remark" />--%>
                                    </Columns>
                                </asp:GridView>
                            </div>
                    </fieldset>
                    
                    <fieldset runat="server" id="fldTDSItem" visible="false">
                    <legend>TDS</legend>
                    
                    TDS Applicable: <b><asp:Label ID="lblTDSApplicable" Text="No" runat="server"></asp:Label></b>&nbsp;&nbsp;
                    TDS Rate Type:<b> <asp:Label ID="lblTDSRateType" runat="server"></asp:Label></b>&nbsp;&nbsp;
                    TDS Rate:<b> <asp:Label ID="lblTDSRate" runat="server"></asp:Label></b>&nbsp;&nbsp;
                    
                    Total TDS: &nbsp;&nbsp; <b><asp:Label ID="lblTotalTDS" runat="server"></asp:Label></b>
                        
                    <div class="clear"></div>
                    <div id="DivTDS2" runat="server" style="max-height: 550px; overflow: auto;">
                        <asp:GridView ID="GridViewTDS" runat="server" CssClass="table" AutoGenerateColumns="false"
                            AllowPaging="false" DataKeyNames="lid" DataSourceID="InvoiceDataSource" Visible="false">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField  DataField="TDSLedgerCode" HeaderText="TDS Ledger Code" />
                                <asp:BoundField  DataField="TDSRateTypeName" HeaderText="Rate Type" />
                                <asp:BoundField  DataField="NetTaxableValue" HeaderText="Net Taxable value" />
                                <asp:BoundField  DataField="TDSRate" HeaderText="TDS Rate" />
                                <asp:BoundField  DataField="TDSAmount" HeaderText="TDS Amount" />
                                <asp:BoundField  DataField="NetTDSPayable" HeaderText="Net Payable" />
                            </Columns>
                        </asp:GridView>
                    </div>
                    
                </fieldset>

                    <fieldset runat="server" id="fldRCMItem" visible="false">
                    <legend>RCM</legend>
                    RCM Applicable &nbsp;&nbsp; <b><asp:Label ID="lblRCMYes" runat="server" Text="No" ></asp:Label></b>
                    RCM Rate &nbsp;&nbsp;<b><asp:Label ID="lblRCMRate" runat="server"></asp:Label></b>
                    GST : <b><asp:Label ID="lblRCMGstType" runat="server">  </asp:Label></b>
                    RCM Total Amount &nbsp;&nbsp;<b><asp:Label ID="lblRCMTotalAmount" runat="server"></asp:Label></b>
                    <div class="clear">
                    </div>
                    <div class="clear"></div>
                    <div id="DivRCM2" runat="server" style="max-height: 550px; overflow: auto;">
                        <asp:GridView ID="GridViewRCM" runat="server" CssClass="table" AutoGenerateColumns="false"
                            AllowPaging="false" DataKeyNames="lid" DataSourceID="InvoiceDataSource"  Visible="false">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField  DataField="ChargeName" HeaderText="Charge Name" />
                                <asp:BoundField  DataField="NetTaxableValue" HeaderText="Net Taxable value" />
                                <asp:BoundField  DataField="RCMRate" HeaderText="RCM Rate" />
                                <asp:BoundField  DataField="RCMAmount" HeaderText="RCM Amount" />
                                <asp:BoundField  DataField="RCMIGSTAmt" HeaderText="IGST" />
                                <asp:BoundField  DataField="RCMCGSTAmt" HeaderText="CGST" />
                                <asp:BoundField  DataField="RCMSGSTAmt" HeaderText="SGST" />
                                <asp:BoundField  DataField="NetRCMPayable" HeaderText="Liability payable under RCM" />
                                </Columns>
                        </asp:GridView>
                    </div>
                </fieldset>

                    <fieldset runat="server" id="fldPayment" visible="false">
                    <legend>Payment Detail</legend>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        Net Payable Amount Rs. <asp:Label ID="lblNetPayable" runat="server"></asp:Label>
                    <div class="clear">
                    </div>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>
                                Payment
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblPayment" runat="server" RepeatDirection="Horizontal" 
                                    Enabled="false">
                                    <asp:ListItem Text="Full Payment" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Partial Payment" Value="0"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                Amount
                            </td>
                            <td>
                                <asp:Label ID="txtPayAmount" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Payment Mode
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPaymentType" runat="server" AppendDataBoundItems="true" DataSourceID="DataSourcePaymentType"
                                    DataTextField="sName" DataValueField="lid" Enabled="false">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Bank Name
                            </td>                        
                            <td>
                                <asp:DropDownList ID="ddBabajiBankName" runat="server" Enabled="false"></asp:DropDownList>

                            </td>
                        </tr>
                        <tr>
                            <td>
                                Bank Account/Cash Book
                            </td>
                            <td>
                                <asp:DropDownList ID="ddBabajiBankAccount" runat="server" Enabled="false">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Fund Transfer From Live Tracking ?
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblFundTransferFromLiveTracking" runat="server" Enabled="false"
                                    RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                    <fieldset>
                    <legend>Invoice Document</legend>
                    <asp:GridView ID="gvDocument" runat="server" CssClass="table" AutoGenerateColumns="false"
                        PagerStyle-CssClass="pgr" DataKeyNames="InvoiceID" AllowPaging="True" AllowSorting="True" PageSize="20"
                        DataSourceID="DataSourcePaymentDoument" OnRowCommand="gvDocument_RowCommand" PagerSettings-Position="TopAndBottom" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1%>
                                </ItemTemplate>
                            </asp:TemplateField>                                 
                            <asp:BoundField DataField="DocumentName" HeaderText="Document Name" SortExpression="DocumentName"/>
                            <asp:BoundField DataField="FileName" HeaderText="File Name" SortExpression="FileName"/>
                            <asp:TemplateField HeaderText="Download">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                        CommandArgument='<%#Eval("lid") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="View">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkView" runat="server" Text="View" CommandName="View" 
                                        CommandArgument='<%#Eval("lid") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView> 
                </fieldset>
                    <fieldset>
                    <legend>Invoice History</legend>    
                    <asp:GridView ID="gvInvoiceHistory" runat="server" AutoGenerateColumns="False"
                        CssClass="table" Width="99%" PagerStyle-CssClass="pgr"
                        DataKeyNames="lId" DataSourceID="DataSourceInvoiceHistory" CellPadding="4"
                        AllowPaging="True" AllowSorting="True" PageSize="40" OnRowDataBound="gvInvoiceHistory_RowDataBound">
                        <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Status" DataField="StatusName" />
                        <%--<asp:BoundField HeaderText="Remark" DataField="sRemark" />--%>
                        <asp:TemplateField HeaderText="Remark">
                            <ItemTemplate>
                                <div style="word-wrap: break-word; width: 400px; white-space:normal;">
                                <asp:Label ID="lblRemarkView" runat="server" Text='<%#Eval("sRemark") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="User" DataField="UserName" />
                        <asp:BoundField HeaderText="Date" DataField="CreatedDate" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" />
                        </Columns>
                        </asp:GridView>
                    </fieldset>
                    
                    <fieldset>
                    <legend>Previous Payment Request</legend>   
                    <asp:GridView ID="grdJobHistory" runat="server" CssClass="table" AutoGenerateColumns="false"
                        PagerStyle-CssClass="pgr" DataKeyNames="InvoiceID" AllowPaging="True" AllowSorting="True" PageSize="20"
                        DataSourceID="DataSourceJobHistory" PagerSettings-Position="TopAndBottom" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" SortExpression="InvoiceNo"/>
                            <%--<asp:BoundField DataField="FARefNo" HeaderText="BS Job No" SortExpression="FARefNo"/>--%>
                            <%--<asp:BoundField DataField="PaymentTypeName" HeaderText="Payment Type" SortExpression="PaymentTypeName"/>--%>
                            <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName"/>
                            <asp:BoundField DataField="ExpenseTypeName" HeaderText="Expense Type" SortExpression="ExpenseTypeName"/>
                            <asp:BoundField DataField="TotalAmount" HeaderText="Total Amt" SortExpression="TotalAmount" />
                            <asp:BoundField DataField="PaidAmount" HeaderText="Paid Amt" SortExpression="PaidAmount" />
                            <asp:BoundField DataField="VendorName" HeaderText="Paid To" SortExpression="VendorName" ReadOnly="true"/>
                            <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy"/>
                            <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss tt}" />
                        </Columns>
                    </asp:GridView>
                    </fieldset>
                    <fieldset>
                    <legend>Invoice Detail</legend>
                        <div class="m clear">
                        <asp:Button ID="btnSubmit" Text="Approve" OnClick="btnSubmit_Click" runat="server" ValidationGroup="Required" />
                        <asp:Button ID="btnReject" Text="Reject" OnClick="btnReject_Click" runat="server" ValidationGroup="Required"/>
                        <asp:Button ID="btnHold" Text="Hold" OnClick="btnHold_Click" runat="server" ValidationGroup="Required" />
                        <asp:Button ID="btnBack" Text="Back" runat="server" CausesValidation="false" />
                    </div>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                             <td>
                                Billing Party Name
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lblBillingPartyName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                             <td>
                                Billing GSTN
                            </td>
                            <td>
                                <asp:Label ID="lblBillingGSTN" runat="server"></asp:Label>
                            </td>
                            <td>
                                Billing PAN
                            </td>
                            <td>
                                <asp:Label ID="lblBillingPAN" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                             <td>
                                RIM/NON RIM
                            </td>
                            <td>
                                <asp:Label ID="lblRIM" runat="server"></asp:Label>
                            </td>
                            <td>
                                Invoice Type
                            </td>
                            <td>
                                <asp:Label ID="lblInvoiceType" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                             <td>
                                Invoice No
                            </td>
                            <td>
                                <asp:Label ID="lblInvoiceNo" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                Invoice Date
                            </td>
                            <td>
                                <asp:Label ID="lblInvoiceDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Total Invoice Value
                            </td>
                            <td>
                                <asp:Label ID="lblTotalInvoiceValue" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                               &nbsp;&nbsp; 
                                <asp:Label ID="lblInvoiceCurrency" runat="server"></asp:Label>
                            </td>
                            <td>
                                Taxable Value
                            </td>
                            <td>
                                <asp:Label ID="lblTaxableValue" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                GST Amount
                            </td>
                            <td>
                                <asp:Label ID="lblGSTValue" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                Current O/S
                            </td>
                            <td>
                                <asp:Label ID="lblCurrentOutstanding" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Advance Received
                            </td>
                            <td>
                                <asp:Label ID="lblAdvanceReceived" runat="server"></asp:Label>
                            </td>
                            <td>
                                Advance Amount
                            </td>
                            <td>
                                <asp:Label ID="lblAdvanceAmount" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Request Remark
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lblRequestRemark" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Request By
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lblRequestBy" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                           <td>
                               Authorised Remark
                               <asp:RequiredFieldValidator ID="rfvRemark" runat="server" ValidationGroup="Required" InitialValue=""
                                   ControlToValidate="txtRemark" Text="Required" ErrorMessage="Please Enter Remark" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" MaxLength="200"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    </fieldset>
                    <fieldset>
                        <legend>Bill Detail</legend>
                        <asp:GridView ID="gvBillDetail" runat="server" AutoGenerateColumns="False"
                            CssClass="table" Width="99%" PagerStyle-CssClass="pgr"
                            DataKeyNames="JobId,Billid" DataSourceID="DataSourceBillJob" CellPadding="4"
                            AllowPaging="True" AllowSorting="True" PageSize="40" OnRowCommand="gvBillDetail_RowCommand"
                            OnRowDataBound="gvBillDetail_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BJV No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBJVNo" runat="server" Text='<%#Eval("BJVNo")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Bill Number">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBillNumber" runat="server" Text='<%#Eval("INVNO")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Bill Date" DataField="INVDATE" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField HeaderText="Bill Amount" DataField="INVAMOUNT" />
                                <asp:BoundField HeaderText="Adjustment Amount" DataField="ADJAmount" />
                                <asp:BoundField HeaderText="Adjustment Date" DataField="ADJDate" DataFormatString="{0:dd/MM/yyyy}"/>
                                <asp:TemplateField HeaderText="View Bill">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBillView" runat="server" Text="View" CommandName="View" CommandArgument='<%#Eval("BillId")%>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </fieldset>
                </fieldset>
            </div>
            <div>
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
                                <asp:GridView ID="gvBJVDetail" runat="server" CssClass="table" AutoGenerateColumns="false" AllowPaging="false">
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
                <div id="divDatasourc"> 
                <asp:SqlDataSource ID="InvoiceDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetInvoiceItem" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="InvoiceID" SessionField="InvoiceID" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourcePaymentDoument" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetInvoiceDocument" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="InvoiceID" SessionField="InvoiceID" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceInvoiceHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetInvoiceHistory" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="InvoiceID" SessionField="InvoiceID" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceJobHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetInvoiceJobPrevHistory" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="InvoiceID" SessionField="InvoiceID" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourcePaymentType" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetPaymentTypeMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceBillJob" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="BL_GetPendingBillDetail" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hdnJobId" Name="JobId" PropertyName="Value" ConvertEmptyStringToNull="true"  />
                        <asp:Parameter Name="ModuleId" DefaultValue="1"/>
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
    </ContentTemplate>
            </asp:UpdatePanel>
</asp:Content>

