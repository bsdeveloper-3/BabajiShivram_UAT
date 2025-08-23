<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TransClearance.aspx.cs" Inherits="Transport_TransClearance" 
    MasterPageFile="~/MasterPage.master" Title="Consolidated - Delivery Detail" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <%--<asp:UpdatePanel ID="updUpdatePanlel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>--%>
    <div align="center">
        <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" />
        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="RequiredJob" CssClass="errorMsg" />
    </div>
    <fieldset style="width: 90%;">
        <legend>Consolidated - Job Delivery Detail</legend>
        <asp:Wizard ID="wzDelivery" runat="server" DisplaySideBar="false" OnNextButtonClick="wzDelivery_NextButtonClick"
            OnFinishButtonClick="wzDelivery_FinishButtonClick">
            <WizardSteps>
                <asp:WizardStep ID="StepMain" StepType="Start" Title="Vehicle Detail">
                    <cc1:CalendarExtender ID="CalReceivedDate" runat="server" Enabled="True" EnableViewState="False"
                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgVehicleRecd" PopupPosition="BottomRight"
                        TargetControlID="txtVehicleRecdDate">
                    </cc1:CalendarExtender>
                    <cc1:CalendarExtender ID="CalDispatchDate" runat="server" Enabled="True" EnableViewState="False"
                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDispatch" PopupPosition="BottomRight"
                        TargetControlID="txtDispatchDate">
                    </cc1:CalendarExtender>
                    <cc1:CalendarExtender ID="CalCommonLRDate" runat="server" Enabled="True" EnableViewState="False"
                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgCommonLRdate" PopupPosition="BottomRight"
                        TargetControlID="txtCommonLRDate">
                    </cc1:CalendarExtender>
                    <asp:HiddenField ID="hdnTransRateId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnDestination" runat="server" />
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>Transportation By Babaji
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rdlTransport" runat="server" RepeatDirection="Horizontal"
                                    AutoPostBack="true" OnSelectedIndexChanged="rdlTransport_SelectedIndexChanged">
                                    <asp:ListItem Text="Yes" Value="1" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                </asp:RadioButtonList>
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
                                <asp:TextBox ID="txtTransporterName" runat="server" MaxLength="100" Visible="false"></asp:TextBox>
                                <asp:DropDownList ID="ddTransporter" runat="server" Visible="true">
                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Vehicle Number
                        <asp:RequiredFieldValidator ID="RFVVehicleNo" runat="server" ControlToValidate="txtVehicleNo" SetFocusOnError="true"
                            InitialValue="" Text="*" ErrorMessage="Please Enter Vehicle No." Display="Static" ValidationGroup="Required"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVehicleNo" runat="server" MaxLength="50"></asp:TextBox>
                                <asp:HiddenField ID="hdnConsolidateID" runat="server" Value="0" />
                            </td>
                            <td>Vehicle Type
                                <asp:RequiredFieldValidator ID="RFVVehicleType" runat="server" ControlToValidate="ddVehicleType" SetFocusOnError="true"
                                    Text="*" InitialValue="0" ErrorMessage="Please Select Vehicle Type" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddVehicleType" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Vehicle Received Date
                        <cc1:MaskedEditExtender ID="MEditReceivedDate" TargetControlID="txtVehicleRecdDate" Mask="99/99/9999" MessageValidatorTip="true"
                            MaskType="Date" AutoComplete="false" runat="server">
                        </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="MEditValReceivedDate" ControlExtender="MEditReceivedDate" ControlToValidate="txtVehicleRecdDate" IsValidEmpty="false"
                                    EmptyValueBlurredText="*" EmptyValueMessage="Please Enter Vehicle Received Date" InvalidValueBlurredMessage="Invalid Date"
                                    InvalidValueMessage="Vehicle Received Date is invalid" MinimumValueMessage="Invalid Date" MaximumValueMessage="Future date for Vehicle Received not allowed"
                                    MaximumValueBlurredMessage="Invalid Date" MinimumValue="01/01/2015" SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVehicleRecdDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                                <asp:Image ID="imgVehicleRecd" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                            <td>Dispatch Date
                        <cc1:MaskedEditExtender ID="MEditDispatchDate" TargetControlID="txtDispatchDate" Mask="99/99/9999" MessageValidatorTip="true"
                            MaskType="Date" AutoComplete="false" runat="server">
                        </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="MEditValDispatchDate" ControlExtender="MEditDispatchDate" ControlToValidate="txtDispatchDate" IsValidEmpty="false"
                                    EmptyValueBlurredText="*" EmptyValueMessage="Please Enter Dispatch Date" InvalidValueBlurredMessage="Invalid Date"
                                    InvalidValueMessage="Dispatch Date is invalid" MinimumValueMessage="Invalid Date" MaximumValueMessage="Future date for Dispatch Date not allowed"
                                    MaximumValueBlurredMessage="Invalid Date" MinimumValue="01/01/2015" SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDispatchDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                                <asp:Image ID="imgDispatch" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                        </tr>
                        <asp:Panel ID="pnlCommonLRDetail" runat="server" Visible="false">
                            <tr>
                                <td>LR No
