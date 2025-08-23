<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FreightEnquiry.aspx.cs" Inherits="Freight_FreightEnquiry" 
    MasterPageFile="~/MasterPage.master" Title="Freight Enquiry" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <%--<script src="../JS/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../JS/jquery-ui-1.8.7.custom.min.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        function ConfirmTransfer() {

            var ddTransfer = document.getElementById('<%=ddTransferEmp.ClientID %>');
            var EmpId = ddTransfer.options[ddTransfer.selectedIndex].value;
            
            if (EmpId > 0) {
                            
                var EmpName = ddTransfer.options[ddTransfer.selectedIndex].text;
                var strMessage = "This enquiry will be assigned to "+ EmpName +". Do you want to Save Enquiry ?"; 
                confirm(strMessage);
            }
        }

        function OnPortOfLoadingSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=hdnLoadingPortId.ClientID%>').value = results.PortOfLoadingId;
            $get('<%=txtPortLoading.ClientID%>').focus();
        }
        

        function OnPortOfDischargedSelected(source, eventArgs) {
            var resDischarged = eval('(' + eventArgs.get_value() + ')');
            $get('<%=hdnPortOfDischargedId.ClientID%>').value = resDischarged.PortOfLoadingId;
            $get('<%=txtPortOfDischarged.ClientID%>').focus();
        }
        

        function OnCountrySelected(source, eventArgs) {
                
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=hdnCountryId.ClientID%>').value = results.CountryId;
        }
        

        function OnSalesRepSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=hdnSalesRepId.ClientID %>').value = results.Userid;
        }


    </script>

    <%--<script src="../JS/jquery-3.1.0.min.js" type="text/javascript"></script>--%>
    <%--<script src="../JS/toastr/toastr.min.js" type="text/javascript"></script>--%>
    <%--<link href="../JS/toastr/toastr.min.css" rel="stylesheet" type="text/css" />--%>
     
    <%--<script type="text/javascript">

        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": false,
            "progressBar": false,
            "positionClass": "toast-bottom-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "0",
            "hideDuration": "0",
            "timeOut": "0",
            "extendedTimeOut": "0",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };

       toastr.info("Agent Information Updation Pending! Please Provide Details", "Information");
    </script>--%>
    <div>
    <%--<div id="toast"></div>--%>
        <asp:UpdateProgress ID="updProgress1" AssociatedUpdatePanelID="updFreight" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="updFreight" runat="server" UpdateMode="conditional">
        <Triggers>
                <asp:PostBackTrigger ControlID="btnSave" />
            </Triggers>
            <ContentTemplate>
        <fieldset><legend>Enquiry Details</legend>
        
            
                <div>
                    <asp:ValidationSummary ID="ValSummary" runat="server" ShowMessageBox="True" ShowSummary="False" 
                        ValidationGroup="validateEnquiry" CssClass="errorMsg" EnableViewState="false" />
                </div>
                <div>
                    <cc1:CalendarExtender ID="CalRemindDate" runat="server" Enabled="True" EnableViewState="False"
                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgRmdate" PopupPosition="BottomRight"
                        TargetControlID="txtRemindDate">
                    </cc1:CalendarExtender>
                </div>
                <div align="center">
                    <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                </div>
                <div class="m">
                    <asp:Button ID="btnSave" Text="Save" ValidationGroup="validateEnquiry" runat="server" 
                        OnClientClick="javascript:ConfirmTransfer();" OnClick="btnSave_Click" TabIndex="26"/>
                    <asp:Button ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" runat="server"
                        CausesValidation="false" TabIndex="27"/>
                </div>
                <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
                    <tr>
                        <td>
                            Enquiry No
                        </td>
                        <td>
                            <asp:Label ID="lblEnquiryRefNo" runat="server" ></asp:Label>
                        </td>
                        <td>
                            Enquiry Date
                        </td>
                        <td>
                           <span> <%=DateTime.Now.ToString("dd/MM/yyyy") %> </span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           Freight Type
                        </td>
                        <td>
                            <asp:DropDownList ID="ddFreightType" runat="server" TabIndex="1">
                                <asp:ListItem Text="Import" Value="1" ></asp:ListItem>
                                <asp:ListItem Text="Export" Value="2" ></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                           Freight Mode
                        </td>
                        <td>
                            <asp:DropDownList ID="ddFreightMode" runat="server" AutoPostBack="true" Width="40%" 
                                OnSelectedIndexChanged="ddFreightMode_SelectedIndexChanged" TabIndex="2">
                                <asp:ListItem Text="Air" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Sea" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Breakbulk" Value="3" ></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Customer
                            <asp:RequiredFieldValidator ID="RFVCustName" runat="server" ValidationGroup="validateEnquiry" SetFocusOnError="true"
                                ControlToValidate="txtCustomer" Text="*" ErrorMessage="Please Enter Customer Name" InitialValue=""> </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCustomer" CssClass="SearchTextbox" Width="80%" runat="server" placeholder="Search" 
                                TabIndex="3" MaxLength="100"></asp:TextBox>
                            <div id="divwidthCust">
                            </div>
                            <cc1:AutoCompleteExtender ID="CustomerExtender" runat="server" TargetControlID="txtCustomer"
                                CompletionListElementID="divwidthCust" ServicePath="../WebService/FreightCustomerAutoComplete.asmx"
                                ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCust" 
                                ContextKey="4317" UseContextKey="True" CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                CompletionListHighlightedItemCssClass="AutoExtenderHighlight" FirstRowSelected="true">
                            </cc1:AutoCompleteExtender>
                        </td>
                        <td>
                            Assign To Employee
                            <a href="#" data-tooltip="Pick the employee name to transfer the Enquiry. You can leave this field blank.">
                                <img src="../Images/info-icon.png" width="14px" height="14px" alt="info" /></a>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddTransferEmp" runat="server" DataSourceID="DatasourceTranasfer" DataTextField="sName"
                             DataValueField="lId" AppendDataBoundItems="true">
                                <asp:ListItem Text="-- Select --" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Ref No/Email Ref
                            <asp:RequiredFieldValidator ID="RFVRefNo" runat="server" ValidationGroup="validateEnquiry" SetFocusOnError="true"
                                ControlToValidate="txtCustRefNo" Text="*" ErrorMessage="Please Enter Customer Ref No or Email Ref No" InitialValue=""> </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCustRefNo" runat="server" Width="80%" TabIndex="5" MaxLength="400"></asp:TextBox>
                        </td>
                        <td>
                            Country
                            <asp:RequiredFieldValidator ID="RFVCountry" runat="server" ValidationGroup="validateEnquiry" SetFocusOnError="true"
                                ControlToValidate="txtCountry" Text="*" ErrorMessage="Please Select Country" InitialValue=""> </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCountry" runat="server" CssClass="SearchTextbox" placeholder="Search" TabIndex="6" 
                                AutoPostBack="true" OnTextChanged="txtCountry_TextChanged"></asp:TextBox>
                            <asp:HiddenField ID="hdnCountryId" runat="server" Value="0" />
                            <div id="divwidthCountry"></div>
                            <cc1:AutoCompleteExtender ID="CountryExtender" runat="server" TargetControlID="txtCountry"
                                CompletionListElementID="divwidthCountry" ServicePath="../WebService/CountryAutoComplete.asmx"
                                ServiceMethod="GetCountryCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCountry"
                                ContextKey="4244" UseContextKey="True" OnClientItemSelected="OnCountrySelected"
                                CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                            </cc1:AutoCompleteExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Shipper
                        </td>
                        <td>
                            <asp:TextBox ID="txtShipper" runat="server" TabIndex="7"></asp:TextBox>
                            <div id="divwidthShipper">
                            </div>
                            <cc1:AutoCompleteExtender ID="AutoCompleteShipper" runat="server" TargetControlID="txtShipper"
                                CompletionListElementID="divwidthShipper" ServicePath="~/WebService/FreightShipperAutoComplete.asmx"
                                ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthShipper" 
                                ContextKey="5556" UseContextKey="True" CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                CompletionListHighlightedItemCssClass="AutoExtenderHighlight" FirstRowSelected="true">
                            </cc1:AutoCompleteExtender>
                        </td>
                        <td>
                            Consignee
                        </td>
                        <td>
                            <asp:TextBox ID="txtConsignee" runat="server" TabIndex="8"></asp:TextBox>
                            <div id="divwidthConsignee">
                            </div>
                            <cc1:AutoCompleteExtender ID="AutoCompleteConsignee" runat="server" TargetControlID="txtConsignee"
                                CompletionListElementID="divwidthConsignee" ServicePath="../WebService/FreightConsigneeAutoComplete.asmx"
                                ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthConsignee" 
                                ContextKey="4317" UseContextKey="True" CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                CompletionListHighlightedItemCssClass="AutoExtenderHighlight" FirstRowSelected="true">
                            </cc1:AutoCompleteExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Port of Loading
                        </td>
                        <td>
                            <asp:TextBox ID="txtPortLoading" runat="server" CssClass="SearchTextbox" placeholder="Search" TabIndex="9"></asp:TextBox>
                            <asp:HiddenField ID="hdnLoadingPortId" runat="server" Value="0" />
                            <div id="divwidthLoadingPort"></div>
                            <cc1:AutoCompleteExtender ID="AutoCompletePortLoading" runat="server" TargetControlID="txtPortLoading"
                                CompletionListElementID="divwidthLoadingPort" ServicePath="../WebService/PortOfLoadingAutoComplete.asmx"
                                ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthLoadingPort"
                                ContextKey="1267" UseContextKey="True" OnClientItemSelected="OnPortOfLoadingSelected"
                                CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                            </cc1:AutoCompleteExtender>
                        </td>
                        <td>
                            Port of Discharged
                            <asp:RequiredFieldValidator ID="RFVDischarged" runat="server" ValidationGroup="validateEnquiry" SetFocusOnError="true"
                                InitialValue="" ControlToValidate="txtPortOfDischarged" Text="*" ErrorMessage="Please Select Port of Discharged"> </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPortOfDischarged" runat="server" CssClass="SearchTextbox" placeholder="Search" TabIndex="10"></asp:TextBox>
                            <asp:HiddenField ID="hdnPortOfDischargedId" runat="server" Value="0" />
                            <div id="divwidthDischargPort"></div>
                            <cc1:AutoCompleteExtender ID="AutoCompletePortOfDischarged" runat="server" TargetControlID="txtPortOfDischarged"
                            CompletionListElementID="divwidthDischargPort" ServicePath="../WebService/PortOfLoadingAutoComplete.asmx"
                            ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthDischargPort"
                            ContextKey="7268" UseContextKey="True" OnClientItemSelected="OnPortOfDischargedSelected"
                            CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                            CompletionListHighlightedItemCssClass="AutoExtenderHighlight" DelimiterCharacters="" Enabled="True">
                            </cc1:AutoCompleteExtender>
                            <asp:LinkButton ID="lnkViewRate" runat="server" Text="View Rate" OnClick="lnkViewRate_Click" CausesValidation="false"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Terms
                        </td>
                        <td>
                            <asp:DropDownList ID="ddTerms" runat="server" TabIndex="11" DataSourceID="dataSourceTermsMS"
                                DataValueField="lid" DataTextField="sName" Width="50%">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="dataSourceTermsMS" runat="server" ConnectionString="<%$ ConnectionStrings:ConBsImport %>"
                                SelectCommand="FR_GetTermsMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                        </td>
                        <td>
                            Enquiry Value
                        </td>
                        <td>
                            <asp:TextBox ID="txtEnquiryValue" runat="server" TabIndex="14" Width="25%"></asp:TextBox>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtEnquiryValue"
                                Operator="DataTypeCheck" SetFocusOnError="true" Type="Integer" ErrorMessage="Invalid Enquiry Value"
                                Display="Dynamic" ValidationGroup="validateEnquiry"></asp:CompareValidator>
                        </td>
                    </tr>
                    <asp:Panel ID="pnlSea" runat="server" Visible="false">
                    <tr>
                        <td>
                            Container 20"
                        </td>
                        <td>
                            <asp:TextBox ID="txtCont20" runat="server" TabIndex="13" Width="25%" type="Number" ></asp:TextBox>
                            <asp:CompareValidator ID="CompValCon20" runat="server" ControlToValidate="txtCont20"
                                Operator="DataTypeCheck" SetFocusOnError="true" Type="Integer" ErrorMessage="Invalid Count of 20"
                                Display="Dynamic" ValidationGroup="validateEnquiry"></asp:CompareValidator>
                        </td>
                        <td>
                            Container 40"
                        </td>
                        <td>
                            <asp:TextBox ID="txtCont40" runat="server" TabIndex="13" Width="25%" type="Number"></asp:TextBox>
                            <asp:CompareValidator ID="CompValCon40" runat="server" ControlToValidate="txtCont40"
                                Operator="DataTypeCheck" SetFocusOnError="true" Type="Integer" ErrorMessage="Invalid Count of 40"
                                Display="Dynamic" ValidationGroup="validateEnquiry"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            LCL (CBM)
                        </td>
                        <td>
                            <asp:TextBox ID="txtLCLVolume" runat="server" TabIndex="13" Width="25%"></asp:TextBox>
                            <asp:CompareValidator ID="CompValConLCL" runat="server" ControlToValidate="txtLCLVolume"
                                Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" ErrorMessage="Invalid LCL Volume"
                                Display="Dynamic" ValidationGroup="validateEnquiry"></asp:CompareValidator>
                        </td>
                        <td>
                            FCL/LCL
                        </td>
                        <td>
                            <asp:DropDownList ID="ddFCL" runat="server" TabIndex="13" Width="100px">
                            <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                <asp:ListItem Text="FCL" Value="1"></asp:ListItem>
                                <asp:ListItem Text="LCL" Value="2"></asp:ListItem>
                            </asp:DropDownList>

                            <asp:DropDownList ID="ddSubType" runat="server" TabIndex="13" Width="120px">
                            <asp:ListItem Text="-Sub Type-" Value=""></asp:ListItem> 
                                <asp:ListItem Text="GP" Value="GP"></asp:ListItem>
                                <asp:ListItem Text="HD" Value="HD"></asp:ListItem>
                                <asp:ListItem Text="HQ" Value="HQ"></asp:ListItem>
                                <asp:ListItem Text="OT" Value="OT"></asp:ListItem>
                                <asp:ListItem Text="FR" Value="FR"></asp:ListItem>
                                <asp:ListItem Text="FB" Value="FB"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    </asp:Panel>
                    <asp:Panel ID="pnlAir" runat="server" Visible="true">
                    <tr>
                        <td>
                            No of Packages
                        </td>
                        <td>
                            <asp:TextBox ID="txtNoOfPkgs" runat="server" TabIndex="13" Width="25%"></asp:TextBox>
                            <asp:CompareValidator ID="CompValPackgs" runat="server" ControlToValidate="txtNoOfPkgs"
                                Operator="DataTypeCheck" SetFocusOnError="true" Type="Integer" ErrorMessage="Invalid No Of Packages"
                                Display="Dynamic" ValidationGroup="validateEnquiry"></asp:CompareValidator>
                        </td>
                        <td></td>
                        <td>
                            <asp:TextBox ID="txtAgent" runat="server" TabIndex="12" Visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    </asp:Panel>
                    <tr>
                        <td>
                            Gross Weight (Kgs)
                        </td>
                        <td>
                            <asp:TextBox ID="txtGrossWeight" runat="server" TabIndex="15" Width="25%"></asp:TextBox>
                            <asp:CompareValidator ID="ComValGrossWT" runat="server" ControlToValidate="txtGrossWeight"
                                Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" ErrorMessage="Invalid Gross Weight"
                                Display="Dynamic" ValidationGroup="validateEnquiry"></asp:CompareValidator>
                        </td>
                        <td>
                            Chargeable Weight (Kgs)
                        </td>
                        <td>
                            <asp:TextBox ID="txtChargWeight" runat="server" TabIndex="16" Width="25%"></asp:TextBox>
                            <asp:CompareValidator ID="CompValChargeWeight" runat="server" ControlToValidate="txtChargWeight"
                                Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" ErrorMessage="Invalid Chargeable Weight"
                                Display="Dynamic" ValidationGroup="validateEnquiry"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Is Dangerous Goods ?
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rdlGoodsType" runat="server" RepeatDirection="Horizontal" TabIndex="17">
                                <asp:ListItem Value="false" Text="No" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="true" Text="Yes"></asp:ListItem>
                            </asp:RadioButtonList> 
                        </td>
                        <td>
                            Sales Representative
                            <asp:RequiredFieldValidator ID="RFVSalesRep" runat="server" ValidationGroup="validateEnquiry" SetFocusOnError="true"
                              ControlToValidate="txtSalesRep" Text="*" ErrorMessage="Please Enter Sales Representative Name" InitialValue=""> </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSalesRep" runat="server" CssClass="SearchTextbox" placeholder="Search" TabIndex="18"></asp:TextBox>
                            <asp:HiddenField ID="hdnSalesRepId" runat="server" Value="0" />
                            <div id="divwidthSalesRep">
                                </div>
                            <cc1:AutoCompleteExtender ID="SalesRepExtender" runat="server" TargetControlID="txtSalesRep"
                                CompletionListElementID="divwidthSalesRep" ServicePath="../WebService/UserAutoComplete.asmx"
                                ServiceMethod="GetUserCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthSalesRep"
                                ContextKey="7164" UseContextKey="True" OnClientItemSelected="OnSalesRepSelected"
                                CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                            </cc1:AutoCompleteExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>Shipment Information</td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark" runat="server" TabIndex="19" TextMode="MultiLine" Width="80%" Columns="4"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <fieldset><legend>Available Agent</legend>
                            <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
                                <tr>
                                    <td>
                                        <asp:ListBox ID="lbAgentCompany" runat="server" SelectionMode="Multiple" Width="100%" Height="120px" 
                                        TabIndex="21" AutoPostBack="true" OnSelectedIndexChanged="lbAgentCompany_IndexChanged">
                                           
                                        </asp:ListBox>
					                </td>    
                                </tr>
                            </table>
                            </fieldset>
                        </td>
                        <td colspan="2">
                            <fieldset><legend>Enquiry Contact</legend>
                            <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%" height="120px">
                                <tr>
                                    <td>
                                        <asp:ListBox ID="lbAgentContact" runat="server" SelectionMode="Multiple" Width="100%" Height="120px" TabIndex="21">
                                           
                                        </asp:ListBox>
                                    </td>
                                </tr>
                            </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                    <td colspan="2">
                        <fieldset><legend>Attach Document</legend>
                        <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
                            <tr>
                                <td>
                                    <asp:FileUpload ID="fuAttachment" runat="server" TabIndex="21"/>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAttachDocName" runat="server" TabIndex="22" Placeholder="Document Name"></asp:TextBox>     
                                </td>
                            </tr>
                        </table>
                        </fieldset>
                    </td>
                    <td colspan="2">
                        <fieldset><legend>Reminder</legend>
                            <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="chkReminMode" runat="server" RepeatDirection="Horizontal" TabIndex="23">
                                            <asp:ListItem Text="Email" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="SMS" Value="2"></asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemindNotes" runat="server" TextMode="MultiLine" placeholder="Reminder Note" TabIndex="24" MaxLength="200"></asp:TextBox>
                                    </td>
                                    <td>
                                        Reminder Date
                                        <asp:CompareValidator ID="ComValRmDate" runat="server" ControlToValidate="txtRemindDate" Display="Dynamic" ValidationGroup="validateEnquiry"
                                         SetFocusOnError="true" Text="Invalid Date" ErrorMessage="Invalid Reminder Date." Type="Date" CultureInvariantValues="false" Operator="DataTypeCheck"></asp:CompareValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemindDate" Width="100px" TabIndex="25" runat="server" placeholder="dd/mm/yyyy"></asp:TextBox>
                                        <asp:Image ID="imgRmdate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server"/>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        </td>
                    </tr>
                </table>
                <div>
                    <asp:SqlDataSource ID="DatasourceTranasfer" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                        SelectCommand="FR_GetEnqTransferUser" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                </div>
                
                <!--Popup Agent Rate Detail  -->
                <div>
                    <asp:LinkButton ID="lnkDummySummary" runat="server" Text=""></asp:LinkButton>
                </div>
                <div id="divPopupRate">
                <cc1:ModalPopupExtender ID="ModalPopupRateDetail" runat="server" CacheDynamicResults="false"
                    DropShadow="False" PopupControlID="Panel2Summary" TargetControlID="lnkDummySummary">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="Panel2Summary" runat="server" CssClass="ModalPopupPanel" Style="display: none">
                <div class="header">
                <div class="fleft">
                    &nbsp;<asp:Button ID="btnCancelRatePopup" runat="server" OnClick="btnCancelRatePopup_Click" Text="Close" CausesValidation="false" />
                </div>
                <div class="fright">
                    <asp:ImageButton ID="imgCancelRatePopup" ImageUrl="~/Images/delete.gif" OnClick="btnCancelRatePopup_Click" runat="server" ToolTip="Close"  />
                </div>
                </div>
                <!--Freight Detail Start-->
                <div id="Div3" runat="server" style="max-height: 550px; max-width:900px; overflow: auto;">
                <asp:GridView ID="gvRateSummary" runat="server" CssClass="table" Width="98%" AutoGenerateColumns="true"
                    AllowSorting="true" AllowPaging="false" PageSize="20" PagerStyle-CssClass="pgr" 
                    OnSorting="gvRateSummary_Sorting" OnPageIndexChanging="gvRateSummary_PageIndexChanging">
                    <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex +1 %>
                        </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField DataField="COUNTRY" HeaderText="COUNTRY" SortExpression="COUNTRY" />
                        <asp:BoundField DataField="POL" HeaderText="POL" SortExpression="POL"/>
                        <asp:BoundField DataField="POD" HeaderText="POD" SortExpression="POD"/>
                        <asp:BoundField DataField="LINE" HeaderText="LINE" SortExpression="LINE"/>
                        <asp:BoundField DataField="TransitDays" HeaderText="Transit Days" SortExpression="TransitDays" />
                        <asp:BoundField DataField="20GP" HeaderText="20GP" SortExpression="20GP"/>
                        <asp:BoundField DataField="40GPHQ" HeaderText="40GP HQ" SortExpression="40GPHQ"/>
                        <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark"/>
                        <asp:BoundField DataField="Agent" HeaderText="Agent" SortExpression="Agent"/>
                        <asp:BoundField DataField="RateValidityDate" HeaderText="Validity Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="RateValidityDate"/>--%>
                    </Columns>
                </asp:GridView>
                </div>
                </asp:Panel>
                
            </div>
            </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    
    
</asp:Content>