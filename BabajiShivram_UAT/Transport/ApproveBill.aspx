<%@ Page Title="Approve Bill" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ApproveBill.aspx.cs" Inherits="Transport_ApproveBill" %>

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
        <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" CausesValidation="false" />
        <asp:Button ID="btnApprovePopup" runat="server" Text="Approve" OnClick="btnApprovePopup_Click" />
        <asp:Button ID="btnRejectPopup" runat="server" Text="Reject" OnClick="btnRejectPopup_Click" />
        &nbsp;
                <asp:TextBox ID="txtRemark" runat="server" CssClass="form-control" Visible="false" placeholder="Enter Remark ..." TextMode="MultiLine" Rows="1" Width="900px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvRemark" runat="server" ControlToValidate="txtRemark" SetFocusOnError="true" Display="Dynamic"
            ErrorMessage="Please enter remark." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
        <asp:HiddenField ID="hdnPageValid" runat="server" />
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
                <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" ReadOnly="true" />
                <asp:BoundField DataField="TransporterName" HeaderText="Transporter" ReadOnly="true" />
                <asp:BoundField DataField="VehicleTypeName" HeaderText="Type" ReadOnly="true" />
                <asp:BoundField DataField="LocationFrom" HeaderText="Delivery From" ReadOnly="true" />
                <asp:BoundField DataField="DeliveryPoint" HeaderText="Delivery Point" ReadOnly="true" />
                <asp:BoundField DataField="MarketBillingRate" HeaderText="Market Rate" SortExpression="MarketBillingRate" ReadOnly="true" />
                <asp:BoundField DataField="Rate" HeaderText="Freight Rate" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="Advance" HeaderText="Advance(%)" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="AdvanceAmount" HeaderText="Advance Amt" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="DetentionAmount" HeaderText="Detention Amt" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="VaraiExpense" HeaderText="Varai Exp" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="EmptyContRecptCharges" HeaderText="Empty Cont Charges" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="TollCharges" HeaderText="Toll Charges" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="OtherCharges" HeaderText="Other Charges" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="TotalAmount" HeaderText="Total" ItemStyle-Font-Bold="true" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="LRNo" HeaderText="LR No" ReadOnly="true" ItemStyle-HorizontalAlign="Right" Visible="false" />
                <asp:BoundField DataField="LRDate" HeaderText="LR Date" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" ItemStyle-HorizontalAlign="Center" Visible="false" />
                <asp:BoundField DataField="ChallanNo" HeaderText="Challan No" ReadOnly="true" ItemStyle-HorizontalAlign="Right" Visible="false" />
                <asp:BoundField DataField="ChallanDate" HeaderText="Challan Date" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" Visible="false" />
                <asp:BoundField DataField="UnloadingDate" HeaderText="Unloading Date" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" ItemStyle-HorizontalAlign="Center" />
                <%--<asp:BoundField DataField="Instruction" HeaderText="Billing Intruction" SortExpression="Instruction" ReadOnly="true" />--%>
            </Columns>
        </asp:GridView>
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
        <legend>Bill Detail</legend>
        <div>
            <asp:GridView ID="gvBillDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId" OnRowDataBound="gvBillDetail_RowDataBound"
                DataSourceID="DataSourceBillDetail" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="true"
                PageSize="20" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                <Columns>
                    <asp:BoundField HeaderText="Transporter" DataField="Transporter" Visible="false" />
                    <asp:BoundField HeaderText="Bill Number" DataField="BillNumber" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="Bill Submit Date" DataField="BillSubmitDate" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="Bill Date" DataField="BillDate" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="Billing Person" DataField="BillPersonName" />
                    <asp:BoundField HeaderText="Bill Amount" DataField="BillAmount" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField HeaderText="Detention" DataField="DetentionAmount" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField HeaderText="Varai" DataField="VaraiAmount" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField HeaderText="Empty Cont Charges" DataField="EmptyContRcptCharges" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField HeaderText="Toll Charges" DataField="TollCharges" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField HeaderText="Other Charges" DataField="OtherCharges" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField HeaderText="Total" DataField="TotalAmount" ItemStyle-HorizontalAlign="Right" ItemStyle-Font-Bold="true" />
                    <asp:BoundField HeaderText="Justification" DataField="Justification" ItemStyle-Width="35%" />
                </Columns>
            </asp:GridView>
            <br />
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
        <%-- <asp:SqlDataSource ID="DataSourceRate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetTransRateDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqID" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>--%>
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
    <div id="dvModalPopup">
        <asp:HiddenField ID="hdnPopup" runat="server" Value="0" />
        <cc1:ModalPopupExtender ID="mpePopup" runat="server" TargetControlID="hdnPopup" BackgroundCssClass="modalBackground" CancelControlID="imgClosePopup"
            PopupControlID="pnlPopup" DropShadow="true">
        </cc1:ModalPopupExtender>
        <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup1" BackColor="#F5F5DC" Width="500px" Height="300px" BorderStyle="Solid" BorderWidth="1px">
            <div id="div1" runat="server">
                <table width="100%">
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblPopup_Error" runat="server"></asp:Label>
                            <asp:HiddenField ID="hdnTransBillId" runat="server" Value="0" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center"><b><u>
                            <asp:Label ID="lblPopup_Title" runat="server"></asp:Label></u></b>
                            <span style="float: right">
                                <asp:ImageButton ID="imgClosePopup" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgClosePopup_Click" ToolTip="Close" />
                            </span>
                        </td>
                    </tr>
                </table>
                <div>
                    <asp:Panel ID="pnlFundRequest2" runat="server" Width="450px" Height="260px" ScrollBars="Auto" Style="padding-left: 10px">
                        <fieldset>
                            <div id="dvApprovalAmount" runat="server" style="padding-top: 20px;">
                                <table class="table" border="0" cellpadding="0" cellspacing="0" width="40%" bgcolor="white">
                                    <tr>
                                        <td><b>Actual Amount</b></td>
                                        <td>
                                            <asp:Label ID="lblActualTotalAmt" runat="server" Font-Bold="true"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td><b>Approved Amount</b></td>
                                        <td>
                                            <asp:TextBox ID="txtApprovedAmount" runat="server" TabIndex="1" Font-Size="14px" Font-Bold="true" Style="padding: 5px; border-radius: 5px"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td><b>Remark (if any)</b></td>
                                        <td>
                                            <asp:TextBox ID="txtApprovalRemark" runat="server" TextMode="MultiLine" Rows="2" TabIndex="2" placeholder=" Remark ...." Style="padding: 5px; border-radius: 5px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Button ID="btnApprove" runat="server" Text="Approve" OnClientClick="return confirm('Are you sure to approve the bill?')" OnClick="btnApprove_Click" TabIndex="3" CssClass="form-control" Width="120px" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <br />
                            <div id="dvRemark" runat="server" style="padding-top: 20px">
                                <asp:TextBox ID="txtRemark_Rejected" runat="server" Width="95%" placeholder="Enter remark...." TabIndex="1" Rows="5" TextMode="MultiLine"
                                    CssClass="form-control"></asp:TextBox>
                                <br />
                                <span style="text-align: center">
                                    <asp:Button ID="btnReject" runat="server" Text="Reject" TabIndex="2" CssClass="form-control"
                                        OnClientClick="return confirm('Are you sure to reject the bill?')" OnClick="btnReject_Click" Width="80px" />
                                </span>
                            </div>
                        </fieldset>
                    </asp:Panel>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
