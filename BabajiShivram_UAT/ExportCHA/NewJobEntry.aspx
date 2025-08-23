<%@ Page Title="New Export Job" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="NewJobEntry.aspx.cs" Inherits="ExportCHA_NewJobEntry" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
    </asp:ScriptManager>
    <style type="text/css">
        .modalBackground {
            background-color: black;
            filter: alpha(opacity=90);
            opacity: 0.5;
        }

        .modalPopup1 {
            border-radius: 5px;
            background-color: #FFFFFF;
            border-width: 1px;
            border-style: solid;
            border-color: black;
            padding-top: 5px;
            padding-left: 3px;
            width: 600px;
            height: 300px;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function toggleDiv(chk, FUid, RFVId) {
            var checkboxId = document.getElementById(chk);
            var fileUploadId = document.getElementById(FUid);
            var RFVDocument = document.getElementById(RFVId);

            if (checkboxId.checked == true && fileUploadId.disabled == true) {
                checkboxId.checked = true;
                fileUploadId.disabled = false;
                ValidatorEnable(RFVDocument, true);
            }
            else if (checkboxId.checked == false && fileUploadId.disabled == true) {

                checkboxId.checked = false;
                fileUploadId.disabled = true;
                ValidatorEnable(RFVDocument, false);
            }

            else if (fileUploadId.disabled == true) {
                fileUploadId.disabled = false;
                ValidatorEnable(RFVDocument, true);
            }
            else {
                fileUploadId.disabled = true;
                ValidatorEnable(RFVDocument, false);
            }
        }
    </script>
    <script type="text/javascript">
        function ValidatePage() {
            var POD = document.getElementById('<%=hdnDischargePortId.ClientID%>');
            alert(POD.value);
        }

        function ActiveTabChanged12() {
            /* Clear Error Message on Tab change Event */
            document.getElementById('<%=lblError.ClientID%>').innerHTML = '';
            document.getElementById('<%=lblError.ClientID%>').className = '';
        }
    </script>
    <script type="text/javascript">
        function OnCustomerSelected(source, eventArgs) {
            // alert(eventArgs.get_value());
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnCustId').value = results.ClientId;
         
        }
       
        function OnShipperSelected(source, eventArgs) {
            var resultsA = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnShipperId').value = resultsA.ShipperId;
        
        }
       
        function OnPortOfDischargeSelected(source, eventArgs) {
            var resultsPOD = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnDischargePortId').value = resultsPOD.PortOfLoadingId;
         
        }

        function OnPortOfLoadingSelected(source, eventArgs) {
            var resultsPOL = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnLoadingPortId').value = resultsPOL.PortId;
         
        }

        function OnDestCountrySelected(source, eventArgs) {
            var resultsDC = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnDestCountryId').value = resultsDC.CountryId;
         
        }

        function OnCountrySelected(source, eventArgs) {
            var resultsC = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnCountryId').value = resultsC.CountryId;
         
        }

    </script>
    <script type="text/javascript">
        var atLeast = 1
        function CheckFields() {
            Page_ClientValidate("Required");
            // Port Of Loading
            var POL = document.getElementById('<%=hdnLoadingPortId.ClientID%>');
            if (POL.value == "0") {
                alert('Please Select Port Of Loading.');
                return false;
            }

            // Port Of Discharge
            var POD = document.getElementById('<%=hdnDischargePortId.ClientID%>');
            if (POD.value == "0") {
                alert('Please Select Port Of Discharge.');
                return false;
            }

            // Destination Country
            var DestCnt = document.getElementById('<%=hdnDestCountryId.ClientID%>');
            if (DestCnt.value == "0") {
                alert('Please Select Destination Country.');
                return false;
            }

            // Consignment Country
            var Country = document.getElementById('<%=hdnCountryId.ClientID%>');
            if (Country.value == "0") {
                alert('Please Select Consignment Country.');
                return false;
            }

            // Customer
            var Customer = document.getElementById('<%=hdnCustId.ClientID%>');
            if (Customer.value == "0") {
                alert('Please Select Customer Name.');
                return false;
            }

            // Applicable Fields function
            var ddlMode = document.getElementById('<%=ddlMode.ClientID%>');
            var ModeId = ddlMode.options[ddlMode.selectedIndex].value;
            var ddlBabajiBranch = document.getElementById('<%=ddlBabajiBranch.ClientID%>');
            var BranchId = ddlBabajiBranch.options[ddlBabajiBranch.selectedIndex].value;
            if (ModeId == "1" && BranchId == "2") // air & Mumbai Cargo then atleast 1 field to be checked..!!
            {
             <%--   var CheckBoxAppicable = document.getElementById('<%=chkApplicable.ClientID%>');--%>
                var checkbox = CheckBoxAppicable.getElementsByTagName("input");
                var counter = 0;
                for (var i = 0; i < checkbox.length; i++) {
                    if (checkbox[i].checked) {
                        counter++;
                    }
                }
                if (atLeast > counter) {
                    alert('Please check atleast one Applicable Field..!!');
                    return false;
                }
                else {
                    Page_ClientValidate("Required");
                }
            }
            else {
                Page_ClientValidate("Required");
            }
        }
    </script>
    <asp:UpdatePanel ID="upJobDetail" runat="server">
        <ContentTemplate>
            <div>
                <div align="center">
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
                    <asp:ValidationSummary ID="ValSummaryInvoice" runat="server" ShowMessageBox="true"
                        ShowSummary="false" ValidationGroup="RequiredProduct" />
                    <asp:ValidationSummary ID="ValSummaryContainer" runat="server" ShowMessageBox="true"
                        ShowSummary="false" ValidationGroup="valContainer" />
                    <asp:HiddenField ID="hdnLoadingPortId" Value="0" runat="server" />
                    <asp:HiddenField ID="hdnDischargePortId" Value="0" runat="server" />
                    <asp:HiddenField ID="hdnCustDocFolder" runat="server" />
                    <asp:HiddenField ID="hdnJobFileDir" runat="server" />
                    <asp:HiddenField ID="hdnDocPath" runat="server" />
                    <asp:HiddenField ID="hdnCustId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnShipperId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnCountryId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnDestCountryId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnNewJobId" runat="server" />
                </div>

                <div class="m clear">
                    <asp:Button ID="btnSubmit" Text="Save Job" OnClientClick="return CheckFields();" OnClick="btnSubmit_Click" runat="server" ValidationGroup="Required"
                        TabIndex="39" />
                    <asp:Button ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false" TabIndex="40"
                        runat="server" />
                </div>
                <fieldset>
                    <legend>Job Detail</legend>
                    <div id="divInstruction" class="info" runat="server">
                    </div>

                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                           <td>
                              Pre Alert
                           </td>
                           <td>
                              <asp:DropDownList ID="ddlPreAlertExport" runat="server" AutoPostBack="true"  Width="300px"
                                  OnSelectedIndexChanged="ddlPreAlertExport_SelectedIndexChanged">                                  
                              </asp:DropDownList>
                            </td>
                            <td>
                                 <asp:Label ID="lblFrJobNoTitle" runat="server" Text="Freight Job No" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblFrJobNo" runat="server" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Job Number
                                <asp:RequiredFieldValidator ID="rfvJObNo" runat="server" ValidationGroup="Required"
                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtJobNumber"
                                   InitialValue="" Text="*" ErrorMessage="Please Enter Job Number"> </asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revJobNo" runat="server" ControlToValidate="txtJobNumber" Display="Dynamic"
                                    ValidationGroup="Required" SetFocusOnError="true" ErrorMessage="Invalid Job Number (eg:CBMBOE-1617-00001603)"
                                    ValidationExpression="^[A-Z]{6}(-)[0-9]{4}(-)[0-9]{8}$" ForeColor="Red"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtJobNumber" runat="server" TabIndex="1" Width="50%" ToolTip="Shows Job Number."></asp:TextBox>

                                <%-- Job format :- (CBMBOE-1617-00001603)  --%>
                            </td>
                            <td>Babaji Branch
                                <asp:RequiredFieldValidator ID="rfvBranch" runat="server" ValidationGroup="Required"
                                    Display="Dynamic" SetFocusOnError="true" InitialValue="0" ControlToValidate="ddlBabajiBranch"
                                    Text="*" ErrorMessage="Please Select Babaji Branch"> </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlBabajiBranch" runat="server" Width="50%" TabIndex="2" DataSourceID="DataSourceBranch"
                                    DataTextField="BranchName" DataValueField="BranchId" AutoPostBack="true" OnSelectedIndexChanged="ddlBabajiBranch_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Customer Name
                                <asp:RequiredFieldValidator ID="rfvCustomerName" ValidationGroup="Required" SetFocusOnError="true"
                                    runat="server" Display="Dynamic" ControlToValidate="txtCustomer" ErrorMessage="Please Enter Customer Name"
                                    Text="*"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCustomer" Width="80%" runat="server" ToolTip="Enter Customer Name."
                                    CssClass="SearchTextbox" placeholder="Search" TabIndex="3" AutoPostBack="true" OnTextChanged="txtCustomer_TextChanged"></asp:TextBox>
                                <div id="divwidthCust" runat="server">
                                </div>
                                <cc1:AutoCompleteExtender ID="CustomerExtender" runat="server" TargetControlID="txtCustomer"
                                    CompletionListElementID="divwidthCust" ServicePath="~/WebService/CustomerAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCust"
                                    ContextKey="1234" UseContextKey="True" OnClientItemSelected="OnCustomerSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>Shipper Name
                                <asp:RequiredFieldValidator ID="rfvShipperName" ValidationGroup="Required" runat="server"
                                    SetFocusOnError="true" Display="Dynamic" ControlToValidate="ddlShipper" ErrorMessage="Please Select Shipper Name"
                                    Text="*" InitialValue="0"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlShipper" TabIndex="4" runat="server" Width="82%" DataSourceID="DataSourceShipper" DataTextField="ShipperName" DataValueField="ShipperId"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Customer Division/Branch
                                <asp:RequiredFieldValidator ID="RFVDivision" runat="server" ValidationGroup="Required" SetFocusOnError="true"
                                    InitialValue="0" ControlToValidate="ddDivision" Text="*" ErrorMessage="Please Select Customer Division"> </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddDivision" runat="server" AutoPostBack="true" Width="50%"
                                    OnSelectedIndexChanged="ddDivision_SelectedIndexChanged" TabIndex="5">
                                    <asp:ListItem Value="0" Text="-Select-"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>Customer Plant
                                <asp:RequiredFieldValidator ID="RFVPlant" runat="server" ValidationGroup="Required" SetFocusOnError="true"
                                    InitialValue="0" ControlToValidate="ddPlant" Text="*" ErrorMessage="Please Select Customer Plant"> </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddPlant" runat="server" TabIndex="6">
                                    <asp:ListItem Value="0" Text="-Select-"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Consignee Name
                                <asp:RequiredFieldValidator ID="rfvConsignee" ValidationGroup="Required" runat="server"
                                    SetFocusOnError="true" Display="Dynamic" ControlToValidate="txtConsignee" ErrorMessage="Please Enter Consignee"
                                    Text="*"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtConsignee" runat="server" TabIndex="7" Width="80%" ToolTip="Enter Consignee."></asp:TextBox>
                                <asp:HiddenField ID="hdnConsigneeId" runat="server" Value="0" />
                            </td>
                            <td>Customer Reference No
                            </td>
                            <td>
                                <asp:TextBox ID="txtCustRefNo" MaxLength="100" TabIndex="8" ToolTip="Enter Customer Reference No"
                                    runat="server" Width="60%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Mode
                                <asp:RequiredFieldValidator ID="rfvMode" ValidationGroup="Required" runat="server"
                                    SetFocusOnError="true" Display="Dynamic" ControlToValidate="ddlMode" InitialValue="0"
                                    ErrorMessage="Please Select Mode." Text="*"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlMode" runat="server" TabIndex="9" Width="40%" AutoPostBack="true" OnSelectedIndexChanged="ddlMode_OnSelectedIndexChanged">
                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Air" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Sea" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:HiddenField ID="hdnMode" runat="server" />
                            </td>
                            <td>Transport By
                                <asp:RequiredFieldValidator ID="rfvTransportBy" ValidationGroup="Required" runat="server"
                                    SetFocusOnError="true" Display="dynamic" ControlToValidate="ddlTransportBy" InitialValue="0"
                                    ErrorMessage="Please Select Transport By" Text="*"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTransportBy" runat="server" TabIndex="10" ToolTip="Select Transport By" Width="40%" AutoPostBack="true" OnSelectedIndexChanged="ddlTransportBy_OnSelectedIndexChanged">
                                    <asp:ListItem Selected="True" Text="-Select-" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Babaji Shivram" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Customer" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </td>

                        </tr>
                        <tr id="trTpPickupLocation" runat="server">
                            <td>Pickup Location From  
                                <asp:RequiredFieldValidator ID="rfvpickupfrom" ValidationGroup="Required" runat="server"
                                    SetFocusOnError="true" Display="dynamic" ControlToValidate="txtPickupLocation"
                                    ErrorMessage="Please Enter Pickup Location From." Text="*"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPickupLocation" runat="server" Width="80%" ToolTip="Enter Pickup Location From." TabIndex="11"></asp:TextBox>
                            </td>
                            <td>To 
                                <asp:RequiredFieldValidator ID="rfvdropto" ValidationGroup="Required" runat="server"
                                    SetFocusOnError="true" Display="dynamic" ControlToValidate="txtLocationTo"
                                    ErrorMessage="Please Enter To." Text="*"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLocationTo" MaxLength="100" ToolTip="Enter Destination To." TabIndex="12"
                                    runat="server" Width="40%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trTpPickupPersonDetails" runat="server">
                            <td>Pick Up Date
                                <cc1:CalendarExtender ID="calPickupDate" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="imgPickUpDate"
                                    Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtPickUpDate">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="meePickupDate" TargetControlID="txtPickUpDate" Mask="99/99/9999" MessageValidatorTip="true"
                                    MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="mevPickUpDate" ControlExtender="meePickupDate" ControlToValidate="txtPickUpDate" IsValidEmpty="true"
                                    InvalidValueMessage="Pick Up Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true"
                                    MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2014" MaximumValue="31/12/2025"
                                    runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPickUpDate" runat="server" Width="100px" placeholder="dd/mm/yyyy" TabIndex="13"></asp:TextBox>
                                <asp:Image ID="imgPickUpDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                            <td>Pick Up Person Name
                            </td>
                            <td>
                                <asp:TextBox ID="txtPickupPersonName" runat="server" ToolTip="Pickup Person Name." TabIndex="14" Width="40%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trTpPickupPersonDetails2" runat="server">
                            <td>Mobile No
                            </td>
                            <td>
                                <asp:TextBox ID="txtPickupMobileNo" runat="server" TabIndex="15"></asp:TextBox>
                            </td>
                            <td>Dimension
                                <asp:RequiredFieldValidator ID="rfvDimension" runat="server" ControlToValidate="txtDimension"
                                    SetFocusOnError="true" ErrorMessage="Please Enter Dimension." Display="Dynamic"
                                    Text="*" ValidationGroup="Required"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDimension" runat="server" MaxLength="10" TabIndex="37" Width="95%" TextMode="MultiLine" Rows="2"
                                    ToolTip="Enter Dimension."></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Port of Loading
                                <asp:RequiredFieldValidator ID="rfvPortOfDischarge" ValidationGroup="Required" runat="server"
                                    SetFocusOnError="true" Display="Dynamic" ControlToValidate="txtPortOfLoading"
                                    ErrorMessage="Please Select Port of Loading" Text="*"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPortOfLoading" Width="80%" runat="server" TabIndex="16" CssClass="SearchTextbox"
                                    placeholder="Search" ToolTip="Enter Port Of Loading." MaxLength="100" />
                                <div id="divwidthLoadingPort">
                                </div>
                                <cc1:AutoCompleteExtender ID="AutoCompletePortLoading" runat="server" TargetControlID="txtPortOfLoading"
                                    CompletionListElementID="divwidthLoadingPort" ServicePath="~/WebService/PortAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthLoadingPort"
                                    ContextKey="1234" UseContextKey="True" OnClientItemSelected="OnPortOfLoadingSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>Port of Discharge
                                <asp:RequiredFieldValidator ID="RFVPortDischarge" ValidationGroup="Required" runat="server"
                                    SetFocusOnError="true" Display="Dynamic" ControlToValidate="txtPortOfDischarge"
                                    ErrorMessage="Please Select Port of Discharge" Text="*"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPortOfDischarge" Width="80%" runat="server" TabIndex="17" CssClass="SearchTextbox"
                                    placeholder="Search" ToolTip="Enter Port Of Discharge." MaxLength="100" />
                                <div id="divwidthDischargePort">
                                </div>
                                <cc1:AutoCompleteExtender ID="DischargePortExtender" runat="server" TargetControlID="txtPortOfDischarge"
                                    CompletionListElementID="divwidthDischargePort" ServicePath="~/WebService/PortOfLoadingAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthDischargePort"
                                    ContextKey="1234" UseContextKey="True" OnClientItemSelected="OnPortOfDischargeSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>Consignment Country
                                <asp:RequiredFieldValidator ID="rfvcountryConsign" runat="server" ValidationGroup="Required"
                                    SetFocusOnError="true" ControlToValidate="txtCountry" Text="*" ErrorMessage="Please Select Consignment Country"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCountry" runat="server" CssClass="SearchTextbox" placeholder="Search"
                                    Width="80%" ToolTip="Enter Consignment Country." TabIndex="18"></asp:TextBox>
                                <div id="divwidthCountry">
                                </div>
                                <cc1:AutoCompleteExtender ID="CountryExtender" runat="server" TargetControlID="txtCountry"
                                    CompletionListElementID="divwidthCountry" ServicePath="../WebService/CountryAutoComplete.asmx"
                                    ServiceMethod="GetCountryCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCountry"
                                    ContextKey="4244" UseContextKey="True" OnClientItemSelected="OnCountrySelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>Destination Country
                                <asp:RequiredFieldValidator ID="rfvDestinationCountry" runat="server" ValidationGroup="Required"
                                    SetFocusOnError="true" ControlToValidate="txtDestinationCountry" Text="*" ErrorMessage="Please Select Destination Country"> </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDestinationCountry" runat="server" CssClass="SearchTextbox" placeholder="Search"
                                    Width="80%" ToolTip="Enter Final Destination Country" TabIndex="19"></asp:TextBox>
                                <div id="divwidthDestCountry">
                                </div>
                                <cc1:AutoCompleteExtender ID="DestCountryExtender" runat="server" TargetControlID="txtDestinationCountry"
                                    CompletionListElementID="divwidthDestCountry" ServicePath="../WebService/CountryAutoComplete.asmx"
                                    ServiceMethod="GetCountryCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthDestCountry"
                                    ContextKey="4244" UseContextKey="True" OnClientItemSelected="OnDestCountrySelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                        </tr>
                        <tr id="trMBL" runat="server">
                            <td>MBL/MAWBL No
                            </td>
                            <td>
                                <asp:TextBox ID="txtMblNo" runat="server" MaxLength="50" Text='<%#Eval("MAWBNo") %>' TabIndex="20"></asp:TextBox>
                            </td>
                            <td>MBL/MAWBL Date
                                <cc1:CalendarExtender ID="calMAWB" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="imgMAWBDate"
                                    Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtMAWBDate">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="MskExtMAWBDate" TargetControlID="txtMAWBDate" Mask="99/99/9999" MessageValidatorTip="true"
                                    MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="MskValMAWBDate" ControlExtender="MskExtMAWBDate" ControlToValidate="txtMAWBDate" IsValidEmpty="true"
                                    InvalidValueMessage="MBL/MAWBL Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true"
                                    MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2014" MaximumValue="31/12/2025"
                                    runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMAWBDate" runat="server" Width="100px" placeholder="dd/mm/yyyy" TabIndex="21" Text='<%#Eval("MAWBDate","{0:dd/MM/yyyy}")%>'></asp:TextBox>
                                <asp:Image ID="imgMAWBDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                        </tr>
                        <tr id="trHBL" runat="server">
                            <td>HBL/HAWBL No
                            </td>
                            <td>
                                <asp:TextBox ID="txtHblNo" runat="server" MaxLength="50" TabIndex="22" Text='<%#Eval("HAWBNo") %>'></asp:TextBox>
                            </td>
                            <td>HBL/HAWBL Date
                                <cc1:CalendarExtender ID="calHAWB" runat="server" EnableViewState="False" FirstDayOfWeek="Sunday"
                                    Format="dd/MM/yyyy" PopupButtonID="imgHAWBDate" PopupPosition="BottomRight" TargetControlID="txtHAWBDate">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="MskExtHAWBDate" TargetControlID="txtHAWBDate" Mask="99/99/9999" MessageValidatorTip="true"
                                    MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="MskValHAWBDate" ControlExtender="MskExtHAWBDate" ControlToValidate="txtHAWBDate" IsValidEmpty="true"
                                    InvalidValueMessage="HBL/HAWBL Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true"
                                    MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2014" MaximumValue="31/12/2025"
                                    runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtHAWBDate" runat="server" Width="100px" TabIndex="23" placeholder="dd/mm/yyyy" Text='<%#Eval("HAWBDate","{0:dd/MM/yyyy}")%>'></asp:TextBox>
                                <asp:Image ID="imgHAWBDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Type of Export
                                <asp:RequiredFieldValidator ID="RFVExportType" runat="server" ControlToValidate="ddlExportType"
                                    InitialValue="0" SetFocusOnError="true" ErrorMessage="Please Select Type of Export"
                                    Display="Dynamic" Text="*" ValidationGroup="Required"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <%--<asp:DropDownList ID="ddlExportType" Width="40%" runat="server" DataSourceID="DataSourceExportType"
                                    DataTextField="sName" DataValueField="lid" TabIndex="24" ToolTip="Select Shipping Bill Type" 
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlExportType_OnSelectedIndexchanged">
                                    <asp:ListItem Selected="True" Text="-- Select Bill Type --" Value="0"></asp:ListItem>
                                </asp:DropDownList>--%>
                                <asp:DropDownList ID="ddlExportType" Width="40%" runat="server" ToolTip="Select Shipping Bill Type"
                                     AutoPostBack="true" OnSelectedIndexChanged="ddlExportType_OnSelectedIndexchanged">
                                     <asp:ListItem Selected="True" Text="-- Select Bill Type --" Value="0"></asp:ListItem>
                                     <asp:ListItem Text="Factory Stuff" Value="1"></asp:ListItem>
                                     <asp:ListItem Text="Docs Stuff" Value="2"></asp:ListItem>
                                     <asp:ListItem Text="On Wheel" Value="3"></asp:ListItem>
                                     <asp:ListItem Text="Excise Sealing" Value="4"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>Shipping Bill Type
                                <asp:RequiredFieldValidator ID="rfvShippingbillType" runat="server" ControlToValidate="ddlShippingBillType"
                                    InitialValue="0" SetFocusOnError="true" ErrorMessage="Please Select Shipping Bill Type"
                                    Display="Dynamic" Text="*" ValidationGroup="Required"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlShippingBillType" Width="53%" runat="server" DataSourceID="DataSourceShippingBillType"
                                    DataTextField="sName" DataValueField="lid" TabIndex="25" ToolTip="Select Shipping Bill Type">
                                    <asp:ListItem Selected="True" Text="-- Select Bill Type --" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Priority
                                <asp:RequiredFieldValidator ID="rfvPriority" ValidationGroup="Required" runat="server"
                                    Display="Dynamic" ControlToValidate="ddlPriority" InitialValue="0" ErrorMessage="Please Select Job Priority"
                                    Text="*"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPriority" runat="server" CssClass="DropDownBox" Width="40%"
                                    ToolTip="Select Priority" TabIndex="26">
                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Normal" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="High" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Intense" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>Buyer Name
                            </td>
                            <td>
                                <asp:TextBox ID="txtBuyerName" runat="server" TabIndex="27" Width="80%" ToolTip="Enter Buyer Name."></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Product Description
                                <asp:RequiredFieldValidator ID="rfvProductDesc" ValidationGroup="Required" runat="server"
                                    SetFocusOnError="true" Display="Dynamic" ControlToValidate="txtProductDesc" ErrorMessage="Please Enter Product Description"
                                    Text="*"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtProductDesc" runat="server" Width="80%" TabIndex="28" ToolTip="Enter Product Description."></asp:TextBox>
                            </td>
                            <td>Forward To Babaji Shivram
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlForwardedBy" runat="server" TabIndex="29" ToolTip="Select Forward To Babaji Shivram" Width="40%" AutoPostBack="true" OnSelectedIndexChanged="ddlForwardedBy_OnSelectedIndexChanged">
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Forwarder Name
                                <asp:RequiredFieldValidator ID="rfvForwardedName" ValidationGroup="Required" runat="server"
                                    SetFocusOnError="true" Display="Dynamic" ControlToValidate="txtForwardedName" ErrorMessage="Please Enter Forwarded Name."
                                    Text="*"></asp:RequiredFieldValidator>
                            </td>
                            <td id="td_NonBabajiForwarder" runat="server">
                                <asp:TextBox ID="txtForwardedName" runat="server" TabIndex="30" ToolTip="Enter Forwarded Name."
                                    Width="80%"></asp:TextBox>
                            </td>
                            <td>No Of Packages
                                <asp:RequiredFieldValidator ID="rfvNoOfPackages" runat="server" ControlToValidate="txtNoOfPackages"
                                    SetFocusOnError="true" ErrorMessage="Please Enter No Of Packages" Display="Dynamic"
                                    Text="*" ValidationGroup="Required"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNoOfPackages" runat="server" TabIndex="31" ToolTip="Enter No Of Packages."
                                    MaxLength="8" Width="36%" Text='<%#Eval("NoOfPackages") %>'></asp:TextBox>
                                <asp:CompareValidator ID="CompValPackages" runat="server" ControlToValidate="txtNoOfPackages"
                                    Operator="DataTypeCheck" SetFocusOnError="true" Type="Integer" ErrorMessage="Invalid Number Of Packages."
                                    Display="Dynamic" ValidationGroup="Required"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>Package Type
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPackageType" runat="server" DataSourceID="DataSourcePackageType"
                                    Width="50%" TabIndex="32" ToolTip="Select Package Type" DataTextField="sName"
                                    DataValueField="lId">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblddlContainer" runat="server" Text="Container Loaded"></asp:Label>
                                <asp:RequiredFieldValidator ID="rfvcontloaded" runat="server" ControlToValidate="ddlContainerLoaded"
                                    SetFocusOnError="true" ErrorMessage="Please Select Container Loaded." Display="Dynamic" InitialValue="0"
                                    Text="*" ValidationGroup="Required"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlContainerLoaded" Width="40%" runat="server" TabIndex="33"
                                    ToolTip="Select Container Loaded">
                                    <asp:ListItem Selected="True" Text="-Select-" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="FCL" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="LCL" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;&nbsp;
                                 <asp:Button ID="btnAddContainerDetails" TabIndex="34" runat="server" Text="Add Container" OnClick="btnAddContainerDetails_OnClick" CausesValidation="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>Gross Weight (in Kgs)
                            </td>
                            <td>
                                <asp:TextBox ID="txtGrossWT" runat="server" MaxLength="10" Width="36%" TabIndex="35"
                                    ToolTip="Enter Gross Weight"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revGrossWeight" runat="server" ControlToValidate="txtGrossWT"
                                    SetFocusOnError="true" ErrorMessage="Invalid Gross Weight." Display="Dynamic"
                                    ValidationGroup="Required" ValidationExpression="^[0-9]\d{0,13}(\.\d{1,3})?$"></asp:RegularExpressionValidator>
                            </td>
                            <td style="width: 25%;">Net Weight (in Kgs)
                            </td>
                            <td style="width: 25%;">
                                <asp:TextBox ID="txtNetWT" runat="server" MaxLength="10" TabIndex="36" Width="37%"
                                    ToolTip="Enter Net Weight"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revNetWT" runat="server" ControlToValidate="txtNetWT"
                                    SetFocusOnError="true" ErrorMessage="Invalid Net Weight." Display="Dynamic" ValidationGroup="Required"
                                    ValidationExpression="^[0-9]\d{0,13}(\.\d{1,3})?$"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>ADC Applicable
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblADC" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>Haze Cargo
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblHazeCargo" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>Job Remark</td>
                            <td colspan="3">
                                <asp:TextBox ID="txtJobRemark" runat="server" TextMode="MultiLine" Rows="3" TabIndex="37"></asp:TextBox>
                            </td>
                        </tr>
                        <%--<tr id="tr_ApplicableFields" runat="server">
                            <td>
                            </td>
                            <td colspan="3">
                                <asp:CheckBoxList ID="chkApplicable" runat="server" RepeatDirection="Horizontal" TabIndex="38" Visible="false">
                                    <asp:ListItem Text="Octroi Applicable" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="S Form Applicable" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="N Form Applicable" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Road Permit Applicable" Value="4"></asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                        </tr>--%>
                    </table>
                    <div id="divDataSource">
                        <asp:SqlDataSource ID="DataSourcePackageType" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                            SelectCommand="GetPackageType" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                        <asp:SqlDataSource ID="DataSourceShippingBillType" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                            SelectCommand="EX_GetEX_ShippingBillTypeMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                        <asp:SqlDataSource ID="DataSourceBranch" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                            SelectCommand="EX_GetAllBranchMS" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="DataSourceExportType" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                            SelectCommand="EX_GetExportTypeMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                        <asp:SqlDataSource ID="DataSourceShipper" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                            SelectCommand="EX_GetCustWsShipperDetail" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hdnCustId" Name="CustId" PropertyName="Value" DbType="Int32" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </fieldset>

                <fieldset>
                    <legend>Upload Document</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="55%" bgcolor="white">
                        <tr>
                            <td class="clientcontentb">
                                <span id="Span22">Upload Document</span>
                            </td>
                            <td colspan="3">
                                <asp:Repeater ID="rpDocument" runat="server" OnItemDataBound="rpDocument_ItemDataBound">
                                    <ItemTemplate>
                                        <div class="clear">
                                        </div>
                                        <div style="float: left;">
                                            <asp:CheckBox ID="chkDocType" Text='<%#DataBinder.Eval(Container.DataItem,"DocumentName") %>'
                                                runat="server" TabIndex="29" />
                                        </div>
                                        <div style="float: right;">
                                            <asp:FileUpload ID="fuDocument" runat="server" Enabled="false" TabIndex="14" />
                                            <asp:RequiredFieldValidator ID="RFVDocument" ControlToValidate="fuDocument" runat="server"
                                                ValidationGroup="validateform" SetFocusOnError="true" Text="Required" ErrorMessage='<%#DataBinder.Eval(Container.DataItem,"DocumentName") + "- Document Required"%>'
                                                Enabled="false"></asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdnDocId" Value='<%#DataBinder.Eval(Container.DataItem,"lId") %>'
                                                runat="server" Visible="false"></asp:HiddenField>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </td>
                        </tr>
                    </table>
                </fieldset>

            </div>


        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div id="divAddContainerDetails">
                <asp:LinkButton ID="modelPopup1" runat="server" />
                <cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="modelPopup1" BackgroundCssClass="modalBackground"
                    PopupControlID="Panel2" DropShadow="true">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="Panel2" runat="server" CssClass="modalPopup1" BackColor="#F5F5DC" Width="100%" Height="400px">
                    <div id="div2" runat="server" style="padding: 5px">
                        <table width="100%">
                            <tr>
                                <td align="center">
                                    <u><b>Add Container Details</b></u>
                                    &nbsp;&nbsp;&nbsp;                                        
                                    <span style="float: right">
                                        <asp:ImageButton ID="imgbtnContainer" ImageUrl="~/Images/delete.gif" runat="server" OnClick="btnCancelPopup_Click1" ToolTip="Close" />
                                    </span>
                                </td>
                            </tr>
                        </table>
                        <div style="text-align: center">
                            <asp:Label ID="lblError_Popup" runat="server"></asp:Label>
                        </div>
                        <br />
                        <asp:Panel ID="pnlAddContainer" runat="server" Width="100%" ScrollBars="Auto" BorderStyle="Solid" BorderWidth="1px">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white" style="border: 1px solid #1c698a;">
                                <tr>
                                    <td>Container No
                                        <asp:RequiredFieldValidator ID="RFVContainer" runat="server" ControlToValidate="txtContainerNo"
                                            ValidationGroup="valContainer" SetFocusOnError="True" ErrorMessage="Enter Container No"
                                            Text="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtContainerNo" runat="server" Width="60%" MaxLength="11" TabIndex="1"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="REVContainer" runat="server" ControlToValidate="txtContainerNo"
                                            ValidationGroup="valContainer" SetFocusOnError="True" ErrorMessage="Enter 11 Digit Container No."
                                            Display="Dynamic" ValidationExpression="^[a-zA-Z0-9]{11}$"></asp:RegularExpressionValidator>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>Container Type
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddContainerType" TabIndex="2" runat="server" Width="68%" AutoPostBack="True" OnSelectedIndexChanged="ddContainerType_SelectedIndexChanged">
                                            <asp:ListItem Text="FCL" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="LCL" Value="2"></asp:ListItem>
                                        </asp:DropDownList>

                                    </td>
                                    <td>Container Size
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddContainerSize" runat="server" Width="100%" TabIndex="3">
                                            <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="20" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="40" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="45" Value="3"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>

                                </tr>
                                <tr>
                                    <td>Seal No</td>
                                    <td>
                                        <asp:TextBox ID="txtSealNo" runat="server" MaxLength="80" TabIndex="4"></asp:TextBox>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                            <div style="padding-top: 5px; padding: 5px; background-color: lightgrey; border: 1px solid #1c698a;">
                                <asp:Button ID="btnAddContainer" Text="Add Container" OnClick="btnAddContainer_Click" BackColor="#1c698a"
                                    ValidationGroup="valContainer" runat="server" TabIndex="5" />
                                <asp:Button ID="btnCancelContainer" Text="Cancel" TabIndex="6" OnClick="btnCancelContainer_Click" BackColor="#1c698a"
                                    CausesValidation="false" runat="server" />
                            </div>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlShowContainer" runat="server" Height="230px" Width="100%" ScrollBars="Auto">
                            <asp:GridView ID="gvContainer" CssClass="gridview" runat="server" AutoGenerateColumns="false" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="PkId" HeaderText="Sl" ReadOnly="true" />
                                    <asp:BoundField DataField="JobId" HeaderText="Job Id" Visible="false" ReadOnly="true" />
                                    <asp:BoundField DataField="ContainerNo" HeaderText="Container No" />
                                    <asp:BoundField DataField="ContainerSize" HeaderText="Container Size" />
                                    <asp:BoundField DataField="ContainerType" HeaderText="Container Type" />
                                    <asp:BoundField DataField="SealNo" HeaderText="Seal No" />
                                    <asp:BoundField DataField="UserId" HeaderText="User Id" Visible="false" ReadOnly="true" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton Text="Delete" runat="server" OnClick="OnContainerDelete" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>


                        </asp:Panel>
                    </div>
                </asp:Panel>
                <div>
                    <asp:SqlDataSource ID="DataSourceVehicle" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="EX_GetDeliveryDetail" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:SessionParameter Name="JobId" SessionField="JobId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
