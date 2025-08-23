<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FujiFilm_Report.aspx.cs" 
    Inherits="Reports_FujiFilm_Report" Culture="en-GB"%>

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
                    ShowFooter="false" ><%--DataSourceID="DataSourceReport"--%>
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Bs Job No" DataField="BS Job No" />
                        <asp:BoundField HeaderText="CHA Name" DataField="CHA" />
                        <asp:BoundField HeaderText="Customer Plant" DataField="Plant" />
                        <asp:BoundField HeaderText="Port Code" DataField="PortCode" />
                        <asp:BoundField HeaderText="Customer Division/Branch" DataField="Division" />
                        <asp:BoundField HeaderText="Term Of Invoice" DataField="TermsOfInvoice" />
                        <asp:BoundField HeaderText="Mode" DataField="Mode" />
                        <asp:BoundField HeaderText="Pre Alert Date" DataField="PreAlertDate" DataFormatString="{0:dd/MM/yy}"/>
                        <asp:BoundField HeaderText="Doc Received Time" DataField="Doc Received Time" />
                        <asp:BoundField HeaderText="Checklist Sending Date" DataField="Checklist Sending Date" DataFormatString="{0:dd/MM/yy}"/>
                        <asp:BoundField HeaderText="Checklist Sending Time" DataField="Checklist Sending Time" />
                        <asp:BoundField HeaderText="Checklist Approve time" DataField="Checklist Approve time" />
                        <asp:BoundField HeaderText="Invoice No" DataField="InvoiceNo" />
                        <asp:BoundField HeaderText="Invoice Date" DataField="Invoice Date" DataFormatString="{0:dd/MM/yy}"/>
                        <asp:BoundField HeaderText="WPC Remark" DataField="WPC REMARKS" /> 
                        <asp:BoundField HeaderText="Shipping Airline" DataField="Shipping/Airline" />
                        <asp:BoundField HeaderText="Console Agent" DataField="Console Agent" />
                        <asp:BoundField HeaderText="MBL/MAWBL No" DataField="MBL/MAWBL No" />
                        <asp:BoundField HeaderText="MBL/MAWBL Date" DataField="MBL/MAWBL Date" DataFormatString="{0:dd/MM/yy}"/>
                        <asp:BoundField HeaderText="HBL/HAWBL No" DataField="HBL/HAWBL No" />
                        <asp:BoundField HeaderText="HBL/HAWBL Date" DataField="HBL/HAWBL Date" DataFormatString="{0:dd/MM/yy}"/>
                        <asp:BoundField HeaderText="Type Of Shipment" DataField="Type Of Shipment" /> 
                        <asp:BoundField HeaderText="Short Description" DataField="Short Description" /> 
                        <asp:BoundField HeaderText="Sum Of 40" DataField="Sum Of 40" />
                        <asp:BoundField HeaderText="Sum Of 20" DataField="Sum Of 20" />
                        <asp:BoundField HeaderText="No Of Pallet" DataField="No of Pallets" />
                        <asp:BoundField HeaderText="Container No" DataField="Container No" />
                        <asp:BoundField HeaderText="Gross Weight" DataField="Gross Weight" />
                        <asp:BoundField HeaderText="No of package" DataField="No of package" />
                        <asp:BoundField HeaderText="Port Of Loading" DataField="Port Of Loading" />
                        <asp:BoundField HeaderText="Port Of Discharge" DataField="Port Of Discharge" /> 
                        <asp:BoundField HeaderText="Country Of Origin" DataField="Country Of Origin" />
                        <asp:BoundField HeaderText="1st ETA" DataField="1st ETA" DataFormatString="{0:dd/MM/yy}"/>   <%--Same Date prealert --%>
                        <asp:BoundField HeaderText="2nd ETA" DataField="2nd ETA" DataFormatString="{0:dd/MM/yy}"/>
                        <asp:BoundField HeaderText="Date Of Arrival" DataField="Date Of Arrival" DataFormatString="{0:dd/MM/yy}"/>
                        <asp:BoundField HeaderText="Berthing Date" DataField="Berthing Date" DataFormatString="{0:dd/MM/yy}"/> 
                        <asp:BoundField HeaderText="Expected Delivery Date" DataField="Expected Delivery Date" DataFormatString="{0:dd/MM/yy}"/>
                        <asp:BoundField HeaderText="ATD" DataField="ATD" /> 
                        <asp:BoundField HeaderText="IGM No" DataField="IGM No" />
                        <asp:BoundField HeaderText="IGM Date" DataField="IGM Date" DataFormatString="{0:dd/MM/yy}"/>
                        <asp:BoundField HeaderText="LCL Destuffing Date" DataField="LCL Destuffing Date" DataFormatString="{0:dd/MM/yy}"/> 
                        <asp:BoundField HeaderText="Bill of Entry No" DataField="Bill of Entry No" />
                        <asp:BoundField HeaderText="Bill Of Entry Date" DataField="Bill of Entry Date" DataFormatString="{0:dd/MM/yy}"/>
                        <asp:BoundField HeaderText="Invoice Currency" DataField="Invoice Currency" />
                        <asp:BoundField HeaderText="Invoice Freight Amount" DataField="Invoice Freight Amount" />
                        <asp:BoundField HeaderText="Insurance USD" DataField="INSURANCE /USD" />
                        <asp:BoundField HeaderText="Invoice Value INR" DataField="Invoice Value INR" />
                        <asp:BoundField HeaderText="Assessable Value" DataField="Assessable Value" />
                        <asp:BoundField HeaderText="%(AV/CIF RS.)" DataField="%(AV/CIF RS.)" /> 
                        <asp:BoundField HeaderText="Duty Amount" DataField="Duty Amount" />
                        <asp:BoundField HeaderText="%(Cust duty/AV)" DataField="%(CUSTODUTY/AV)" />
                        <asp:BoundField HeaderText="Demurrage Charges" DataField="Demurrage Charges" />                       
                        <asp:BoundField HeaderText="Insert Amount" DataField="Insert Amount" />
                        <asp:BoundField HeaderText="Duty Fine Amount" DataField="Duty Fine Amount" />
                        <asp:BoundField HeaderText="Short Reason" DataField="SHORT REASON" />
                        <asp:BoundField HeaderText="Responsible" DataField="Responsible" />
                        <asp:BoundField HeaderText="Claimable/Non Claimable" DataField="CLAIMABLE / NON CLAIMABLE" />
                        <asp:BoundField HeaderText="Recovery Form" DataField="Recovery From" />
                        <asp:BoundField HeaderText="Status From Recovery" DataField="Status of Recovery" />
                        <asp:BoundField HeaderText="Recovery Remark" DataField="Recovery Remarks" />
                        <asp:BoundField HeaderText="Truck Request Date" DataField="Truck Request Date" DataFormatString="{0:dd/MM/yy}"/>
                        <asp:BoundField HeaderText="Vehicle Placement Date" DataField="Vehicle Placement Date" DataFormatString="{0:dd/MM/yy}"/>
                        <asp:BoundField HeaderText="Port Entry Time" DataField="Port Entry Time" />
                        <asp:BoundField HeaderText="Port Exit time" DataField="Port Exit time" />
                        <asp:BoundField HeaderText="Custom Duty Intimated on" DataField="Customs Duty Intimated On" /> 
                        <asp:BoundField HeaderText="Duty Paid Date" DataField="Duty Paid Date" DataFormatString="{0:dd/MM/yy}"/>
                        <asp:BoundField HeaderText="BOE Field under Duty Deferment" DataField="BOE filed under Duty Deferment" />
                        <asp:BoundField HeaderText="Duty Challan no" DataField="Duty Challan no" />
                        <asp:BoundField HeaderText="Delivery Planning Date" DataField="Delivery Planning Date" DataFormatString="{0:dd/MM/yy}"/> 
                        <asp:BoundField HeaderText="Out of charge Date" DataField="Out of charge Date" DataFormatString="{0:dd/MM/yy}"/>
                        <asp:BoundField HeaderText="Date of Delivery at Warehouse" DataField="Date of Delivery at Warehouse" DataFormatString="{0:dd/MM/yy}"/>
                        <asp:BoundField HeaderText="Net Clearance Day" DataField="Net Clearance Days" />
                        <asp:BoundField HeaderText="Vehicle No" DataField="Vehicle No" />
                        <asp:BoundField HeaderText="Vehicle type" DataField="Vehicle type" />
                        <asp:BoundField HeaderText="Transporter Name" DataField="Transporter Name" />
                        <asp:BoundField HeaderText="CFS Movement" DataField="CFS Movement" /> 
                        <asp:BoundField HeaderText="DO Status" DataField="DO Status" />
                        <asp:BoundField HeaderText="DPD Delivery/CFS" DataField="DPD Delivery/CFS" />
                        <asp:BoundField HeaderText="Destuffing Status(For LCL)" DataField="Destuffing Status (for LCL)" />
                        <asp:BoundField HeaderText="Job Current Status" DataField="Job Current Status" />
                        <asp:BoundField HeaderText="Next Step" DataField="Next Step" />
                        <asp:BoundField HeaderText="Progress Report" DataField="Progress Report" />
                        <asp:BoundField HeaderText="ADC Put in custom" DataField="ADC Put In Custom" />
                        <asp:BoundField HeaderText="ADC Release" DataField="ADC Release" />
                        <asp:BoundField HeaderText="IGM NO2" DataField="IGMNo2" />
                        <asp:BoundField HeaderText="IGM Date2" DataField="IGMDate2" DataFormatString="{0:dd/MM/yy}"/>
                        <asp:BoundField HeaderText="Pism Registration Charges Paid" DataField="Pims Registration Charges Paid" />
                        
                   </Columns>
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="DataSourceReport" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="BS_rptFujiFilm_Report" SelectCommandType="StoredProcedure">  
                     <SelectParameters>
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        <asp:ControlParameter Name="FromDate" ControlID="txtFromdate" PropertyName="Text" Type="DateTime" />
                        <asp:ControlParameter Name="ToDate" ControlID="txtTodate" PropertyName="Text" Type="DateTime" />
                         <%--<asp:SessionParameter Name="JobDateFrom" SessionField="JobDateFrom" />
                            <asp:SessionParameter Name="ToJobDate" SessionField="ToJobDate" />--%>
                  </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
