<%@ Page Title="Booking Details" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ExBookingDetail.aspx.cs"
    Inherits="FreightOperation_ExBookingDetail" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<AjaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" />--%>
    <asp:ScriptManager ID="ScriptManager1" runat="server" ></asp:ScriptManager>

    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPanelDetail" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <script type="text/javascript">

      function OnCustomerSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');

            $get('<%=hdnCustId.ClientID%>').value = results.ClientId;
        }
        $addHandler
        (
            $get('txtCustomerName'), 'keyup',

            function () {
                $get('<%=txtCustomer.ClientID%>').value = '0';
            }
        );


       <%-- function OnShipperSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');

            $get('<%=hdnShipperId.ClientID%>').value = results.ClientId;
        }
        $addHandler
        (
            $get('txtShipperName'), 'keyup',

            function () {
                $get('<%=txtShipper.ClientID%>').value = '0';
            }
        );--%>


    </script>
    <script type="text/javascript">

 function OnPortOfLoadingSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=hdnLoadingPortId.ClientID%>').value = results.PortOfLoadingId;
        }
        $addHandler
        (
            $get('ctl00_ContentPlaceHolder1_EnquiryTabs_TabPanelFreightDetail_FVFreightDetail_txtPortLoading'), 'keyup',

            function () {
                $get('<%=hdnLoadingPortId.ClientID %>').value = '0';
            }
        );

        function OnPortOfDischargedSelected(source, eventArgs) {
            var resDischarged = eval('(' + eventArgs.get_value() + ')');
            $get('<%=hdnPortOfDischargedId.ClientID%>').value = resDischarged.PortOfLoadingId;
        }
        $addHandler
        (
            $get('ctl00_ContentPlaceHolder1_EnquiryTabs_TabPanelFreightDetail_FVFreightDetail_txtPortOfDischarged'), 'keyup',

            function () {
                $get('<%=hdnPortOfDischargedId.ClientID %>').value = '0';
            }
        );

    </script>
    <asp:UpdatePanel ID="upPanelDetail" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:HiddenField ID="hdnLoadingPortId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnPortOfDischargedId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnModeId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnTypeId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnStatusId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnStateCode" runat="server" Value="0" />
                <asp:ValidationSummary ID="ValSummaryFreightDetail" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="RequiredBooking" CssClass="errorMsg" EnableViewState="false" />
            </div>
            <div class="clear"></div>
            <fieldset>
                <legend runat="server" id="lgtBooking">Booking Confirmation</legend>
                <div class="m clear">
                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" ValidationGroup="RequiredBooking" />
                    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CausesValidation="False"
                        Text="Cancel" TabIndex="20" />
                </div>
                <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
                    <tr>
                        <td>Branch
                            <asp:RequiredFieldValidator ID="RFVBranch" runat="server" ControlToValidate="ddBranch" Display="Dynamic" InitialValue="0"
                                SetFocusOnError="true" Text="*" ErrorMessage="Please Select Branch" ValidationGroup="RequiredBooking"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddBranch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddBranch_SelectedIndexChanged">
                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Mumbai - Cargo Complex" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Mumbai - Nhava Seva" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Delhi" Value="5"></asp:ListItem>
                                <asp:ListItem Text="Bangalore" Value="20"></asp:ListItem>
                                <asp:ListItem Text="Mundra/Gandhidham" Value="8"></asp:ListItem>
                                <asp:ListItem Text="Hyderabad" Value="23"></asp:ListItem>
                                <asp:ListItem Text="Jaipur" Value="15"></asp:ListItem>
                                <asp:ListItem Text="Ahmedabad" Value="13"></asp:ListItem>
                                <asp:ListItem Text="Ankleshwar" Value="16"></asp:ListItem>
                                <asp:ListItem Text="Chennai" Value="6"></asp:ListItem>
                                <asp:ListItem Text="Vizag" Value="14"></asp:ListItem>
                                <asp:ListItem Text="Kolkata" Value="7"></asp:ListItem>
                                <asp:ListItem Text="Punjab" Value="28"></asp:ListItem>
                                <asp:ListItem Text="HAZIRA" Value="26"></asp:ListItem>
                                <asp:ListItem Text="USA" Value="35"></asp:ListItem>
                                <asp:ListItem Text="KAKINADA" Value="12"></asp:ListItem>

                            </asp:DropDownList>
                        </td>
                        <td>Job No
                            <asp:RequiredFieldValidator ID="RFVJobNo" runat="server" ControlToValidate="txtJobNo" Display="Dynamic" InitialValue=""
                                SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Job No" ValidationGroup="RequiredBooking"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtJobNo" runat="server" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Freight Mode
                        </td>
                        <td>
                            <asp:DropDownList ID="ddFreightMode" runat="server" AutoPostBack="true" Width="100px"
                                OnSelectedIndexChanged="ddFreightMode_SelectedIndexChanged">
                                <asp:ListItem Text="Air" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Sea" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Breakbulk" Value="3"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>Awarded Month
                        </td>
                        <td>
                            <asp:Label ID="lblAwardeMonth" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Port of Loading
                            <asp:RequiredFieldValidator ID="RFVPortLoading" runat="server" ControlToValidate="txtPortLoading" Display="Dynamic"
                                InitialValue="" SetFocusOnError="true" Text="*" ErrorMessage="Please Select Port of Loading." ValidationGroup="RequiredBooking"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPortLoading" runat="server"></asp:TextBox>

                            <div id="divwidthLoadingPort">
                            </div>
                            <AjaxToolkit:AutoCompleteExtender ID="AutoCompletePortLoading" runat="server" TargetControlID="txtPortLoading"
                                CompletionListElementID="divwidthLoadingPort" ServicePath="../WebService/PortOfLoadingAutoComplete.asmx"
                                ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthLoadingPort"
                                ContextKey="1801" UseContextKey="True" OnClientItemSelected="OnPortOfLoadingSelected"
                                CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                            </AjaxToolkit:AutoCompleteExtender>
                        </td>
                        <td>Port of Discharged
                            <asp:RequiredFieldValidator ID="RFVDischarged" runat="server" ControlToValidate="txtPortOfDischarged" Display="Dynamic"
                                InitialValue="" SetFocusOnError="true" Text="*" ErrorMessage="Please Select Port Of Discharged" ValidationGroup="RequiredBooking"> </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPortOfDischarged" runat="server"></asp:TextBox>

                            <div id="divwidthDischargPort">
                            </div>
                            <AjaxToolkit:AutoCompleteExtender ID="AutoCompletePortOfDischarged" runat="server" TargetControlID="txtPortOfDischarged"
                                CompletionListElementID="divwidthDischargPort" ServicePath="../WebService/PortOfLoadingAutoComplete.asmx"
                                ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthDischargPort"
                                ContextKey="1802" UseContextKey="True" OnClientItemSelected="OnPortOfDischargedSelected"
                                CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                DelimiterCharacters="" Enabled="True">
                            </AjaxToolkit:AutoCompleteExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>Enquiry No
                        </td>
                        <td>
                            <asp:Label ID="lblEnqNo" runat="server"></asp:Label>
                        </td>
                        <td>Enquiry Date
                        </td>
                        <td>
                            <asp:Label ID="lblEnqDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Freight SPC
                        </td>
                        <td>
                            <asp:Label ID="lblFreightSPC" runat="server"></asp:Label>
                        </td>
                        <td>Sales Rep
                        </td>
                        <td>
                            <asp:Label ID="lblSalesRep" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Country</td>
                        <td>
                            <asp:Label ID="lblCountryName" runat="server"></asp:Label>
                        </td>
                        <td>Is Dangerous Goods ?</td>
                        <td>
                            <asp:Label ID="lblDangerousGood" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Cust Ref No
                        </td>
                        <td colspan="3">
                            <asp:Label ID="lblCustRefNo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Customer
                            <asp:RequiredFieldValidator ID="RFVCustName" runat="server" ValidationGroup="validateEnquiry" SetFocusOnError="true"
                                ControlToValidate="txtCustomer" Text="*" ErrorMessage="Please Enter Customer Name" InitialValue=""> </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCustomer" Width="80%" runat="server" MaxLength="100" OnTextChanged="txtCustomer_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <asp:HiddenField ID="hdnCustId" runat="server" />
                            <div id="divwidthCust">
                            </div>
                            <%--<AjaxToolkit:AutoCompleteExtender ID="CustomerExtender" runat="server" TargetControlID="txtCustomer"
                                CompletionListElementID="divwidthCust" ServicePath="../WebService/FreightCustomerAutoComplete.asmx"
                                ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCust"
                                ContextKey="4317" UseContextKey="True" CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                CompletionListHighlightedItemCssClass="AutoExtenderHighlight" FirstRowSelected="true">
                            </AjaxToolkit:AutoCompleteExtender>--%>
                            <AjaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender10" runat="server" TargetControlID="txtCustomer"
                                CompletionListElementID="divwidthCust" ServicePath="~/WebService/CustomerAutoComplete.asmx"
                                ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCust"
                                ContextKey="4329" UseContextKey="True" OnClientItemSelected="OnCustomerSelected"
                                CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                CompletionListHighlightedItemCssClass="AutoExtenderHighlight" FirstRowSelected="true">
                            </AjaxToolkit:AutoCompleteExtender>
                        </td>
                        <td>Terms
                        </td>
                        <td>
                            <asp:DropDownList ID="ddTerms" runat="server" DataSourceID="dataSourceTermsMS"
                                DataValueField="lid" DataTextField="sName" Width="50%">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="dataSourceTermsMS" runat="server" ConnectionString="<%$ ConnectionStrings:ConBsImport %>"
                                SelectCommand="FR_GetTermsMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
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
                                <asp:DropDownList ID="ddPlant" runat="server" TabIndex="6" OnSelectedIndexChanged="ddPlant_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="0" Text="-Select-"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>

                      <tr>
                        <td>
                            <%--<asp:Label ID="lblShipCons" runat="server"></asp:Label>--%>
                                Shipper
                        </td>
                        <td>
                            <%--<asp:TextBox ID="txtShipper" runat="server" AutoPostBack="true" OnTextChanged="txtShipper_TextChanged" Width="80%"></asp:TextBox>--%>
                             <asp:HiddenField ID="hdnShipperId" runat="server" />
                           <asp:DropDownList ID="ddlShipper" TabIndex="4" runat="server" Width="82%" DataTextField="ShipperName" DataValueField="ShipperId" OnSelectedIndexChanged="ddlShipper_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>

                             <asp:RequiredFieldValidator ID="rfvShipperName" ValidationGroup="Required" runat="server"
                                    SetFocusOnError="true" Display="Dynamic" ControlToValidate="ddlShipper" ErrorMessage="Please Select Shipper Name"
                                    Text="*" InitialValue="0"></asp:RequiredFieldValidator>

                           <%-- <div id="divwidthShipper">
                            </div>                     
                              <AjaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtShipper"
                                CompletionListElementID="divwidthShipper" ServicePath="~/WebService/CustomerAutoComplete.asmx"
                                ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthShipper"
                                ContextKey="4329" UseContextKey="True" OnClientItemSelected="OnShipperSelected"
                                CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                CompletionListHighlightedItemCssClass="AutoExtenderHighlight" FirstRowSelected="true">
                            </AjaxToolkit:AutoCompleteExtender>--%>

                     <%--    <asp:SqlDataSource ID="DataSourceShipper" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                            SelectCommand="EX_GetCustWsShipperDetail" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hdnCustId" Name="CustId" PropertyName="Value" DbType="Int32" />
                            </SelectParameters>
                        </asp:SqlDataSource>--%>

                        </td>
                        <td>Place Of Supply
                            <asp:RequiredFieldValidator ID="RFVConsigneeState" runat="server" ControlToValidate="ddConsigneeState" Display="Dynamic"
                                InitialValue="0" SetFocusOnError="true" Text="*" ErrorMessage="Please Select Place Of Delivery" ValidationGroup="RequiredBooking"> </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddConsigneeState" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddConsigneeState_OnSelectedIndexChanged"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%--<asp:Label ID="lblShipConsAddr" runat="server"></asp:Label>--%>
                                Shipper Address
                            <asp:RequiredFieldValidator ID="RFVSHipperAddress" runat="server" ControlToValidate="txtShipperAddress" Display="Dynamic"
                                InitialValue="" SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Shipper Address" ValidationGroup="RequiredBooking"> </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtShipperAddress" runat="server" TextMode="MultiLine" MaxLength="400" Enabled="false"></asp:TextBox>
                        </td>

                        <td>GSTIN
                            <asp:RequiredFieldValidator ID="RFVGSTN" runat="server" ControlToValidate="txtGSTN" Display="Dynamic"
                                InitialValue="" SetFocusOnError="true" Text="*" ErrorMessage="Please Enter The GSTN" 
                                ValidationGroup="RequiredBooking"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtGSTN" runat="server" MaxLength="15" AutoPostBack="true" OnTextChanged="txtGSTN_TextChanged"
                                Enabled="false" ForeColor="Black"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%--<asp:Label ID="lblConsShip" runat="server"></asp:Label>--%>  <%--txtConsignee--%>
                            Consignee
                        </td>
                        <td>
                            <asp:TextBox ID="txtConsignee" runat="server" AutoPostBack="true" OnTextChanged="txtConsignee_TextChanged" Width="80%"></asp:TextBox>
                            <div id="divwidthConsignee">
                            </div>
                            <AjaxToolkit:AutoCompleteExtender ID="AutoCompleteConsignee" runat="server" TargetControlID="txtConsignee"
                                CompletionListElementID="divwidthConsignee" ServicePath="../WebService/FreightConsigneeAutoComplete.asmx"
                                ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthConsignee"
                                ContextKey="1990" UseContextKey="True" CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                CompletionListHighlightedItemCssClass="AutoExtenderHighlight" FirstRowSelected="true">
                            </AjaxToolkit:AutoCompleteExtender>
                        </td>
                        <td>
                            <%--<asp:Label ID="lblConsShipAddr" runat="server"></asp:Label>--%>
                                Consignee Address
                            <asp:RequiredFieldValidator ID="RFVConsigneeAddress" runat="server" ControlToValidate="txtConsigneeAddress" Display="Dynamic"
                                InitialValue="" SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Consignee Address" ValidationGroup="RequiredBooking"> </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtConsigneeAddress" runat="server" TextMode="MultiLine" MaxLength="400"></asp:TextBox>
                        </td>
                    </tr>


                    <asp:Panel ID="pnlSea" runat="server" Visible="false">
                        <tr>
                            <td>Container 20"
                           <%-- <asp:CompareValidator ID="CompValCon20" runat="server" ControlToValidate="txtCont20"
                                Operator="DataTypeCheck" SetFocusOnError="true" Type="Integer" ErrorMessage="Invalid Count Of 20"
                                Display="Dynamic" ValidationGroup="RequiredBooking"></asp:CompareValidator>--%>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCont20" Width="20%" runat="server" type="number"></asp:TextBox>
                            </td>
                            <td>Container 40"
                          <%--  <asp:CompareValidator ID="ComVal40" runat="server" ControlToValidate="txtCont40"
                                Operator="DataTypeCheck" SetFocusOnError="true" Type="Integer" ErrorMessage="Invalid Count Of 40"
                                Display="Dynamic" ValidationGroup="RequiredBooking"></asp:CompareValidator>--%>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCont40" Width="20%" runat="server" type="number"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>LCL (CBM)
                            <asp:CompareValidator ID="CompValLCL" runat="server" ControlToValidate="txtLCLVolume"
                                Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" ErrorMessage="Invalid Volume of LCL"
                                Display="Dynamic" ValidationGroup="RequiredBooking"></asp:CompareValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLCLVolume" runat="server"></asp:TextBox>
                            </td>
                            <td>LCL/FCL
                           <%-- <asp:RequiredFieldValidator ID="RFVSea" runat="server" ControlToValidate="ddContainerType" Display="Dynamic" InitialValue="0" Enabled="false"
                                SetFocusOnError="true" Text="*" ErrorMessage="Please Select LCL/FCL" ValidationGroup="RequiredBooking"></asp:RequiredFieldValidator>--%>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddContainerType" runat="server" Width="80px">
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="FCL" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="LCL" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddSubType" runat="server" Width="100px">
                                    <asp:ListItem Text="-- Sub Type--" Value=""></asp:ListItem>
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
                    <tr>
                        <td>No of Packages
                            <asp:RequiredFieldValidator ID="RFVNoOfPackage" runat="server" ControlToValidate="txtNoOfPkgs" InitialValue="0"
                                 Text="*" ErrorMessage="Please Enter number of packages" ValidationGroup="RequiredBooking"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNoOfPkgs" runat="server" Width="25%" MaxLength="8"></asp:TextBox>
                            <asp:CompareValidator ID="CompValPackgs" runat="server" ControlToValidate="txtNoOfPkgs"
                                Operator="DataTypeCheck" SetFocusOnError="true" Type="Integer" ErrorMessage="Invalid No Of Packages"
                                Display="Dynamic" ValidationGroup="RequiredBooking"></asp:CompareValidator>
                        </td>
                        <td>Type of Packing
                            <asp:RequiredFieldValidator ID="RFVPkgsType" runat="server" ControlToValidate="ddPackageType" Display="Dynamic"
                                InitialValue="0" SetFocusOnError="true" Text="*" ErrorMessage="Please Select Type of Packing." ValidationGroup="RequiredBooking"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddPackageType" runat="server"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Gross Weight (Kgs)
                            <asp:RequiredFieldValidator ID="RFVGrossWeight" runat="server" ControlToValidate="txtGrossWeight" InitialValue="0.000"
                                Text="*" ErrorMessage="Please Enter gross weight." ValidationGroup="RequiredBooking"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RFVGrossWeight1" runat="server" ControlToValidate="txtGrossWeight" InitialValue="0"
                                Text="*" ErrorMessage="Please Enter gross weight." ValidationGroup="RequiredBooking"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtGrossWeight" runat="server" Width="25%"></asp:TextBox>
                            <asp:CompareValidator ID="ComValGrossWT" runat="server" ControlToValidate="txtGrossWeight"
                                Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" ErrorMessage="Invalid Gross Weight"
                                Display="Dynamic" ValidationGroup="RequiredBooking"></asp:CompareValidator>
                        </td>
                        <td>Chargeable Weight (Kgs)
                        </td>
                        <td>
                            <asp:TextBox ID="txtChargWeight" runat="server" Width="25%"></asp:TextBox>
                            <asp:CompareValidator ID="CompValChargeWeight" runat="server" ControlToValidate="txtChargWeight"
                                Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" ErrorMessage="Invalid Chargeable Weight"
                                Display="Dynamic" ValidationGroup="RequiredBooking"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Supplier Invoice No
                        </td>
                        <td>
                            <asp:TextBox ID="txtInvoiceNo" runat="server"></asp:TextBox>
                        </td>
                        <td>Invoice Date
                            <AjaxToolkit:CalendarExtender ID="calInvoiceDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgInvDate" PopupPosition="BottomRight"
                                TargetControlID="txtInvoiceDate">
                            </AjaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtInvoiceDate" runat="server" Width="100px"></asp:TextBox>
                            <asp:Image ID="imgInvDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                           <%-- <AjaxToolkit:MaskedEditExtender ID="MskExtInvoiceDate" TargetControlID="txtInvoiceDate" Mask="99/99/9999" MessageValidatorTip="true"
                                MaskType="Date" AutoComplete="false" runat="server">
                            </AjaxToolkit:MaskedEditExtender>
                            <AjaxToolkit:MaskedEditValidator ID="MskValInvoiceDate" ControlExtender="MskExtInvoiceDate" ControlToValidate="txtInvoiceDate" IsValidEmpty="true"
                                InvalidValueMessage="Invoice Date is invalid" InvalidValueBlurredMessage="Invalid Invoice Date" SetFocusOnError="true" Display="Dynamic"
                                EmptyValueMessage="Required" MinimumValueMessage="Invalid Invoice Date" MaximumValueMessage="Invalid Invoice Date" MinimumValue="01/01/2015" MaximumValue="01/01/2024"
                                runat="Server" ValidationGroup="RequiredBooking"></AjaxToolkit:MaskedEditValidator>--%>
                        </td>
                    </tr>
                    <tr id="trAirMode" runat="server">
                        <td> Select Options</td>
                        <td>
                             <asp:RadioButtonList ID="rblAirMode" runat="server" RepeatDirection="Horizontal"  OnSelectedIndexChanged="rblAirMode_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Text="Master" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="House" Value="2" Selected="True"></asp:ListItem>
                                        </asp:RadioButtonList>
                        </td>
                        <td></td>
                        <td></td>

                    </tr>
                    <tr>
                        <td>PO No
                        </td>
                        <td>
                            <asp:TextBox ID="txtPONumber" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </td>
                        <td> 
                            <asp:Label ID="lblAgent" runat="server" Text="Agent"></asp:Label>
                            <asp:Label ID="lblAirLine" runat="server" Text="AirLine" Visible="false"></asp:Label>
                           <%-- <asp:RequiredFieldValidator ID="RFVAgent" runat="server" ControlToValidate="ddAgent" Display="Dynamic"
                                InitialValue="0" SetFocusOnError="true" Text="*" ErrorMessage="Please Select Agent." ValidationGroup="RequiredBooking"></asp:RequiredFieldValidator>--%>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddAgent" runat="server">
                            </asp:DropDownList>
                            <asp:TextBox ID="txtOverseasAgent" runat="server" Visible="false"></asp:TextBox>


                            <asp:DropDownList ID="ddlAirLine" runat="server" Visible="false">
                            </asp:DropDownList>
                            <asp:TextBox ID="txtAirLine" runat="server" Visible="false"></asp:TextBox>

                        </td>
                    </tr>
                  
                    <tr>
                        <td>Booking Info Received Date
                            <AjaxToolkit:CalendarExtender ID="CalBookingDt" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgBookDate" PopupPosition="BottomRight"
                                TargetControlID="txtBookingDate">
                            </AjaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBookingDate" runat="server" Width="100px"></asp:TextBox>
                            <asp:Image ID="imgBookDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                           <%-- <AjaxToolkit:MaskedEditExtender ID="MskExtBookingDate" TargetControlID="txtBookingDate" Mask="99/99/9999" MessageValidatorTip="true"
                                MaskType="Date" AutoComplete="false" runat="server">
                            </AjaxToolkit:MaskedEditExtender>
                            <AjaxToolkit:MaskedEditValidator ID="MskValBookingDate" ControlExtender="MskExtBookingDate" ControlToValidate="txtBookingDate" IsValidEmpty="false"
                                InvalidValueMessage="Booking Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                EmptyValueMessage="Please Enter Booking Info Date" EmptyValueBlurredText="Required" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2024"
                                runat="Server" ValidationGroup="RequiredBooking"></AjaxToolkit:MaskedEditValidator>--%>
                        </td>
                        <td>Cargo Description
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" MaxLength="400"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Booking Details</td>
                        <td colspan="3">
                            <asp:TextBox ID="txtBookingDetails" runat="server" MaxLength="800" TextMode="MultiLine" Width="90%" Height="50px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trExport2" runat="server" visible="false">
                        <td>Type Of Export</td>
                        <td>
                            <asp:DropDownList ID="ddlExportType" Width="40%" runat="server" DataSourceID="DataSourceExportType"
                                DataTextField="sName" DataValueField="lid" TabIndex="24" ToolTip="Select Shipping Bill Type"
                                OnSelectedIndexChanged="ddlExportType_SelectedIndexChanged" AutoPostBack="true">
                                
                                <asp:ListItem Selected="True" Text="-- Select Export Type --" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="DataSourceExportType" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                                SelectCommand="EX_GetExportTypeMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                        </td>
                        <td>
                            <table style="border:none; width:80%;">
                                <tr>
                                    <td style="width:30% ; border:none;">
                                         <asp:Label ID="lblCHABY" runat="server">CHA BY :</asp:Label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="rdlbtnCHABy" 
                                Text="*" ErrorMessage="Please select CHA by." ValidationGroup="RequiredBooking"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="width:50%; border:none;">
                                         <asp:RadioButtonList ID="rdlbtnCHABy" runat="server" RepeatDirection="Horizontal"  OnSelectedIndexChanged="rdlbtnCHABy_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Text="Babaji" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Other" Value="2"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>                           
                           
                        </td>
                        <td> 
                            <asp:TextBox ID="txtCHABy" runat="server" Width="80%" Visible="false" Placeholder="CHA Name"></asp:TextBox>
                        </td>
                    </tr>
                   
                    <tr id="trExport" runat="server" visible="false">
                        <td>Carting Point</td>
                        <td>
                            <asp:TextBox ID="txtCartingPoint" runat="server"></asp:TextBox>
                        </td>
                       
                    </tr>
                    <tr id="trExport1" runat="server" visible="false">
                        <td>Transportation By</td>
                        <td>
                            <asp:RadioButtonList ID="rdlbtnTransport" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rdlbtnTransport_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Text="Babaji" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Customer" Value="2"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td>
                             <asp:TextBox ID="txtTransportBy" runat="server" Width="80%" Visible="false" Placeholder="Customer Name"></asp:TextBox>
                        </td>
                        <td>
                           
                        </td>
                    </tr>
                </table>
            </fieldset>

            <fieldset>
                <legend>Upload Document</legend>
                <table border="0" cellpadding="0" cellspacing="0" width="99%" bgcolor="white">
                    <tr>
                        <td width="110px" align="center">Document Name
                                            <asp:RequiredFieldValidator ID="RFVDocName" runat="server" ControlToValidate="ddl_DocumentType"
                                                Display="Dynamic" ValidationGroup="validateDocument" SetFocusOnError="true" Text="*"
                                                ErrorMessage="Enter Document Name."></asp:RequiredFieldValidator>
                        </td>
                        <td width="50px" align="center">
                            <%--<asp:TextBox ID="txtDocName" runat="server"></asp:TextBox>--%>
                            <asp:DropDownList ID="ddl_DocumentType" runat="server" DataSourceID="FrDocTypeSqlDataSource"
                                DataTextField="Sname" DataValueField="lid">
                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:HiddenField ID="hdnUploadPath" runat="server" />
                        </td>
                        <td align="center">Attachment
                                            <asp:RequiredFieldValidator ID="RFVAttach" runat="server" ControlToValidate="fuDocument"
                                                Display="Dynamic" ValidationGroup="validateDocument" SetFocusOnError="true" Text="*"
                                                ErrorMessage="Attach File For Upload."></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:FileUpload ID="fuDocument" runat="server" /><asp:Button ID="btnUpload" runat="server"
                                Text="Upload" OnClick="btnFileUpload_Click" ValidationGroup="validateDocument" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset>
                <legend>Download</legend>
                <asp:GridView ID="gvFreightDocument" runat="server" AutoGenerateColumns="False" Width="99%"
                    DataKeyNames="DocId" DataSourceID="FreightDocumentSqlDataSource" CssClass="table"
                    OnRowCommand="gvFreightDocument_RowCommand" CellPadding="4" PagerStyle-CssClass="pgr"
                    AllowPaging="true" PageSize="20" AllowSorting="True" PagerSettings-Position="TopAndBottom">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="DocName" HeaderText="Document Name" SortExpression="DocName" />
                        <asp:BoundField DataField="UserName" HeaderText="Uploaded By" />
                        <asp:BoundField DataField="UploadedDate" HeaderText="Uploaded Date" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:TemplateField HeaderText="Download">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                    CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remove">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnlRemoveDocument" runat="server" Text="Remove" CommandName="RemoveDocument"
                                    CommandArgument='<%#Eval("DocId") %>' OnClientClick="return confirm('Are you sure to remove document?');"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </fieldset>
            <asp:SqlDataSource ID="FreightDocumentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="FR_GetUploadedDocument" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                </SelectParameters>
            </asp:SqlDataSource>
            <div>
                <asp:SqlDataSource ID="FrDocTypeSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="Get_FRDocumentType" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                        <asp:QueryStringParameter Name="JobType" DbType="String" DefaultValue='Export' />
                        <%--<asp:SessionParameter Name="JobMode" SessionField="JobMode" /> --%>           
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

