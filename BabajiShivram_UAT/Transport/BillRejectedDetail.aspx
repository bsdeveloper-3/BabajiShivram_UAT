<%@ Page Title="Approve Bill Rejected" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BillRejectedDetail.aspx.cs"
    Inherits="Transport_BillRejectedDetail" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <div>
        <cc1:CalendarExtender ID="calBillSubmitDate" runat="server" Enabled="True" EnableViewState="False"
            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgBillSubmitDate"
            PopupPosition="BottomRight" TargetControlID="txtBillSubmitDate">
        </cc1:CalendarExtender>
        <cc1:CalendarExtender ID="calBillDate" runat="server" Enabled="True" EnableViewState="False"
            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgBillDate"
            PopupPosition="BottomRight" TargetControlID="txtBillDate">
        </cc1:CalendarExtender>
        <asp:ValidationSummary ID="csRequiredFields" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="BillRequired" CssClass="errorMsg" />
    </div>
    <div>
        <script type="text/javascript">
            function ConfirmMessage() {
                var PageValid = document.getElementById('<%=hdnPageValid.ClientID%>').value;
                if (PageValid == "1") {
                    if (confirm('Bill Amount is not matching with approved amount, are you sure to send Bill for Approval?')) {
                        return true;
                    } else {
                        Page_ClientValidate("BillRequired");
                    }
                }
            }
        </script>
    </div>
    <div>
        <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" />
        <asp:HiddenField ID="hdnPageValid" runat="server" />
    </div>
    <div>
        <fieldset id="fsGeneralTransportDetails" runat="server">
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
                    <td>Customer Name
                    </td>
                    <td>
                        <asp:Label ID="lblCustName" runat="server"></asp:Label>
                    </td>
                    <td>Consignee Name
                    </td>
                    <td>
                        <asp:Label ID="lblConsigneeName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Truck Request Date
                    </td>
                    <td>
                        <asp:Label ID="lblTruckRequestDate" runat="server"></asp:Label>
                    </td>
                    <td>Vehicle Place Require Date
                    </td>
                    <td>
                        <asp:Label ID="lblVehiclePlaceDate" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDispatch_Title" runat="server" Text="Number of vehicles required"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblDispatch_Value" runat="server"></asp:Label>
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
                    <td>Dimension</td>
                    <td>
                        <asp:Label ID="lblDimension" runat="server"></asp:Label>
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
                    <td>
                        <asp:Label ID="lblDelExportType_Title" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblDelExportType_Value" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Packing List Documents</td>
                    <td>
                        <asp:ImageButton ID="imgbtnPackingList" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20"
                            ToolTip="Click to view documents." OnClick="imgbtnPackingList_Click" />
                    </td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
        </fieldset>
    </div>
    <fieldset id="fsConsolidateJobs" runat="server">
        <legend>Consolidate Jobs</legend>
        <div style="width: 1360px; overflow: auto">
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
                    <asp:BoundField DataField="PlanningDate" HeaderText="Planning Date" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="LRNo" HeaderText="LR No" />
                    <asp:BoundField DataField="LRDate" HeaderText="LR Date" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="BabajiChallanNo" HeaderText="Challan No" />
                    <asp:BoundField DataField="BabajiChallanDate" HeaderText="Challan Date" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="RequestedBy" HeaderText="Requested By" />
                    <asp:BoundField DataField="RequestedDate" HeaderText="Request Date" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="UnloadingDate" HeaderText="Unloading Date" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="ReportingDate" HeaderText="Reporting Date" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="ContReturnDate" HeaderText="Cont Return Date" DataFormatString="{0:dd/MM/yyyy}" />
                </Columns>
            </asp:GridView>
        </div>
        <asp:SqlDataSource ID="DataSourceTransportJobDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetConsolidateJobDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="ConsolidateID" SessionField="TRConsolidateId" />
            </SelectParameters>
        </asp:SqlDataSource>
    </fieldset>
    <fieldset>
        <legend>Vehicle Rate Detail</legend>
        <div style="width: 1360px; overflow-x: scroll">
            <asp:GridView ID="GridViewVehicle" runat="server" AutoGenerateColumns="False" CssClass="table"
                Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                DataSourceID="DataSourceRate" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="true"
                PageSize="20" OnRowCommand="GridViewVehicle_RowCommand" OnRowDataBound="GridViewVehicle_RowDataBound"
                OnRowEditing="GridViewVehicle_RowEditing" OnRowUpdating="GridViewVehicle_RowUpdating" OnRowCancelingEdit="GridViewVehicle_RowCancelingEdit"
                FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="TRRefNo" HeaderText="Ref No" ReadOnly="true" Visible="false" />
                    <asp:BoundField DataField="JobRefNo" HeaderText="Job No" ReadOnly="true" Visible="false" />
                    <%--<asp:TemplateField HeaderText="Vehicle No">
                        <ItemTemplate>
                            <asp:Label ID="lblVehicleNo" runat="server" Text='<%#Bind("VehicleNo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" ReadOnly="true" />
                    <asp:BoundField DataField="TransporterName" HeaderText="Transporter" ReadOnly="true" />
                    <asp:BoundField DataField="MarketBillingRate" HeaderText="Market Rate" SortExpression="MarketBillingRate" ReadOnly="true" />
                    <asp:BoundField DataField="Rate" HeaderText="Freight Rate" SortExpression="Rate" ReadOnly="true" />
                    <asp:BoundField DataField="SavingAmt" HeaderText="Saving Rate" SortExpression="SavingAmt" ReadOnly="true" />
                    <asp:BoundField DataField="Advance" HeaderText="Advance" SortExpression="Advance" ReadOnly="true" Visible="false" />
                    <asp:BoundField DataField="AdvanceAmount" HeaderText="Advance Amt" SortExpression="AdvanceAmount" ReadOnly="true" />
                    <asp:BoundField DataField="DetentionAmount" HeaderText="Detention Amt" SortExpression="DetentionAmount" ReadOnly="true" />
                    <asp:BoundField DataField="VaraiExpense" HeaderText="Varai Exp" SortExpression="VaraiExpense" ReadOnly="true" />
                    <asp:BoundField DataField="EmptyContRecptCharges" HeaderText="Empty Cont Charges" SortExpression="EmptyContRecptCharges" ReadOnly="true" />
                    <asp:BoundField DataField="TollCharges" HeaderText="Toll Charges" SortExpression="TollCharges" ReadOnly="true" />
                    <asp:BoundField DataField="OtherCharges" HeaderText="Other Charges" SortExpression="OtherCharges" ReadOnly="true" />
                    <asp:BoundField DataField="VehicleTypeName" HeaderText="Type" ReadOnly="true" />
                    <asp:BoundField DataField="LocationFrom" HeaderText="Delivery From" ReadOnly="true" />
                    <asp:BoundField DataField="DeliveryPoint" HeaderText="Delivery Point" ReadOnly="true" />
                    <asp:BoundField DataField="LRNo" HeaderText="LR No" SortExpression="LRNo" ReadOnly="true" Visible="true" />
                    <asp:BoundField DataField="LRDate" HeaderText="LR Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LRDate" ReadOnly="true" />
                    <asp:BoundField DataField="ChallanNo" HeaderText="Challan No" SortExpression="ChallanNo" ReadOnly="true" />
                    <asp:BoundField DataField="ChallanDate" HeaderText="Challan Date" SortExpression="ChallanDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="UnloadingDate" HeaderText="Unloading Date" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                    <%--<asp:BoundField DataField="Instruction" HeaderText="Billing Intruction" SortExpression="Instruction" ReadOnly="true" />--%>
                </Columns>
            </asp:GridView>
        </div>
    </fieldset>
    <fieldset>
        <legend>Fund Payment History</legend>
        <div>
            <asp:GridView ID="gvGetPaymentHistory" runat="server" AutoGenerateColumns="False" CssClass="table"
                Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                DataSourceID="DataSourcePaymentHistory" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false"
                PageSize="20" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="JobRefNo" DataField="JobRefNo" />
                    <asp:BoundField HeaderText="Expense Type" DataField="ExpenseTypeName" />
                    <asp:BoundField HeaderText="Payment Type" DataField="PaymentTypeName" />
                    <asp:BoundField HeaderText="Amount" DataField="Amount" />
                    <asp:BoundField HeaderText="Payment Type" DataField="PaymentTypeName" />
                    <asp:BoundField HeaderText="Paid To" DataField="PaidTo" />
                    <asp:BoundField HeaderText="Transporter" DataField="Transporter" />
                    <asp:BoundField HeaderText="Vehicle No" DataField="VehicleNo" />
                    <asp:BoundField HeaderText="CreatedBy" DataField="CreatedBy" />
                    <asp:BoundField HeaderText="Created Date" DataField="CreatedOn" DataFormatString="{0:dd/MM/yyyy HH:mm:tt}" />
                </Columns>
            </asp:GridView>
        </div>
        <div>
            <asp:SqlDataSource ID="DataSourcePaymentHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="TR_GetPaymentDetails" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </fieldset>
    <fieldset id="fsRejectHistory" runat="server">
        <legend>Bill Rejection History</legend>
        <div style="width: 1360px; overflow-x: scroll">
            <asp:GridView ID="gvBillRejectHistory" runat="server" AutoGenerateColumns="False" CssClass="table" Width="100%"
                AlternatingRowStyle-CssClass="alt" DataKeyNames="lId" DataSourceID="DataSourceBillRejectHistory" CellPadding="4">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="BillNumber" HeaderText="Bill No" ReadOnly="true" />
                    <asp:BoundField DataField="Remark" HeaderText="Remark" ReadOnly="true" />
                    <asp:BoundField DataField="RejectedBy" HeaderText="Rejected By" ReadOnly="true" />
                    <asp:BoundField DataField="RejectedDate" HeaderText="Rejected Date" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                </Columns>
            </asp:GridView>
        </div>
    </fieldset>
    <fieldset>
        <legend>Billing Detail</legend>
        <div>
            <asp:HiddenField ID="hdnFreightAmt" runat="server" Value="0" />
            <asp:HiddenField ID="hdnDetentionAmt" runat="server" Value="0" />
            <asp:HiddenField ID="hdnVaraiAmt" runat="server" Value="0" />
            <asp:HiddenField ID="hdnEmptyContReturnAmt" runat="server" Value="0" />
            <asp:HiddenField ID="hdnTollCharges" runat="server" Value="0" />
            <asp:HiddenField ID="hdnOtherCharges" runat="server" Value="0" />
            <asp:HiddenField ID="hdnAdvanceAmt" runat="server" Value="0" />
            <asp:HiddenField ID="hdnTotalAmount" runat="server" Value="0" />
            <asp:HiddenField ID="hdnSavingAmt" runat="server" Value="0" />
            <asp:HiddenField ID="hdnMarketRate" runat="server" Value="0" />

        </div>
        <div align="center">
            <b><span style="color: red"><u>NOTE: ** - Amount is more than the approved amount </u></span></b>
            <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" />
        </div>
        <div class="m clear">
            <asp:Button ID="btnBillSubmit" Text="Update" runat="server" CausesValidation="true" ValidationGroup="BillRequired" OnClick="btnBillSubmit_Click" />
            <asp:Button ID="btnCancelBill" runat="server" Text="Cancel" OnClick="btnCancelBill_Click" />
        </div>
        <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
            <tr>
                <td>Transporter
                    <asp:RequiredFieldValidator ID="RFVTransporter" runat="server" ControlToValidate="ddTransporter" SetFocusOnError="true"
                        InitialValue="0" Text="*" ErrorMessage="Please Select Transporter" Display="Dynamic" ValidationGroup="BillRequired"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:DropDownList ID="ddTransporter" runat="server" DataSourceID="DSTransporterPlaced"
                        DataTextField="TransporterName" DataValueField="TransporterID" AppendDataBoundItems="true">
                        <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>Bill Submit Date                                                          
                   <cc1:MaskedEditExtender ID="MEditBillSubmitDate" TargetControlID="txtBillSubmitDate" Mask="99/99/9999" MessageValidatorTip="true"
                       MaskType="Date" AutoComplete="false" runat="server">
                   </cc1:MaskedEditExtender>
                    <cc1:MaskedEditValidator ID="MEditValBillSubmitDate" ControlExtender="MEditBillSubmitDate" ControlToValidate="txtBillSubmitDate" IsValidEmpty="false"
                        EmptyValueBlurredText="Required" SetFocusOnError="true" runat="Server" ValidationGroup="BillRequired"></cc1:MaskedEditValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtBillSubmitDate" runat="server" Width="100px" TabIndex="8" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:Image ID="imgBillSubmitDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Bill No
                    <asp:RequiredFieldValidator ID="RFVBillNo" runat="server" ControlToValidate="txtBillNo" SetFocusOnError="true"
                        InitialValue="" Text="*" ErrorMessage="Please Enter Bill No" Display="Dynamic" ValidationGroup="BillRequired"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtBillNo" runat="server" Width="100px" TabIndex="8"></asp:TextBox>
                </td>
                <td>Bill Date
                    <cc1:MaskedEditExtender ID="MEditBillDate" TargetControlID="txtBillDate" Mask="99/99/9999" MessageValidatorTip="true"
                        MaskType="Date" AutoComplete="false" runat="server">
                    </cc1:MaskedEditExtender>
                    <cc1:MaskedEditValidator ID="MEditValBillDate" ControlExtender="MEditBillDate" ControlToValidate="txtBillDate" IsValidEmpty="false"
                        EmptyValueBlurredText="Required" SetFocusOnError="true" runat="Server" ValidationGroup="BillRequired"></cc1:MaskedEditValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtBillDate" runat="server" Width="100px" TabIndex="8" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:Image ID="imgBillDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Freight Amount
                    <asp:RequiredFieldValidator ID="RFVBillAmount" runat="server" ControlToValidate="txtBillAmount" SetFocusOnError="true"
                        InitialValue="" Text="*" ErrorMessage="Please Enter Bill Amount" Display="Dynamic" ValidationGroup="BillRequired"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblFreightValidator" runat="server" Text="**" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtBillAmount" runat="server" Width="100px" TabIndex="8" AutoPostBack="true" OnTextChanged="txtBillAmount_TextChanged"></asp:TextBox>
                </td>
                <td>Detention Amount
                   <asp:Label ID="lblDetentionValidator" runat="server" Text="**" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDetentionAmount" runat="server" Width="100px" TabIndex="8" AutoPostBack="true" OnTextChanged="txtDetentionAmount_TextChanged"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Varai Exp
                    <asp:Label ID="lblVaraiValidator" runat="server" Text="**" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtVaraiExp" runat="server" Width="100px" TabIndex="8" AutoPostBack="true" OnTextChanged="txtVaraiExp_TextChanged"></asp:TextBox>
                </td>
                <td>Empty Cont Rcpt Charges
                    <asp:Label ID="lblEmptyContValidator" runat="server" Text="**" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEmptyContCharges" runat="server" Width="100px" TabIndex="8" AutoPostBack="true" OnTextChanged="txtEmptyContCharges_TextChanged"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Toll Charges
                    <asp:Label ID="lblTollChargesValidator" runat="server" Text="**" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtTollCharges" runat="server" Width="100px" TabIndex="9" AutoPostBack="true" OnTextChanged="txtTollCharges_TextChanged"></asp:TextBox>
                </td>
                <td>Other Charges
                    <asp:Label ID="lblOtherChargesValidator" runat="server" Text="**" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtOtherCharges" runat="server" Width="100px" TabIndex="10" AutoPostBack="true" OnTextChanged="txtOtherCharges_TextChanged"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Total Amount
                    <asp:RequiredFieldValidator ID="RFVTotalAmount" runat="server" ControlToValidate="txtTotalAmount" SetFocusOnError="true"
                        InitialValue="" Text="*" ErrorMessage="Please Enter Total Amount" Display="Dynamic" ValidationGroup="BillRequired"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtTotalAmount" runat="server" Width="100px" TabIndex="8"></asp:TextBox>
                </td>
                <td>Bill A/C Person Name
                </td>
                <td>
                    <asp:TextBox ID="txtBillingEmpoyee" runat="server" Width="100px" TabIndex="8"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Justification
                    <asp:RequiredFieldValidator ID="rfvJustification" runat="server" ControlToValidate="txtJustification" SetFocusOnError="true"
                        InitialValue="" Text="*" ErrorMessage="Bill Amount is not matching with approved amount, please enter justification." Display="Dynamic" ValidationGroup="BillRequired"></asp:RequiredFieldValidator>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtJustification" runat="server" TabIndex="9" TextMode="MultiLine" Rows="3" Width="500px"></asp:TextBox>
                </td>
            </tr>

        </table>
        <div class="m clear">
            <div>
                <asp:GridView ID="GridViewBillDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                    Width="100%" DataKeyNames="lId" DataSourceID="DataSourceBillDetail" CellPadding="4" Visible="false">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Ref No" DataField="TRRefNo" />
                        <asp:BoundField HeaderText="Job No" DataField="JobRefNo" />
                        <asp:BoundField HeaderText="Transporter" DataField="Transporter" />
                        <asp:BoundField HeaderText="Bill Number" DataField="BillNumber" />
                        <asp:BoundField HeaderText="Bill Submit Date" DataField="BillSubmitDate" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField HeaderText="Bill Date" DataField="BillDate" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField HeaderText="Bill Amount" DataField="BillAmount" />
                        <asp:BoundField HeaderText="Detention" DataField="DetentionAmount" />
                        <asp:BoundField HeaderText="Varai" DataField="VaraiAmount" />
                        <asp:BoundField HeaderText="Empty Cont Charges" DataField="EmptyContRcptCharges" />
                        <asp:BoundField HeaderText="Total" DataField="TotalAmount" />
                        <asp:BoundField HeaderText="Justification" DataField="Justification" ItemStyle-Width="40%" />
                        <asp:BoundField HeaderText="Billing Person" DataField="BillPersonName" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </fieldset>
    <div id="divDataSource">
        <asp:SqlDataSource ID="DataSourceRate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetTransRateDetailByTP" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
                <asp:SessionParameter Name="TransporterId" SessionField="TransporterId" />
            </SelectParameters>
        </asp:SqlDataSource>

        <%--        <asp:SqlDataSource ID="DataSourceRate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetTransRateDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqID" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>--%>
        <asp:SqlDataSource ID="DSTransporterPlaced" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetTransporterPlaced" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransRequestId" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceBillDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetTransBillDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqID" SessionField="TRId" />
                <asp:SessionParameter Name="TransporterID" SessionField="TransporterId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceBillRejectHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetBillRejectHistory" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransBillId" SessionField="TransBillId" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>

</asp:Content>

