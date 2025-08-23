<%@ page title="" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="wabtec_DSR_report.aspx.cs" inherits="Reports_webtec_DSR_report" %>

<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />

    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="updWebtecReport" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="updWebtecReport" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white" style="width: 100%; align-content: center;">
                <tr>
                    <td>Job Date From
                        <cc1:calendarextender id="CalExtJobFromDate" runat="server" enabled="True" enableviewstate="False"
                            firstdayofweek="Sunday" Format="dd-MMM-yyyy" popupbuttonid="imgFromDate" popupposition="BottomRight"
                            targetcontrolid="txtFromDate">
                        </cc1:calendarextender>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate" runat="server" Width="100px" MaxLength="10" TabIndex="1" placeholder="dd/mm/yyyy"></asp:TextBox>
                        <asp:Image ID="imgFromDate" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif"
                            runat="server" />
                        <%--<asp:CompareValidator ID="ComValFromDate" runat="server" ControlToValidate="txtFromDate" Display="Dynamic" Text="Invalid Date." Type="Date"
                            CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="Required" SetFocusOnError="true" ErrorMessage="Invalid From Date">
                        </asp:CompareValidator>--%>
                    </td>
                    <td>Job Date To
                        <cc1:calendarextender id="CalExtJobFromTo" runat="server" enabled="True" enableviewstate="False"
                            firstdayofweek="Sunday" Format="dd-MMM-yyyy" popupbuttonid="imgToDate" popupposition="BottomRight"
                            targetcontrolid="txtToDate">
                        </cc1:calendarextender>
                    </td>
                    <td>
                        <asp:TextBox ID="txtToDate" runat="server" Width="100px" MaxLength="10" TabIndex="2" placeholder="dd/mm/yyyy"></asp:TextBox>
                        <asp:Image ID="imgToDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                            runat="server" />
                        <%--<asp:CompareValidator ID="ComValToDate" runat="server" ControlToValidate="txtToDate" Display="Dynamic" ErrorMessage="Invalid To Date." Text="Invalid To Date."
                            Type="Date" CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="Required" SetFocusOnError="true">
                        </asp:CompareValidator>--%>
                    </td>
                    <td>Consignee Name</td>
                    <td>
                        <asp:DropDownList ID="ddConsignee" runat="server" DataSourceID="DataSourceDivision" DataTextField="DivisionName" DataValueField="lid" AppendDataBoundItems="true">
                            <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddClearedStatus" runat="server">
                            <%--<asp:ListItem Value="" Text="All Job"></asp:ListItem>--%>
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
            <div>
                <div id="DivDataSrouce222">
                    <asp:SqlDataSource ID="DataSourceDivision" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="GetCustomerDivision" SelectCommandType="StoredProcedure" DeleteCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:Parameter Name="CustomerId" DefaultValue="19994" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </div>
            <div class="clear"></div>
            <div class="clear"></div>

            <fieldset>
                <legend>Webtec DSR Report</legend>
                <div>
                    <asp:LinkButton ID="lnkReportXls" runat="server" OnClick="lnkReportXls_Click">
                        <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                </div>
                <div class="clear"></div>
                <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="False" CssClass="table" 
                    ShowFooter="false">
                    <%-- DataSourceID="DataSourceReport"--%>
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField HeaderText="JobDate" DataField="JobDate" />--%>

                        <asp:BoundField HeaderText="Key number" DataField="Key number" />
                        <asp:BoundField HeaderText="BS Job No" DataField="JobRefNo" />
                        <asp:BoundField HeaderText="Port" DataField="Port" />
                        <asp:BoundField HeaderText="Unit" DataField="Unit" />
                        <asp:BoundField HeaderText="Month" DataField="month" />
                        <asp:BoundField HeaderText="Status Code" DataField="Status Code" />
                        <asp:BoundField HeaderText="FFName" DataField="FFName" />
                        <asp:BoundField HeaderText="Mode" DataField="Mode" />
                        <asp:BoundField HeaderText="Importer Name" DataField="Importer Name" />
                        <asp:BoundField HeaderText="Supplier Name" DataField="Supplier Name" />
                        <asp:BoundField HeaderText="Country Of Export" DataField="Country Of Export" />
                        <asp:BoundField HeaderText="Country Of Origin" DataField="Country Of Export" />
                        <asp:BoundField HeaderText="Port Of Loading" DataField="Port Of Loading" />
                        <asp:BoundField HeaderText="Port Of Destination" DataField="Port Of Destination" />
                        <asp:BoundField HeaderText="Inco Terms" DataField="Inco Terms" />
                        <asp:BoundField HeaderText="Master No" DataField="Master No" />
                        <asp:BoundField HeaderText="House No" DataField="House No" />
                        <asp:BoundField HeaderText="ETD" DataField="ETD" />
                        <asp:BoundField HeaderText="ETA Date" DataField="ETA Date" />
                        <asp:BoundField HeaderText="ATA Date" DataField="ATA Date" />
                        <asp:BoundField HeaderText="Date" DataField="Date" />
                        <asp:BoundField HeaderText="Packages" DataField="Packages" />
                        <asp:BoundField HeaderText="Gross Weight" DataField="Gross Weight" />
                        <asp:BoundField HeaderText="CBM/ CHG Wt" DataField="CBM CHG WT" />
                        <asp:BoundField HeaderText="CBM 2 Wt" DataField="CBM 2 WT" />
                        <asp:BoundField HeaderText="Report Mode Type" DataField="Report Mode Type" />
                        <asp:BoundField HeaderText="Liner/Agent" DataField="Liner" />
                        <asp:BoundField HeaderText="Container No" DataField="Container No" />
                        <asp:BoundField HeaderText="Count 20" DataField="Count 20" />
                        <asp:BoundField HeaderText="Count 40" DataField="Count 40" />
                        <asp:BoundField HeaderText="Vessel Name Flight No(Departure)" DataField="Vessel Name Flight No(Departure)" />
                        <asp:BoundField HeaderText="Vessel Name Flight No(Arrival)" DataField="Vessel Name Flight No(Arrival)" />
                        <asp:BoundField HeaderText="De-Stuffing location( for Sea)" DataField="De-Stuffing location( for Sea)" />
                        <asp:BoundField HeaderText="De-Stuffing Dt" DataField="De-Stuffing Dt" />
                        <asp:BoundField HeaderText="Cl Request Dt" DataField="Cl Request Dt" />
                        <asp:BoundField HeaderText="Cl Approve Dt" DataField="Cl Approve Dt" />
                        <asp:BoundField HeaderText="IGM No" DataField="IGM No" />
                        <asp:BoundField HeaderText="IGM Date" DataField="IGM Date" />
                        <asp:BoundField HeaderText="Container Inward Date" DataField="Inward Date" />
                        <asp:TemplateField HeaderText="Supplier Invoice No">
                            <ItemTemplate>
                                <div style="word-wrap: break-word; width: 400px; white-space: normal;">
                                    <asp:Label ID="lblRemark1" runat="server" Text='<%#Bind("[Supplier Invoice No]") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Supplier Invoice Date">
                            <ItemTemplate>
                                <div style="word-wrap: break-word; width: 400px; white-space: normal;">
                                    <asp:Label ID="lblRemark" runat="server" Text='<%#Bind("[Supplier Invoice Date]") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Supplier Invoice Value">
                            <ItemTemplate>
                                <div style="word-wrap: break-word; width: 400px; white-space: normal;">
                                    <asp:Label ID="lblRemark" runat="server" Text='<%#Bind("[Supplier Invoice Value]") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Currency" DataField="Currency" />
                        <asp:BoundField HeaderText="Related party Import (SVB)" DataField="" />
                        <asp:BoundField HeaderText="BOE No" DataField="BOE No" />
                        <asp:BoundField HeaderText="BOE DATE" DataField="BOE DATE" />
                        <asp:BoundField HeaderText="Clearance Type" DataField="Clearance Type" />
                        <asp:BoundField HeaderText="Assesment Type" DataField="Assesment Type" />
                        <asp:BoundField HeaderText="Import catogery " DataField="Import Category" />
                        <asp:BoundField HeaderText="Schemes" DataField="Schemes" />
                        <asp:BoundField HeaderText="FOB value " DataField="FOB value" />
                        <asp:BoundField HeaderText="Freight" DataField="Freight" />
                        <asp:BoundField HeaderText="Misc" DataField="Misc" />
                        <asp:BoundField HeaderText="Insurance" DataField="Insurance" />
                        <asp:TemplateField HeaderText="Exchange Rate">
                            <ItemTemplate>
                                <div style="word-wrap: break-word; width: 400px; white-space: normal;">
                                    <asp:Label ID="Label1" runat="server" Text='<%#Bind("[Exchange Rate]") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                       <%-- <asp:BoundField HeaderText="Exchange Rate" DataField="Exchange Rate" />--%>
                        <asp:BoundField HeaderText="Assessable Value" DataField="Assessable Value" />
                        <asp:BoundField HeaderText="BCD Forgone(Scrips)" DataField="BCD Forgone(Scrips)" />
                        <asp:BoundField HeaderText="BCD Paid" DataField="BCD Paid" />
                        <asp:BoundField HeaderText="SWS" DataField="SWS" />
                        <asp:BoundField HeaderText="IGST" DataField="IGST" />
                        <asp:BoundField HeaderText="Customs Duty(in INR)" DataField="Customs Duty(in INR)" />
                        <asp:BoundField HeaderText="Interest" DataField="Interest" />
                        <asp:BoundField HeaderText="Penalty" DataField="Penalty" />
                        <asp:BoundField HeaderText="Tot Custom Duty (in INR)" DataField="Tot Custom Duty (in INR)" />
                        <asp:BoundField HeaderText="Duty Req Dt" DataField="Duty Req Dt" />
                        <asp:BoundField HeaderText="Duty Paid Dt" DataField="Duty Paid Dt" />
                        <asp:BoundField HeaderText="TR6 No" DataField="TR6 No" />
                        <asp:BoundField HeaderText="TR6 Date" DataField="TR6 Date" />
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
                       <%-- <asp:BoundField HeaderText="Billed Dt" DataField="Billed Dt" />
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
                        <asp:BoundField HeaderText="Clearance Service Charges Invoice Amount" DataField="Clearance Invoice Amount" />
                        <asp:BoundField HeaderText="Receipted Charges PO No" DataField="Receipted Charges PO No" />
                        <asp:BoundField HeaderText="Receipted Charges Invoice No" DataField="Receipted Invoice No" />
                        <asp:BoundField HeaderText="Receipted Charges Invoice Date" DataField="Receipted Invoice Date" />
                        <asp:BoundField HeaderText="Receipted Charges Invoice Amount" DataField="Receipted Invoice Amount" />
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
                        <asp:BoundField HeaderText="Transport By" DataField="TransportationBy" />
                        <asp:BoundField HeaderText="Transporter Name" DataField="Transporter Name" />
                        <asp:BoundField HeaderText="LR No" DataField="LR No" />
                        <asp:BoundField HeaderText="Transport Invoice No." DataField="Transport Invoice No." />
                        <asp:BoundField HeaderText="Transport Invoice Dt" DataField="Transport Invoice Dt" />
                        <asp:BoundField HeaderText="Type OF Vehicle" DataField="Type OF Vehicle" />
                        <asp:BoundField HeaderText="Weight as per LR Kgs" DataField="Weight as per LR Kgs" />
                        <asp:BoundField HeaderText="Transportation Charges" DataField="Transportation Charges" />
                        <asp:BoundField HeaderText="Halting Days" DataField="Halting Days" />
                        <asp:BoundField HeaderText="Halting Charges" DataField="Halting Charges" />
                        <asp:BoundField HeaderText="Total Transport Charges" DataField="Total Transport Charges" />
                        <asp:BoundField HeaderText="Reference" DataField="Reference" />--%>


                        <%--<asp:BoundField HeaderText="Date of Delivery to Warehouse" DataField="Date of Delivery to Warehouse" />--%>
                    </Columns>
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="DataSourceReport" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="WabTec_DSR_ReportTest" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        <asp:ControlParameter ControlID="ddClearedStatus" Name="Status" PropertyName="SelectedValue" />
                        <asp:ControlParameter ControlID="ddConsignee" Name="Consignee" PropertyName="SelectedValue" />
                        <asp:ControlParameter Name="DateFrom" ControlID="txtFromDate" PropertyName="Text" Type="datetime" />
                        <asp:ControlParameter Name="DateTo" ControlID="txtToDate" PropertyName="Text" Type="datetime"/>
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

