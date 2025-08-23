<%@ Page Title="Becton Billing DSR" Language="C#" AutoEventWireup="true" CodeFile="BectonBillingDSR.aspx.cs" MasterPageFile="~/MasterPage.master"
    Inherits="Reports_BectonBillingDSR" %>

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
            <fieldset><legend>Becton Billing DSR</legend>
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
                        <asp:BoundField HeaderText="Scac" DataField="Scac" />
                        <asp:BoundField HeaderText="CustNo" DataField="CustNo" />
                        <asp:BoundField HeaderText="Pronum" DataField="Pronum" />
                        <asp:BoundField HeaderText="BillOfLading" DataField="BillOfLading" />
                        <asp:BoundField HeaderText="MB_MasterBol" DataField="MB_MasterBol" />
                        <asp:BoundField HeaderText="Prodate" DataField="Prodate" />
                        <asp:BoundField HeaderText="ShipDate" DataField="ShipDate" />
                        <asp:BoundField HeaderText="DelDate" DataField="DelDate" />
                        <asp:BoundField HeaderText="BillToNum" DataField="BillToNum" />
                        <asp:BoundField HeaderText="Mode" DataField="Mode" />
                        <asp:BoundField HeaderText="Shipper" DataField="Shipper" />
                        <asp:BoundField HeaderText="Oaddr1" DataField="Oaddr1" />
                        <asp:BoundField HeaderText="Ocity" DataField="Ocity" />
                        <asp:BoundField HeaderText="OstateProvience" DataField="OstateProvience" />
                        <asp:BoundField HeaderText="OpostCode" DataField="OpostCode" />
                        <asp:BoundField HeaderText="OCountryCode" DataField="OCountryCode" />
                        <asp:BoundField HeaderText="OPortCode" DataField="OPortCode" />
                        <asp:BoundField HeaderText="L1_OriginRegion" DataField="L1_OriginRegion" />
                        <asp:BoundField HeaderText="Consignee" DataField="Consignee" />
                        <asp:BoundField HeaderText="Daddr1" DataField="Daddr1" />
                        <asp:BoundField HeaderText="Dcity" DataField="Dcity" />
                        <asp:BoundField HeaderText="DstateProvience" DataField="DstateProvience" />
                        <asp:BoundField HeaderText="Dpostcode" DataField="Dpostcode" />
                        <asp:BoundField HeaderText="DcountryCode" DataField="DcountryCode" />
                        <asp:BoundField HeaderText="DPortCode" DataField="DPortCode" />
                        <asp:BoundField HeaderText="L2_DestinationRegion" DataField="L2_DestinationRegion" />
                        <asp:BoundField HeaderText="SvcLevel" DataField="SvcLevel" />
                        <asp:BoundField HeaderText="IncoTerms" DataField="IncoTerms" />
                        <asp:BoundField HeaderText="Weight" DataField="Weight" />
                        <asp:BoundField HeaderText="DimWeight" DataField="DimWeight" />
                        <asp:BoundField HeaderText="WeightUom" DataField="WeightUom" />
                        <asp:BoundField HeaderText="Volume" DataField="Volume" />
                        <asp:BoundField HeaderText="VolumeUom" DataField="VolumeUom" />
                        <asp:BoundField HeaderText="PackageType" DataField="PackageType" />
                        <asp:BoundField HeaderText="Pieces" DataField="Pieces" />
                        <asp:BoundField HeaderText="SecondaryCarrier" DataField="SecondaryCarrier" />
                        <asp:BoundField HeaderText="Code1" DataField="Code1" />
                        <asp:BoundField HeaderText="Amt1" DataField="Amt1" />
                        <asp:BoundField HeaderText="RateQualifier1" DataField="RateQualifier1" />
                        <asp:BoundField HeaderText="AuditAux1" DataField="AuditAux1" />
                        <asp:BoundField HeaderText="RateValue1" DataField="RateValue1" />
                        <asp:BoundField HeaderText="Currency" DataField="Currency" />
                        <asp:BoundField HeaderText="BilledAmt" DataField="BilledAmt" />
                        <asp:BoundField HeaderText="ExchangeRate" DataField="ExchangeRate" />
                        <asp:BoundField HeaderText="AuxDate1" DataField="AuxDate1" />
                        <asp:BoundField HeaderText="Currency2" DataField="Currency2" />
                        <asp:BoundField HeaderText="T1_Totalamounttaxableat18%" DataField="T1_Totalamounttaxableat18%" />
                        <asp:BoundField HeaderText="T2_Totalamounttaxableat0%" DataField="T2_Totalamounttaxableat0%" />
                        <asp:BoundField HeaderText="T3_TotalGSTamountat18%" DataField="T3_TotalGSTamountat18%" />
                        <asp:BoundField HeaderText="T4_TotalBilledamount" DataField="T4_TotalBilledamount" />
                        <asp:BoundField HeaderText="T5_TotalamountexemptedfromTax" DataField="T5_TotalamountexemptedfromTax" />
                        <asp:BoundField HeaderText="T6_Totalamounttaxableat5%" DataField="T6_Totalamounttaxableat5%" />
                        <asp:BoundField HeaderText="T7_TotalGSTamountat5%" DataField="T7_TotalGSTamountat5%" />
                        <asp:BoundField HeaderText="T8_Totalamounttaxable12%" DataField="T8_Totalamounttaxable12%" />
                        <asp:BoundField HeaderText="T9_TotalGSTamountat12%" DataField="T9_TotalGSTamountat12%" />
                        <asp:BoundField HeaderText="T10_Totalamounttaxable28%" DataField="T10_Totalamounttaxable28%" />
                        <asp:BoundField HeaderText="T11_TotalGSTamountat28%" DataField="T11_TotalGSTamountat28%" />
                        <asp:BoundField HeaderText="I5_InvoiceType" DataField="I5_InvoiceType" />
                        <asp:BoundField HeaderText="VX_TaxRegistration" DataField="VX_TaxRegistration" />
                        <asp:BoundField HeaderText="CR_BDShipmentReference1" DataField="CR_BDShipmentReference1" />
                        <asp:BoundField HeaderText="CR_BDShipmentReference2" DataField="CR_BDShipmentReference2" />
                        <asp:BoundField HeaderText="CR_BDShipmentReference3" DataField="CR_BDShipmentReference3" />
                        <asp:BoundField HeaderText="CR_BDShipmentReference4" DataField="CR_BDShipmentReference4" />
                        <asp:BoundField HeaderText="CR_BDShipmentReference5" DataField="CR_BDShipmentReference5" />
                        
                        </Columns>
                </asp:GridView>
            </fieldset>  
            <div>
                <asp:SqlDataSource ID="DataSourceReport" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                 SelectCommand="rptDSRBectonBilling" SelectCommandType="StoredProcedure">
                <SelectParameters>
                  <asp:Parameter Name="UserId" DefaultValue="1" />
                  <asp:Parameter Name ="FinYearId" DefaultValue="6" />

                    <%--<asp:SessionParameter Name="UserId" SessionField="UserId"  ConvertEmptyStringToNull="true"/>
                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" ConvertEmptyStringToNull="true" />--%>
                </SelectParameters>

            </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>