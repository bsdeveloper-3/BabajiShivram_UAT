<%@ Page Title="Weekly Trip Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="WeeklyTripReport.aspx.cs" Inherits="Transport_WeeklyTripReport" Culture="en-GB" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <asp:UpdatePanel ID="upnlWeeklyTrip" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false" ForeColor="Red"></asp:Label>
                <asp:ValidationSummary ID="csRequiredFields" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="vgRequired" CssClass="errorMsg" />
            </div>
            <div>
                <fieldset>
                    <legend>Generate Status Report</legend>
                    <table class="table" border="0" cellpadding="0" cellspacing="0" width="80%" bgcolor="white">
                        <tr>
                            <td>Report Date From
                                <cc1:CalendarExtender ID="calReportDateFrom" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="imgReportDateFrom"
                                    Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtReportDateFrom">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="meeReportDateFrom" TargetControlID="txtReportDateFrom" Mask="99/99/9999" MessageValidatorTip="true"
                                    MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="mevReportDateFrom" ControlExtender="meeReportDateFrom" ControlToValidate="txtReportDateFrom" IsValidEmpty="false"
                                    InvalidValueMessage="Report Date From is invalid" InvalidValueBlurredMessage="Invalid Report Date From" SetFocusOnError="true"
                                    MinimumValueMessage="Invalid Report Date From" MaximumValueMessage="Invalid Report Date From" MinimumValue="01/01/2015" MaximumValue="31/12/2025"
                                    runat="Server"></cc1:MaskedEditValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReportDateFrom" runat="server" Width="125px" placeholder="dd/mm/yyyy" TabIndex="1" ToolTip="Enter Report Date From." AutoPostBack="true" OnTextChanged="txtReportDateFrom_TextChanged"></asp:TextBox>
                                <asp:Image ID="imgReportDateFrom" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                            <td>Report Date To
                                <cc1:CalendarExtender ID="calReportDateTo" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="imgReportDateTo"
                                    Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtReportDateTo">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="meeReportDateTo" TargetControlID="txtReportDateTo" Mask="99/99/9999" MessageValidatorTip="true"
                                    MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="mevReportDateTo" ControlExtender="meeReportDateTo" ControlToValidate="txtReportDateTo" IsValidEmpty="false"
                                    InvalidValueMessage="Report Date To is invalid" InvalidValueBlurredMessage="Invalid Report Date To" SetFocusOnError="true"
                                    MinimumValueMessage="Invalid Report Date To" MaximumValueMessage="Invalid Report Date To" MinimumValue="01/01/2015" MaximumValue="31/12/2025"
                                    runat="Server"></cc1:MaskedEditValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReportDateTo" runat="server" Width="125px" placeholder="dd/mm/yyyy" TabIndex="2" ToolTip="Enter Report Date To." OnTextChanged="txtReportDateTo_TextChanged" AutoPostBack="true"></asp:TextBox>
                                <asp:Image ID="imgReportDateTo" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>Transporter Name
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTransporter" runat="server" Width="300px" TabIndex="3">
                                    <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>Report Type</td>
                            <td>
                                <asp:DropDownList ID="ddlReportType" runat="server" Width="300px">
                                    <asp:ListItem Selected="True" Value="1" Text="Analysis Report"></asp:ListItem>
                                    <asp:ListItem Selected="False" Value="2" Text="Detailed Report"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <div class="m clear">
                        <asp:Button ID="btnGenerateReport" runat="server" Text="Generate Report" ValidationGroup="vgRequired" TabIndex="3"
                            OnClick="btnGenerateReport_Click" />
                        <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
                            <asp:Image ID="imgExport" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                        </asp:LinkButton>
                    </div>
                    <asp:GridView ID="gvWeeklyTripReport" runat="server" CssClass="table" DataSourceID="DataSourceWeeklyTripReport"
                        Width="99%" AutoGenerateColumns="true" AllowSorting="true" AllowPaging="true" PagerSettings-Position="TopAndBottom" PageSize="80">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </fieldset>
            </div>
            <div>
                <asp:SqlDataSource ID="DataSourceTPForTrip" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TR_GetTransporterForTripReport" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceWeeklyTripReport" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TR_rptWeeklyNewTrip" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtReportDateFrom" Name="DateFrom" PropertyName="Text" Type="DateTime" />
                        <asp:ControlParameter ControlID="txtReportDateTo" Name="DateTo" PropertyName="Text" Type="DateTime" />
                        <asp:ControlParameter ControlID="ddlTransporter" Name="TransporterId" PropertyName="SelectedValue" />
                        <asp:ControlParameter ControlID="ddlReportType" Name="ReportType" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

