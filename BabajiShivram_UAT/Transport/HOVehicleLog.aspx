<%@ Page Title="Vehicle Travel Log" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="HOVehicleLog.aspx.cs" Inherits="Transport_HOVehicleLog" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="content2" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upExpense" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <div style="overflow: auto;">
        <fieldset>
            <legend>Vehicle Travel Log</legend>
            <asp:UpdatePanel ID="upExpense" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>
                    <div style="text-align: center;">
                        <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                    </div>
                    <div class="clear">
                    </div>
                    <div>
                        <asp:Button ID="btnSubmit" runat="Server" Text="Save Daily Status" OnClick="btnSubmit_Click" />
                        <asp:Button ID="btnCancel" runat="Server" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false" />
                        <b>Travel Date</b> &nbsp;<asp:TextBox ID="txtStatusDate" AutoPostBack="true" runat="server" MaxLength="100" Width="80px"
                            OnTextChanged="txtStatusDate_TextChanged"></asp:TextBox>
                        <AjaxToolkit:CalendarExtender ID="calStatusDate" runat="server" EnableViewState="False"
                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtStatusDate"></AjaxToolkit:CalendarExtender>
                        &nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkModelPopup" runat="server" Text="Additional Trip Details" />
                    </div>

                    <div>
                        <asp:GridView ID="GridViewExpense" runat="server" AutoGenerateColumns="false" DataKeyNames="VehicleId,TravelLogId" CssClass="table"
                            AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" PageSize="200" AllowPaging="False" AllowSorting="True"
                            AutoGenerateEditButton="false" DataSourceID="SqlDataSourceLog">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" ReadOnly="true" SortExpression="VehicleNo" />
                                <asp:BoundField DataField="VehicleModel" HeaderText="Model" ReadOnly="true" SortExpression="VehicleModel" />
                                <asp:TemplateField HeaderText="Driver" SortExpression="DriverName">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDriver" Text='<%#BIND("DriverName") %>' runat="server" placeholder="Driver" Width="100px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Employee/Guest" SortExpression="EmployeeName">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtEmployee" Text='<%#BIND("EmployeeName") %>' runat="server" placeholder="Employee" Width="100px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Location" SortExpression="LocationTo">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtLocation" Text='<%#BIND("LocationTo") %>' runat="server" placeholder="Location" Width="100px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Out Time">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtOutTime" Text='<%#BIND("OutTime") %>' runat="server" placeholder="Out Time" Width="65px"></asp:TextBox>
                                        <AjaxToolkit:MaskedEditExtender ID="meeStartTime" runat="server" AcceptAMPM="true" MaskType="Time"
                                            Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                                            ErrorTooltipEnabled="true" UserTimeFormat="None" TargetControlID="txtOutTime"
                                            InputDirection="LeftToRight" AcceptNegative="None"></AjaxToolkit:MaskedEditExtender>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="In Time">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtInTime" Text='<%#BIND("InTime") %>' runat="server" placeholder="In Time" Width="65px"></asp:TextBox>
                                        <AjaxToolkit:MaskedEditExtender ID="meeEndTime" runat="server" AcceptAMPM="true" MaskType="Time"
                                            Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                                            ErrorTooltipEnabled="true" UserTimeFormat="None" TargetControlID="txtInTime" 
                                            InputDirection="LeftToRight" AcceptNegative="Left"></AjaxToolkit:MaskedEditExtender>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Open Reading" SortExpression="OpenReading">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtOpenReading" Text='<%#BIND("OpenReading") %>' runat="server" placeholder="Opening Reading" Width="80px"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegExValOpen" ControlToValidate="txtOpenReading" runat="server" ValidationGroup="Required"
                                         SetFocusOnError="true" Display="Dynamic"  ErrorMessage="Only Numbers allowed" ValidationExpression="\d+">
                                        </asp:RegularExpressionValidator>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Close Reading" SortExpression="CloseReading">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtCloseReading" Text='<%#BIND("CloseReading") %>' runat="server" placeholder="Closing Reading" Width="80px"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegExValClose" ControlToValidate="txtCloseReading" runat="server" ValidationGroup="Required"
                                          SetFocusOnError="true" Display="Dynamic"  ErrorMessage="Only Numbers allowed" ValidationExpression="\d+">
                                        </asp:RegularExpressionValidator>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fuel (L)" SortExpression="FuelLiter">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtFuel" Text='<%#BIND("FuelLiter") %>' runat="server" placeholder="Fuel" Width="50px"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegxWeight" runat="server" ControlToValidate="txtFuel"
                                            SetFocusOnError="true" ErrorMessage="Invalid Fuel Liter." Display="Dynamic"
                                            ValidationExpression="^[0-9]\d{0,13}(\.\d{1,3})?$"></asp:RegularExpressionValidator>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount(Rs)" SortExpression="FuelAmount">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtAmount" Text='<%#BIND("FuelAmount") %>' Width="50px" runat="server" placeholder="Amount"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegxAmount" runat="server" ControlToValidate="txtAmount"
                                            SetFocusOnError="true" ErrorMessage="Invalid Amount." Display="Dynamic"
                                            ValidationExpression="^[0-9]\d{0,13}(\.\d{1,3})?$"></asp:RegularExpressionValidator>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Type">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddFuelType" runat="server" SelectedValue='<%#Bind("FuelType") %>' Width="80px">
                                            <asp:ListItem Text="CNG" Value="CNG"></asp:ListItem>
                                            <asp:ListItem Text="PETROL" Value="PETROL"></asp:ListItem>
                                            <asp:ListItem Text="DISEL" Value="DISEL"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div>
                        <asp:SqlDataSource ID="SqlDataSourceLog" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="TR_GetVehicleDailyLog" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:ControlParameter Name="ReportDate" ControlID="txtStatusDate" PropertyName="Text" Type="datetime" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>

                    <!--Additional Trip Detail Start -->
                    <div id="divTrip">
                        <AjaxToolkit:ModalPopupExtender ID="ModalPopupNewTrip" runat="server" TargetControlID="lnkModelPopup"
                            DropShadow="False" PopupControlID="Panel21" CacheDynamicResults="false">
                        </AjaxToolkit:ModalPopupExtender>
                        <asp:Panel ID="Panel21" CssClass="ModalPopupPanel" runat="server">
                            <div class="header">
                                <div class="fleft">
                                    Add Additional Trip Detail
                                </div>
                                <div class="fright">
                                    <asp:ImageButton ID="imgClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="btnCancelPopup_Click"
                                        ToolTip="Close" />
                                </div>
                            </div>

                            <div class="m"></div>
                            <div id="DivABC" runat="server" style="max-height: 500px; overflow: auto;">
                                <asp:Label ID="lblTripMessage" runat="server"></asp:Label>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                    <tr>
                                        <td>
                                            Vehicle No
                                            <asp:RequiredFieldValidator ID="RFVNewVehicleNo" runat="server" ControlToValidate="ddNewVehicleNo"
                                                ValidationGroup="ValAdditional" InitialValue="0" SetFocusOnError="true" ErrorMessage="Required" Display="Dynamic">
                                            </asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddNewVehicleNo" runat="server" DataSourceID="SqlDataPopupVehicle" DataTextField="VehicleName"
                                               DataValueField="lid" AppendDataBoundItems="true">
                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            Driver
                                            <asp:RequiredFieldValidator ID="RFVNewDriver" runat="server" ControlToValidate="txtNewDriverName"
                                                ValidationGroup="ValAdditional" InitialValue="" SetFocusOnError="true" ErrorMessage="Required" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNewDriverName" runat="server" MaxLength="100"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Employee/Guest
                                            <asp:RequiredFieldValidator ID="RFVNewEmployee" runat="server" ControlToValidate="txtNewEmployee"
                                            ValidationGroup="ValAdditional" InitialValue="" SetFocusOnError="true" ErrorMessage="Required" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNewEmployee" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            Location
                                            <asp:RequiredFieldValidator ID="RFVNewLocation" runat="server" ControlToValidate="txtNewLocation"
                                            ValidationGroup="ValAdditional" InitialValue="" SetFocusOnError="true" ErrorMessage="Required" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNewLocation" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Open Reading
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNewOpenReading" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            Close Reading
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNewCloseReading" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Out Time
                                        <asp:RequiredFieldValidator ID="RFVOutTime" runat="server" ControlToValidate="txtNewOutTime"
                                            ValidationGroup="ValAdditional" InitialValue="" SetFocusOnError="true" ErrorMessage="Required" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNewOutTime" runat="server"></asp:TextBox>
                                            <AjaxToolkit:MaskedEditExtender ID="meeNewStartTime" runat="server" AcceptAMPM="true" MaskType="Time"
                                                Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                                                ErrorTooltipEnabled="true" UserTimeFormat="None" TargetControlID="txtNewOutTime"
                                                InputDirection="LeftToRight" AcceptNegative="None"></AjaxToolkit:MaskedEditExtender>
                                        </td>
                                        <td>
                                            IN Time
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNewInTime" runat="server"></asp:TextBox>
                                            <AjaxToolkit:MaskedEditExtender ID="meeEndTime" runat="server" AcceptAMPM="true" MaskType="Time"
                                                Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                                                ErrorTooltipEnabled="true" UserTimeFormat="None" TargetControlID="txtNewInTime"
                                                InputDirection="LeftToRight" AcceptNegative="Left"></AjaxToolkit:MaskedEditExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Fuel (L)
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNewFuel" runat="server"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegxWeight" runat="server" ControlToValidate="txtNewFuel"
                                                SetFocusOnError="true" ErrorMessage="Invalid Fuel Liter." Display="Dynamic"
                                                ValidationExpression="^[0-9]\d{0,13}(\.\d{1,3})?$"></asp:RegularExpressionValidator>
                                        </td>
                                        <td>
                                            Amount(Rs)
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNewAmount" runat="server"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegxAmount" runat="server" ControlToValidate="txtNewAmount"
                                                SetFocusOnError="true" ErrorMessage="Invalid Amount." Display="Dynamic"
                                                ValidationExpression="^[0-9]\d{0,13}(\.\d{1,3})?$"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Fuel Type
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddNewFuelType" runat="server" Width="120px">
                                                <asp:ListItem Text="CNG" Value="CNG"></asp:ListItem>
                                                <asp:ListItem Text="PETROL" Value="PETROL"></asp:ListItem>
                                                <asp:ListItem Text="DISEL" Value="DISEL"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnSaveAdditional" Text="Add Trip Detail" runat="server" OnClick="btnSaveAdditional_Click"
                                                ValidationGroup="ValAdditional" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnCancelPopup" runat="server" OnClick="btnCancelPopup_Click"
                                                Text="Close" CausesValidation="false" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                        <asp:SqlDataSource ID="SqlDataPopupVehicle" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="TR_GetEquipmentByType" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:Parameter Name="lType" DefaultValue="20" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
    </div>
</asp:Content>
