<%@ Page Title="Consolidate Bill Tracking" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ConsolidateTracking.aspx.cs"
    Inherits="Transport_ConsolidateTracking" Culture="en-GB" %>

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
        <asp:HiddenField ID="hdnPageValid" runat="server" />
    </div>
    <script type="text/javascript">
        function ActiveTabChanged12() {
            /* Clear Error Message on Tab change Event */
            document.getElementById('<%=lblError.ClientID%>').innerHTML = '';
            document.getElementById('<%=lblError.ClientID%>').className = '';
        }
    </script>
    <div>
        <div align="center">
            <asp:Label ID="lblError" runat="server"></asp:Label>
        </div>
        <asp:UpdatePanel ID="upnlConsolidateTracking" runat="server">
            <ContentTemplate>
                <cc1:TabContainer runat="server" ID="TabRequestRecd" ActiveTabIndex="0" CssClass="Tab"
                    Width="100%" OnClientActiveTabChanged="ActiveTabChanged12" AutoPostBack="true">
                    <cc1:TabPanel runat="server" ID="TabPanelNormalJob" TabIndex="0" HeaderText="Request">
                        <ContentTemplate>
                            <fieldset>
                                <legend>Consolidate Request Detail</legend>
                                <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                    <tr>
                                        <td>Ref No.
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTRRefNo" runat="server" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td>Transporter Name
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTransporter" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Created By
                                        </td>
                                        <td>
                                            <asp:Label ID="lblJobCreatedBy" runat="server"></asp:Label>
                                        </td>
                                        <td>Created Date
                                        </td>
                                        <td>
                                            <asp:Label ID="lblJobCreatedDate" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="TabPanelJobDetail" TabIndex="1" HeaderText="Job Detail">
                        <ContentTemplate>
                            <fieldset>
                                <legend>Job Detail</legend>
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
                                            <asp:BoundField DataField="PlanningDate" HeaderText="Planning Date" DataFormatString="{0:dd/MM/yyyy}" Visible="false" />
                                            <asp:BoundField DataField="LRNo" HeaderText="LR No" Visible="false" />
                                            <asp:BoundField DataField="LRDate" HeaderText="LR Date" DataFormatString="{0:dd/MM/yyyy}" Visible="false" />
                                            <asp:BoundField DataField="BabajiChallanNo" HeaderText="Challan No" Visible="false" />
                                            <asp:BoundField DataField="BabajiChallanDate" HeaderText="Challan Date" DataFormatString="{0:dd/MM/yyyy}" Visible="false" />
                                            <asp:BoundField DataField="RequestedBy" HeaderText="Requested By" />
                                            <asp:BoundField DataField="RequestedDate" HeaderText="Request Date" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="UnloadingDate" HeaderText="Unloading Date" DataFormatString="{0:dd/MM/yyyy}" Visible="false" />
                                            <asp:BoundField DataField="ReportingDate" HeaderText="Reporting Date" DataFormatString="{0:dd/MM/yyyy}" Visible="false" />
                                            <asp:BoundField DataField="ContReturnDate" HeaderText="Cont Return Date" DataFormatString="{0:dd/MM/yyyy}" Visible="false" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="TabPanelVehicle" TabIndex="2" HeaderText="Vehicle">
                        <ContentTemplate>
                            <fieldset>
                                <legend>Vehicle Rate Detail</legend>
                                <div style="width: 1360px; overflow-x: scroll">
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
                                            <asp:BoundField DataField="TransporterName" HeaderText="Transporter" SortExpression="TransporterName" ReadOnly="true" />
                                            <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" SortExpression="VehicleNo" ReadOnly="true" />
                                            <asp:BoundField DataField="VehicleTypeName" HeaderText="Vehicle Type" SortExpression="VehicleTypeName" ReadOnly="true" />
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
                                            <asp:BoundField DataField="TollCharges" HeaderText="Toll Charges" SortExpression="TollCharges" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="OtherCharges" HeaderText="Other Charges" SortExpression="OtherCharges" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="Instruction" HeaderText="Billing Intruction" SortExpression="Instruction" ReadOnly="true" />
                                            <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy" ReadOnly="true" />
                                            <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" SortExpression="CreatedDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="UpdatedBy" HeaderText="Updated By" SortExpression="UpdatedBy" ReadOnly="true" />
                                            <asp:BoundField DataField="UpdatedDate" HeaderText="Updated Date" SortExpression="UpdatedDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanelMovement" runat="server" TabIndex="3" HeaderText="Movement">
                        <ContentTemplate>
                            <fieldset>
                                <legend>Movement History</legend>
                                <div>
                                    <asp:GridView ID="gvMovementHistory" runat="server" AutoGenerateColumns="true" CssClass="table"
                                        Width="100%" PagerStyle-CssClass="pgr" DataSourceID="DataSourceMovementHistory"
                                        CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sl">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                            <fieldset>
                                <legend>Vehicle Daily Status History</legend>
                                <div>
                                    <asp:GridView ID="gvVehicleDailyStatus" runat="server" AutoGenerateColumns="False" CssClass="table"
                                        Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="TransReqId"
                                        DataSourceID="DataSourceDailyStatus" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false"
                                        PageSize="20" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" OnRowCommand="gvVehicleDailyStatus_RowCommand" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sl">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Vehicle No" DataField="VehicleNo" />
                                            <asp:BoundField HeaderText="Vehicle Type" DataField="VehicleType" />
                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnDailyStatus" runat="server" Font-Underline="true" CommandName="DailyStatus" Text='<%#Eval("CurrentStatus") %>'
                                                        CommandArgument='<%#Eval("VehicleNo") + ";" + Eval("VehicleType") + ";" + Eval("DeliveryFrom") + ";" + Eval("DeliveryTo") + ";" + Eval("DispatchDate","{0:dd/MM/yyyy}") + ";" + Eval("CustomerMail") + ";" + Eval("TransReqId") + ";" + Eval("JobRefNo") + ";" + Eval("CustName") + ";" + Eval("CustRefNo") + ";" + Eval("CurrentStatus") + ";" + Eval("StatusCreatedBy")  + ";" + Eval("EmailTo") + ";" + Eval("EmailCC")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Current Status" DataField="CurrentStatus" Visible="false" />
                                            <asp:BoundField HeaderText="CreatedBy" DataField="StatusCreatedBy" />
                                            <asp:BoundField HeaderText="Created Date" DataField="StatusCreatedDate" DataFormatString="{0:dd/MM/yyyy HH:mm:tt}" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <%-- START  : Pop-up For Daily Status --%>
                                <div>
                                    <asp:HiddenField ID="hdnDailyStatus" runat="server" Value="0" />
                                    <cc1:ModalPopupExtender ID="mpeDailyStatus" runat="server" TargetControlID="hdnDailyStatus" BackgroundCssClass="modalBackground" CancelControlID="imgClosePopup"
                                        PopupControlID="pnlDailyStatus" DropShadow="true">
                                    </cc1:ModalPopupExtender>
                                    <asp:Panel ID="pnlDailyStatus" runat="server" CssClass="modalPopup1" BackColor="#F5F5DC" Style="border-radius: 5px" Width="810px" Height="520px" BorderStyle="Solid" BorderWidth="2px">
                                        <div id="div1" runat="server">
                                            <table width="100%" style="border-bottom: 1px solid black">
                                                <tr>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td align="center"><b><u>Daily Status</u></b>
                                                        <span style="float: right">
                                                            <asp:ImageButton ID="imgClosePopup" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgClosePopup_Click" ToolTip="Close" />
                                                        </span>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div>
                                                <asp:Panel ID="pnlDailyStatus2" runat="server" Width="800px" Height="480px" ScrollBars="Auto">
                                                    <div id="DivABC" runat="server" style="border: 1px solid black; margin: 5px; margin-top: 0px; border-radius: 4px; max-height: 620px; max-width: 780px;">
                                                        <div class="m" style="padding: 10px;">
                                                            <asp:Label ID="lblPopMessageEmail" runat="server" EnableViewState="false"></asp:Label>
                                                            <table border="0" width="100%">
                                                                <tr>
                                                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  <b>To : </b>
                                                                        <u>
                                                                            <asp:Label ID="lblCustomerEmail" runat="server" Font-Underline="true" Width="89%" Enabled="false" CssClass="cssStatus"></asp:Label></u>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b>CC :</b>
                                                                        <u>
                                                                            <asp:Label ID="txtMailCC" runat="server" Width="89%" Font-Underline="true" Enabled="false" CssClass="cssStatus"></asp:Label></u>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td><b>Subject :</b>
                                                                        <u>
                                                                            <asp:Label ID="txtSubject" runat="server" Width="89%" Enabled="false" Font-Underline="true" CssClass="cssStatus"></asp:Label></u>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div id="divPreviewEmail" runat="server" style="padding: 10px; background-color: white; border-radius: 3px; margin-left: 10px; margin-right: 10px; margin-bottom: 20px; border: 1px solid black; border-style: ridge">
                                                        </div>
                                                        <div id="DivSendEmail" runat="server" style="text-align: right; margin-left: 350px">
                                                            <asp:Button ID="btnCancelEmailPp" runat="server" OnClick="btnCancelEmailPp_Click" Text="Cancel"></asp:Button>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                                <%-- END    : Pop-up For Daily Status --%>
                            </fieldset>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanelBillStatus" runat="server" TabIndex="4" HeaderText="Bill Detail">
                        <ContentTemplate>
                            <fieldset>
                                <legend>Bill Detail</legend>
                                <div>
                                    <asp:GridView ID="gvBillDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                                        Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                                        DataSourceID="DataSourceBillDetail" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false"
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
                                            <asp:BoundField HeaderText="Total" DataField="TotalAmount" ItemStyle-HorizontalAlign="Right" ItemStyle-Font-Bold="true" />
                                            <asp:BoundField HeaderText="Justification" DataField="Justification" ItemStyle-Width="35%" />
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                </div>
                            </fieldset>
                            <fieldset>
                                <legend>Bill Approval History</legend>
                                <div>
                                    <asp:GridView ID="gvBillApprovalHistory" runat="server" AutoGenerateColumns="False" CssClass="table"
                                        Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                                        DataSourceID="DataSourceBillApprovalHistory" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false"
                                        PageSize="20" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sl">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="BillNumber" DataField="BillNumber" />
                                            <asp:BoundField HeaderText="Status" DataField="Status" />
                                            <asp:BoundField HeaderText="Remark" DataField="Remark" />
                                            <asp:BoundField HeaderText="Active/Inactive" DataField="ActiveStatus" Visible="false" />
                                            <asp:BoundField HeaderText="Created By" DataField="CreatedBy" />
                                            <asp:BoundField HeaderText="Created Date" DataField="CreatedDate" DataFormatString="{0:dd/MM/yyyy HH:mm:tt}" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                            <fieldset>
                            <legend>Vehicle Rate Detail</legend>
                            <div style="width: 1250px; overflow-x: auto">
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table"
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
                                        <asp:BoundField DataField="Instruction" HeaderText="Billing Intruction" SortExpression="Instruction" ReadOnly="true" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                            <fieldset>
                            <legend>Selling Rate Detail</legend>
                            <div>
                                <div style="width: 1250px;overflow-x: scroll">
                                    <asp:GridView ID="gvSellingDetail" runat="server" AutoGenerateColumns="False" CssClass="table" Width="90%" AlternatingRowStyle-CssClass="alt"
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
                                            <asp:BoundField DataField="SellFreighRate" HeaderText="Selling Freight rate" ReadOnly="true" />
                                            <asp:BoundField DataField="SellDetentionAmount" HeaderText="Detention Amount" ReadOnly="true" />
                                            <asp:BoundField DataField="SellVaraiExpense" HeaderText="Varai Amount No" ReadOnly="true" />
                                            <asp:BoundField DataField="SellEmptyContRecptCharges" HeaderText="Empty Cont Amount" ReadOnly="true" />
                                            <asp:BoundField DataField="SellTollCharges" HeaderText="Toll Amount No" ReadOnly="true" />
                                            <asp:BoundField DataField="SellOtherCharges" HeaderText="Other Amount" ReadOnly="true" />
                                            <asp:TemplateField HeaderText="Detention Doc">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnDetentionCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download detention copy."
                                                        CommandName="DetentionCopy" CommandArgument='<%#Eval("DetentionDoc")%>' Visible='<%# DecideHereImg((string)Eval("DetentionDoc")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Varai Doc">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnVaraiCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download varai copy."
                                                        CommandName="varaiCopy" CommandArgument='<%#Eval("VaraiDoc")%>' Visible='<%# DecideHereImg((string)Eval("VaraiDoc")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Empty Cont Doc">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnemptyContCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download Empty cont copy."
                                                        CommandName="EmptyContCopy" CommandArgument='<%#Eval("EmptyContDoc")%>' Visible='<%# DecideHereImg((string)Eval("EmptyContDoc")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Toll Doc">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnTollCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download toll copy."
                                                        CommandName="TollCopy" CommandArgument='<%#Eval("TollDoc")%>' Visible='<%# DecideHereImg((string)Eval("TollDoc")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Other Doc">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnOtherCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download other copy."
                                                        CommandName="OtherCopy" CommandArgument='<%#Eval("OtherDoc")%>' Visible='<%# DecideHereImg((string)Eval("OtherDoc")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Email Approval Copy">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnEmailApprovalCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download Email Approval copy."
                                                        CommandName="EmailApprovalCopy" CommandArgument='<%#Eval("EmailAttachment")%>' Visible='<%# DecideHereImg((string)Eval("EmailAttachment")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Contract Copy">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnContractCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download contract copy."
                                                        CommandName="ContractCopy" CommandArgument='<%#Eval("ContractAttachment")%>' Visible='<%# DecideHereImg((string)Eval("ContractAttachment")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Instruction" HeaderText="Billing Instruction" ReadOnly="true" />
                                            <asp:BoundField DataField="Remark" HeaderText="Other Remark" ReadOnly="true" />
                                            <asp:BoundField DataField="SellDetail" HeaderText="Charge to Party" ReadOnly="true" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </fieldset>
                            <fieldset>
                                <legend>Bill Received Detail</legend>
                                <div>
                                    <asp:GridView ID="gvBillReceived" runat="server" AutoGenerateColumns="False" CssClass="table"
                                        Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                                        DataSourceID="DataSourceBillReceived" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false"
                                        PageSize="20" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sl">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="BillNumber" DataField="BillNumber" />
                                            <asp:BoundField HeaderText="Sent User" DataField="SentUser" />
                                            <asp:BoundField HeaderText="Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField HeaderText="ReceivedBy" DataField="ReceivedBy" />
                                            <asp:BoundField HeaderText="ReceivedDate" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField HeaderText="Status" DataField="StatusName" />
                                            <asp:BoundField HeaderText="Cheque No" DataField="ChequeNo" />
                                            <asp:BoundField HeaderText="Cheque Date" DataField="ChequeDate" DataFormatString="{0:dd/MM/yyyy}" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                            <fieldset>
                                <legend>Bill Received Status History</legend>
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
                                            <asp:BoundField HeaderText="Cheque No" DataField="ChequeNo" />
                                            <asp:BoundField HeaderText="Cheque Date" DataField="ChequeDate" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField HeaderText="Hold Reason" DataField="HoldReason" />
                                            <asp:BoundField HeaderText="Release Date" DataField="ReleaseDate" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField HeaderText="CreatedBy" DataField="CreatedBy" />
                                            <asp:BoundField HeaderText="Created Date" DataField="CreatedDate" DataFormatString="{0:dd/MM/yyyy HH:mm:tt}" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanelDelivery" runat="server" TabIndex="5" HeaderText="Delivery">
                        <ContentTemplate>
                            <fieldset>
                                <legend>Delivery Detail</legend>
                                <asp:GridView ID="gvVehicleDelivery" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="TransporterId"
                                    DataSourceID="DataSourceDelivery" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false"
                                    PageSize="20" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Transporter" DataField="TransporterName" />
                                        <asp:BoundField HeaderText="Vehicle No" DataField="VehicleNo" />
                                        <asp:BoundField HeaderText="Vehicle Type" DataField="VehicleTypeName" />
                                        <asp:BoundField HeaderText="Total Packages" DataField="NoofPackages" />
                                        <asp:BoundField HeaderText="Container No" DataField="ContainerNo" />
                                        <asp:BoundField HeaderText="LR No" DataField="LRNo" />
                                        <asp:BoundField HeaderText="LR Date" DataField="LRDate" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField HeaderText="Challan No" DataField="ChallanNo" />
                                        <asp:BoundField HeaderText="Challan Date" DataField="ChallanDate" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField HeaderText="Location From" DataField="LocationFrom" />
                                        <asp:BoundField HeaderText="Delivery To" DataField="DeliveryPoint" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanelFund" runat="server" TabIndex="6" HeaderText="Fund Detail">
                        <ContentTemplate>
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
                                    <br />
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
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanelTripDetail" runat="server" TabIndex="7" HeaderText="Trip Detail">
                        <ContentTemplate>
                            <fieldset>
                                <legend>Trip Detail</legend>
                                <div style="width: 1350px; overflow: auto">
                                    <asp:GridView ID="gvVehicleExpense" runat="server" AutoGenerateColumns="False" CssClass="table"
                                        Width="100%" DataKeyNames="lId" DataSourceID="DataSourceVehicleExpense" CellPadding="4">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sl">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Vehicle No" DataField="VehicleNo" ReadOnly="true" />
                                            <asp:BoundField HeaderText="Fuel" DataField="Fuel2" ReadOnly="true" />
                                            <asp:BoundField HeaderText="Fuel Liter" DataField="Fuel2Liter" ReadOnly="true" />
                                            <asp:BoundField HeaderText="Toll Charges" DataField="TollCharges" ReadOnly="true" />
                                            <asp:BoundField HeaderText="Fine Without Cleaner" DataField="FineWithoutCleaner" ReadOnly="true" />
                                            <asp:BoundField HeaderText="Xerox" DataField="Xerox" ReadOnly="true" />
                                            <asp:BoundField HeaderText="Varai Unloading" DataField="VaraiUnloading" ReadOnly="true" />
                                            <asp:BoundField HeaderText="Empty Container Receipt" DataField="EmptyContainerReceipt" ReadOnly="true" />
                                            <asp:BoundField HeaderText="Parking/GatePass" DataField="ParkingGatePass" ReadOnly="true" />
                                            <asp:BoundField HeaderText="Garage" DataField="Garage" ReadOnly="true" />
                                            <asp:BoundField HeaderText="Bhatta" DataField="Bhatta" ReadOnly="true" />
                                            <asp:BoundField HeaderText="ODC/Overweight" DataField="AdditionalChargesForODCOverweight" ReadOnly="true" />
                                            <asp:BoundField HeaderText="OtherCharges" DataField="OtherCharges" ReadOnly="true" />
                                            <asp:BoundField HeaderText="NakaPassing/DamageContainer" DataField="NakaPassingDamageContainer" ReadOnly="true" />
                                            <asp:BoundField HeaderText="Created By" DataField="CreatedBy" ReadOnly="true" />
                                            <asp:BoundField HeaderText="Created Date" DataField="CreatedDate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                                        </Columns>
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="DataSourceVehicleExpense" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                        SelectCommand="TR_GetVehicleRateExpense" SelectCommandType="StoredProcedure">
                                        <SelectParameters>
                                            <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>
                            </fieldset>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divDataSource">
        <asp:SqlDataSource ID="DataSourceDelivery" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetConsolDeliveryDetail_TRId" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="ConsolidateID" SessionField="TRConsolidateId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceRate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetTransRateDetailForConsolidate" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <%--<asp:SessionParameter Name="TransReqId" SessionField="TRId" />--%>
                <asp:SessionParameter Name="ConsolidateID" SessionField="TRConsolidateId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceBillDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetTransBillDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceBillReceived" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetBillReceivedByTransReqId" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceBillApprovalHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetBillApprovalHistory" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceBillStatusHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetBillReceivedDetailHistory" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourcePaymentHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetPaymentDetails" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceTransportJobDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetConsolidateJobDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <%--<asp:SessionParameter Name="ConsolidateID" SessionField="TRConsolidateId" />--%>
                 <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceSellingRate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetVehicleSellingDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqID" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceMovementHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetConsolidateMovementHistory" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="ConsolidateID" SessionField="TRConsolidateId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceDailyStatus" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetDailyStatusHistory" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>

    </div>

</asp:Content>