<%--                        <asp:RequiredFieldValidator ID="RFVCommonLRNo" runat="server" ControlToValidate="txtCommonLRNo" SetFocusOnError="true"
                            InitialValue="" Text="*" ErrorMessage="Please Enter LR No." ValidationGroup="Required"></asp:RequiredFieldValidator>--%>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCommonLRNo" runat="server" MaxLength="50"></asp:TextBox>
                                </td>
                                <td>LR Date
                        <cc1:MaskedEditExtender ID="MECommonLRDate" TargetControlID="txtCommonLRDate" Mask="99/99/9999" MessageValidatorTip="true"
                            MaskType="Date" AutoComplete="false" runat="server">
                        </cc1:MaskedEditExtender>
                                    <cc1:MaskedEditValidator ID="MEValCommonLRDate" ControlExtender="MECommonLRDate" ControlToValidate="txtCommonLRDate" IsValidEmpty="true"
                                        EmptyValueBlurredText="*" EmptyValueMessage="Please Enter LR Date"
                                        InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="LR Date is invalid" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date"
                                        MinimumValue="01/01/2015" SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCommonLRDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                                    <asp:Image ID="imgCommonLRdate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>LR Attachment
                                </td>
                                <td>
                                    <asp:FileUpload ID="fuCommonLRUpload" runat="server" />
                                    <asp:HiddenField ID="hdnCommonUploadPath" runat="server" />
                                    <asp:HiddenField ID="hdnCommonLRPath" runat="server" />
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                        </asp:Panel>
                    </table>

                    <fieldset>
                        <legend>Job Balance Detail</legend>
                        <asp:GridView ID="gvBalance" runat="server" AutoGenerateColumns="False" CssClass="table"
                            Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="JobId"
                            DataSourceID="DataSourceBalance" CellPadding="4" AllowPaging="True" AllowSorting="True">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Job No" DataField="JobRefNo" />
                                <asp:BoundField HeaderText="Customer" DataField="Customer" />
                                <asp:BoundField HeaderText="Packages" DataField="Packages" />
                                <asp:BoundField HeaderText="Balance" DataField="BalancePackages" />
                            </Columns>
                        </asp:GridView>
                    </fieldset>
                </asp:WizardStep>
                <asp:WizardStep ID="wzFinish" StepType="Finish" Title="Update">
                    <fieldset>
                        <legend>Dispatch Review & Save</legend>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>Vehicle Number
                                </td>
                                <td>
                                    <asp:Label ID="lblVehicleNo" runat="server"></asp:Label>
                                </td>
                                <td>Vehicle Type
                                </td>
                                <td>
                                    <asp:Label ID="lblVehicleType" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Transporter Name
                                </td>
                                <td>
                                    <asp:Label ID="lblTransporterName" runat="server"></asp:Label>
                                </td>

                                <td>Dispatch Date
                                </td>
                                <td>
                                    <asp:Label ID="lblDispatchDate" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <div class="m"></div>
                        <asp:GridView ID="gvJobReview" runat="server" AutoGenerateColumns="False" CssClass="table"
                            Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr"
                            CellPadding="4" DataSourceID="DataSourcePreConsolidate">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Job No" DataField="JobRefNo" />
                                <asp:BoundField HeaderText="Customer" DataField="Customer" />
                                <asp:BoundField HeaderText="Package" DataField="DeliveredPackages" />
                                <asp:BoundField HeaderText="Balance" DataField="BalancePackages" />
                            </Columns>
                        </asp:GridView>
                    </fieldset>
                </asp:WizardStep>
                <asp:WizardStep ID="wzComplete" StepType="Complete" Title="Close" AllowReturn="false">
                    <fieldset>
                        <legend>Status</legend>
                        <div class="success" align="center" style="width: 600px; text-align: center;">
                            <asp:Label ID="lblSuccessMessage" Text="Dispatch details updated successfully!" runat="server"></asp:Label>
                        </div>
                        <div class="m" style="float: left;">
                            <asp:Button ID="btnClose" runat="server" Text="Close" CausesValidation="false" OnClick="btnCancel_OnClick" />
                        </div>
                    </fieldset>
                </asp:WizardStep>
            </WizardSteps>
            <HeaderTemplate>
                <ul id="wizHeader">
                    <asp:Repeater ID="SideBarList" runat="server">
                        <ItemTemplate>
                            <li><a class="<%# GetClassForWizardStep(Container.DataItem) %>" title="<%#Eval("Name")%>">
                                <%# Eval("Name")%></a> </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </HeaderTemplate>
            <StartNavigationTemplate>
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" OnClick="btnCancel_OnClick" />
                &nbsp;&nbsp;<asp:Button ID="btnNext" runat="server" CommandName="MoveNext" Text="Next" ValidationGroup="Required" />
            </StartNavigationTemplate>
            <StepNavigationTemplate>
                <asp:Button ID="btnStepPrevious" runat="server" Text="Previous" CommandName="MovePrevious" CausesValidation="false" />
                &nbsp;&nbsp;<asp:Button ID="btnStepNext" runat="server" CommandName="MoveNext" Text="Next" CausesValidation="true" ValidationGroup="RequiredJob" />
            </StepNavigationTemplate>
            <FinishNavigationTemplate>
                <asp:Button ID="Button1" runat="server" Text="Cancel" CausesValidation="false"
                    OnClientClick="return confirm('Are you sure you want to cancel');" OnClick="btnCancel_OnClick" />
                <asp:Button ID="btnFinPrevious" runat="server" Text="Previous" CommandName="MovePrevious" CausesValidation="false" />
                &nbsp;&nbsp;<asp:Button ID="btnFinish" runat="server" Text="Save Detail" CausesValidation="true" CommandName="MoveComplete" />
            </FinishNavigationTemplate>
        </asp:Wizard>
    </fieldset>
    <div id="divDatasource">
        <asp:SqlDataSource ID="DataSourceBalance" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="GetConsolidateBalance" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="JobIDList" SessionField="ConsolidateJob" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourcePreConsolidate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="GetBalanceByConsolidateId" SelectCommandType="StoredProcedure" EnableCaching="false">
            <SelectParameters>
                <asp:SessionParameter Name="ConsolidateId" SessionField="ConsolidateId" />
                <asp:SessionParameter Name="JobIDList" SessionField="ConsolidateJob" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>

    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
