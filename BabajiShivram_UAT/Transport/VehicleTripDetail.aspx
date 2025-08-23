<%@ Page Title="Vehicle Trip detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="VehicleTripDetail.aspx.cs"
    Inherits="Transport_VehicleTripDetail" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function GetTotalAmt() {
            var a = document.getElementById('<%=txtFuel2.ClientID%>').value;
            var b = document.getElementById('<%=txtTollCharges.ClientID%>').value;
            var c = document.getElementById('<%=txtWithoutCleaner.ClientID%>').value;
            var d = document.getElementById('<%=txtXerox.ClientID%>').value;
            var e = document.getElementById('<%=txtVaraiUnloading.ClientID%>').value;
            var f = document.getElementById('<%=txtEmptyContainer.ClientID%>').value;
            var g = document.getElementById('<%=txtParking.ClientID%>').value;
            var h = document.getElementById('<%=txtGarage.ClientID%>').value;
            var i = document.getElementById('<%=txtBhatta.ClientID%>').value;
            var j = document.getElementById('<%=txtODCOverweight.ClientID%>').value;
            var k = document.getElementById('<%=txtOtherCharges.ClientID%>').value;
            var l = document.getElementById('<%=txtDamageContainer.ClientID%>').value;
            var Total = (Number(b) + Number(c) + Number(d) + Number(e) + Number(f) + Number(g) + Number(h) + Number(i) + Number(j) + Number(k) + Number(l));
            document.getElementById('<%=txtTotalAmt.ClientID%>').value = (Number(b) + Number(c) + Number(d) + Number(e) + Number(f) + Number(g) + Number(h) + Number(i) + Number(j) + Number(k) + Number(l)).toFixed(2);

           <%-- if (Number(Total) > 0) {
                if (Number(Total) > Number(a)) {
                    document.getElementById('<%=txtTotalAmt.ClientID%>').value = ((Number(b) + Number(c) + Number(d) + Number(e) + Number(f) + Number(g) + Number(h) + Number(i) + Number(j) + Number(k) + Number(l))).toFixed(2);
                }
                else if (Number(Total) == Number(a)) {
                    document.getElementById('<%=txtTotalAmt.ClientID%>').value = '0.00';
                }
        }--%>
    }
    </script>

    <cc1:toolkitscriptmanager runat="server" id="ScriptManager1" scriptmode="Release" />
    <div>
        <asp:HiddenField ID="hdnTransReqId" runat="server" Value="0" />
        <asp:ValidationSummary ID="csRequiredFields" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="BillRequired" CssClass="errorMsg" />
    </div>
    <div>
        <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" />
        <asp:HiddenField ID="hdnPageValid" runat="server" Value="0" />
    </div>
    <fieldset id="fsTransportRequest" runat="server">
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
    <fieldset>
        <legend>Rate Detail</legend>
        <div style="width: 1360px; overflow-x: scroll">
            <asp:GridView ID="GridViewVehicle" runat="server" AutoGenerateColumns="False" CssClass="table"
                Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                DataSourceID="DataSourceRate" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false"
                PageSize="20"
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
                    <asp:BoundField DataField="ReportingDate" HeaderText="Reporting Date" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                    <asp:BoundField DataField="Instruction" HeaderText="Billing Intruction" SortExpression="Instruction" ReadOnly="true" />
                </Columns>
            </asp:GridView>
        </div>
    </fieldset>
    <fieldset>
        <legend>Add Trip Detail</legend>
        <div align="center">
            <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            <asp:HiddenField ID="hdnLid" runat="server" Value="0" />
            <asp:HiddenField ID="hdnRateId" runat="server" Value="0" />
            <asp:ValidationSummary ID="vsRequired" runat="server" ShowMessageBox="True"
                ShowSummary="False" ValidationGroup="vgRequired" CssClass="errorMsg" />
        </div>
        <div class="m clear">
            <asp:Button ID="btnSaveExpense" runat="server" Text="Save" TabIndex="16" OnClick="btnSaveExpense_Click" ValidationGroup="vgRequired" />
            <asp:Button ID="btnCancel" TabIndex="17" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
        </div>
        <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
            <%--<tr>
                <td>Vehicle No
                    <asp:RequiredFieldValidator ID="rfvVehicleNo" runat="server" ControlToValidate="ddVehicleNo" InitialValue="0" SetFocusOnError="true"
                        Display="Dynamic" ErrorMessage="Please Select Vehicle No." Text="*" ForeColor="Red" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:DropDownList ID="ddVehicleNo" runat="server" TabIndex="1" Width="225px"></asp:DropDownList>
                </td>
                <td></td>
                <td>
                    <asp:TextBox ID="txtFuel" Width="100px" runat="server" TabIndex="1" MaxLength="8" placeholder="Fuel" Visible="false"></asp:TextBox>
                    <asp:TextBox ID="txtFuelLiter" Width="100px" runat="server" TabIndex="2" MaxLength="8" placeholder="Liter" Visible="false"></asp:TextBox>
                </td>
            </tr>--%>
            <tr>
                <td>Fuel Amount/Liter</td>
                <td>
                    <asp:TextBox ID="txtFuel2" Width="100px" runat="server" TabIndex="3" MaxLength="8" placeholder="Fuel" onchange="GetTotalAmt();"></asp:TextBox>
                    <asp:TextBox ID="txtFuelLiter2" Width="100px" runat="server" TabIndex="4" MaxLength="8" placeholder="Liter"></asp:TextBox>
                </td>
                <td>Toll Charges</td>
                <td>
                    <asp:TextBox ID="txtTollCharges" Width="100px" runat="server" TabIndex="5" MaxLength="8" placeholder="" onchange="GetTotalAmt();"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Fine Without Cleaner</td>
                <td>
                    <asp:TextBox ID="txtWithoutCleaner" Width="100px" runat="server" TabIndex="6" MaxLength="8" placeholder="" onchange="GetTotalAmt();"></asp:TextBox>
                </td>
                <td>Xerox</td>
                <td>
                    <asp:TextBox ID="txtXerox" Width="100px" runat="server" MaxLength="8" TabIndex="7" placeholder="" onchange="GetTotalAmt();"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Varai / Unloading</td>
                <td>
                    <asp:TextBox ID="txtVaraiUnloading" Width="100px" runat="server" TabIndex="8" MaxLength="8" placeholder="" onchange="GetTotalAmt();"></asp:TextBox>
                </td>
                <td>Empty Container Receipt</td>
                <td>
                    <asp:TextBox ID="txtEmptyContainer" Width="100px" runat="server" TabIndex="9" MaxLength="8" placeholder="" onchange="GetTotalAmt();"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Parking/Gate Pass</td>
                <td>
                    <asp:TextBox ID="txtParking" Width="100px" runat="server" MaxLength="8" TabIndex="10" placeholder="" onchange="GetTotalAmt();"></asp:TextBox>
                </td>
                <td>Garage</td>
                <td>
                    <asp:TextBox ID="txtGarage" Width="100px" runat="server" MaxLength="8" TabIndex="11" placeholder="" onchange="GetTotalAmt();"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Bhatta</td>
                <td>
                    <asp:TextBox ID="txtBhatta" Width="100px" runat="server" MaxLength="8" TabIndex="12" placeholder="" onchange="GetTotalAmt();"></asp:TextBox>
                </td>
                <td>ODC / Overweight</td>
                <td>
                    <asp:TextBox ID="txtODCOverweight" Width="100px" runat="server" MaxLength="8" TabIndex="13" placeholder="" onchange="GetTotalAmt();"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Other Charges</td>
                <td>
                    <asp:TextBox ID="txtOtherCharges" Width="100px" runat="server" MaxLength="8" TabIndex="14" placeholder="" onchange="GetTotalAmt();"></asp:TextBox>
                </td>
                <td>Naka Passing / Damage Container</td>
                <td>
                    <asp:TextBox ID="txtDamageContainer" Width="100px" runat="server" MaxLength="8" TabIndex="15" placeholder="" onchange="GetTotalAmt();"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Total</td>
                <td>
                    <asp:TextBox ID="txtTotalAmt" runat="server" Enabled="false"></asp:TextBox></td>
                <td></td>
                <td></td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <legend>Trip Detail</legend>
        <div>
            <asp:GridView ID="gvVehicleExpense" runat="server" AutoGenerateColumns="False" CssClass="table"
                Width="100%" DataKeyNames="lId" DataSourceID="DataSourceVehicleExpense" CellPadding="4" OnRowCommand="gvVehicleExpense_RowCommand">
                <Columns>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" CommandName="Edit" runat="server" Text="Edit" CommandArgument='<%#Bind("lid") %>'
                                ToolTip="Click To Update Rate Expense Detail."></asp:LinkButton>
                            <asp:LinkButton ID="lnkDelete" CommandName="DeleteRow" runat="server" Text="Delete" CommandArgument='<%#Bind("lid") %>'
                                ToolTip="Click To Delete Rate Expense Detail."></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
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
        </div>
    </fieldset>
    <div id="divDataSource">
        <asp:SqlDataSource ID="DataSourceRate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetTransRateDetailById" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="lid" SessionField="RateId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceVehicleExpense" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetVehicleRateExpense" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="hdnTransReqId" Name="TransReqId" PropertyName="Value" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>

</asp:Content>

