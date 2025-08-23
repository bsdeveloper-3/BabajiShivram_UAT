<%@ Page Title="Update Invoice" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PaymentRequestEdit.aspx.cs" 
    Inherits="AccountExpense_PaymentRequestEdit" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <cc1:toolkitscriptmanager id="ScriptManager1" runat="server" scriptmode="Release">
    </cc1:toolkitscriptmanager>
        <script type="text/javascript">
        
        function OnVendorSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=txtSupplierGSTIN.ClientID%>').value = results.GSTIN;
            $get('<%=txtVendorName.ClientID%>').value = results.Name;
            $get('<%=hdnVendorCode.ClientID%>').value = results.Code;
            $get('<%=txtVendorPANNo.ClientID%>').value = results.PANNo;
            $get('<%=txtPaymentTerms.ClientID%>').value = results.CreditDays;

            if (results.Code.includes('BSC') || results.Code.includes('BS4S') ) {
                
                $get('<%=ddRIM.ClientID%>').selectedIndex = 1;
                $get('<%=ddGSTINType.ClientID%>').selectedIndex = 0;
            }
            else if (results.PANNo == 'PANNOTAVBL') {
                $get('<%=ddRIM.ClientID%>').selectedIndex = 0;
                $get('<%=ddGSTINType.ClientID%>').selectedIndex = 2;
            }
            else if (results.GSTIN == '') {
                $get('<%=ddGSTINType.ClientID%>').selectedIndex = 1;
            }
            else if (results.GSTIN != '') {
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

            if (results.Code.includes('BSC') || results.Code.includes('BS4S')) {

                $get('<%=ddRIM.ClientID%>').selectedIndex = 1;
                $get('<%=ddGSTINType.ClientID%>').selectedIndex = 0;
            }

            if (results.PANNo == 'PANNOTAVBL') {
                $get('<%=ddGSTINType.ClientID%>').selectedIndex = 2;
            }

        }

        function OnPanNoSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=txtSupplierGSTIN.ClientID%>').value = results.GSTIN;
            $get('<%=txtVendorName.ClientID%>').value = results.Name;
            $get('<%=hdnVendorCode.ClientID%>').value = results.Code;
            $get('<%=txtVendorPANNo.ClientID%>').value = results.PANNo;
            $get('<%=txtPaymentTerms.ClientID%>').value = results.CreditDays;

            if (results.Code.includes('BSC') || results.Code.includes('BS4S')) {

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
            }
        }

        function OnConsingeeSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=hdnConsigneeCode.ClientID%>').value = results.Code;
         
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
        function CheckConsgineeGSTIN()
        {
            
            var varJobGSTIN = $get('<%=lblConsgneeGSTIN.ClientID%>').innerText;

            var varUserGSTIN = $get('<%=txtCongisgneeGSTIN.ClientID%>').value;

            if (varJobGSTIN.toUpperCase() == varUserGSTIN.toUpperCase())
            {
                $get('<%=ddRIM.ClientID%>').selectedIndex = 1;
            }
            else {
                alert('Consginee GSTIN is different from Bill of Entry GSTIN!')
                $get('<%=ddRIM.ClientID%>').selectedIndex = 0;
            }
            }


    </script>
        <asp:UpdatePanel ID="upFillDetails" runat="server">
        <ContentTemplate>
            <div>
                <div align="center">
                    <asp:HiddenField ID="hdnJobId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnBranchId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnModuleId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnNewPaymentLid" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnVendorCode" runat="server" />
                    <asp:HiddenField ID="hdnStatusId" runat="server" />
                    <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                    <asp:ValidationSummary ID="vsFillDetails" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
                </div>

                <fieldset>
                    <legend>Payment Request</legend>
                    <div class="m clear">
                        <asp:Button ID="btnSubmit" Text="Update" OnClick="btnSubmit_Click" runat="server" ValidationGroup="Required" />
                        <asp:Button ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false" runat="server" />
                    </div>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>Job Number
                            </td>
                            <td width="80%" colspan="3" >
                                <asp:Label ID="lblJobNumber" runat="server" ></asp:Label>
                                
                                <asp:Label ID="lblConsgneeGSTIN" runat="server"></asp:Label>
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
                        <tr>
                            <td>
                                Vendor type
                            </td>
                            <td>
                                <asp:DropDownList ID="ddGSTINType" runat="server">
                                    <asp:ListItem Text="Registered" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Un-Registered" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Foreign Party" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                Supplier GSTIN No
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
                            </td
                        </tr>
                        <tr>
                             <td>
                                Vendor Name
                            </td>
                            <td>
                                <asp:TextBox ID="txtVendorName" runat="server" Width="90%"></asp:TextBox>
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
                                PAN No</td>
                            <td>
                                <asp:TextBox ID="txtVendorPANNo" runat="server"></asp:TextBox>
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
                             <td>
                                Invoice To (GSTIN No)
                            </td>
                            <td>
                                <asp:TextBox ID="txtCongisgneeGSTIN" runat="server" onfocusout="CheckConsgineeGSTIN()"></asp:TextBox>
                            </td>
                            <td>
                                Invoice Party
                            </td>
                            <td>
                                <asp:TextBox ID="txtCongisgneeName" runat="server"></asp:TextBox>
                                <div id="divwidthConsignee"></div>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtCongisgneeName"
                                    CompletionListElementID="divwidthConsignee" ServicePath="../WebService/FAVendorAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthConsignee"
                                    ContextKey="1163" UseContextKey="True" OnClientItemSelected="OnConsingeeSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight" DelimiterCharacters="">
                                </cc1:AutoCompleteExtender>

                                <asp:HiddenField ID="hdnConsigneeCode" runat="server"></asp:HiddenField>
                            </td>
                        <tr>
                            <td>
                                RIM/Non-RIM</td>
                            <td>
                                <asp:DropDownList ID="ddRIM" runat="server">
                                    <asp:ListItem Text="RIM" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Non-RIM" Value="2" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                             <td>
                                Invoice Type
                            </td>
                            <td>
                                <asp:DropDownList ID="ddInvoiceType" runat="server">
                                    <asp:ListItem Text="Final" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Proforma" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                Vendor Payment Terms</td>
                            <td>
                                <asp:TextBox ID="txtPaymentTerms" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Vendor Payment due date
                                <cc1:calendarextender id="CalExtPayDueDate" runat="server" firstdayofweek="Sunday" popupbuttonid="imgPaymentDueDate"
                                    format="dd/MM/yyyy" popupposition="BottomRight" targetcontrolid="txtPaymentDueDate">
                                </cc1:calendarextender>
                                <cc1:maskededitextender id="MskEdtPayDueDate" targetcontrolid="txtPaymentDueDate" mask="99/99/9999" messagevalidatortip="true"
                                    masktype="Date" autocomplete="false" runat="server">
                                </cc1:maskededitextender>
                                <cc1:maskededitvalidator id="MskEdtValPayDueDate" controlextender="MskEdtPayDueDate" controltovalidate="txtPaymentDueDate" isvalidempty="true"
                                    invalidvaluemessage="Payment Due Date is invalid" invalidvalueblurredmessage="Invalid Payment Due Date" setfocusonerror="true"
                                    minimumvaluemessage="Invalid Payment Due Date" maximumvaluemessage="Invalid Payment Due Date" minimumvalue="01/01/2020" maximumvalue="31/12/2025"
                                    runat="Server"></cc1:maskededitvalidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPaymentDueDate" runat="server" Width="125px" placeholder="dd/mm/yyyy" TabIndex="3" ToolTip="Enter Planning Date."></asp:TextBox>
                                <asp:Image ID="imgPaymentDueDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            
                            </td>
                            <td>
                                Payment Required Date
                                <cc1:calendarextender id="CalExtPayReqrdDate" runat="server" firstdayofweek="Sunday" popupbuttonid="imgPaymentRequdDate"
                                    format="dd/MM/yyyy" popupposition="BottomRight" targetcontrolid="txtPaymentRequiredDate">
                                </cc1:calendarextender>
                                <cc1:maskededitextender id="MskEdtPayReqrdDate" targetcontrolid="txtPaymentRequiredDate" mask="99/99/9999" messagevalidatortip="true"
                                    masktype="Date" autocomplete="false" runat="server">
                                </cc1:maskededitextender>
                                <cc1:maskededitvalidator id="MskEdtValPayReqrdDate" controlextender="MskEdtPayReqrdDate" controltovalidate="txtPaymentRequiredDate" isvalidempty="true"
                                    invalidvaluemessage="Payment Required Date is invalid" invalidvalueblurredmessage="Invalid Payment Required Date" setfocusonerror="true"
                                    minimumvaluemessage="Invalid Payment DuRequirede Date" maximumvaluemessage="Invalid Payment Required Date" minimumvalue="01/01/2020" maximumvalue="31/12/2025"
                                    runat="Server"></cc1:maskededitvalidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPaymentRequiredDate" runat="server" Width="125px" placeholder="dd/mm/yyyy" TabIndex="3" ToolTip="Enter Planning Date."></asp:TextBox>
                                <asp:Image ID="imgPaymentRequdDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            
                            </td>
                        </tr>
                        <tr>
                             <td>
                                Invoice No
                            </td>
                            <td>
                                <asp:TextBox ID="txtInvoiceNo" runat="server" Width="125px"></asp:TextBox>
                            </td>
                            <td>Invoice Date
                                <cc1:calendarextender id="CalExtInvoiceDate" runat="server" firstdayofweek="Sunday" popupbuttonid="imgInvoiceDate"
                                    format="dd/MM/yyyy" popupposition="BottomRight" targetcontrolid="txtInvoiceDate">
                                </cc1:calendarextender>
                                <cc1:maskededitextender id="MskEdtInvoice" targetcontrolid="txtInvoiceDate" mask="99/99/9999" messagevalidatortip="true"
                                    masktype="Date" autocomplete="false" runat="server">
                                </cc1:maskededitextender>
                                <cc1:maskededitvalidator id="MskEdtValInvoice" controlextender="MskEdtInvoice" controltovalidate="txtInvoiceDate" isvalidempty="true"
                                    invalidvaluemessage="Invoice Date is invalid" invalidvalueblurredmessage="Invalid Invoice Date" setfocusonerror="true"
                                    minimumvaluemessage="Invalid Invoice Date" maximumvaluemessage="Invalid Date" minimumvalue="01/01/2015" maximumvalue="31/12/2025"
                                    runat="Server"></cc1:maskededitvalidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtInvoiceDate" runat="server" Width="125px" placeholder="dd/mm/yyyy" TabIndex="3" ToolTip="Enter Planning Date."></asp:TextBox>
                                <asp:Image ID="imgInvoiceDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                        </tr>
                        <tr>
                             <td>
                                Advance Received
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblAdvanceReceived" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                Advance Amount
                            </td>
                            <td>
                                <asp:TextBox ID="txtAdvanceAmount" runat="server" Width="125px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Payment Type
                            <asp:RequiredFieldValidator ID="rfvpaytype" runat="server" ValidationGroup="Required"
                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlPaymentType" InitialValue="0"
                                Text="*" ErrorMessage="Please Select Payment Type"> </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPaymentType" runat="server" AppendDataBoundItems="true" DataSourceID="DataSourcePaymentType"
                                    DataTextField="sName" DataValueField="lid">
                                </asp:DropDownList>
                            </td>
                            <td>Upload Document</td>
                            <td>
                                <asp:FileUpload ID="fuDocument" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Total Invoice Value
                            </td>
                            <td>
                                <asp:TextBox ID="txtTotalInvoiceValue" runat="server" Width="125px"></asp:TextBox>
                            </td>
                           <td>
                               Remark
                               <asp:RequiredFieldValidator ID="rfvRemark" runat="server" ValidationGroup="Required" InitialValue=""
                                   ControlToValidate="txtRemark" Text="Required" ErrorMessage="Please Enter Remark" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" MaxLength="200"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                Total Taxable Value: &nbsp;<asp:Label ID="lblTotalTaxableValue" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;
                                Total Tax: &nbsp;<asp:Label ID="lblTotalGST" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;
                                Total Value: &nbsp;<asp:Label ID="lblTotalValue" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                
                <fieldset runat="server" id="fldInvoiceItem" visible="true">
                    <div class="clear">
                    </div>
                        <legend>Charge Detail</legend>
                        <asp:Button ID="btnNewCharge" Text="Add New Charge" runat="server" OnClick="btnNewCharge_Click" />
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
                                        <asp:BoundField DataField="OtherDeduction" HeaderText="Other Deduction" />
                                        <asp:BoundField DataField="IGSTRate" HeaderText="IGST Rate" />
                                        <asp:BoundField DataField="IGSTAmount" HeaderText="IGST Amt" />
                                        <asp:BoundField DataField="CGSTRate" HeaderText="CGST Rate" />
                                        <asp:BoundField DataField="CGSTAmount" HeaderText="CGST Amt" />
                                        <asp:BoundField DataField="SGSTRate" HeaderText="SGST Rate" />
                                        <asp:BoundField DataField="SGSTAmount" HeaderText="SGST Amt" />
                                        <asp:BoundField DataField="Amount" HeaderText="Total Value" />
                                        <asp:BoundField DataField="Remark" HeaderText="Remark" />
                                        <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEditCharge" runat="server" Text="Edit" OnClick="lnkEditCharge_Click" CommandArgument='<%#BIND("lid") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remove">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnlRemoveCharge" runat="server" Text="Remove" OnClick="lnlRemoveCharge_Click" CommandArgument='<%#BIND("lid") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                </fieldset>
        </div>

            <asp:Button ID="modelPopup" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="modelPopup" CancelControlID="btnCancel" PopupControlID="Panel1"></cc1:ModalPopupExtender>
            <asp:Panel ID="Panel1" Style="display: none" runat="server">
                <fieldset class="ModalPopupPanel">
                                <div title="Charge Details" class="header">
                                    <textbox>Add New Charge Detail</textbox>
                                </div>
                                <div class="AutoExtenderList">
                                    <td>
                                        <asp:Label ID="lblChargeError" runat="server"></asp:Label>
                                        <asp:Button ID="btnClosePopup" runat="server" Text="Close" OnClick="btnClosePopup_Click" ToolTip="Cancel" />
                                    </td>

                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
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
                                <asp:DropDownList ID="ddGSTRate" runat="server">                                    
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
                            </td>
                            <td>
                                <asp:TextBox ID="txtChargeRemark" runat="server" TextMode="MultiLine" MaxLength="200"></asp:TextBox>
                            </td>
                            <td colspan="3">
                                <asp:Button ID="btnAddCharges" runat="server" Text="Add Charges" OnClick="btnAddCharges_Click" />
                            </td>
                        </tr>
                    </table>
                                </div>
                            </fieldset>
            </asp:Panel>
            <asp:HiddenField ID="HiddenField1" runat="server" />
            <div id="divDatasourc"> 
                <asp:SqlDataSource ID="InvoiceDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetInvoiceItem" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="InvoiceID" SessionField="InvoiceID" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
            <div id="divDatasourc12">
                <asp:SqlDataSource ID="DataSourcePaymentType" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetPaymentTypeMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </div>
    </ContentTemplate>
            </asp:UpdatePanel>
</asp:Content>

