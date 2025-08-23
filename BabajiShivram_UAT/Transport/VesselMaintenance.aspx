<%@ Page Title="Vessel Maintenance" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="VesselMaintenance.aspx.cs" Inherits="Transport_VesselMaintenance" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release"/>
    <div>
        <cc1:CalendarExtender ID="calWorkDate" runat="server" Enabled="True" EnableViewState="False"
            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgWorkDate"
            PopupPosition="BottomRight" TargetControlID="txtWorkDate">
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
                Vessel No 
            </td>
            <td>
                <asp:Label ID="lblVesselNo" Text="VX DRISANA 01" runat="server"></asp:Label>
            </td>
            <td>
                Work Date
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
                    MinimumValue="01/01/2020" SetFocusOnError="true" Runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
            </td>
        </tr>
        <tr>

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
                <asp:ListBox ID="lbCategory" SelectionMode="Multiple" runat="server" TabIndex="8" Height="200px">
                   
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

