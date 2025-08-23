<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VehicleDetail.aspx.cs" Inherits="Transport_VehicleDetail"
    MasterPageFile="~/MasterPage.master" Title="Vehicle Details" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <div align="center">
        <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" />
    </div>
    <div>
        <cc1:CalendarExtender ID="calDispatchDate" runat="server" Enabled="True" EnableViewState="False"
            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDisatchDate"
            PopupPosition="BottomRight" TargetControlID="txtDispatchDate">
        </cc1:CalendarExtender>
        <cc1:CalendarExtender ID="calDeliveredDate" runat="server" Enabled="True" EnableViewState="False"
            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDeliveryDate"
            PopupPosition="BottomRight" TargetControlID="txtDeliveryDate">
        </cc1:CalendarExtender>
    </div>
    <div>
        <fieldset>
            <legend>Transport Request Detail</legend>
            <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                <tr>
                    <td>Ref No.
                    </td>
                    <td>
                        <asp:Label ID="lblTRRefNo" runat="server"></asp:Label>
                    </td>
                    <td>Truck Request Date
                    </td>
                    <td>
                        <asp:Label ID="lblVehRequestDate" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Vehicle Place Require Date
                    </td>
                    <td>
                        <asp:Label ID="lblVehiclePlaceDate" runat="server"></asp:Label>
                    </td>
                    <td>Customer Name
                    </td>
                    <td>
                        <asp:Label ID="lblCustName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Job No.
                    </td>
                    <td>
                        <asp:Label ID="lblJobNo" runat="server"></asp:Label>
                    </td>
                    <td>Location
                    </td>
                    <td>
                        <asp:Label ID="lblLocationFrom" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Destination
                    </td>
                    <td>
                        <asp:Label ID="lblDestination" runat="server"></asp:Label>
                    </td>
                    <td>Packages    
                    </td>
                    <td>
                        <asp:Label ID="lblNoOfPackages" runat="server"></asp:Label>
                        <%--&nbsp;<asp:Label ID="lblPackageType" runat="server"></asp:Label>--%>
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
                        <asp:Label ID="lblDeliveryType" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:LinkButton ID="lnkViewDocument" runat="server" Text="View Document" OnClick="lnkViewDocument_Click"></asp:LinkButton>
                    </td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
        </fieldset>
        <div id="divPopupDocument">
            <cc1:ModalPopupExtender ID="ModalPopupDocument" runat="server" CacheDynamicResults="false"
                DropShadow="False" PopupControlID="Panel2Summary" TargetControlID="lnkViewDocument">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="Panel2Summary" runat="server" CssClass="ModalPopupPanel">
                <div class="header">
                    <div class="fleft">
                        &nbsp;<asp:Button ID="btnCancelPopup" runat="server" OnClick="btnCancelPopup_Click" Text="Close" CausesValidation="false" />
                    </div>
                    <div class="fleft">
                        &nbsp;<asp:Label ID="lblSummary" Text="Document Detail" runat="server" />
                    </div>
                    <div class="fright">
                        <asp:ImageButton ID="imgCancelPopup" ImageUrl="~/Images/delete.gif" OnClick="btnCancelPopup_Click" runat="server" ToolTip="Close" />
                    </div>
                </div>
                <!--Document Detail Start-->
                <div id="Div3" runat="server" style="max-height: 550px; max-width: 900px; overflow: auto;">
                    <asp:GridView ID="gvDocumentDetail" runat="server" CssClass="table" Width="98%" AutoGenerateColumns="false"
                        DataKeyNames="lId" DataSourceID="DataSourceDocument" AllowPaging="false" PageSize="20" PagerStyle-CssClass="pgr"
                        OnRowCommand="gvDocumentDetail_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DocumentName" HeaderText="Document" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDownliad" CommandName="download" CommandArgument='<%#Eval("lID") %>' Text="Download" runat="server"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>
            <div>
                <asp:HiddenField ID="hdnJobID" runat="server" />
                <asp:SqlDataSource ID="DataSourceDocument" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetUploadedDocument" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:Parameter Name="JobId" DefaultValue="0" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </div>
    </div>
    <fieldset>
        <legend>Rate Detail</legend>
        <div>
            <asp:GridView ID="GridViewVehicle" runat="server" AutoGenerateColumns="False" CssClass="table"
                Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                DataSourceID="DataSourceRate" CellPadding="4" AllowPaging="True" AllowSorting="True"
                PageSize="20" OnRowCommand="GridViewVehicle_RowCommand" OnRowDataBound="GridViewVehicle_RowDataBound"
                OnRowEditing="GridViewVehicle_RowEditing" OnRowUpdating="GridViewVehicle_RowUpdating" OnRowCancelingEdit="GridViewVehicle_RowCancelingEdit">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" CommandName="Edit" runat="server" Text="Add Rate"
                                ToolTip="Click To Add Transport Unit Rate"></asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update Transport Detail" runat="server"
                                Text="Update" ValidationGroup="RateRequired"></asp:LinkButton>
                            <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel Rate Update" CausesValidation="false"
                                runat="server" Text="Cancel"></asp:LinkButton>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Unit Rate">
                        <ItemTemplate>
                            <asp:Label ID="txtRate" runat="server" Text='<%#BIND("TransportAmount") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtRate" runat="server" Text='<%#BIND("TransportAmount") %>' MaxLength="10" Width="100px"></asp:TextBox>
                            <asp:CompareValidator ID="CompValRate" runat="server" ControlToValidate="txtRate" Operator="DataTypeCheck" SetFocusOnError="true"
                                Type="Integer" Text="Invalid Rate" ErrorMessage="Invalid Transport Rate" Display="Dynamic" ValidationGroup="RateRequired"></asp:CompareValidator>
                            <asp:RequiredFieldValidator ID="RFVRate" runat="server" ControlToValidate="txtRate" SetFocusOnError="true"
                                Text="*" ErrorMessage="Please Enter Transport Rate" Display="Dynamic" ValidationGroup="RateRequired"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Expenses" ItemStyle-Width="20px">
                        <ItemTemplate>
                            <asp:LinkButton ID="imgbtnAddExpenses" runat="server" Text="Add" CommandName="Expenses"
                                CommandArgument='<%#Eval("lid")%>' ToolTip="Add expenses for vehicle."></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Approved Rate" DataField="ApprovedAmount" ReadOnly="true" Visible="false" />
                    <asp:BoundField HeaderText="Pkgs" DataField="Packages" ReadOnly="true" />
                    <%--<asp:BoundField HeaderText="Vehicle No" DataField="VehicleNo" ReadOnly="true" />--%>
                    <asp:TemplateField HeaderText="Vehicle No">
                        <ItemTemplate>
                            <asp:Label ID="lblVehicleNo" runat="server" Text='<%#BIND("VehicleNo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Type" DataField="VehicleType" ReadOnly="true" />
                    <asp:BoundField HeaderText="Delivery From" DataField="DeliveryFrom" ReadOnly="true" />
                    <asp:BoundField HeaderText="Delivery Point" DataField="DeliveryTo" ReadOnly="true" />
                    <asp:BoundField HeaderText="Transporter" DataField="Transporter" ReadOnly="true" />
                    <asp:BoundField HeaderText="Dispatch Date" DataField="DispatchDate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                    <%--<asp:BoundField HeaderText="Status" DataField="UserName" />--%>
                    <%--<asp:BoundField HeaderText="User" DataField="UserName" />--%>
                </Columns>
            </asp:GridView>
        </div>
    </fieldset>
    <fieldset>
        <legend>Add Transporter</legend>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
            <th style="color: #FFFFFF; background-color: #5A7FA5;">Transporter</th>
            <th style="color: #FFFFFF; background-color: #5A7FA5">ADD</th>
            <th style="color: #FFFFFF; background-color: #5A7FA5">Placed</th>
            <th style="color: #FFFFFF; background-color: #5A7FA5">Remove</th>
            <tr>
                <td>
                    <asp:ListBox ID="lstTransport" SelectionMode="Multiple" runat="server" Height="200px" Width="70%"></asp:ListBox>
                </td>
                <td>
                    <asp:Button ID="btnADDTransport" Text="Add Transporter" runat="server" OnClick="btnADDTransport_Click" />
                </td>
                <td class="label">
                    <asp:ListBox ID="lstPlaced" SelectionMode="Multiple" runat="server" Height="200px" Width="70%"
                        DataSourceID="DSTransporterPlaced" DataTextField="TransporterName" DataValueField="TransporterId"></asp:ListBox>
                    <%--<asp:BulletedList ID="blTransporter" DataSourceID="DSTransporterPlaced" DataTextField="TransporterName"
                    DataValueField="TransporterId" runat="server" DisplayMode="Text" CssClass="ulList">
                </asp:BulletedList>--%>
                </td>
                <td>
                    <asp:Button ID="btnRemoveTransport" Text="Remove Transporter" runat="server" OnClick="btnRemoveTransport_Click" />

                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="DSTransporterPlaced" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_getTransporterPlaced" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransRequestId" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
    </fieldset>
    <fieldset id="flNewVehicle" runat="server" visible="false">
        <legend>Vehicle Detail</legend>
        <asp:Button ID="btnAddVehicle" runat="server" Text="Add Vehicle" OnClick="btnAddVehicle_Click" TabIndex="12" />
        <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
            <tr>
                <td>No of Packages
                    <asp:CompareValidator ID="CompValPackages" runat="server" ControlToValidate="txtNoOfPackages" Operator="DataTypeCheck" SetFocusOnError="true"
                        Type="Integer" Text="Invalid Packages" ErrorMessage="Invalid No Of Packages" Display="Dynamic" ValidationGroup="Required"></asp:CompareValidator>
                    <asp:RequiredFieldValidator ID="RFVNoOfPkgs" runat="server" ControlToValidate="txtNoOfPackages" SetFocusOnError="true"
                        Text="*" ErrorMessage="Please Enter No Of Packages" Display="Dynamic" ValidationGroup="VehicleRequired"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtNoOfPackages" MaxLength="8" runat="server" Width="100px" TabIndex="1" type="number"></asp:TextBox>
                </td>
                <td>Container
                </td>
                <td>20"
                    <asp:TextBox ID="txtCount20" MaxLength="8" runat="server" Width="50px" TabIndex="2" type="number"></asp:TextBox>

                    <asp:TextBox ID="txtCount40" MaxLength="8" runat="server" Width="50px" TabIndex="3" type="number"></asp:TextBox>
                    40"
                </td>
            </tr>
            <tr>
                <td>Vehicle No & Type
                    <asp:RequiredFieldValidator ID="RFVVehicleNo" runat="server" ControlToValidate="txtVehicleNo" SetFocusOnError="true"
                        Text="*" ErrorMessage="Please Enter Vehicle No." Display="Dynamic" ValidationGroup="VehicleRequired"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtVehicleNo" runat="server" MaxLength="50" Width="100px" TabIndex="4"></asp:TextBox>&nbsp;
                    <asp:DropDownList ID="ddVehicleType" runat="server" Width="100px" TabIndex="5"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RFVVehicleType" runat="server" ControlToValidate="ddVehicleType" SetFocusOnError="true"
                        Text="*" InitialValue="0" ErrorMessage="Please Select Vehicle Type" Display="Dynamic" ValidationGroup="VehicleRequired"></asp:RequiredFieldValidator>
                </td>
                <td>Transporter Name
                </td>
                <td>
                    <asp:DropDownList ID="ddTransporter" runat="server" TabIndex="6"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RFVTrasnporterPlaced" runat="server" ControlToValidate="ddTransporter" SetFocusOnError="true"
                        Text="*" InitialValue="0" ErrorMessage="Please Select Transporter Name" Display="Dynamic" ValidationGroup="VehicleRequired"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>Location From
                </td>
                <td>
                    <asp:TextBox ID="txtDeliveryFrom" runat="server" MaxLength="100" TabIndex="7"></asp:TextBox>
                </td>
                <td>Destination
                </td>
                <td>
                    <asp:TextBox ID="txtDestination" runat="server" MaxLength="100" TabIndex="8"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Dispatch Date
                    <cc1:MaskedEditExtender ID="MEditDispatchDate" TargetControlID="txtDispatchDate" Mask="99/99/9999" MessageValidatorTip="true"
                        MaskType="Date" AutoComplete="false" runat="server">
                    </cc1:MaskedEditExtender>
                    <cc1:MaskedEditValidator ID="MEditValDispatchDate" ControlExtender="MEditDispatchDate" ControlToValidate="txtDispatchDate" IsValidEmpty="false"
                        EmptyValueBlurredText="*" EmptyValueMessage="Please Enter Dispatch Date" InvalidValueBlurredMessage="Invalid Date"
                        InvalidValueMessage="Dispatch Date is invalid" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date"
                        MinimumValue="01/01/2015" SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtDispatchDate" runat="server" Width="100px" TabIndex="9" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:Image ID="imgDisatchDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                        runat="server" />
                </td>
                <td>Delivered Date
                    <cc1:MaskedEditExtender ID="MEditDeliveredDate" TargetControlID="txtDeliveryDate" Mask="99/99/9999" MessageValidatorTip="true"
                        MaskType="Date" AutoComplete="false" runat="server">
                    </cc1:MaskedEditExtender>
                    <cc1:MaskedEditValidator ID="MEditValDeliveredDate" ControlExtender="MEditDeliveredDate" ControlToValidate="txtDeliveryDate" IsValidEmpty="true"
                        InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Cargo Delivered Date is invalid" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date"
                        MinimumValue="01/01/2015" SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtDeliveryDate" runat="server" Width="100px" TabIndex="10" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:Image ID="imgDeliveryDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                </td>
            </tr>
        </table>
    </fieldset>
    <div>
        <asp:SqlDataSource ID="DataSourceRate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetTransDeliveryDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    <%-- START  : Pop-up For Expenses --%>
    <div>
        <asp:HiddenField ID="hdnExpenses" runat="server" Value="0" />
        <cc1:ModalPopupExtender ID="mpeExpenses" runat="server" TargetControlID="hdnExpenses" BackgroundCssClass="modalBackground" CancelControlID="imgClosePopup"
            PopupControlID="pnlExpenses" DropShadow="true">
        </cc1:ModalPopupExtender>
        <asp:Panel ID="pnlExpenses" runat="server" CssClass="modalPopup1" BackColor="#F5F5DC" Width="810px" Height="290px" BorderStyle="Solid" BorderWidth="1px" Style="border-radius: 4px">
            <div id="div1" runat="server">
                <br />
                <table width="100%">
                    <tr>
                        <td align="center"><b><u>Budgetary Expense Detail</u></b>
                            <span style="float: right">
                                <asp:ImageButton ID="imgClosePopup" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgClosePopup_Click" ToolTip="Close" />
                            </span>
                        </td>
                    </tr>
                </table>
                <div>
                    <asp:Panel ID="pnlExpenses2" runat="server" Width="800px" Height="300px" ScrollBars="Auto">
                        <div class="m clear" align="center">
                            <asp:Label ID="lblError_Popup" runat="server"></asp:Label>
                            <asp:HiddenField ID="hdnRateDetailId" runat="server" Value="0" />
                        </div>
                        <table border="0" cellpadding="0" cellspacing="0" width="95%" bgcolor="white" style="margin-left: 10px; border: 1px solid #293452;">
                            <tr>
                                <td>Vehicle No
                                </td>
                                <td>
                                    <asp:Label ID="lblVehicleNo" runat="server"></asp:Label>
                                </td>
                                <td>Vehicle Type</td>
                                <td>
                                    <asp:Label ID="lblVehicleType" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Delivery From
                                </td>
                                <td>
                                    <asp:Label ID="lblDeliveryFrom" runat="server"></asp:Label>
                                </td>
                                <td>Delivery To
                                </td>
                                <td>
                                    <asp:Label ID="lblDeliveryTo" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Transporter
                                </td>
                                <td>
                                    <asp:Label ID="lblTransporter" runat="server"></asp:Label>
                                </td>
                                <td>Dispatch Date
                                </td>
                                <td>
                                    <asp:Label ID="lblDispatchDate" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table border="0" cellpadding="0" cellspacing="0" width="95%" bgcolor="white" style="margin-left: 10px; border: 1px solid #293452;">
                            <tr>
                                <td>Varai Expenses
                                </td>
                                <td>
                                    <asp:TextBox ID="txtVaraiAmount" runat="server" Width="100px" TabIndex="1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Detention Amount
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDetentionAmount" runat="server" Width="100px" TabIndex="2"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Empty Cont Rcpt Charges
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmptyContCharges" runat="server" Width="100px" TabIndex="3"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <div id="DivSendEmail" runat="server" style="max-height: 100px; margin-left: 10px">
                            <asp:Button ID="btnSaveExpenses" runat="server" OnClick="btnSaveExpenses_Click" Text="Save Expense" />
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </asp:Panel>
    </div>
    <%-- END    : Pop-up For Expenses --%>
</asp:Content>
