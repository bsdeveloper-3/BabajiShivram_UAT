<%@ Page Title="DDP KPI" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
        CodeFile="DDP_DSR_KPI.aspx.cs" Inherits="Reports_DDP_DSR_KPI" %>
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
            <fieldset><legend>DDP KPI</legend>    
            <div>
                <asp:LinkButton ID="lnkReportXls" runat="server" OnClick="lnkReportXls_Click">
                    <asp:Image ID="imgExcel" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
                &nbsp;&nbsp;&nbsp;&nbsp;<b>** A. KPI= ETA to OOC, InBond 5 Days And Home 2 Days, B. Actual Time Take (For Cleared)– Doc Open date to Cleared date. C. Gap Days from KPI- ETA to OOC</b>
            </div>
            <div class="clear"></div>
                <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="False" CssClass="table"
                    ShowFooter="false" DataSourceID="DataSourceReport" OnRowDataBound="gvReport_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Customer Ref No" DataField="Customer Ref No" />
                        <asp:BoundField HeaderText="ETA" DataField="ETA" />
                        <asp:BoundField HeaderText="Consignee" DataField="Consignee" />
                        <asp:BoundField HeaderText="ShippingName" DataField="ShippingName" />
                        <asp:BoundField HeaderText="BS Job No" DataField="BS Job No" />
                        <asp:BoundField HeaderText="Mode" DataField="Mode" />
                        <asp:BoundField HeaderText="Port of Discharge" DataField="Port of Discharge" />
                        <asp:BoundField HeaderText="Business" DataField="Business" />
                        <asp:BoundField HeaderText="Planner" DataField="Planner" />
                        <asp:BoundField HeaderText="Trail of events" DataField="Trail of events" />
                        <asp:BoundField HeaderText="Actual Time Take (For Cleared)" DataField="Actual Time Take (For Cleared)" />
                        <asp:BoundField HeaderText="KPI" DataField="KPI" />
                        <asp:BoundField HeaderText="Gap Days from KPI" DataField="Gap Days from KPI" />
                        <asp:BoundField HeaderText="Reason for Gap" DataField="Reason for Gap" />
                        <asp:BoundField HeaderText="Reason for Gap2" DataField="Reason for Gap2" />
                        <asp:BoundField HeaderText="Det & Dem Paid" DataField="Det & Dem Paid" />
                        <asp:BoundField HeaderText="Late BOE Charge" DataField="Late BOE Charge" />
                        <asp:BoundField HeaderText="Gap Days from KPI" DataField="Total Avoidable Charg" />
                        <asp:BoundField HeaderText="Cleared & Uncleared" DataField="ClearedStatusName" />
                    </Columns>
                </asp:GridView>
            </fieldset>  
            <div>
                <asp:SqlDataSource ID="DataSourceReport" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                 SelectCommand="rptDSRDDP_KPI" SelectCommandType="StoredProcedure"
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

