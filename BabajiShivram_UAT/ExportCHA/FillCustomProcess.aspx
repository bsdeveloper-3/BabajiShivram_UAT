<%@ Page Title="Fill custom Process" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FillCustomProcess.aspx.cs"
    Inherits="ExportCHA_FillCustomProcess" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <cc1:CalendarExtender ID="calCartDate" runat="server" Enabled="True" EnableViewState="False"
            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgCartDate"
            PopupPosition="BottomRight" TargetControlID="txtCartDate">
        </cc1:CalendarExtender>
        <cc1:CalendarExtender ID="calRegisterationDate" runat="server" Enabled="True" EnableViewState="False"
            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgRegistrationDate"
            PopupPosition="BottomRight" TargetControlID="txtRegistrationDate">
        </cc1:CalendarExtender>
        <cc1:CalendarExtender ID="calExamineDate" runat="server" Enabled="True" EnableViewState="False"
            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgExamineDate" PopupPosition="BottomRight"
            TargetControlID="txtExamineDate">
        </cc1:CalendarExtender>
        <cc1:CalendarExtender ID="calExamineReportDate" runat="server" Enabled="True" EnableViewState="False"
            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgExamineReport" PopupPosition="BottomRight"
            TargetControlID="txtExamineReportDate">
        </cc1:CalendarExtender>
        <cc1:CalendarExtender ID="calLEODate" runat="server" Enabled="True" EnableViewState="False"
            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgLEODate"
            PopupPosition="BottomRight" TargetControlID="txtLEODate">
        </cc1:CalendarExtender>
    </div>

    <div align="center">
        <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" />
      <asp:HiddenField ID="hdnExportType" runat="server" />
    </div>
    <div class="clear">
    </div>
    <fieldset id="fsJobDetail" runat="server">
        <legend>Job Detail</legend>
        <div class="m clear">
            <asp:Button ID="btnSubmit" Text="Save" runat="server" OnClick="btnSubmit_Click"
                ValidationGroup="Required" TabIndex="7" />
            <asp:Button ID="btnCancel" Text="Cancel" CausesValidation="false"
                runat="server" OnClick="btnCancel_Click" TabIndex="8" />
        </div>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
            <tr>
                <td>BS Job No.
                </td>
                <td>
                    <asp:Label ID="lblJobRefNo" runat="server"></asp:Label>
                </td>
                <td>Cust Ref No
                </td>
                <td>
                    <asp:Label ID="lblCustRefNo" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Customer
                </td>
                <td>
                    <asp:Label ID="lblCustName" runat="server"></asp:Label>
                </td>
                <td>Consignee
                </td>
                <td>
                    <asp:Label ID="lblConsigneeName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Shipper
                </td>
                <td>
                    <asp:Label ID="lblShipper" runat="server"></asp:Label>
                </td>
                <td>Mode
                </td>
                <td>
                    <asp:Label ID="lblMode" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Port Of Loading
                </td>
                <td>
                    <asp:Label ID="lblPOL" runat="server"></asp:Label>
                </td>
                <td>Port Of Discharge
                </td>
                <td>
                    <asp:Label ID="lblPOD" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Country Consignment
                </td>
                <td>
                    <asp:Label ID="lblCountryConsgn" runat="server"></asp:Label>
                </td>
                <td>Destination Country  
                </td>
                <td>
                    <asp:Label ID="lblDestCountry" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Forwarder Name</td>
                <td>
                    <asp:Label ID="lblForwarderName" runat="server"></asp:Label>
                </td>
                <td>No Of Packages
                </td>
                <td>
                    <asp:Label ID="lblNoOfPkg" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Gross Weight
                </td>
                <td>
                    <asp:Label ID="lblGrossWT" runat="server"></asp:Label>
                </td>
                <td>Net WT
                </td>
                <td>
                    <asp:Label ID="lblNetWT" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>SB No
                </td>
                <td>
                    <asp:Label ID="lblSBNo" runat="server"></asp:Label>
                </td>
                <td>SB Date
                </td>
                <td>
                    <asp:Label ID="lblSBDate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Carting Date
                    <cc1:MaskedEditExtender ID="meeCartdate" TargetControlID="txtCartDate" Mask="99/99/9999"
                        MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                    </cc1:MaskedEditExtender>
                    <cc1:MaskedEditValidator ID="mevCartDate" ControlExtender="meeCartdate" ControlToValidate="txtCartDate" IsValidEmpty="false"
                        EmptyValueMessage="Enter Carting Date." EmptyValueBlurredText="*" InvalidValueMessage="Carting Date is invalid" SetFocusOnError="true"
                        MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid"
                        MinimumValue="01/01/2014" MaximumValue="31/12/2025" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtCartDate" runat="server" Width="100px" MaxLength="10" TabIndex="2" placeholder="dd/mm/yyyy"
                        OnTextChanged="txtCartDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                    <asp:Image ID="imgCartDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                        runat="server" />
                </td>
                <td>
                    <asp:Label ID="lblMarkedPassingDate" runat="server"></asp:Label>
                    <cc1:CalendarExtender ID="calMarkPassingDate" runat="server" Enabled="True" EnableViewState="False"
                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgMarkPassingdate"
                        PopupPosition="BottomRight" TargetControlID="txtMarkPassingDate">
                    </cc1:CalendarExtender>
                    <cc1:MaskedEditExtender ID="MEditMarkPassingDate" TargetControlID="txtMarkPassingDate" Mask="99/99/9999" MessageValidatorTip="true"
                        MaskType="Date" AutoComplete="false" runat="server">
                    </cc1:MaskedEditExtender>
                    <cc1:MaskedEditValidator ID="MEditValMarkPassingDate" ControlExtender="MEditMarkPassingDate" ControlToValidate="txtMarkPassingDate" IsValidEmpty="false"
                        EmptyValueBlurredText="*" InvalidValueMessage="Mark Date is invalid" Display="Dynamic"
                        SetFocusOnError="true" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid"
                        MinimumValue="01/01/2014" MaximumValue="31/12/2025" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtMarkPassingDate" runat="server" Width="100px" MaxLength="10" TabIndex="1" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:Image ID="imgMarkPassingdate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                        runat="server" />
                </td>
            </tr>
            <tr>

                <td>Registration Date
                    <cc1:MaskedEditExtender ID="MEditRegistrationDate" TargetControlID="txtRegistrationDate" Mask="99/99/9999"
                        MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                    </cc1:MaskedEditExtender>
                    <cc1:MaskedEditValidator ID="MEditValRegistrationDate" ControlExtender="MEditRegistrationDate" ControlToValidate="txtRegistrationDate" IsValidEmpty="true"
                        EmptyValueMessage="Enter Registration Date." EmptyValueBlurredText="*" InvalidValueMessage="Registration Date is invalid" SetFocusOnError="true"
                        MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid"
                        MinimumValue="01/01/2014" MaximumValue="31/12/2025" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtRegistrationDate" runat="server" Width="100px" MaxLength="10" TabIndex="2" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:Image ID="imgRegistrationDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                        runat="server" />
                </td>

                <td>Examine Date
                    <cc1:MaskedEditExtender ID="MEditExamineDate" TargetControlID="txtExamineDate" Mask="99/99/9999"
                        MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                    </cc1:MaskedEditExtender>
                    <cc1:MaskedEditValidator ID="MEditValExamineDate" ControlExtender="MEditExamineDate" ControlToValidate="txtExamineDate" IsValidEmpty="true"
                        EmptyValueMessage="Enter Examine Date" EmptyValueBlurredText="*" InvalidValueMessage="Examine Date is invalid" SetFocusOnError="true"
                        MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid"
                        MinimumValue="01/01/2014" MaximumValue="31/12/2025" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtExamineDate" runat="server" Width="100px" MaxLength="10" TabIndex="3" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:Image ID="imgExamineDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Examine Report Date
                    <cc1:MaskedEditExtender ID="MEditExamineReportDate" TargetControlID="txtExamineReportDate" Mask="99/99/9999"
                        MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                    </cc1:MaskedEditExtender>
                    <cc1:MaskedEditValidator ID="MEditValExamineReportDate" ControlExtender="MEditExamineReportDate" ControlToValidate="txtExamineReportDate" IsValidEmpty="true"
                        EmptyValueMessage="Enter Examine Report Date" EmptyValueBlurredText="*" InvalidValueMessage="Examine Report Date is invalid" SetFocusOnError="true"
                        MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid"
                        MinimumValue="01/01/2014" MaximumValue="31/12/2025" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtExamineReportDate" runat="server" Width="100px" TabIndex="4" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:Image ID="imgExamineReport" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                        runat="server" />
                </td>

                <td>Supretendent LEO Date
                    <cc1:MaskedEditExtender ID="MEditLEODate" TargetControlID="txtLEODate" Mask="99/99/9999"
                        MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                    </cc1:MaskedEditExtender>
                    <cc1:MaskedEditValidator ID="MEditValLEODate" ControlExtender="MEditLEODate" ControlToValidate="txtLEODate" IsValidEmpty="true"
                        EmptyValueMessage="Enter Supretendent LEO Date" EmptyValueBlurredText="*" InvalidValueMessage="Supretendent LEO Date is invalid" SetFocusOnError="true"
                        MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid"
                        MinimumValue="01/01/2014" MaximumValue="31/12/2025" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtLEODate" runat="server" Width="100px" MaxLength="10" TabIndex="5" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:Image ID="imgLEODate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Remark</td>
                <td>
                    <asp:TextBox ID="txtRemark" runat="server" Width="200px" TabIndex="6" TextMode="MultiLine" Rows="3"></asp:TextBox>
                </td>
                <td>
                    <asp:HiddenField ID="hdnDeliveryStatus" runat="server" />
                </td>
                <td></td>
            </tr>
        </table>
    </fieldset>
</asp:Content>

