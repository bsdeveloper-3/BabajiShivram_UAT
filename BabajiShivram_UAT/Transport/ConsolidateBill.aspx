<%@ Page Title="Add Bill Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ConsolidateBill.aspx.cs"
    Inherits="Transport_ConsolidateBill" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>

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
                    if (Page_ClientValidate()) {
                        if (confirm('Bill Amount is not matching with approved amount, are you sure to send Bill for Approval?')) {
                            return true;
                        }
                    }
                }
                else {
                    Page_ClientValidate("BillRequired");
                }
            }
        </script>
    </div>
    <div>
        <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" />
        <asp:HiddenField ID="hdnPageValid" runat="server" />
    </div>
    <fieldset id="fsConsolidateJobs" runat="server">
        <legend>Consolidate Job Detail</legend>
        <div style="width: 1360px; overflow: auto">
            <asp:GridView ID="gvTransportJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="TransReqId" PagerStyle-CssClass="pgr"
                AllowPaging="True" AllowSorting="True" Width="100%" PageSize="20" PagerSettings-Position="TopAndBottom" DataSourceID="DataSourceTransportJobDetail"
                OnRowDataBound="gvTransportJobDetail_RowDataBound">
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
                    <asp:BoundField DataField="PlanningDate" HeaderText="Planning Date" DataFormatString="{0:dd/MM/yyyy}" Visible="false" />
                    <asp:BoundField DataField="LRNo" HeaderText="LR No" Visible="false" />
                    <asp:BoundField DataField="LRDate" HeaderText="LR Date" DataFormatString="{0:dd/MM/yyyy}" Visible="false" />
                    <asp:BoundField DataField="BabajiChallanNo" HeaderText="Challan No" Visible="false" />
                    <asp:BoundField DataField="BabajiChallanDate" HeaderText="Challan Date" DataFormatString="{0:dd/MM/yyyy}" Visible="false" />
                    <asp:BoundField DataField="RequestedBy" HeaderText="Requested By" />
                    <asp:BoundField DataField="RequestedDate" HeaderText="Request Date" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="UnloadingDate" HeaderText="Unloading Date" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="ReportingDate" HeaderText="Reporting Date" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="ContReturnDate" HeaderText="Cont Return Date" DataFormatString="{0:dd/MM/yyyy}" />
                </Columns>
                <PagerTemplate>
                    <asp:GridViewPager ID="GridViewPager1" runat="server" />
                </PagerTemplate>
            </asp:GridView>
        </div>
        <asp:SqlDataSource ID="DataSourceTransportJobDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetConsolidateJobDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <%--<asp:SessionParameter Name="ConsolidateID" SessionField="TRConsolidateId" />--%>
                 <asp:SessionParameter Name="TransReqId" SessionField="TransReqId" />
            </SelectParameters>
        </asp:SqlDataSource>
    </fieldset>

    <fieldset>
        <legend>Vehicle Rate Detail</legend>
        <div style="width: 1360px; overflow-x: auto">
            <asp:GridView ID="GridViewVehicle" runat="server" AutoGenerateColumns="False" CssClass="table"
                Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                DataSourceID="DataSourceRate" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false"
                PageSize="20" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="TRRefNo" HeaderText="Ref No" ReadOnly="true" Visible="false" />
                    <asp:BoundField DataField="JobRefNo" HeaderText="Job No" ReadOnly="true" Visible="false" />
                    <asp:BoundField DataField="TransitType" HeaderText="Delivery To" ReadOnly="true" />
                    <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" ReadOnly="true" />
                    <asp:BoundField DataField="TransporterName" HeaderText="Transporter" ReadOnly="true" />
                    <asp:BoundField DataField="MarketBillingRate" HeaderText="Market Rate" SortExpression="MarketBillingRate" ReadOnly="true" Visible="false" />
                    <asp:BoundField DataField="Rate" HeaderText="Freight Rate" SortExpression="Rate" ReadOnly="true" />
                    <asp:BoundField DataField="Advance" HeaderText="Advance" SortExpression="Advance" ReadOnly="true" Visible="false" />
                    <asp:BoundField DataField="AdvanceAmount" HeaderText="Advance Amt" SortExpression="AdvanceAmount" ReadOnly="true" />
                    <asp:BoundField DataField="DetentionAmount" HeaderText="Detention Amt" SortExpression="DetentionAmount" ReadOnly="true" />
                    <asp:BoundField DataField="VaraiExpense" HeaderText="Varai Exp" SortExpression="VaraiExpense" ReadOnly="true" />
                    <asp:BoundField DataField="EmptyContRecptCharges" HeaderText="Empty Cont Charges" SortExpression="EmptyContRecptCharges" ReadOnly="true" />
                    <asp:BoundField DataField="TotalAmount" HeaderText="Total" SortExpression="TotalAmount" ReadOnly="true" />
                    <asp:BoundField DataField="TollCharges" HeaderText="Toll Charges" SortExpression="TollCharges" ReadOnly="true" />
                    <asp:BoundField DataField="OtherCharges" HeaderText="Other Charges" SortExpression="OtherCharges" ReadOnly="true" />
                    <asp:BoundField DataField="VehicleTypeName" HeaderText="Type" ReadOnly="true" />
                    <asp:BoundField DataField="LocationFrom" HeaderText="Delivery From" ReadOnly="true" />
                    <asp:BoundField DataField="DeliveryPoint" HeaderText="Delivery Point" ReadOnly="true" />
                    <asp:BoundField DataField="City" HeaderText="City" ReadOnly="true" />
                    <asp:BoundField DataField="LRNo" HeaderText="LR No" SortExpression="LRNo" ReadOnly="true" />
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
        <legend>Selling Rate Detail</legend>
        <div style="width: 1360px; overflow-x: auto">
            <asp:GridView ID="gvSellingDetail" runat="server" AutoGenerateColumns="False" CssClass="table" Width="100%" AlternatingRowStyle-CssClass="alt"
                PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceSellingRate" OnRowCommand="gvSellingDetail_RowCommand"
                OnPreRender="gvSellingDetail_PreRender" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" ReadOnly="true" />
                    <asp:BoundField DataField="TransporterName" HeaderText="Transporter" ReadOnly="true" />
                    <asp:BoundField DataField="SellDetentionAmount" HeaderText="Detention Amount" ReadOnly="true" />
                    <asp:BoundField DataField="SellVaraiExpense" HeaderText="Varai Amount No" ReadOnly="true" />
                    <asp:BoundField DataField="SellEmptyContRecptCharges" HeaderText="Empty Cont Amount" ReadOnly="true" />
                    <asp:BoundField DataField="SellTollCharges" HeaderText="Toll Amount No" ReadOnly="true" />
                    <asp:BoundField DataField="SellOtherCharges" HeaderText="Other Amount" ReadOnly="true" />
                    <asp:TemplateField HeaderText="Detention Doc">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgbtnDetentionCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download detention copy."
                                CommandName="DetentionCopy" CommandArgument='<%#Eval("DetentionDoc")%>' Visible='<%# DecideHere((string)Eval("DetentionDoc")) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Varai Doc">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgbtnVaraiCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download varai copy."
                                CommandName="varaiCopy" CommandArgument='<%#Eval("VaraiDoc")%>' Visible='<%# DecideHere((string)Eval("VaraiDoc")) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Empty Cont Doc">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgbtnemptyContCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download Empty cont copy."
                                CommandName="EmptyContCopy" CommandArgument='<%#Eval("EmptyContDoc")%>' Visible='<%# DecideHere((string)Eval("EmptyContDoc")) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Toll Doc">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgbtnTollCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download toll copy."
                                CommandName="TollCopy" CommandArgument='<%#Eval("TollDoc")%>' Visible='<%# DecideHere((string)Eval("TollDoc")) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Other Doc">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgbtnOtherCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download other copy."
                                CommandName="OtherCopy" CommandArgument='<%#Eval("OtherDoc")%>' Visible='<%# DecideHere((string)Eval("OtherDoc")) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="SellFreighRate" HeaderText="Selling Freight rate" ReadOnly="true" />
                    <asp:TemplateField HeaderText="Email Approval Copy">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgbtnEmailApprovalCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download Email Approval copy."
                                CommandName="EmailApprovalCopy" CommandArgument='<%#Eval("EmailAttachment")%>' Visible='<%# DecideHere((string)Eval("EmailAttachment")) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Contract Copy">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgbtnContractCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download contract copy."
                                CommandName="ContractCopy" CommandArgument='<%#Eval("ContractAttachment")%>' Visible='<%# DecideHere((string)Eval("ContractAttachment")) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField DataField="Instruction" HeaderText="Billing Instruction" ReadOnly="true" />--%>
                    <asp:BoundField DataField="Remark" HeaderText="Other Remark" ReadOnly="true" />
                    <asp:BoundField DataField="SellDetail" HeaderText="Charge to Party" ReadOnly="true" />
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <div class="m clear">
            <asp:HiddenField ID="hdnJobId" runat="server" Value="0" />
            <asp:HiddenField ID="hdnIsUpdate" runat="server" Value="0" />
            <asp:HiddenField ID="hdnFilePath" runat="server" />
        </div>
        <div>
        </div>
    </fieldset>

    <fieldset>
        <legend>Fund Payment Detail</legend>
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
                    <asp:SessionParameter Name="TransReqId" SessionField="TransReqId" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </fieldset>
    <fieldset>
        <legend>Fund Payment History</legend>
        <div>
            <asp:GridView ID="gvReqHistory" runat="server" AutoGenerateColumns="False" CssClass="table"
                Width="100%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceRequestHistory"
                CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                    <asp:BoundField DataField="Remark" HeaderText="Remark" />
                    <asp:BoundField DataField="CreatedBy" HeaderText="Updated By" />
                    <asp:BoundField DataField="CreatedDate" HeaderText="Updated Date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss tt}" />
                </Columns>
            </asp:GridView>
        </div>
        <asp:SqlDataSource ID="DataSourceRequestHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetPaymentStatusHistory" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
    </fieldset>
    <fieldset>
        <legend>Billing Detail</legend>
        <div>
            <asp:HiddenField ID="hdnFreightAmt" runat="server" Value="0" />
            <asp:HiddenField ID="hdnDetentionAmt" runat="server" Value="0" />
            <asp:HiddenField ID="hdnVaraiAmt" runat="server" Value="0" />
            <asp:HiddenField ID="hdnEmptyContReturnAmt" runat="server" Value="0" />
            <asp:HiddenField ID="hdnAdvanceAmt" runat="server" Value="0" />
            <asp:HiddenField ID="hdnTotalAmount" runat="server" Value="0" />
            <asp:HiddenField ID="hdnTollCharges" runat="server" Value="0" />
            <asp:HiddenField ID="hdnOtherCharges" runat="server" Value="0" />
        </div>
        <div align="center">
            <b><span style="color: red"><u>NOTE: ** - Amount is more than the approved amount </u></span></b>
            <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" />
        </div>
        <div class="m clear">
            <asp:Button ID="btnBillSubmit" Text="Save" runat="server" CausesValidation="true" ValidationGroup="BillRequired"
                TabIndex="14" OnClientClick="return ConfirmMessage();" OnClick="btnBillSubmit_Click" />
            <asp:Button ID="btnCancelBill" TabIndex="15" runat="server" Text="Cancel" OnClick="btnCancelBill_Click" />
        </div>
        <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
            <tr>
                <td>Transporter
                    <asp:RequiredFieldValidator ID="RFVTransporter" runat="server" ControlToValidate="ddTransporter" SetFocusOnError="true"
                        InitialValue="0" Text="*" ErrorMessage="Please Select Transporter" Display="Dynamic" ValidationGroup="BillRequired"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:DropDownList ID="ddTransporter" runat="server" DataSourceID="DSTransporterPlaced" TabIndex="1"
                        DataTextField="TransporterName" DataValueField="TransporterID" AppendDataBoundItems="true">
                        <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>Bill Submit Date                                                          
                   <cc1:MaskedEditExtender ID="MEditBillSubmitDate" TargetControlID="txtBillSubmitDate" Mask="99/99/9999" MessageValidatorTip="true"
                       MaskType="Date" AutoComplete="false" runat="server">
                   </cc1:MaskedEditExtender>
                    <cc1:MaskedEditValidator ID="MEditValBillSubmitDate" ControlExtender="MEditBillSubmitDate" ControlToValidate="txtBillSubmitDate" IsValidEmpty="false"
                        EmptyValueBlurredText="*" EmptyValueMessage="Please Enter Bill Submit Date"
                        SetFocusOnError="true" runat="Server" ValidationGroup="BillRequired"></cc1:MaskedEditValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtBillSubmitDate" runat="server" Width="100px" TabIndex="2" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:Image ID="imgBillSubmitDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Bill No
                    <asp:RequiredFieldValidator ID="RFVBillNo" runat="server" ControlToValidate="txtBillNo" SetFocusOnError="true"
                        InitialValue="" Text="*" ErrorMessage="Please Enter Bill No" Display="Dynamic" ValidationGroup="BillRequired"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtBillNo" runat="server" Width="100px" TabIndex="3"></asp:TextBox>
                </td>
                <td>Bill Date
                    <cc1:MaskedEditExtender ID="MEditBillDate" TargetControlID="txtBillDate" Mask="99/99/9999" MessageValidatorTip="true"
                        MaskType="Date" AutoComplete="false" runat="server">
                    </cc1:MaskedEditExtender>
                    <cc1:MaskedEditValidator ID="MEditValBillDate" ControlExtender="MEditBillDate" ControlToValidate="txtBillDate" IsValidEmpty="false"
                        EmptyValueBlurredText="*" EmptyValueMessage="Please Enter Bill Date"
                        SetFocusOnError="true" runat="Server" ValidationGroup="BillRequired"></cc1:MaskedEditValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtBillDate" runat="server" Width="100px" TabIndex="4" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:Image ID="imgBillDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Freight Rate
                    <asp:RequiredFieldValidator ID="RFVBillAmount" runat="server" ControlToValidate="txtBillAmount" SetFocusOnError="true"
                        InitialValue="" Text="*" ErrorMessage="Please Enter Bill Amount" Display="Dynamic" ValidationGroup="BillRequired"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblFreightValidator" runat="server" Text="**" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtBillAmount" runat="server" Width="100px" TabIndex="5" AutoPostBack="true" OnTextChanged="txtBillAmount_TextChanged"></asp:TextBox>
                </td>
                <td>Detention Amount
                   <asp:Label ID="lblDetentionValidator" runat="server" Text="**" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDetentionAmount" runat="server" Width="100px" TabIndex="6" AutoPostBack="true" OnTextChanged="txtDetentionAmount_TextChanged"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Varai Exp
                    <asp:Label ID="lblVaraiValidator" runat="server" Text="**" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtVaraiExp" runat="server" Width="100px" TabIndex="7" AutoPostBack="true" OnTextChanged="txtVaraiExp_TextChanged"></asp:TextBox>
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
                </td>
                <td>
                    <asp:TextBox ID="txtTotalAmount" runat="server" Width="100px" TabIndex="11"></asp:TextBox>
                </td>
                <td>Bill A/C Person Name
                    <asp:RequiredFieldValidator ID="rfvBillingPerson" runat="server" ControlToValidate="txtBillingEmpoyee" SetFocusOnError="true"
                        InitialValue="" Text="*" ErrorMessage="Please Enter Bill A/C Person Name." Display="Dynamic" ValidationGroup="BillRequired"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtBillingEmpoyee" runat="server" Width="100px" TabIndex="12"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Justification
                    <asp:RequiredFieldValidator ID="rfvJustification" runat="server" ControlToValidate="txtJustification" SetFocusOnError="true"
                        InitialValue="" Text="*" ErrorMessage="Please Enter Justification." Display="Dynamic" ValidationGroup="BillRequired"></asp:RequiredFieldValidator>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtJustification" runat="server" TabIndex="13" TextMode="MultiLine" Rows="3" Width="980px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Document
                <asp:RequiredFieldValidator ID="RFVAttach" runat="server" ControlToValidate="fuDocument"
                    Display="Dynamic" ValidationGroup="BillRequired" SetFocusOnError="true" Text="*"
                    ErrorMessage="Attach File For Upload."></asp:RequiredFieldValidator>
                </td>
                <td colspan="3">
                    <asp:FileUpload ID="fuDocument" runat="server" /><%--<asp:Button ID="btnUpload" runat="server"
                    Text="Upload" OnClick="btnFileUpload_Click" ValidationGroup="validateDocument" />--%>
                </td>
            </tr>
        </table>
        <div class="m clear">
            <div>
                <asp:GridView ID="GridViewBillDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                    Width="100%" DataKeyNames="lId" DataSourceID="DataSourceBillDetail" CellPadding="4">
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
                        <asp:BoundField HeaderText="Toll Charges" DataField="TollCharges" />
                        <asp:BoundField HeaderText="Other Charges" DataField="OtherCharges" />
                        <asp:BoundField HeaderText="Total" DataField="TotalAmount" />
                        <asp:BoundField HeaderText="Billing Person" DataField="BillPersonName" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </fieldset>
    <div id="divDataSource">
        <asp:SqlDataSource ID="DataSourceRate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetTransRateDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqID" SessionField="TransReqId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DSTransporterPlaced" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetTransporterPlaced" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransRequestId" SessionField="TransReqId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceBillDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetTransBillDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqID" SessionField="TransReqId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceSellingRate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetVehicleSellingDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqID" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>

</asp:Content>

