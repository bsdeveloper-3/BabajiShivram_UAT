<%@ Page Title="Transport Bill HOD Approval" Language="C#" MasterPageFile="~/MasterPage.master" 
    AutoEventWireup="true" CodeFile="TransBillHoD.aspx.cs" Inherits="BSCCPLTransport_TransBillHoD" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <style type="text/css">
        .form-control {
            border-top: 1px solid black;
            border-right: 1px solid black;
            border-bottom: 1px solid black;
            border-left: 1px solid black;
            border-radius: 3px;
            font-size: 9pt;
            padding: 2px 5px;
            margin-right: 5px;
            margin-top: 5px;
            font-family: Arial;
            padding-top: 5px;
            padding-bottom: 5px;
        }
    </style>
    <div>
        <asp:ValidationSummary ID="csRequiredFields" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="vgRequired" CssClass="errorMsg" />
        <asp:ValidationSummary ID="vsReqStatus" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="ReqStatus" CssClass="errorMsg" />
        <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" CausesValidation="false" />
        <asp:ValidationSummary ID="vsSRRequired" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="SRRequired" CssClass="errorMsg" />
    </div>
    <div>
        <div align="center">
            <asp:Label ID="lblError" runat="server"></asp:Label>
        </div>
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
                    <asp:BoundField DataField="AdvanceAmount" HeaderText="Advance" SortExpression="AdvanceAmount" ReadOnly="true" />
                    <asp:BoundField DataField="DetentionAmount" HeaderText="Detention" SortExpression="DetentionAmount" ReadOnly="true" />
                    <asp:BoundField DataField="VaraiExpense" HeaderText="Varai" SortExpression="VaraiExpense" ReadOnly="true" />
                    <asp:BoundField DataField="EmptyContRecptCharges" HeaderText="Empty" SortExpression="EmptyContRecptCharges" ReadOnly="true" />
                    <asp:BoundField DataField="TotalAmount" HeaderText="Total" SortExpression="TotalAmount" ReadOnly="true" />
                    <asp:BoundField DataField="TollCharges" HeaderText="Toll" SortExpression="TollCharges" ReadOnly="true" />
                    <asp:BoundField DataField="OtherCharges" HeaderText="Union" SortExpression="OtherCharges" ReadOnly="true" />
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
                    <asp:BoundField DataField="SellDetentionAmount" HeaderText="Detention" ReadOnly="true" />
                    <asp:BoundField DataField="SellVaraiExpense" HeaderText="Varai Amount" ReadOnly="true" />
                    <asp:BoundField DataField="SellEmptyContRecptCharges" HeaderText="Empty Cont" ReadOnly="true" />
                    <asp:BoundField DataField="SellTollCharges" HeaderText="Toll" ReadOnly="true" />
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
        <legend>Bill Status Detail</legend>
        <div>
            <asp:HiddenField ID="hdnTransBillId" runat="server" Value="0" />
            <div class="m clear">
                <asp:Label ID="lblError_BillStatus" runat="server" Font-Bold="true"></asp:Label>
                <asp:Button ID="btnApproveBill" runat="server" OnClick="btnApproveBill_Click" Text="Approve Bill & Send to Accounts" CausesValidation="true" ValidationGroup="ReqStatus" />
                <asp:Button ID="btnRejectBill" runat="server" OnClick="btnRejectBill_Click" Text="Reject Bill Detail" CausesValidation="true" ValidationGroup="ReqStatus" />
            </div>
            <table border="0" cellpadding="0" cellspacing="0" width="99%" bgcolor="white">
                <tr>
                    <td>Remark
                        <asp:RequiredFieldValidator ID="rfvHoldReason" runat="server" ControlToValidate="txtRemark" SetFocusOnError="true"
                            Display="Dynamic" ErrorMessage="Please Enter Hold Reason." ValidationGroup="ReqStatus"></asp:RequiredFieldValidator>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtRemark" runat="server" TabIndex="4" Width="900px" TextMode="MultiLine" Rows="2"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div>
            <asp:GridView ID="gvBillStatusHistory" runat="server" AutoGenerateColumns="False" CssClass="table"
                Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                DataSourceID="DataSourceBillStatusHistory" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false"
                PageSize="20" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Status" DataField="StatusName" />
                    <asp:BoundField HeaderText="Remark" DataField="HoldReason" />
                    <asp:BoundField HeaderText="User" DataField="CreatedBy" />
                    <asp:BoundField HeaderText="Date" DataField="CreatedDate" DataFormatString="{0:dd/MM/yyyy HH:mm:tt}" />
                </Columns>
            </asp:GridView>
        </div>
    </fieldset>
    <fieldset>
        <legend>Bill Detail</legend>
        <div>
            <%--<asp:Label ID="lbltest" runat ="server" Text="ABC"></asp:Label>--%>
            <asp:GridView ID="gvBillDetail" runat="server" AutoGenerateColumns="False" CssClass="table" 
                Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                DataSourceID="DataSourceBillDetail" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="true"
                PageSize="20" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False"
                FooterStyle-BackColor="#CCCCFF" OnRowCommand="gvBillDetail_RowCommand" >
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Ref No" DataField="TRRefNo" />
                    <asp:BoundField HeaderText="Job No" DataField="JobRefNo" />
                    <asp:BoundField HeaderText="Transporter" DataField="Transporter" />
                    <asp:BoundField HeaderText="Bill No" DataField="BillNumber" />
                    <asp:BoundField HeaderText="Bill Submit Date" DataField="BillSubmitDate" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField HeaderText="Bill Date" DataField="BillDate" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField HeaderText="Bill Amount" DataField="BillAmount" />
                    <asp:BoundField HeaderText="Detention" DataField="DetentionAmount" />
                    <asp:BoundField HeaderText="Varai" DataField="VaraiAmount" />
                    <asp:BoundField HeaderText="Empty" DataField="EmptyContRcptCharges" />
                    <asp:BoundField HeaderText="Total" DataField="TotalAmount" />
                    <asp:BoundField HeaderText="Billing Person" DataField="BillPersonName" />
                    <asp:TemplateField HeaderText="Document">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDownload" CommandName="Detail" runat="server" Text='<%#Eval("DocName")%>'
                                 CommandArgument='<%#Eval("DocPath")%>' CausesValidation="false"
                                ></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </fieldset>

    <fieldset>
        <legend>Sales Bill Details</legend>
        <asp:GridView ID="gvSalesBillDetail" runat="server" AutoGenerateColumns="False"
            CssClass="table" Width="99%" PagerStyle-CssClass="pgr"
            DataKeyNames="JobId,Billid" DataSourceID="DataSourceBillJob" CellPadding="4"
            AllowPaging="True" AllowSorting="True" PageSize="40" OnRowCommand="gvSalesBillDetail_RowCommand"
                            OnRowDataBound="gvSalesBillDetail_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="BJV No">
                    <ItemTemplate>
                        <asp:Label ID="lblBJVNo" runat="server" Text='<%#Eval("BJVNo")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Bill Number">
                    <ItemTemplate>
                        <asp:Label ID="lblBillNumber" runat="server" Text='<%#Eval("INVNO")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Bill Date" DataField="INVDATE" DataFormatString="{0:dd/MM/yyyy}" />
                <%--<asp:BoundField HeaderText="Bill Amount" DataField="INVAMOUNT" />--%>
                <asp:BoundField HeaderText="Adjustment Amount" DataField="ADJAmount" />
                <asp:BoundField HeaderText="Adjustment Date" DataField="ADJDate" DataFormatString="{0:dd/MM/yyyy}"/>
                <%--<asp:TemplateField HeaderText="View Bill">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBillView" runat="server" Text="View" CommandName="View" CommandArgument='<%#Eval("BillId")%>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>--%>
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="DataSourceBillJob" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="BL_GetPendingBillDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="hdnJobId" Name="JobId" PropertyName="Value" ConvertEmptyStringToNull="true"  />
                <asp:Parameter Name="ModuleId" DefaultValue="1"/>
            </SelectParameters>
        </asp:SqlDataSource>
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
                    <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
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
    
    <div id="divDataSource">
        <asp:SqlDataSource ID="DataSourceBillStatusHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetBillReceivedDetailHistory" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceStatus" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetBillReceiveStatusMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceRate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TRS_GetTransRateDetailByTP" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqID" SessionField="TRId" />
                <asp:SessionParameter Name="TransporterId" SessionField="TransporterId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceBillDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetTransBillDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqID" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceBillReceived" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetBillReceivedByTransReqId" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqID" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        
        <asp:SqlDataSource ID="DataSourceVehicle" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetVehiclesForDelivery" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="hdnJobId" Name="JobId" PropertyName="Value" />
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

