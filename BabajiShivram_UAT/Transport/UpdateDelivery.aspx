<%@ Page Title="Update Delivery" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UpdateDelivery.aspx.cs"
    Inherits="Transport_UpdateDelivery" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
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
                <asp:ValidationSummary ID="vsSummaryFields" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" />
                <asp:HiddenField ID="hdnBranchId" runat="server" />
            </div>
            <div class="clear">
            </div>
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
                <cc1:CalendarExtender ID="CalChallahDate" runat="server" Enabled="True" EnableViewState="False"
                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgChallanDate" PopupPosition="BottomRight"
                    TargetControlID="txtBabajiChallanDate">
                </cc1:CalendarExtender>
                <asp:HiddenField ID="hdnTruckRequestDate" runat="server" />
            </div>
            <fieldset>
                <legend>Transport Request Detail</legend>
                <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <tr>
                        <td>Ref No.
                        </td>
                        <td>
                            <asp:Label ID="lblTRRefNo" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                        <td>Job No.
                        </td>
                        <td>
                            <asp:Label ID="lblJobNo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Truck Request Date
                        </td>
                        <td>
                            <asp:Label ID="lblTruckRequestDate" runat="server"></asp:Label>
                        </td>
                        <td>Customer Name
                        </td>
                        <td>
                            <asp:Label ID="lblCustName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Division
                        </td>
                        <td>
                            <asp:Label ID="lblDivision" runat="server"></asp:Label>
                        </td>
                        <td>Plant
                        </td>
                        <td>
                            <asp:Label ID="lblPlant" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Location
                        </td>
                        <td>
                            <asp:Label ID="lblLocationFrom" runat="server"></asp:Label>
                        </td>
                        <td>Destination
                        </td>
                        <td>
                            <asp:Label ID="lblDestination" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Gross Weight (Kgs)
                        </td>
                        <td>
                            <asp:Label ID="lblGrossWeight" runat="server"></asp:Label>
                        </td>
                        <td>Cont 20"
                        </td>
                        <td>
                            <asp:Label ID="lblCon20" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Cont 40"
                        </td>
                        <td>
                            <asp:Label ID="lblCon40" runat="server"></asp:Label>
                        </td>
                        <td>Delivery Type
                        </td>
                        <td>
                            <asp:Label ID="lblDeliveryType_Loc" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                            <td>
                                PickUp Address
                            </td>
                            <td>
                                 <asp:Label ID="lblPickAdd" runat="server"></asp:Label>
                            </td>
                            <td>
                                Drop Address
                            </td>
                            <td>
                                  <asp:Label ID="lblDropAdd" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Address Details
                            </td>
                            <td>
                                Pincode&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; City&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp State
                            </td>
                            <td> Address Details
                            </td>
                            <td>
                                Pincode&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;City &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp State
                            </td>
                        </tr>
                        <tr>
                            <td>
                                </td>
                            <td>
                                 <asp:Label ID="lblpickPincode" runat="server">  </asp:Label>
                                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
                                  <asp:Label ID="lblpickCity" runat="server"></asp:Label>
                                  &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
                                  <asp:Label ID="lblpickState" runat="server"></asp:Label>
                            </td>
                            <td>
                                </td>
                            <td>
                                  <asp:Label ID="lblDropPincode" runat="server"></asp:Label> 
                                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                                  <asp:Label ID="lblDropCity" runat="server"> </asp:Label> 
                                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
                                  <asp:Label ID="lblDropState" runat="server"></asp:Label>
                            </td>
                        </tr>
                </table>
            </fieldset>
            <fieldset>
                <legend>Delivery Detail</legend>
                <div class="m">
                    <asp:Button ID="btnSubmit" Text="Save" runat="server" OnClick="btnSubmit_Click" ValidationGroup="Required" TabIndex="16" />
                    <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="false" OnClick="btnCancel_Click" TabIndex="16" />
                </div>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <div id="DivPackages" runat="server">
                        <tr>
                            <td>No of Packages
                                <asp:CompareValidator ID="CompValPackages" runat="server" ControlToValidate="txtNoOfPackages"
                                    Operator="DataTypeCheck" SetFocusOnError="true" Type="Integer" Text="Invalid Packages"
                                    ErrorMessage="Invalid No of Packages" Display="Dynamic" ValidationGroup="Required"></asp:CompareValidator>
                                <asp:RequiredFieldValidator ID="RFVNoOfPkgs" runat="server" ControlToValidate="txtNoOfPackages"
                                    SetFocusOnError="true" Text="*" ErrorMessage="Please Enter No of Packages" Display="Dynamic"
                                    ValidationGroup="Required"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNoOfPackages" MaxLength="8" AutoPostBack="true" OnTextChanged="txtNoOfPackages_TextChanged"
                                    runat="server" Width="100px" TabIndex="3"></asp:TextBox>
                            </td>
                            <td>Balance Packages
                            </td>
                            <td>
                                <asp:Label ID="lblBalancePackage" runat="server"></asp:Label>
                                <asp:HiddenField ID="hdnBalancePkg" runat="server" Value="0" />
                            </td>
                        </tr>
                    </div>
                    <div id="DivContainer" runat="server">
                        <tr>
                            <td>Container No
                                <asp:RequiredFieldValidator ID="RFVContainer" runat="server" ControlToValidate="ddContainerNo"
                                    InitialValue="0" SetFocusOnError="true" Text="*" ErrorMessage="Please Select Container No."
                                    Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddContainerNo" runat="server" TabIndex="3">
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>Delivered Container
                            </td>
                            <td>
                                <asp:Label ID="lblDeliveredContainer" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </div>
                    <tr>
                        <td>Vehicle No & Type
                            <asp:RequiredFieldValidator ID="RFVVehicleNo" runat="server" ControlToValidate="ddVehicleNo" InitialValue="0"
                                SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Vehicle No." Display="Dynamic"
                                ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddVehicleNo" runat="server" Width="100px" TabIndex="3" AutoPostBack="true" OnSelectedIndexChanged="ddVehicleNo_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddVehicleType" runat="server" Width="100px" TabIndex="4">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RFVVehicleType" runat="server" ControlToValidate="ddVehicleType"
                                SetFocusOnError="true" Text="*" InitialValue="0" ErrorMessage="Please Select Vehicle Type"
                                Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>Vehicle Received Date
                            <cc1:MaskedEditExtender ID="MEditReceivedDate" TargetControlID="txtVehicleRecdDate"
                                Mask="99/99/9999" MessageValidatorTip="true" MaskType="Date" AutoComplete="false"
                                runat="server">
                            </cc1:MaskedEditExtender>
                            <cc1:MaskedEditValidator ID="MEditValReceivedDate" ControlExtender="MEditReceivedDate"
                                ControlToValidate="txtVehicleRecdDate" IsValidEmpty="true" InvalidValueBlurredMessage="Invalid Received Date"
                                InvalidValueMessage="Vehicle Received Date is invalid" MinimumValueMessage="Invalid Received Date"
                                MaximumValueMessage="Invalid Received Date" MaximumValueBlurredMessage="Invalid Date" MinimumValue="01/01/2015" SetFocusOnError="true"
                                runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVehicleRecdDate" runat="server" Width="100px" TabIndex="4" placeholder="dd/mm/yyyy"></asp:TextBox>
                            <asp:Image ID="imgVehicleRecd" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>Delivery Destination
                            <asp:RequiredFieldValidator ID="RFVDestination" runat="server" ControlToValidate="txtDeliveryPoint"
                                InitialValue="" SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Delivery Destination" Display="Dynamic"
                                ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDeliveryPoint" runat="server" MaxLength="100" TabIndex="5"></asp:TextBox>
                        </td>
                        <td>Transporter
                            <asp:RequiredFieldValidator ID="RFVTransID" runat="server" ControlToValidate="ddTransporter"
                                InitialValue="0" SetFocusOnError="true" Text="*" ErrorMessage="Please Select Transporter" Display="Dynamic"
                                ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddTransporter" runat="server" Visible="false" TabIndex="6">
                                <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Dispatch Date
                            <cc1:MaskedEditExtender ID="MEditDispatchDate" TargetControlID="txtDispatchDate"
                                Mask="99/99/9999" MessageValidatorTip="true" MaskType="Date" AutoComplete="false"
                                runat="server">
                            </cc1:MaskedEditExtender>
                            <cc1:MaskedEditValidator ID="MEditValDispatchDate" ControlExtender="MEditDispatchDate"
                                ControlToValidate="txtDispatchDate" IsValidEmpty="false" EmptyValueBlurredText="*"
                                EmptyValueMessage="Please Enter Dispatch Date" InvalidValueBlurredMessage="Invalid Date"
                                InvalidValueMessage="Dispatch Date is invalid" MinimumValueMessage="Invalid Dispatch Date"
                                MaximumValueMessage="Invalid Dispatch Date" MinimumValue="01/01/2015" SetFocusOnError="true"
                                MaximumValueBlurredMessage="Invalid Date" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDispatchDate" runat="server" Width="100px" TabIndex="7" placeholder="dd/mm/yyyy"></asp:TextBox>
                            <asp:Image ID="imgClearance" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                                runat="server" />
                        </td>
                        <td>LR No
                        </td>
                        <td>
                            <asp:TextBox ID="txtLRNo" runat="server" MaxLength="50" TabIndex="8"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>LR Date
                            <cc1:MaskedEditExtender ID="MEditLRDate" TargetControlID="txtLRDate" Mask="99/99/9999"
                                MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                            </cc1:MaskedEditExtender>
                            <cc1:MaskedEditValidator ID="MEditValLRDate" ControlExtender="MEditLRDate" ControlToValidate="txtLRDate"
                                IsValidEmpty="true" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="LR Date is invalid"
                                MinimumValueMessage="Invalid LR Date" MaximumValueMessage="Invalid LR Date" MinimumValue="01/01/2015"
                                MaximumValueBlurredMessage="Invalid Date" SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLRDate" runat="server" Width="100px" TabIndex="9" placeholder="dd/mm/yyyy"></asp:TextBox>
                            <asp:Image ID="imgLR" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                        </td>
                        <td>Cargo Delivered Date
                            <cc1:MaskedEditExtender ID="MEditDeliveredDate" TargetControlID="txtDeliveryDate"
                                Mask="99/99/9999" MessageValidatorTip="true" MaskType="Date" AutoComplete="false"
                                runat="server">
                            </cc1:MaskedEditExtender>
                            <cc1:MaskedEditValidator ID="MEditValDeliveredDate" ControlExtender="MEditDeliveredDate"
                                ControlToValidate="txtDeliveryDate" IsValidEmpty="true" InvalidValueBlurredMessage="Invalid Date"
                                InvalidValueMessage="Cargo Delivered Date is invalid" MinimumValueMessage="Invalid Delivered Date"
                                MaximumValueMessage="Invalid Delivered Date" MinimumValue="01/01/2015" SetFocusOnError="true"
                                MaximumValueBlurredMessage="Invalid Date" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
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
                            <asp:TextBox ID="txtCargoPersonName" runat="server" MaxLength="50" TabIndex="11"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Delivery Type
                        </td>
                        <td>
                            <asp:Label ID="lblDeliveryType" runat="server"></asp:Label>
                            <asp:HiddenField ID="hdnDeliveryTypeId" Value="0" runat="server" />
                        </td>
                        <td>Empty container return date
                                <cc1:MaskedEditExtender ID="MEditReturnDate" TargetControlID="txtReturnDate" Mask="99/99/9999"
                                    MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                            <cc1:MaskedEditValidator ID="MEditValReturnDate" ControlExtender="MEditReturnDate"
                                ControlToValidate="txtReturnDate" IsValidEmpty="true" InvalidValueBlurredMessage="Invalid Date"
                                InvalidValueMessage="Container return date is invalid" MinimumValueMessage="Invalid Date"
                                MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015"
                                SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtReturnDate" runat="server" Width="100px" TabIndex="12" placeholder="dd/mm/yyyy"></asp:TextBox>
                            <asp:Image ID="imgReturn" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>Babaji Challan No
                            <asp:RequiredFieldValidator ID="RFVBabajiChalanNo" runat="server" ControlToValidate="txtBabajiChallanNo"
                                SetFocusOnError="true" Text="*" InitialValue="" ErrorMessage="Please Enter Babaji Challan No"
                                Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBabajiChallanNo" runat="server" MaxLength="50" TabIndex="13"></asp:TextBox>&nbsp;
                        </td>
                        <td>Babaji Challan Date
                            <cc1:MaskedEditExtender ID="MEditChallanDate" TargetControlID="txtBabajiChallanDate"
                                Mask="99/99/9999" MessageValidatorTip="true" MaskType="Date" AutoComplete="false"
                                runat="server">
                            </cc1:MaskedEditExtender>
                            <cc1:MaskedEditValidator ID="MEditValChallanDate" ControlExtender="MEditChallanDate"
                                ControlToValidate="txtBabajiChallanDate" IsValidEmpty="true" InvalidValueBlurredMessage="Invalid Date"
                                InvalidValueMessage="Challan Date is invalid" MinimumValueMessage="Invalid Challan Date"
                                MaximumValueMessage="Invalid Challan Date" MinimumValue="01/01/2015"
                                SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBabajiChallanDate" runat="server" Width="100px" TabIndex="13"
                                placeholder="dd/mm/yyyy"></asp:TextBox>
                            <asp:Image ID="imgChallanDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>Babaji Challan Copy
                        </td>
                        <td>
                            <asp:FileUpload ID="fuChallanCopy" runat="server" TabIndex="13" />
                        </td>
                        <td>Damage Image
                        </td>
                        <td>
                            <asp:FileUpload ID="fuDamageCopy" runat="server" TabIndex="13" />
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
                </table>
            </fieldset>
            <fieldset>
                <legend>Delivery History</legend>
                <div style="overflow: scroll;">
                    <asp:GridView ID="GridViewVehicle" runat="server" AutoGenerateColumns="False" CssClass="table"
                        Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="DeliveryId"
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
                                    <asp:LinkButton ID="lnkDownload" runat="server" Text='<%#Eval("PODAttachmentPath") %>'
                                        CommandName="Download" CommandArgument='<%#Eval("PODDownload") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="User" DataField="UserName" />
                        </Columns>
                    </asp:GridView>
                </div>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="DataSourceVehicle" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TR_GetDeliveryDetail" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

