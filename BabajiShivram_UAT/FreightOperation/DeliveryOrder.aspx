<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DeliveryOrder.aspx.cs" 
    Inherits="FreightOperation_DeliveryOrder" Title="Delivery Order" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AjaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPanelDetail" runat="server">
            <ProgressTemplate>
                <div style="position:absolute;visibility:visible;border:none;z-index:100;width:90%;height:90%;background:#FAFAFA; filter: alpha(opacity=80);-moz-opacity:.8; opacity:.8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position:relative; top:40%;left:40%; "/>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPanelDetail" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="ValSummaryFreightDetail" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
            </div>
            <div class="clear"></div>
            <fieldset><legend>Delivery Order</legend>
                <div class="m clear">
                    <asp:Button ID="btnUpdate" runat="server" Text="Save" OnClick="btnSubmit_Click" ValidationGroup="Required" TabIndex="8"/>
                    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CausesValidation="False"
                        Text="Cancel" TabIndex="9" />
                </div>
                <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
                    <tr>
                        <td>
                            Job No
                        </td>
                        <td>
                            <asp:Label ID="lblJobNo" runat="server"></asp:Label>
                        </td>
                        <td>
                            Booking Month
                        </td>
                        <td>
                            <asp:Label ID="lblBookingMonth" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            CHA Name
                            <asp:RequiredFieldValidator ID="RFVCHA" runat="server" ControlToValidate="txtChaName" Display="Dynamic" 
                            SetFocusOnError="true" Text="*" ErrorMessage="Please Enter CHA Name" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                           <asp:TextBox ID="txtChaName" runat="server" TabIndex="1"></asp:TextBox>
                        </td>
                        <td>
                            Terms of Payment
                            <asp:RequiredFieldValidator ID="RFVPayTerms" runat="server" ControlToValidate="ddPaymentsTerms" Display="Dynamic" 
                             InitialValue="0" SetFocusOnError="true" Text="*" ErrorMessage="Please Select Payments Terms" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddPaymentsTerms" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddPaymentsTerms_SelectedIndexChanged" TabIndex="2">
                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Credit" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Against DO" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            DO Issued To
                            <asp:RequiredFieldValidator ID="RFVIssuedTo" runat="server" ControlToValidate="txtDoIssuedTo" Display="Dynamic" 
                            SetFocusOnError="true" Text="*" ErrorMessage="Please Enter DO Issued To" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                           <asp:TextBox ID="txtDoIssuedTo" runat="server" TabIndex="3"></asp:TextBox>
                        </td>
                        <td>
                            Payment Type
                            <asp:RequiredFieldValidator ID="RFVPaytype" runat="server" ControlToValidate="ddPaymentType" Display="Dynamic" 
                             Enabled="false" InitialValue="0" SetFocusOnError="true" Text="*" ErrorMessage="Please Select Payment Type" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddPaymentType" runat="server" TabIndex="4">
                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Cheque" Value="1"></asp:ListItem>
                                <asp:ListItem Text="DD" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Cash" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Credit" Value="4"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Cheque No / DD No
                        </td>
                        <td>
                           <asp:TextBox ID="txtChequeNo" runat="server" TabIndex="5"></asp:TextBox>
                        </td>
                        <td>
                            Cheque Date / DD Date
                            <AjaxToolkit:CalendarExtender ID="calChequeDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgChequeDate" PopupPosition="BottomRight"
                                TargetControlID="txtChequeDate">
                            </AjaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtChequeDate" runat="server" Width="100px" TabIndex="6"></asp:TextBox>
                            <asp:Image ID="imgChequeDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server"/>
                            <AjaxToolkit:MaskedEditExtender ID="MskExtChequeDate" TargetControlID="txtChequeDate" Mask="99/99/9999" MessageValidatorTip="true" 
                                MaskType="Date" AutoComplete="false" runat="server"></AjaxToolkit:MaskedEditExtender>
                            <AjaxToolkit:MaskedEditValidator ID="MskValChequeDate" ControlExtender="MskExtChequeDate" ControlToValidate="txtChequeDate" IsValidEmpty="true" 
                                InvalidValueMessage="Cheque/DD Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                EmptyValueMessage="Required" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2025" 
                                Runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            Amount
                            <asp:CompareValidator ID="CompValAmount" runat="server" ControlToValidate="txtAmount"
                            Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" ErrorMessage="Invalid Amount"
                            Display="Dynamic" ValidationGroup="Required"></asp:CompareValidator>
                        </td>
                        <td>
                           <asp:TextBox ID="txtAmount" runat="server" TabIndex="7" MaxLength="15"></asp:TextBox>
                        </td>
                        <td>
                            <asp:LinkButton ID="lnkCreateDOPDF" Text="Create Delivery Order" runat="server" OnClick="lnkCreateDOPDF_Click"></asp:LinkButton>
                        </td>
                        <td>
                        </td>
                    </tr>
			<tr>
                        <td>
                            Remark
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark" Width="80%" TextMode="MultiLine" MaxLength="400" runat="server" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

