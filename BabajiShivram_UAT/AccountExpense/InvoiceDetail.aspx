<%@ Page Title="Invoice Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="InvoiceDetail.aspx.cs" Inherits="AccountExpense_InvoiceDetail" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <cc1:toolkitscriptmanager id="ScriptManager1" runat="server" scriptmode="Release">
    </cc1:toolkitscriptmanager>
        <asp:UpdatePanel ID="upFillDetails" runat="server">
        <ContentTemplate>
            <div>
                <div align="center">
                    <asp:HiddenField ID="hdnJobId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnBranchId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnModuleId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnInvoiceType" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnBillType" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnProformaInvoiceId" runat="server" Value="0" />
                    
                    <asp:Label ID="lblError" runat="server" EnableViewState="false" Font-Size="Medium" ForeColor="Red"></asp:Label>
                    <asp:ValidationSummary ID="vsFillDetails" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
                </div>

                <fieldset>
                    <legend>Vendor Invoice Detail</legend>

                    <fieldset>
                    <legend>Job Detail</legend>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rblInvoiceMode" runat="server" RepeatDirection="Horizontal" Enabled="false">
                                    <asp:ListItem Text="Tax Invoice" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Credit Note" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Debit Note" Value="3"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td colspan="2">
                                <asp:LinkButton ID="lnkJobDetail" Text="View Job Detail" ForeColor="Blue" runat="server" OnClick="lnkJobDetail_Click"></asp:LinkButton>
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
                                    <asp:Label ID="lblLTRefNo" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                        </tr>
                        <tr>
                            <td>Customer
                            </td>
                            <td>
                                <asp:Label ID="lblCustomer" runat="server"></asp:Label>
                            </td>
                            <td>
                                Consignee
                            </td>
                            <td>
                                <asp:Label ID="lblConsignee" runat="server"></asp:Label>
                            </td>                         
                        </tr>
                        <tr>
                            <td>BE/SB No
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
                                <asp:Label ID="lblExpenseType" runat="server"></asp:Label>
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
                                Bank Name
                            </td>
                            <td>
                                <asp:Label ID="lblVendorBankName" runat="server"></asp:Label>
                            </td>
                            <td>
                                Account Name
                            </td>
                            <td>
                                <asp:Label ID="lblVendorBankAccountName" runat="server"></asp:Label>
                            </td
                        </tr>
                        <tr>                            
                            <td>
                                Account No
                            </td>
                            <td>
                                <asp:Label ID="lblVendorBankAccountNo" runat="server"></asp:Label>
                            </td>
                            
                            <td>
                                IFSC
                            </td>
                            <td>
                                <asp:Label ID="lblVendorBankAccountIFSC" runat="server"></asp:Label>
                            </td>
                        </tr>
                        
                    </table>
                    </fieldset>
                    <fieldset id="fldVendorBuySell" runat="server" visible="false">
                        <legend>Buy Sell Detail</legend>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="font-size:14px" bgcolor="white">
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
                                    <asp:Label ID="txtJobProfitRemark" runat="server" Width="300px" TextMode="MultiLine" MaxLength="200"></asp:Label>
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
                    <fieldset>
                    <legend>Invoice Detail</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                             <td>
                                Billing Party Name
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lblBillingPartyName" runat="server" ></asp:Label>
                            </td>
                            <%--<td>
                                Current O/S
                            </td>
                            <td>
                                <asp:Label ID="lblCurrentOutstanding" runat="server"></asp:Label>
                            </td>--%>
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
                        <tr>
                             <td>
                                RIM/NON RIM
                            </td>
                            <td>
                                <asp:Label ID="lblRIM" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                Invoice Type
                            </td>
                            <td>
                                <asp:Label ID="lblInvoiceType" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
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
                                Invoice Currency
                            </td>
                            <td>
                                <asp:Label ID="lblInvoiceCurrency" runat="server"></asp:Label>
                            </td>
                            <td>
                                Exhange Rate
                            </td>
                            <td>
                                <asp:Label ID="lblExchangeRate" runat="server"></asp:Label>
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
                                Paid Amount
                            </td>
                            <td>
                                <asp:Label ID="lblPaidAmount" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
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
                                JB No
                            </td>
                            <td>
                                <asp:Label ID="lblJBNo" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                Request By
                            </td>
                            <td>
                                <asp:Label ID="lblRequestBy" runat="server"></asp:Label>
                            </td>
                        </tr>
                     </table>
                    </fieldset>
                    <fieldset id="fldProfrmaInvoice" runat="server" visible="false">
                    <legend>Proforma Invoice Detail</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                             <td>
                                Billing GSTN
                            </td>
                            <td>
                                <asp:Label ID="lblProformaBillingGSTN" runat="server"></asp:Label>
                            </td>
                            <td>
                                Billing PAN
                            </td>
                            <td>
                                <asp:Label ID="lblProformaBillingPAN" runat="server"></asp:Label>
                            </td>
                            
                        </tr>
                        <tr>
                             <td>
                                Billing Party Name
                            </td>
                            <td>
                                <asp:Label ID="lblProformaBillingPartyName" runat="server"></asp:Label>
                            </td>
                            <td>
                                Current O/S
                            </td>
                            <td>
                                <asp:Label ID="lblProformaCurrentOutstanding" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                             <td>
                                Payment Due Date
                            </td>
                            <td>
                                <asp:Label ID="lblProformaPaymentDueDate" runat="server"></asp:Label>
                            </td>
                            <td>
                                Patyment Request Date
                            </td>
                            <td>
                                <asp:Label ID="lblProformaPatymentRequestDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                             <td>
                                RIM/NON RIM
                            </td>
                            <td>
                                <asp:Label ID="lblProformaRIM" runat="server"></asp:Label>
                            </td>
                            <td>
                                Invoice Type
                            </td>
                            <td>
                                <asp:Label ID="lblProformaInvoiceType" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                             <td>
                                Invoice No
                            </td>
                            <td>
                                <asp:Label ID="lblProformaInvoiceNo" runat="server"></asp:Label>
                            </td>
                            <td>
                                Invoice Date
                            </td>
                            <td>
                                <asp:Label ID="lblProformaInvoiceDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                Total Invoice Value
                            </td>
                            <td>
                                <asp:Label ID="lblProformaTotalInvoiceValue" runat="server"></asp:Label>
                            </td>
                            <td>
                                Taxable Value
                            </td>
                            <td>
                                <asp:Label ID="lblProformaTaxableValue" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Invoice Currency
                            </td>
                            <td>
                                <asp:Label ID="lblProformaInvoiceCurrency" runat="server"></asp:Label>
                            </td>
                            <td>
                                Exhange Rate
                            </td>
                            <td>
                                <asp:Label ID="lblProformaExchangeRate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                GST Amount
                            </td>
                            <td>
                                <asp:Label ID="lblProformaGSTValue" runat="server"></asp:Label>
                            </td>
                            <td>
                                Paid Amount
                            </td>
                            <td>
                                <asp:Label ID="lblProformaPaidAmount" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Advance Received
                            </td>
                            <td>
                                <asp:Label ID="lblProformaAdvanceReceived" runat="server"></asp:Label>
                            </td>
                            <td>
                                Advance Amount
                            </td>
                            <td>
                                <asp:Label ID="lblProformaAdvanceAmount" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Request Remark
                            </td>
                            <td>
                                <asp:Label ID="lblProformaRequestRemark" runat="server"></asp:Label>
                            </td>
                            <td>
                                Request By
                            </td>
                            <td>
                                <asp:Label ID="lblProformRequestBy" runat="server"></asp:Label>
                            </td>
                        </tr>
                     </table>

                    <fieldset id="fldProformaPayment" runat="server" visible="false">
                    <legend>Proforma Invoice Payment History</legend>
                        <asp:GridView ID="gvProformaPaymentHistory" runat="server" AutoGenerateColumns="False"
                            CssClass="table" Width="99%" PagerStyle-CssClass="pgr"
                            DataKeyNames="lId" DataSourceID="DataSourceProformaPaymentHistory" CellPadding="4"
                            AllowPaging="True" AllowSorting="True" PageSize="40">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Payment Type" DataField="PaymentType" />
                                <asp:BoundField HeaderText="Bank Name" DataField="BankName" />
                                <asp:BoundField HeaderText="Amount" DataField="PaidAmount" />
                                <asp:BoundField HeaderText="Instrument No" DataField="InstrumentNo" />
                                <asp:BoundField HeaderText="Instrument Date" DataField="InstrumentDate" DataFormatString="{0:dd/MM/yyyy}"/>
                                <asp:BoundField HeaderText="Remark" DataField="Remark" />
                                <asp:BoundField HeaderText="User" DataField="UserName" />
                                <asp:BoundField HeaderText="Date" DataField="CreatedDate" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" />
                                </Columns>
                        </asp:GridView>
                </fieldset>
                    </fieldset>
                </fieldset>
                
                <fieldset id="fldInvoiceitem" runat="server" visible="true">
                    <div class="clear">
                    </div>
                        <legend>Charge Detail</legend>
                        Transaction Type <asp:Label ID="lblTransactionTypeName" runat="server"></asp:Label><br />
                        <asp:CheckBox ID="chkNoITC" runat="server" Text="No ITC" Enabled="false" />
                        <div class="clear"></div>
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
                
                <fieldset runat="server" id="fldTDSItem">
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


                <fieldset id="fldPaymentHistory" runat="server">
                    <legend>Payment Detail</legend>
                        <asp:GridView ID="gvPaymentHistory" runat="server" AutoGenerateColumns="False"
                            CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" 
                            DataSourceID="DataSourcePaymentHistory" CellPadding="4" OnRowCommand="gvPaymentHistory_RowCommand"
                            AllowPaging="True" AllowSorting="True" PageSize="40" OnRowDataBound="gvPaymentHistory_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Pay Type" DataField="PaymentType" />
                                <asp:BoundField HeaderText="Bank Name" DataField="BankName" />
                                <asp:BoundField HeaderText="Amount" DataField="PaidAmount" />
                                <asp:BoundField HeaderText="UTR No" DataField="InstrumentNo" />
                                <asp:BoundField HeaderText="UTR Date" DataField="InstrumentDate" DataFormatString="{0:dd/MM/yyyy}"/>
                                <asp:BoundField HeaderText="BJV M1" DataField="BJVPaymentNo" />
                                <asp:BoundField HeaderText="Paid From" DataField="FundTransFrom" />
                                <asp:BoundField HeaderText="Status" DataField="RespStatus" />
                                <asp:BoundField HeaderText="BJV Flag" DataField="PaymentFlagStatus" />
                                <%--<asp:BoundField HeaderText="Remark" DataField="Remark" />--%>
                                <asp:BoundField HeaderText="User" DataField="UserName" />
                                <asp:BoundField HeaderText="Date" DataField="updDate" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkSendEmail" runat="server" Text="Send Email" CommandName="SendPaymentEmail"  CommandArgument='<%#Eval("lid") %>'
                                         OnClientClick="return confirm('Are you sure to Send Email?');"   ></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                </Columns>
                        </asp:GridView>
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
            AllowPaging="True" AllowSorting="True" PageSize="40">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Status" DataField="StatusName" />
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
                            <asp:BoundField DataField="InvoiceMode" HeaderText="Type" SortExpression="InvoiceMode"/>
                            <%--<asp:BoundField DataField="PaymentTypeName" HeaderText="Payment Type" SortExpression="PaymentTypeName"/>--%>
                            <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName"/>
                            <asp:BoundField DataField="ExpenseTypeName" HeaderText="Expense Type" SortExpression="ExpenseTypeName"/>
                            <asp:BoundField DataField="TotalAmount" HeaderText="Total Amt" SortExpression="TotalAmount" />
                            <asp:BoundField DataField="PaidAmount" HeaderText="Paid Amt" SortExpression="PaidAmount" />
                            <asp:BoundField DataField="VendorName" HeaderText="Paid To" SortExpression="VendorName" ReadOnly="true"/>
                            <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy"/>
                            <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy}" />
                        </Columns>
                    </asp:GridView>
                </fieldset>
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
                        <asp:SessionParameter Name="InvoiceID" SessionField="InvoiceIdTrack" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourcePaymentHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetInvoicePaymentDetail" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="InvoiceID" SessionField="InvoiceIdTrack" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceProformaPaymentHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetInvoicePayment" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:Parameter Name="InvoiceID" DefaultValue="0" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourcePaymentDoument" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetInvoiceDocument" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="InvoiceID" SessionField="InvoiceIdTrack" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceInvoiceHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetInvoiceHistory" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="InvoiceID" SessionField="InvoiceIdTrack" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceJobHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetInvoiceJobPrevHistory" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="InvoiceID" SessionField="InvoiceIdTrack" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
    </ContentTemplate>
            </asp:UpdatePanel>
</asp:Content>

