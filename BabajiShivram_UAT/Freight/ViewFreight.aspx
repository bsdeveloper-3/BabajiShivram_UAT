<%@ Page Title="View Freight Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="ViewFreight.aspx.cs" Inherits="Freight_ViewFreight" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <style type="text/css">
        .Tab .ajax__tab_header {white-space:nowrap !important;}
    </style>
    <script type="text/javascript">
        function ActiveTabChanged12() {
            /* Clear Error Message on Tab change Event */
            document.getElementById('<%=lblError.ClientID%>').innerHTML = '';
            document.getElementById('<%=lblError.ClientID%>').className = '';
        }
    </script>
    <script type="text/javascript">

        function ConfirmLost() {

            // Import Cancelled Enquiry

            var strMessage = "Enquiry Cancelled! This enquiry will be moved to Lost Tab. Do you want to Update Status ?";

            if (confirm(strMessage))
            { }
            else {
                return false;
            }
            
        }

        function OnPortOfLoadingSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=hdnLoadingPortId.ClientID%>').value = results.PortOfLoadingId;
        }
        
        function OnPortOfDischargedSelected(source, eventArgs) {
            var resDischarged = eval('(' + eventArgs.get_value() + ')');
            $get('<%=hdnPortOfDischargedId.ClientID%>').value = resDischarged.PortOfLoadingId;
        }
        
        function OnCountrySelected(source, eventArgs) {

            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=hdnCountryId.ClientID%>').value = results.CountryId;
        }
        
    </script>
    
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPanelDetail" runat="server">
            <ProgressTemplate>
                <div style="position:absolute;visibility:visible;border:none;z-index:100;width:90%;height:90%;background:#FAFAFA; filter: alpha(opacity=80);-moz-opacity:.8; opacity:.8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position:relative; top:40%;left:40%; "/>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upPanelDetail" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server"></asp:Label>        
                <asp:HiddenField ID="hdnUploadPath" runat="server" />
                <asp:HiddenField ID="hdnLoadingPortId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnPortOfDischargedId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnCountryId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnSalesRepId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnModeId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnStatusId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnTypeId" runat="server" Value="0" />
            </div>
            <div class="clear"></div>
            <AjaxToolkit:TabContainer runat="server" ID="EnquiryTabs" ActiveTabIndex="0" CssClass="Tab" CssTheme="None"
                Width="100%" OnClientActiveTabChanged="ActiveTabChanged12" AutoPostBack="false">
                <AjaxToolkit:TabPanel runat="server" ID="TabPanelFreightDetail" HeaderText="Freight Detail">
                    <ContentTemplate>
                        <AjaxToolkit:Accordion ID="Accordion1" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" 
                        ContentCssClass="accordionContent" runat="server" SelectedIndex="0" FadeTransitions="true" SuppressHeaderPostbacks="true" 
                        TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false" AutoSize="None">
                    <Panes>
                    <AjaxToolkit:AccordionPane ID="accFreight" runat="server">
                    <Header>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Freight Detail</Header>
                    <Content>
                        <asp:FormView ID="FVFreightDetail" runat="server" DataKeyNames="EnqId" Width="100%" OnDataBound="FVFreightDetail_DataBound">
                            <HeaderStyle Font-Bold="True" />
                            <ItemTemplate>
                                <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
                                    <tr>
                                        <td>
                                            Enquiry No
                                        </td>
                                        <td>
                                            <asp:Label ID="lblEnquiryRefNo" runat="server" Text='<%# Eval("ENQRefNo")%>' ></asp:Label>
                                        </td>
                                        <td>
                                            Enquiry Date
                                        </td>
                                        <td>
                                            <%#Eval("ENQDate","{0:dd/MM/yyyy}")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Job No.
                                        </td>
                                        <td>
                                            <asp:Label ID="lblFRJobNo" Text='<%#Eval("FRJobNo")%>' runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            Booking Date
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("BookingDate", "{0:dd/MM/yyyy}")%>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Freight Type
                                        </td>
                                        <td>
                                            <%#Eval("TypeName")%>
                                        </td>
                                        <td>
                                            Freight Mode
                                        </td>
                                        <td>
                                            <%#Eval("ModeName")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Customer
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCustomer" runat="server" Text='<%#Eval("Customer")%>' ></asp:Label>
                                        </td>
                                        <td>
                                            Branch
                                        </td>
                                        <td>
                                            <%# Eval("BranchName")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Ref No/Email Ref
                                        </td>
                                        <td colspan="3">
                                            <%#Eval("CustRefNo")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Shipper
                                        </td>
                                        <td>
                                            <%#Eval("Shipper")%>
                                        </td>
                                        <td>
                                            Consignee
                                        </td>
                                        <td>
                                            <%#Eval("Consignee")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Port of Loading
                                        </td>
                                        <td>
                                            <%#Eval("LoadingPortName")%>
                                        </td>
                                        <td>
                                            Port of Discharged
                                        </td>
                                        <td>
                                            <%#Eval("PortOfDischargedName")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Terms
                                        </td>
                                        <td>
                                            <%#Eval("TermsName")%>
                                        </td>
                                        <td>
                                            Agent
                                        </td>
                                        <td>
                                            <%#Eval("AgentName")%>
                                        </td>
                                    </tr>
                                    <asp:Panel ID="pnlSea" runat="server" Visible="false">
                                    <tr>
                                        <td>
                                            Container 20"
                                        </td>
                                        <td>
                                            <%#Eval("CountOf20")%>
                                        </td>
                                        <td>
                                            Container 40"
                                        </td>
                                        <td>
                                            <%#Eval("CountOf40")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            LCL (CBM)
                                        </td>
                                        <td>
                                            <%#Eval("LCLVolume")%>
                                        </td>
                                        <td>
                                            Container Type
                                        </td>
                                        <td>
                                            <%#Eval("ContainerTypeName")%> &nbsp;&nbsp; <%#Eval("ContainerSubType")%>
                                        </td>
                                    </tr>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlAir" runat="server" Visible="false">
                                    <tr>
                                        <td>
                                            No of Packages
                                        </td>
                                        <td>
                                            <%#Eval("NoOfPackages")%>
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    </asp:Panel>
                                    <tr>
                                        <td>
                                            Gross Weight (Kgs)
                                        </td>
                                        <td>
                                            <%#Eval("GrossWeight")%>
                                        </td>
                                            
                                        <td>
                                            Chargeable Weight (Kgs)
                                        </td>
                                        <td>
                                            <%#Eval("ChargeableWeight")%>    
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Is Dangerous Goods ?</td>
                                        <td>
                                            <%# (Boolean.Parse(Eval("IsDangerousGood").ToString())) ? "Yes" : "No"%>
                                        </td>
                                        <td>
                                            Sales representative
                                        </td>
                                        <td>
                                            <%#Eval("SalesRepName")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Freight SPC
                                        </td>
                                        <td>
                                            <%#Eval("EnquiryUser") %>
                                        </td>
                                        <td>
                                            Country
                                        </td>
                                        <td>
                                            <%#Eval("CountryName")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Remarks</td>
                                        <td colspan="3">
                                            <%#Eval("Remarks") %>
                                        </td>
                                    </tr
                                    
                                    <asp:Panel ID="pnlSharedWith" runat="server">
                                    <tr>
                                        <td>
                                            Project Emp/Agent
                                        </td>
                                        <td class="label" colspan="3">
                                            <asp:BulletedList ID="blEmployee" DataSourceID="DataSourceSharedEmp" DataTextField="sName"
                                                DataValueField="lid" runat="server" DisplayMode="LinkButton" CausesValidation="false"
                                                Target="_blank" CssClass="ulList">
                                            </asp:BulletedList>
                                            <asp:SqlDataSource ID="DataSourceSharedEmp" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                                SelectCommand="FR_GetEnquiryUser" SelectCommandType="StoredProcedure">
                                                <SelectParameters>
                                                    <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                        </td>
                                    </tr>
                                    </asp:Panel>
                                </table>
                            </ItemTemplate>
                        </asp:FormView>
                    </Content>
                    </AjaxToolkit:AccordionPane>
                    <%-- Agent PreAlert--%>
                    <AjaxToolkit:AccordionPane ID="accAgentPreAlert" runat="server">
                    <Header>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Agent PreAlert</Header>
                    <Content>
                    <asp:FormView ID="fvAgentPreAlert" runat="server" DataKeyNames="EnqId" Width="99%">
                        <ItemTemplate>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>
                                    MBL No
                                </td>
                                <td>
                                    <%# Eval("MBLNo")%>
                                </td>
                                <td>
                                    MBL Date
                                </td>
                                <td>
                                    <%# Eval("MBLDate","{0:dd/MM/yyyy}")%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    HBL No
                                </td>
                                    <td>
                                    <%# Eval("HBLNo")%>
                                </td>
                                <td>
                                    HBL Date
                                </td>
                                <td>
                                    <%# Eval("HBLDate","{0:dd/MM/yyyy}")%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Vessel / Airline
                                </td>
                                <td>
                                    <%# Eval("VesselName")%>
                                </td>
                                <td>
                                    Flight / Voyage
                                </td>
                                <td>
                                    <%# Eval("VesselNumber")%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    ETD
                                </td>
                                <td>
                                    <%# Eval("ETD","{0:dd/MM/yyyy}")%>
                                </td>
                                <td>
                                    ETA
                                </td>
                                <td>
                                     <%# Eval("ETA","{0:dd/MM/yyyy}")%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Final Agent
                                </td>
                                <td colspan="3">
                                    <%# Eval("FinalAgent")%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Created By
                                </td>
                                <td>
                                    <%# Eval("AgentPreAlertUser")%>
                                </td>
                                <td>
                                    Created Date
                                </td>
                                <td>
                                    <%# Eval("AgentPreAlertDate", "{0:dd/MM/yyyy}")%>
                                </td>
                            </tr>
                        </table>
                        </ItemTemplate>
                    </asp:FormView>
                    </Content>
                    </AjaxToolkit:AccordionPane>
                    <%-- Customer PreAlert--%>
                    <AjaxToolkit:AccordionPane ID="AccCustPreAlert" runat="server">
                    <Header>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Customer PreAlert</Header>
                    <Content>
                        <asp:FormView ID="fvCustomerPreAlert" runat="server" DataKeyNames="EnqId" Width="99%">
                        <ItemTemplate>
                        
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>
                                    Shipped on Board
                                </td>
                                <td>
                                    <%# Eval("ShippedOnBoardDate", "{0:dd/MM/yyyy}")%>
                                </td>
                                <td>
                                    PreAlert Date
                                </td>
                                <td>
                                    <%# Eval("PreAlertToCustDate", "{0:dd/MM/yyyy}")%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Customer Email
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lblEmailSentTo" Text='<%#Eval("CustomerEmail")%>' runat="server"></asp:Label>
                                </td>
                           </tr>
                            <tr>
                                <td>
                                    Alert Email Sent ?
                                </td>
                                <td>
                                    <asp:Label ID="lblAlertEmailSent" Text='<%#GetBooleanToYesNo(Eval("AlertEmailSent"))%>' runat="server"></asp:Label>
                                </td>
                                <td>
                                    Alert Email Date
                                </td>
                                <td>
                                    <%# Eval("AlertEmailDate","{0:dd/MM/yyyy}")%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Created By
                                </td>
                                <td>
                                    <%# Eval("CustomerPreAlertUser")%>
                                </td>
                                <td>
                                    Created Date
                                </td>
                                <td>
                                    <%# Eval("CustomerPreAlertDate", "{0:dd/MM/yyyy}")%>
                                </td>
                            </tr>
                        </table>
                        </ItemTemplate>
                        
                        </asp:FormView>
                    </Content>
                    </AjaxToolkit:AccordionPane>
                    <%-- Cargo Arrival Notice--%>
                    <AjaxToolkit:AccordionPane ID="accArrival" runat="server">
                    <Header>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Cargo Arrival Notice</Header>
                    <Content>
                        <asp:FormView ID="fvCAN" runat="server" DataKeyNames="EnqId" Width="99%">
                        <ItemTemplate>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>
                                        IGM No
                                    </td>
                                    <td>
                                        <%# Eval("IGMNo")%>
                                    </td>
                                    <td>
                                        IGM Date
                                    </td>
                                    <td>
                                        <%# Eval("IGMDate", "{0:dd/MM/yyyy}")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Item No
                                    </td>
                                    <td>
                                        <%# Eval("ItemNo")%>
                                    </td>
                                    <td>
                                        ATA Date
                                    </td>
                                    <td>
                                        <%# Eval("ATADate", "{0:dd/MM/yyyy}")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lnkCreateCANPdf" Text="Create CAN (PDF)" runat="server" OnClick="lnkCreateCANPdf_Click"></asp:LinkButton>
                                    </td>
                                    <td>
                                        CAN Print Date &nbsp;&nbsp;
                                        <AjaxToolkit:CalendarExtender ID="calCANDate" runat="server" Enabled="True" EnableViewState="False"
                                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgCANDate" PopupPosition="BottomRight"
                                            TargetControlID="txtCANDate">
                                        </AjaxToolkit:CalendarExtender>
                                        <asp:TextBox ID="txtCANDate" runat="server" Width="100px"></asp:TextBox>
                                        <asp:Image ID="imgCANDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server"/>
                                        <AjaxToolkit:MaskedEditExtender ID="MskExtCANDate" TargetControlID="txtCANDate" Mask="99/99/9999" MessageValidatorTip="true" 
                                            MaskType="Date" AutoComplete="false" runat="server"></AjaxToolkit:MaskedEditExtender>
                                        <AjaxToolkit:MaskedEditValidator ID="MskValCANDate" ControlExtender="MskExtCANDate" ControlToValidate="txtCANDate" IsValidEmpty="true" 
                                            InvalidValueMessage="CAN Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                            EmptyValueMessage="Please Enter CAN Date" EmptyValueBlurredText="Required" MinimumValueMessage="Invalid Date" 
                                            MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2025" 
                                            Runat="Server"></AjaxToolkit:MaskedEditValidator>
                                    </td>
                                    <td colspan="2"></td>
                                </tr>
                                <tr>
                                    <td>
                                        Remark
                                    </td>
                                    <td colspan="3">
                                        <%#Eval("CANRemark") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Created By
                                    </td>
                                    <td>
                                        <%# Eval("CANUser")%>
                                    </td>
                                    <td>
                                        Created Date
                                    </td>
                                    <td>
                                        <%# Eval("CANDate", "{0:dd/MM/yyyy}")%>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                        </asp:FormView>
                        <fieldset>
                        <div align="center">
                            <asp:Label ID="lblInvoiceError" runat="server" EnableViewState="false"></asp:Label>
                        </div>
                        <legend>Invoice</legend>                                                                                                                                                                                                                                                                                                                                                                  
                        
                        <div>
                        <asp:GridView ID="gvCanInvoice" runat="server" DataSourceID="SqlDataSourceCanInvoice" DataKeyNames="InvoiceItemId" Width="100%" 
                            AllowPaging="True" PageSize="40" AllowSorting="true" PagerStyle-CssClass="pgr" PagerSettings-Position="TopAndBottom"
                            CssClass="table" AutoGenerateColumns="False">
                            <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FieldName" HeaderText="INVOICE ITEM" ReadOnly="true"/>
                            <asp:BoundField DataField="ReportHeader" HeaderText="REPORT HEADER" ReadOnly="true"/>
                            <asp:BoundField DataField="UnitOfMeasurement" HeaderText="UOM" ReadOnly="true"/>
                            <asp:BoundField DataField="Rate" HeaderText="Rate" SortExpression="RATE" ReadOnly="true"/>
                            <%--<asp:BoundField DataField="Currency" HeaderText="Currency" SortExpression="Currency"/>
                            <asp:BoundField DataField="ExchangeRate" HeaderText="Exchange Rate" SortExpression="ExchangeRate"/>--%>
                            <asp:TemplateField HeaderText="CURRENCY" SortExpression="CURRENCY">
                                <ItemTemplate>
                                    <asp:Label ID="lblCurrency" runat="server" Text='<%#Eval("Currency") + " - " + Eval("ExchangeRate")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="AMOUNT (Rs)" SortExpression="Amount">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmount" runat="server" Text='<%#Eval("Amount")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TAX" SortExpression="TaxAmount">
                                <ItemTemplate>
                                    <asp:Label ID="lblTaxAmount" runat="server" Text='<%#Eval("TaxAmount")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TOTAL (Rs)" SortExpression="TotalAmount">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotal" runat="server" Text='<%#Eval("TotalAmount") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="dtDate" HeaderText="DATE" DataFormatString="{0:dd/MM/yyyy}" SortExpression="dtDate" ReadOnly="true"/>
                            <asp:BoundField DataField="CreatedBy" HeaderText="USER" SortExpression="CreatedBy" ReadOnly="true"/>
                        </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSourceCanInvoice" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="FOP_GetCANInvoiceDetail" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        </div>
                        </fieldset>
                    </Content>
                    </AjaxToolkit:AccordionPane>
                    <%-- Delivery Order--%>
                    <AjaxToolkit:AccordionPane ID="accDeliveryOrder" runat="server">
                    <Header>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Delivery Order</Header>
                    <Content>
                    <asp:FormView ID="fvDelivery" runat="server" DataKeyNames="EnqId" Width="99%">
                        <ItemTemplate>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>
                                   CHA Name
                                </td>
                                <td>
                                    <%# Eval("CHAName")%>
                                </td>
                                <td>
                                    Terms of Payment
                                </td>
                                <td>
                                    <%# Eval("PaymentTermName")%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    DO Issued To
                                </td>
                                <td>
                                    <%# Eval("DOIssuedTo")%>
                                </td>
                                <td>
                                    Payment Type
                                </td>
                                <td>
                                    <%# Eval("DOPaymentType")%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Cheque No / DD No
                                </td>
                                <td>
                                    <%# Eval("ChequeNo")%>
                                </td>
                                <td>
                                    Cheque Date / DD Date
                                </td>
                                <td>
                                    <%# Eval("ChequeDate", "{0:dd/MM/yyyy}")%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    DO Amount
                                </td>
                                <td>
                                    <%#Eval("DOAmount") %>
                                </td>
                                <td colspan="2">
                                    <asp:LinkButton ID="lnkCreateDOPDF" Text="Create Delivery Order" runat="server" OnClick="lnkCreateDOPDF_Click"></asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Remark
                                </td>
                                <td colspan="3">
                                    <%#Eval("DORemark") %>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Created By
                                </td>
                                <td>
                                    <%# Eval("DOCreatedBy")%>
                                </td>
                                <td>
                                    Created Date
                                </td>
                                <td>
                                    <%# Eval("DOCreatedDate", "{0:dd/MM/yyyy}")%>
                                </td>
                            </tr>
                        </table>
                        </ItemTemplate>
                    </asp:FormView>
                    </Content>
                    </AjaxToolkit:AccordionPane>
                    <%-- Billing Advice--%>
                    <AjaxToolkit:AccordionPane ID="accBillingAdvice" runat="server">
                        <Header>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Billing Advice</Header>
                        <Content>
                            <asp:FormView ID="fvAdvice" runat="server" DataKeyNames="EnqId" Width="99%">
                                <ItemTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                        <tr>
                                            <td>
                                                Agent Invoice Received?
                                            </td>
                                            <td>
                                                <asp:Label ID="lblAgentINVRcvd" Text='<%#GetBooleanToYesNo(Eval("IsAgentInvoiceRcvd"))%>'
                                                    runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                Sent To Billing Dept
                                            </td>
                                            <td>
                                                <asp:Label ID="lblSenToBillDep" Text='<%#GetBooleanToYesNo(Eval("IsFileSentToBilling"))%>'
                                                    runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                File Sent To Billing Date
                                            </td>
                                            <td>
                                                <%# Eval("FileSentToBillingDate", "{0:dd/MM/yyyy}")%>
                                            </td>
                                            <td>
                                                Remark
                                            </td>
                                            <td>
                                                <%# Eval("AdviceRemark")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Created By
                                            </td>
                                            <td>
                                                <%# Eval("AdviceCreatedBy")%>
                                            </td>
                                            <td>
                                                Created Date
                                            </td>
                                            <td>
                                                <%# Eval("AdviceCreatedDate", "{0:dd/MM/yyyy}")%>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                                
                            </asp:FormView>
                        </Content>
                    </AjaxToolkit:AccordionPane>
                    <%-- Billing Detail--%> 
                    <AjaxToolkit:AccordionPane ID="accAgentInvoice" runat="server">
                    <Header>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Billing & Back Office</Header>
                    <Content>
                        <asp:FormView ID="fvAgentInvoice" runat="server" DataKeyNames="EnqId" Width="99%">
                        <ItemTemplate>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                            <td>
                                File Received for Billing
                            </td>
                            <td>
                                <%# Eval("FileReceivedDate", "{0:dd/MM/yyyy}")%>
                            </td>
                            <td>
                                Bill Amount
                            </td>
                            <td>
                                <%# Eval("BillAmount") %>
                            </td>
                            </tr>
                            <tr>
                                <td>
                                    Bill Number
                                </td>
                                <td>
                                    <%# Eval("BillNumber") %>
                                </td>
                                <td>
                                    Bill Date
                                </td>
                                <td>
                                    <%# Eval("BillDate", "{0:dd/MM/yyyy}")%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Remark
                                </td>
                                <td colspan="3">
                                    <span><%#Eval("BackOfficeRemark") %> </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Created By
                                </td>
                                <td>
                                    <%# Eval("AgentInvoiceCreatedBy")%>
                                </td>
                                <td>
                                    Created Date
                                </td>
                                <td>
                                    <%# Eval("AgentInvoiceCreateDate", "{0:dd/MM/yyyy}")%>
                                </td>
                            </tr>
                        </table>
                        </ItemTemplate>
                        </asp:FormView>
                        <div class="m clear"></div>
                    <div>
                    <asp:GridView ID="GridViewInvoiceDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                        Width="100%" DataKeyNames="lId" DataSourceID="DataSourceInvoiceDetail" CellPadding="4">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="JB Number" DataField="JBNumber"/>
                            <asp:BoundField HeaderText="JB Date" DataField="JBDate" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField HeaderText="Agent" DataField="AgentInvoiceName"/>
                            <asp:BoundField HeaderText="Invoice No" DataField="AgentInvoiceNo"/>
                            <asp:BoundField HeaderText="Invoice Date" DataField="AgentInvoiceDate" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField HeaderText="Invoice Amount" DataField="AgentInvoiceAmount"/>
                            <asp:BoundField HeaderText="Currency" DataField="Currency"/>
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="DataSourceInvoiceDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="FOP_GetAgentInvoiceDetail" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    </div>
                    </Content>
                    </AjaxToolkit:AccordionPane>
                    <AjaxToolkit:AccordionPane ID="accContainer" runat="server">
                    <Header>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Container</Header>
                    <Content>
                    <div>
                        <fieldset><legend>Container Detail</legend>
                            <asp:GridView ID="gvContainer" runat="server" AllowPaging="true" CssClass="table"
                                PagerStyle-CssClass="pgr" AutoGenerateColumns="false" DataKeyNames="lid" Width="100%"
                                PageSize="20" DataSourceID="DataSourceContainer">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ContainerNo" HeaderText="Container No" />
                                    <asp:BoundField DataField="ContainerTypeName" HeaderText="Container Type" />
                                    <asp:BoundField DataField="ContainerSizeName" HeaderText="Container Size" />
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                    </div>
                    <div>
                        <asp:SqlDataSource ID="DataSourceContainer" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="FOP_GetContainerMS" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                    </Content>
                    </AjaxToolkit:AccordionPane>
                    <AjaxToolkit:AccordionPane ID="accActivity" runat="server">
                    <Header>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Daily Activity</Header>
                    <Content>
                    <fieldset><legend>Daily Activity</legend>
                        <div class="m clear">
                        <asp:Button ID="btnSaveActivity" runat="server" ValidationGroup="RequiredActivity" 
                            Text="Save" OnClick="btnSaveActivity_Click" TabIndex="2"/>
                        </div>
                        <table border="0" cellpadding="0" cellspacing="0" width="99%" bgcolor="white">
                        <tr>
                            <td width="110px" align="center">
                                Daily Progress
                                <asp:RequiredFieldValidator ID="RFVProgress" runat="server" ControlToValidate="txtDailyProgress" Display="Dynamic" ValidationGroup="RequiredActivity"
                                    SetFocusOnError="true" Text="*" ErrorMessage="Enter Daily Progress."></asp:RequiredFieldValidator>
                            </td>
                            <td width="95px" colspan="3">
                                <asp:TextBox ID="txtDailyProgress" runat="server" TextMode="MultiLine" Rows="3" TabIndex="1"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    </fieldset>
                    <fieldset><legend>Activity</legend>
                        <asp:GridView ID="gvDailyActivity" runat="server" AutoGenerateColumns="False" CssClass="table"
                            Width="98%" PagerStyle-CssClass="pgr" DataKeyNames="lid" CellPadding="4" PageSize="10" 
                            PagerSettings-Position="TopAndBottom" AllowPaging="true" AllowSorting="true"
                            OnPreRender="gvDailyActivity_PreRender" OnRowDataBound="gvDailyActivity_RowDataBound"
                            OnRowCommand="gvDailyActivity_RowCommand" DataSourceID="DataSourceActivity">
                            <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex +1%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="dtDate" HeaderText="Date & Time" SortExpression="dtDate" 
                                DataFormatString="{0:dd/MM/yyyy hh:mm tt}" ReadOnly="true" />
                            <asp:TemplateField HeaderText="Progress Detail">
                                <ItemTemplate>
                                    <asp:Label ID="lblProgress" runat="server" Text='<%#Eval("ShortProgress") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkMoreProgress" CommandName="ProgressPopup" CommandArgument='<%#Eval("DailyProgress")%>' Text="...More" runat="server" TabIndex="6"></asp:LinkButton>
                                </ItemTemplate>
                             </asp:TemplateField>
                            <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" ReadOnly="true" />
                            <asp:TemplateField HeaderText="Remove">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDelete" Text="Remove" CommandName="ActivityDelete" CommandArgument='<%#Eval("lId") %>' OnClientClick="return confirm('Sure to delete?');"
                                        runat="server"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                            <PagerTemplate>
                            <asp1:GridViewPager ID="GridViewPager1" runat="server" />
                        </PagerTemplate>
                        </asp:GridView>
                        <asp:SqlDataSource ID="DataSourceActivity" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="FOP_GetFreightActivity" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </fieldset>
                    </Content>
                    </AjaxToolkit:AccordionPane>
                    </Panes>
                    </AjaxToolkit:Accordion>        
                </ContentTemplate>
                </AjaxToolkit:TabPanel>
                <AjaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Status">
                    <ContentTemplate>
                        <fieldset id="fieldStatus" runat="server">
                            <legend>Freight Status</legend>
                            <asp:FormView ID="fvFreightStatus" runat="server" DataKeyNames="EnqId" Width="100%">
                            <HeaderStyle Font-Bold="True" />
                                <ItemTemplate>
                                    <div class="m clear">
                                        <asp:Label ID="lblStatusMsg" runat="server"></asp:Label>
                                        <asp:Button ID="btnStatusChange" runat="server" OnClick="btnStatusChange_Click" Text="Change Status" ToolTip="Update Enquiry Status" CausesValidation="false" />
                                    </div>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div class="m clear">
                                        <asp:Button ID="btnStatusUpdate" runat="server" OnClick="btnStatusUpdate_Click" Text="Update"
                                            ValidationGroup="validateStatus" OnClientClick="if(Page_ClientValidate('validateStatus')) return ConfirmLost(); return false;" />
                                        <asp:Button ID="btnStatusCancel" runat="server" OnClick="btnStatusCancel_Click" CausesValidation="False"
                                            Text="Cancel" />
                                    </div>
                                    <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
                                        <tr>
                                            <td>
                                               Current Status
                                               <asp:RequiredFieldValidator ID="RFVCurrentStatus" runat="server" ControlToValidate="ddFreightStatus" Display="Dynamic" ValidationGroup="validateStatus"
                                                InitialValue="0" SetFocusOnError="true" Text="Required"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddFreightStatus" runat="server">
                                                    <asp:ListItem Text="Lost" Value="4"></asp:ListItem>
                                                    <asp:ListItem Text="Quoted" Value="2"></asp:ListItem>
                                                </asp:DropDownList> 
                                                <asp:DropDownList ID="ddLostStaus" runat="server">
                                                    <asp:ListItem Text="--Lost Reason--" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Rate Issue upto 10%" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Rate Issue 10%-20%" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Rate Issue 20%-30%" Value="3"></asp:ListItem>
                                                    <asp:ListItem Text="Agent did not Quote rates" Value="4"></asp:ListItem>
                                                    <asp:ListItem Text="Quote Submitted Late" Value="5"></asp:ListItem>
                                                    <asp:ListItem Text="No Feedback" Value="6"></asp:ListItem>
                                                    <asp:ListItem Text="Others" Value="7"></asp:ListItem>
                                                </asp:DropDownList> 
                                                <%--<asp:RequiredFieldValidator ID="RFVLostStatus" runat="server" ControlToValidate="ddLostStaus" Display="Dynamic" ValidationGroup="validateStatus"
                                                    InitialValue="0" SetFocusOnError="true" Text="Required"></asp:RequiredFieldValidator>--%>
                                            </td>
                                            <td>
                                                Status Date
                                                <AjaxToolkit:CalendarExtender ID="CalStatusDate" runat="server" Enabled="True" EnableViewState="False"
                                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgStatDate" PopupPosition="BottomRight"
                                                    TargetControlID="txtStatusDate">
                                                </AjaxToolkit:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStatusDate" runat="server" Width="100px"></asp:TextBox>
                                                <asp:Image ID="imgStatDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server"/>
                                                
                                                <AjaxToolkit:MaskedEditExtender ID="MskExtStatusDate" TargetControlID="txtStatusDate" Mask="99/99/9999" MessageValidatorTip="true" 
                                                    MaskType="Date" AutoComplete="false" runat="server"></AjaxToolkit:MaskedEditExtender>
                                                <AjaxToolkit:MaskedEditValidator ID="MskValETABDate" ControlExtender="MskExtStatusDate" ControlToValidate="txtStatusDate" IsValidEmpty="false" 
                                                  InvalidValueMessage="Status Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" 
                                                  MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="31/12/2025" 
                                                  Runat="Server" ValidationGroup="validateStatus"></AjaxToolkit:MaskedEditValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Status Remarks
                                                <asp:RequiredFieldValidator ID="RFVStatusRemark" runat="server" ControlToValidate="txtStatusRemark" Display="Dynamic" ValidationGroup="validateStatus"
                                                InitialValue="" SetFocusOnError="true" Text="Required"></asp:RequiredFieldValidator>
                                                <%--<asp:RegularExpressionValidator ID="REVRemark" Display = "Dynamic" ControlToValidate = "txtStatusRemark" ValidationGroup="validateStatus"
                                                    ValidationExpression = "^[\s\S]{10,}$" runat="server" ErrorMessage="Minimum 10 characters required."></asp:RegularExpressionValidator>--%>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtStatusRemark" runat="server" TextMode="MultiLine" Width="80%" MaxLength="800"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </EditItemTemplate>
                            </asp:FormView>
                            <div>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="FR_GetStatusHistory" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            </div>
                            </fieldset>
                        <fieldset><legend>Status History</legend>
                            <asp:GridView ID="gvStatusHistory" runat="server" AutoGenerateColumns="False" CssClass="table"
                                Width="100%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="StatusHistorySqlDataSource"
                                CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20" PagerSettings-Position="TopAndBottom">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="StatusName" HeaderText="Status" />
                                    <asp:BoundField DataField="StatusDate" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="UserName" HeaderText="User"/>
                                    <asp:BoundField DataField="Remarks" HeaderText="Remark" />
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                        <div>
                            <asp:SqlDataSource ID="StatusHistorySqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="FR_GetStatusHistory" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
                <AjaxToolkit:TabPanel ID="TabDocument" runat="server" HeaderText="Document">
                    <ContentTemplate>
                        <fieldset><legend>Upload Freight Document</legend>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td width="110px" align="center">
                                        Document Type
                                        <asp:RequiredFieldValidator ID="RFVDocName" runat="server" ControlToValidate="ddl_DocumentType" Display="Dynamic" ValidationGroup="validateDocument"
                                            SetFocusOnError="true" Text="*" ErrorMessage="Enter Document Name."></asp:RequiredFieldValidator>
                                    </td>
                                    <td width="50" align="center">
                                        <%--<asp:TextBox ID="txtDocName" runat="server"></asp:TextBox>--%>
                                        <asp:DropDownList ID="ddl_DocumentType" runat="server" DataSourceID="FrDocTypeSqlDataSource" 
                                           DataTextField="Sname" DataValueField="lid" >
                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td align="center">
                                        Attachment
                                        <asp:RequiredFieldValidator ID="RFVAttach" runat="server" ControlToValidate="fuDocument" Display="Dynamic" ValidationGroup="validateDocument"
                                            SetFocusOnError="true" Text="*" ErrorMessage="Attach File For Upload."></asp:RequiredFieldValidator>
                                    </td>    
                                    <td>
                                        <asp:FileUpload ID="fuDocument" runat="server" />
                                        <asp:Button ID="btnUpload" runat="server"
                                            Text="Upload" OnClick="btnUpload_Click" ValidationGroup="validateDocument"  />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset><legend>Download Document</legend>
                            <asp:GridView ID="gvFreightDocument" runat="server" AutoGenerateColumns="False" Width="100%"  
                                DataKeyNames="DocId" DataSourceID="FreightDocumentSqlDataSource" CssClass="table"
                                OnRowCommand="gvFreightDocument_RowCommand" CellPadding="4" PagerStyle-CssClass="pgr"
                                AllowPaging="true" PageSize="20" AllowSorting="True" PagerSettings-Position="TopAndBottom">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DocName" HeaderText="Document Name" SortExpression="DocName" />
                                    <asp:BoundField DataField="UserName" HeaderText="Uploaded By" />
                                    <asp:BoundField DataField="UploadedDate" HeaderText="Uploaded Date" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:TemplateField HeaderText="Download">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                                CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Remove">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnlRemoveDocument" runat="server" Text="Remove" CommandName="RemoveDocument" Visible="false"
                                                CommandArgument='<%#Eval("DocId") %>' OnClientClick="return confirm('Are you sure to remove document?');" ></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                        <div>
                            <asp:SqlDataSource ID="FreightDocumentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="FR_GetUploadedDocument" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                        <div>
                            <asp:SqlDataSource ID="FrDocTypeSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="Get_FRDocumentType" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>

                <AjaxToolkit:TabPanel ID="TabReminder" runat="server" HeaderText="Reminder">
                    <ContentTemplate>
                        <fieldset><legend>Add Reminder</legend>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>
                                        Type
                                        <asp:CustomValidator runat="server" ID="CValRemindMode" ClientValidationFunction="ValidateCheckList" Display="Dynamic"
                                            Text="Required" ErrorMessage="Please Select Atleast One Type" ValidationGroup="validateReminder"></asp:CustomValidator>
                                    </td>
                                    <td>
                                        <asp:CheckBoxList ID="chkRemindMode" runat="server" RepeatDirection="Horizontal" TabIndex="1">
                                            <asp:ListItem Text="Email" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="SMS" Value="2"></asp:ListItem>
                                        </asp:CheckBoxList>
                                        
                                    </td>
                                    <td>
                                        Reminder Note
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemindNote" runat="server" TextMode="MultiLine" TabIndex="2"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Reminder Date
                                        <AjaxToolkit:CalendarExtender ID="calRemindDate" runat="server" Enabled="True" EnableViewState="False"
                                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgRmdate" PopupPosition="BottomRight"
                                            TargetControlID="txtRemindDate">
                                        </AjaxToolkit:CalendarExtender>
                                        <AjaxToolkit:MaskedEditExtender ID="MskExtRemindDate" TargetControlID="txtRemindDate" Mask="99/99/9999" MessageValidatorTip="true" 
                                            MaskType="Date" AutoComplete="false" runat="server"></AjaxToolkit:MaskedEditExtender>
                                        <AjaxToolkit:MaskedEditValidator ID="MskValRemindDate" ControlExtender="MskExtRemindDate" ControlToValidate="txtRemindDate" IsValidEmpty="false" 
                                            InvalidValueMessage="Reminder Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                            EmptyValueMessage="Required" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2016" MaximumValue="31/12/2025" 
                                            Runat="Server" ValidationGroup="validateReminder"></AjaxToolkit:MaskedEditValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemindDate" Width="100px" TabIndex="3" runat="server" placeholder="dd/mm/yyyy"></asp:TextBox>
                                        <asp:Image ID="imgRmdate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server"/>
                                    </td>
                                    <td colspan="2">
                                        <asp:Button ID="btnReminder" runat="server" Text="Add Reminder" OnClick="btnAddReminder_Click"
                                            ValidationGroup="validateReminder" TabIndex="4" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset><legend>Reminder Details</legend>
                            <div>
                                <asp:GridView ID="gvReminder" runat="server" AutoGenerateColumns="False" CssClass="table" Width="100%"
                                    PagerStyle-CssClass="pgr" CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20" DataKeyNames="lId"
                                    DataSourceID="ReminderDetailSqlDataSource" OnRowCommand="gvReminder_RowCommand" PagerSettings-Position="TopAndBottom">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ReminderType" HeaderText="Reminder Type" />
                                        <asp:BoundField DataField="ReminderNotes" HeaderText="Notes" />
                                        <asp:BoundField DataField="ReminderUser" HeaderText="Reminder To" />
                                        <asp:BoundField DataField="RemindDate" HeaderText="Reminder Date" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                            <asp:Label ID="lblRemindStatus" Text='<%#(Boolean.Parse(Eval("RemindStatus").ToString())? "Closed" : "Active") %>'
                                                runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remove">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnlRemoveReminder" runat="server" Text="Remove" CommandName="RemoveRemind"
                                                 CommandArgument='<%#Eval("lId") %>' OnClientClick="return confirm('Are you sure to remove reminder?');" ></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                        <div>
                            <asp:SqlDataSource ID="ReminderDetailSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="FR_GetReminderDetail" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
                
                <AjaxToolkit:TabPanel ID="TabSharedUser" runat="server" HeaderText="Shared">
                    <ContentTemplate>
                        <fieldset><legend>Project Participants</legend>
                            <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
                                <tr>
                                    <td>
                                    <asp:Button ID="btnUpdParticipant" runat="server" Text="Update Enquiry Participants" OnClick="btnUpdParticipant_Click" /><br />
                                        <asp:ListBox ID="lbEmployee" runat="server" SelectionMode="Multiple" Width="50%" Height="180px" TabIndex="21">
                                            <asp:ListItem Text="Dhaval Davada" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Devendra Donde" Value="313"></asp:ListItem>
                                            <asp:ListItem Text="Hemali Patel" Value="177"></asp:ListItem>
                                            <asp:ListItem Text="Manish Radhakrishnan" Value="185"></asp:ListItem>
                                            <asp:ListItem Text="Ridhi Davada" Value="171"></asp:ListItem>
                                            <asp:ListItem Text="Rohan Patil" Value="1109"></asp:ListItem>
                                            <asp:ListItem Text="Rizwan Sayyed" Value="1157"></asp:ListItem>
                                            <asp:ListItem Text="Waleed Shaikh" Value="799"></asp:ListItem>
                                        </asp:ListBox>
					                </td>    
                                    <td>
                                        
                                    </td>
                                </tr>
                            </table>
                            </fieldset>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>

                <AjaxToolkit:TabPanel ID="TabAgent" runat="server" HeaderText="Agent">
                    <ContentTemplate>
                        <fieldset><legend>Enquiry Agent Detail</legend>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td colspan="4">
                                        <fieldset><legend>Enquiry Agent</legend>
                                            <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
                                            <tr>
                                                <td>
                                                    <asp:ListBox ID="lbEnquiryAgent" runat="server" Width="100%" Height="120px">
                                                    </asp:ListBox>
					                            </td>    
                                            </tr>
                                        </table>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                            
                            <div>
                                <asp:ImageButton ID="btnSendAgentEmail" ImageUrl="../Images/email-icon.png" runat="server" 
                                    ToolTip = "Send Email To Agent" OnClick="btnSendAgentEmail_Click" />
                                    <div style="color:green; font-size: 10px;"><h2>Send Enquiry Email To Selected Agent Contact:</h2>
                             </div>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td colspan="2">
                                        <fieldset><legend>Available Agent</legend>
                                            <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
                                            <tr>
                                                <td>
                                                    <asp:ListBox ID="lbAgentCompany" runat="server" SelectionMode="Multiple" Width="100%" Height="120px"
                                                        TabIndex="21" AutoPostBack="true" OnSelectedIndexChanged="lbAgentCompany_IndexChanged">
                                           
                                                    </asp:ListBox>
					                            </td>    
                                            </tr>
                                        </table>
                                        </fieldset>
                                    </td>
                                    <td colspan="2">
                                        <fieldset><legend>Enquiry Contact</legend>
                                            <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%" height="120px">
                                                <tr>
                                                    <td>
                                                        <asp:ListBox ID="lbAgentContact" runat="server" SelectionMode="Multiple" Width="100%" Height="120px" TabIndex="21">
                                           
                                                        </asp:ListBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
            </AjaxToolkit:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

