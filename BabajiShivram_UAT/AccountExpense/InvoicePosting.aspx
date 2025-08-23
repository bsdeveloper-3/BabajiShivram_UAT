<%@ Page Title="Invoice Audit L1" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="InvoicePosting.aspx.cs" Inherits="AccountExpense_InvoicePosting" Culture="en-GB"%>
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
    <script type="text/javascript">
        function OnChargeCodeSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=txtChargeCode.ClientID%>').value = results.Charge_Code;
            $get('<%=txtChargeName.ClientID%>').value = results.Charge_Name
            $get('<%=hdnChargeHSN.ClientID%>').value = results.HSN_Code
        }

        function OnChargeNameSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=txtChargeCode.ClientID%>').value = results.Charge_Code;
            $get('<%=txtChargeName.ClientID%>').value = results.Charge_Name
            $get('<%=hdnChargeHSN.ClientID%>').value = results.HSN_Code
        }

    </script>
        <asp:UpdatePanel ID="upFillDetails" runat="server">
        <ContentTemplate>
            <div>
                <div align="center">
                    <asp:HiddenField ID="hdnJobId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnBranchId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnModuleId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnStatusId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnExpenseTypeID" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnIsRim" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnInvoiceType" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnBillType" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnInvoiceMode" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnProformaInvoiceId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnNewPaymentLid" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnNetPayableAmount" runat="server" Value="0" />
                    <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                    <asp:ValidationSummary ID="vsFillDetails" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
                </div>

                <fieldset>
                    <legend>Invoice Audit L1</legend>
                    <div class="m clear">
                        <asp:Button ID="btnReject" Text="Reject" OnClick="btnReject_Click" ValidationGroup="Required" runat="server" />
                        <asp:Button ID="btnHold" Text="Hold" OnClick="btnHold_Click" ValidationGroup="Required" runat="server" />
                        <asp:Button ID="btnCancel" Text="Back" CausesValidation="false" runat="server" />
                    </div>
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
                            <td>
                                IEC No
                            </td>
                            <td>
                                <asp:Label ID="lblIECNo" runat="server"></asp:Label>
                            </td>
                            <td>
                                Port Code
                            </td>
                            <td>
                                <asp:Label ID="lblPortCode" runat="server"></asp:Label>
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
                    </fieldset>
                    <fieldset runat="server" id="fldVendor">
                    <legend>Vendor Detail</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
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
                                Vendor Name
                            </td>
                            <td>
                                <asp:Label ID="lblVendorName" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                Vendor Type
                            </td>
                            <td>
                                <asp:Label ID="lblVendorType" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Vendor Bank
                                 <asp:RequiredFieldValidator ID="RFVVendorBank" runat="server" ValidationGroup="RequiredAudit" InitialValue="0" Display="Dynamic"
                                ControlToValidate="ddVendorBank" Text="Required" ErrorMessage="Please Select Vendor Bank Account" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddVendorBank" runat="server" OnSelectedIndexChanged="ddVendorBank_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
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
                            </td>
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
                        <tr>
                            <td>Account Type</td>
                            <td>
                                <asp:Label ID="lblVendorBankAccountType" runat="server"></asp:Label>
                            </td>
                            <td>
                                Bank Remark
                            </td>
                            <td>
                                <asp:Label ID="lblVendorBankRemark" runat="server"></asp:Label>
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
                                    <asp:RequiredFieldValidator ID="RFVVendorBuy" runat="server" ControlToValidate="txtVendorBuyValue"
                                         ValidationGroup="InvoiceRequired" Text="Required" InitialValue=""></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtVendorBuyValue" runat="server" Width="125px" TextMode="Number" MaxLength="10"></asp:TextBox>
                                </td>
                                <td>
                                    Vendor Sell Value
                                    <asp:RequiredFieldValidator ID="RFVVendorSell" runat="server" ControlToValidate="txtVendorSellValue"
                                         ValidationGroup="InvoiceRequired" Text="Required" InitialValue=""></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtVendorSellValue" runat="server" Width="125px" TextMode="Number" MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Job Buy Value
                                    <asp:RequiredFieldValidator ID="RFVCustomerBuyValue" runat="server" ControlToValidate="txtCustomerBuyValue"
                                         ValidationGroup="InvoiceRequired" Text="Required" InitialValue=""></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCustomerBuyValue" runat="server" Width="125px" TextMode="Number" MaxLength="10"></asp:TextBox>
                                </td>
                                <td>
                                    Job Sell Value
                                    <asp:RequiredFieldValidator ID="RFVCustomerSellValue" runat="server" ControlToValidate="txtCustomerSellValue"
                                         ValidationGroup="InvoiceRequired" Text="Required" InitialValue=""></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCustomerSellValue" runat="server" Width="125px" TextMode="Number" MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Remark (Job Profit)
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtJobProfitRemark" runat="server" Width="300px" TextMode="MultiLine" MaxLength="200"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset>
                    <legend>Invoice Detail</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                             <td>
                                Billing GSTIN
                            </td>
                            <td>
                                <asp:Label ID="lblBillingGSTN" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="SteelBlue"></asp:Label>
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
                                Billing Party Name
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lblBillingPartyName" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
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
                                Payment Due Date
                            </td>
                            <td>
                                <asp:Label ID="lblPaymentDueDate" runat="server"></asp:Label>
                            </td>
                            <td>
                                Patyment Request Date
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
                                <asp:Label ID="lblRIM" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label>
                            </td>
                            <td>
                                Invoice Type
                            </td>
                            <td>
                                <asp:Label ID="lblInvoiceType" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                             <td>
                                Invoice No
                            </td>
                            <td>
                                <asp:Label ID="lblInvoiceNo" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="SteelBlue"></asp:Label>
                            </td>
                            <td>
                                Invoice Date
                            </td>
                            <td>
                                <asp:Label ID="lblInvoiceDate" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="SteelBlue"></asp:Label>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                Total Invoice Value
                            </td>
                            <td>
                                <asp:Label ID="lblTotalInvoiceValue" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label>
                            </td>
                            <td>
                                Taxable Value
                            </td>
                            <td>
                                <asp:Label ID="lblTaxableValue" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Invoice Currency
                            </td>
                            <td>
                                <asp:Label ID="lblInvoiceCurrency" runat="server" Text="INR"></asp:Label>
                            </td>
                            <td>
                                Exhange Rate
                            </td>
                            <td>
                                <asp:Label ID="lblExchangeRate" runat="server" Text="1"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                GST Amount
                            </td>
                            <td>
                                <asp:Label ID="lblGSTValue" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label>
                            </td>
                            <td>
                                Other Deduction
                            </td>
                            <td>
                                <asp:Label ID="lblOtherDeduction" runat="server"></asp:Label>
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
                                <asp:Label ID="lblAdvanceAmount" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label>
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
                               Reject/Hold Remark
                               <asp:RequiredFieldValidator ID="rfvRejectRemark" runat="server" ValidationGroup="Required" InitialValue="" Display="Dynamic"
                                   ControlToValidate="txtRejectRemark" Text="Required" ErrorMessage="Please Enter Remark" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtRejectRemark" runat="server" TextMode="MultiLine" MaxLength="200"></asp:TextBox>
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
                                <asp:Label ID="lblProformaBillingPartyName" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
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
                                <asp:Label ID="lblProformaRIM" runat="server"  Font-Bold="true" Font-Size="Large"></asp:Label>
                            </td>
                            <td>
                                Invoice Type
                            </td>
                            <td>
                                <asp:Label ID="lblProformaInvoiceType" runat="server"  Font-Bold="true" Font-Size="Large"></asp:Label>
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
                                <asp:Label ID="lblProformaGSTValue" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label>
                            </td>
                            <td>
                               <span style="Font-Size:Large; font-weight:bold;"> Paid Amount</span>
                            </td>
                            <td>
                                <asp:Label ID="lblProformaPaidAmount" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label>
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
                    
                    TDS Applicable: <b><asp:Label ID="lblProformaTDSApplicable" Text="No" runat="server"></asp:Label></b>&nbsp;&nbsp;
                    TDS Rate Type:<b> <asp:Label ID="lblProformaTDSRateType" runat="server"></asp:Label></b>&nbsp;&nbsp;
                    TDS Rate:<b> <asp:Label ID="lblTProformaDSRate" runat="server" Text="0"></asp:Label></b>&nbsp;&nbsp;
                    
                    Total TDS: &nbsp;Rs.&nbsp; <b><asp:Label ID="lblProformaTDSAmount" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label></b>
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
                                <asp:BoundField HeaderText="BankName" DataField="BankName" />
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
                </fieldset>

                <fieldset runat="server" id="fldInvoiceItem">
                    <div class="clear">
                    </div>
                        <legend>Charge Detail</legend>
                        Transaction Type : 
                        <asp:DropDownList ID="ddTransactionType" runat="server" OnSelectedIndexChanged="ddTransactionType_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="--Transaction Type-" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Not Applicable" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Exempt Supply" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Non GST Supply" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Composite Dealer" Value="4"></asp:ListItem>
                            <asp:ListItem Text="Export of services" Value="5"></asp:ListItem>
                            <asp:ListItem Text="Supply by SEZ" Value="6"></asp:ListItem>
                            <asp:ListItem Text="Supply to SEZ" Value="7"></asp:ListItem>
                            <asp:ListItem Text="Import of Service" Value="8"></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkNoITC" runat="server" Text="No ITC" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnNewCharge" Text="Add New Charge" runat="server" OnClick="btnNewCharge_Click" Visible="false"/>
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
                                        <%--<asp:BoundField DataField="HSN" HeaderText="HSN" />--%>
                                        <asp:BoundField DataField="ChargeName" HeaderText="Charge Name" />
                                        <asp:BoundField DataField="ChargeCode" HeaderText="Code" />
                                        <asp:BoundField DataField="TaxAmount" HeaderText="Taxable Value" />
                                        <asp:BoundField DataField="TaxAmountINR" HeaderText="Taxable Value (INR)" DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="CurrencyName" HeaderText="Currency" />
                                        <asp:BoundField DataField="InvoiceCurrencyExchangeRate" HeaderText="Rate" />
                                        <asp:BoundField DataField="OtherDeduction" HeaderText="Deduction" />
                                        <asp:BoundField DataField="IGSTRate" HeaderText="IGST Rate" />
                                        <asp:BoundField DataField="IGSTAmount" HeaderText="IGST Amt" />
                                        <asp:BoundField DataField="CGSTRate" HeaderText="CGST Rate" />
                                        <asp:BoundField DataField="CGSTAmount" HeaderText="CGST Amt" />
                                        <asp:BoundField DataField="SGSTRate" HeaderText="SGST Rate" />
                                        <asp:BoundField DataField="SGSTAmount" HeaderText="SGST Amt" />
                                        <asp:BoundField DataField="Amount" HeaderText="Total" />
                                        <%--<asp:BoundField DataField="Remark" HeaderText="Remark" />--%>
                                        <%--<asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEditCharge" runat="server" Text="Edit" OnClick="lnkEditCharge_Click" CommandArgument='<%#BIND("lid") %>' CausesValidation="false"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <%--   <asp:TemplateField HeaderText="Remove">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnlRemoveCharge" runat="server" Text="Remove" OnClick="lnlRemoveCharge_Click" CommandArgument='<%#BIND("lid") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField--%>
                                    </Columns>
                                </asp:GridView>
                            </div>
                </fieldset>
                
                <fieldset runat="server" id="fldTDSItem" visible="true">
                    <legend>TDS</legend>
                        TDS Not Applicable <asp:CheckBox ID="chkTDSNo" runat="server" Text="Yes" 
                        OnCheckedChanged="chkTDSNo_CheckedChanged" AutoPostBack="true" />&nbsp;&nbsp;
                        <asp:DropDownList ID="ddTDSExempt" runat="server">
                        <asp:ListItem Text="--Exempt Reason--" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Exemption Certificate" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Below the threshold limit" Value="2"></asp:ListItem>
                        <asp:ListItem Text="No 3rd Party TDS deduction allowed " Value="3"></asp:ListItem>
                        <asp:ListItem Text="Threshold limit Rate" Value="4"></asp:ListItem>
                        <asp:ListItem Text="Already Deducted" Value="49"></asp:ListItem>
                        <asp:ListItem Text="N.A." Value="50"></asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <br /><br />
                    TDS Applicable &nbsp;&nbsp; <asp:CheckBox ID="chkTDSYes" runat="server" Text="Yes" 
                        OnCheckedChanged="chkTDSYes_CheckedChanged" AutoPostBack="true" />&nbsp;&nbsp;
                     <asp:DropDownList ID="ddTDSLedgerCode" runat="server">
                        <asp:ListItem Text="--TDSLedgerCode--" Value="0"></asp:ListItem>
                        <asp:ListItem Text="194C-Contractors" Value="1"></asp:ListItem>
                        <asp:ListItem Text="194H-Commission" Value="2"></asp:ListItem>
                        <asp:ListItem Text="194I-Rent" Value="3"></asp:ListItem>
                        <asp:ListItem Text="194I-Rent on P&M" Value="4"></asp:ListItem>
                        <asp:ListItem Text="194IA- sale of immovable prop" Value="5"></asp:ListItem>
                        <asp:ListItem Text="194J-Professional fees" Value="6"></asp:ListItem>
                        <asp:ListItem Text="I.TAX DEDUCTED - (NOT TO PAY)" Value="7"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddTDSRateType" runat="server" OnSelectedIndexChanged="ddTDSRateType_SelectedIndexChanged" 
                        AutoPostBack="true" Width="110px">
                        <asp:ListItem Text="Standard" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Concessional" Value="2"></asp:ListItem>
                    </asp:DropDownList>&nbsp;&nbsp;
                    TDS Rate &nbsp;&nbsp;
                    <asp:RequiredFieldValidator ID="rfvddTDSRate" runat="server" ValidationGroup="TDSRequired" ControlToValidate="ddTDSRate"
                        InitialValue="0" Text="Required"> </asp:RequiredFieldValidator>
                    <asp:DropDownList ID="ddTDSRate" runat="server" Width="80px">
                        <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                        <asp:ListItem Text="0.10%" Value="0.10"></asp:ListItem>
                        <asp:ListItem Text="0.20%" Value="0.20"></asp:ListItem>
                        <asp:ListItem Text="0.50%" Value="0.50"></asp:ListItem>
                        <asp:ListItem Text="0.60%" Value="0.60"></asp:ListItem>
                        <asp:ListItem Text="0.70%" Value="0.70"></asp:ListItem>
                        <asp:ListItem Text="0.75%" Value="0.75"></asp:ListItem>
                        <asp:ListItem Text="0.90%" Value="0.90"></asp:ListItem>
                        <asp:ListItem Text="1.00%" Value="1.00"></asp:ListItem>
                        <asp:ListItem Text="1.40%" Value="1.40"></asp:ListItem>
                        <asp:ListItem Text="1.50%" Value="1.50"></asp:ListItem>
                        <asp:ListItem Text="2.00%" Value="2.00"></asp:ListItem>
                        <asp:ListItem Text="3.75%" Value="3.75"></asp:ListItem>
                        <asp:ListItem Text="5.00%" Value="5.00"></asp:ListItem>
                        <asp:ListItem Text="7.5%" Value="7.50"></asp:ListItem>
                        <asp:ListItem Text="10.00%" Value="10.00"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvtxtTdsRate" runat="server" ValidationGroup="TDSRequired" ControlToValidate="txtTDSRate"
                        InitialValue="" Text="Required"> </asp:RequiredFieldValidator>
                    <asp:TextBox id="txtTDSRate" runat="server" MaxLength="5" Visible="false" Width="80px"></asp:TextBox>
                    &nbsp;&nbsp;
                    <asp:Button ID="btnAddTDS" runat="server" Text="Calculate TDS" OnClick="btnAddTDS_Click" ValidationGroup="TDSRequired"></asp:Button>

                    Total TDS &nbsp;Rs.&nbsp;
                    <asp:Label ID="lblTotalTDS" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                        
                    <div class="clear">
                    </div>
                    <div class="clear"></div>
                    <div id="Div2" runat="server" style="max-height: 550px; overflow: auto;">
                        <asp:GridView ID="GridViewTDS" runat="server" CssClass="table" AutoGenerateColumns="false"
                            AllowPaging="false" DataKeyNames="lid" DataSourceID="InvoiceDataSource"  Visible="false">
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
                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEditTDS" runat="server" Text="Edit" OnClick="lnkEditTDS_Click" CommandArgument='<%#BIND("lid") %>' CausesValidation="false"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remove">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkRemoveTDS" runat="server" Text="Remove" OnClick="lnkRemoveTDS_Click" CommandArgument='<%#BIND("lid") %>' CausesValidation="false"
                                          OnClientClick="return confirm('Sure to Remove TDS From Charge Detail?');"  ></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </fieldset>

                <fieldset runat="server" id="fldRCMItem" visible="true">
                    <legend>RCM</legend>
                    RCM Applicable &nbsp;&nbsp; <asp:CheckBox ID="chkRCMYes" runat="server" Text="Yes" 
                        OnCheckedChanged="chkRCMYes_CheckedChanged" AutoPostBack="true" />&nbsp;&nbsp;
                    RCM Rate &nbsp;&nbsp;<asp:DropDownList ID="ddRCMRate" runat="server" Width="70px">
                        <asp:ListItem Text="18%" Value="18.00"></asp:ListItem>
                        <asp:ListItem Text="12%" Value="12.00"></asp:ListItem>
                        <asp:ListItem Text="5%" Value="5.00"></asp:ListItem>
                    </asp:DropDownList>&nbsp;&nbsp;
                    <asp:RadioButtonList ID="rbRCMGstType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                        <asp:ListItem Text="CGST & SGST" Value="2" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="IGST" Value="1"></asp:ListItem>
                    </asp:RadioButtonList>
                    &nbsp;<asp:Button ID="btnAddRCM" runat="server" Text="Calculate RCM" OnClick="btnAddRCM_Click"></asp:Button>
                    &nbsp;&nbsp;RCM Total Amount &nbsp;&nbsp;<b><asp:Label ID="lblRCMTotalAmount" runat="server"></asp:Label></b>    
                    <div class="clear">
                    </div>
                    <div class="clear"></div>
                    <div id="Div3" runat="server" style="max-height: 550px; overflow: auto;">
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
                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEditRCM" runat="server" Text="Edit" OnClick="lnkEditRCM_Click" CommandArgument='<%#BIND("lid") %>' CausesValidation="false"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </fieldset>

                <fieldset runat="server" id="fldPayment">
                    <legend>Payment Request</legend>
                       <b> Net Payable Amount <asp:Label ID="lblPaymentCurrencyName1" runat="server"></asp:Label>
                        &nbsp;<asp:Label ID="lblNetPayable" runat="server"></asp:Label></b>
                    <div class="clear">
                    </div>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>
                                Payment
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblPayment" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblPayment_SelectedIndexChanged"
                                    AutoPostBack="true">
                                    <asp:ListItem Text="Full Payment" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Partial Payment" Value="0"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                Amount
                                <asp:RequiredFieldValidator ID="rfvPayAmount" runat="server" ValidationGroup="RequiredAudit" InitialValue=""
                                     ControlToValidate="txtPayAmount" Text="Required"  SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPayAmount" runat="server" MaxLength="10" TextMode="Number" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Payment Currency
                            </td>
                            <td>
                                <asp:DropDownList ID="ddCurrency" runat="server" DataSourceID="dataSourceCurrency"
                                    AppendDataBoundItems="true" DataValueField="lId" DataTextField="Currency" TabIndex="10">
                                <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                Exchange Rate
                            </td>
                            <td>
                                <asp:TextBox ID="txtExchangeRate" runat="server" Width="50px" Text="1" Enabled="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Payment Mode
                                <asp:RequiredFieldValidator ID="rfvPaymentType" runat="server" ValidationGroup="RequiredAudit" InitialValue="0"
                                     ControlToValidate="ddlPaymentType" Text="Required"  SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPaymentType" runat="server" AppendDataBoundItems="true" DataSourceID="DataSourcePaymentType"
                                    DataTextField="sName" DataValueField="lid" AutoPostBack="true" OnSelectedIndexChanged="ddlPaymentType_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Bank Name
                            </td>                        
                            <td>
                                <asp:DropDownList ID="ddBabajiBankName" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddBabajiBankName_SelectedIndexChanged"></asp:DropDownList>

                            </td>
                        </tr>
                        <tr>
                            <td>
                                Bank Account/Cash Book
                                <asp:RequiredFieldValidator ID="rfvBank" runat="server" ValidationGroup="RequiredAudit" InitialValue="0"
                                     ControlToValidate="ddBabajiBankAccount" Text="Required"  SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddBabajiBankAccount" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddBabajiBankAccount_SelectedIndexChanged">
                            <%--    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Yes BANK LTD - 001790600004039" Value="Y20" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="AXIS BANK LTD - 912020005850066" Value="B20"></asp:ListItem>
                                    <asp:ListItem Text="H D F C BANK" Value="B11"></asp:ListItem>
                                    <asp:ListItem Text="IDBI BANK JNPT" Value="B18"></asp:ListItem>
                                    <asp:ListItem Text="YES BANK LTD. MUMBAI" Value="YBL000"></asp:ListItem>--%>
                                </asp:DropDownList>
                            </td>
                            <td>
                                Fund Transfer From Live Tracking ?
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblFundTransferFromLiveTracking" runat="server" Enabled="false" AutoPostBack="true"
                                    OnSelectedIndexChanged="rblFundTransferFromLiveTracking_SelectedIndexChanged" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                </fieldset>

                <fieldset id="fldPostInvoice" runat="server"><legend>Audit Invoice L1</legend>
                <br />                     
                    
                    <table border="0" cellpadding="0" cellspacing="0" width="50%" bgcolor="white">
                        <td>
                            Narration
                            <asp:RequiredFieldValidator ID="RFVAuditRemark" runat="server" ValidationGroup="RequiredAudit" InitialValue=""
                                ControlToValidate="txtAuditRemark" Text="Required" ErrorMessage="Please Enter Audit Narration" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtAuditRemark" runat="server" TextMode="MultiLine" MaxLength="200"></asp:TextBox>
                        </td>    
                        <td>
                            <asp:Button ID="btnPostSubmit" Text="Confirm Audit Invoice L1" OnClick="btnPostSubmit_Click" ValidationGroup="RequiredAudit"
                                OnClientClick="return confirm('Sure to Complete Audit L1 Detail? Details will not be modified afterwards.');" runat="server" />
                        </td>
                    </table>

                </fieldset>
                <fieldset id="fldPaymentHistory" runat="server" visible="false">
                    <legend>Payment Detail</legend>
                        <asp:GridView ID="gvPaymentHistory" runat="server" AutoGenerateColumns="False"
                            CssClass="table" Width="99%" PagerStyle-CssClass="pgr"
                            DataKeyNames="lId" DataSourceID="DataSourcePaymentHistory" CellPadding="4"
                            AllowPaging="True" AllowSorting="True" PageSize="40">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Payment Type" DataField="PaymentType" />
                                <asp:BoundField HeaderText="BankName" DataField="BankName" />
                                <asp:BoundField HeaderText="Amount" DataField="PaidAmount" />
                                <asp:BoundField HeaderText="Instrument No" DataField="InstrumentNo" />
                                <asp:BoundField HeaderText="Instrument Date" DataField="InstrumentDate" DataFormatString="{0:dd/MM/yyyy}"/>
                                <asp:BoundField HeaderText="Remark" DataField="Remark" />
                                <asp:BoundField HeaderText="User" DataField="UserName" />
                                <asp:BoundField HeaderText="Date" DataField="CreatedDate" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" />
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
                    </asp:GridView> </fieldset>          
                
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
                                <div style="word-wrap: break-word; width: 700px; white-space:normal;">
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
            
            <!--- Invoice Item -->
            <div>
            <asp:Button ID="modelPopup" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="modelPopup" PopupControlID="Panel1"></cc1:ModalPopupExtender>
            <asp:Panel ID="Panel1" Style="display: none" runat="server">
                <fieldset class="ModalPopupPanel">
                    <div title="Charge Details" class="header">
                        <asp:Label ID="lblPopupHeader" Text="Add New Charge Detail" runat="server"></asp:Label>
                    </div>
                    <div>
                        <div class="AutoExtenderList"></div>
                        <asp:Label ID="lblChargeError" runat="server"></asp:Label>
                    </div>

                    <table border="0" cellpadding="0" cellspacing="0" width="100%"  bgcolor="white" style="height:200px;">
                        <tr>
                            <td>
                                Charge Code
                                <div id="divwidthChargeCode" runat="server">
                                </div>
                                <cc1:autocompleteextender id="AutocompleteChargeCode" runat="server" TargetControlID="txtChargeCode"
                                    completionlistelementid="divwidthChargeCode" ServicePath="~/WebService/FAPayToAutoComplete.asmx"
                                    ServiceMethod="GetFAChargeCodeByCode" MinimumPrefixLength="2" BehaviorID="divwidthChargeCode"
                                    ContextKey="3299" usecontextkey="True" OnClientItemSelected="OnChargeCodeSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:autocompleteextender>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChargeCode" runat="server"></asp:TextBox>
                                <asp:HiddenField ID="hdnChargeHSN" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hdnItemId" runat="server" Value="0"></asp:HiddenField>
                            </td>
                            <td>
                                Charge Name
                                <div id="divwidthChargeName" runat="server">
                                </div>
                                <cc1:autocompleteextender id="AutocompleName" runat="server" TargetControlID="txtChargeName"
                                    completionlistelementid="divwidthChargeName" ServicePath="~/WebService/FAPayToAutoComplete.asmx"
                                    ServiceMethod="GetFAChargeCodeByName" MinimumPrefixLength="2" BehaviorID="divwidthChargeName"
                                    ContextKey="3290" usecontextkey="True" OnClientItemSelected="OnChargeCodeSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:autocompleteextender>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChargeName" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Taxable Value
                            </td>
                            <td>
                                <asp:TextBox ID="txtTaxableValue" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                GST
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblGSTType" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="IGST" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="CGST/SGST" Value="0" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                GST Rate
                            </td>
                            <td>
                                <asp:DropDownList ID="ddGSTRate" runat="server" Width="100px">                                    
                                    <asp:ListItem Text="-Select-" Value="-1" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="18%" Value="18"></asp:ListItem>
                                    <asp:ListItem Text="12%" Value="12"></asp:ListItem>
                                    <asp:ListItem Text="5%" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="No GST" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                Other Deduction
                            </td>
                            <td>
                                <asp:TextBox ID="txtOtherDeduction" runat="server">                                    
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Remark
                                <asp:RequiredFieldValidator ID="rfvChargeRemark" runat="server" ValidationGroup="ChargeRequired" InitialValue=""
                                    ControlToValidate="txtChargeRemark" Text="Required" ErrorMessage="Please Enter Charge Remark" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChargeRemark" runat="server" TextMode="MultiLine" MaxLength="200"></asp:TextBox>
                            </td>
                            <td colspan="3">
                                <asp:Button ID="btnAddCharges" runat="server" Text="Add Charges" OnClick="btnAddCharges_Click" ValidationGroup="ChargeRequired" />
                                &nbsp;&nbsp; 
                                <asp:Button ID="btnClosePopup" runat="server" Text="Close" OnClick="btnClosePopup_Click" CausesValidation="false" ToolTip="Cancel" />
                            </td>
                        </tr>
                    </table>
                    <div class="m clear"></div>
                </fieldset>
            </asp:Panel>
            
            </div>
            <!-- Invoice TDS -->
            
            <div>
            <asp:Button ID="lnkModalPopupTDS" runat="server" Style="display: none"/>
            <cc1:ModalPopupExtender ID="ModalPopupTDS" runat="server" TargetControlID="lnkModalPopupTDS" PopupControlID="Panel2"></cc1:ModalPopupExtender>
            <asp:Panel ID="Panel2" Style="display: none" runat="server">
                <fieldset class="ModalPopupPanel">
                    <div title="TDS Details" class="header">
                        <asp:Label ID="Label101" Text="Edit TDS Detail" runat="server"></asp:Label>
                        <asp:HiddenField ID="hdnTDSItemId" runat="server" />
                    </div>
                        
                    <table border="0" cellpadding="0" cellspacing="0" width="100%"  bgcolor="white" style="height:200px;">
                        <tr>
                            <td>
                                Taxable Value
                            </td>
                            <td>
                                <asp:Label ID="lblTDSTaxableValue" runat="server"></asp:Label>
                            </td>
                            <td>
                                TDS Ledger Code
                                <asp:RequiredFieldValidator ID="rfvEditTDSLedgerCode" runat="server" ValidationGroup="RequiredEditTDS" ControlToValidate="ddEditTDSLedgerCode"
                                    InitialValue="" Text="Required"> </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddEditTDSLedgerCode" runat="server">
                                    <asp:ListItem Text="--TDSLedgerCode--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="194C-Contractors" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="194H-Commission" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="194I-Rent" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="194I-Rent on P&M" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="194IA- sale of immovable prop" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="194J-Professional fees" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="I.TAX DEDUCTED - (NOT TO PAY)" Value="7"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                TDS Rate Type
                            </td>
                            <td>
                                <asp:DropDownList ID="ddEditTDSRateType" runat="server" AutoPostBack="true" 
                                    OnSelectedIndexChanged="ddEditTDSRateType_SelectedIndexChanged" Width="110px">
                                    <asp:ListItem Text="Standard" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Concessional" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                Rate
                                <asp:RequiredFieldValidator ID="rfvddEditTDSRate" runat="server" ValidationGroup="RequiredEditTDS" ControlToValidate="ddEditTDSLedgerCode"
                                    InitialValue="0" Text="Required" Enabled="false"> </asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvtxtEditTDSRate" runat="server" ValidationGroup="RequiredEditTDS" ControlToValidate="ddEditTDSLedgerCode"
                                    InitialValue="" Text="Required" Enabled="false"> </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddEditTDSRate" runat="server" Width="80px">
                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="0.10%" Value="0.10"></asp:ListItem>
                                    <asp:ListItem Text="0.20%" Value="0.20"></asp:ListItem>
                                    <asp:ListItem Text="0.50%" Value="0.50"></asp:ListItem>
                                    <asp:ListItem Text="0.60%" Value="0.60"></asp:ListItem>
                                    <asp:ListItem Text="0.70%" Value="0.70"></asp:ListItem>
                                    <asp:ListItem Text="0.75%" Value="0.75"></asp:ListItem>
                                    <asp:ListItem Text="0.90%" Value="0.90"></asp:ListItem>
                                    <asp:ListItem Text="1.00%" Value="1.00"></asp:ListItem>
                                    <asp:ListItem Text="1.40%" Value="1.40"></asp:ListItem>
                                    <asp:ListItem Text="1.50%" Value="1.50"></asp:ListItem>
                                    <asp:ListItem Text="2.00%" Value="2.00"></asp:ListItem>
                                    <asp:ListItem Text="3.75%" Value="3.75"></asp:ListItem>
                                    <asp:ListItem Text="5.00%" Value="5.00"></asp:ListItem>
                                    <asp:ListItem Text="7.5%" Value="7.50"></asp:ListItem>
                                    <asp:ListItem Text="10.00%" Value="10.00"></asp:ListItem>
                                </asp:DropDownList>

                                <asp:TextBox id="txtEditTDSRate" runat="server" TextMode="Number" MaxLength="5" Visible="false" Width="80px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
    
                                <asp:Button ID="btnUpdateTDS" runat="server" Text="Update TDS" OnClick="btnUpdateTDS_Click" ValidationGroup="RequiredEditTDS" />
                                <asp:Button ID="btnCloseTDSPopup" runat="server" Text="Close" OnClick="btnCloseTDSPopup_Click" CausesValidation="false" ToolTip="Cancel" />
                            </td>
                        </tr>
                    </table>
                    <div class="m clear"></div>
                </fieldset>
            </asp:Panel>
            </div>

            <!-- Invoice RCM -->
            
            <div>
            <asp:Button ID="lnkModalPopupRCM" runat="server" Style="display: none"/>
            <cc1:ModalPopupExtender ID="ModalPopupRCM" runat="server" TargetControlID="lnkModalPopupRCM" PopupControlID="Panel3"></cc1:ModalPopupExtender>
            <asp:Panel ID="Panel3" Style="display: none" runat="server">
                <fieldset class="ModalPopupPanel">
                    <div title="RCM Details" class="header">
                        <asp:Label ID="Label1" Text="Edit RCM Detail" runat="server"></asp:Label>
                        <asp:HiddenField ID="hdnRCMItemId" runat="server" />
                    </div>
                        
                    <table border="0" cellpadding="0" cellspacing="0" width="100%"  bgcolor="white" style="height:200px;">
                        <tr>
                            <td>
                                Taxable Value
                            </td>
                            <td>
                                <asp:Label ID="lblRCMTaxableValue" runat="server"></asp:Label>
                            </td>
                            <td>
                                RCM Rate
                                <asp:RequiredFieldValidator ID="rfvddEditRCMRate" runat="server" ValidationGroup="RequiredEditRCM" 
                                    ControlToValidate="ddEditRCMRate" InitialValue="" Text="Required"> </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddEditRCMRate" runat="server" Width="70px">
                                    <asp:ListItem Text="18%" Value="18.00"></asp:ListItem>
                                    <asp:ListItem Text="12%" Value="12.00"></asp:ListItem>
                                    <asp:ListItem Text="5%" Value="5.00"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                GST Type
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rbEditRCMGstType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" Enabled="false">
                                    <asp:ListItem Text="CGST & SGST" Value="2" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="IGST" Value="1"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
    
                                <asp:Button ID="btnUpdateRCM" runat="server" Text="Update RCM" OnClick="btnUpdateRCM_Click" ValidationGroup="RequiredEditRCM" />
                                <asp:Button ID="btnCloseRCMPopup" runat="server" Text="Close" OnClick="btnCloseRCMPopup_Click" CausesValidation="false" ToolTip="Cancel" />

                            </td>
                        </tr>
                    </table>
                    <div class="m clear"></div>
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
                <asp:SqlDataSource ID="InvoiceRCMDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetInvoiceRCMItem" SelectCommandType="StoredProcedure">
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
                <asp:SqlDataSource ID="DataSourcePaymentHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetInvoicePayment" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="InvoiceID" SessionField="InvoiceID" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceProformaPaymentHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetInvoicePayment" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:Parameter Name="InvoiceID" DefaultValue="0" />
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
                <asp:SqlDataSource ID="dataSourceCurrency" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetCurrencyMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </div>
    </ContentTemplate>
            </asp:UpdatePanel>
</asp:Content>

