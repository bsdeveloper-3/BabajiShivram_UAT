<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Wabtec_BillingReport.aspx.cs" Inherits="Reports_Wabtec_BillingReport" %>

<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
                <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="False" CssClass="table" DataSourceID="DataSourceReport"
                    ShowFooter="false">
                    <%-- --%>
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField HeaderText="JobDate" DataField="JobDate" />--%>

                        
                        <asp:BoundField HeaderText="BS Job No" DataField="JobRefNo" />
                        <asp:BoundField HeaderText="Port" DataField="Port" />
                        <asp:BoundField HeaderText="Unit" DataField="Unit" />
                        <asp:BoundField HeaderText="Mode" DataField="Mode" />
                        <asp:BoundField HeaderText="Port Of Destination" DataField="Port Of Destination" />
                        <asp:BoundField HeaderText="BOE No" DataField="BOE No" />
                        <asp:BoundField HeaderText="BOE DATE" DataField="BOE DATE" />
                        <asp:BoundField HeaderText="Cust Ref No" DataField="CustRefNo" />
                        <asp:BoundField HeaderText="Billed Dt" DataField="Billed Dt" />
                        <asp:BoundField HeaderText="CDR upload Dt" DataField="CDR upload Dt" />
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
                        <asp:BoundField HeaderText="Reference" DataField="Reference" />

                    </Columns>
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="DataSourceReport" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="WabTec_BillingDSR_Report" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        <asp:ControlParameter ControlID="ddConsignee" Name="Consignee" PropertyName="SelectedValue" />
                        <asp:ControlParameter Name="DateFrom" ControlID="txtFromDate" PropertyName="Text" Type="datetime" />
                        <asp:ControlParameter Name="DateTo" ControlID="txtToDate" PropertyName="Text" Type="datetime"/>
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

