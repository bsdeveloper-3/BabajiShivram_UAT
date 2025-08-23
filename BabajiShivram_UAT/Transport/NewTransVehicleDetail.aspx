<%@ Page Title="New Transport Vehicle Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="NewTransVehicleDetail.aspx.cs" Inherits="Transport_NewTransVehicleDetail" Culture="en-GB" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <style type="text/css">
        .modalBackground {
            background-color: black;
            filter: alpha(opacity=90);
            opacity: 0.5;
        }
    </style>
    <script type="text/javascript">
        function OnJobSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnJobId').value = results.JobId;
        }
    </script>
    <asp:UpdatePanel ID="upnlVehicleDetail" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="csRequiredFields" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="vgRequired" CssClass="errorMsg" />
                <asp:ValidationSummary ID="vsPopup_FundRequest" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" />
                <asp:HiddenField ID="hdnLid" runat="server" Value="0" />
                <asp:HiddenField ID="hdnJobId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnNewPaymentLid" runat="server" Value="0" />
                <asp:HiddenField ID="hdnGetMemoFile" runat="server" />
            </div>
            <div>
                <fieldset>
                    <legend>Transport Request Detail</legend>
                    <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>Ref No.
                            </td>
                            <td>
                                <asp:Label ID="lblTRRefNo" runat="server" Font-Bold="true"></asp:Label>
                            </td>
                            <td>Job No.
                            </td>
                            <td>
                                <asp:Label ID="lblJobNo" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Truck Request Date
                            </td>
                            <td>
                                <asp:Label ID="lblTruckRequestDate" runat="server"></asp:Label>
                            </td>
                            <td>Customer Name
                            </td>
                            <td>
                                <asp:Label ID="lblCustName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Division
                            </td>
                            <td>
                                <asp:Label ID="lblDivision" runat="server"></asp:Label>
                            </td>
                            <td>Plant
                            </td>
                            <td>
                                <asp:Label ID="lblPlant" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Location
                            </td>
                            <td>
                                <asp:Label ID="lblLocationFrom" runat="server"></asp:Label>
                            </td>
                            <td>Destination
                            </td>
                            <td>
                                <asp:Label ID="lblDestination" runat="server"></asp:Label>
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
                    </table>
                </fieldset>
            </div>
            <div>
                <fieldset>
                    <legend>Add Vehicle Rate</legend>
                    <div class="m clear">
                        <asp:HiddenField ID="hdnIsUpdate" runat="server" Value="0" />
                        <asp:Button ID="btnSaveRate" runat="server" Text="Save" OnClick="btnSaveRate_Click" ValidationGroup="vgRequired" TabIndex="17" />
                        <asp:Button ID="btnCancelRate" runat="server" Text="Cancel" OnClick="btnCancelRate_Click" CausesValidation="false" TabIndex="18" />
                        <asp:HiddenField ID="hdnFilePath" runat="server" />
                    </div>
                    <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>Transporter
                                <asp:RequiredFieldValidator ID="rfvTransporter" runat="server" ControlToValidate="ddlTransporter" InitialValue="0"
                                    SetFocusOnError="true" Display="Dynamic" ErrorMessage="Please select transporter." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTransporter" runat="server" Width="300px" TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="ddlTransporter_SelectedIndexChanged">
                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>Vehicle Number
                                <asp:RequiredFieldValidator ID="rfvVehicleNo" runat="server" ControlToValidate="txtVehicleNo" SetFocusOnError="true" Display="Dynamic"
                                    ErrorMessage="Please enter vehicle number." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegExVehicleNo" runat="server" ControlToValidate="txtVehicleNo"
                                    ValidationGroup="vgRequired" Text="Invalid No" Display="Dynamic" SetFocusOnError="true"
                                    ValidationExpression="^[a-z A-Z 0-9 . -]{8,16}$" ErrorMessage="Please Enter Valid Vehicle Number">
                                </asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="rfvBabajiVehicleNo" runat="server" ControlToValidate="ddVehicleNo" InitialValue="0" SetFocusOnError="true" Display="Dynamic"
                                    ErrorMessage="Please select vehicle number." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVehicleNo" runat="server" TabIndex="2" MaxLength="10" Width="160px"></asp:TextBox>
                                <asp:DropDownList ID="ddVehicleNo" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Vehicle Type
                                <asp:RequiredFieldValidator ID="rfvVehicleType" runat="server" ControlToValidate="ddlVehicleType" InitialValue="0" SetFocusOnError="true" Display="Dynamic"
                                    ErrorMessage="Please select vehicle type." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlVehicleType" runat="server" Width="175px" TabIndex="3">
                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>City                               
                            </td>
                            <td>
                                <asp:TextBox ID="txtCity" runat="server" TabIndex="4" MaxLength="10" Width="160px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Attach Collection Memo
                                <asp:RequiredFieldValidator ID="rfvMemoCopy" runat="server" ControlToValidate="fuMemoDocument" Display="Dynamic" SetFocusOnError="true"
                                    ForeColor="Red" ErrorMessage="Please attach collection memo." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:FileUpload ID="fuMemoDocument" runat="server" TabIndex="5" Width="160px" />
                            </td>
                            <td>Freight Rate
                                <asp:RequiredFieldValidator ID="rfvRate" runat="server" ControlToValidate="txtRate" SetFocusOnError="true" Display="Dynamic"
                                    ErrorMessage="Please enter freight rate." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revRate" runat="server" ValidationExpression="^[1-9]\d*(\.\d+)?$" ControlToValidate="txtRate"
                                    SetFocusOnError="true" Display="Dynamic" ErrorMessage="(Invalid rate)" ForeColor="Red"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRate" runat="server" TabIndex="6" Width="160px" AutoPostBack="true" OnTextChanged="txtRate_TextChanged"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Advance (%)
                               <%-- <asp:RequiredFieldValidator ID="rfvAdvance" runat="server" ControlToValidate="txtAdvance" SetFocusOnError="true" Display="Dynamic"
                                    ErrorMessage="Please enter advance (%)." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>--%>
                                <asp:RegularExpressionValidator ID="revAdvance" runat="server" ValidationExpression="^[1-9]\d*(\.\d+)?$" ControlToValidate="txtRate"
                                    SetFocusOnError="true" Display="Dynamic" ErrorMessage="(Invalid advance)" ForeColor="Red"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAdvance" runat="server" TabIndex="7" Width="160px" AutoPostBack="true" OnTextChanged="txtAdvance_TextChanged"></asp:TextBox>
                            </td>
                            <td>Advance Amount</td>
                            <td>
                                <asp:TextBox ID="txtAdvanceAmount" runat="server" TabIndex="8" Enabled="false" Width="160px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Market Rate
                                  <asp:RequiredFieldValidator ID="rfvMarketRate" runat="server" ControlToValidate="txtMarketBillingRate" SetFocusOnError="true" Display="Dynamic"
                                      ErrorMessage="Please enter market billing rate." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="rvMarketRate" runat="server" ControlToValidate="txtMarketBillingRate" Type="Double" ErrorMessage="(Invalid rate)"
                                    ForeColor="Red" SetFocusOnError="true" Display="Dynamic"></asp:RangeValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMarketBillingRate" runat="server" TabIndex="9" Width="160px"></asp:TextBox>
                            </td>
                            <%-- <tr>
                            <td>LR No
                                <asp:RequiredFieldValidator ID="rfvLRNo" runat="server" ControlToValidate="txtLRNo" SetFocusOnError="true" Display="Dynamic"
                                    ErrorMessage="Please enter LR No." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLRNo" runat="server" TabIndex="8" Width="160px"></asp:TextBox>
                            </td>
                            <td>LR Date
                                 <cc1:CalendarExtender ID="calLRDate" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="imgLR"
                                     Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtLRDate">
                                 </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="meeLrDate" TargetControlID="txtLRDate" Mask="99/99/9999" MessageValidatorTip="true"
                                    MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="mevLrDate" ControlExtender="meeLrDate" ControlToValidate="txtLRDate" IsValidEmpty="false"
                                    InvalidValueMessage="LR Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true"
                                    EmptyValueBlurredText="*" EmptyValueMessage="Please enter LR Date." MinimumValueMessage="Invalid Date"
                                    MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" runat="Server" ValidationGroup="vgRequired"></cc1:MaskedEditValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLRDate" runat="server" Width="110px" TabIndex="9" placeholder="dd/mm/yyyy"></asp:TextBox>
                                <asp:Image ID="imgLR" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>Babaji Challan No
                                <asp:RequiredFieldValidator ID="RFVBabajiChalanNo" runat="server" ControlToValidate="txtBabajiChallanNo"
                                    SetFocusOnError="true" Text="*" InitialValue="" ErrorMessage="Please enter Babaji Challan No"
                                    Display="Dynamic" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBabajiChallanNo" runat="server" MaxLength="50" TabIndex="10" Width="160px"></asp:TextBox>&nbsp;
                            </td>
                            <td>Babaji Challan Date
                                <cc1:CalendarExtender ID="calChallanDate" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="imgChallanDate"
                                    Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtBabajiChallanDate">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="MEditChallanDate" TargetControlID="txtBabajiChallanDate"
                                    Mask="99/99/9999" MessageValidatorTip="true" MaskType="Date" AutoComplete="false"
                                    runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="MEditValChallanDate" ControlExtender="MEditChallanDate"
                                    ControlToValidate="txtBabajiChallanDate" IsValidEmpty="false"
                                    MinimumValueMessage="Invalid Challan Date"
                                    MaximumValueMessage="Invalid Challan Date" EmptyValueBlurredText="*" EmptyValueMessage="Please enter Babaji Challan Date."
                                    SetFocusOnError="true" runat="Server" ValidationGroup="vgRequired"></cc1:MaskedEditValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBabajiChallanDate" runat="server" Width="110px" TabIndex="11"
                                    placeholder="dd/mm/yyyy"></asp:TextBox>
                                <asp:Image ID="imgChallanDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                                    runat="server" />
                            </td>
                        </tr>--%>
                            <td>Detention Amount
                                <asp:RangeValidator ID="rvDetentionAmt" runat="server" ControlToValidate="txtDetentionAmount" Type="Double" ErrorMessage="(Invalid amount)"
                                    ForeColor="Red" SetFocusOnError="true" Display="Dynamic"></asp:RangeValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDetentionAmount" runat="server" Width="160px" TabIndex="10"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Varai Expense
                                <asp:RangeValidator ID="rvVaraiExp" runat="server" ControlToValidate="txtVaraiExp" Type="Double" ErrorMessage="(Invalid expense)"
                                    ForeColor="Red" SetFocusOnError="true" Display="Dynamic"></asp:RangeValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVaraiExp" runat="server" Width="160px" TabIndex="11"></asp:TextBox>
                            </td>
                            <td>Empty Cont Rcpt Charges
                                <asp:RangeValidator ID="rvEmptyContRecptCharges" runat="server" ControlToValidate="txtEmptyContCharges" Type="Double" ErrorMessage="(Invalid charges)"
                                    ForeColor="Red" SetFocusOnError="true" Display="Dynamic"></asp:RangeValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmptyContCharges" runat="server" Width="160px" TabIndex="12"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Toll Charges
                                <asp:RangeValidator ID="rvTollCharges" runat="server" ControlToValidate="txtTollCharges" Type="Double" ErrorMessage="(Invalid charges)"
                                    ForeColor="Red" SetFocusOnError="true" Display="Dynamic"></asp:RangeValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTollCharges" runat="server" Width="160px" TabIndex="13"></asp:TextBox>
                            </td>
                            <td>Other Charges
                                <asp:RangeValidator ID="rvOtherCharges" runat="server" ControlToValidate="txtOtherCharges" Type="Double" ErrorMessage="(Invalid charges)"
                                    ForeColor="Red" SetFocusOnError="true" Display="Dynamic"></asp:RangeValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOtherCharges" runat="server" Width="160px" TabIndex="14"></asp:TextBox>
                            </td>
                        </tr>
                    </table>

                    <fieldset>
                        <legend>Selling Rate Detail</legend>
                        <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                            <td>Selling Detail
                                <asp:RequiredFieldValidator ID="rfvSellingPrice" runat="server" ControlToValidate="rdlRequestReceive" SetFocusOnError="true" Display="Dynamic"
                                    ErrorMessage="Select Selling Detail." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rdlRequestReceive" runat="server" OnSelectedIndexChanged="rdlRequestReceive_SelectedIndexChanged"
                                    AutoPostBack="true" TabIndex="14" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="As per Contract" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="As per Email" Value="2"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr id="trContractDetail" runat="server" visible="false">
                            <td>
                                Selling Freight Rate
                                <asp:RequiredFieldValidator ID="rfvPrice" runat="server" ControlToValidate="txtContractPrice" SetFocusOnError="true" Display="Dynamic"
                                    ErrorMessage="Please enter selling freight price." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtContractPrice" runat="server"  Width="160px" TabIndex="15"></asp:TextBox>
                            </td>
                            <td>
                                Contract Copy
                            </td>
                            <td>
                                <asp:FileUpload ID="fuContractUploadCopy" runat="server" />
                                <asp:RegularExpressionValidator
                                    ID="RegularExpressionValidator1" runat="server"
                                    ErrorMessage="Only PDF files are allowed!" Display="Dynamic" SetFocusOnError="true"
                                    ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" type="reset"
                                    ControlToValidate="fuContractUploadCopy" CssClass="text-red"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr id="trEmailDetail" runat="server" visible="false">
                            <td>Selling Freight Rate
                                <asp:RequiredFieldValidator ID="rfvSellPrice" runat="server" ControlToValidate="txtEmailSellPrice"
                                     SetFocusOnError="true" Display="Dynamic"
                                    ErrorMessage="Please enter Selling freight price." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmailSellPrice" runat="server" Width="160px" TabIndex="16"></asp:TextBox>
                            </td>
                            <td>
                                Email Approval Copy
                            </td>
                            <td>
                                <asp:FileUpload ID="fuEmailApprovalCopy" runat="server" />
                                <asp:RequiredFieldValidator ID="RFVFile" runat="server" ControlToValidate="fuEmailApprovalCopy" CausesValidation="true"
                                    InitialValue="" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Please upload email copy."
                                    ValidationGroup="vgRequired" Enabled="false"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator
                                    ID="RegularExpressionValidator2" runat="server" CausesValidation="true"
                                    ErrorMessage="Only PDF files are allowed!" Display="Dynamic" SetFocusOnError="true"
                                    ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" type="reset"
                                    ControlToValidate="fuEmailApprovalCopy" CssClass="text-red"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>Billing Instruction
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtBillingInstruction" SetFocusOnError="true" Display="Dynamic"
                                    ErrorMessage="Please enter billing instruction." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtBillingInstruction" runat="server" Width="650px" Rows="3" TextMode="MultiLine" TabIndex="18"></asp:TextBox>
                            </td>
                        </tr>

                        </table>
                    </fieldset>
                </fieldset>
                <div style="text-align: center">
                    <asp:Label ID="lblError_RateDetail" runat="server"></asp:Label>
                </div>
                <fieldset>
                    <legend>Rate Detail</legend>
                    <asp:Button ID="btnFundRequest" runat="server" Text="Send Fund Request" OnClick="btnFundRequest_Click" />
                    <asp:GridView ID="gvRateDetail" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr"
                        DataKeyNames="lid,FundRequestId" OnRowCommand="gvRateDetail_RowCommand" AllowPaging="false" AllowSorting="True" CssClass="table" PageSize="20"
                        DataSourceID="RateSqlDataSource" OnPreRender="gvRateDetail_PreRender" OnRowDataBound="gvRateDetail_RowDataBound">
                        <Columns>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" CommandName="Edit" runat="server" Text="Edit" CommandArgument='<%#Bind("lid") %>'
                                        ToolTip="Click To Update Rate Detail."></asp:LinkButton>
                                    <asp:LinkButton ID="lnkDelete" CommandName="DeleteRow" runat="server" Text="Delete" CommandArgument='<%#Bind("lid") %>'
                                        ToolTip="Click To Delete Rate Detail."></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sl" Visible="false">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1%>.
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" ToolTip="Select to send fund request." />
                                    <asp:HiddenField ID="hdnFundReqId" runat="server" Value='<%#Bind("FundRequestId") %>' />
                                    <asp:HiddenField ID="hdnTransporterId" runat="server" Value='<%#Bind("TransporterId") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fund Req" ItemStyle-HorizontalAlign="Center" Visible="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnFundRequest" runat="server" Text="Send" Font-Underline="true" AutoPostBack="true" ToolTip="Send fund request"
                                        OnClick="lnkbtnFundRequest_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="TransRefNo" HeaderText="TR Ref No" SortExpression="TransRefNo" ReadOnly="true" Visible="false" />
                            <asp:TemplateField HeaderText="Memo">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgbtnMemoCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download collection memo copy."
                                        CommandName="MemoCopy" CommandArgument='<%#Eval("MemoAttachment")%>' />
                                    <asp:HiddenField ID="hdnMemoPath" runat="server" Value='<%#Eval("MemoAttachment")%>' />
                                    <asp:HiddenField ID="hdnMemoCopyName" runat="server" Value='<%#Eval("MemoCopyName")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="TransporterName" HeaderText="Transporter" SortExpression="TransporterName" ReadOnly="true" />
                            <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" SortExpression="VehicleNo" ReadOnly="true" />
                            <asp:BoundField DataField="VehicleTypeName" HeaderText="Vehicle Type" SortExpression="VehicleTypeName" ReadOnly="true" />
                            <asp:BoundField DataField="City" HeaderText="City" SortExpression="City" ReadOnly="true" />
                            <asp:BoundField DataField="LRNo" HeaderText="LRNo" SortExpression="LRNo" ReadOnly="true" />
                            <asp:BoundField DataField="LRDate" HeaderText="LRDate" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LRDate" ReadOnly="true" />
                            <asp:BoundField DataField="ChallanNo" HeaderText="ChallanNo" SortExpression="ChallanNo" ReadOnly="true" />
                            <asp:BoundField DataField="ChallanDate" HeaderText="Challan Date" SortExpression="ChallanDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="Rate" HeaderText="Freight Rate" SortExpression="Rate" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Advance" HeaderText="Advance (%)" SortExpression="Advance" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="AdvanceAmount" HeaderText="AdvanceAmount" SortExpression="AdvanceAmount" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="MarketBillingRate" HeaderText="Market Rate" SortExpression="MarketBillingRate" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="FreightAmount" HeaderText="Freight Amt" SortExpression="FreightAmount" ReadOnly="true" Visible="false" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="DetentionAmount" HeaderText="Detention Amt" SortExpression="DetentionAmount" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="VaraiExpense" HeaderText="Varai Exp" SortExpression="VaraiExpense" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="EmptyContRecptCharges" HeaderText="Empty Cont Charges" SortExpression="EmptyContRecptCharges" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="TollCharges" HeaderText="Toll Charges" SortExpression="TollCharges" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="OtherCharges" HeaderText="Other Charges" SortExpression="OtherCharges" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy" ReadOnly="true" />
                            <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" SortExpression="CreatedDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="UpdatedBy" HeaderText="Updated By" SortExpression="UpdatedBy" ReadOnly="true" />
                            <asp:BoundField DataField="UpdatedDate" HeaderText="Updated Date" SortExpression="UpdatedDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                        </Columns>
                        <PagerTemplate>
                            <asp:GridViewPager runat="server" />
                        </PagerTemplate>
                    </asp:GridView>
                    <div>
                        <asp:SqlDataSource ID="RateSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="TR_GetTransRateDetail" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </fieldset>
            </div>
            <div id="dvModalPopup">
                <asp:HiddenField ID="hdnFundRequest" runat="server" Value="0" />
                <cc1:ModalPopupExtender ID="mpeFundRequest" runat="server" TargetControlID="hdnFundRequest" BackgroundCssClass="modalBackground" CancelControlID="imgClosePopup"
                    PopupControlID="pnlFundRequest" DropShadow="true">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pnlFundRequest" runat="server" CssClass="modalPopup1" BackColor="#F5F5DC" Width="1100px" Height="600px" BorderStyle="Solid" BorderWidth="1px">
                    <div id="div1" runat="server">
                        <br />
                        <table width="100%">
                            <tr>
                                <td align="center"><b><u>FUND REQUEST</u></b>
                                    <span style="float: right">
                                        <asp:ImageButton ID="imgClosePopup" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgClosePopup_Click" ToolTip="Close" />
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblFundError" runat="server" Font-Bold="true" Font-Size="13px"></asp:Label>
                                    <asp:HiddenField ID="hdnRateDetailId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnMemoCopyPath" runat="server" />
                                </td>
                            </tr>
                        </table>
                        <div>
                            <asp:Panel ID="pnlFundRequest2" runat="server" Width="1060px" Height="530px" ScrollBars="Auto" Style="padding-left: 10px">
                                <fieldset>
                                    <div id="divInstruction" class="info" runat="server">
                                    </div>
                                    <div class="m clear">
                                        <asp:HiddenField ID="hdnTransporterId" runat="server" Value="0" />
                                        <asp:Button ID="btnSubmit" Text="Save" OnClick="btnSubmit_Click" runat="server" ValidationGroup="Required"
                                            TabIndex="7" />
                                        <asp:Button ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false" TabIndex="8"
                                            runat="server" />
                                    </div>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                        <tr>
                                            <td width="20%">Job Number
                                            </td>
                                            <td>
                                                <asp:Label ID="txtJobNumber" Width="160px" runat="server" ToolTip="Enter Job Number."
                                                    placeholder="Search" TabIndex="1" AutoPostBack="true" OnTextChanged="txtJobNumber_TextChanged"></asp:Label>
                                            </td>
                                            <td>Consignee
                                            </td>
                                            <td>
                                                <asp:Label ID="lblConsignee" runat="server" Enabled="false" Width="230px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Branch</td>
                                            <td>
                                                <asp:HiddenField ID="hdnBranchId" runat="server" Value="0" />
                                                <asp:Label ID="lblBranch" runat="server"></asp:Label>
                                            </td>
                                            <td>Type of Expense                           
                                            </td>
                                            <td>
                                                <asp:Label ID="lblExpenseType" runat="server" Text="Advance Payment"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Planning Date
                                                <cc1:CalendarExtender ID="calPlanningDate" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="imgPlanningDate"
                                                    Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtPlanningDate">
                                                </cc1:CalendarExtender>
                                                <cc1:MaskedEditExtender ID="meePlanningDate" TargetControlID="txtPlanningDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                    MaskType="Date" AutoComplete="false" runat="server">
                                                </cc1:MaskedEditExtender>
                                                <cc1:MaskedEditValidator ID="mevPlanningDate" ControlExtender="meePlanningDate" ControlToValidate="txtPlanningDate" IsValidEmpty="true"
                                                    InvalidValueMessage="Planning Date is invalid" InvalidValueBlurredMessage="Invalid Planning Date" SetFocusOnError="true"
                                                    MinimumValueMessage="Invalid Planning Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="31/12/2025"
                                                    runat="Server"></cc1:MaskedEditValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPlanningDate" runat="server" Width="125px" placeholder="dd/mm/yyyy" TabIndex="3" ToolTip="Enter Planning Date."></asp:TextBox>
                                                <asp:Image ID="imgPlanningDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                            </td>
                                            <td>Type of Payment
                                            <asp:RequiredFieldValidator ID="rfvpaytype" runat="server" ValidationGroup="Required"
                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlPaymentType" InitialValue="0"
                                                Text="*" ErrorMessage="Please Select Type of Payment."> </asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlPaymentType" runat="server" AppendDataBoundItems="true" DataSourceID="DataSourcePaymentType"
                                                    DataTextField="sName" DataValueField="lid" TabIndex="4" ToolTip="Select Type Of Payment.">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="trAmount" runat="server">
                                            <td>Freight Rate</td>
                                            <td>
                                                <asp:Label ID="lblFreightRate" runat="server"></asp:Label>
                                            </td>
                                            <td>Amount
                                            </td>
                                            <td>
                                                <asp:Label ID="txtAmount" runat="server" Width="160px" ToolTip="Enter Amount." TabIndex="6" OnTextChanged="txtAmount_OnTextChanged" AutoPostBack="true"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Paid To
                                            </td>
                                            <td>
                                                <asp:Label ID="txtPaidTo" runat="server" ToolTip="Enter Paid To." Width="290px" TabIndex="7" Enabled="false"></asp:Label>
                                            </td>
                                            <td>Bank Name</td>
                                            <td>
                                                <asp:Label ID="lblBankName" runat="server"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td>Account No</td>
                                            <td>
                                                <asp:Label ID="lblAccountNo" runat="server"></asp:Label>
                                            </td>
                                            <td>IFSC Code</td>
                                            <td>
                                                <asp:Label ID="lblIFSCCode" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Remark
                                            <asp:RequiredFieldValidator ID="rfvRemark" runat="server" ValidationGroup="Required"
                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtRemark"
                                                Text="*" ErrorMessage="Please Enter Remark."> </asp:RequiredFieldValidator>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtRemark" runat="server" Rows="3" TextMode="MultiLine" TabIndex="8"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <fieldset>
                                        <legend>Vehicle Detail</legend>
                                        <div>
                                            <asp:GridView ID="gvPopup_Vehicle" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr"
                                                DataKeyNames="lid" AllowPaging="false" AllowSorting="True" CssClass="table" PageSize="20" DataSourceID="SqlDataSourceVehicle" OnRowCommand="gvPopup_Vehicle_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Memo">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgbtnMemoCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download collection memo copy."
                                                                CommandName="MemoCopy" CommandArgument='<%#Eval("MemoAttachment")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" ReadOnly="true" />
                                                    <asp:BoundField DataField="VehicleTypeName" HeaderText="Vehicle Type" ReadOnly="true" />
                                                    <asp:BoundField DataField="LocationFrom" HeaderText="Location From" ReadOnly="true" />
                                                    <asp:BoundField DataField="DeliveryPoint" HeaderText="Delivery Point" ReadOnly="true" />
                                                    <asp:BoundField DataField="AdvanceAmount" HeaderText="Advance" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Rate" HeaderText="Vehicle Hire(Broker Rate)" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Balance" HeaderText="Balance" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="MarketBillingRate" HeaderText="Market Billing Rate" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="SavingAmt" HeaderText="Saving Amt" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                                </Columns>
                                                <PagerTemplate>
                                                    <asp:GridViewPager runat="server" />
                                                </PagerTemplate>
                                            </asp:GridView>
                                            <div>
                                                <asp:SqlDataSource ID="SqlDataSourceVehicle" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                                    SelectCommand="TR_GetTransRateDetailByTP" SelectCommandType="StoredProcedure">
                                                    <SelectParameters>
                                                        <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
                                                        <asp:ControlParameter ControlID="hdnTransporterId" Name="TransporterId" PropertyName="Value" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                            </div>
                                        </div>
                                    </fieldset>
                                    <fieldset>
                                        <legend>Add Documents</legend>
                                        <div id="dvUploadNewFile2" runat="server" style="max-height: 200px; overflow: auto;">
                                            <asp:FileUpload ID="fuDocument2" runat="server" />
                                            <asp:Button ID="btnSaveDocument2" Text="Save Document" runat="server" OnClick="btnSaveDocument2_Click" />
                                        </div>
                                        <br />
                                        <div>
                                            <asp:Repeater ID="rptDocument2" runat="server" OnItemCommand="rptDocument2_ItemCommand">
                                                <HeaderTemplate>
                                                    <table class="table" border="0" cellpadding="0" cellspacing="0" style="width: 50%">
                                                        <tr bgcolor="#FF781E">
                                                            <th>Sl
                                                            </th>
                                                            <th>Document Name
                                                            </th>
                                                            <th>Action
                                                            </th>
                                                        </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <%#Container.ItemIndex +1%>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="lnkDownload" Text='<%#DataBinder.Eval(Container.DataItem,"DocumentName") %>'
                                                                CommandArgument='<%# Eval("DocPath") %>' CausesValidation="false" runat="server"
                                                                Width="200px" CommandName="DownloadFile"></asp:LinkButton>
                                                            &nbsp;
                                                            <asp:HiddenField ID="hdnDocLid" Value='<%#DataBinder.Eval(Container.DataItem,"PkId") %>'
                                                                runat="server"></asp:HiddenField>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="lnkDelete" CommandName="Delete" ToolTip="Delete" Width="39" CausesValidation="false"
                                                                runat="server" Text="Delete" Font-Underline="true" OnClientClick="return confirm('Are you sure you want to remove this document?')"></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </fieldset>
                                </fieldset>
                            </asp:Panel>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div>
                <asp:SqlDataSource ID="DataSourceJobdetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetJobDetailForArchive" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceBranch" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="BS_GetBranchByUser" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceExpense" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetRequestTypeMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourcePaymentType" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetPaymentTypeMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

