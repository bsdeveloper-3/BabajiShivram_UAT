<%@ Page Title="" Language="C#" MasterPageFile="~/CustomerMaster.master" AutoEventWireup="true" 
    CodeFile="JOHNSONDsrReport.aspx.cs" Inherits="Reports_JOHNSONDsrReport" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
                        <asp:TextBox ID="txtFromDate" runat="server" Width="100px" MaxLength="10" TabIndex="1" placeholder="dd/mm/yyyy" OnTextChanged="txtFromDate_TextChanged" AutoPostBack="true" ></asp:TextBox>
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
                        <asp:TextBox ID="txtToDate" runat="server" Width="100px" MaxLength="10" TabIndex="2" placeholder="dd/mm/yyyy" OnTextChanged="txtToDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                        <asp:Image ID="imgToDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                            runat="server"/>
                        <asp:CompareValidator ID="ComValToDate" runat="server" ControlToValidate="txtToDate" Display="Dynamic" ErrorMessage="Invalid To Date." Text="Invalid To Date." 
                            Type="Date" CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="Required" SetFocusOnError="true">
                        </asp:CompareValidator>    
                    </td>
                    <td>
                        <asp:DropDownList ID="ddClearedStatus" runat="server" OnSelectedIndexChanged="ddClearedStatus_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="" Text="All Job" ></asp:ListItem>
                            <asp:ListItem Value="1" Text="Cleared" ></asp:ListItem>
                            <asp:ListItem Value="0" Text="Un-Cleared" ></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;
                        <asp:Button ID="btnAddFilter" runat="server" Text="Add Filter"  OnClick="btnShowReport_Click" />
                        &nbsp;&nbsp;
                        <asp:Button ID="btnClearFilter" runat="server" Text="Clear Filter" OnClick="btnClearFilter_Click"  />
                    </td>
                </tr>
            </table>
            <div class="clear"></div>
            <div class="clear"></div>

            <fieldset><legend>JOHNSON-HITACHI DSR Report</legend>
            <div>
                <asp:LinkButton ID="lnkReportXls" runat="server"  OnClick="lnkReportXls_Click">
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
                        <asp:BoundField HeaderText="BS JOB NO" DataField="BS Job No" />
                        <asp:BoundField HeaderText="Clearing Location" DataField="LOCATION" />
                        <asp:BoundField HeaderText="Mode of Transport" DataField="AIR/SEA" />
                        <asp:BoundField HeaderText="Supplier" DataField="SUPPLIER" />
                        <asp:BoundField HeaderText="Your Ref No. Invoice No." DataField="INV NO" />
                        <asp:BoundField HeaderText="Invoice Date" DataField="INV DATE" />
                        <asp:BoundField HeaderText="Inco Term" DataField="SHIPMENT TERMS" />
                        <asp:BoundField HeaderText="Vessel Name" DataField="VesselName" />
                        <asp:BoundField HeaderText="BL NO." DataField="BL NO" />
                        <asp:BoundField HeaderText="BL DATE" DataField="BL DATE" />
                        <asp:BoundField HeaderText="CONTAINERS NO." DataField="ContainerList" />
                        <asp:BoundField HeaderText="20 Ft" DataField="20 FT" />
                        <asp:BoundField HeaderText="40 Ft" DataField="40 FT" />
                        <asp:BoundField HeaderText="LCL/Air Loose Delivery" DataField="LCL" />
                        <asp:BoundField HeaderText="ORACLE CODE" />
                        <asp:BoundField HeaderText="Goods Discription" DataField="Description" />
                        <asp:BoundField HeaderText="Qty" DataField="Quantity" />
                        <asp:BoundField HeaderText="Unit" DataField="unit" />
                        <asp:BoundField HeaderText="Unit Price" DataField="UnitPrice" />
                        <asp:BoundField HeaderText="Invoice Currency" DataField="FC" />
                         <asp:BoundField HeaderText="Invoice Value" DataField="InvoiceValue" />
                        <asp:BoundField HeaderText="Assessble Value" DataField="Assessable Value" />
                        <asp:BoundField HeaderText=" Basic (x%) " DataField="BasicDutyAmount" />
                        <asp:BoundField HeaderText=" Cess on cust duty (10%) " DataField="Cess on Cust Duty" />
                        <asp:BoundField HeaderText=" IGST (18% OR 28%)" DataField="IGST Amount" />
                        <asp:BoundField HeaderText="Gross Duty (BOE Amount)" DataField="GROSS DUTY" />
                        <asp:BoundField HeaderText="Docs received" DataField="Docs Received" />
                        <asp:BoundField HeaderText="Cont Bond informed" DataField="Cont Bond Informed" />
                        <asp:BoundField HeaderText="Cont Bond received" DataField="Cont Bond Received" />
                        <asp:BoundField HeaderText="Original Docs Received on" DataField="Original Docs Received On" />
                        <asp:BoundField HeaderText="Original FTA received on" DataField="Original FTA received On" />
                        <asp:BoundField HeaderText="Checklist Provided" DataField="ChecklistPreparedDate" />
                        <asp:BoundField HeaderText=" ETA " DataField="ETA" />
                        <asp:BoundField HeaderText=" IGM Filed" DataField="IGM FILED" />
                        <asp:BoundField HeaderText="BOE Filed" DataField="BOE DATE" />
                        <asp:BoundField HeaderText="Assessment Done" DataField="AssessmentDate" />
                        <asp:BoundField HeaderText="Duty informed" DataField="DutyPaymentDate" />
                        <asp:BoundField HeaderText="Duty Paid" DataField="Duty Paid" />
                        <asp:BoundField HeaderText="Phy Examination" DataField="Phy Examination" />
                        <asp:BoundField HeaderText=" OOC " DataField="OOC" />
                        <asp:BoundField HeaderText=" DO Dates" DataField="FinalDODate" />
                        <asp:BoundField HeaderText="Detention Start Date" DataField="Detention Start Date" />
                        <asp:BoundField HeaderText="Delivery Date from port" DataField="DeliveryPlanningDate" />
                        <asp:BoundField HeaderText="Empty Cont. Return Date" DataField="Empty Cont. Return Date" />
                        <asp:BoundField HeaderText="License required" DataField="License Required" />
                        <asp:BoundField HeaderText=" S.line " DataField="ShippingName" />
                        <asp:BoundField HeaderText=" CFS" DataField="CFSName" />
                        <asp:BoundField HeaderText="CHA" DataField="CHA" />
                        <asp:BoundField HeaderText="Free Days (CFS)" DataField="Free Days (CFS)" />
                        <asp:BoundField HeaderText="Free Days (S.line)" DataField="Free Days (S.line)" />
                        <asp:BoundField HeaderText="BE NO." DataField="BOE NO" />
                        <asp:BoundField HeaderText="BE Date" DataField="BOE DATE" />
                        <asp:BoundField HeaderText="Remarks" DataField="REMARKS" />
                        <asp:BoundField HeaderText="Detention amount" DataField="Detention Amount (INR)" />
                        <asp:BoundField HeaderText=" Ground Rent amount" DataField="Ground Rent Amount" />
                        <asp:BoundField HeaderText="Date"/>
                        <asp:BoundField HeaderText="Current Status" DataField="Current Status" />
                    </Columns>
                </asp:GridView>
            </fieldset>  
            <div>
                <asp:SqlDataSource ID="DataSourceReport" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                  SelectCommand="rptJOHNSONDsr" SelectCommandType="StoredProcedure">
                <SelectParameters>
                  <asp:Parameter Name="UserId" DefaultValue="1" />
                    <asp:Parameter Name ="FinYearId" DefaultValue="8" />
                    <asp:ControlParameter ControlID="ddClearedStatus" PropertyName="SelectedValue" Name="ClearedStatus" DefaultValue="2" />
                    <asp:ControlParameter Name="DateFrom" ControlID="txtFromDate" PropertyName="Text" Type="datetime" />
                    <asp:ControlParameter Name="DateTo" ControlID="txtToDate" PropertyName="Text" Type="datetime"/>
                    <%--<asp:SessionParameter Name="UserId" SessionField="UserId"  ConvertEmptyStringToNull="true"/>
                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" ConvertEmptyStringToNull="true" />--%>
                </SelectParameters>

            </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

