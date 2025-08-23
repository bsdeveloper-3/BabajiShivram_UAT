<%@ Page Title="Consolidate Transport Request" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ConsolidateVehicle.aspx.cs" Inherits="Transport_ConsolidateVehicle" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <style type="text/css">
        .modalBackground {
            background-color: black;
            filter: alpha(opacity=90);
            opacity: 0.5;
        }
    </style>
    <asp:UpdatePanel ID="upnlConsolidateRequest" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="csRequiredFields" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="vgRequired" CssClass="errorMsg" />
                <asp:HiddenField ID="hdnJobId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnNewPaymentLid" runat="server" Value="0" />
                <asp:HiddenField ID="hdnMemoCopyPath" runat="server" />
                <asp:HiddenField ID="hdnBranchId" runat="server" Value="0" />
            </div>
            <div>
                <fieldset>
                    <legend>Consolidate Detail</legend>
                    <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>Consolidate TR Ref No.
                            </td>
                            <td>
                                <asp:Label ID="lblTRRefNo" runat="server" Font-Bold="true"></asp:Label>
                            </td>
                            <td>Transporter
                            </td>
                            <td>
                                <asp:HiddenField ID="hdnTransporterId" runat="server" Value="0" />
                                <asp:Label ID="lblTransporter" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Created By
                            </td>
                            <td>
                                <asp:Label ID="lblCreatedBy" runat="server"></asp:Label>
                            </td>
                            <td>Created Date</td>
                            <td>
                                <asp:Label ID="lblCreatedDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>Transport Job Detail</legend>
                    <div>
                        <asp:GridView ID="gvTransportJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="TransReqId" PagerStyle-CssClass="pgr"
                            AllowPaging="True" AllowSorting="True" Width="100%" PageSize="20" PagerSettings-Position="TopAndBottom" DataSourceID="DataSourceTransportJobDetail">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="TRRefNo" HeaderText="TR Ref No" />
                                <asp:BoundField DataField="JobRefNo" HeaderText="Job Ref No" />
                                <asp:BoundField DataField="CustName" HeaderText="Customer" />
                                <asp:BoundField DataField="VehiclePlaceDate" HeaderText="Vehicle Place Date" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="LocationFrom" HeaderText="From" />
                                <asp:BoundField DataField="Destination" HeaderText="Destination" />
                                <asp:BoundField DataField="NoOfPkgs" HeaderText="No Of Pkgs" />
                                <asp:BoundField DataField="GrossWeight" HeaderText="Gross Weight" />
                                <asp:BoundField DataField="Count20" HeaderText="Cont 20" />
                                <asp:BoundField DataField="Count40" HeaderText="Cont 40" />
                                <asp:BoundField DataField="DeliveryType" HeaderText="Delivery Type" />
                                <asp:BoundField DataField="RequestedBy" HeaderText="Requested By" />
                                <asp:BoundField DataField="RequestedDate" HeaderText="Request Date" DataFormatString="{0:dd/MM/yyyy}" />
                            </Columns>
                            <PagerTemplate>
                                <asp:GridViewPager ID="GridViewPager1" runat="server" />
                            </PagerTemplate>
                        </asp:GridView>
                    </div>
                </fieldset>
                <div style="text-align: center">
                    <asp:Label ID="lblError_RateDetail" runat="server"></asp:Label>
                </div>
                <fieldset>
                    <legend>Rate Detail</legend>
                    <div style="width: 1350px; overflow: auto">
                        <asp:GridView ID="gvRateDetail" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr"
                            DataKeyNames="lid,FundRequestId" OnRowCommand="gvRateDetail_RowCommand" AllowPaging="false" AllowSorting="True" CssClass="table" PageSize="20"
                            DataSourceID="RateSqlDataSource" OnPreRender="gvRateDetail_PreRender" OnRowDataBound="gvRateDetail_RowDataBound">
                            <Columns>
                                <asp:TemplateField ShowHeader="False" Visible="false">
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
                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" Visible="false">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" ToolTip="Select to send fund request." />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fund Req" ItemStyle-HorizontalAlign="Center" Visible="false">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnFundRequest" runat="server" Text="Send" Font-Underline="true" AutoPostBack="true" ToolTip="Send fund request"
                                            OnClick="lnkbtnFundRequest_Click" />
                                        <asp:HiddenField ID="hdnFundReqId" runat="server" Value='<%#Bind("FundRequestId") %>' />
                                        <asp:HiddenField ID="hdnTransporterId" runat="server" Value='<%#Bind("TransporterId") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="TransRefNo" HeaderText="TR Ref No" SortExpression="TransRefNo" ReadOnly="true" Visible="false" />
                                <asp:TemplateField HeaderText="Memo">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnMemoCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download collection memo copy."
                                            CommandName="MemoCopy" CommandArgument='<%#Eval("MemoAttachment")%>' />
                                        <asp:HiddenField ID="hdnMemoPath" runat="server" Value='<%#Eval("MemoAttachment")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="TransporterName" HeaderText="Transporter" SortExpression="TransporterName" ReadOnly="true" />
                                <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" SortExpression="VehicleNo" ReadOnly="true" />
                                <asp:BoundField DataField="VehicleTypeName" HeaderText="Vehicle Type" SortExpression="VehicleTypeName" ReadOnly="true" />
                                <asp:BoundField DataField="City" HeaderText="City" SortExpression="City" ReadOnly="true" />
                                <asp:BoundField DataField="LRNo" HeaderText="LRNo" SortExpression="LRNo" ReadOnly="true" Visible="false" />
                                <asp:BoundField DataField="LRDate" HeaderText="LRDate" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LRDate" ReadOnly="true" Visible="false" />
                                <asp:BoundField DataField="ChallanNo" HeaderText="ChallanNo" SortExpression="ChallanNo" ReadOnly="true" Visible="false" />
                                <asp:BoundField DataField="ChallanDate" HeaderText="Challan Date" SortExpression="ChallanDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" Visible="false" />
                                <asp:BoundField DataField="Rate" HeaderText="Freight Rate" SortExpression="Rate" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="Advance" HeaderText="Advance (%)" SortExpression="Advance" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="AdvanceAmount" HeaderText="AdvanceAmount" SortExpression="AdvanceAmount" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="MarketBillingRate" HeaderText="Market Billing Rate" SortExpression="MarketBillingRate" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
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
                    </div>
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
                <asp:Panel ID="pnlFundRequest" runat="server" CssClass="modalPopup1" BackColor="#F5F5DC" Width="1200px" Height="600px" BorderStyle="Solid" BorderWidth="1px">
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
                                </td>
                            </tr>
                        </table>
                        <div>
                            <asp:Panel ID="pnlFundRequest2" runat="server" Width="1150px" Height="560px" ScrollBars="Auto" Style="padding-left: 10px">
                                <fieldset>
                                    <div id="divInstruction" class="info" runat="server">
                                    </div>
                                    <div class="m clear">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
                                        <asp:Button ID="btnSubmit" Text="Save" OnClick="btnSubmit_Click" runat="server" ValidationGroup="Required"
                                            TabIndex="7" />
                                        <asp:Button ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false" TabIndex="8"
                                            runat="server" />
                                    </div>
                                    <asp:GridView ID="gvPopup_JobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                                        Width="95%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="JobId, BranchId"
                                        DataSourceID="DataSourceTransportJobDetail" CellPadding="4" AllowPaging="True" AllowSorting="True"
                                        OnRowEditing="gvPopup_JobDetail_RowEditing" OnRowCancelingEdit="gvPopup_JobDetail_RowCancelingEdit"
                                        OnRowUpdating="gvPopup_JobDetail_RowUpdating">
                                        <Columns>
                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEdit" CommandName="Edit" runat="server" Text="Edit"
                                                        ToolTip="Click To Update Planning Date."></asp:LinkButton>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update Planning Date." runat="server"
                                                        Text="Update" ValidationGroup="Required"></asp:LinkButton>
                                                    <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel Planning Date Update." CausesValidation="false"
                                                        runat="server" Text="Cancel"></asp:LinkButton>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sl">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Job No" DataField="JobRefNo" ItemStyle-Width="120px" ReadOnly="true" />
                                            <asp:BoundField HeaderText="Consignee" DataField="ConsigneeName" ReadOnly="true" />
                                            <asp:BoundField HeaderText="Branch" DataField="BranchName" ReadOnly="true" />
                                            <asp:BoundField HeaderText="From" DataField="LocationFrom" ReadOnly="true" />
                                            <asp:BoundField HeaderText="Destination" DataField="Destination" ReadOnly="true" />
                                            <asp:BoundField DataField="LRNo" HeaderText="LR No" SortExpression="LRNo" ReadOnly="true" />
                                            <asp:BoundField DataField="LRDate" HeaderText="LR Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LRDate" ReadOnly="true" />
                                            <asp:BoundField DataField="BabajiChallanNo" HeaderText="ChallanNo" SortExpression="BabajiChallanNo" ReadOnly="true" />
                                            <asp:BoundField DataField="BabajiChallanDate" HeaderText="Challan Date" SortExpression="BabajiChallanDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:TemplateField HeaderText="Planning Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPlanningDate" Text='<%# Bind("PlanningDate","{0:dd/MM/yyyy}")%>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtPlanningDate" runat="server" Width="80px" MaxLength="10" Text='<%# Bind("PlanningDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="calPlanningDate" runat="server" Enabled="True"
                                                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgPlanningDate" PopupPosition="BottomRight"
                                                        TargetControlID="txtPlanningDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:Image ID="imgPlanningDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                    <cc1:MaskedEditExtender ID="meePlanningDate" TargetControlID="txtPlanningDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                        MaskType="Date" AutoComplete="false" runat="server">
                                                    </cc1:MaskedEditExtender>
                                                    <cc1:MaskedEditValidator ID="mevPlanningDate" ControlExtender="meePlanningDate" ControlToValidate="txtPlanningDate" IsValidEmpty="false"
                                                        EmptyValueMessage="Please Enter Planning Date" EmptyValueBlurredText="*" InvalidValueMessage="Planning Date is invalid"
                                                        InvalidValueBlurredMessage="Invalid Date" runat="Server" SetFocusOnError="true" ValidationGroup="Required"></cc1:MaskedEditValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PlanningDate" HeaderText="PlanningDate" SortExpression="PlanningDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" Visible="false" />
                                        </Columns>
                                    </asp:GridView>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                        <tr>
                                            <%--<td>Planning Date
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
                                            </td>--%>
                                            <td>Type of Expense                           
                                            </td>
                                            <td>
                                                <asp:Label ID="lblExpenseType" runat="server" Text="Advance Payment"></asp:Label>
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
                                                    <asp:BoundField DataField="TransporterName" HeaderText="Transporter" ReadOnly="true" />
                                                    <asp:BoundField DataField="LocationFrom" HeaderText="Location From" ReadOnly="true" Visible="false" />
                                                    <asp:BoundField DataField="DeliveryPoint" HeaderText="Delivery Point" ReadOnly="true" Visible="false" />
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
                <asp:SqlDataSource ID="DataSourceTransportJobDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TR_GetConsolidateJobDetail" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="ConsolidateID" SessionField="TRConsolidateId" />
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
            <fieldset style="visibility: hidden">
                <legend>Add Vehicle Rate</legend>
                <div class="m clear">
                    <asp:HiddenField ID="hdnLid" runat="server" Value="0" />
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
                                <asp:RequiredFieldValidator ID="rfvVehicleId" runat="server" ControlToValidate="ddVehicleNo" InitialValue="0" SetFocusOnError="true" Display="Dynamic"
                                    ErrorMessage="Please select vehicle number." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="rfvVehicleNo" runat="server" ControlToValidate="txtVehicleNo" SetFocusOnError="true" Display="Dynamic"
                                ErrorMessage="Please enter vehicle number." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegExVehicleNo" runat="server" ControlToValidate="txtVehicleNo"
                                ValidationGroup="vgRequired" Text="Invalid No" Display="Dynamic" SetFocusOnError="true"
                                ValidationExpression="^[a-z A-Z 0-9 . -]{8,16}$" ErrorMessage="Please Enter Valid Vehicle Number">
                            </asp:RegularExpressionValidator>
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
                        <td>Attach Collection Memo</td>
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
                    <tr>
                        <td>Billing Instruction</td>
                        <td colspan="3">
                            <asp:TextBox ID="txtBillingInstruction" runat="server" Width="750px" Rows="3" TextMode="MultiLine" TabIndex="15"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
