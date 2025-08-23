<%@ Page Title="" Language="C#" MasterPageFile="~/TransportMaster.master" AutoEventWireup="true"
    CodeFile="BillingVehicle.aspx.cs" Inherits="BillingTransport_BillingVehicle"
    Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <style type="text/css">
        input[type="text" i]:disabled
        {
            background-color: rgba(211, 211, 211, 0.72);
            color: black;
        }
        .fleft
        {
            margin-left: 2px;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function Validate() {
            var NFormDoc = document.getElementById('ctl00_ContentPlaceHolder1_fuNformDoc');
            var nFormNo = document.getElementById('<%= hdnNFormNo.ClientID%>');
            if (nFormNo.value != '' && nFormNo.value == '1') {
                if (NFormDoc.value == null || NFormDoc.value == '') {
                    alert('Please browse N Form Document.');
                    document.getElementById('ctl00_ContentPlaceHolder1_fuNformDoc').focus();
                    return false;
                }
            }

            var txtDetentionDay = document.getElementById('ctl00_ContentPlaceHolder1_txtDetentionDay');
            if (txtDetentionDay.value != null && txtDetentionDay.value != '') {
                if (txtDetentionDay.value > '0') {
                    var txtDetentionCharges = document.getElementById('ctl00_ContentPlaceHolder1_txtDetentionCharges');
                    if (txtDetentionCharges.value == '') {
                        alert('Please Enter Detention Charges.');
                        document.getElementById('ctl00_ContentPlaceHolder1_txtDetentionCharges').focus();
                        return false;
                    }
                }
            }

            var txtDetentionCharges = document.getElementById('ctl00_ContentPlaceHolder1_txtDetentionCharges');
            if (txtDetentionCharges.value != null && txtDetentionCharges.value != '') {
                if (txtDetentionDay.value == '' || txtDetentionDay.value == '0') {
                    alert('Please Enter Detention Day.');
                    document.getElementById('ctl00_ContentPlaceHolder1_txtDetentionDay').focus();
                    return false;
                }
            }

            if (typeof (Page_ClientValidate) == 'function') {
                Page_ClientValidate('Required');
            }
        }

        function ValidateDetention() {
            var txtDetentionDay = document.getElementById('ctl00_ContentPlaceHolder1_txtDetentionDay');
            if (txtDetentionDay.value != null && txtDetentionDay.value != '') {
                if (txtDetentionDay.value > '0') {
                    var txtDetentionCharges = document.getElementById('ctl00_ContentPlaceHolder1_txtDetentionCharges');
                    if (txtDetentionCharges.value == '') {
                        alert('Please Enter Detention Charges.');
                        document.getElementById('ctl00_ContentPlaceHolder1_txtDetentionCharges').focus();
                        return false;
                    }
                }
            }

            var txtDetentionCharges = document.getElementById('ctl00_ContentPlaceHolder1_txtDetentionCharges');
            if (txtDetentionCharges.value != null && txtDetentionCharges.value != '') {
                if (txtDetentionDay.value == '' || txtDetentionDay.value == '0') {
                    alert('Please Enter Detention Day.');
                    document.getElementById('ctl00_ContentPlaceHolder1_txtDetentionDay').focus();
                    return false;
                }
            }
        }
    </script>
    <div align="center">
        <asp:Label ID="lberror" Text="" runat="server" EnableViewState="false"></asp:Label>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="Required" />
        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="vgJobNo" />
        <div>
            <asp:Label ID="lblDeliveryLid" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="lblTotal" runat="server" Visible="false"></asp:Label>
            <cc1:CalendarExtender ID="calDeliveredDate" runat="server" Enabled="True" EnableViewState="False"
                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDeliveryDate" PopupPosition="BottomRight"
                TargetControlID="txtDeliveryDate">
            </cc1:CalendarExtender>
            <cc1:CalendarExtender ID="calReportDt" runat="server" Enabled="True" EnableViewState="False"
                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgReportDt" PopupPosition="BottomRight"
                TargetControlID="txtReportDate">
            </cc1:CalendarExtender>
            <cc1:CalendarExtender ID="calUnloadDt" runat="server" Enabled="True" EnableViewState="False"
                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgUnloadDt" PopupPosition="BottomRight"
                TargetControlID="txtUnloadDate">
            </cc1:CalendarExtender>
            <cc1:CalendarExtender ID="calEmptyReturnDt" runat="server" Enabled="True" EnableViewState="False"
                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgEmptyReturnDt"
                PopupPosition="BottomRight" TargetControlID="txtEmptyReturnDate">
            </cc1:CalendarExtender>
           
        </div>
    </div>
    <fieldset style="min-height: 380px; margin-top: 0px">
        <fieldset id="fsAllVehicles" runat="server" style="min-height: 100px">
            <legend>Total Vehicles</legend>
            <div>
                <asp:HiddenField ID="hdnVehicleNo" runat="server" />
                <asp:HiddenField ID="hdnJobId" runat="server" />
                <asp:HiddenField ID="hdnDeliveryLid" runat="server" />
                <asp:Button ID="btnBackToJobDet" runat="server" Text="Go Back" OnClick="btnBackToJobDet_OnClick" Visible="false" />
                <asp:GridView ID="GridViewDelivery" runat="server" AutoGenerateColumns="False" CssClass="table"
                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId,JobId,VehicleNo"
                    DataSourceID="DataSourceDelivery" OnRowCommand="GridViewDelivery_RowCommand"
                    CellPadding="4" AllowPaging="True" AllowSorting="True" PagerSettings-Position="TopAndBottom"
                    PageSize="20">
                    <Columns>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit Delivery Details" Width="22"
                                    runat="server" Text="Update" Font-Underline="true"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="BS Job No" DataField="JobRefNo" ReadOnly="true" />
                        <asp:BoundField HeaderText="Vehicle No" DataField="VehicleNo" ReadOnly="true" />
                        <asp:BoundField HeaderText="Container No" DataField="ContainerNo" ReadOnly="true" />
                        <asp:BoundField HeaderText="Packages" DataField="NoOfPackages" ReadOnly="true" />
                        <asp:BoundField HeaderText="Vehicle Type" DataField="vehiclename" ReadOnly="true" />
                        <asp:BoundField HeaderText="Destination" DataField="DeliveryPoint" ReadOnly="true" />
                        <asp:BoundField HeaderText="Dispatch Date" DataField="DispatchDate" DataFormatString="{0:dd/MM/yyyy}"
                            ReadOnly="true" />
                        <asp:BoundField HeaderText="Container Type" DataField="ContainerType" ReadOnly="true"
                            Visible="false" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="DataSourceDelivery" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetDeliveryDetailForVendor" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="JobId" SessionField="JobId" Type="Int32" />
                        <asp:SessionParameter Name="UserId" SessionField="VendorId" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </fieldset>
        <div class="fleft" style="float: none">
            <asp:Button ID="btnBackToVehicleDetail" runat="server" Text="Go Back" OnClick="btnBackToVehicleDetail_OnClick" />
        </div>
        <fieldset id="fsVehicleDetail" runat="server" style="margin-bottom: 0px">
            <legend>Vehicle Detail</legend>
            <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                Width="51%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataSourceID="DataSourceJobDetail"
                CellPadding="4" AllowPaging="True" AllowSorting="True" PagerSettings-Position="TopAndBottom"
                PageSize="5">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Job Id" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblJobId" runat="server" Text='<%#Eval("JobId") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="BS Job No">
                        <ItemTemplate>
                            <asp:Label ID="lblJobRefNo" runat="server" Text='<%#Eval("JobRefNo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Container No">
                        <ItemTemplate>
                            <asp:Label ID="lblContainerNo" runat="server" Text='<%#Eval("ContainerNo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="No of Packages">
                        <ItemTemplate>
                            <asp:Label ID="lblPkg" runat="server" Text='<%#Eval("NoOfPackages") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delievered Packages">
                        <ItemTemplate>
                            <asp:Label ID="lblDeliveredPkgs" runat="server" Text='<%#Eval("DeliveredPkgs") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="DataSourceJobDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="TR_GetConsolidateTransportJob" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:ControlParameter ControlID="lblDeliveryLid" PropertyName="Text" Name="DeliveryId"
                        Type="Int32" />
                    <asp:ControlParameter ControlID="hdnJobId" PropertyName="Value" Name="JobId" Type="Int32" />
                    <asp:SessionParameter Name="UserId" SessionField="VendorId" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
            <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                <tr>
                    <td>
                        Vehicle No
                    </td>
                    <td>
                        <asp:Label ID="lblVehicleNo" runat="server" MaxLength="50" Width="150px"></asp:Label>
                    </td>
                    <td>
                        Vehicle Type
                    </td>
                    <td>
                        <asp:Label ID="lblVehicleType" runat="server" Enabled="false" Width="150px"></asp:Label>
                        <asp:DropDownList ID="ddVehicleType" runat="server" Width="150px" Enabled="false"
                            Visible="false">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Transporter Name
                    </td>
                    <td>
                        <asp:Label ID="lblTransporterName" runat="server" Width="230px"></asp:Label>
                    </td>
                    <td>
                        Location From
                    </td>
                    <td>
                        <asp:Label ID="lblDeliveryFrom" runat="server" MaxLength="100" Width="150px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Destination
                    </td>
                    <td>
                        <asp:Label ID="lblDestination" runat="server" MaxLength="150" Width="230px"></asp:Label>
                    </td>
                    <td>
                        Dispatch Date
                    </td>
                    <td>
                        <asp:Label ID="lblDispatchDate" runat="server" Width="150px" placeholder="dd/mm/yyyy"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        LR No
                    </td>
                    <td>
                        <asp:Label ID="lblLRNo" runat="server" MaxLength="150" Width="230px"></asp:Label>
                    </td>
                    <td>
                        LR Date
                    </td>
                    <td>
                        <asp:Label ID="lblLRDate" runat="server" Width="150px" placeholder="dd/mm/yyyy"></asp:Label>
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset id="fsEditDelivery" runat="server" style="margin-bottom: 0px">
            <legend>Delivery Detail</legend>
            <asp:Button ID="btnUpdateDelivery" runat="server" Text="Update" OnClientClick="return Validate();"
                ValidationGroup="Required" OnClick="btnUpdateDelivery_Click" TabIndex="19" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                CausesValidation="false" TabIndex="20" />
            <asp:HiddenField ID="hdnNFormNo" runat="server" />
            <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                <tr>
                    <td>
                        Delivered Date
                        <cc1:MaskedEditExtender ID="MEditDeliveredDate" TargetControlID="txtDeliveryDate"
                            Mask="99/99/9999" MessageValidatorTip="true" MaskType="Date" AutoComplete="false"
                            runat="server">
                        </cc1:MaskedEditExtender>
                        <cc1:MaskedEditValidator ID="MEditValDeliveredDate" ControlExtender="MEditDeliveredDate"
                            EmptyValueBlurredText="*" EmptyValueMessage="Please Enter Delivered Date." ControlToValidate="txtDeliveryDate"
                            IsValidEmpty="false" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Cargo Delivered Date is invalid"
                            MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015"
                            SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDeliveryDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"
                            TabIndex="5"></asp:TextBox>
                        <asp:Image ID="imgDeliveryDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                            runat="server" />
                    </td>
                    <td>
                        Reporting Date
                        <cc1:MaskedEditExtender ID="meeReportDt" TargetControlID="txtReportDate" Mask="99/99/9999"
                            MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                        </cc1:MaskedEditExtender>
                        <cc1:MaskedEditValidator ID="mevReportDt" ControlExtender="meeReportDt" ControlToValidate="txtReportDate"
                            EmptyValueBlurredText="*" EmptyValueMessage="Please Enter Report Date." IsValidEmpty="false"
                            InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Report Date is invalid"
                            MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015"
                            SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtReportDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"
                            TabIndex="6"></asp:TextBox>
                        <asp:Image ID="imgReportDt" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Unloading Date
                        <cc1:MaskedEditExtender ID="meeUnloadDt" TargetControlID="txtUnloadDate" Mask="99/99/9999"
                            MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                        </cc1:MaskedEditExtender>
                        <cc1:MaskedEditValidator ID="mevUnloadDt" ControlExtender="meeUnloadDt" ControlToValidate="txtUnloadDate"
                            IsValidEmpty="false" EmptyValueBlurredText="*" EmptyValueMessage="Please Enter Unloading Date."
                            InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Unloading Date is invalid"
                            MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015"
                            SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtUnloadDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"
                            TabIndex="7"></asp:TextBox>
                        <asp:Image ID="imgUnloadDt" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                            runat="server" />
                    </td>
                    <td>
                        Detention Days
                        <asp:CompareValidator ID="cvDetentionDays" runat="server" ControlToValidate="txtDetentionDay"
                            Operator="DataTypeCheck" SetFocusOnError="true" Type="Integer" Text="*" ErrorMessage="Invalid Detention Days."
                            Display="Dynamic" ValidationGroup="Required"></asp:CompareValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDetentionDay" MaxLength="12" runat="server" Width="150px" TabIndex="8"
                            type="number"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Detention Charges
                        <asp:CompareValidator ID="cvDetentionCharges" runat="server" ControlToValidate="txtDetentionCharges"
                            Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" Text="*" ErrorMessage="Invalid Detention Charges."
                            Display="Dynamic" ValidationGroup="Required"></asp:CompareValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDetentionCharges" MaxLength="15" runat="server" Width="150px"
                            onchange="return ValidateDetention();" TabIndex="9"></asp:TextBox>
                    </td>
                    <td>
                        Warai Charges
                        <asp:CompareValidator ID="cvVaraiCharges" runat="server" ControlToValidate="txtVaraiCharges"
                            Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" Text="*" ErrorMessage="Invalid Varai Charges."
                            Display="Dynamic" ValidationGroup="Required"></asp:CompareValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtVaraiCharges" MaxLength="15" runat="server" Width="150px" TabIndex="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Empty Off Loading Charges
                        <asp:CompareValidator ID="cvEmptyLoadngChrges" runat="server" ControlToValidate="txtEmptyOffLoadingCharges"
                            Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" Text="*" ErrorMessage="Invalid Empty Off Loading Charges."
                            Display="Dynamic" ValidationGroup="Required"></asp:CompareValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmptyOffLoadingCharges" MaxLength="15" runat="server" Width="150px"
                            TabIndex="11"></asp:TextBox>
                    </td>
                    <td>
                        Tempo Union Charges
                        <asp:CompareValidator ID="cvTempoUnionChrges" runat="server" ControlToValidate="txtTempoUnionCharges"
                            Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" Text="*" ErrorMessage="Invalid Tempo Union Charges."
                            Display="Dynamic" ValidationGroup="Required"></asp:CompareValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTempoUnionCharges" MaxLength="15" runat="server" Width="150px"
                            TabIndex="12"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Transporter Freight Rate
                        <asp:CompareValidator ID="cvFrightRate" runat="server" ControlToValidate="txtFrightRate"
                            Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" Text="*" ErrorMessage="Invalid Fright Rate"
                            Display="Dynamic" ValidationGroup="Required"></asp:CompareValidator>
                        <asp:RequiredFieldValidator ID="RFVNoOfPkgs" runat="server" ControlToValidate="txtFrightRate"
                            SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Transporter Freight Rate."
                            Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFrightRate" MaxLength="12" runat="server" Width="150px" TabIndex="13"></asp:TextBox>
                    </td>
                    <td>
                        Empty Container Return Date By Transporter
                        <cc1:MaskedEditExtender ID="meeEmptyRetDt" TargetControlID="txtEmptyReturnDate" Mask="99/99/9999"
                            MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                        </cc1:MaskedEditExtender>
                        <cc1:MaskedEditValidator ID="mevEmptyRetDt" ControlExtender="meeEmptyRetDt" ControlToValidate="txtEmptyReturnDate"
                            IsValidEmpty="true" EmptyValueBlurredText="*" EmptyValueMessage="Please Enter Empty Container Return Date By Transporter"
                            InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Empty Container Return Date is invalid"
                            MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015"
                            SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmptyReturnDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"
                            TabIndex="14"></asp:TextBox>
                        <asp:Image ID="imgEmptyReturnDt" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                            runat="server" />
                    </td>
                </tr>
                
                
                <tr>
                    <td>
                        Upload LR Copies
                        <asp:RequiredFieldValidator ID="rfvLrCopies" runat="server" ControlToValidate="fuUploadLrCopies"
                            SetFocusOnError="true" Text="*" ErrorMessage="Please Upload LR Copies." Display="Dynamic"
                            ValidationGroup="Required"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:FileUpload ID="fuUploadLrCopies" runat="server" TabIndex="15" />
                    </td>
                    <td>
                        Upload Receipt
                        <asp:RequiredFieldValidator ID="rfvReceipt" runat="server" ControlToValidate="fuUploadReceipt"
                            SetFocusOnError="true" Text="*" ErrorMessage="Please Upload Receipt." Display="Dynamic"
                            ValidationGroup="Required"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:FileUpload ID="fuUploadReceipt" runat="server" TabIndex="16" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Remarks
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtRemarks" MaxLength="450" runat="server" Width="870px" TabIndex="18"
                            Rows="2" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </fieldset>
    </fieldset>
    <!--Document for Doc Upload-->
    <div id="divDocument">
        <cc1:ModalPopupExtender ID="ModalPopupDocument" runat="server" CacheDynamicResults="false"
            DropShadow="False" PopupControlID="Panel2Document" TargetControlID="lnkDummy">
        </cc1:ModalPopupExtender>
        <asp:Panel ID="Panel2Document" runat="server" CssClass="ModalPopupPanel" Width="400px">
            <div class="header">
                <div class="fleft">
                    Upload Invoice Copy
                </div>
                <div class="fright">
                    <asp:ImageButton ID="imgClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="btnCancelPopup_Click"
                        ToolTip="Close" />
                </div>
            </div>
            <div class="m">
            </div>
            <div id="Div1" runat="server" style="max-height: 200px; overflow: auto;">
                <asp:HiddenField ID="HiddenField1" runat="server" />
                <asp:HiddenField ID="hdnVehicleDeliveryId" runat="server" />
                <asp:HiddenField ID="hdnTransportBillStatus" runat="server" />
                <!-- Lists Of All Documents -->
                <div align="center">
                    <asp:Label ID="lbError_Popup" runat="server" Visible="true"></asp:Label>
                </div>
            </div>
            <!-- Add new Document -->
            <div class="m clear">
            </div>
            <div id="dvUploadNewFile" runat="server" style="max-height: 200px; overflow: auto;
                margin-left: 15px">
                <asp:FileUpload ID="fuDocument" runat="server" />
                <asp:Button ID="btnSaveDocument" Text="Save Document" runat="server" OnClick="btnSaveDocument_Click"
                    CausesValidation="false" />
            </div>
            <div class="m clear">
            </div>
            <!--Document for BIlling Advice- END -->
        </asp:Panel>
    </div>
    <div>
        <asp:LinkButton ID="lnkDummy" runat="server" Text=""></asp:LinkButton>
    </div>
    <!--Document for Doc Upload - END -->
</asp:Content>
