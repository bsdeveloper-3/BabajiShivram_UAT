<%@ Page Title="Vendor Invoice Submission" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="PaymentRequest2.aspx.cs" Inherits="AccountExpense_PaymentRequest2" Culture="en-GB"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:ToolkitScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
    </cc1:ToolkitScriptManager>
    <script type="text/javascript">
        function OnJobSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=hdnJobId.ClientID%>').value = results.JobId;
            $get('<%=hdnModuleId.ClientID%>').value = results.ModuleId;
        }
        function OnVendorSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=txtSupplierGSTIN.ClientID%>').value = results.GSTIN;
            $get('<%=txtVendorName.ClientID%>').value = results.Name;
            $get('<%=hdnVendorCode.ClientID%>').value = results.Code;
            $get('<%=txtVendorPANNo.ClientID%>').value = results.PANNo;
            $get('<%=txtPaymentTerms.ClientID%>').value = results.CreditDays;
        }
        function OnGSTINSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=txtSupplierGSTIN.ClientID%>').value = results.GSTIN;
            $get('<%=txtVendorName.ClientID%>').value = results.Name;
            $get('<%=hdnVendorCode.ClientID%>').value = results.Code;
            $get('<%=txtVendorPANNo.ClientID%>').value = results.PANNo;
            $get('<%=txtPaymentTerms.ClientID%>').value = results.CreditDays;
        }
        function OnPanNoSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=txtSupplierGSTIN.ClientID%>').value = results.GSTIN;
            $get('<%=txtVendorName.ClientID%>').value = results.Name;
            $get('<%=hdnVendorCode.ClientID%>').value = results.Code;
            $get('<%=txtVendorPANNo.ClientID%>').value = results.PANNo;
            $get('<%=txtPaymentTerms.ClientID%>').value = results.CreditDays;

        }
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

            $get('<%=hdnChargeCode.ClientID%>').value = results.Charge_Code;
            $get('<%=hdnChargeName.ClientID%>').value = results.Charge_Name
            $get('<%=hdnChargeHSN.ClientID%>').value = results.HSN_Code

        }
        function OnChargeNameSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=txtChargeCode.ClientID%>').value = results.Charge_Code;
            $get('<%=txtChargeName.ClientID%>').value = results.Charge_Name

            $get('<%=hdnChargeCode.ClientID%>').value = results.Charge_Code;
            $get('<%=hdnChargeName.ClientID%>').value = results.Charge_Name
            $get('<%=hdnChargeHSN.ClientID%>').value = results.HSN_Code
        }
        function CheckConsgineeGSTIN() {
            var varBEGSTIN = $get('<%=hdnBOEGSTIN.ClientID%>').value;

            var varAllowDiffGST = $get('<%=hdnDifferentGSTNoAllowed.ClientID%>').value;

            var varBillToParty = $get('<%=txtCongisgneeGSTIN.ClientID%>').value;

            if (varBEGSTIN.toUpperCase() == varBillToParty.toUpperCase()) {

                $get('<%=hdnIsRIM.ClientID%>').value = '0';
                // DO Nothing - NonRIM
            }
            else if (varBillToParty == "") {
                // Do Nothing 
            }
            else {

                var objIsRim = 0;

                if (varBillToParty.toUpperCase() == '24AAACB0466A2ZG') { objIsRim = 1; }
                else if (varBillToParty.toUpperCase() == '37AAACB0466A1ZA') { objIsRim = 1; }
                else if (varBillToParty.toUpperCase() == '24AAACB0466A1ZH') { objIsRim = 1; }
                else if (varBillToParty.toUpperCase() == '29AAACB0466A1Z7') { objIsRim = 1; }
                else if (varBillToParty.toUpperCase() == '03AAACB0466A1ZL') { objIsRim = 1; }
                else if (varBillToParty.toUpperCase() == '36AAACB0466A1ZC') { objIsRim = 1; }
                else if (varBillToParty.toUpperCase() == '07AAACB0466A1ZD') { objIsRim = 1; }
                else if (varBillToParty.toUpperCase() == '30AAACB0466A1ZO') { objIsRim = 1; }
                else if (varBillToParty.toUpperCase() == '06AAACB0466A1ZF') { objIsRim = 1; }
                else if (varBillToParty.toUpperCase() == '27AAACB0466A1ZB') { objIsRim = 1; }
                else if (varBillToParty.toUpperCase() == '08AAACB0466A1ZB') { objIsRim = 1; }
                else if (varBillToParty.toUpperCase() == '33AAACB0466A1ZI') { objIsRim = 1; }
                else if (varBillToParty.toUpperCase() == '19AAACB0466A1Z8') { objIsRim = 1; }

                if (objIsRim == 1) {
                    $get('<%=hdnIsRIM.ClientID%>').value = '1';
                    alert('Non-RIM - Invoice To (GSTIN No) is different from Bill of Entry GSTIN!')
                    
                }
                else if (varAllowDiffGST == "1") {
                    $get('<%=hdnIsRIM.ClientID%>').value = '0';
                    alert('RIM - Invoice To (GSTIN No) is different from Bill of Entry GSTIN!')
                }
                else {
                    $get('<%=hdnIsRIM.ClientID%>').value = '0';
                    alert('Invoice To (GSTIN No) not Matched With BoE GSTIN or Babaji GSTIN! Please Enter Valid GSTIN')
                }
            }

        }

        function chkNoGSTINCustomer_OnChange() {
            var varCheckGSTIN = $get('<%=chkNoGSTINCustomer.ClientID%>').checked;

            var requiredConsigneeGSTNo = document.getElementById('<%=rfvConsigneeGST.ClientID%>');
            var requiredConsigneeName = document.getElementById('<%=rfvConsigneeName.ClientID%>');

            if (varCheckGSTIN == true) {
                $get('<%=txtCongisgneeGSTIN.ClientID%>').disabled = true;
                $get('<%=txtCongisgneeName.ClientID%>').disabled = false;

                $get('<%=txtCongisgneeGSTIN.ClientID%>').value = '';
                $get('<%=hdnConsigneeCode.ClientID%>').value = '';

                ValidatorEnable(requiredConsigneeGSTNo, false);
                ValidatorEnable(requiredConsigneeName, true);
            }
            else {
                $get('<%=txtCongisgneeGSTIN.ClientID%>').disabled = false;
                $get('<%=txtCongisgneeName.ClientID%>').disabled = true;

                $get('<%=txtCongisgneeName.ClientID%>').value = '';
                $get('<%=hdnConsigneeCode.ClientID%>').value = '';

                ValidatorEnable(requiredConsigneeGSTNo, true);
                ValidatorEnable(requiredConsigneeName, false);
            }
        }

    </script>
