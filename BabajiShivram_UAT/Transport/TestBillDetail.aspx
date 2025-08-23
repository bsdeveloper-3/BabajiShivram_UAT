<%@ Page Title="Billing Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="TestBillDetail.aspx.cs" Inherits="Transport_TestBillDetail" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:toolkitscriptmanager runat="server" id="ScriptManager1" scriptmode="Release" />
    <div>
        <cc1:calendarextender id="calBillSubmitDate" runat="server" enabled="True" enableviewstate="False"
            firstdayofweek="Sunday" format="dd/MM/yyyy" popupbuttonid="imgBillSubmitDate"
            popupposition="BottomRight" targetcontrolid="txtBillSubmitDate">
        </cc1:calendarextender>
        <cc1:calendarextender id="calBillDate" runat="server" enabled="True" enableviewstate="False"
            firstdayofweek="Sunday" format="dd/MM/yyyy" popupbuttonid="imgBillDate"
            popupposition="BottomRight" targetcontrolid="txtBillDate">
        </cc1:calendarextender>
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
        <asp:HiddenField ID="hdnPageValid" runat="server" Value="0" />
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
                            <td>
                                PickUp Address
                            </td>
                            <td>
                                 <asp:Label ID="lblPickAdd" runat="server"></asp:Label>
                            </td>
                            <td>
                                Drop Address
                            </td>
                            <td>
                                  <asp:Label ID="lblDropAdd" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Address Details
                            </td>
                            <td>
                                Pincode&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; City&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp State
                            </td>
                            <td> Address Details
                            </td>
                            <td>
                                Pincode&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;City &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp State
                            </td>
                        </tr>
                        <tr>
                            <td>
                                </td>
                            <td>
                                 <asp:Label ID="lblpickPincode" runat="server">  </asp:Label>
                                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
                                  <asp:Label ID="lblpickCity" runat="server"></asp:Label>
                                  &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
                                  <asp:Label ID="lblpickState" runat="server"></asp:Label>
                            </td>
                            <td>
                                </td>
                            <td>
                                  <asp:Label ID="lblDropPincode" runat="server"></asp:Label> 
                                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                                  <asp:Label ID="lblDropCity" runat="server"> </asp:Label> 
                                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
                                  <asp:Label ID="lblDropState" runat="server"></asp:Label>
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
    <fieldset>
        <legend>Vehicle Rate Detail</legend>
        <div style="width: 1360px; overflow-x: scroll">
            <asp:GridView ID="GridViewVehicle" runat="server" AutoGenerateColumns="False" CssClass="table"
                Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                DataSourceID="DataSourceRate" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="true"
                PageSize="20" OnRowDataBound="GridViewVehicle_RowDataBound"
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
                    <asp:BoundField DataField="TransitType" HeaderText="Delivery To" ReadOnly="true" />
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
                    <asp:BoundField DataField="LRNo" HeaderText="LR No" SortExpression="LRNo" ReadOnly="true" Visible="false" />
                    <asp:BoundField DataField="LRDate" HeaderText="LR Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LRDate" ReadOnly="true" Visible="false" />
                    <asp:BoundField DataField="ChallanNo" HeaderText="Challan No" SortExpression="ChallanNo" ReadOnly="true" Visible="false" />
                    <asp:BoundField DataField="ChallanDate" HeaderText="Challan Date" SortExpression="ChallanDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" Visible="false" />
                    <asp:BoundField DataField="UnloadingDate" HeaderText="Unloading Date" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" Visible="false" />
                    <%--<asp:BoundField DataField="Instruction" HeaderText="Billing Intruction" SortExpression="Instruction" ReadOnly="true" />--%>
                </Columns>
            </asp:GridView>
        </div>
    </fieldset>

    <fieldset>
        <legend>Selling Rate Detail</legend>
        <div style="width: 1220px; overflow-x: auto">

            <asp:GridView ID="gvSellDetail" runat="server" AutoGenerateColumns="false" CssClass="table" Width="100%" AlternatingRowStyle-CssClass="alt"
                PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceSellingRate" OnRowCommand="gvSellDetail_RowCommand">
                <Columns>
                    <asp:BoundField DataField="TransporterName" HeaderText="Transporter" ReadOnly="true" />
                    <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" ReadOnly="true" />
                    <asp:BoundField DataField="SellFreighRate" HeaderText="Selling Freight rate" ReadOnly="true" />
                    <asp:BoundField DataField="SellDetentionAmount" HeaderText="Detention Amount" ReadOnly="true" />
                    <asp:BoundField DataField="SellVaraiExpense" HeaderText="Varai Amount No" ReadOnly="true" />
                    <asp:BoundField DataField="SellEmptyContRecptCharges" HeaderText="Empty Cont Amount" ReadOnly="true" />
                    <asp:BoundField DataField="SellTollCharges" HeaderText="Toll Amount No" ReadOnly="true" />
                    <asp:BoundField DataField="SellOtherCharges" HeaderText="Other Amount" ReadOnly="true" />
                    
                    <asp:TemplateField HeaderText="Email Approval Copy">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgbtnEmailApprovalCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download Email Approval copy."
                                CommandName="EmailApprovalCopy" CommandArgument='<%#Eval("EmailAttachment")%>' Visible='<%# DecideHere((string)Eval("EmailAttachment")) %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Contract Copy">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgbtnContractCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download contract copy."
                                CommandName="ContractCopy" CommandArgument='<%#Eval("ContractAttachment")%>' Visible='<%# DecideHere((string)Eval("ContractAttachment")) %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Detention Copy">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgbtnDetentionCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download detention copy."
                                CommandName="DetentionCopy" CommandArgument='<%#Eval("DetentionDoc")%>' Visible='<%# DecideHere((string)Eval("DetentionDoc")) %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Varai Copy">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgbtnVaraiCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download varai copy."
                                CommandName="varaiCopy" CommandArgument='<%#Eval("VaraiDoc")%>' Visible='<%# DecideHere((string)Eval("VaraiDoc")) %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Empty Cont Copy">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgbtnemptyContCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download Empty cont copy."
                                CommandName="EmptyContCopy" CommandArgument='<%#Eval("EmptyContDoc")%>' Visible='<%# DecideHere((string)Eval("EmptyContDoc")) %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Toll Copy">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgbtnTollCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download toll copy."
                                CommandName="TollCopy" CommandArgument='<%#Eval("TollDoc")%>' Visible='<%# DecideHere((string)Eval("TollDoc")) %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Other Copy">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgbtnOtherCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download other copy."
                                CommandName="OtherCopy" CommandArgument='<%#Eval("OtherDoc")%>' Visible='<%# DecideHere((string)Eval("OtherDoc")) %>'/>
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
            <asp:Button ID="btnBillSubmit" Text="Save" runat="server" OnClientClick="return ConfirmMessage();" CausesValidation="true" ValidationGroup="BillRequired"
                TabIndex="14" OnClick="btnBillSubmit_Click" />
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
                        &nbsp;
                    <asp:Button ID="btnAddTransporter" runat="server" Text="Add Transporter" OnClick="btnAddTransporter_Click" />
            </td>
                <td>Bill Submit Date                                                          
                   <cc1:maskededitextender id="MEditBillSubmitDate" targetcontrolid="txtBillSubmitDate" mask="99/99/9999" messagevalidatortip="true"
                       masktype="Date" autocomplete="false" runat="server">
                   </cc1:maskededitextender>
                    <cc1:maskededitvalidator id="MEditValBillSubmitDate" controlextender="MEditBillSubmitDate" controltovalidate="txtBillSubmitDate" isvalidempty="false"
                        invalidvalueblurredmessage="Invalid Date" invalidvaluemessage="Bill Submit Date is invalid" minimumvaluemessage="Invalid Date" maximumvaluemessage="Invalid Date"
                        emptyvalueblurredtext="*" emptyvaluemessage="Please Enter Bill Submit Date"
                        minimumvalue="01/01/2016" setfocusonerror="true" runat="Server" validationgroup="BillRequired"></cc1:maskededitvalidator>
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
                    <cc1:maskededitextender id="MEditBillDate" targetcontrolid="txtBillDate" mask="99/99/9999" messagevalidatortip="true"
                        masktype="Date" autocomplete="false" runat="server">
                    </cc1:maskededitextender>
                    <cc1:maskededitvalidator id="MEditValBillDate" controlextender="MEditBillDate" controltovalidate="txtBillDate" isvalidempty="false"
                        invalidvalueblurredmessage="Invalid Date" invalidvaluemessage="Bill Date is invalid" minimumvaluemessage="Invalid Date" maximumvaluemessage="Invalid Date"
                        emptyvalueblurredtext="*" emptyvaluemessage="Please Enter Bill Date"
                        minimumvalue="01/01/2016" setfocusonerror="true" runat="Server" validationgroup="BillRequired"></cc1:maskededitvalidator>
                </td>
                <td>
                    <asp:TextBox ID="txtBillDate" runat="server" Width="100px" TabIndex="4" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:Image ID="imgBillDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Freight Rate
                    <asp:RequiredFieldValidator ID="RFVBillAmount" runat="server" ControlToValidate="txtBillAmount" SetFocusOnError="true"
                        InitialValue="" Text="*" ErrorMessage="Please Enter Freight Rate" Display="Dynamic" ValidationGroup="BillRequired"></asp:RequiredFieldValidator>
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
                        Text="*" ErrorMessage="Please Enter Justification." Display="Dynamic" ValidationGroup="BillRequired"></asp:RequiredFieldValidator>
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
                    <asp:FileUpload ID="fuDocument" runat="server" />
                    <asp:HiddenField ID="hdnUploadPath" runat="server" />
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
            SelectCommand="TR_GetTransRateDetailByTP" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqId" SessionField="TransReqId" />
                <asp:SessionParameter Name="TransporterId" SessionField="TransporterId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <%--<asp:SqlDataSource ID="DataSourceRate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetTransRateDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqID" SessionField="TransReqId" />
            </SelectParameters>
        </asp:SqlDataSource>--%>
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
                <asp:SessionParameter Name="TransReqID" SessionField="TransReqId" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
</asp:Content>

