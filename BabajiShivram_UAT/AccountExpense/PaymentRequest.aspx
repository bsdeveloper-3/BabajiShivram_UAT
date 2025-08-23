<%@ Page Title="Payment Request" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PaymentRequest.aspx.cs" Inherits="AccountExpense_PaymentRequest" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
    </asp:ScriptManager>
    <script type="text/javascript">
        function OnJobSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnJobId').value = results.JobId;
            $get('ctl00_ContentPlaceHolder1_hdnModuleId').value = results.ModuleId;
        }

        function OnVendorTypeChanged() {
            // ddGSTINType

            var objVendorType = $get('<%=ddGSTINType.ClientID%>').selectedIndex;

        <%--  if (objVendorType == 0)
            {
                $get('<%=txtSupplierGSTIN.ClientID%>').disable = true;
                $get('<%=txtVendorName.ClientID%>').disable = false;

            }--%>
        }
        function OnVendorSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=txtSupplierGSTIN.ClientID%>').value = results.GSTIN;
            $get('<%=txtVendorName.ClientID%>').value = results.Name;
            $get('<%=hdnVendorCode.ClientID%>').value = results.Code;
            $get('<%=txtVendorPANNo.ClientID%>').value = results.PANNo;
            $get('<%=txtPaymentTerms.ClientID%>').value = results.CreditDays;

            var objPayDueDate = new Date();
            var objCredirDays = 0;

            if (results.CreditDays != '')
            { 
               objCredirDays = parseInt(results.CreditDays, 10);
            }

            objPayDueDate.setDate(objPayDueDate.getDate() + objCredirDays);

            $get('<%=txtPaymentDueDate.ClientID%>').value = objPayDueDate.toLocaleDateString('en-GB') ;

            <%--  if (results.Code.includes('BSC') || results.Code.includes('BS4S') ) {
                
                $get('<%=ddRIM.ClientID%>').selectedIndex = 1;
                $get('<%=ddGSTINType.ClientID%>').selectedIndex = 0;
            }
            else if (results.PANNo == 'PANNOTAVBL') {
                $get('<%=ddRIM.ClientID%>').selectedIndex = 0;
                $get('<%=ddGSTINType.ClientID%>').selectedIndex = 2;
            }--%>

                if (results.GSTIN != '') {
                    $get('<%=ddGSTINType.ClientID%>').selectedIndex = 0;
            }

        }

        function OnGSTINSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=txtSupplierGSTIN.ClientID%>').value = results.GSTIN;
                $get('<%=txtVendorName.ClientID%>').value = results.Name;
                $get('<%=hdnVendorCode.ClientID%>').value = results.Code;
                $get('<%=txtVendorPANNo.ClientID%>').value = results.PANNo;
                $get('<%=txtPaymentTerms.ClientID%>').value = results.CreditDays;

                $get('<%=ddGSTINType.ClientID%>').selectedIndex = 0;

            <%-- if (results.Code.includes('BSC') || results.Code.includes('BS4S')) {

                $get('<%=ddRIM.ClientID%>').selectedIndex = 1;
                $get('<%=ddGSTINType.ClientID%>').selectedIndex = 0;
            }--%>

            <%--  if (results.PANNo == 'PANNOTAVBL') {
                $get('<%=ddGSTINType.ClientID%>').selectedIndex = 2;
            }--%>

        }

        function OnPanNoSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=txtSupplierGSTIN.ClientID%>').value = results.GSTIN;
                $get('<%=txtVendorName.ClientID%>').value = results.Name;
                $get('<%=hdnVendorCode.ClientID%>').value = results.Code;
                $get('<%=txtVendorPANNo.ClientID%>').value = results.PANNo;
                $get('<%=txtPaymentTerms.ClientID%>').value = results.CreditDays;

                if (results.GSTIN != '') {
                    $get('<%=ddGSTINType.ClientID%>').selectedIndex = 0;
                }

            <%--if (results.Code.includes('BSC') || results.Code.includes('BS4S')) {

                $get('<%=ddRIM.ClientID%>').selectedIndex = 1;
                $get('<%=ddGSTINType.ClientID%>').selectedIndex = 0;
            }
            else {
                $get('<%=ddRIM.ClientID%>').selectedIndex = 1;
            }

            if (results.PANNo == 'PANNOTAVBL') {
                $get('<%=ddGSTINType.ClientID%>').selectedIndex = 2;
            }
            else if (results.GSTIN != '')
            {
                $get('<%=ddGSTINType.ClientID%>').selectedIndex = 0;
            }--%>
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
                $get('<%=hdnChargeHSN.ClientID%>').value = results.HSN_Code
        }

        function OnChargeNameSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=txtChargeCode.ClientID%>').value = results.Charge_Code;
                $get('<%=txtChargeName.ClientID%>').value = results.Charge_Name
                $get('<%=hdnChargeHSN.ClientID%>').value = results.HSN_Code
        }

        function CheckConsgineeGSTIN() {
            var varUserGSTIN = $get('<%=lblConsgneeGSTIN.ClientID%>').innerText;

            var varJobGSTIN = $get('<%=txtCongisgneeGSTIN.ClientID%>').value;

            if (varJobGSTIN.toUpperCase() == varUserGSTIN.toUpperCase()) {
                $get('<%=ddRIM.ClientID%>').selectedIndex = 0;
            }
            else if (varJobGSTIN == "") {
                // Do Nothing 
            }
            else {

                    var objIsRim = 0;

                    if (varJobGSTIN.toUpperCase() == '27AAACB0466A1ZB') { objIsRim = 1; }
                    else if (varJobGSTIN.toUpperCase() == '07AAACB0466A1ZD') { objIsRim = 1; }
                    else if (varJobGSTIN.toUpperCase() == '19AAACB0466A1Z8') { objIsRim = 1; }
                    else if (varJobGSTIN.toUpperCase() == '24AAACB0466A1ZH') { objIsRim = 1; }
                    else if (varJobGSTIN.toUpperCase() == '24AAACB0466A2ZG') { objIsRim = 1; }
                    else if (varJobGSTIN.toUpperCase() == '33AAACB0466A1ZI') { objIsRim = 1; }
                    else if (varJobGSTIN.toUpperCase() == '37AAACB0466A1ZA') { objIsRim = 1; }
                    else if (varJobGSTIN.toUpperCase() == '29AAACB0466A1Z7') { objIsRim = 1; }
                    else if (varJobGSTIN.toUpperCase() == '30AAACB0466A1ZO') { objIsRim = 1; }
                    else if (varJobGSTIN.toUpperCase() == '08AAACB0466A1ZB') { objIsRim = 1; }
                    else if (varJobGSTIN.toUpperCase() == '36AAACB0466A1ZC') { objIsRim = 1; }
                    else if (varJobGSTIN.toUpperCase() == '03AAACB0466A1ZL') { objIsRim = 1; }
                    else if (varJobGSTIN.toUpperCase() == '06AAACB0466A1ZF') { objIsRim = 1; }
                    else if (varJobGSTIN.toUpperCase() == '09AAACB0466A1Z9') { objIsRim = 1; }

                    if (objIsRim == 1) {
                        alert('Invoice To (GSTIN No) is different from Bill of Entry GSTIN!')
                        $get('<%=ddRIM.ClientID%>').selectedIndex = 1;
                }
                else {
                    $get('<%=ddRIM.ClientID%>').selectedIndex = 0;
                    alert('Invoice To (GSTIN No) not Matched With BoE GSTIN or Babaji GSTIN! Please Enter Valid GSTIN')
                }
            }

        }

        function chkNoGSTINCustomer_OnChange()
        {
            var varCheckGSTIN = $get('<%=chkNoGSTINCustomer.ClientID%>').checked;

            var requiredConsigneeGSTNo = document.getElementById('<%=rfvConsigneeGST.ClientID%>');
            var requiredConsigneeName  = document.getElementById('<%=rfvConsigneeName.ClientID%>');

            if (varCheckGSTIN == true) {
                $get('<%=txtCongisgneeGSTIN.ClientID%>').disabled = true;
                $get('<%=txtCongisgneeName.ClientID%>').disabled = false;

                $get('<%=txtCongisgneeGSTIN.ClientID%>').value = '';
                $get('<%=hdnConsigneeCode.ClientID%>').value = '';

                ValidatorEnable(requiredConsigneeGSTNo, false);
                ValidatorEnable(requiredConsigneeName, true);
            }
            else
            {
                $get('<%=txtCongisgneeGSTIN.ClientID%>').disabled = false;
                $get('<%=txtCongisgneeName.ClientID%>').disabled = true;

                $get('<%=txtCongisgneeName.ClientID%>').value = '';
                $get('<%=hdnConsigneeCode.ClientID%>').value = '';

                ValidatorEnable(requiredConsigneeGSTNo, true);
                ValidatorEnable(requiredConsigneeName, false);
            }
        }

        function ChargeVendorTypeValidation()
        {
            var varMultiChargeCode = $get('<%=rblMultiChargeCode.ClientID%>');

            var varMultiValue = 0; // No

           // alert(varMultiChargeCode);

            if (varMultiChargeCode.rows[0].cells[0].firstChild.checked) {

                varMultiValue = varMultiChargeCode.rows[0].cells[0].firstChild.value; // yes - 1
            }
           
        }
    </script>
    
    <asp:UpdatePanel ID="upFillDetails" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <div>
            <div align="center">
                <asp:HiddenField ID="hdnJobId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnBranchId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnModuleId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnNewPaymentLid" runat="server" Value="0" />
                <asp:HiddenField ID="hdnVendorCode" runat="server" />
                <asp:HiddenField ID="hdnConsigneeCode" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hdnConsigneePANNo" runat="server"></asp:HiddenField>
                    
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
<%--                <asp:ValidationSummary ID="vsFillDetails" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />--%>
            </div>

                <fieldset>
                    <legend>Payment Request</legend>
                <div class="m clear">
                    <asp:Button ID="btnSubmit" Text="Save" OnClick="btnSubmit_Click" runat="server" ValidationGroup="Required" />
                    <asp:Button ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false" runat="server" />
                </div>
                <fieldset>
                    <legend>Job Detail</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>Job Number
                            <asp:RequiredFieldValidator ID="rfvJobNo" runat="server" ValidationGroup="Required"
                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtJobNumber"
                                Text="Required" ErrorMessage="Please Select Job Number."></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtJobNumber" Width="160px" runat="server" ToolTip="Enter Job Number."
                                    CssClass="SearchTextbox" placeholder="Search" AutoPostBack="true" OnTextChanged="txtJobNumber_TextChanged"></asp:TextBox>
                                <div id="divwidthCust_Loc" runat="server">
                                </div>
                                <cc1:AutoCompleteExtender ID="JobDetailExtender" runat="server" TargetControlID="txtJobNumber"
                                    CompletionListElementID="divwidthCust_Loc" ServicePath="~/WebService/JobNumberAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="5" BehaviorID="divwidthCust_Loc"
                                    ContextKey="1665" UseContextKey="True" OnClientItemSelected="OnJobSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>
                                Type of Expense
                                <asp:RequiredFieldValidator ID="rfvExpenseType" runat="server" ValidationGroup="Required"
                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlExpenseType" InitialValue="0"
                                    Text="Required" ErrorMessage="Please Select Type of Expense."> </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlExpenseType" runat="server" AppendDataBoundItems="false" DataSourceID="DataSourceExpense"
                                    DataTextField="sName" DataValueField="lid" ToolTip="Select Type Of Expense." AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlExpenseType_OnSelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
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
                                Planning Date
                            </td>
                            <td>
                                <asp:Label ID="lblPlanningDate" runat="server"></asp:Label>
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
                </fieldset>
                <asp:UpdatePanel ID="updPnlVendor" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <fieldset runat="server" id="fldVendor">
                    <legend>Vendor Detail</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>Vendor type
                            </td>
                            <td>
                                <asp:DropDownList ID="ddGSTINType" runat="server" OnSelectedIndexChanged="ddGSTINType_SelectedIndexChanged"
                                    AutoPostBack="true">
                                    <asp:ListItem Text="Registered" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Un-Registered" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Foreign Party" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>Supplier GSTIN No
                            </td>
                            <td>
                                <asp:TextBox ID="txtSupplierGSTIN" runat="server"></asp:TextBox>
                                <div id="divwidthGST"></div>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtenderGST" runat="server" TargetControlID="txtSupplierGSTIN"
                                    CompletionListElementID="divwidthGST" ServicePath="../WebService/FAVendorAutoComplete.asmx"
                                    ServiceMethod="GetCompletionListByGSTIN" MinimumPrefixLength="2" BehaviorID="divwidthGST"
                                    ContextKey="8844" UseContextKey="True" OnClientItemSelected="OnGSTINSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight" DelimiterCharacters="">
                                </cc1:AutoCompleteExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>Vendor Name
                                 <asp:RequiredFieldValidator ID="rfvVendorName" runat="server" ValidationGroup="Required" InitialValue=""
                                     ControlToValidate="txtVendorName" Text="Required" ErrorMessage="Please Enter Vendor Name" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVendorName" runat="server" Width="90%" Enabled="false"></asp:TextBox>
                                <div id="divwidthVendor"></div>
                                <cc1:AutoCompleteExtender ID="AutoCompleteVendor" runat="server" TargetControlID="txtVendorName"
                                    CompletionListElementID="divwidthVendor" ServicePath="../WebService/FAVendorAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthVendor"
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
                                    ServiceMethod="GetCompletionListByPANNo" MinimumPrefixLength="2" BehaviorID="divwidthGST"
                                    ContextKey="8854" UseContextKey="True" OnClientItemSelected="OnPanNoSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight" DelimiterCharacters="">
                                </cc1:AutoCompleteExtender>
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
                                    MinimumValueMessage="Invalid Payment Due Date" MaximumValueMessage="Invalid Payment Due Date" ValidationGroup="Required"
                                    MinimumValue='<%#DateTime.Now.ToString("dd/MM/yyyy") %>' MaximumValue='01/01/2022'
                                    EmptyValueBlurredText="Required" EmptyValueMessage="Invalid Payment Due Date" runat="Server"></cc1:MaskedEditValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPaymentDueDate" runat="server" Width="125px" placeholder="dd/mm/yyyy"></asp:TextBox>
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
                                    InvalidValueMessage="Payment Required Date is invalid" InvalidValueBlurredMessage="Invalid Payment Required Date" SetFocusOnError="true"
                                    MinimumValueMessage="Invalid Payment Requirede Date" MaximumValueMessage="Invalid Payment Required Date" ValidationGroup="Required"
                                    runat="Server"></cc1:MaskedEditValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPaymentRequiredDate" runat="server" Width="125px" placeholder="dd/mm/yyyy" ToolTip="Enter Payment Required Date"></asp:TextBox>
                                <asp:Image ID="imgPaymentRequdDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Vendor Payment Terms(Days)
                            </td>
                            <td>
                                <asp:TextBox ID="txtPaymentTerms" runat="server" MaxLength="4" TextMode="Number" Width="80px"></asp:TextBox>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </fieldset>
                    </ContentTemplate>
                    </asp:UpdatePanel>
                <asp:UpdatePanel ID="updPnlInvoice" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <fieldset>
                    <legend>Invoice Detail</legend>
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
                                <asp:RequiredFieldValidator ID="rfvConsigneeName" runat="server" ValidationGroup="Required" InitialValue="" Enabled="false"
                                     ControlToValidate="txtCongisgneeName" Text="Required" ErrorMessage="Please Enter Invoice Party Name" SetFocusOnError="true"></asp:RequiredFieldValidator>
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
                                <asp:DropDownList ID="ddInvoiceType" runat="server" OnSelectedIndexChanged="ddInvoiceType_SelectedIndexChanged" AutoPostBack="true">
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
                            <td>
                                <asp:Label id="lblInvoiceNo" Text="Invoice No" runat="server"></asp:Label> 
                                
                                <asp:RequiredFieldValidator ID="RFVEnquiryNo" runat="server" ControlToValidate="txtInvoiceNo"
                                     ErrorMessage="Invoice No Required" Text="Required" ValidationGroup="Required" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
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
                                    MinimumValueMessage="Invalid Invoice Date" MaximumValueMessage="Invoice Invalid Date" MinimumValue="01/01/2019"
                                    ValidationGroup="Required"
                                    EmptyValueMessage="Invoice Date Required" EmptyValueBlurredText="Required" runat="Server"></cc1:MaskedEditValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtInvoiceDate" runat="server" Width="125px" placeholder="dd/mm/yyyy" ToolTip="Enter Invoice Date"></asp:TextBox>
                                <asp:Image ID="imgInvoiceDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>Advance Received
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblAdvanceReceived" runat="server" RepeatDirection="Horizontal"
                                    OnSelectedIndexChanged="rblAdvanceReceived_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>Advance Amount
                                <asp:RequiredFieldValidator ID="RFVAdvanceAmt" runat="server" ControlToValidate="txtAdvanceAmount" InitialValue="" Enabled="false"
                                    ErrorMessage="Required" ValidationGroup="Required" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAdvanceAmount" runat="server" Width="125px" TextMode="Number" Enabled="false" MaxLength="10"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Payment Type
                            <asp:RequiredFieldValidator ID="rfvpaytype" runat="server" ValidationGroup="Required"
                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlPaymentType" InitialValue="0"
                                Text="Required" ErrorMessage="Please Select Payment Type"> </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPaymentType" runat="server" AppendDataBoundItems="true" DataSourceID="DataSourcePaymentType"
                                    DataTextField="sName" DataValueField="lid">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblTotalInvoiceValue" runat="server" Text="Total Invoice Value" ></asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="Required"
                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtTotalInvoiceValue" InitialValue=""
                                Text="Required" ErrorMessage="Please Enter Total Value"> </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTotalInvoiceValue" runat="server" Width="125px" MaxLength="12"></asp:TextBox>
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
                                        <td  id="tdtxtInterestAmnt" runat="server" visible="false">
                                            <asp:TextBox ID="txtInterestAmnt" runat="server" OnTextChanged="txtInterestAmnt_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                               <asp:CompareValidator ID="CVInterestAmount" runat="server" ControlToValidate="txtInterestAmnt"
                                                Display="Dynamic" SetFocusOnError="true" Type="Double" Operator="DataTypeCheck"
                                                ErrorMessage="Invalid Interest Amount" ValidationGroup="Required"></asp:CompareValidator>
                                        </td>
                                        <td width="40%"></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            
                            <td>Upload Document</td>
                            <td>
                                <asp:FileUpload ID="fuDocument" runat="server" />
                            </td>
                            <td>Remark
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" MaxLength="200"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblMultipleChargeCode" runat="server" Text="Multiple GST rates in the invoice ?"></asp:Label>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblMultiChargeCode" runat="server" RepeatDirection="Horizontal" Enabled="true"
                                    onclick="ChargeVendorTypeValidation()" OnSelectedIndexChanged="rblMultiChargeCode_SelectedIndexChanged"
                                    AutoPostBack="true">
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td></td>
                            <td></td>
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
                            <td>
                                Currency
                            </td>
                            <td>
                                 <asp:DropDownList ID="ddCurrency" runat="server" DataSourceID="dataSourceCurrency"
                                    AppendDataBoundItems="true" DataValueField="lId" DataTextField="Currency" TabIndex="10">
                                <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="txtExchangeRate" runat="server" Width="50px" Text="1" Enabled="true"></asp:TextBox>
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
                
               </ContentTemplate>
                    </asp:UpdatePanel> 
                </fieldset>
            </div>
            <div id="divDatasourc">
                <asp:SqlDataSource ID="DataSourcePaymentType" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetPaymentTypeMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceExpense" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetRequestTypeMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="dataSourceCurrency" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetCurrencyMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

