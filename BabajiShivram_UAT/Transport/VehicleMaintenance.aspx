<%@ Page Title="Vehicle Maintenance" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="VehicleMaintenance.aspx.cs" Inherits="Transport_VehicleMaintenance" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release"/>
<script type="text/javascript" src="../JS/jquery-1.4.4.min.js"></script>
<script type="text/javascript">
    $(function () {
        $("[id*=rdlMistake]").click(function () {
            var checked_radio = $("[id*=rdlMistake] input:checked");
            var value = checked_radio.val();
            var text = checked_radio.closest("td").find("label").html();
            var txtMistakePersonName = document.getElementById('<%=txtMistakePersonName.ClientID%>');
            var txtMistakeRemark = document.getElementById('<%=txtMistakeRemark.ClientID%>');
            var RFVMistakeName = document.getElementById('<%=RFVMistakeName.ClientID%>');
            var RFVMistakeReason = document.getElementById('<%=RFVMistakeReason.ClientID%>');

            if (value == "0") {
                txtMistakePersonName.disabled = true;
                txtMistakeRemark.disabled = true;
                ValidatorEnable(RFVMistakeName, false);
                ValidatorEnable(RFVMistakeReason, false);
            }
            else {
                txtMistakePersonName.disabled = false;
                txtMistakeRemark.disabled = false;
                ValidatorEnable(RFVMistakeName, true);
                ValidatorEnable(RFVMistakeReason, true);
            }
            return true;
        });
    });
