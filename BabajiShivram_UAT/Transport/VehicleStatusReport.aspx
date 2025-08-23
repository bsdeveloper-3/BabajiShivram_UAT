<%@ Page Title="Vehicle Status Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="VehicleStatusReport.aspx.cs"
    Inherits="Transport_VehicleStatusReport" Culture="en-GB" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <asp:UpdatePanel ID="upnlVehicleStatus" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false" ForeColor="Red"></asp:Label>
                <asp:ValidationSummary ID="csRequiredFields" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="vgRequired" CssClass="errorMsg" />
            </div>
            <div>
                <fieldset>
                    <legend>Generate Status Report</legend>
                    <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>From Date
                                <cc1:CalendarExtender ID="calReportDate" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="imgReportDate"
                                    Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtReportDate">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="meeReportDate" TargetControlID="txtReportDate" Mask="99/99/9999" MessageValidatorTip="true"
                                    MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="mevReportDate" ControlExtender="meeReportDate" ControlToValidate="txtReportDate" IsValidEmpty="false"
                                    InvalidValueMessage="Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true"
                                    MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="31/12/2025"
                                    runat="Server"></cc1:MaskedEditValidator>
                                <asp:TextBox ID="txtReportDate" runat="server" Width="125px" placeholder="dd/mm/yyyy" TabIndex="1" ToolTip="Enter Date."></asp:TextBox>
                                <asp:Image ID="imgReportDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                            <td>To Date
                                <cc1:CalendarExtender ID="calReportDateTo" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="imgReportDateTo"
                                    Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtReportDateTo">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="meeReportDateTo" TargetControlID="txtReportDate" Mask="99/99/9999" MessageValidatorTip="true"
                                    MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="mevReportDateTo" ControlExtender="meeReportDateTo" ControlToValidate="txtReportDateTo" IsValidEmpty="false"
                                    InvalidValueMessage="Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true"
                                    MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="31/12/2025"
                                    runat="Server"></cc1:MaskedEditValidator>

                                <asp:TextBox ID="txtReportDateTo" runat="server" Width="125px" placeholder="dd/mm/yyyy" TabIndex="1" ToolTip="Enter Date."></asp:TextBox>
                                <asp:Image ID="imgReportDateTo" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                            
                            <td>Transporter Name
                                <asp:RequiredFieldValidator ID="rfvTransporter" runat="server" ControlToValidate="ddlTransporter" InitialValue="0"
                                    SetFocusOnError="true" Display="Dynamic" ErrorMessage="Please select transporter." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTransporter" runat="server" Width="300px" TabIndex="2" DataSourceID="DataSourceTPForTrip"
                                    DataTextField="sName" DataValueField="lid" AppendDataBoundItems="true">
                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Button ID="btnGenerateReport" runat="server" Text="Generate Report" ValidationGroup="vgRequired" TabIndex="3"
                                    OnClick="btnGenerateReport_Click" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <div>
                <asp:SqlDataSource ID="DataSourceTPForTrip" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TR_GetTransporterForTripReport" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

