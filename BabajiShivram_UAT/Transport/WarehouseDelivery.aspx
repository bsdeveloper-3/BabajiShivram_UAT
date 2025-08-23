<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WarehouseDelivery.aspx.cs" Inherits="WarehouseDelivery"
    MasterPageFile="~/MasterPage.master" Title="Delivery - Clearance" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <script type="text/javascript">
        function ActiveTabChanged12() {
            /* Clear Error Message on Tab change Event */
            document.getElementById('<%=lblError.ClientID%>').innerHTML = '';
            document.getElementById('<%=lblError.ClientID%>').className = '';
        }
    </script>
    <asp:UpdatePanel ID="upJobDetail" runat="server">
        <ContentTemplate>

            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" />
                <asp:HiddenField ID="hdnBranchId" runat="server" />
            </div>
            <cc1:TabContainer ID="TabJobDetail" runat="server" ActiveTabIndex="0" CssClass="Tab" CssTheme="None" OnClientActiveTabChanged="ActiveTabChanged12">
                <cc1:TabPanel ID="Tab1" runat="server" HeaderText="Job Detail">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Job Detail</legend>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>BS Job No.
                                    </td>
                                    <td>
                                        <asp:Label ID="lblJobRefNo" runat="server"></asp:Label>
                                    </td>
                                    <td>Cust Ref No.
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCustRefNo" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Customer Name
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCustName" runat="server"></asp:Label>
                                    </td>
                                    <td>Consignee Name
                                    </td>
                                    <td>
                                        <asp:Label ID="lblConsigneeName" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Consol Agent
                                    </td>
                                    <td>
                                        <asp:Label ID="lblFFName" runat="server"></asp:Label>
                                    </td>
                                    <td>Mode
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMode" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Short Description
                                    </td>
                                    <td>
                                        <asp:Label ID="lblShortDesc" runat="server"></asp:Label>
                                    </td>
                                    <td>Delivery Type
                                    </td>
                                    <td>
                                        <asp:Label ID="lblLoaded" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Total No Of Packages    
                                    </td>
                                    <td>
                                        <asp:Label ID="lblNoOfPackages" runat="server"></asp:Label>
                                        &nbsp;<asp:Label ID="lblPackageType" runat="server"></asp:Label>
                                    </td>
                                    <td>Gross Weight (In kg)
                                    </td>
                                    <td>
                                        <asp:Label ID="lblGrossWeight" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <asp:Panel ID="pnlForSea" runat="server" Visible="false">
                                    <tr>
                                        <td>Con20'
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCon20" runat="server"></asp:Label>
                                        </td>
                                        <td>Con40'
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCon40" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>LCL
                                        </td>
                                        <td>
                                            <asp:Label ID="lblLCL" runat="server"></asp:Label></td>
                                        <td>LCL/FCL
                                        </td>
                                        <td>
                                            <asp:Label ID="lblShipmentType" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </table>
                        </fieldset>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="Tab2" runat="server" HeaderText="Update Delivery Detail">
                    <ContentTemplate>
                        <div>
                            <cc1:CalendarExtender ID="CalReceivedDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgVehicleRecd" PopupPosition="BottomRight"
                                TargetControlID="txtVehicleRecdDate">
                            </cc1:CalendarExtender>
                            <cc1:CalendarExtender ID="CalPaymentDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgLR" PopupPosition="BottomRight"
                                TargetControlID="txtLRDate">
                            </cc1:CalendarExtender>
                            <cc1:CalendarExtender ID="CalDeliveryDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDelivery" PopupPosition="BottomRight"
                                TargetControlID="txtDeliveryDate">
                            </cc1:CalendarExtender>
                            <cc1:CalendarExtender ID="CalDisptachDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgClearance" PopupPosition="BottomRight"
                                TargetControlID="txtDispatchDate">
                            </cc1:CalendarExtender>
                            <cc1:CalendarExtender ID="CalReturnDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgReturn" PopupPosition="BottomRight"
                                TargetControlID="txtReturnDate">
                            </cc1:CalendarExtender>
                            <cc1:CalendarExtender ID="CalPermitDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgPermitDate" PopupPosition="BottomRight"
                                TargetControlID="txtRoadPermitDate">
                            </cc1:CalendarExtender>
                            <cc1:CalendarExtender ID="CalNFormDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgNFormDate" PopupPosition="BottomRight"
                                TargetControlID="txtNFormDate">
                            </cc1:CalendarExtender>
                            <cc1:CalendarExtender ID="CalNClosingDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgNClosingDate" PopupPosition="BottomRight"
                                TargetControlID="txtNClosingDate">
                            </cc1:CalendarExtender>
                            <cc1:CalendarExtender ID="CalSFormDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgSFormDate" PopupPosition="BottomRight"
                                TargetControlID="txtSFormDate">
                            </cc1:CalendarExtender>
                            <cc1:CalendarExtender ID="CalSClosingDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgSClosingDate" PopupPosition="BottomRight"
                                TargetControlID="txtSClosingDate">
                            </cc1:CalendarExtender>
                            <cc1:CalendarExtender ID="CalOctroiPaidDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgOctroiPaidDate" PopupPosition="BottomRight"
                                TargetControlID="txtOctroiPaidDate">
                            </cc1:CalendarExtender>
                            <cc1:CalendarExtender ID="CalChallahDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgChallanDate" PopupPosition="BottomRight"
                                TargetControlID="txtBabajiChallanDate">
                            </cc1:CalendarExtender>
                        </div>

                        <fieldset>
                            <legend>Delivery Detail</legend>
                            <div class="m">
                                <asp:Button ID="btnSubmit" Text="Save" runat="server" OnClick="btnSubmit_Click"
                                    ValidationGroup="Required" TabIndex="16" />
                                <asp:Button ID="btnCancel" Text="Cancel" CausesValidation="false"
                                    OnClick="btnCancel_OnClick" runat="server" TabIndex="16" />
                            </div>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <div>
                                    <tr>
                                        <td>Transportation By Babaji
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rdlTransport" runat="server" RepeatDirection="Horizontal" TabIndex="9"
                                                AutoPostBack="true" OnSelectedIndexChanged="rdlTransport_SelectedIndexChanged">
                                                <asp:ListItem Text="Yes" Value="1" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <div id="DivPackages" runat="server">
                                        <tr>
                                            <td>No of Packages
                <asp:CompareValidator ID="CompValPackages" runat="server" ControlToValidate="txtNoOfPackages" Operator="DataTypeCheck" SetFocusOnError="true"
                    Type="Integer" Text="Invalid Packages" ErrorMessage="Invalid No of Packages" Display="Dynamic" ValidationGroup="Required"></asp:CompareValidator>
                                                <asp:RequiredFieldValidator ID="RFVNoOfPkgs" runat="server" ControlToValidate="txtNoOfPackages" SetFocusOnError="true"
                                                    Text="*" ErrorMessage="Please Enter No of Packages" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNoOfPackages" MaxLength="8" AutoPostBack="true" OnTextChanged="txtNoOfPackages_TextChanged"
                                                    runat="server" Width="100px" TabIndex="1"></asp:TextBox>
                                            </td>
                                            <td>Balance Packages
                                            </td>
                                            <td>
                                                <asp:Label ID="lblBalancePackage" runat="server"></asp:Label>
                                                <asp:HiddenField ID="hdnBalancePkg" runat="server" Value="0" />
                                            </td>
                                        </tr>
                                    </div>
                                    <tr>
                                        <div id="DivContainer" runat="server">
                                            <td>Container No
                <asp:RequiredFieldValidator ID="RFVContainer" runat="server" ControlToValidate="ddContainerNo" InitialValue="0" SetFocusOnError="true"
                    Text="*" ErrorMessage="Please Select Container No." Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddContainerNo" runat="server" TabIndex="1">
                                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>Delivered Container
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDeliveredContainer" runat="server"></asp:Label>
                                            </td>
                                        </div>
                                    </tr>
                                    <tr>
                                        <td>Vehicle No & Type
                                        <asp:RequiredFieldValidator ID="RFVVehicleNo" runat="server" ControlToValidate="txtVehicleNo" SetFocusOnError="true"
                                            Text="*" ErrorMessage="Please Enter Vehicle No." Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvddlVehicleNo" runat="server" ControlToValidate="ddVehicleNo" InitialValue="0"
                                                SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Vehicle No." Display="Dynamic"
                                                ValidationGroup="Required"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtVehicleNo" runat="server" MaxLength="50" Width="100px" TabIndex="2"></asp:TextBox>&nbsp;
                                            <asp:DropDownList ID="ddVehicleNo" runat="server" Width="100px" TabIndex="3" AutoPostBack="true" OnSelectedIndexChanged="ddVehicleNo_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddVehicleType" runat="server" Width="120px" TabIndex="3"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RFVVehicleType" runat="server" ControlToValidate="ddVehicleType" SetFocusOnError="true"
                                                Text="*" InitialValue="0" ErrorMessage="Please Select Vehicle Type" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>Vehicle Received Date
                                            <cc1:MaskedEditExtender ID="MEditReceivedDate" TargetControlID="txtVehicleRecdDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                MaskType="Date" AutoComplete="false" runat="server">
                                            </cc1:MaskedEditExtender>
                                            <cc1:MaskedEditValidator ID="MEditValReceivedDate" ControlExtender="MEditReceivedDate" ControlToValidate="txtVehicleRecdDate" IsValidEmpty="true"
                                                InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Vehicle Received Date is invalid" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date"
                                                MinimumValue="01/01/2015" SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtVehicleRecdDate" runat="server" Width="100px" TabIndex="4" placeholder="dd/mm/yyyy"></asp:TextBox>
                                            <asp:Image ID="imgVehicleRecd" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Delivery Destination
                <asp:RequiredFieldValidator ID="RFVDeliveryPoiny" runat="server" ControlToValidate="txtDeliveryPoint" SetFocusOnError="true"
                    Text="*" InitialValue="" ErrorMessage="Please Enter Delivery Destination" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDeliveryPoint" runat="server" MaxLength="100" TabIndex="5"></asp:TextBox>
                                        </td>
                                        <td>Transporter
                <asp:RequiredFieldValidator ID="RFVTransName" runat="server" ControlToValidate="txtTransporterName"
                    InitialValue="" SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Transporter Name" Display="Dynamic"
                    ValidationGroup="Required"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="RFVTransID" runat="server" ControlToValidate="ddTransporter"
                                                InitialValue="0" SetFocusOnError="true" Text="*" ErrorMessage="Please Select Transporter" Display="Dynamic"
                                                ValidationGroup="Required"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTransporterName" runat="server" MaxLength="100" Visible="true" TabIndex="6"></asp:TextBox>
                                            <asp:DropDownList ID="ddTransporter" runat="server" Visible="false" TabIndex="6">
                                                <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>LR No
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLRNo" runat="server" MaxLength="50" TabIndex="7"></asp:TextBox>
                                        </td>
                                        <td>LR Date
                <cc1:MaskedEditExtender ID="MEditLRDate" TargetControlID="txtLRDate" Mask="99/99/9999" MessageValidatorTip="true"
                    MaskType="Date" AutoComplete="false" runat="server">
                </cc1:MaskedEditExtender>
                                            <cc1:MaskedEditValidator ID="MEditValLRDate" ControlExtender="MEditLRDate" ControlToValidate="txtLRDate" IsValidEmpty="true"
                                                InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="LR Date is invalid" MinimumValueMessage="Invalid LR Date" MaximumValueMessage="Invalid LR Date"
                                                MinimumValue="01/01/2015" SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLRDate" runat="server" Width="100px" TabIndex="8" placeholder="dd/mm/yyyy"></asp:TextBox>
                                            <asp:Image ID="imgLR" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Dispatch Date
                <cc1:MaskedEditExtender ID="MEditDispatchDate" TargetControlID="txtDispatchDate" Mask="99/99/9999" MessageValidatorTip="true"
                    MaskType="Date" AutoComplete="false" runat="server">
                </cc1:MaskedEditExtender>
                                            <cc1:MaskedEditValidator ID="MEditValDispatchDate" ControlExtender="MEditDispatchDate" ControlToValidate="txtDispatchDate" IsValidEmpty="false"
                                                EmptyValueBlurredText="*" EmptyValueMessage="Please Enter Dispatch Date" InvalidValueBlurredMessage="Invalid Date"
                                                InvalidValueMessage="Dispatch Date is invalid" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date"
                                                MinimumValue="01/01/2015" SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDispatchDate" runat="server" Width="100px" TabIndex="9" placeholder="dd/mm/yyyy"></asp:TextBox>
                                            <asp:Image ID="imgClearance" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                                                runat="server" />
                                        </td>
                                        <td>Cargo Delivered Date
                <cc1:MaskedEditExtender ID="MEditDeliveredDate" TargetControlID="txtDeliveryDate" Mask="99/99/9999" MessageValidatorTip="true"
                    MaskType="Date" AutoComplete="false" runat="server">
                </cc1:MaskedEditExtender>
                                            <cc1:MaskedEditValidator ID="MEditValDeliveredDate" ControlExtender="MEditDeliveredDate" ControlToValidate="txtDeliveryDate" IsValidEmpty="true"
                                                InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Cargo Delivered Date is invalid" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date"
                                                MinimumValue="01/01/2015" SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDeliveryDate" runat="server" Width="100px" TabIndex="10" placeholder="dd/mm/yyyy"></asp:TextBox>
                                            <asp:Image ID="imgDelivery" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>LR Attachment
                                        </td>
                                        <td>
                                            <asp:FileUpload ID="fuPOD" runat="server" TabIndex="11" />
                                            <asp:HiddenField ID="hdnUploadPath" runat="server" />
                                        </td>
                                        <td>Cargo Recvd Person Name
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCargoPersonName" runat="server" MaxLength="50" TabIndex="12"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <asp:Panel ID="pnlOctroiApplicable" runat="server" Visible="false">
                                        <tr>
                                            <td>Octroi Amount
                    <asp:RequiredFieldValidator ID="RFVOctroiAmt" runat="server" ControlToValidate="txtOctroiAmount" SetFocusOnError="true"
                        Text="*" ErrorMessage="Please Enter Octroi Amount." Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                                <asp:CompareValidator ID="CVOctroiAmount" runat="server" ControlToValidate="txtOctroiAmount" Display="Dynamic" SetFocusOnError="true"
                                                    Type="Double" Operator="DataTypeCheck" ErrorMessage="Invalid Octroi Amount" ValidationGroup="Required"></asp:CompareValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOctroiAmount" runat="server" MaxLength="12" Width="100px" TabIndex="12"></asp:TextBox>
                                            </td>
                                            <td>Octroi Receipt No
                    <asp:RequiredFieldValidator ID="RFVOctroiReceipt" runat="server" ControlToValidate="txtOctroiReceiptNo" SetFocusOnError="true"
                        Text="*" ErrorMessage="Please Enter Octroi Receipt No." Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOctroiReceiptNo" runat="server" MaxLength="100" TabIndex="12"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Octroi Paid Date
                    <cc1:MaskedEditExtender ID="MEditOctroiDate" TargetControlID="txtOctroiPaidDate" Mask="99/99/9999" MessageValidatorTip="true"
                        MaskType="Date" AutoComplete="false" runat="server">
                    </cc1:MaskedEditExtender>
                                                <cc1:MaskedEditValidator ID="MEditValOctroiDate" ControlExtender="MEditOctroiDate" ControlToValidate="txtOctroiPaidDate" IsValidEmpty="false"
                                                    EmptyValueBlurredText="*" EmptyValueMessage="Please Enter Octroi Paid Date" InvalidValueBlurredMessage="Invalid Date"
                                                    InvalidValueMessage="Octroi Paid Date is invalid" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date"
                                                    MinimumValue="01/01/2015" SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOctroiPaidDate" runat="server" Width="100px" TabIndex="12" placeholder="dd/mm/yyyy"></asp:TextBox>
                                                <asp:Image ID="imgOctroiPaidDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                            </td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlSFormApplicable" runat="server" Visible="false">
                                        <tr>
                                            <td>S Form No
                    <asp:RequiredFieldValidator ID="RFVSFormNo" runat="server" ControlToValidate="txtSFormNo" SetFocusOnError="true"
                        Text="*" ErrorMessage="Please Enter S Form No." Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSFormNo" runat="server" MaxLength="100" TabIndex="12"></asp:TextBox>
                                            </td>
                                            <td>S Form Date
                    <cc1:MaskedEditExtender ID="MEditSFormDate" TargetControlID="txtSFormDate" Mask="99/99/9999" MessageValidatorTip="true"
                        MaskType="Date" AutoComplete="false" runat="server">
                    </cc1:MaskedEditExtender>
                                                <cc1:MaskedEditValidator ID="MEditValSFormDate" ControlExtender="MEditSFormDate" ControlToValidate="txtSFormDate" IsValidEmpty="false"
                                                    EmptyValueBlurredText="*" EmptyValueMessage="Please Enter S Form Date" InvalidValueBlurredMessage="Invalid Date"
                                                    InvalidValueMessage="S Form Date is invalid" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date"
                                                    MinimumValue="01/01/2015" SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSFormDate" runat="server" Width="100px" TabIndex="12" placeholder="dd/mm/yyyy"></asp:TextBox>
                                                <asp:Image ID="imgSFormDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>S Form Closing Date
                    <cc1:MaskedEditExtender ID="MEditSFormCloseDate" TargetControlID="txtSClosingDate" Mask="99/99/9999" MessageValidatorTip="true"
                        MaskType="Date" AutoComplete="false" runat="server">
                    </cc1:MaskedEditExtender>
                                                <cc1:MaskedEditValidator ID="MEditValSFormCloseDate1" ControlExtender="MEditSFormCloseDate" ControlToValidate="txtSClosingDate" IsValidEmpty="true"
                                                    InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="S Form Closing Date is invalid" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date"
                                                    MinimumValue="01/01/2015" MaximumValue="01/01/2025" SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSClosingDate" runat="server" Width="100px" TabIndex="12" placeholder="dd/mm/yyyy"></asp:TextBox>
                                                <asp:Image ID="imgSClosingDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                            </td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlNFormApplicable" runat="server" Visible="false">
                                        <tr>
                                            <td>N Form No
                    <asp:RequiredFieldValidator ID="RFVNFormNo" runat="server" ControlToValidate="txtNFormNo" SetFocusOnError="true"
                        Text="*" ErrorMessage="Please Enter N Form No." Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNFormNo" runat="server" MaxLength="100" TabIndex="12"></asp:TextBox>
                                            </td>
                                            <td>N Form Date
                    <cc1:MaskedEditExtender ID="MEditNFormDate" TargetControlID="txtNFormDate" Mask="99/99/9999" MessageValidatorTip="true"
                        MaskType="Date" AutoComplete="false" runat="server">
                    </cc1:MaskedEditExtender>
                                                <cc1:MaskedEditValidator ID="MEditValNFormDate" ControlExtender="MEditNFormDate" ControlToValidate="txtNFormDate" IsValidEmpty="false"
                                                    EmptyValueBlurredText="*" EmptyValueMessage="Please Enter N Form Date" InvalidValueBlurredMessage="Invalid Date"
                                                    InvalidValueMessage="N Form Date is invalid" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date"
                                                    MinimumValue="01/01/2015" SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNFormDate" runat="server" Width="100px" TabIndex="12" placeholder="dd/mm/yyyy"></asp:TextBox>
                                                <asp:Image ID="imgNFormDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>N Form Closing Date
                     <cc1:MaskedEditExtender ID="MEditNFormCloseDate" TargetControlID="txtNClosingDate" Mask="99/99/9999" MessageValidatorTip="true"
                         MaskType="Date" AutoComplete="false" runat="server">
                     </cc1:MaskedEditExtender>
                                                <cc1:MaskedEditValidator ID="MEditValNFormCloseDate" ControlExtender="MEditNFormCloseDate" ControlToValidate="txtNClosingDate" IsValidEmpty="true"
                                                    InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="N Form Closing Date is invalid" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date"
                                                    MinimumValue="01/01/2015" MaximumValue="01/01/2025" SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNClosingDate" runat="server" Width="100px" TabIndex="12" placeholder="dd/mm/yyyy"></asp:TextBox>
                                                <asp:Image ID="imgNClosingDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                            </td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlRoadPermitApplicable" runat="server" Visible="false">
                                        <tr>
                                            <td>Road Permit No
                    <asp:RequiredFieldValidator ID="RFVRoadPermitNo" runat="server" ControlToValidate="txtRoadPermitNo" SetFocusOnError="true"
                        Text="*" ErrorMessage="Please Enter Road Permit No." Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRoadPermitNo" runat="server" MaxLength="50" TabIndex="12"></asp:TextBox>
                                            </td>
                                            <td>Road Permit Date
                    <cc1:MaskedEditExtender ID="MEditRoadPermitDate" TargetControlID="txtRoadPermitDate" Mask="99/99/9999" MessageValidatorTip="true"
                        MaskType="Date" AutoComplete="false" runat="server">
                    </cc1:MaskedEditExtender>
                                                <cc1:MaskedEditValidator ID="MEditValRoadPermitDate" ControlExtender="MEditRoadPermitDate" ControlToValidate="txtRoadPermitDate" IsValidEmpty="false"
                                                    EmptyValueBlurredText="*" EmptyValueMessage="Please Enter Road Permit Date" InvalidValueBlurredMessage="Invalid Date"
                                                    InvalidValueMessage="Road Permit Date is invalid" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date"
                                                    MinimumValue="01/01/2015" SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRoadPermitDate" runat="server" Width="100px" TabIndex="12" placeholder="dd/mm/yyyy"></asp:TextBox>
                                                <asp:Image ID="ImgPermitDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                            </td>
                                        </tr>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlForSea2" runat="server" Visible="false">
                                        <tr>
                                            <td>Delivery Type
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDeliveryType" runat="server"></asp:Label>
                                                <asp:HiddenField ID="hdnDeliveryTypeId" Value="0" runat="server" />
                                            </td>
                                            <td>Empty container return date
                    <cc1:MaskedEditExtender ID="MEditReturnDate" TargetControlID="txtReturnDate" Mask="99/99/9999" MessageValidatorTip="true"
                        MaskType="Date" AutoComplete="false" runat="server">
                    </cc1:MaskedEditExtender>
                                                <cc1:MaskedEditValidator ID="MEditValReturnDate" ControlExtender="MEditReturnDate" ControlToValidate="txtReturnDate" IsValidEmpty="true"
                                                    InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Container return date is invalid" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date"
                                                    MinimumValue="01/01/2015" MaximumValue="01/01/2025" SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtReturnDate" runat="server" Width="100px" TabIndex="12" placeholder="dd/mm/yyyy"></asp:TextBox>
                                                <asp:Image ID="imgReturn" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                            </td>
                                        </tr>
                                    </asp:Panel>
                                    <tr>
                                        <td>Babaji Challan No
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBabajiChallanNo" runat="server" MaxLength="50" TabIndex="13"></asp:TextBox>&nbsp;
                                        </td>
                                        <td>Babaji Challan Date
                    <cc1:MaskedEditExtender ID="MEditChallanDate" TargetControlID="txtBabajiChallanDate" Mask="99/99/9999" MessageValidatorTip="true"
                        MaskType="Date" AutoComplete="false" runat="server">
                    </cc1:MaskedEditExtender>
                                            <cc1:MaskedEditValidator ID="MEditValChallanDate" ControlExtender="MEditChallanDate" ControlToValidate="txtBabajiChallanDate" IsValidEmpty="true"
                                                InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Challan Date is invalid" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date"
                                                MinimumValue="01/01/2015" MaximumValue="01/01/2025" SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBabajiChallanDate" runat="server" Width="100px" TabIndex="13" placeholder="dd/mm/yyyy"></asp:TextBox>
                                            <asp:Image ID="imgChallanDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Babaji Challan Copy
                                        </td>
                                        <td>
                                            <asp:FileUpload ID="fuChallanCopy" runat="server" />
                                        </td>
                                        <td>Damage Image
                                        </td>
                                        <td>
                                            <asp:FileUpload ID="fuDamageCopy" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Driver Name
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDriverName" runat="server" MaxLength="50" TabIndex="14"></asp:TextBox>
                                        </td>
                                        <td>Driver Phone
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDriverPhone" runat="server" MaxLength="50" TabIndex="14"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <%--<tr>
            <td>
                Cargo Move To
                <asp:RequiredFieldValidator ID="RFVCargoMoveTo" runat="server" Text="*" InitialValue="0" 
                    SetFocusOnError="true" ControlToValidate="ddTransitType" ErrorMessage="Please Select Cargo Move To"
                    ValidationGroup="Required" Enabled="false"> </asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:DropDownList ID="ddTransitType" runat="server" AutoPostBack="true" 
                    OnSelectedIndexChanged="ddTransitType_SelectedIndexChanged" TabIndex="12">
                    <asp:ListItem Value="0" Text="-Select-"></asp:ListItem>
                    <asp:ListItem Value="1" Text="Move to Customer Place"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Move To General Warehouse"></asp:ListItem>
                    <asp:ListItem Value="3" Text="Move To In Bonded Warehouse"></asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddWarehouse" runat="server" Visible="false" OnSelectedIndexChanged="ddWarehouse_SelectedIndexChanged"
                    AutoPostBack="true" TabIndex="10">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RFVBonded" runat="server" ControlToValidate="ddWarehouse"
                    SetFocusOnError="true" InitialValue="0" ErrorMessage="Please Select Warehouse Name"
                    Text="*" ValidationGroup="Required" Enabled="false"></asp:RequiredFieldValidator>
            </td>
        </tr>--%>
                            </table>
                        </fieldset>
                        <fieldset>
                            <legend>Delivery History</legend>
                            <div style="overflow: scroll;">
                                <asp:GridView ID="GridViewVehicle" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                                    DataSourceID="DataSourceVehicle" CellPadding="4" AllowPaging="True" AllowSorting="True"
                                    PageSize="20" OnRowCommand="GridViewVehicle_RowCommand" OnRowDataBound="GridViewVehicle_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Container No" DataField="ContainerNo" />
                                        <asp:BoundField HeaderText="Packages" DataField="NoOfPackages" />
                                        <asp:BoundField HeaderText="Vehicle No" DataField="VehicleNo" />
                                        <asp:BoundField HeaderText="Transporter" DataField="TransporterName" />
                                        <asp:BoundField HeaderText="LR No" DataField="LRNo" />
                                        <asp:BoundField HeaderText="LR Date" DataField="LRDate" DataFormatString="{0:dd/MM/yyyy}" />
                                        <%--<asp:BoundField HeaderText="Delivery Date" DataField="DeliveryDate" DataFormatString="{0:dd/MM/yyyy}" />--%>
                                        <asp:BoundField HeaderText="Dispatch Date" DataField="DispatchDate" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:TemplateField HeaderText="LR Attachment">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownload" runat="server" Text='<%#Eval("PODAttachmentPath") %>' CommandName="Download"
                                                    CommandArgument='<%#Eval("PODDownload") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="User" DataField="UserName" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                        <div>
                            <asp:SqlDataSource ID="DataSourceVehicle" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetDeliveryDetail" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    <asp:Parameter Name="TransitType" DefaultValue="1" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </ContentTemplate>
                </cc1:TabPanel>
            </cc1:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
