<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ThermaxLTD.aspx.cs" Inherits="Reports_ThermaxLTD" %>

<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />

    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="updThermaxReport" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="updThermaxReport" runat="server" UpdateMode="Conditional" RenderMode="Inline">
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
                            <asp:ListItem Value="" Text="All Job"  ></asp:ListItem>
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
                <legend>Thermax Report</legend>
                <div>
                    <asp:LinkButton ID="lnkReportXls" runat="server" OnClick="lnkReportXls_Click">
                        <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                </div>
                <div class="clear"></div>
                <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="False" CssClass="table"
                    ShowFooter="false">
                    <%--  DataSourceID="DataSourceReport" --%>
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField HeaderText="JobDate" DataField="JobDate" />--%>
                       <asp:BoundField HeaderText="HBL Number" DataField="HBL Number" />
                       <asp:BoundField HeaderText="Job Number" DataField="Job Number" />
                       <asp:BoundField HeaderText="Job Date" DataField="Job Date" />
                        <asp:BoundField HeaderText="CB Code" DataField="CB Code" />
                        <asp:BoundField HeaderText="CB Branch Code" DataField="CB Branch Code" />
                        <asp:BoundField HeaderText="In-Bond BoE No." DataField="In-Bond BoE No." />
                        <asp:BoundField HeaderText="In-Bond BoE Date" DataField="In-Bond BoE Date" />
                        <asp:BoundField HeaderText="Warehouse Code" DataField="Warehouse Code" />
                         <asp:BoundField HeaderText="Custom Site ID" DataField="Custom Site ID" />
                        <asp:BoundField HeaderText="Bond No." DataField="Bond No." />
                        <asp:BoundField HeaderText="Bond Date" DataField="Bond Date" />
                        <asp:BoundField HeaderText="Bond Expiry" DataField="Bond Expiry" />
                        <asp:TemplateField HeaderText="Remarks 1">
                            <ItemTemplate>
                                <div style="word-wrap: break-word; width: 400px; white-space: normal;">
                                    <asp:Label ID="lblRemark" runat="server" Text='<%#Bind("[Remarks 1]") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="BL Number" DataField="BL Number" />
                        <asp:BoundField HeaderText="IGM No." DataField="IGM No." />
                        <asp:BoundField HeaderText="IGM Date" DataField="IGM Date" />
                        <asp:BoundField HeaderText="Inward Date" DataField="Inward Date" />
                        <asp:BoundField HeaderText="Import Invoice No." DataField="Import Invoice No." />
                        <asp:BoundField HeaderText="Import Invoice Date" DataField="Import Invoice Date" />
                        <asp:BoundField HeaderText="INCOTERMS As per BOE" DataField="INCOTERMS As per BOE" />
                        <asp:BoundField HeaderText="Invoice Currency" DataField="Invoice Currency" />
                        <asp:BoundField HeaderText="Invoice Exchange Rate" DataField="Invoice Exchange Rate" />
                        <asp:BoundField HeaderText="Invoice Value (FC)" DataField="Invoice Value (FC)" />
                        <asp:BoundField HeaderText="Invoice Value (INR)" DataField="Invoice Value (INR)" />
                        <asp:BoundField HeaderText="BoE Freight Currency" DataField="BoE Freight Currency" />
                        <asp:BoundField HeaderText="Freight Exchange Rate" DataField="Freight Exchange Rate" />
                        <asp:BoundField HeaderText="Freight %" DataField="Freight %" />
                        <asp:BoundField HeaderText="Freight Value in BoE" DataField="Freight Value in BoE" />
                        <asp:BoundField HeaderText="Insurance Currency" DataField="Insurance Currency" />
                        <asp:BoundField HeaderText="Insurance Exchange Rate" DataField="Insurance Exchange Rate" />
                        <asp:BoundField HeaderText="Insurance %" DataField="Insurance %" />
                        <asp:BoundField HeaderText="Insurance Value in BoE" DataField="Insurance Value in BoE" />
                        <asp:BoundField HeaderText="Misc. Charges Currency" DataField="Misc. Charges Currency" />
                        <asp:BoundField HeaderText="Misc. Charges Exchange Rate" DataField="Misc. Charges Exchange Rate" />
                        <asp:BoundField HeaderText="Misc. Charges %" DataField="Misc. Charges %" />
                        <asp:BoundField HeaderText="Misc. Charges Value in BoE" DataField="Misc. Charges Value in BoE" />
                        <asp:BoundField HeaderText="Nature of Transaction" DataField="Nature of Transaction" />
                        <asp:BoundField HeaderText="Terms of Payment" DataField="Terms of Payment" />
                        <asp:BoundField HeaderText="Buyer Seller Related" DataField="Buyer Seller Related" />
                        <asp:BoundField HeaderText="HS CODE" DataField="HS CODE" />
                        <asp:BoundField HeaderText="Part No." DataField="Part No." />
                        <asp:TemplateField HeaderText="Product Description">
                            <ItemTemplate>
                                <div style="word-wrap: break-word; width: 400px; white-space: normal;">
                                    <asp:Label ID="lblRemark" runat="server" Text='<%#Bind("[Product Description]") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Quantity" DataField="Quantity" />
                        <asp:BoundField HeaderText="Quantity Unit" DataField="Quantity Unit" />
                        <asp:BoundField HeaderText="Unit Per Price" DataField="Unit Per Price" />
                        <asp:BoundField HeaderText="Product Amount" DataField="Product Amount" />
                        <asp:BoundField HeaderText="Product Type" DataField="Product Type" />
                        <asp:BoundField HeaderText="EXIM Scheme Code" DataField="EXIM Scheme Code" />
                        <asp:BoundField HeaderText="Exim Scheme Notn. No." DataField="Exim Scheme Notn. No." />
                        <asp:BoundField HeaderText="Exim Scheme Notn. Sr. No." DataField="Exim Scheme Notn. Sr. No." />
                        <asp:BoundField HeaderText="Duty Exemption" DataField="Duty Exemption" />
                        <asp:BoundField HeaderText="Re-Import" DataField="Re-Import" />
                         <asp:BoundField HeaderText="In-Bond Invoice Sr. No." DataField="In-Bond Invoice Sr. No." />
                        <asp:BoundField HeaderText="In-Bond Invoice Item Sr. No." DataField="In-Bond Invoice Item Sr. No." />
                        <asp:BoundField HeaderText="BCD Notn No." DataField="BCD Notn No." />
                        <asp:BoundField HeaderText="BCD Notn Sr No." DataField="BCD Notn Sr No." />                       
                        <asp:BoundField HeaderText="BCD Rate Percentage" DataField="BCD Rate Percentage" />
                        <asp:BoundField HeaderText="BCD Duty Flag (H/L/+)" DataField="BCD Duty Flag (H/L/+)" />
                        <asp:BoundField HeaderText="BCD Rate Amount" DataField="BCD Rate Amount" />
                        <asp:BoundField HeaderText="BCD Rate Amount Unit Name" DataField="BCD Rate Amount Unit Name" />
                        <asp:BoundField HeaderText="BCD Duty" DataField="BCD Duty" />
                        <asp:BoundField HeaderText="SWS Notn. No." DataField="SWS Notn. No." />
                        <asp:BoundField HeaderText="SWS Notn. Sr. No." DataField="SWS Notn. Sr. No." />
                        <asp:BoundField HeaderText="SWS Rate %" DataField="SWS Rate %" />
                        <asp:BoundField HeaderText="SWS Duty" DataField="SWS Duty" />
                        <asp:BoundField HeaderText="Custom Health Cess Notn. No." DataField="Custom Health Cess Notn. No." />
                        <asp:BoundField HeaderText="Custom Health Cess Notn. Sr. No." DataField="Custom Health Cess Notn. Sr. No." />
                        <asp:BoundField HeaderText="Custom Health Cess Rate %" DataField="Custom Health Cess Rate %" />
                        <asp:BoundField HeaderText="Custom Health Cess Duty" DataField="Custom Health Cess Duty" />
                        <asp:BoundField HeaderText="Customs Health Cess" DataField="Customs Health Cess" />
                        <asp:BoundField HeaderText="IGST Notn. No." DataField="IGST Notn. No." />
                        <asp:BoundField HeaderText="IGST Notn. Sr. No." DataField="IGST Notn. Sr. No." />
                        <asp:BoundField HeaderText="IGST Notn. Rate" DataField="IGST Notn. Rate" />
                        <asp:BoundField HeaderText="IGST" DataField="IGST" />
                        <asp:BoundField HeaderText="IGST Exemption Type" DataField="IGST Exemption Type" />
                        <asp:BoundField HeaderText="IGST Exemption Notn. No." DataField="IGST Exemption Notn.No." />
                        <asp:BoundField HeaderText="IGST Exemption Notn. Sr. No." DataField="IGST Exemption Notn. Sr. No." />
                        <asp:BoundField HeaderText="IGST Exemption Notn. Rate" DataField="IGST Exemption Notn. Rate" />
                        <asp:BoundField HeaderText="IGST Exemption" DataField="IGST Exemption" />
                        <asp:BoundField HeaderText="IGST Compensation Exemption Type" DataField="IGST Compensation Exemption Type" />
                        <asp:BoundField HeaderText="IGST Compensation Exemption Notn. No." DataField="IGST Compensation Exemption Notn. No." />
                        <asp:BoundField HeaderText="IGST Compensation Exemption Notn. Sr. No." DataField="IGST Compensation Exemption Notn. Sr. No." />
                        <asp:BoundField HeaderText="IGST Compensation Exemption Notn. Rate" DataField="IGST Compensation Exemption Notn. Rate" />
                        <asp:BoundField HeaderText="IGST  Compensation Exemption Cess" DataField="IGST  Compensation Exemption Cess" />
                        <asp:BoundField HeaderText="MRP Applicable" DataField="MRP Applicable" />
                        <asp:BoundField HeaderText="RSP Notn. No." DataField="RSP Notn. No." />
                        <asp:BoundField HeaderText="RSP Notn. Sr. No." DataField="RSP Notn. Sr. No." />
                        <asp:BoundField HeaderText="RSP MRP (INR)" DataField="RSP MRP (INR)" />
                        <asp:BoundField HeaderText="Sr. No. in List" DataField="Sr. No. in List" />
                        <asp:BoundField HeaderText="Abetment %" DataField="Abetment %" />
                        <asp:BoundField HeaderText="Generic Description" DataField="Generic Description" />
                        <asp:BoundField HeaderText="Brand Name" DataField="Brand Name" />
                        <asp:BoundField HeaderText="Model" DataField="Model" />
                        <asp:BoundField HeaderText="End Use Code" DataField="End Use Code" />
                        <asp:BoundField HeaderText="Country of Origin" DataField="Country Of Origin" />
                        <asp:BoundField HeaderText="Manufacturer Name" DataField="Manufacturer Name" />
                        <asp:BoundField HeaderText="Manufacturer Country" DataField="Manufacturer Country" />
                        <asp:BoundField HeaderText="Source Country" DataField="Source Country" />
                        <asp:BoundField HeaderText="Transit Country" DataField="Transit Country" />
                        <asp:BoundField HeaderText="Anti Dumping Notn. No." DataField="Anti Dumping Notn. No." />
                        <asp:BoundField HeaderText="Anti Dumping Notn. Sr. No." DataField="Anti Dumping Notn. Sr. No." />
                        <asp:BoundField HeaderText="Anti Dumping Notn. Item Sr. No." DataField="Anti Dumping Notn. Item Sr. No." />
                        <asp:BoundField HeaderText="Anti Dumping Notn. Suplier Sr. No." DataField="Anti Dumping Notn. Suplier Sr. No." />
                        <asp:BoundField HeaderText="Anti Dumping Duty %" DataField="Anti Dumping Duty %" />
                        <asp:BoundField HeaderText="Anti Dumping Qty" DataField="Anti Dumping Qty" />
                         <asp:BoundField HeaderText="Anti Dumping Qty Unit" DataField="Anti Dumping Qty Unit" />
                        <asp:BoundField HeaderText="Anti Dumping Rate Currency" DataField="Anti Dumping Rate Currency" />
                        <asp:BoundField HeaderText="Anti Dumping Rate Value" DataField="Anti Dumping Rate Value" />
                        <asp:BoundField HeaderText="Anti Dumping Rate Unit" DataField="Anti Dumping Rate Unit" />
                        <asp:BoundField HeaderText="Anti Dumping CTH Sr. No." DataField="Anti Dumping CTH Sr. No." />
                        <asp:BoundField HeaderText="Anti Dumping Duty" DataField="Anti Dumping Duty" />
                        <asp:BoundField HeaderText="FTA" DataField="FTA" />
                        <asp:BoundField HeaderText="FTA Name" DataField="FTA Name" />
                        <asp:BoundField HeaderText="CIF Value" DataField="CIF Value" />
                        <asp:BoundField HeaderText="Assessable Value" DataField="Assessable Value" />
                        <asp:BoundField HeaderText="Duty Amount" DataField="Duty Amount" />
                        <asp:BoundField HeaderText="BCD Forgone Amount" DataField="BCD Forgone Amount" />
                        <asp:BoundField HeaderText="Shipping Bill No. (1)" DataField="Shipping Bill No. (1)" />
                        <asp:BoundField HeaderText="Shipping Bill Date (1)" DataField="Shipping Bill Date (1)" />
                        <asp:BoundField HeaderText="CDInterest" DataField="CDInterest" />
                        <asp:BoundField HeaderText="Fine/Penalty" DataField="Fine/Penalty" />
                        <asp:BoundField HeaderText="Licence No. (1)" DataField="Licence No. (1)" />
                        <asp:BoundField HeaderText="Licence Date (1)" DataField="Licence Date (1)" />
                        <asp:BoundField HeaderText="Licence Item Sr. No. (1)" DataField="Licence Item Sr. No. (1)" />
                        <asp:BoundField HeaderText="Licence Regn. No. (1)" DataField="Licence Regn. No. (1)" />
                        <asp:BoundField HeaderText="Licence Regn. Date (1)" DataField="Licence Regn. Date (1)" />
                        <asp:BoundField HeaderText="Licence Reg. Port (1)" DataField="Licence Reg. Port (1)" />
                        <asp:BoundField HeaderText="Licence Qty (1)" DataField="Licence Qty (1)" />
                        <asp:BoundField HeaderText="Licence Qty Unit (1)" DataField="Licence Qty Unit (1)" />
                        <asp:BoundField HeaderText="Licence CIF Value (1)" DataField="Licence CIF Value (1)" />
                        <asp:BoundField HeaderText="Duty Saved/Debit Duty (1)" DataField="Duty Saved/Debit Duty (1)" />
                        <asp:BoundField HeaderText="Licence No. (2)" DataField="Licence No. (2)" />
                        <asp:BoundField HeaderText="Licence Date (2)" DataField="Licence Date (2)" />
                       <asp:BoundField HeaderText="Licence Item Sr. No. (2)" DataField="Licence Item Sr. No. (2)" />
                        <asp:BoundField HeaderText="Licence Regn. No. (2)" DataField="Licence Regn. No. (2)" />
                        <asp:BoundField HeaderText="Licence Regn. Date (2)" DataField="Licence Regn. Date (2)" />
                        <asp:BoundField HeaderText="Licence Reg. Port (2)" DataField="Licence Reg. Port (2)" />
                        <asp:BoundField HeaderText="Licence Qty (2)" DataField="Licence Qty (2)" />
                        <asp:BoundField HeaderText="Licence Qty Unit (2)" DataField="Licence Qty Unit (2)" />
                        <asp:BoundField HeaderText="Licence CIF Value (2)" DataField="Licence CIF Value (2)" />
                        <asp:BoundField HeaderText="Duty Saved/Debit Duty (2)" DataField="Duty Saved/Debit Duty (2)" />
                        <asp:BoundField HeaderText="Remarks" DataField="Remarks" />
                        <asp:BoundField HeaderText="BoE No" DataField="BOE No" />
                        <asp:BoundField HeaderText="BoE Date" DataField="BOE Date" />

                        <%--<asp:BoundField HeaderText="Date of Delivery to Warehouse" DataField="Date of Delivery to Warehouse" />--%>
                    </Columns>
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="DataSourceReport" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetThermaxLTD_DSr" SelectCommandType="StoredProcedure">
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>

