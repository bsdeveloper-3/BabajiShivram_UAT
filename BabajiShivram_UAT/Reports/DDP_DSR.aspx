<%@ Page Title="DDP DSR" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DDP_DSR.aspx.cs" 
    Inherits="Reports_DDP_DSR" Culture="en-GB" EnableEventValidation="false" %>
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
            <fieldset><legend>DDP DSR</legend>    
            <div>
                <asp:LinkButton ID="lnkReportXls" runat="server" OnClick="lnkReportXls_Click">
                    <asp:Image ID="imgExcel" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
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
                        <asp:BoundField HeaderText="ECC_PO" DataField="ECC_PO" />
                        <asp:BoundField HeaderText="ETA" DataField="ETA" />
                        <asp:BoundField HeaderText="Transit" DataField="Transit" />
                        <asp:BoundField HeaderText="Scan docs received date" DataField="Scan docs received date" />
                        <asp:BoundField HeaderText="Original docs received date" DataField="Original docs received date" />
                        <asp:BoundField HeaderText="CHA Job No" DataField="CHA Job No" />
                        <asp:BoundField HeaderText="B/E type" DataField="B/E type" />
                        <asp:BoundField HeaderText="No of Document" DataField="No of Document" />
                        <asp:BoundField HeaderText="GMID" DataField="GMID" />
                        <asp:BoundField HeaderText="Batch number" DataField="Batch number" />
                        <asp:BoundField HeaderText="Planner" DataField="Planner" />
                        <asp:BoundField HeaderText="Trail of events" DataField="Trail of events" />
                        <asp:BoundField HeaderText="KPI" DataField="KPI" />
                        <asp:BoundField HeaderText="Actual Time Take (For Cleared)" DataField="Actual Time Take (For Cleared)" />
                        <asp:BoundField HeaderText="Gap Days from KPI" DataField="Gap Days from KPI" />
                        <asp:BoundField HeaderText="Reason for Gap" DataField="Reason for Gap" />
                        <asp:BoundField HeaderText="Reason for Gap 2" DataField="Reason for Gap2" />
                        <asp:BoundField HeaderText="Det & Dem Paid" DataField="Det & Dem Paid" />
                        <asp:BoundField HeaderText="Late BOE Charge" DataField="Late BOE Charge" />
                        <asp:BoundField HeaderText="Total Avoidable Charge" DataField="Total Avoidable Charg" />
                        
                        <asp:BoundField HeaderText="Material_Description" DataField="Material_Description" />
                        <asp:BoundField HeaderText="Business" DataField="Business" />
                        <asp:BoundField HeaderText="HS_code" DataField="HS_code" />
                        <asp:BoundField HeaderText="Warehouse" DataField="Warehouse" />
                        <asp:BoundField HeaderText="Branchcode" DataField="Branchcode" />
                        <asp:BoundField HeaderText="Warehouse ID" DataField="Warehouse ID" />
                        <asp:BoundField HeaderText="Container_type" DataField="Container_type" />
                        <asp:BoundField HeaderText="No of Container" DataField="No of Container" />
                        <asp:BoundField HeaderText="Netwt(kg)_per_contanier" DataField="Netwt(kg)_per_contanier" />
                        <asp:BoundField HeaderText="Gr Wt (kg) per container" DataField="Gr Wt (kg) per container" />
                        <asp:BoundField HeaderText="No. of packages" DataField="No. of packages" />
                        <asp:BoundField HeaderText="Packages" DataField="Packages" />
                        <asp:BoundField HeaderText="Total_weight Available" DataField="Total_weight Available" />
                        <asp:BoundField HeaderText="Exbond Qty" DataField="Exbond Qty" />
                        <asp:BoundField HeaderText="After Exbond Balance Quantity" DataField="After Exbond Balance Quantity" />
                        <asp:BoundField HeaderText="Invoice Number" DataField="Invoice Number" />
                        <asp:BoundField HeaderText="Invoice_Date" DataField="Invoice_Date" />
                        <asp:BoundField HeaderText="Product Value (per Unit)" DataField="Product Value (per Unit)" />
                        <asp:BoundField HeaderText="Invoice Freight Value" DataField="Invoice Freight Value" />
                        <asp:BoundField HeaderText="Invoice Insurance Value" DataField="Invoice Insurance Value" />
                        <asp:BoundField HeaderText="TOTAL _CIF_Value" DataField="TOTAL _CIF_Value" />
                        <asp:BoundField HeaderText="Foreign Currency" DataField="Foreign Currency" />
                        <asp:BoundField HeaderText="Exchange _Rate" DataField="Exchange _Rate" />
                        <asp:BoundField HeaderText="CIF_(Rs)" DataField="CIF_(Rs)" />
                        <asp:BoundField HeaderText="Assesible Value" DataField="Assesible Value" />
                        <asp:BoundField HeaderText="Basic Duty (%)" DataField="Basic Duty (%)" />
                        <asp:BoundField HeaderText="Import Clearance with FTA/ Advance_Licence/ any other import licence" DataField="Import Clearance with FTA/ Advance_Licence/ any other import licence" />
                        <asp:BoundField HeaderText="Basic Duty Amount" DataField="Basic Duty Amount" />
                        <asp:BoundField HeaderText="Social Welfare Surcharge 10%" DataField="Social Welfare Surcharge 10%" />
                        <asp:BoundField HeaderText="Gross duty" DataField="Gross duty" />
                        <asp:BoundField HeaderText="IGST Duty %" DataField="IGST Duty %" />
                        <asp:BoundField HeaderText="IGST Duty Amount" DataField="IGST Duty Amount" />
                        <asp:BoundField HeaderText="GST Compn Cess Levy %" DataField="GST Compn Cess Levy %" />
                        <asp:BoundField HeaderText="GST Compn Cess Levy Amt." DataField="GST Compn Cess Levy Amt." />
                        <asp:BoundField HeaderText="Total Duty" DataField="Total Duty" />
                        <asp:BoundField HeaderText="Round Off _ Duty" DataField="Round Off _ Duty" />
                        <asp:BoundField HeaderText="Interest on Duty5,fv sa" DataField="Interest on Duty5,fv sa" />
                        <asp:BoundField HeaderText="Shipping Line" DataField="Shipping Line" />
                        <asp:BoundField HeaderText="Vessel Name" DataField="Vessel Name" />
                        <asp:BoundField HeaderText="BL number" DataField="BL number" />
                        <asp:BoundField HeaderText="BL Date" DataField="BL Date" />
                        <asp:BoundField HeaderText="container_no" DataField="container_no" />
                        <asp:BoundField HeaderText="Port of loading" DataField="Port of loading" />
                        <asp:BoundField HeaderText="Country of Origin" DataField="Country of Origin" />
                        <asp:BoundField HeaderText="Supplier" DataField="Supplier" />
                        <asp:BoundField HeaderText="IGM Date" DataField="IGM Date" />
                        <asp:BoundField HeaderText="POD" DataField="POD" />
                        <asp:BoundField HeaderText="CFS in Date" DataField="CFS in Date" />
                        <asp:BoundField HeaderText="CFS Name" DataField="CFS Name" />
                        <asp:BoundField HeaderText="Dow instruction date" DataField="Dow instruction date" />
                        <asp:BoundField HeaderText="Custom duty applied date" DataField="Custom duty applied date" />
                        <asp:BoundField HeaderText="Custom duty paid date" DataField="Custom duty paid date" />
                        <asp:BoundField HeaderText="Ready for dispatch date" DataField="Ready for dispatch date" />
                        <asp:BoundField HeaderText="Material dispatch date" DataField="Material dispatch date" />
                        <asp:BoundField HeaderText="WH in Date for Inbond" DataField="WH in Date for Inbond" />
                        <asp:BoundField HeaderText="DO validity Date" DataField="DO validity Date" />
                        <asp:BoundField HeaderText="Container Return date" DataField="Container Return date" />
                        <asp:BoundField HeaderText="Bond No." DataField="Bond No." />
                        <asp:BoundField HeaderText="Shipment handle Month" DataField="Shipment handle Month" />
                        <asp:BoundField HeaderText="B/E Number" DataField="B/E Number" />
                        <asp:BoundField HeaderText="B/E Date" DataField="B/E Date" />
                        <asp:BoundField HeaderText="B/E dispatch Date" DataField="B/E dispatch Date" />
                        <asp:BoundField HeaderText="Bill NO" DataField="Bill NO" />
                        <asp:BoundField HeaderText="BILL DATE" DataField="BILL DATE" />
                        <asp:BoundField HeaderText="Bill Dispatch Date" DataField="Bill Dispatch Date" />
                        <asp:BoundField HeaderText="Remarks" DataField="Remarks" />
                        <asp:BoundField HeaderText="HS Code Any issue" DataField="HS Code Any issue" />
                        <asp:BoundField HeaderText="Safety Isuues Any" DataField="Safety Isuues Any" />
                        <asp:BoundField HeaderText="DOW / Customer Complaint" DataField="DOW / Customer Complaint" />
                        <asp:BoundField HeaderText="CHA Clerance Time" DataField="CHA Clerance Time" />
                        <asp:BoundField HeaderText="Custom Duty Process No. of Days" DataField="Custom Duty Process No. of Days" />
                        <asp:BoundField HeaderText="Actual No. of days For the Clearance" DataField="Actual No. of days For the Clearance" />
                        <asp:BoundField HeaderText="Transporter Name" DataField="Transporter Name" />
                        <asp:BoundField HeaderText="Entity" DataField="Entity" />
                        <asp:BoundField HeaderText="Late BOE filling charges" DataField="Late BOE filling charges" />
                        <asp:BoundField HeaderText="Ocean Freight value in USD (BL/certificate)" DataField="Ocean Freight value in USD (BL/certificate)" />
                        <asp:BoundField HeaderText="DSR Uploaded Date" DataField="DSR Uploaded Date" />
                        <asp:BoundField HeaderText="RMS/NON RMS	" DataField="RMS/NON RMS" />
                        <asp:BoundField HeaderText="Clearance Reference number" DataField="Clearance Reference number" />
                        <%--<asp:BoundField HeaderText="Cleared & Uncleared" DataField="ClearedStatusName" />--%>
                    </Columns>
                </asp:GridView>
            </fieldset>  
            <div>
                <asp:SqlDataSource ID="DataSourceReport" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                 SelectCommand="rptDSRDDP" SelectCommandType="StoredProcedure"
                 FilterExpression=""   >
                <SelectParameters>
                    <asp:SessionParameter Name="UserId" SessionField="UserId" />
                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                </SelectParameters>
            </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

