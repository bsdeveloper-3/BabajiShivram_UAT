<%@ Page Title="Final Invoice" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="InvoiceFinal.aspx.cs" Inherits="AccountExpense_InvoiceFinal" Culture="en-GB"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Scriptmanager id="ScriptManager1" runat="server" scriptmode="Release">
    </asp:Scriptmanager>
    <script type="text/javascript">

        function OnCongisgneeGSTINSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=txtCongisgneeGSTIN.ClientID%>').value = results.GSTIN;
            $get('<%=txtCongisgneeName.ClientID%>').value = results.Name;
            $get('<%=hdnConsigneeCode.ClientID%>').value = results.Code;
            $get('<%=hdnConsigneePANNo.ClientID%>').value = results.PANNo;

        }

        function OnConsingeeSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=txtCongisgneeGSTIN.ClientID%>').value = results.GSTIN;
            $get('<%=txtCongisgneeName.ClientID%>').value = results.Name;
            $get('<%=hdnConsigneeCode.ClientID%>').value = results.Code;
            $get('<%=hdnConsigneePANNo.ClientID%>').value = results.PANNo;

            if (results.GSTIN != '') {
                $get('<%=chkNoGSTINCustomer.ClientID%>').checked = false;
            }
        }

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

        function CheckConsgineeGSTIN() {
            var varUserGSTIN = $get('<%=hdnBoEGSTIN.ClientID%>').innerText;

            var varJobGSTIN = $get('<%=txtCongisgneeGSTIN.ClientID%>').value;

            if (varJobGSTIN.toUpperCase() == varUserGSTIN.toUpperCase()) {
        $get('<%=ddRIM.ClientID%>').selectedIndex = 0;
            }
            else if (varJobGSTIN == "") {
        // Do Nothing 
    }
            else {

                    var objIsRim = 0;

                    if (varJobGSTIN.toUpperCase() == '27AAACB0466A1ZB') {objIsRim = 1; }
                    else if (varJobGSTIN.toUpperCase() == '07AAACB0466A1ZD') {objIsRim = 1; }
                    else if (varJobGSTIN.toUpperCase() == '19AAACB0466A1Z8') {objIsRim = 1; }
                    else if (varJobGSTIN.toUpperCase() == '24AAACB0466A1ZH') {objIsRim = 1; }
                    else if (varJobGSTIN.toUpperCase() == '24AAACB0466A2ZG') {objIsRim = 1; }
                    else if (varJobGSTIN.toUpperCase() == '33AAACB0466A1ZI') {objIsRim = 1; }
                    else if (varJobGSTIN.toUpperCase() == '37AAACB0466A1ZA') {objIsRim = 1; }
                    else if (varJobGSTIN.toUpperCase() == '29AAACB0466A1Z7') {objIsRim = 1; }
                    else if (varJobGSTIN.toUpperCase() == '30AAACB0466A1ZO') {objIsRim = 1; }
                    else if (varJobGSTIN.toUpperCase() == '08AAACB0466A1ZB') {objIsRim = 1; }
                    else if (varJobGSTIN.toUpperCase() == '36AAACB0466A1ZC') {objIsRim = 1; }
                    else if (varJobGSTIN.toUpperCase() == '03AAACB0466A1ZL') {objIsRim = 1; }
                    else if (varJobGSTIN.toUpperCase() == '06AAACB0466A1ZF') {objIsRim = 1; }
                    else if (varJobGSTIN.toUpperCase() == '09AAACB0466A1Z9') {objIsRim = 1; }

                    if (objIsRim == 1) {
                        alert('Invoice To (GSTIN No) is different from Bill of Entry GSTIN!')
                        $get('<%=ddRIM.ClientID%>').selectedIndex = 1;
                }
                else {
                        $get('<%=ddRIM.ClientID%>').selectedIndex = 0;
                    alert('Invoice To (GSTIN No) not Matched With BoE GSTIN or Babaji GSTIN! Please Enter Valid GSTIN')
                }
            }

            $get('<%=txtCongisgneeName.ClientID%>').setFocus();
        }

        function chkNoGSTINCustomer_OnChange()
        {
            var varCheckGSTIN = $get('<%=chkNoGSTINCustomer.ClientID%>').checked;

            if (varCheckGSTIN == true) {
        $get('<%=txtCongisgneeGSTIN.ClientID%>').disabled = true;
                $get('<%=txtCongisgneeName.ClientID%>').disabled = false;

                $get('<%=txtCongisgneeGSTIN.ClientID%>').value = '';
                $get('<%=hdnConsigneeCode.ClientID%>').value = '';
            }
            else
            {
        $get('<%=txtCongisgneeGSTIN.ClientID%>').disabled = false;
                $get('<%=txtCongisgneeName.ClientID%>').disabled = true;

                $get('<%=txtCongisgneeName.ClientID%>').value = '';
                $get('<%=hdnConsigneeCode.ClientID%>').value = '';
            }
        }
        
    </script>
        <asp:UpdatePanel ID="upFillDetails" runat="server">
        <ContentTemplate>
            <div>
                <div align="center">
                    <asp:HiddenField ID="hdnJobId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnBranchId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnBoEGSTIN" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnConsigneeCode" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hdnConsigneePANNo" runat="server"></asp:HiddenField>
                    
                    <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                    
                    <asp:ValidationSummary ID="vsFillDetails" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
                </div>

                <fieldset>
                    <legend>Final Invoice Receive Confirmation</legend>
                    <div class="m clear">
                        <asp:Button ID="btnSave" Text="Save" OnClick="btnSave_Click" ValidationGroup="Required" runat="server" />
                        <asp:Button ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false" runat="server" />
                    </div>
                    <fieldset>
                    <legend>Job Detail</legend>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>Job Number
                            </td>
                            <td width="80%" colspan="3">
                                <asp:Label ID="lblJobNumber" Width="160px" runat="server"></asp:Label>
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
                        
                    </table>
                    </fieldset>
                    <fieldset>
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
                                <asp:Label ID="lblVendorName" runat="server"></asp:Label>
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
                                Credit Terms (Days)
                            </td>
                            <td>
                                <asp:Label ID="lblCreditTerms" runat="server"></asp:Label>
                            </td>
                            <td></td>
                            <td></td>
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
                    </table>
                    </fieldset>
                     <fieldset>
                    <legend>Proforma Invoice Detail</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                             <td>
                                Billing Party Name
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lblBillingPartyName" runat="server"></asp:Label>
                            </td>
                            <td>
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
                                <asp:Label ID="lblInvoiceNo" runat="server"></asp:Label>
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
                                <asp:Label ID="lblTotalInvoiceValue" runat="server"></asp:Label>
                            </td>
                            <td>
                                Paid Amount
                            </td>
                            <td>
                                <asp:Label ID="lblPaidAmount" runat="server"></asp:Label>
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
                            <td>
                                <asp:Label ID="lblRequestRemark" runat="server"></asp:Label>
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
                    
                    <fieldset id="fldPaymentHistory" runat="server">
                    <legend>Payment History</legend>
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
                    
                </fieldset>
                <fieldset>
                    <legend>Final Invoice Detail</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>
                                Invoice To (GSTIN Number)
                                 <asp:RequiredFieldValidator ID="rfvConsigneeGST" runat="server" ValidationGroup="Required" InitialValue=""
                                     ControlToValidate="txtCongisgneeGSTIN" Text="Required" ErrorMessage="Please Enter Invoice To (GSTIN No)" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCongisgneeGSTIN" runat="server" onfocusout="CheckConsgineeGSTIN()" > </asp:TextBox>
                                <div id="divwidthConsigneeCode"></div>
                                <cc1:AutoCompleteExtender ID="AutoCompleteConsigneeGSTIN" runat="server" TargetControlID="txtCongisgneeGSTIN"
                                    CompletionListElementID="divwidthConsigneeCode" ServicePath="../WebService/FAVendorAutoComplete.asmx"
                                    ServiceMethod="GetCompletionListByGSTIN" MinimumPrefixLength="12" BehaviorID="divwidthConsigneeCode"
                                    ContextKey="2863" UseContextKey="True" OnClientItemSelected="OnCongisgneeGSTINSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight" DelimiterCharacters="">
                                </cc1:AutoCompleteExtender>

                                <asp:CheckBox ID="chkNoGSTINCustomer" runat="server" Text="No GSTIN" OnChange="chkNoGSTINCustomer_OnChange()" />
                            </td>
                            <td>
                                Invoice Party Name
                            </td>
                            <td>
                                <asp:TextBox ID="txtCongisgneeName" runat="server" Enabled="false"></asp:TextBox>
                                <div id="divwidthConsignee"></div>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtCongisgneeName"
                                    CompletionListElementID="divwidthConsignee" ServicePath="../WebService/FAVendorAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthConsignee"
                                    ContextKey="1163" UseContextKey="True" OnClientItemSelected="OnConsingeeSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight" DelimiterCharacters="">
                                </cc1:AutoCompleteExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Invoice Type
                            </td>
                            <td>
                                <asp:DropDownList ID="ddInvoiceType" runat="server" Enabled="false">
                                    <asp:ListItem Text="Final" Value="1" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Proforma" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>RIM/Non-RIM</td>
                            <td>
                                <asp:DropDownList ID="ddRIM" runat="server">
                                    <asp:ListItem Text="RIM" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Non-RIM" Value="2" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>Invoice No
                                 <asp:RequiredFieldValidator ID="RFVEnquiryNo" runat="server" ControlToValidate="txtInvoiceNo"
                                     ErrorMessage="Invoice No Required" Text="Required" ValidationGroup="Required" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtInvoiceNo" runat="server" Width="125px" MaxLength="50"></asp:TextBox>
                            </td>
                            <td>Invoice Date
                                <cc1:CalendarExtender ID="CalExtInvoiceDate" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="imgInvoiceDate"
                                    Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtInvoiceDate">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="MskEdtInvoice" TargetControlID="txtInvoiceDate" Mask="99/99/9999" MessageValidatorTip="true"
                                    MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="MskEdtValInvoice" ControlExtender="MskEdtInvoice" ControlToValidate="txtInvoiceDate" IsValidEmpty="false"
                                    InvalidValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid Invoice Date" SetFocusOnError="true"
                                    MinimumValueMessage="Invalid Invoice Date" MaximumValueMessage="Invoice Invalid Date" MinimumValue="01/01/2019"
                                    ValidationGroup="Required"
                                    EmptyValueMessage="Invoice Date Required" EmptyValueBlurredText="Required" runat="Server"></cc1:MaskedEditValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtInvoiceDate" runat="server" Width="125px" placeholder="dd/mm/yyyy" ToolTip="Enter Planning Date."></asp:TextBox>
                                <asp:Image ID="imgInvoiceDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Total Invoice Value
                                <asp:RequiredFieldValidator ID="TotalInvoiceValue" runat="server" ValidationGroup="Required" InitialValue=""
                                     ControlToValidate="txtTotalInvoiceValue" Text="Required" ErrorMessage="Please Enter Total Invoice Value" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            
                            </td>
                            <td>
                                <asp:TextBox ID="txtTotalInvoiceValue" runat="server" Width="125px" MaxLength="15"></asp:TextBox>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            
                            <td>Upload Document</td>
                            <td>
                                <asp:FileUpload ID="fuDocument" runat="server" />
                            </td>
                            <td>
                                Remark
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" MaxLength="200"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">Total Taxable Value: &nbsp;<asp:Label ID="lblTotalTaxableValue" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;
                                Total Tax: &nbsp;<asp:Label ID="lblTotalGST" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;
                                Total Value: &nbsp;<asp:Label ID="lblTotalValue" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                
                <fieldset runat="server" id="fldInvoiceItem" visible="true">
                    <legend>Charge Detail</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>Charge Code
                                <div id="divwidthChargeCode" runat="server">
                                </div>
                                <cc1:AutoCompleteExtender ID="AutocompleteChargeCode" runat="server" TargetControlID="txtChargeCode"
                                    CompletionListElementID="divwidthChargeCode" ServicePath="~/WebService/FAPayToAutoComplete.asmx"
                                    ServiceMethod="GetFAChargeCodeByCode" MinimumPrefixLength="2" BehaviorID="divwidthChargeCode"
                                    ContextKey="3299" UseContextKey="True" OnClientItemSelected="OnChargeCodeSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChargeCode" runat="server"></asp:TextBox>
                                <asp:HiddenField ID="hdnChargeHSN" runat="server"></asp:HiddenField>
                            </td>
                            <td>Charge Name
                                <div id="divwidthChargeName" runat="server">
                                </div>
                                <cc1:AutoCompleteExtender ID="AutocompleName" runat="server" TargetControlID="txtChargeName"
                                    CompletionListElementID="divwidthChargeName" ServicePath="~/WebService/FAPayToAutoComplete.asmx"
                                    ServiceMethod="GetFAChargeCodeByName" MinimumPrefixLength="2" BehaviorID="divwidthChargeName"
                                    ContextKey="3290" UseContextKey="True" OnClientItemSelected="OnChargeCodeSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChargeName" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Taxable Value
                            </td>
                            <td>
                                <asp:TextBox ID="txtTaxableValue" runat="server"></asp:TextBox>
                            </td>
                            <td>GST
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblGSTType" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="IGST" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="CGST/SGST" Value="0" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>GST Rate
                            </td>
                            <td>
                                <asp:DropDownList ID="ddGSTRate" runat="server">
                                    <asp:ListItem Text="18%" Value="18"></asp:ListItem>
                                    <asp:ListItem Text="12%" Value="12"></asp:ListItem>
                                    <asp:ListItem Text="5%" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="No GST" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Remark
                            </td>
                            <td>
                                <asp:TextBox ID="txtChargeRemark" runat="server" TextMode="MultiLine" MaxLength="200"></asp:TextBox>
                            </td>
                            <td colspan="3">
                                <asp:Button ID="btnAddCharges" runat="server" Text="Add Charges" OnClick="btnAddCharges_Click" />
                            </td>
                        </tr>
                    </table>
                    <div class="clear">
                    </div>
                    <legend>Charge Detail</legend>
                    <div class="clear"></div>
                    <div id="Div1" runat="server" style="max-height: 550px; overflow: auto;">
                        <asp:GridView ID="gvCharges" runat="server" CssClass="table" AutoGenerateColumns="false"
                            AllowPaging="false" DataKeyNames="SL">
                            <Columns>
                                <%--<asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:BoundField DataField="ChargeName" HeaderText="Charge Name" />
                                <asp:BoundField DataField="ChargeCode" HeaderText="ChargeCode" />
                                <asp:BoundField DataField="TaxableValue" HeaderText="Taxable Amount" />
                                <asp:BoundField DataField="IGSTRate" HeaderText="IGST Rate" />
                                <asp:BoundField DataField="IGSTAmt" HeaderText="IGST Amt" />
                                <asp:BoundField DataField="CGSTRate" HeaderText="CGST Rate" />
                                <asp:BoundField DataField="CGSTAmt" HeaderText="CGST Amt" />
                                <asp:BoundField DataField="SGSTRate" HeaderText="SGST Rate" />
                                <asp:BoundField DataField="SGSTAmt" HeaderText="SGST Amt" />
                                <asp:BoundField DataField="ChargeTotal" HeaderText="Amount" />
                                <asp:BoundField DataField="ChargeRemark" HeaderText="Remark" />
                                <asp:TemplateField HeaderText="Remove">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnlRemove" runat="server" Text="Remove" OnClick="lnlRemove_Click" CommandArgument='<%#BIND("SL") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </fieldset>
                
            </div>
            
            <div id="divDatasourc"> 
                <asp:SqlDataSource ID="InvoiceDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetInvoiceItem" SelectCommandType="StoredProcedure">
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
            </div>
    </ContentTemplate>
            </asp:UpdatePanel>
</asp:Content>