<%--    <asp:UpdatePanel ID="upFillDetails" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
            <asp:HiddenField ID="hdnJobId" runat="server" Value="0" />
            <asp:HiddenField ID="hdnJobRefNo" runat="server" Value="" />
            <asp:HiddenField ID="hdnModuleId" runat="server" Value="0" />
            <asp:HiddenField ID="hdnBranchId" runat="server" Value="0" />
            <asp:HiddenField ID="hdnBOEGSTIN" runat="server" Value="" />
            <asp:HiddenField ID="hdnIsRIM" runat="server" Value="0" />
            <asp:HiddenField ID="hdnVendorCode" runat="server" />
            <asp:HiddenField ID="hdnConsigneeCode" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="hdnConsigneePANNo" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="hdnChargeName" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="hdnChargeCode" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="hdnStatusId" runat="server" />
            <asp:HiddenField ID="hdnPaymentHold" runat="server" Value="0" />
            <asp:HiddenField ID="hdnDifferentGSTNoAllowed" runat="server" Value="0" />

            <div align="center">
            <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            </div>
                <fieldset style="width: 80%;">
                <legend>Vendor Invoice Submission</legend>
                <asp:Wizard ID="wzRequest" runat="server" DisplaySideBar="false" OnNextButtonClick="wzRequest_NextButtonClick"
                    OnFinishButtonClick="wzRequest_FinishButtonClick">
                <WizardSteps>
                    <asp:WizardStep ID="stpJob" runat="server" Title="Request Detail" StepType="Start">
                    <table border="0" cellpadding="50" cellspacing="50" width="100%" style="font-size:14px" bgcolor="white">
                        <tr>
                            <td colspan="4">
                                <asp:RadioButtonList ID="rblInvoiceMode" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Tax Invoice" Value="1" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Credit Note" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Debit Note" Value="3"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>Job Number
                            <asp:RequiredFieldValidator ID="rfvJobNo" runat="server" ValidationGroup="JobRequired"
                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtJobNumber"
                                Text="Required" ErrorMessage="Please Select Job Number."></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtJobNumber" Width="160px" runat="server" ToolTip="Enter Job Number." MaxLength="18"
                                    CssClass="SearchTextbox" placeholder="Search" AutoPostBack="true" OnTextChanged="txtJobNumber_TextChanged"></asp:TextBox>
                                <div id="divwidthCust_Loc" runat="server">
                                </div>
                                <cc1:AutoCompleteExtender ID="JobDetailExtender" runat="server" TargetControlID="txtJobNumber"
                                    CompletionListElementID="divwidthCust_Loc" ServicePath="~/WebService/JobNumberAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="10" BehaviorID="divwidthCust_Loc"
                                    ContextKey="1885" UseContextKey="True" OnClientItemSelected="OnJobSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>
                                Type of Expense
                                <asp:RequiredFieldValidator ID="rfvExpenseType" runat="server" ValidationGroup="JobRequired"
                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlExpenseType" InitialValue="0"
                                    Text="Required" ErrorMessage="Please Select Type of Expense."> </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlExpenseType" runat="server" AppendDataBoundItems="false" DataSourceID="DataSourceExpense"
                                    DataTextField="sName" DataValueField="lid" OnSelectedIndexChanged="ddlExpenseType_OnSelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Invoice Type
                                <asp:RequiredFieldValidator ID="rfvInvoiceType" runat="server" ValidationGroup="JobRequired"
                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddInvoiceType" InitialValue="-1"
                                    Text="Required" ErrorMessage="Please Select Invoice Type."> </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddInvoiceType" runat="server" OnSelectedIndexChanged="ddInvoiceType_SelectedIndexChanged">
                                    <asp:ListItem Text="-Select-" Value="-1" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Final" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Proforma" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                Vendor Type
                            </td>
                            <td>
                                <asp:DropDownList ID="ddGSTINType" runat="server">
                                    <asp:ListItem Text="Registered" Value="1" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Un-Registered" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Foreign Party" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Payment Type
                            <asp:RequiredFieldValidator ID="rfvpaytype" runat="server" ValidationGroup="JobRequired"
                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlPaymentType" InitialValue="0"
                                Text="Required" ErrorMessage="Please Select Payment Type"> </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPaymentType" runat="server">
                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Cash" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Cheque" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="DD" Value="3"></asp:ListItem>
                                    <%--<asp:ListItem Text="RTGS" Value="4"></asp:ListItem>--%>
                                    <asp:ListItem Text="NEFT/RTGS" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="REMITTANCE" Value="10"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                Advance Received
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblAdvanceReceived" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label id="lblInvoiceNo" Text="Invoice No" runat="server"></asp:Label> 
                                
                                <asp:RequiredFieldValidator ID="RFVEnquiryNo" runat="server" ControlToValidate="txtInvoiceNo"
                                     ErrorMessage="Invoice No Required" Text="Required" ValidationGroup="JobRequired" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RFVInvoiceNo" runat="server" ControlToValidate="txtInvoiceNo" ValidationExpression="^[a-zA-Z0-9- / -]*$"
                                     ErrorMessage="Invalid Invoice No " Text="Special characters not allowed except '/' and '-'" ValidationGroup="Required" Display="Dynamic" SetFocusOnError="true"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtInvoiceNo" runat="server" Width="125px" MaxLength="16"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label id="lblInvoiceDate" Text="Invoice Date" runat="server"></asp:Label>
                                
                                <cc1:CalendarExtender ID="CalExtInvoiceDate" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="imgInvoiceDate"
                                    Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtInvoiceDate">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="MskEdtInvoice" TargetControlID="txtInvoiceDate" Mask="99/99/9999" MessageValidatorTip="true"
                                    MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="MskEdtValInvoice" ControlExtender="MskEdtInvoice" ControlToValidate="txtInvoiceDate" IsValidEmpty="false"
                                    InvalidValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid Invoice Date" SetFocusOnError="true"
                                    MinimumValueMessage="Invalid Invoice Date" MaximumValueMessage="Invoice Invalid Date" MinimumValue="01/04/2024"
                                    ValidationGroup="JobRequired"
                                    EmptyValueMessage="Invoice Date Required" EmptyValueBlurredText="Required" runat="Server"></cc1:MaskedEditValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtInvoiceDate" runat="server" Width="125px" placeholder="dd/mm/yyyy" ToolTip="Enter Invoice Date"></asp:TextBox>
                                <asp:Image ID="imgInvoiceDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                  Invoice Currency
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
                                <asp:Label ID="lblTotalInvoiceValue" runat="server" Text="Total Invoice Value" ></asp:Label>
                                <asp:RequiredFieldValidator ID="rfvTotalValue" runat="server" ValidationGroup="JobRequired"
                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtTotalInvoiceValue" InitialValue=""
                                    Text="Required" ErrorMessage="Please Enter Total Value"> </asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegxTotalValue" runat="server" ControlToValidate="txtTotalInvoiceValue"
                                    SetFocusOnError="true" ErrorMessage="Invalid Amount." Display="Dynamic"
                                    ValidationGroup="JobRequired" ValidationExpression="^[0-9]\d{0,13}(\.\d{1,2})?$"></asp:RegularExpressionValidator>
                            
                            </td>
                            <td>
                                <asp:TextBox ID="txtTotalInvoiceValue" runat="server" Width="125px" MaxLength="12"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblMultipleChargeCode" runat="server" Text="Multiple GST rates in the invoice ?"></asp:Label>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblMultiChargeCode" runat="server" RepeatDirection="Horizontal" Enabled="true"> 
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr id="trDutyInterest" runat="server" visible="false">
                           <td>Interest </td>
                            <td  colspan="3">
                                <table width="100%" style="border-collapse: collapse">
                                    <tr>                                         
                                        <td>
                                            <asp:RadioButtonList ID="rdlInterest" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                                                  OnSelectedIndexChanged="rdlInterest_OnSelectedIndexChanged" Width="150px">
                                                 <asp:ListItem Text="YES" Value="1"></asp:ListItem>
                                               <asp:ListItem Text="NO" Value="2" Selected="True"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>

                                        <td  id="tdlblInterestAmnt" runat="server" visible="false">
                                             <asp:Label ID="lblInterestAmnt" runat="server" Text="Interest Amount" ></asp:Label>
                                        </td>
                                        <td  id="tdtxtInterestAmnt" runat="server" visible="false" colspan="2">
                                            <asp:TextBox ID="txtInterestAmnt" runat="server" Text="0"
                                                OnTextChanged="txtInterestAmnt_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                               <asp:CompareValidator ID="CVInterestAmount" runat="server" ControlToValidate="txtInterestAmnt"
                                                Display="Dynamic" SetFocusOnError="true" Type="Double" Operator="DataTypeCheck" Enabled="false"
                                                ErrorMessage="Invalid Interest Amount" ValidationGroup="JobRequired"></asp:CompareValidator>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                PD Account
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblPDAccount" runat="server" RepeatDirection="Horizontal" 
                                    AutoPostBack="true" OnSelectedIndexChanged="rblPDAccount_SelectedIndexChanged">
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                Credit Vendor
                                <a href="#" data-tooltip="Option Need to be selected for those vendors where we have Margin or Commission with Vendor">
                                <img src="../Images/info-icon.png" width="14px" height="14px" alt="info" /></a>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblCreditVendor" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Remark
                                <asp:RequiredFieldValidator ID="RFVRemark" runat="server" ValidationGroup="JobRequired" InitialValue=""
                                     ControlToValidate="txtRemark" Text="Required" ErrorMessage="Please Enter Remark" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" MaxLength="200"></asp:TextBox>
                            </td>
                        </tr>
                     </table>
                        <br /><br /><br />
                     <table border="0" cellpadding="0" cellspacing="0" width="100%" style="font-size:14px" bgcolor="white">
                        <tr>
                            <td>
                                Customer
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
                            <td>
                                Branch
                            </td>
                             <td>
                                 <asp:Label ID="lblBranchName" runat="server"></asp:Label>
                             </td>
                            <td>
                                IGM No / Date
                            </td>
                            <td>
                                <asp:Label ID="lblIGMNo" runat="server"></asp:Label>
                                /&nbsp<asp:Label ID="lblIGMDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                BE No / Date
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server"></asp:Label>
                               /&nbsp;<asp:Label ID="lblBOEDate" runat="server"></asp:Label>
                            </td>
                            <td>
                                BL No / Date
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server"></asp:Label>
                                / &nbsp;<asp:Label ID="lblBLDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                BE No
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
                                <asp:Label ID="lblIECNumber" runat="server"></asp:Label>
                            </td>
                            <td>
                                BoE GSTIN
                            </td>
                            <td>
                                <asp:Label ID="lblConsgneeGSTIN" runat="server"></asp:Label>
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
                                Duty Amount
                            </td>
                            <td>
                                <asp:Label ID="lblDutyAmount" runat="server"></asp:Label>
                            </td>

                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                
                    </asp:WizardStep>

                    <asp:WizardStep ID="stpVendor" runat="server" Title="Vendor /Invoice Party" AllowReturn="true" StepType="Step">
                        <fieldset>
                            <legend> Invoice From -  Vendor Detail </legend>

                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                            <td>Vendor GSTIN No
                            </td>
                            <td>
                                <asp:TextBox ID="txtSupplierGSTIN" runat="server"></asp:TextBox>
                                <div id="divwidthGST"></div>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtenderGST" runat="server" TargetControlID="txtSupplierGSTIN"
                                    CompletionListElementID="divwidthGST" ServicePath="../WebService/FAVendorAutoComplete.asmx"
                                    ServiceMethod="GetVendorByGSTIN" MinimumPrefixLength="2" BehaviorID="divwidthGST"
                                    ContextKey="21" UseContextKey="True" OnClientItemSelected="OnGSTINSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight" DelimiterCharacters="">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>
                                Vendor Payment Terms(Days)
                            </td>
                            <td>
                                <asp:TextBox ID="txtPaymentTerms" runat="server" MaxLength="4" TextMode="Number" Width="80px" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Vendor Name
                                 <asp:RequiredFieldValidator ID="rfvVendorName" runat="server" ValidationGroup="Required" InitialValue=""
                                     ControlToValidate="txtVendorName" Text="Required" ErrorMessage="Please Enter Vendor Name" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVendorName" runat="server" Width="90%"></asp:TextBox>
                                <div id="divwidthVendor"></div>
                                <cc1:AutoCompleteExtender ID="AutoCompleteVendor" runat="server" TargetControlID="txtVendorName"
                                    CompletionListElementID="divwidthVendor" ServicePath="../WebService/FAVendorAutoComplete.asmx"
                                    ServiceMethod="GetVendorListByName" MinimumPrefixLength="2" BehaviorID="divwidthVendor"
                                    ContextKey="2863" UseContextKey="True" OnClientItemSelected="OnVendorSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight" DelimiterCharacters="">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>
                                PAN No
                            </td>
                            <td>
                                <asp:TextBox ID="txtVendorPANNo" runat="server" Enabled="false"></asp:TextBox>
                                <div id="divwidthPAN"></div>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtenderPAN" runat="server" TargetControlID="txtVendorPANNo"
                                    CompletionListElementID="divwidthPAN" ServicePath="../WebService/FAVendorAutoComplete.asmx"
                                    ServiceMethod="GetVendorListByPANNo" MinimumPrefixLength="2" BehaviorID="divwidthGST"
                                    ContextKey="8854" UseContextKey="True" OnClientItemSelected="OnPanNoSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight" DelimiterCharacters="">
                                </cc1:AutoCompleteExtender>
                            </td>
                        </tr>                        
                        
                    </table>

                            </fieldset>

                        <fieldset>
                            <legend> Invoice To -  Party Detail </legend>

                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr style="margin-top:20px;">
                            <td>
                                Invoice To (GSTIN Number)
                                 <asp:RequiredFieldValidator ID="rfvConsigneeGST" runat="server" ValidationGroup="Required" InitialValue=""
                                     ControlToValidate="txtCongisgneeGSTIN" Text="Required" ErrorMessage="Please Enter Invoice To (GSTIN No)" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCongisgneeGSTIN" runat="server" onfocusout="CheckConsgineeGSTIN()" MaxLength="15"> </asp:TextBox>
                                <div id="divwidthConsigneeCode"></div>
                                <cc1:AutoCompleteExtender ID="AutoCompleteConsigneeGSTIN" runat="server" TargetControlID="txtCongisgneeGSTIN"
                                    CompletionListElementID="divwidthConsigneeCode" ServicePath="../WebService/FAVendorAutoComplete.asmx"
                                    ServiceMethod="GetCustomerByGSTIN" MinimumPrefixLength="8" BehaviorID="divwidthConsigneeCode"
                                    ContextKey="2863" UseContextKey="True" OnClientItemSelected="OnCongisgneeGSTINSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight" DelimiterCharacters="">
                                </cc1:AutoCompleteExtender>

                                <asp:CheckBox ID="chkNoGSTINCustomer" runat="server" Text="No GSTIN" OnChange="chkNoGSTINCustomer_OnChange()" />
                            </td>
                            <td>
                                Invoice Party Name
                                <asp:RequiredFieldValidator ID="rfvConsigneeName" runat="server" ValidationGroup="Required" InitialValue="" Enabled="false"
                                     ControlToValidate="txtCongisgneeName" Text="Required" ErrorMessage="Please Enter Invoice Party Name" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCongisgneeName" runat="server" Enabled="false" Width="200px"></asp:TextBox>
                                <div id="divwidthConsignee"></div>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtCongisgneeName"
                                    CompletionListElementID="divwidthConsignee" ServicePath="../WebService/FAVendorAutoComplete.asmx"
                                    ServiceMethod="GetCustomerListByName" MinimumPrefixLength="2" BehaviorID="divwidthConsignee"
                                    ContextKey="1163" UseContextKey="True" OnClientItemSelected="OnConsingeeSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight" DelimiterCharacters="">
                                </cc1:AutoCompleteExtender>
                            </td>
                        </tr>
                            </table>
                        </fieldset>
                    </asp:WizardStep>

                    <asp:WizardStep ID="stpInvoice" runat="server" Title="Invoice Detail" AllowReturn="true" StepType="Finish">
                     <fieldset>
                        <legend>Request Detail</legend>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" style="font-size:14px" bgcolor="white">
                                <tr>
                                    <td>
                                        Job Number
                                    </td>
                                    <td>
                                        <asp:Label ID="lblJobRefNO" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        Type of Expense
                                    </td>
                                    <td>
                                        <asp:Label ID="lblExpenseType" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Invoice Type
                                    </td>
                                    <td>
                                        <asp:Label ID="lblInvoiceType" runat="server"></asp:Label>
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
                                        Invoice Currency
                                    </td>
                                    <td>
                                        <asp:Label ID="lblInvoiceCurrency" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        Exchange Rate	
                                    </td>
                                    <td>
                                        <asp:Label ID="lblExchangeRate" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Total Invoice Value (INR)
                                    </td>
                                    <td>
                                        <asp:Label ID="lblEnteredTotalValue" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        Multiplle GST Rate ?
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMultipleGSTRate" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            
                     </fieldset>
                        <fieldset id="fldVendorBuySell" runat="server" visible="false">
                        <legend>Buy Sell Detail</legend>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="font-size:14px" bgcolor="white">
                            <tr>
                                <td>
                                    HOD For Approval
                                    <asp:RequiredFieldValidator ID="RFVHOD" runat="server" ControlToValidate="ddHOD"
                                        ValidationGroup="InvoiceRequired" Text="Required" InitialValue="0"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddHOD" runat="server"></asp:DropDownList>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
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
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="font-size:14px" bgcolor="white">
                        <tr>
                            <td>
                                Immedite Payment
                                <asp:RequiredFieldValidator ID="rfvImmediatePayment" runat="server" ControlToValidate="rblImmediatePayment"
                                     ValidationGroup="InvoiceRequired" Text="Required"></asp:RequiredFieldValidator>
                            </td>
                            <td colspan="3">
                                <asp:RadioButtonList ID="rblImmediatePayment" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" 
                                    RepeatLayout="Table" OnSelectedIndexChanged="rblImmediatePayment_SelectedIndexChanged">
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Within Credit Days" Value="0"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>Vendor Payment Due Date
                                <cc1:CalendarExtender ID="CalExtPayDueDate" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="imgPaymentDueDate"
                                    Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtPaymentDueDate">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="MskEdtPayDueDate" TargetControlID="txtPaymentDueDate" Mask="99/99/9999" MessageValidatorTip="true"
                                    MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="MskEdtValPayDueDate" ControlExtender="MskEdtPayDueDate" ControlToValidate="txtPaymentDueDate" IsValidEmpty="false"
                                    InvalidValueMessage="Payment Due Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true"
                                    MinimumValueMessage="Invalid Payment Due Date" MaximumValueMessage="Invalid Payment Due Date" ValidationGroup="InvoiceRequired"
                                    MinimumValue='<%#DateTime.Now.ToString("dd/MM/yyyy") %>' MaximumValue='<%#DateTime.Now.AddDays(100).ToString("dd/MM/yyyy") %>'
                                    EmptyValueBlurredText="Required" EmptyValueMessage="Invalid Payment Due Date" runat="Server"></cc1:MaskedEditValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPaymentDueDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                                <asp:Image ID="imgPaymentDueDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                            <td>Payment Required Date
                                <cc1:CalendarExtender ID="CalExtPayReqrdDate" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="imgPaymentRequdDate"
                                    Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtPaymentRequiredDate">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="MskEdtPayReqrdDate" TargetControlID="txtPaymentRequiredDate" Mask="99/99/9999" MessageValidatorTip="true"
                                    MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="MskEdtValPayReqrdDate" ControlExtender="MskEdtPayReqrdDate" ControlToValidate="txtPaymentRequiredDate" IsValidEmpty="false"
                                    InvalidValueMessage="Payment Required Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true"
                                    MinimumValueMessage="Invalid Payment Required Date" MaximumValueMessage="Invalid Date" ValidationGroup="InvoiceRequired"
                                    MinimumValue='<%#DateTime.Now.AddYears(-1).ToString("dd/MM/yyyy") %>' MaximumValue='<%#DateTime.Now.AddDays(100).ToString("dd/MM/yyyy") %>'
                                    runat="Server"></cc1:MaskedEditValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPaymentRequiredDate" runat="server" Width="100px" placeholder="dd/mm/yyyy" ToolTip="Enter Payment Required Date"></asp:TextBox>
                                <asp:Image ID="imgPaymentRequdDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                        </tr>
                        
                        <tr>
                            <td>RIM/Non-RIM</td>
                            <td>
                                <asp:DropDownList ID="ddRIM" runat="server" Enabled="false">
                                    <asp:ListItem Text="RIM" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Non-RIM" Value="2" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                Advance Amount
                                <asp:RequiredFieldValidator ID="RFVAdvanceAmt" runat="server" ControlToValidate="txtAdvanceAmount" InitialValue="" Enabled="false"
                                    ErrorMessage="Required" ValidationGroup="InvoiceRequired" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="REVAdvanceAmount" runat="server" ControlToValidate="txtAdvanceAmount"
                                    SetFocusOnError="true" ErrorMessage="Invalid Amount." Display="Dynamic"
                                    ValidationGroup="InvoiceRequired" ValidationExpression="^[0-9]\d{0,13}(\.\d{1,2})?$"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAdvanceAmount" runat="server" Width="125px" TextMode="Number" Enabled="false" MaxLength="10"></asp:TextBox>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                Total Taxable Value 
                            </td>
                             <td>   
                                 <asp:TextBox ID="lblTotalTaxableValue" runat="server" Text="0" Enabled="false" OnTextChanged="lblTotalTaxableValue_TextChanged" AutoPostBack="true"></asp:TextBox>
                             </td>
                             <td>   
                                 Total Tax 
                            </td>
                            <td>
                                <asp:TextBox ID="lblTotalGST" runat="server" Enabled="false" Text="0" OnTextChanged="lblTotalGST_TextChanged" AutoPostBack="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Total Value
                            </td>
                            <td>
                                <asp:TextBox ID="lblTotalValue" runat="server" Enabled="false"></asp:TextBox>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </fieldset>
                        
                <fieldset runat="server" id="fldInvoiceItem" visible="true">
                    <legend>Charge Detail</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="font-size:14px" bgcolor="white">
                        <tr>
                            <td>
                                Charge Name
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
                            <td>
                                Charge Code
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
                        </tr>
                        <tr>
                            <td>Taxable Value
                            </td>
                            <td>
                                <asp:TextBox ID="txtTaxableValue" runat="server" MaxLength="12"></asp:TextBox>
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
                                <asp:DropDownList ID="ddGSTRate" runat="server" >
                                    <asp:ListItem Text="-Select-" Value="-1" Selected="True"></asp:ListItem>
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
                                <asp:Button ID="btnAddCharges" runat="server" Text="Add Charges" OnClick="btnAddCharges_Click"/>
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
               
                <fieldset id="fldDocument">
                    <legend>Upload Document</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="font-size:14px" bgcolor="white">
                        <tr>
                            <td>
                                Invoice Copy
                                <asp:RequiredFieldValidator ID="RFVDocInvoice" runat="server" ControlToValidate="fuDocumentInvoice" InitialValue="" 
                                    ErrorMessage="Required" ValidationGroup="InvoiceRequired" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator id="regpdf" text="Only PDF File" errormessage="Please upload only PDF File" ControlToValidate="fuDocumentInvoice" 
                                    ValidationExpression="^.*\.(pdf|PDF)$" runat="server" ValidationGroup="InvoiceRequired"/>

                            </td>
                            <td>
                                <asp:FileUpload ID="fuDocumentInvoice" runat="server" />
                            </td>
                            <td>
                                E-Mail Approval Copy
                                <asp:RequiredFieldValidator ID="RFVDocEmailApproval" runat="server" ControlToValidate="fuDocumentEmailApproval" InitialValue="" 
                                    ErrorMessage="Required" ValidationGroup="InvoiceRequired" Display="Dynamic" SetFocusOnError="true" Enabled="false"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:FileUpload ID="fuDocumentEmailApproval" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Other Document
                            </td>
                            <td>
                                <asp:FileUpload ID="fuDocumentOther" runat="server" />
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </fieldset>

                    </asp:WizardStep>
                </WizardSteps>

                <HeaderTemplate>
                <ul id="wizHeader">
                    <asp:Repeater ID="SideBarList" runat="server">
                        <ItemTemplate>
                            <li><a class="<%# GetClassForWizardStep(Container.DataItem) %>" title="<%#Eval("Name")%>">
                                <%# Eval("Name")%></a> </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
                </HeaderTemplate>

                <StartNavigationTemplate>
                   <asp:Button ID="btnNext" runat="server" CommandName="MoveNext" Text="Next - Vendor Detail" ValidationGroup="JobRequired" />
                </StartNavigationTemplate>
                <FinishNavigationTemplate>
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false"
                        OnClientClick="return confirm('Are you sure you want to cancel');" />
                    <asp:Button ID="btnFinPrevious" runat="server" Text="Previous" CommandName="MovePrevious" CausesValidation="false" />
                    &nbsp;&nbsp;<asp:Button ID="btnFinish" runat="server" Text="Save Detail" CausesValidation="true" 
                        CommandName="MoveComplete" ValidationGroup="InvoiceRequired" />
                </FinishNavigationTemplate>
            </asp:Wizard>
            </fieldset>
        <div id="divDatasourc">
                <asp:SqlDataSource ID="DataSourcePaymentType" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetPaymentTypeMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceExpense" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetRequestTypeMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="dataSourceCurrency" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetCurrencyMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </div>
<%--        </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>

