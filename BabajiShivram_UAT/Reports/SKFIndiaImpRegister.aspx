<%@ Page Title="SKF Import Register" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="SKFIndiaImpRegister.aspx.cs" Inherits="Reports_SKFIndiaImpRegister" Culture="en-GB" %>
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
                        <asp:Button ID="btnAddFilter" runat="server" Text="Add Filter"  OnClick="btnShowReport_Click" />
                        &nbsp;&nbsp;
                        <asp:Button ID="btnClearFilter" runat="server" Text="Clear Filter" OnClick="btnClearFilter_Click"  />
                    </td>
                </tr>
            </table>
            
            <div class="clear"></div>
            <div class="clear"></div>
            <fieldset><legend>
                <asp:Label ID="lblLegend" runat="server"></asp:Label>
                      </legend>
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
                        <asp:BoundField HeaderText="LOCATION" DataField="LOCATION" />
                        <asp:BoundField HeaderText="SHIPMENT NO" DataField="SHIPMENT NO" />
                        <asp:BoundField HeaderText="Shipment Date" DataField="Shipment Date" />
                        <asp:BoundField HeaderText="AIR/SEA" DataField="AIR/SEA" />
                        <asp:BoundField HeaderText="SUPPLIER" DataField="Supplier" />
                        <asp:BoundField HeaderText="MATERIAL DESCRIPTION" DataField="Material Description" />
                        <asp:BoundField HeaderText="PO NO" DataField="PO NO" />
                        <asp:BoundField HeaderText="INV NO" DataField="INV NO" />
                        <asp:BoundField HeaderText="INV DATE" DataField="INV DATE" />
                        <asp:BoundField HeaderText="INV QUANTITY" DataField="INV QUANTITY" />
                        <asp:BoundField HeaderText="UNIT" DataField="UNIT" />
                        <asp:BoundField HeaderText="INVOICE RATE" DataField="Invoice Rate" />
                        <asp:BoundField HeaderText="OTHER" DataField="OTHER" />
                        <asp:BoundField HeaderText="FC" DataField="FC" />
                        <asp:BoundField HeaderText="INV AMT" DataField="INV AMT" />
                        <asp:BoundField HeaderText="SHIPMENT TERMS" DataField="SHIPMENT TERMS" />
                        <asp:BoundField HeaderText="Freight Charges Mention on Forwarder Invoice" DataField="Freight Charges Mention on Forwarder Invoice" />
                        <asp:BoundField HeaderText="TRANSPORTATION AMOUNT" DataField="TRANSPORTATION AMOUNT" />
                        <asp:BoundField HeaderText="EX-RATE" DataField="EX-RATE" />
                         <asp:BoundField HeaderText="BOE DATE" DataField="BOE DATE" />
                        <asp:BoundField HeaderText="BOE NO" DataField="BOE NO" />
                        <asp:BoundField HeaderText="ASSESSABLE VALUE" DataField="Assessable Value" />
			<asp:BoundField HeaderText="Social Welfare Surcharge" DataField="Social Welfare Surcharge" />
                        <asp:BoundField HeaderText="IGST Amount" DataField="IGST Amount" />
                        <asp:BoundField HeaderText="DEPB LIC NO" DataField="DEPB LIC NO" />
                        <asp:BoundField HeaderText="DEPB AMT" DataField="DEPB AMT" />
                        <asp:BoundField HeaderText="% OF DUTY/DEPB" DataField="% OF DUTY/DEPB" />
                        <asp:BoundField HeaderText="GROSS WT" DataField="GROSS WT" />
                        <asp:BoundField HeaderText="AIRWAY HOUSE B/L NO" DataField="AIRWAY HOUSE B/L NO" />
                        <asp:BoundField HeaderText="AIRWAY HOUSE B/L DATE" DataField="AIRWAY HOUSE B/L DATE" />
                        <asp:BoundField HeaderText="MARKS BILL NO" DataField="MARKS BILL NO" />
                        <asp:BoundField HeaderText="SERVICE TAX AMT" DataField="SERVICE TAX AMT" />
                        <asp:BoundField HeaderText="BILL/CLEARED DATE" DataField="BILL/CLEARED DATE" />
                        <asp:BoundField HeaderText="FREIGHT AMT" DataField="FREIGHT AMT" />
                        <asp:BoundField HeaderText="TAXABLE VALUE" DataField="TAXABLE VALUE" />
                        <asp:BoundField HeaderText="MFG NO" DataField="MFG NO" />
                        <asp:BoundField HeaderText="MARKS (JOB NO)" DataField="MARKS (JOB NO)" />
                    </Columns>
                </asp:GridView>
            </fieldset>  
            <div>
                <asp:SqlDataSource ID="DataSourceReport" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                 SelectCommand="rptSKFIndiaImpRegister" SelectCommandType="StoredProcedure">
                <SelectParameters>
                  <%--<asp:Parameter Name="UserId" DefaultValue="1" />
                    <asp:Parameter Name ="FinYearId" DefaultValue="6" />--%>
                    <asp:SessionParameter Name="UserId" SessionField="UserId" />
                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    <asp:SessionParameter Name="CustomerId" SessionField="CustId" />
                    <%--<asp:SessionParameter Name="UserId" SessionField="UserId"  ConvertEmptyStringToNull="true"/>
                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" ConvertEmptyStringToNull="true" />--%>
                </SelectParameters>

            </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
