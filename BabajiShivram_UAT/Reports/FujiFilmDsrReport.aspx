<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FujiFilmDsrReport.aspx.cs" Inherits="Reports_FujiFilmDsrReport" %>

<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />

    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="updFujiFilmReport" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

     <asp:UpdatePanel ID="updFujiFilmReport" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white" style="width: 100%; align-content: center;">
                <tr>
                    <td>Job Date From
                        <cc1:calendarextender id="CalExtJobFromDate" runat="server" enabled="True" enableviewstate="False"
                            firstdayofweek="Sunday" format="dd/MM/yyyy" popupbuttonid="imgFromDate" popupposition="BottomRight"
                            targetcontrolid="txtFromDate">
                        </cc1:calendarextender>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate" runat="server" Width="100px" MaxLength="10" TabIndex="1" placeholder="dd/mm/yyyy"></asp:TextBox>
                        <asp:Image ID="imgFromDate" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif"
                            runat="server" />
                        <asp:CompareValidator ID="ComValFromDate" runat="server" ControlToValidate="txtFromDate" Display="Dynamic" Text="Invalid Date." Type="Date"
                            CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="Required" SetFocusOnError="true" ErrorMessage="Invalid From Date">
                        </asp:CompareValidator>
                    </td>
                    <td>Job Date To
                        <cc1:calendarextender id="CalExtJobFromTo" runat="server" enabled="True" enableviewstate="False"
                            firstdayofweek="Sunday" format="dd/MM/yyyy" popupbuttonid="imgToDate" popupposition="BottomRight"
                            targetcontrolid="txtToDate">
                        </cc1:calendarextender>
                    </td>
                    <td>
                        <asp:TextBox ID="txtToDate" runat="server" Width="100px" MaxLength="10" TabIndex="2" placeholder="dd/mm/yyyy"></asp:TextBox>
                        <asp:Image ID="imgToDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                            runat="server" />
                        <asp:CompareValidator ID="ComValToDate" runat="server" ControlToValidate="txtToDate" Display="Dynamic" ErrorMessage="Invalid To Date." Text="Invalid To Date."
                            Type="Date" CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="Required" SetFocusOnError="true">
                        </asp:CompareValidator>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddClearedStatus" runat="server">
                            <asp:ListItem Value="" Text="All Job"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Cleared"></asp:ListItem>
                            <asp:ListItem Value="0" Text="Un-Cleared" Selected="True" ></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;
                        <asp:Button ID="btnAddFilter" runat="server" Text="Add Filter" OnClick="btnAddFilter_Click" />
                        &nbsp;&nbsp;
                        <asp:Button ID="btnClearFilter" runat="server" Text="Clear Filter" OnClick="btnClearFilter_Click" />
                    </td>

                </tr>
            </table>
           
            <div class="clear"></div>
            <div class="clear"></div>

            <fieldset>
                <legend>Fuji Film Report</legend>
                <div>
                    <asp:LinkButton ID="lnkReportXls" runat="server" OnClick="lnkReportXls_Click">
                        <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                </div>
                <div class="clear"></div>
                <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="False" CssClass="table" 
                    ShowFooter="false">
                    <%--DataSourceID="DataSourceReport" --%>
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField HeaderText="JobDate" DataField="JobDate" />--%>

                        <asp:BoundField HeaderText="Month" DataField="Month" />
                        <asp:BoundField HeaderText="CHA" DataField="CHA" />
                        <asp:BoundField HeaderText="Division" DataField="Division" />
                        <asp:BoundField HeaderText="Supplier" DataField="Supplier" />
                        <asp:BoundField HeaderText="Supplier Invoice No" DataField="Supplier Invoice No" />
                        <asp:BoundField HeaderText="Supplier Invoice Date" DataField="Supplier Invoice Date" />
                        <asp:BoundField HeaderText="MBL" DataField="MAWBNo" />
                        <asp:BoundField HeaderText="HBL" DataField="HAWBNo" />
                        <asp:BoundField HeaderText="Port Of Loading" DataField="PortOfLoading" />
                        <asp:BoundField HeaderText="Country" DataField="CountryOfOrigin" />
                        <asp:BoundField HeaderText="Forwarder" DataField="FFName" />
                        <asp:BoundField HeaderText="Shipping Name" DataField="ShippingName" />
                        <asp:BoundField HeaderText="Port" DataField="Port" />
                        <asp:BoundField HeaderText="Port Entry Time" DataField="Port Entry Time" />
                        <asp:BoundField HeaderText="Port Exit Time" DataField="Port Exit Time" />
                        <asp:BoundField HeaderText="Location" DataField="DeliveryDestination" />
                        <asp:BoundField HeaderText="Plant" DataField="Plant" />
                        <asp:BoundField HeaderText="Job No" DataField="JobRefNo" />
                        <asp:BoundField HeaderText="BOE No" DataField="BOENo" />
                        <asp:BoundField HeaderText="BOE Date" DataField="BOEDate" />
                        <asp:BoundField HeaderText="ShortDescription" DataField="ShortDescription" />
                        <asp:BoundField HeaderText="Mode" DataField="Mode" />
                        <asp:BoundField HeaderText="Type" DataField="CONTAINERTYPE" />
                        <asp:BoundField HeaderText="Count 20" DataField="Count20" />
                        <asp:BoundField HeaderText="Count 40" DataField="Count40" />
                        <asp:BoundField HeaderText="Weight" DataField="GrossWT" />
                        <asp:BoundField HeaderText="Pallets" DataField="No of Pallets" />
                        <asp:BoundField HeaderText="Cartons" DataField="No. of Cartons" />
                        <asp:BoundField HeaderText="CBM[LCL]" DataField="" />
                        <asp:BoundField HeaderText="Vol Mt.[Air]" DataField="" />
                        <asp:BoundField HeaderText="Free Days" DataField="FreeDays" />
                        <asp:BoundField HeaderText="CFS Movement Reason" DataField="" />
                        <asp:BoundField HeaderText="CFS Charges" DataField="" />
                        <asp:BoundField HeaderText="IncoTerm" DataField="IncoTerm" />
                        <asp:BoundField HeaderText="Assessable Value" DataField="AssessableValue" />
                        <asp:BoundField HeaderText="CHA Bill No" DataField="CHA Bill No" />
                        <asp:BoundField HeaderText="CHA Bill Date" DataField="CHA Bill Date" />
                        <asp:BoundField HeaderText="CHA Bill Amount" DataField="CHA Bill Amount" />
                        <asp:BoundField HeaderText="Bill No" DataField="Bill No" />
                        <asp:BoundField HeaderText="Bill Date" DataField="Bill Date" />
                        <asp:BoundField HeaderText="Bill Amount" DataField="Bill Amount" />
                        <asp:BoundField HeaderText="Total Bill Amount" DataField="Total Bill Amount" />
                        <%--<asp:BoundField HeaderText="DailyProgress" DataField="DailyProgress" />--%>
                        <asp:TemplateField HeaderText="Remark">
                            <ItemTemplate>
                                <div style="word-wrap: break-word; width: 400px; white-space: normal;">
                                    <asp:Label ID="lblRemark" runat="server" Text='<%#Bind("[DailyProgress]") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Arrival Date" DataField="ArrivalDate" />
                        <asp:BoundField HeaderText="Clearance Date" DataField="LastDispatchDate" />
                        <asp:BoundField HeaderText="Delivery Date" DataField="DeliveryDate" />
                        <asp:BoundField HeaderText="Bonding and Deonding" DataField="" />
                        <asp:BoundField HeaderText="Agency Charges" DataField="" />
                        <asp:BoundField HeaderText="CFS AMOUNT" DataField="CFS AMOUNT" />
                        <asp:BoundField HeaderText="TX-CFS" DataField="" />
                        <asp:BoundField HeaderText="Shipping Line Charge" DataField="" />
                        <asp:BoundField HeaderText="THC Payment" DataField="THC Payment" />
                        <asp:BoundField HeaderText="DetentionAmount" DataField="DetentionAmount" />
                        <asp:BoundField HeaderText="AAI Charges" DataField="AAI Charges" />
                        <asp:BoundField HeaderText="DO Fee" DataField="" />
                        <asp:BoundField HeaderText=" EDI Charges " DataField="" />
                        <asp:BoundField HeaderText=" Documentation Charges " DataField="" />
                        <asp:BoundField HeaderText=" Examination Charges " DataField="" />
                        <asp:BoundField HeaderText=" Loading Charges " DataField="" />
                        <asp:BoundField HeaderText=" Survey Charges " DataField="" />
                        <asp:BoundField HeaderText=" Provisional BE Assessment " DataField="" />
                        <asp:BoundField HeaderText=" N-Form Charges " DataField="" />
                        <asp:BoundField HeaderText="STAMP DUTY AMOUNT" DataField="STAMPDUTYAMOUNT" />
                        <asp:BoundField HeaderText="Bond Paper Charges " DataField="" />
                        <asp:BoundField HeaderText=" Insurance Charges " DataField="" />
                        <asp:BoundField HeaderText=" MICS/ Other  Expanses " DataField="" />
                        <asp:BoundField HeaderText=" ADC CLEARANCE CHARGES " DataField="" />
                        <asp:BoundField HeaderText=" WARE HOUSE CHARGES " DataField="" />
                        <asp:BoundField HeaderText=" LABELING CHARGES" DataField="" />
                        <asp:BoundField HeaderText=" ADCODE Registration " DataField="" />
                        <asp:BoundField HeaderText=" OCTROI CHARGES " DataField="" />
                        <asp:BoundField HeaderText="Toll/Green Tax" DataField="" />
                        <asp:BoundField HeaderText="Transportation Charges" DataField="Transportation Charges" />
                        <asp:BoundField HeaderText="TTL Bill no." DataField="" />
                        <asp:BoundField HeaderText="TTL Transportation" DataField="" />
                        <asp:BoundField HeaderText="Vehicle No" DataField="VehicleNo" />
                        <asp:BoundField HeaderText="vehicle Type" DataField="vehicletype" />
                        <asp:BoundField HeaderText="LRNo" DataField="LRNo" />
                        <asp:BoundField HeaderText="LRDate" DataField="LRDate" />
                        <asp:BoundField HeaderText="VEHICLE FROM" DataField="VEHICLE FROM" />
                        <asp:BoundField HeaderText="VEHICLE TO" DataField="VEHICLE TO" />
                        <asp:BoundField HeaderText="VehiclePlaceRequireDate" DataField="VehiclePlaceRequireDate" />
                        <asp:BoundField HeaderText="FreightAmount" DataField="FreightAmount" />
                        <asp:BoundField HeaderText="DPD CHARGES" DataField="" />
                        <asp:BoundField HeaderText="EMPTY /OTHER CHARGES" DataField="" />
                        <asp:BoundField HeaderText="IGST" DataField="IGST" />
                        <asp:BoundField HeaderText="CGST" DataField="CGST" />
                        <asp:BoundField HeaderText="SGST" DataField="SGST" />
                        <asp:BoundField HeaderText="TOTAL" DataField="TOTAL" />
                        <%--<asp:BoundField HeaderText="TR6 Date" DataField="TR6 Date" />
                        <asp:BoundField HeaderText="Stamp Duty Amount" DataField="Stamp Duty Amount" />
                        <asp:BoundField HeaderText="Examin Date" DataField="Examin Date" />
                        <asp:BoundField HeaderText="OOC Date" DataField="OOC Date" />
                        <asp:BoundField HeaderText="Bond Type" DataField="Bond Type" />
                        <asp:BoundField HeaderText="Bond No" DataField="Bond No" />
                        <asp:BoundField HeaderText="Bond Date" DataField="Bond Date" />
                        <asp:BoundField HeaderText="Warehouse Detail" DataField="Warehouse Detail" />
                        <asp:BoundField HeaderText="Shipment Ready For Delivery" DataField="Shipment Ready For Delivery" />
                        <asp:BoundField HeaderText="Shipment Cleared Date" DataField="Shipment Cleared Date" />
                        <asp:BoundField HeaderText="Shipment Delivered Date" DataField="Shipment Delivered Date" />
                        <asp:BoundField HeaderText="Delivery Location" DataField="Delivery Location" />
                        <asp:BoundField HeaderText="Customs Holiday in between" DataField="Customs Holiday in Between" />
                        <asp:BoundField HeaderText="Clearance TAT" DataField="Clearance TAT" />
                        <asp:BoundField HeaderText="Billed Dt" DataField="Billed Dt" />
                        <asp:BoundField HeaderText="CDR upload Dt" DataField="CDR upload Dt" />
                        <asp:BoundField HeaderText="Status" DataField="" />
                        <asp:TemplateField HeaderText="Remarks(Day Wise Detail action)">
                            <ItemTemplate>
                                <div style="word-wrap: break-word; width: 400px; white-space: normal;">
                                    <asp:Label ID="lblRemark" runat="server" Text='<%#Bind("[Remarks Day Wise Detail action]") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Clearance PO No" DataField="Clearance PO No" />
                        <asp:BoundField HeaderText="Clearance Service Charges Invoice No" DataField="Clearance Invoice No" />
                        <asp:BoundField HeaderText="Clearance Service Charges Invoice Date" DataField="Clearance Invoice Date" />
                        <asp:BoundField HeaderText="Receipted Charges PO No" DataField="Receipted Charges PO No" />
                        <asp:BoundField HeaderText="Receipted Charges Invoice No" DataField="Receipted Invoice No" />
                        <asp:BoundField HeaderText="Receipted Charges Invoice Date" DataField="Receipted Invoice Date" />
                        <asp:BoundField HeaderText="CHA Invoice Status" DataField="CHA Invoice Status" />
                        <asp:BoundField HeaderText="Service Charges" DataField="Service Charges" />
                        <asp:BoundField HeaderText="Standard Storage charges" DataField="Standard Storage charges" />
                        <asp:BoundField HeaderText="Additional Storage charges" DataField="Additional Storage charges" />
                        <asp:BoundField HeaderText="Standard Custodian Charges" DataField="Standard Custodian Charges" />
                        <asp:BoundField HeaderText="Aai Demur/Cont Demur / Container Detention Rs." DataField="Aai Demur/Cont Demur / Container Detention Rs." />
                        <asp:BoundField HeaderText="Container Detention Rs." DataField="Container Detention Rs." />
                        <asp:BoundField HeaderText="Cfs Charges Rs." DataField="Cfs Charges Rs." />
                        <asp:BoundField HeaderText="Others" DataField="Others" />
                        <asp:BoundField HeaderText="GST" DataField="GST" />
                        <asp:BoundField HeaderText="Total Clearance" DataField="Total Clearance" />
                        <asp:BoundField HeaderText="Transporter Name" DataField="Transporter Name" />
                        <asp:BoundField HeaderText="LR No" DataField="LR No" />
                        <asp:BoundField HeaderText="Transport Invoice No." DataField="Transport Invoice No." />
                        <asp:BoundField HeaderText="Transport Invoice Dt" DataField="ransport Invoice Dt" />
                        <asp:BoundField HeaderText="Type OF Vehicle" DataField="Type OF Vehicle" />
                        <asp:BoundField HeaderText="Weight as per LR Kgs" DataField="Weight as per LR Kgs" />
                        <asp:BoundField HeaderText="Transportation Charges" DataField="Transportation Charges" />
                        <asp:BoundField HeaderText="Halting Days" DataField="Halting Days" />
                        <asp:BoundField HeaderText="Halting Charges" DataField="Halting Charges" />
                        <asp:BoundField HeaderText="Total Transport Charges" DataField="Total Transport Charges" />
                        <asp:BoundField HeaderText="BS Job No" DataField="JobRefNo" />
                        <asp:BoundField HeaderText="Reference" DataField="Reference" />--%>


                        <%--<asp:BoundField HeaderText="Date of Delivery to Warehouse" DataField="Date of Delivery to Warehouse" />--%>
                    </Columns>
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="DataSourceReport" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetFujiFilm_DSr" SelectCommandType="StoredProcedure">
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

