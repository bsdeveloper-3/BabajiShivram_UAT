<%@ Page Title="Vehicle Daily Expense" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="TransDailyExpense.aspx.cs" Inherits="Transport_TransDailyExpense" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="content2" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upExpense" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <div style="overflow:auto;">
    <fieldset><legend>Vehicle Daily Expense</legend>
        <asp:UpdatePanel ID="upExpense" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <div style="text-align:left; margin-left:100px;">
                    <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                    
                </div>
                <div class="clear">
                </div>
                <div>
                    <asp:Button ID="btnSubmit" runat="Server" Text="Save Daily Expense" OnClick="btnSubmit_Click" />
                    <asp:Button ID="btnCancel" runat="Server" Text="Cancel" OnClick="btnCancel_Click"/>
                    <b>Dispatch Date</b> &nbsp;<asp:TextBox ID="txtStatusDate" AutoPostBack="true" runat="server" MaxLength="100" Width="80px"
                       OnTextChanged="txtStatusDate_TextChanged" ></asp:TextBox>
                    <AjaxToolkit:CalendarExtender ID="calStatusDate" runat="server" EnableViewState="False"
                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtStatusDate">
                    </AjaxToolkit:CalendarExtender>    
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
                        <asp:Image ID="imgExport" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" ImageAlign="AbsMiddle" />
                        </asp:LinkButton>
                        &nbsp;&nbsp;&nbsp;&nbsp;    
                        <asp:LinkButton ID="lnkExportMonth" runat="server" Text="Month Vehicle Expense" OnClick="lnkExportMonth_Click"> </asp:LinkButton>    
                </div>
                
                <div>
                <asp:GridView ID="GridViewExpense" runat="server" AutoGenerateColumns="false" DataKeyNames="VehicleId" CssClass="table"
                     AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" PageSize="200" AllowPaging="False" AllowSorting="True" 
                     AutoGenerateEditButton="false" DataSourceID="SqlDataSourceExp">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" ReadOnly="true" SortExpression="VehicleNo"/>
                        <asp:BoundField DataField="Total" HeaderText="Total" ReadOnly="true" SortExpression="Total"/>
                        <asp:TemplateField HeaderText="Fuel Amount/Liter">
                            <ItemTemplate>
                                <asp:TextBox ID="txtFuel" Text='<%#BIND("Fuel") %>' Width="60px"  runat="server" MaxLength="8" placeholder="Fuel"></asp:TextBox>
                                <asp:TextBox ID="txtFuelLiter" Text='<%#BIND("FuelLiter") %>' Width="50px"  runat="server" MaxLength="8" placeholder="Liter"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fuel2 Amount/Liter">
                            <ItemTemplate>
                                <asp:TextBox ID="txtFuel2" Text='<%#BIND("Fuel2") %>' Width="60px"  runat="server" MaxLength="8" placeholder="Fuel"></asp:TextBox>
                                <asp:TextBox ID="txtFuel2Liter" Text='<%#BIND("Fuel2Liter") %>' Width="50px"  runat="server" MaxLength="8" placeholder="Liter"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Toll Charges">
                            <ItemTemplate>
                                <asp:TextBox ID="txtTollCharges" Text='<%#BIND("TollCharges") %>' Width="50px"  runat="server" MaxLength="8" placeholder="Toll"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fine Without Cleaner">
                            <ItemTemplate>
                                <asp:TextBox ID="txtWithoutCleaner" Text='<%#BIND("FineWithoutCleaner") %>' Width="100px"  runat="server" MaxLength="8" placeholder="Cleaner Fine"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Xerox">
                            <ItemTemplate>
                                <asp:TextBox ID="txtXerox" Text='<%#BIND("Xerox") %>' Width="50px"  runat="server" MaxLength="8" placeholder="Xerox"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Varai / Unloading">
                            <ItemTemplate>
                                <asp:TextBox ID="txtVaraiUnloading" Text='<%#BIND("VaraiUnloading") %>' Width="100px"  runat="server" MaxLength="8" placeholder="Varai/Unloading"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Empty Container Receipt">
                            <ItemTemplate>
                                <asp:TextBox ID="txtEmptyContainer" Text='<%#BIND("EmptyContainerReceipt") %>' Width="120px"  runat="server" MaxLength="8" placeholder="Empty Container Receipt"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Parking/GatePass">
                            <ItemTemplate>
                                <asp:TextBox ID="txtParking" Text='<%#BIND("ParkingGatePass") %>' Width="120px"  runat="server" MaxLength="8" placeholder="Parking/Gate Pass"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Garage">
                            <ItemTemplate>
                                <asp:TextBox ID="txtGarage" Text='<%#BIND("Garage") %>' Width="50px"  runat="server" MaxLength="8" placeholder="Garage"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Bhatta">
                            <ItemTemplate>
                                <asp:TextBox ID="txtBhatta" Text='<%#BIND("Bhatta") %>' Width="50px"  runat="server" MaxLength="8" placeholder="Bhatta"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ODC / Overweight">
                            <ItemTemplate>
                                <asp:TextBox ID="txtODCOverweight" Text='<%#BIND("AdditionalChargesForODCOverweight") %>' Width="50px"  runat="server" MaxLength="8" placeholder="ODC/Overweight"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Other Charges">
                            <ItemTemplate>
                                <asp:TextBox ID="txtOtherCharges" Text='<%#BIND("OtherCharges") %>' Width="50px"  runat="server" MaxLength="8" placeholder="OtherCharges"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Naka Passing/Damage Container">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDamageContainer" Text='<%#BIND("NakaPassingDamageContainer") %>' Width="120px"  runat="server" MaxLength="8" Placeholder="Naka Passing/Damage Container" ></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                </div>
                
                <div>
                    <asp:SqlDataSource ID="SqlDataSourceExp" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="TR_GetTransDailyExpense" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter Name="ReportDate" ControlID="txtStatusDate" PropertyName="Text" Type="datetime" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>

                <div>
                <asp:GridView ID="gvExport" runat="server" AutoGenerateColumns="false" CssClass="table" 
                     Visible="false" DataSourceID="SqlDataSourceExport">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No"/>
                        <asp:BoundField DataField="DriverName" HeaderText="Driver"/>
                        <asp:BoundField DataField="Total" HeaderText="Total"/>
                        <asp:BoundField DataField="Fuel" HeaderText="Fuel"/>
                        <asp:BoundField DataField="TollCharges" HeaderText="Toll Charges"/>
                        <asp:BoundField DataField="FineWithoutCleaner" HeaderText="Fine Without Cleaner"/>
                        <asp:BoundField DataField="Xerox" HeaderText="Xerox"/>
                        <asp:BoundField DataField="VaraiUnloading" HeaderText="Varai / Unloading"/>
                        <asp:BoundField DataField="EmptyContainerReceipt" HeaderText="Empty Container Receipt"/>
                        <asp:BoundField DataField="ParkingGatePass" HeaderText="Parking/GatePass"/>
                        <asp:BoundField DataField="Garage" HeaderText="Garage"/>
                        <asp:BoundField DataField="Bhatta" HeaderText="Bhatta"/>
                        <asp:BoundField DataField="AdditionalChargesForODCOverweight" HeaderText="ODC/Overweight"/>
                        <asp:BoundField DataField="OtherCharges" HeaderText="Other Charges"/>
                        <asp:BoundField DataField="NakaPassingDamageContainer" HeaderText="Naka Passing/Damage Container"/>
                    </Columns>
                </asp:GridView>
                </div>
                <div>
                <asp:GridView ID="gvExportMonth" runat="server" AutoGenerateColumns="false" CssClass="table" 
                     Visible="false" DataSourceID="SqlDataSourceExportMonth">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No"/>
                        <asp:BoundField DataField="Fuel" HeaderText="Fuel"/>
                        <asp:BoundField DataField="TollCharges" HeaderText="Toll Charges"/>
                        <asp:BoundField DataField="FineWithoutCleaner" HeaderText="Fine Without Cleaner"/>
                        <asp:BoundField DataField="Xerox" HeaderText="Xerox"/>
                        <asp:BoundField DataField="VaraiUnloading" HeaderText="Varai / Unloading"/>
                        <asp:BoundField DataField="EmptyContainerReceipt" HeaderText="Empty Container Receipt"/>
                        <asp:BoundField DataField="ParkingGatePass" HeaderText="Parking/GatePass"/>
                        <asp:BoundField DataField="Garage" HeaderText="Garage"/>
                        <asp:BoundField DataField="Bhatta" HeaderText="Bhatta"/>
                        <asp:BoundField DataField="AdditionalChargesForODCOverweight" HeaderText="ODC/Overweight"/>
                        <asp:BoundField DataField="OtherCharges" HeaderText="Other Charges"/>
                        <asp:BoundField DataField="NakaPassingDamageContainer" HeaderText="Naka Passing/Damage Container"/>
                    </Columns>
                </asp:GridView>
                </div>
                <div>
                    <asp:SqlDataSource ID="SqlDataSourceExport" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="TR_rptVehicleExpense" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter Name="ReportDate" ControlID="txtStatusDate" PropertyName="Text" Type="datetime" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlDataSourceExportMonth" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="TR_rptVehicleExpenseMonth" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter Name="ReportDate" ControlID="txtStatusDate" PropertyName="Text" Type="datetime" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
        </div>
</asp:Content>

