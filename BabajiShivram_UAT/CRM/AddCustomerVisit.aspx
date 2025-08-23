<%@ Page Title="Add Customer Visit" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddCustomerVisit.aspx.cs"
    Inherits="CRM_AddCustomerVisit" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="GVPager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <AjaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlCustVisit" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upnlCustVisit" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:HiddenField ID="hdnCustomerId" runat="server" Value="0" />
                <asp:ValidationSummary ID="vsRequired" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="vgRequired" CssClass="errorMsg" EnableViewState="false" />
            </div>
            <div class="m clear">
                <asp:Button ID="btnAddVisitReport" runat="server" Text="Save" OnClick="btnAddVisitReport_Click" TabIndex="4" CausesValidation="true" ValidationGroup="vgRequired" />
                <asp:Button ID="btnCancelVisitReport" runat="server" Text="Cancel" TabIndex="5" OnClick="btnCancelVisitReport_Click" />
                <asp:Button ID="btnGoBack" runat="server" Text="Go Back" OnClick="btnGoBack_Click" TabIndex="6" />
            </div>
            <fieldset>
                <legend>Add Report</legend>
                <table border="0" cellpadding="0" cellspacing="0" width="70%" bgcolor="white">
                    <colgroup>
                        <col width="10%" />
                        <col width="90%" />
                    </colgroup>
                    <tr>
                        <td>Customer
                            <asp:RequiredFieldValidator ID="rfvCustomer" runat="server" ControlToValidate="ddlCustomer" SetFocusOnError="true" Display="Dynamic"
                                InitialValue="0" ErrorMessage="Please Select Customer" Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCustomer" runat="server" DataSourceID="DataSourceBabajiCustomers" DataTextField="CustName"
                                DataValueField="lid" CssClass="form-control dropdown" AppendDataBoundItems="true" Width="370px" TabIndex="1">
                                <asp:ListItem Selected="True" Value="0" Text="-Select-"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Visit Date
                            <AjaxToolkit:CalendarExtender ID="calVisitDate" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="imgVisitDate"
                                Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtVisitDate">
                            </AjaxToolkit:CalendarExtender>
                            <AjaxToolkit:MaskedEditExtender ID="meeVisitDate" TargetControlID="txtVisitDate" Mask="99/99/9999" MessageValidatorTip="true"
                                MaskType="Date" AutoComplete="false" runat="server">
                            </AjaxToolkit:MaskedEditExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVisitDate" runat="server" Width="80px" placeholder="dd/mm/yyyy" TabIndex="2" ToolTip="Enter Visit Date."></asp:TextBox>
                            <asp:Image ID="imgVisitDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            <AjaxToolkit:MaskedEditValidator ID="mevVisitDate" ControlExtender="meeVisitDate" ControlToValidate="txtVisitDate" IsValidEmpty="false"
                                InvalidValueMessage="Visit Date is invalid" InvalidValueBlurredMessage="Invalid Visit Date" SetFocusOnError="true"
                                MinimumValueMessage="Invalid Visit Date" MaximumValueMessage="Invalid Date" 
                                
                                runat="Server" ValidationGroup="vgRequired" ></AjaxToolkit:MaskedEditValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Visit Category
                            <asp:RequiredFieldValidator ID="rfvCategory" runat="server" ControlToValidate="ddlCustomer" SetFocusOnError="true" Display="Dynamic"
                                InitialValue="0" ErrorMessage="Please Select Customer" Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddCategory" runat="server" DataSourceID="DataSourceVisitCategory" DataTextField="CategoryName"
                                DataValueField="lid" CssClass="form-control dropdown" AppendDataBoundItems="true" Width="370px" TabIndex="1">
                                <asp:ListItem Selected="True" Value="0" Text="-Select-"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Visit Remark
                            <asp:RequiredFieldValidator ID="rfvVisitRemark" runat="server" ControlToValidate="txtVisitRemark" SetFocusOnError="true"
                                Display="Dynamic" ErrorMessage="Please Enter Visit Remark" Text="*" ForeColor="Red" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVisitRemark" runat="server" TextMode="MultiLine" Rows="7" Width="95%" TabIndex="3"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <asp:SqlDataSource ID="DataSourceBabajiCustomers" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="GetCustomerMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            <asp:SqlDataSource ID="DataSourceVisitCategory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="CRM_GetVisitCategory" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

