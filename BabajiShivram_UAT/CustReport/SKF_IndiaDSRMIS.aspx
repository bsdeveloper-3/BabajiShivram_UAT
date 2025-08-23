<%@ Page Title="SKF DSR MIS" Language="C#" AutoEventWireup="true" MasterPageFile="~/CustomerMaster.master"
    CodeFile="SKF_IndiaDSRMIS.aspx.cs" Inherits="CustReport_SKF_IndiaDSRMIS" Culture="en-GB" %>

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
                        <asp:DropDownList ID="ddClearedStatus" runat="server" OnSelectedIndexChanged="ddClearedStatus_SelectedIndexChanged"
                            AutoPostBack="true">
                            <%--<asp:ListItem Value="" Text="All Job" ></asp:ListItem>--%>
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
            <fieldset><legend><asp:Label ID="lblLegend" runat="server"></asp:Label></legend>
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
                        <asp:BoundField HeaderText="Babaji Branch" DataField="Babaji Branch" />
                        <asp:BoundField HeaderText="BS Job No" DataField="BS Job No" />
                        <asp:BoundField HeaderText="Job Creation Date" DataField="Job Creation Date" />
                        <asp:BoundField HeaderText="Mode" DataField="Mode" />
                        <asp:BoundField HeaderText="Shipment Control No" DataField="Shipment Control No" />
                        <asp:BoundField HeaderText="Shipment Date" DataField="Shipment Date" />
                        <asp:BoundField HeaderText="Project Type" DataField="Project Type" />
                        <asp:BoundField HeaderText="Invoice No" DataField="Invoice No" />
                        <asp:BoundField HeaderText="Invoice Date" DataField="Invoice Date" />
                        <asp:BoundField HeaderText="HBL/HAWBL No" DataField="HBL/HAWBL No" />
                        <asp:BoundField HeaderText="BOE Number" DataField="BOE Number" />
                        <asp:BoundField HeaderText="BOE Date" DataField="BOE Date" />
                        <asp:BoundField HeaderText="Assessable Value" DataField="Assessable Value" />
                        <asp:BoundField HeaderText="IGST" DataField="IGST" />
                        <asp:BoundField HeaderText="Non-recoverable duty" DataField="Non-recoverable duty" />
                        <asp:BoundField HeaderText="ETA" DataField="ETA" />
                        <asp:BoundField HeaderText="Delivery Point" DataField="Delivery Point" />
                        <asp:BoundField HeaderText="Progress Report" DataField="Progress Report" />
                        <asp:BoundField HeaderText="Delivery Status" DataField="Delivery Status" />
                        <asp:BoundField HeaderText="Delivery Type" DataField="Delivery Type" />
                        <asp:BoundField HeaderText="Dispatch Date" DataField="Dispatch Date" />
                        <asp:BoundField HeaderText="Container Size" DataField="Container Size" />
                        <asp:BoundField HeaderText="Date of Dispatch From CFS" DataField="Date of Dispatch From CFS" />
                        <asp:BoundField HeaderText="Container Type" DataField="Container Type" />

                        </Columns>
                </asp:GridView>
            </fieldset>  
            <div>
                <asp:SqlDataSource ID="DataSourceReport" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                 SelectCommand="rptSKFIndiaDSRMIS" SelectCommandType="StoredProcedure">
                <SelectParameters>
                  <asp:SessionParameter Name="UserId"  SessionField="CustUserId"  ConvertEmptyStringToNull="true" />
                    <asp:SessionParameter Name ="FinYearId" SessionField="FinYearId" ConvertEmptyStringToNull="true"  /> 
                    <asp:SessionParameter Name="CustomerId" SessionField="CustId" />
                    <asp:SessionParameter Name="Status" SessionField="Status" />
                    <%--<asp:SessionParameter Name="UserId" SessionField="UserId"  ConvertEmptyStringToNull="true" DefaultValue="6"/>
                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" ConvertEmptyStringToNull="true" />--%>
                </SelectParameters>

            </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>