</script>
    <div>
        <cc1:CalendarExtender ID="calWorkDate" runat="server" Enabled="True" EnableViewState="False"
            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgWorkDate"
            PopupPosition="BottomRight" TargetControlID="txtWorkDate">
        </cc1:CalendarExtender>
        <cc1:CalendarExtender ID="calWorkEndDate" runat="server" Enabled="True" EnableViewState="False"
            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgWorkEndDate"
            PopupPosition="BottomRight" TargetControlID="txtWorkEndDate">
        </cc1:CalendarExtender>
    </div>
    <div align="center">
        <asp:Label ID="lblError" runat="server" EnableViewState="false" ></asp:Label>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" />
    </div>
    <div class="clear"></div>
    <fieldset><legend id="legRefNo" runat="server">Work Detail</legend>
    <div class="m clear">
        <asp:Button ID="btnSubmit" Text="Save" runat="server" OnClick="btnSubmit_Click"
            ValidationGroup="Required" TabIndex="12" />
        <asp:Button ID="btnCancel" Text="Cancel" CausesValidation="false"
            OnClick="btnCancel_OnClick" runat="server" TabIndex="13" />
    </div>
    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
        <tr>
            <td>
                Work Start Date
                <asp:RequiredFieldValidator ID="RFVWorkDate" ControlToValidate="txtWorkDate" SetFocusOnError="true"
                    runat="server" Text="*" ErrorMessage="Please Enter Work Date" 
                    ValidationGroup="Required" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:TextBox ID="txtWorkDate" runat="server" Width="100px" TabIndex="1" placeholder="dd/mm/yyyy"></asp:TextBox>
                <asp:Image ID="imgWorkDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                <cc1:MaskedEditExtender ID="MskEdtWorkDate" TargetControlID="txtWorkDate" Mask="99/99/9999" MessageValidatorTip="true" 
                    MaskType="Date" AutoComplete="false" runat="server"></cc1:MaskedEditExtender>
                <cc1:MaskedEditValidator ID="MskEdtValWorkDate" ControlExtender="MskEdtWorkDate" ControlToValidate="txtWorkDate" IsValidEmpty="true" 
                    InvalidValueMessage="Work Date is invalid" MinimumValueMessage="Work Date Is Invalid" MaximumValueMessage="Work Date Is Invalid"
                    MaximumValueBlurredMessage="Invalid Date" MinimumValueBlurredText="Invalid Date"
                    MinimumValue="01/01/2016" SetFocusOnError="true" Runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
            </td>
            <td>
                Work End Date
            </td>
            <td>
                <asp:TextBox ID="txtWorkEndDate" runat="server" Width="100px" TabIndex="2" placeholder="dd/mm/yyyy"></asp:TextBox>
                <asp:Image ID="imgWorkEndDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                <cc1:MaskedEditExtender ID="MskEdtEndWorkDate" TargetControlID="txtWorkEndDate" Mask="99/99/9999" MessageValidatorTip="true" 
                    MaskType="Date" AutoComplete="false" runat="server"></cc1:MaskedEditExtender>
                <cc1:MaskedEditValidator ID="MskEdtValWorkEndDate" ControlExtender="MskEdtEndWorkDate" ControlToValidate="txtWorkEndDate" IsValidEmpty="true" 
                    InvalidValueMessage="Work End Date is invalid" MinimumValueMessage="Work End Date Is Invalid" MaximumValueMessage="Work End Date Is Invalid"
                    MaximumValueBlurredMessage="Invalid Date" MinimumValueBlurredText="Invalid Date"
                    MinimumValue="01/01/2016" SetFocusOnError="true" Runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
            </td>
        </tr>
        <tr>
            <td>
                Vehicle No 
                <asp:RequiredFieldValidator ID="RFVVehicleNo" ControlToValidate="ddVehicle" SetFocusOnError="true"
                   InitialValue="0" runat="server" Text="*" ErrorMessage="Please Select Vehicle No" 
                    ValidationGroup="Required" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:DropDownList id="ddVehicle" runat="server" Width="130px" TabIndex="3">
                    <asp:ListItem Value="0" Text="-Select-"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                Work Location
                <asp:RequiredFieldValidator ID="RFVLocation" ControlToValidate="txtWorkLocation" SetFocusOnError="true"
                InitialValue="" runat="server" Text="*" ErrorMessage="Please Enter Work Location" 
                ValidationGroup="Required" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:TextBox ID="txtWorkLocation" runat="server" MaxLength="100" TabIndex="4"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Start Time
            </td>
            <td>
                <asp:TextBox ID="txtStartTime" runat="server" Text='<%# Bind("StartTime", "{0:t}") %>'
                 Width="60px" TabIndex="5"></asp:TextBox>
                <asp:Label ID="lblStartTimeEdit" runat="server" Text="24 Hours Format"></asp:Label>
                &nbsp;<span>Total Work Hours:</span>&nbsp;&nbsp;<asp:Label ID="lblTotalHour" runat="server" style="color:Blue;"></asp:Label>
                <cc1:MaskedEditExtender ID="meeStartTime" runat="server" AcceptAMPM="false" MaskType="Time"
                    Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                    ErrorTooltipEnabled="true" UserTimeFormat="TwentyFourHour" TargetControlID="txtStartTime"
                    InputDirection="LeftToRight" AcceptNegative="Left">
                </cc1:MaskedEditExtender>
                <cc1:MaskedEditValidator ID="mevStartTime" runat="server" ControlExtender="meeStartTime"
                    ControlToValidate="txtStartTime" IsValidEmpty="False" EmptyValueMessage="Time is required "
                    InvalidValueMessage="Start Time is invalid" Display="Dynamic" EmptyValueBlurredText="Time is required "
                    InvalidValueBlurredMessage="Invalid Start Time" ValidationGroup="Required" />
            </td>
            <td>
                End Time
            </td>
            <td>
                <asp:TextBox ID="txtEndTime" runat="server" Text='<%# Bind("EndTime", "{0:t}") %>'
                    Width="60px" TabIndex="6"></asp:TextBox>
                <cc1:MaskedEditExtender ID="meeEndTime" runat="server" AcceptAMPM="false" MaskType="Time"
                    Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                    ErrorTooltipEnabled="true" UserTimeFormat="TwentyFourHour" TargetControlID="txtEndTime"
                    InputDirection="LeftToRight" AcceptNegative="Left">
                </cc1:MaskedEditExtender>
                <cc1:MaskedEditValidator ID="mevEndTime" runat="server" ControlExtender="meeEndTime"
                    ControlToValidate="txtEndTime" IsValidEmpty="False" EmptyValueMessage="Time is required "
                    InvalidValueMessage="End Time is invalid" Display="Dynamic" EmptyValueBlurredText="Time is required "
                    InvalidValueBlurredMessage="Invalid End Time" ValidationGroup="Required" />
            </td>
        </tr>
        <tr>
            <td>
                Work Description
                <asp:RequiredFieldValidator ID="RFVWorkDesc" ControlToValidate="txtWorkDesc" SetFocusOnError="true"
                InitialValue="" runat="server" Text="*" ErrorMessage="Please Enter Work Description" 
                ValidationGroup="Required" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtWorkDesc" runat="server" TextMode="MultiLine" MaxLength="4000" TabIndex="7"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Negligence of driver ?<br />
                <asp:RadioButtonList ID="rdlMistake" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                    <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td>
                Name <br />
                <asp:TextBox ID="txtMistakePersonName" runat="server" MaxLength="100" Enabled="false"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RFVMistakeName" ControlToValidate="txtMistakePersonName" SetFocusOnError="true"
                InitialValue="" runat="server" Text="*" ErrorMessage="Please Enter Driver Name" Enabled="false"
                ValidationGroup="Required" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td colspan="2">
                Reason<br />
                <asp:TextBox ID="txtMistakeRemark" runat="server" TextMode="MultiLine" Enabled="false" MaxLength="800"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RFVMistakeReason" ControlToValidate="txtMistakeRemark" SetFocusOnError="true"
                InitialValue="" runat="server" Text="*" ErrorMessage="Please Enter Mistake Reason" Enabled="false"
                ValidationGroup="Required" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:ListBox ID="lbCategory" SelectionMode="Multiple" runat="server" TabIndex="8" Height="200px">
                    <%--<asp:ListItem Text="Battery" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Body Repairing" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Electricals" Value="3"></asp:ListItem>
                    <asp:ListItem Text="Engine" Value="4"></asp:ListItem>
                    <asp:ListItem Text="Gear & Clutch" Value="5"></asp:ListItem>
                    <asp:ListItem Text="General" Value="6"></asp:ListItem>
                    <asp:ListItem Text="Labour Charges" Value="7"></asp:ListItem>
                    <asp:ListItem Text="Mechanical" Value="8"></asp:ListItem>
                    <asp:ListItem Text="RTO Annual Passing" Value="9"></asp:ListItem>
                    <asp:ListItem Text="Service" Value="10"></asp:ListItem>
                    <asp:ListItem Text="Spare Parts" Value="11"></asp:ListItem>
                    <asp:ListItem Text="Tyres" Value="12"></asp:ListItem>
                    <asp:ListItem Text="Patta Repairing" Value="14"></asp:ListItem>
                    <asp:ListItem Text="Fuel Injection Pump" Value="15"></asp:ListItem>
                    <asp:ListItem Text="Glass Repairing" Value="16"></asp:ListItem>
                    <asp:ListItem Text="Rope (Rassi)" Value="17"></asp:ListItem>
                    <asp:ListItem Text="Schedule Service" Value="18"></asp:ListItem>
                    <asp:ListItem Text="Tarpaulins" Value="19"></asp:ListItem>
                    <asp:ListItem Text="Washing" Value="20"></asp:ListItem>
                    <asp:ListItem Text="Other" Value="13"></asp:ListItem>
                    <asp:ListItem Text="MPI/TPI" Value="21"></asp:ListItem>
                    <asp:ListItem Text="Barmer Misc" Value="22"></asp:ListItem>--%>
                </asp:ListBox>
            </td>
            <td>
                <asp:ListBox id="lbEmployee" runat="server" SelectionMode="Multiple" TabIndex="9" Height="200px" AppendDataBoundItems="true">
                    <asp:ListItem Text = "Work Done By" Value = "0"></asp:ListItem>
                </asp:ListBox>
            </td>
            <td>
                1. Document Name<br />
                <asp:TextBox ID="txtAttachDocName" runat="server"></asp:TextBox><br />
                2. Document Name<br />
                <asp:TextBox ID="txtAttachDocName1" runat="server"></asp:TextBox><br />
                3.Document Name<br />
                <asp:TextBox ID="txtAttachDocName2" runat="server"></asp:TextBox><br />
            </td>
            <td>
                <asp:FileUpload ID="fuAttachment" runat="server" /><br />
                <asp:FileUpload ID="fuAttachment1" runat="server" /><br />
                <asp:FileUpload ID="fuAttachment2" runat="server" /><br />
            </td>
        
        </tr>
    </table>
   </fieldset>
</asp:Content>

