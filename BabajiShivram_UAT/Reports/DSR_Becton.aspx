<%@ Page Title="DSR BECTON DICKINSON" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
        CodeFile="DSR_Becton.aspx.cs" Inherits="Reports_DSR_Becton" EnableEventValidation="false" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white" style="Width:80%; align-content:center;">
                <tr>
                    <td>
                        Job Date From
                        <cc1:CalendarExtender ID="CalExtJobFromDate" runat="server" Enabled="True" EnableViewState="False"
                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgFromDate" PopupPosition="BottomRight"
                            TargetControlID="txtFromDate">
                        </cc1:CalendarExtender>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate" runat="server" Width="100px" MaxLength="10" TabIndex="1" placeholder="dd/mm/yyyy" ></asp:TextBox>
                        <asp:Image ID="imgFromDate" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif"
                            runat="server"/>
                        <asp:CompareValidator ID="ComValFromDate" runat="server" ControlToValidate="txtFromDate" Display="Dynamic" Text="Invalid Date." Type="Date" 
                            CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="Required" SetFocusOnError="true" ErrorMessage="Invalid From Date">
                        </asp:CompareValidator>
                    </td>
                    <td>
                        Job Date To
                        <cc1:CalendarExtender ID="CalExtJobFromTo" runat="server" Enabled="True" EnableViewState="False"
                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgToDate" PopupPosition="BottomRight"
                            TargetControlID="txtToDate">
                        </cc1:CalendarExtender>
                    </td>
                    <td>
                        <asp:TextBox ID="txtToDate" runat="server" Width="100px" MaxLength="10" TabIndex="2" placeholder="dd/mm/yyyy" ></asp:TextBox>
                        <asp:Image ID="imgToDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                            runat="server"/>
                        <asp:CompareValidator ID="ComValToDate" runat="server" ControlToValidate="txtToDate" Display="Dynamic" ErrorMessage="Invalid To Date." Text="Invalid To Date." 
                            Type="Date" CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="Required" SetFocusOnError="true">
                        </asp:CompareValidator>    
                    </td>
                    <td>
                        <asp:DropDownList ID="ddClearedStatus" runat="server">
                            <asp:ListItem Value="" Text="All Job" ></asp:ListItem>
                            <asp:ListItem Value="1" Text="Cleared" ></asp:ListItem>
                            <asp:ListItem Value="0" Text="Un-Cleared" ></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;
                        <asp:Button ID="btnAddFilter" runat="server" Text="Add Filter" OnClick="btnShowReport_Click" />
                        &nbsp;&nbsp;
                        <asp:Button ID="btnClearFilter" runat="server" Text="Clear Filter" OnClick="btnClearFilter_Click" />
                    </td>
                </tr>
            </table>
            
            <div class="clear"></div>
            <div class="clear"></div>
            <fieldset><legend>DSR_Becton</legend>
            <div>
                <asp:LinkButton ID="lnkReportXls" runat="server" OnClick="lnkReportXls_Click">
                    <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
            </div>
            <div class="clear"></div>
                <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="False" CssClass="table"
                    ShowFooter="false" DataSourceID="DataSourceReport">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField HeaderText="JobDate" DataField="JobDate" />--%>
                        <asp:BoundField HeaderText="Month" DataField="Month" />
                        <asp:BoundField HeaderText="BD FY" DataField="BD FY" />
                        <asp:BoundField HeaderText="IMP NO" DataField="IMP NO" />
                        
                        <asp:BoundField HeaderText="BUSINESS" DataField="BUSINESS" />
                        <asp:BoundField HeaderText="Mode of shipment" DataField="Mode of shipment" />
                        <asp:BoundField HeaderText="MAWB/MBL" DataField="MAWB/MBL" />
                        <asp:BoundField HeaderText="HAWB/HBL" DataField="HAWB/HBL" />
                        <%--<asp:BoundField HeaderText="MAWB DATE" DataField="MAWB DATE" />--%>
                       
                        <asp:BoundField HeaderText="AWB DATE" DataField="AWB DATE" />
                       
                        <asp:BoundField HeaderText="Nature Of Goods(N/P/DG)" DataField="Nature Of Goods(N/P/DG)" />
                        <asp:BoundField HeaderText="FREIGHT FORWARDER" DataField="FREIGHT FORWARDER" />
                        <asp:BoundField HeaderText="No of packages" DataField="No of packages" />
                        <asp:BoundField HeaderText="No of Pallets" DataField="No of Pallets" />
                        <asp:BoundField HeaderText="40' Cntr" DataField="40' Cntr" />
                        <asp:BoundField HeaderText="20' Cntr" DataField="20' Cntr" />
                        <asp:BoundField HeaderText="LCL" DataField="LCL" />
                         <asp:BoundField HeaderText="Gross Weight(kg)" DataField="Gross Weight(kg)" />
                        <asp:BoundField HeaderText="Chargeable Weight(kg)" DataField="Chargeable Weight(kg)" />
                        <asp:BoundField HeaderText="SUPPLIER NAME" DataField="SUPPLIER NAME" />
                        <asp:BoundField HeaderText="Source" DataField="Source" />
                        <asp:BoundField HeaderText="Destination" DataField="Destination" />
                        <asp:BoundField HeaderText="CHA" DataField="CHA" />
                        <asp:BoundField HeaderText="ETD Source" DataField="ETD Source" />
                        <asp:BoundField HeaderText="ETA Destination" DataField="ETA Destination" />
                         <asp:BoundField HeaderText="Docs received Date" DataField="Docs received Date" />
                        <asp:BoundField HeaderText="DO Received Date" DataField="DO Received Date" />
                        <asp:BoundField HeaderText="DO CHARGES" DataField="DO CHARGES" />
                        <asp:BoundField HeaderText="TOTAL AAI CHARGES" DataField="TOTAL AAI CHARGES" />
                         <asp:BoundField HeaderText="DETENTION/DEMURRAGE" DataField="DETENTION" />
                        <asp:BoundField HeaderText="Freight" DataField="Freight" />
                        <asp:BoundField HeaderText="CURRENCY" DataField="CURRENCY" />
                        <asp:BoundField HeaderText="IMPORT BOE" DataField="IMPORT BOE" />
                        <asp:BoundField HeaderText="IMPORT BOE DATE" DataField="IMPORT BOE DATE" />
                         <asp:BoundField HeaderText="Clearance Date" DataField="Clearance Date" />
                        <asp:BoundField HeaderText="Free Period" DataField="Free Period" />
                        <asp:BoundField HeaderText="LG/Normal" DataField="LG/Normal" />
                        <asp:BoundField HeaderText="Date of Delivery to Warehouse" DataField="Date of Delivery to Warehouse" />
                        <asp:BoundField HeaderText="Days Of Customs Holidays" DataField="Days Of Customs Holidays" />
                        <asp:BoundField HeaderText="Agreed TAT" DataField="Agreed TAT" />
                        <asp:BoundField HeaderText="Actual TAT" DataField="Actual TAT" />
                        <asp:BoundField HeaderText="Comments" DataField="Comments" />
                        <asp:BoundField HeaderText="REGULATED / NON REGULATED" DataField="REGULATED / NON REGULATED" />
                        <asp:BoundField HeaderText="DELIVERY DATE TO STWZ" DataField="Delivery Date To FTWZ" />
                        <asp:BoundField HeaderText="LABELS RECEIVED DATE" DataField="LABELS RECEIVED DATE" />
                        <asp:BoundField HeaderText="VAS Qty" DataField="VAS Qty" />
                        <asp:BoundField HeaderText="VAS Start Date" DataField="VAS Start Date" />
                        <asp:BoundField HeaderText="VAS Completion Date" DataField="VAS Completion Date" />
                        <asp:BoundField HeaderText="CHECK LIST SHARED DATE" DataField="CHECK LIST SHARED DATE" />
                        <asp:BoundField HeaderText="CHECK LIST APPROVED DATE" DataField="CHECK LIST APPROVED DATE" />
                        <asp:BoundField HeaderText="DTA BOE No" DataField="DTA BOE No" />
                        <asp:BoundField HeaderText="DTA BOE Date" DataField="DTA BOE Date" />
                        <asp:BoundField HeaderText="Assessble Value in INR" DataField="Assessble Value in INR" />
                        <asp:BoundField HeaderText="Custom Duty" DataField="Custom Duty" />
                        <asp:BoundField HeaderText="IGST" DataField="IGST" />
                        <asp:BoundField HeaderText="DUTY REQUIST DATE" DataField="DUTY REQUIST DATE" />
                        <asp:BoundField HeaderText="TR6 No" DataField="TR6 No" />
                        <asp:BoundField HeaderText="ADC CLEARANCE DATE" DataField="ADC CLEARANCE DATE" />
                        <asp:BoundField HeaderText="DUTY PAID DATE" DataField="DUTY PAID DATE" />
                        <asp:BoundField HeaderText="Clearance Completion Date" DataField="Clearance Completion Date" />
                        <asp:BoundField HeaderText="Delivery Date To CSP" DataField="Delivery Date To CSP" />
                        
                         <asp:BoundField HeaderText="AGREED TAT" DataField="Agreed TAT" />
                         <asp:BoundField HeaderText="ACTUAL TAT" DataField="Actual TAT" />
                        <asp:BoundField HeaderText="Comments" DataField="Comments" />
                        <asp:BoundField HeaderText="Duty Interest" DataField="Duty Interest" />
                        <asp:BoundField HeaderText="Duty Penalty" DataField="Duty Penalty" />
                        <asp:BoundField HeaderText="Airlines" DataField="Airlines" />
                        <asp:BoundField HeaderText="Shipment Ref No" DataField="Shipment Ref No" />
                        <asp:BoundField HeaderText="Invoice No" DataField="Invoice No" />
                        <asp:BoundField HeaderText="PO No" DataField="PO No" />
                        <asp:BoundField HeaderText="BS Job No" DataField="BS Job No" />
                         <%--<asp:BoundField HeaderText="DEMURRAGE" DataField="DEMURRAGE" />--%>
                  
                        <%--<asp:BoundField HeaderText="Delivery To DC Date" DataField="Delivery To DC Date" />--%>
                        
                        <%--<asp:BoundField HeaderText="Fine Penalty Amount" DataField="Fine Penalty Amount" />--%>
                        
<%--                        <asp:BoundField HeaderText="Misc Charges" DataField="Misc Charges" />
                        <asp:BoundField HeaderText="CURRENCY2" DataField="CURRENCY2" />
                        --%>
                        <%--<asp:BoundField HeaderText="JobRefNo" DataField="JobRefNo" />
                        <asp:BoundField HeaderText="Current Status" DataField="Current Status" />--%>
                        <%--<asp:BoundField HeaderText="Job Activity" DataField="Job Activity" />--%>
                        <%--<asp:BoundField HeaderText="Duty TAT" DataField="Duty TAT" />
                        <asp:BoundField HeaderText="OOC TAT" DataField="OOC TAT" />
                        <asp:BoundField HeaderText="Cleared TAT" DataField="Cleared TAT" />--%>
                       
                    </Columns>
                </asp:GridView>
            </fieldset>  
            <div>
                <asp:SqlDataSource ID="DataSourceReport" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                 SelectCommand="rptDSRBecton" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="UserId" SessionField="UserId" />
                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                </SelectParameters>
            </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>