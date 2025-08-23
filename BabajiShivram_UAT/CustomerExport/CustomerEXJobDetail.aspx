<%@ Page Title="Export Job Detail" Language="C#" MasterPageFile="~/ExportCustomerMaster.master" AutoEventWireup="true" CodeFile="CustomerEXJobDetail.aspx.cs"
    Inherits="CustomerExport_CustomerEXJobDetail" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/css">
        .Tab .ajax__tab_header {white-space:nowrap !important;}
    </script>

    <asp:ScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upJobDetail" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); opacity: .8;">
                    <img alt="progress" src="../Images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>

        </asp:UpdateProgress>
    </div>

    <script type="text/javascript">

        function ActiveTabChanged12() {
            /* Clear Error Message on Tab change Event */
            document.getElementById('<%=lblError.ClientID%>').innerHTML = '';
            document.getElementById('<%=lblError.ClientID%>').className = '';
        }
    </script>
    
    <asp:UpdatePanel ID="upJobDetail" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>

            <style type="text/css">
                .hidden {
                    display: none;
                }

                .accordionHeader, .accordionHeaderSelected {
                    background-position-x: 4px;
                }

                .accordionHeader {
                    width: 50%;
                }
            </style>

            <div align="center">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                
            </div>
            <div class="clear"></div>

            <AjaxToolkit:TabContainer runat="server" ID="Tabs" ActiveTabIndex="0" CssClass="Tab"
                Width="100%" OnClientActiveTabChanged="ActiveTabChanged12" CssTheme="None"
                AutoPostBack="false">
                <!-- Job Detail -->
                <AjaxToolkit:TabPanel runat="server" ID="TabPanelJobDetail" HeaderText="Job Detail">
                    <ContentTemplate>
                        <asp:SqlDataSource ID="DataSourceJobDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="EX_GetParticularJobDetail" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="JobId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <div style="padding: 5px">
                            <AjaxToolkit:Accordion ID="Accordion1" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" Width="95%"
                                ContentCssClass="accordionContent" runat="server" SelectedIndex="0" FadeTransitions="true"
                                SuppressHeaderPostbacks="true" TransitionDuration="250" FramesPerSecond="40"
                                RequireOpenedPane="false" AutoSize="None">
                                <Panes>
                                    <AjaxToolkit:AccordionPane ID="accJobDetail" runat="server">
                                        <Header>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Job Detail
                                        </Header>
                                        <Content>
                                            <asp:Label ID="lblDisplay" runat="server" Visible="true"></asp:Label>
                                            <asp:FormView ID="FVJobDetail" runat="server" DataKeyNames="lid" DataSourceID="DataSourceJobDetail" OnDataBound="FVJobDetail_DataBound"
                                                Width="99%">
                                                <HeaderStyle Font-Bold="True" />
                                                <ItemTemplate>
                                                    <div class="m clear">
                                                        
                                                    </div>
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                                        <tr>
                                                            <td>BS Job No.
                                                            </td>
                                                            <td>
                                                                <b><%# Eval("JobRefNo") %></b>
                                                            </td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Babaji Branch</td>
                                                            <td>
                                                                <%# Eval("BranchName") %>
                                                            </td>
                                                            <td>Cust Ref No.
                                                            </td>
                                                            <td>
                                                                <%# Eval("CustRefNo") %>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Customer Name
                                                            </td>
                                                            <td>
                                                                <%# Eval("Customer") %>
                                                            </td>
                                                            <td>Shipper Name
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblShipper" runat="server" Text='<%#Eval("Shipper") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Customer Division/Branch
                                                            </td>
                                                            <td>
                                                                <%# Eval("CustomerDivision") %>
                                                            </td>
                                                            <td>Customer Plant
                                                            </td>
                                                            <td>
                                                                <%# Eval("CustomerPlant")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Consignee Name
                                                            </td>
                                                            <td>
                                                                <%# Eval("ConsigneeName")%>
                                                            </td>
                                                            <td>Mode
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblTransMode" runat="server" Text='<%# Eval("TransMode")%>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Port Of Loading
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblPortOfLoading" runat="server" Text='<%#Eval("PortOfLoading") %>'></asp:Label>
                                                            </td>
                                                            <td>Port Of Discharge
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("PortOfDischarge") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Consignment Country
                                                            </td>
                                                            <td>
                                                                <%# Eval("ConsignmentCountry")%>
                                                            </td>
                                                            <td>Destination Country
                                                            </td>
                                                            <td>
                                                                <%#Eval("DestinationCountry") %>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Type of Export
                                                            </td>
                                                            <td>
                                                                <%# Eval("ExportType")%>
                                                            </td>
                                                            <td>Shipping Bill Type
                                                            </td>
                                                            <td>
                                                                <%# Eval("ShippingBillType")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Priority</td>
                                                            <td>
                                                                <%#Eval("Priority") %>
                                                            </td>
                                                            <td>Transport By
                                                            </td>
                                                            <td>
                                                                <%# Eval("TransportBy")%>
                                                            </td>
                                                        </tr>
                                                        <tr id="trPickUpDetails" runat="server">
                                                            <td>PickUp Location
                                                            </td>
                                                            <td>
                                                                <%# Eval("PickUpFrom")%>
                                                            </td>
                                                            <td>To</td>
                                                            <td>
                                                                <%# Eval("Destination")%>
                                                            </td>
                                                        </tr>
                                                        <tr id="trPickUpPersonDetails" runat="server">
                                                            <td>Pick Up Date</td>
                                                            <td>
                                                                <%# Eval("PickupDate","{0:dd/MM/yyyy}")%>
                                                            </td>
                                                            <td>PickUp Person Name
                                                            </td>
                                                            <td>
                                                                <%# Eval("PickupPersonName")%>                                                         
                                                            </td>
                                                        </tr>
                                                        <tr id="trPickUpPersonDetails2" runat="server">
                                                            <td>Mobile No</td>
                                                            <td>
                                                                <%# Eval("PickupPhoneNo")%>
                                                            </td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Product Desc
                                                            </td>
                                                            <td>
                                                                <%# Eval("ProductDesc")%>
                                                            </td>
                                                            <td>Buyer Name
                                                            </td>
                                                            <td>
                                                                <%# Eval("BuyerName")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>No Of Packages
                                                            </td>
                                                            <td>
                                                                <%# Eval("NoOfPackages")%>
                                                            </td>
                                                            <td>Package Type
                                                            </td>
                                                            <td>
                                                                <%# Eval("PackageType")%>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>Forward to Babaji Freight?</td>
                                                            <td>
                                                                <%#Eval("ForwardToBabaji") %>
                                                            </td>
                                                            <td>Forwarder Name
                                                            </td>
                                                            <td>
                                                                <%# Eval("ForwarderName")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Container Loaded
                                                            </td>
                                                            <td>
                                                                <%# Eval("ContainerLoaded")%>
                                                            </td>
                                                            <td>Seal No</td>
                                                            <td>
                                                                <%# Eval("SealNo")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Gross Weight
                                                            </td>
                                                            <td>
                                                                <%# Eval("GrossWT")%>
                                                            </td>
                                                            <td>Net Weight
                                                            </td>
                                                            <td>
                                                                <%# Eval("NetWT")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>MBL/MAWBL No
                                                            </td>
                                                            <td>
                                                                <%# Eval("MAWBNo")%>
                                                            </td>
                                                            <td>MBL/MAWBL Date
                                                            </td>
                                                            <td>
                                                                <%# Eval("MAWBDate","{0:dd/MM/yyyy}")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>HBL/HAWBL No
                                                            </td>
                                                            <td>
                                                                <%# Eval("HAWBNo")%>
                                                            </td>
                                                            <td>HBL/HAWBL Date
                                                            </td>
                                                            <td>
                                                                <%# Eval("HAWBDate","{0:dd/MM/yyyy}")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Dimension</td>
                                                            <td>
                                                                <%# Eval("Dimension")%>
                                                            </td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        
                                                    </table>
                                                </ItemTemplate>
                                                
                                            </asp:FormView>

                                        </Content>
                                    </AjaxToolkit:AccordionPane>
                                    <AjaxToolkit:AccordionPane ID="accSBPrepare" runat="server">
                                        <Header>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; SB Preparation Detail</Header>
                                        <Content>
                                            <asp:FormView ID="FVSBPrepare" runat="server" Width="99%" DataKeyNames="lid" DataSourceID="DataSourceJobDetail" OnDataBound="FVSBPrepare_DataBound">
                                                <HeaderStyle Font-Bold="True" />
                                                <ItemTemplate>
                                                    <div class="m clear">
                                                        
                                                    </div>
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                                        <tr>
                                                            <td>BS Job No.
                                                            </td>
                                                            <td>
                                                                <b><%# Eval("JobRefNo") %></b>
                                                            </td>
                                                            <td>Checklist Status
                                                            </td>
                                                            <td>
                                                                <%# Eval("ChecklistStatusName")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Checklist Prepared By
                                                            </td>
                                                            <td>
                                                                <%# Eval("ChecklistPreparedBy") %>
                                                            </td>
                                                            <td>Checklist Prepared Date
                                                            </td>
                                                            <td>
                                                                <%# Eval("ChecklistDate","{0:dd/MM/yyyy hh:mm tt}") %>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Download Checklist
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="lnkChecklistDoc_JobDetail" runat="server" Text="Not Uploaded" Enabled="false"
                                                                    OnClick="lnkChecklistDoc_JobDetail_Click"></asp:LinkButton>
                                                                <asp:HiddenField ID="hdnChecklistDocPath2" runat="server" Value='<%#Eval("ChecklistDocPath") %>' />
                                                            </td>
                                                            <td>FOB Value
                                                            </td>
                                                            <td>
                                                                <%# Eval("FOBValue")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>CIF Value</td>
                                                            <td>
                                                                <%# Eval("CIFValue")%>
                                                            </td>
                                                            <td>Remark</td>
                                                            <td>
                                                                <%# Eval("ChecklistRemark")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                                
                                            </asp:FormView>
                                        </Content>
                                    </AjaxToolkit:AccordionPane>
                                    <AjaxToolkit:AccordionPane ID="accSBFiling" runat="server">
                                        <Header>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; SB Filing / Custom Process Detail</Header>
                                        <Content>
                                            <asp:FormView ID="FVFilingCustomProcess" runat="server" Width="99%" DataKeyNames="lid" DataSourceID="DataSourceJobDetail">
                                                <HeaderStyle Font-Bold="True" />
                                                <ItemTemplate>
                                                    <div class="m clear">
                                                        
                                                    </div>
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                                        <tr>
                                                            <td>BS Job No.
                                                            </td>
                                                            <td>
                                                                <b><%# Eval("JobRefNo") %></b>
                                                            </td>
                                                            <td>SB No
                                                            </td>
                                                            <td>
                                                                <%# Eval("SBNo")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>SB Date
                                                            </td>
                                                            <td>
                                                                <%# Eval("SBDate","{0:dd/MM/yyyy}") %>
                                                            </td>
                                                            <td>To be Mark/Appraising
                                                            </td>
                                                            <td>
                                                                <%# Eval("MarkAppraising") %>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Mark/Passing Date
                                                            </td>
                                                            <td>
                                                                <%# Eval("MarkPassingDate","{0:dd/MM/yyyy}") %>
                                                            </td>
                                                            <td>Registration Date
                                                            </td>
                                                            <td>
                                                                <%# Eval("RegisterationDate","{0:dd/MM/yyyy}") %>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Examine Date
                                                            </td>
                                                            <td>
                                                                <%# Eval("ExamineDate","{0:dd/MM/yyyy}") %>
                                                            </td>
                                                            <td>Examine Report Date
                                                            </td>
                                                            <td>
                                                                <%# Eval("ExamineReportDate","{0:dd/MM/yyyy}") %>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>superintendent LEO Date
                                                            </td>
                                                            <td>
                                                                <%# Eval("LEODate","{0:dd/MM/yyyy}") %>
                                                            </td>
                                                            <td>Remark
                                                            </td>
                                                            <td>
                                                                <%# Eval("CustomRemark") %>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                                
                                            </asp:FormView>
                                        </Content>
                                    </AjaxToolkit:AccordionPane>
                                    <AjaxToolkit:AccordionPane ID="accForm13" runat="server">
                                        <Header>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Form 13 Detail</Header>
                                        <Content>
                                            <asp:FormView ID="FVForm13Detail" runat="server" Width="99%" DataKeyNames="lid" DataSourceID="DataSourceJobDetail">
                                                <HeaderStyle Font-Bold="True" />
                                                <ItemTemplate>
                                                    <div class="m clear">
                                                        
                                                    </div>
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                                        <tr>
                                                            <td>BS Job No.
                                                            </td>
                                                            <td>
                                                                <b><%# Eval("JobRefNo") %></b>
                                                            </td>
                                                            <td>Form 13 Applicable
                                                            </td>
                                                            <td>
                                                                <%# Eval("Form13Required")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Form 13 Date
                                                            </td>
                                                            <td>
                                                                <%# Eval("Form13Date","{0:dd/MM/yyyy}") %>
                                                            </td>
                                                            <td>Transporter Hand Over Date
                                                            </td>
                                                            <td>
                                                                <%# Eval("TransHandoverDate","{0:dd/MM/yyyy}") %>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Container Get IN Date
                                                            </td>
                                                            <td>
                                                                <%# Eval("ContainerGetInDate","{0:dd/MM/yyyy}") %>
                                                            </td>
                                                            <td>Form 13 Created Date</td>
                                                            <td>
                                                                <%# Eval("Form13CreatedDate","{0:dd/MM/yyyy}") %>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Form 13 Created By</td>
                                                            <td>
                                                                <%# Eval("Form13CreatedBy") %>
                                                            </td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                                
                                            </asp:FormView>
                                        </Content>
                                    </AjaxToolkit:AccordionPane>
                                    <AjaxToolkit:AccordionPane ID="accShipmentGetIn" runat="server">
                                        <Header>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Shipment Get IN Detail</Header>
                                        <Content>
                                            <asp:FormView ID="FVShipmentGetIN" runat="server" Width="99%" DataKeyNames="lid" DataSourceID="DataSourceJobDetail" OnDataBound="FVShipmentGetIN_DataBound">
                                                <HeaderStyle Font-Bold="True" />
                                                <ItemTemplate>
                                                    <div class="m clear">
                                                        
                                                    </div>
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                                        <tr>
                                                            <td>BS Job No.
                                                            </td>
                                                            <td>
                                                                <b><%# Eval("JobRefNo") %></b>
                                                            </td>
                                                            <td>Document Hand Over To Shipping Line Date
                                                            </td>
                                                            <td>
                                                                <%# Eval("ShippingLineDate","{0:dd/MM/yyyy}")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Exporter Copy
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="lnkDwnloadExporterCopy" runat="server" Text="Not Uploaded" Enabled="false"
                                                                    OnClick="lnkDwnloadExporterCopy_Click"></asp:LinkButton>
                                                                <asp:HiddenField ID="hdnExporterCopy" runat="server" Value='<%#Eval("ExporterCopyPath") %>' />
                                                            </td>
                                                            <td>VGM Copy
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="lnkDwnloadVGMCopy" runat="server" Text="Not Uploaded" Enabled="false"
                                                                    OnClick="lnkDwnloadVGMCopy_Click"></asp:LinkButton>
                                                                <asp:HiddenField ID="hdnVGMCopy" runat="server" Value='<%#Eval("VGMCopyPath") %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Freight Forwarded Date
                                                            </td>
                                                            <td>
                                                                <%# Eval("FreightForwardedDate","{0:dd/MM/yyyy}")%>
                                                            </td>
                                                            <td>Forwarder Person Name
                                                            </td>
                                                            <td>
                                                                <%# Eval("ForwarderPersonName") %>
                                                            </td>
                                                        </tr>
                                                        <tr id="EmailofForwarder" runat="server">
                                                            <td>Forwarder's Email ID</td>
                                                            <td>
                                                                <%# Eval("ForwardToEmail") %>
                                                            </td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                                
                                            </asp:FormView>
                                        </Content>
                                    </AjaxToolkit:AccordionPane>
                                </Panes>
                            </AjaxToolkit:Accordion>
                        </div>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
                <!-- Status -->
                <AjaxToolkit:TabPanel ID="TabPanelStatus" runat="server" HeaderText="Status">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Job Status</legend>
                            <asp:FormView ID="FormViewJobStatus" HeaderStyle-Font-Bold="true" runat="server"
                                Width="100%" DataSourceID="StatusSqlDataSource">
                                <ItemTemplate>
                                    <div class="m clear">
                                    </div>
                                    <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" id="tbl_JobStatus" runat="server">
                                        <tr style="background-color: #cbcbdc; color: black">
                                            <td><b>Stage</b></td>
                                            <td><b>Status</b></td>
                                            <td><b>Completion Date</b></td>
                                        </tr>
                                        <tr>
                                            <td>Job Creation
                                            </td>
                                            <td>
                                                <%# Eval("JobCreated")%>
                                            
                                            </td>
                                            <td><%# Eval("JobCreatedDate","{0:dd/MM/yyyy}") %></td>
                                        </tr>
                                        <tr>
                                            <td>SB Preparation
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("SBPrepare")%>
                                                </span>
                                            </td>
                                            <td><%# Eval("SBPrepareDate","{0:dd/MM/yyyy}") %></td>
                                        </tr>
                                        <tr>
                                            <td>SB Filing
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("SBFiling")%>
                                                </span>
                                            </td>
                                            <td><%# Eval("SBFilingDate","{0:dd/MM/yyyy}") %></td>
                                        </tr>
                                        <tr>
                                            <td>Custom Process
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("CustomProcess")%>
                                                </span>
                                            </td>
                                            <td><%# Eval("CustomProcessDate","{0:dd/MM/yyyy}") %></td>
                                        </tr>
                                        <tr id="tr_JobStatus" runat="server">
                                            <td>Form 13
                                            </td>
                                            <td>
                                                <%# Eval("Form13")%>        
                                            </td>
                                            <td><%# Eval("Form13Date","{0:dd/MM/yyyy}") %></td>
                                        </tr>
                                        <tr>
                                            <td>Shipment Get IN
                                            </td>
                                            <td>
                                                <%#Eval("ShipmentGetIN") %>
                                            </td>
                                            <td><%# Eval("ShipmentGetINDate","{0:dd/MM/yyyy}") %></td>
                                        </tr>
                                        <tr>
                                            <td>PCD
                                            </td>
                                            <td>
                                                <%# Eval("PCD")%>
                                            </td>
                                            <td><%# Eval("PCDDate","{0:dd/MM/yyyy}") %></td>
                                        </tr>
                                        <tr>
                                            <td>PCD Dispatch
                                            </td>
                                            <td>
                                                <%# Eval("PCD Dispatch")%>
                                            </td>
                                            <td><%# Eval("PCDDispatchDate","{0:dd/MM/yyyy}") %></td>
                                        </tr>
                                        <tr>
                                            <td>Billing
                                            </td>
                                            <td>
                                                <%# Eval("Billing")%>
                                            </td>
                                            <td><%# Eval("BillDate","{0:dd/MM/yyyy}") %></td>
                                        </tr>
                                        <tr>
                                            <td>Billing Dispatch
                                            </td>
                                            <td>
                                                <%# Eval("Billing Dispatch")%>
                                            </td>
                                            <td><%# Eval("BillDispatchDate","{0:dd/MM/yyyy}") %></td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:FormView>
                        </fieldset>
                        <div>
                            <asp:SqlDataSource ID="StatusSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="EX_GetJobALLStatusById" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
                <!-- Document -->
                <AjaxToolkit:TabPanel ID="TabDocument" runat="server" HeaderText="Document">
                    <ContentTemplate>
                        <fieldset>
                            <asp:HiddenField ID="HiddenField1" runat="server" />
                            <asp:HiddenField ID="HiddenField2" runat="server" />
                            <legend>Upload Document</legend>
                            <table border="0" cellpadding="0" cellspacing="0" width="65%" bgcolor="white">
                                <tr>
                                    <td>Document Type &nbsp;
                                        <asp:DropDownList ID="ddDocument" runat="server" Width="60%" TabIndex="1">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:FileUpload ID="fuDocument" runat="server" TabIndex="2" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnUpload" runat="server" Text="Upload" TabIndex="3" OnClick="btnUpload_Click" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset>
                            <legend>Download Document</legend>
                            <asp:GridView ID="GridViewDocument" runat="server" AutoGenerateColumns="False" CssClass="table"
                                Width="99%" DataKeyNames="lId" DataSourceID="DocumentSqlDataSource" OnRowDataBound="GridViewDocument_RowDataBound"
                                CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20" OnRowCommand="GridViewDocument_RowCommand" OnPreRender="GridViewDocument_prerender">

                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DocumentName" HeaderText="Document" />
                                    <asp:BoundField DataField="sName" HeaderText="Uploaded By" />
                                    <asp:BoundField DataField="DocTypeName" HeaderText="Document Type" />
                                    <asp:TemplateField HeaderText="Download">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                                CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerTemplate>
                                    <asp1:GridViewPager ID="GridViewPager1" runat="server" />
                                </PagerTemplate>
                            </asp:GridView>
                        </fieldset>
                        <div>
                            <asp:SqlDataSource ID="DocumentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="EX_GetJobDocsDetail" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
                <!-- Container Details -->
                <AjaxToolkit:TabPanel ID="TabSeaContainer" runat="server" HeaderText="Container" Visible="true">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Container Detail</legend>
                            <div>
                                <asp:GridView ID="gvContainer" runat="server" AllowPaging="True" CssClass="table"
                                    AutoGenerateColumns="False" DataKeyNames="JobId,lid" Width="100%" PageSize="40"
                                    DataSourceID="DataSourceContainer" AllowSorting="true">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Container No" SortExpression="ContainerNo">
                                            <ItemTemplate>
                                                <asp:Label ID="lblContainerNo" runat="server" Text='<%#Eval("ContainerNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblType" runat="server" Text='<%#Eval("ContainerTypeName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Size">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSize" runat="server" Text='<%#Eval("ContainerSizeName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Seal No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSealNo" runat="server" Text='<%#Eval("SealNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="User">
                                            <ItemTemplate>
                                                <asp:Label ID="lblContrUser" runat="server" Text='<%#Eval("UserName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Created Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblContrDate" runat="server" Text='<%#Eval("dtDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="pgr" />
                                </asp:GridView>
                            </div>
                        </fieldset>
                        <div>
                            <asp:SqlDataSource ID="DataSourceContainer" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="EX_GetContainerDetail" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
                <!-- Invoice -->
                <AjaxToolkit:TabPanel ID="TabInvoice" runat="server" HeaderText="Invoice">
                    <ContentTemplate>
                        <div style="overflow: scroll;">
                            <fieldset>
                                <legend>Job Invoice Detail</legend>
                                <asp:GridView ID="gvInvoiceDetail" runat="server" CssClass="table" PagerStyle-CssClass="pgr"
                                    AllowSorting="true" AutoGenerateColumns="false" AllowPaging="true" DataSourceID="DataSourceInvoice"
                                    OnPageIndexChanging="gvInvoiceDetail_PageIndexChanging" PageSize="20" PagerSettings-Position="TopAndBottom"
                                    DataKeyNames="JobId,lid" Width="100%" >
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceNo" runat="server" Text='<%#Eval("InvoiceNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceDate" runat="server" Text='<%#Eval("InvoiceDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice Value">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceValue" runat="server" Text='<%#Eval("InvoiceValue") %>'></asp:Label>
                                            </ItemTemplate>
                                            
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice Currency">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceCurrency" runat="server" Text='<%#Eval("InvoiceCurrency") %>'></asp:Label>
                                            </ItemTemplate>
                                            
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Shipment Term">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShipmentTerm" runat="server" Text='<%#Eval("ShipmentTerm") %>'></asp:Label>
                                            </ItemTemplate>
                                            
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DBK Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDBKAmount" runat="server" Text='<%#Eval("DBKAmount") %>'></asp:Label>
                                            </ItemTemplate>
                                            
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="License No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLicenseNo" runat="server" Text='<%#Eval("LicenseNo") %>'></asp:Label>
                                            </ItemTemplate>
                                            
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="License Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLicenseDate" runat="server" Text='<%#Eval("LicenseDate","{0:dd/MM/yyyy}") %>'
                                                    placeholder="dd/mm/yyyy"></asp:Label>
                                            </ItemTemplate>
                                            
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Freight Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFreightAmount" runat="server" Text='<%#Eval("FreightAmount") %>'></asp:Label>
                                            </ItemTemplate>
                                            
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Insurance Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInsuranceAmount" runat="server" Text='<%#Eval("InsuranceAmount") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <div>
                                    <asp:SqlDataSource ID="DataSourceInvoice" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                        SelectCommand="EX_GetInvoiceDetail" SelectCommandType="StoredProcedure">
                                        <SelectParameters>
                                            <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                    <asp:SqlDataSource ID="DataSourceShipmentTerm" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                        SelectCommand="EX_GetShipmentTerms" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                                </div>
                            </fieldset>
                        </div>
                        <div>
                            <%-- <asp:SqlDataSource ID="SqlDataSourceInvoice" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="EX_GetInvoiceDetail" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>--%>
                        </div>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
                <!-- Delivery Details -->
                <AjaxToolkit:TabPanel runat="server" ID="TabPaneDelivery" HeaderText="Delivery Detail">
                    <ContentTemplate>
                        <div>
                            <div style="overflow: scroll;">
                                <fieldset class="fieldset-AutoWidth">
                                    <legend>Delivery To Warehouse</legend>
                                    <asp:GridView ID="GridViewWarehouse" runat="server" AutoGenerateColumns="False" CssClass="table"
                                        Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                                        DataSourceID="DataSourceWarehouse" OnRowDataBound="GridViewWarehouse_RowDataBound"
                                        CellPadding="4" AllowPaging="True" AllowSorting="True" PagerSettings-Position="TopAndBottom" PageSize="40">
                                        <Columns>
                                            
                                            <asp:TemplateField HeaderText="Sl">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Container No" DataField="ContainerNo" ReadOnly="true" />
                                            <asp:TemplateField HeaderText="Packages">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPackages" runat="server" Text='<%#Eval("NoOfPackages")%>'></asp:Label>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Vehicle No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVehicleNo" runat="server" Text='<%#Eval("VehicleNo")%>'></asp:Label>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Vehicle Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVehicleType" runat="server" Text='<%#Eval("vehiclename") %>'></asp:Label>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Vehicle Received Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVehicleRcvdDate" runat="server" Text='<%#Eval("VehicleRcvdDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Transporter Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTransporter" runat="server" Text='<%#Eval("TransporterName")%>'></asp:Label>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="LR No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLRNo" runat="server" Text='<%#Eval("LRNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="LR Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLRDate" runat="server" Text='<%#Eval("LRDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delivery Point">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDeliveryPoint" runat="server" Text='<%#Eval("DeliveryPoint") %>'></asp:Label>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Dispatch Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDispatchDate" runat="server" Text='<%#Eval("DispatchDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delivery Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDeliveryDate" runat="server" Text='<%#Eval("DeliveryDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                           
                                            <asp:TemplateField HeaderText="Road Permit No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRoadPermitNo" runat="server" Text='<%#Eval("RoadPermitNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Road Permit Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRoadPermitDate" runat="server" Text='<%#Eval("RoadPermitDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cargo Received By">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCargoRecBy" runat="server" Text='<%#Eval("CargoReceivedBy") %>'></asp:Label>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="POA Attachment Path">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDownload" runat="server" Text='<%#Eval("PODAttachmentPath") %>'
                                                        CommandName="Download" CommandArgument='<%#Eval("PODDownload") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="N Form No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNFormNo" runat="server" Text='<%#Eval("NFormNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="N Form Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNFormDate" runat="server" Text='<%#Eval("NFormDate","{0:dd/MM/yyyy}") %>' placeholder="dd/mm/yyyy"></asp:Label>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="N Closing Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNClosingDate" runat="server" Text='<%#Eval("NClosingDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="S Form No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSFormNo" runat="server" Text='<%#Eval("SFormNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="S Form Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSFormDate" runat="server" Text='<%#Eval("SFormDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="S Form Closing Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSClosingDate" runat="server" Text='<%#Eval("SClosingDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Octroi Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOctroiAmount" runat="server" Text='<%#Eval("OctroiAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Octroi Receipt No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOctroiReceiptNo" runat="server" Text='<%#Eval("OctroiReceiptNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Octroi Paid Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOctroiPaidDate" runat="server" Text='<%#Eval("OctroiPaidDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Babaji Challan No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChallanNo" runat="server" Text='<%#Eval("BabajiChallanNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Babaji Challan Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChalanDate" runat="server" Text='<%#Eval("BabajiChallanDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Babaji Challan Copy">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkChallanDownload_Warehouse" runat="server" Text='<%#Eval("BabajiChallanCopyFile") %>'
                                                        CommandName="Download" CommandArgument='<%#Eval("BabajiChallanCopyPath") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Damage Image">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDamageDownload" runat="server" Text='<%#Eval("DamagedImageFile") %>'
                                                        CommandName="Download" CommandArgument='<%#Eval("DamagedImagePath") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Labour Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLabourType" runat="server" Text='<%#Eval("LabourTypeName")%>'></asp:Label>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Driver Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldrivername" runat="server" Text='<%#Eval("DriverName")%>'></asp:Label>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Driver Phone no.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldriverphoneno" runat="server" Text='<%#Eval("DriverPhone")%>'></asp:Label>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="User Name" DataField="UserName" ReadOnly="true" />
                                            <asp:BoundField HeaderText="Date" DataField="DeliveryCreatedDate" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" ReadOnly="true" />
                                        </Columns>
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="GetVehicleMSSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                        SelectCommand="GetVehicleMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                                    <asp:SqlDataSource ID="DataSourceTransporter" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                        SelectCommand="EX_GetCompanyByCategoryID" SelectCommandType="StoredProcedure">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="6" Name="CategoryID" Type="Int32" />
                                            <asp:SessionParameter Name="JobId" SessionField="JobId" Type="Int32" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </fieldset>
                            </div>
                            <asp:SqlDataSource ID="DataSourceWarehouse" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="EX_GetDeliveryDetail" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <div class="m clear">
                            </div>
                        </div>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
                <!-- PCD -->
                <AjaxToolkit:TabPanel runat="server" ID="TabPCD" HeaderText="PCA">
                    <ContentTemplate>
                        <div>
                            <asp:FormView ID="fvPCD" HeaderStyle-Font-Bold="true" runat="server" Width="100%">
                                <ItemTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                        <tr>
                                            <td>
                                                <fieldset>
                                                    <legend>Dispatch- Billing Dept</legend>
                                                    <asp:HiddenField ID="hdnBillingDelivery" Value='<%#Eval("BillingDeliveryId")%>' runat="server" />
                                                    <asp:Panel runat="server" ID="pnlDispatchBillingHand" Visible="false">
                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                                            <tr>
                                                                <td>Status
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label6" Text='<%#GetBooleanToCompletedPending(Eval("PCDToDispatch"))%>' runat="server"></asp:Label>
                                                                </td>
                                                                <td>Delivery Type
                                                                </td>
                                                                <td>
                                                                    <%#Eval("BillingTypeOfDelivery")%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Documents Carrying Person Name
                                                                </td>
                                                                <td>
                                                                    <%#Eval("BillingCourierPersonName")%>
                                                                </td>
                                                                <td>Documents Pick Up / Dispatch Date
                                                                </td>
                                                                <td>
                                                                    <%# Eval("BillingDispatchDate", "{0:dd/MM/yyyy}")%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Documents Delivered Date
                                                                </td>
                                                                <td>
                                                                    <%# Eval("BillingPCDRecvdDate", "{0:dd/MM/yyyy}")%>
                                                                </td>
                                                                <td>Documents Received Person Name
                                                                </td>
                                                                <td>
                                                                    <%#Eval("BillingReceivedPersonName")%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>POD Copy Attachment
                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton ID="lnkPODDispatchBilling" runat="server" Text="Download"
                                                                        CommandArgument='<%#Eval("BillingDispatchDocPath") %>' OnClick="lnkPODCopyDownoad_Click"></asp:LinkButton>
                                                                </td>
                                                                <td></td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td>User Name
                                                                </td>
                                                                <td>
                                                                    <%#Eval("BillingDispatchUser")%>
                                                                </td>
                                                                <td>Completed Date
                                                                </td>
                                                                <td>
                                                                    <%# Eval("BillingDispatchUpdateDate", "{0:dd/MM/yyyy}")%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Panel runat="server" ID="pnlDispatchBillingCour" Visible="false">
                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                                            <tr>
                                                                <td>Status
                                                                
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label7" Text='<%#GetBooleanToCompletedPending(Eval("PCDToDispatch"))%>' runat="server"></asp:Label>
                                                                </td>
                                                                <td>Delivery Type
                                                                </td>
                                                                <td>
                                                                    <%#Eval("BillingTypeOfDelivery")%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Courier Name
                                                                </td>
                                                                <td>
                                                                    <%#Eval("BillingCourierName") %>
                                                                </td>
                                                                <td>Dispatch Docket No.
                                                                </td>
                                                                <td>
                                                                    <%#Eval("BillingDocketNo")%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Dispatch Date
                                                                </td>
                                                                <td>
                                                                    <%# Eval("BillingDispatchDate", "{0:dd/MM/yyyy}")%>
                                                                </td>
                                                                <td>Documents Delivered Date
                                                                </td>
                                                                <td>
                                                                    <%# Eval("BillingPCDRecvdDate", "{0:dd/MM/yyyy}")%>
                                                                </td>

                                                            </tr>
                                                            <tr>
                                                                <td>Documents Received Person Name
                                                                </td>
                                                                <td>
                                                                    <%#Eval("BillingReceivedPersonName")%>
                                                                </td>
                                                                <td>POD Copy Attachment
                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton ID="lnkPODDispatchBillingCour" runat="server" Text="Download"
                                                                        CommandArgument='<%#Eval("BillingDispatchDocPath") %>' OnClick="lnkPODCopyDownoad_Click"></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>User Name
                                                                </td>
                                                                <td>
                                                                    <%#Eval("BillingDispatchUser")%>
                                                                </td>
                                                                <td>Completed Date
                                                                </td>
                                                                <td>
                                                                    <%# Eval("BillingDispatchUpdateDate", "{0:dd/MM/yyyy}")%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <fieldset>
                                                    <legend>Dispatch PCA</legend>
                                                    <asp:HiddenField ID="hdnPCADelivery" Value='<%#Eval("PCADeliveryId")%>' runat="server" />
                                                    <asp:Panel runat="server" ID="pnlDispatchPCAHand" Visible="false">
                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                                            <tr>
                                                                <td>Status
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label8" Text='<%#GetBooleanToCompletedPending(Eval("PCDToDispatchCustomer"))%>' runat="server"></asp:Label>
                                                                </td>
                                                                <td>Delivery Type
                                                                </td>
                                                                <td>
                                                                    <%#Eval("CustomerTypeOfDelivery")%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Documents Carrying Person Name
                                                                </td>
                                                                <td>
                                                                    <%#Eval("CarryingPerson")%>
                                                                </td>
                                                                <td>Documents Pick Up / Dispatch Date
                                                                </td>
                                                                <td>
                                                                    <%# Eval("DispatchDate", "{0:dd/MM/yyyy}")%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Documents Delivered Date
                                                                </td>
                                                                <td>
                                                                    <%# Eval("PCDRecvdDate", "{0:dd/MM/yyyy}")%>
                                                                </td>
                                                                <td>Documents Received Person Name
                                                                </td>
                                                                <td>
                                                                    <%#Eval("ReceivedPersonName")%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>POD Copy Attachment
                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton ID="lnkPODDispatchPCA" runat="server" Text="Download"
                                                                        CommandArgument='<%#Eval("CustomerDispatchDocPath") %>' OnClick="lnkPODCopyDownoad_Click"></asp:LinkButton>
                                                                </td>
                                                                <td></td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td>User Name
                                                                </td>
                                                                <td>
                                                                    <%#Eval("CustomerDispatchUser")%>
                                                                </td>
                                                                <td>Completed Date
                                                                </td>
                                                                <td>
                                                                    <%# Eval("CustomerDispatchUpdateDate", "{0:dd/MM/yyyy}")%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Panel runat="server" ID="pnlDispatchPCACour" Visible="false">
                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                                            <tr>
                                                                <td>Status
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label9" Text='<%#GetBooleanToCompletedPending(Eval("PCDToDispatchCustomer"))%>' runat="server"></asp:Label>
                                                                </td>
                                                                <td>Delivery Type
                                                                </td>
                                                                <td>
                                                                    <%#Eval("CustomerTypeOfDelivery")%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Courier Name
                                                                </td>
                                                                <td>
                                                                    <%#Eval("CourierName")%>
                                                                </td>
                                                                <td>Dispatch Docket No.
                                                                </td>
                                                                <td>
                                                                    <%#Eval("DocketNo")%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Dispatch Date
                                                                </td>
                                                                <td>
                                                                    <%# Eval("DispatchDate", "{0:dd/MM/yyyy}")%>
                                                                </td>
                                                                <td>Documents Delivered Date
                                                                </td>
                                                                <td>
                                                                    <%# Eval("PCDRecvdDate", "{0:dd/MM/yyyy}")%>
                                                                </td>

                                                            </tr>
                                                            <tr>
                                                                <td>Documents Received Person Name
                                                                </td>
                                                                <td>
                                                                    <%#Eval("ReceivedPersonName")%>
                                                                </td>
                                                                <td>POD Copy Attachment
                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton ID="lnkPODDispatchPCACour" runat="server" Text="Download"
                                                                        CommandArgument='<%#Eval("CustomerDispatchDocPath") %>' OnClick="lnkPODCopyDownoad_Click"></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>User Name
                                                                </td>
                                                                <td>
                                                                    <%#Eval("CustomerDispatchUser")%>
                                                                </td>
                                                                <td>Completed Date
                                                                </td>
                                                                <td>
                                                                    <%# Eval("CustomerDispatchUpdateDate", "{0:dd/MM/yyyy}")%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </fieldset>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:FormView>
                            <div id="divPCDDatasource">
                                <asp:SqlDataSource ID="DataSourceScrutinyHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="EX_GetPCDScrutinyApprovalHistory" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <asp:SqlDataSource ID="PCDBilingInvoiceSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetPCDInvoice" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </div>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
                <!-- PCD Document -->
                <AjaxToolkit:TabPanel ID="TABPCDDocument" runat="server" HeaderText="PCA Document">
                    <ContentTemplate>
                        <fieldset>
                            <legend>PCA Document</legend>
                            <asp:GridView ID="GridViewPCADoc" runat="server" AutoGenerateColumns="False" CssClass="table"
                                Width="100%" PagerStyle-CssClass="pgr" DataKeyNames="lid" CellPadding="4" AllowPaging="True"
                                AllowSorting="True" PageSize="20" DataSourceID="PCADocSqlDataSource" OnRowCommand="GridViewPCADoc_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DocumentName" HeaderText="Document Type" SortExpression="DocumentName" />
                                    <asp:BoundField DataField="IsCopy" HeaderText="Copy" SortExpression="IsCopy" />
                                    <asp:BoundField DataField="IsOriginal" HeaderText="Original" SortExpression="IsOriginal" />
                                    <asp:BoundField DataField="UploadedBy" HeaderText="Uploaded By" SortExpression="UploadedBy" />
                                    <asp:BoundField DataField="UploadedDate" HeaderText="Uploaded Date" SortExpression="UploadedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:TemplateField HeaderText="Download">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                                CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                        <fieldset>
                            <legend>Billing Advice Document</legend>
                            <asp:GridView ID="GridViewBillingAdvice" runat="server" AutoGenerateColumns="False"
                                CssClass="table" Width="100%" PagerStyle-CssClass="pgr" DataKeyNames="lid" CellPadding="4"
                                AllowPaging="True" AllowSorting="True" PageSize="20" DataSourceID="BillingAdviceSqlDataSource" OnRowCommand="GridViewBillingAdvice_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DocumentName" HeaderText="Document Type" SortExpression="DocumentName" />
                                    <asp:BoundField DataField="IsCopy" HeaderText="Copy" SortExpression="IsCopy" />
                                    <asp:BoundField DataField="IsOriginal" HeaderText="Original" SortExpression="IsOriginal" />
                                    <asp:BoundField DataField="UploadedBy" HeaderText="Uploaded By" SortExpression="UploadedBy" />
                                    <asp:BoundField DataField="UploadedDate" HeaderText="Uploaded Date" SortExpression="UploadedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:TemplateField HeaderText="Download">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                                CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                        <fieldset>
                            <legend>Billing Dispatch Document</legend>
                            <asp:GridView ID="GridViewBillingDept" runat="server" AutoGenerateColumns="False"
                                CssClass="table" Width="100%" PagerStyle-CssClass="pgr" DataKeyNames="lid" CellPadding="4"
                                AllowPaging="True" AllowSorting="True" PageSize="20" DataSourceID="BillingDeptSqlDataSource" OnRowCommand="GridViewBillingDept_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DocumentName" HeaderText="Document Type" SortExpression="DocumentName" />
                                    <asp:BoundField DataField="IsCopy" HeaderText="Copy" SortExpression="IsCopy" />
                                    <asp:BoundField DataField="IsOriginal" HeaderText="Original" SortExpression="IsOriginal" />
                                    <asp:BoundField DataField="UploadedBy" HeaderText="UploadedBy" SortExpression="UploadedBy" />
                                    <asp:BoundField DataField="UploadedDate" HeaderText="Uploaded Date" SortExpression="UploadedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:TemplateField HeaderText="Download">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                                CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                        <div id="divPCDDocDownloadDataSource">
                            <asp:SqlDataSource ID="PCDDocumentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetUploadedPCDDocument" SelectCommandType="StoredProcedure" OnSelected="PCDDocumentSqlDataSource_Selected">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:SqlDataSource ID="BackOfficeSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetPCDDocumentByWorkFlow" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    <asp:Parameter Name="DocumentForType" DefaultValue="1" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:SqlDataSource ID="PCADocSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetPCDDocumentByWorkFlow" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    <asp:Parameter Name="DocumentForType" DefaultValue="2" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:SqlDataSource ID="BillingAdviceSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetPCDDocumentByWorkFlow" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    <asp:Parameter Name="DocumentForType" DefaultValue="3" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:SqlDataSource ID="BillingDeptSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetPCDDocumentByWorkFlow" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    <asp:Parameter Name="DocumentForType" DefaultValue="4" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>

                <AjaxToolkit:TabPanel runat="server" ID="TabPanelBillDetail" HeaderText="Bill Detail">
                <ContentTemplate>
                    <fieldset>
                         <legend>Bill Detail</legend>  
                            <div class="m clear"></div>
                                <asp:GridView ID="grdBilldetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" PagerStyle-CssClass="pgr" DataKeyNames="lId" PagerSettings-Position="TopAndBottom" 
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40"
                                    DataSourceID="DataSourceBillDetails" ><%--OnRowDataBound="gvPCDDocument_RowDataBound">--%>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="BJVNO" HeaderText="JOB NO" SortExpression="BJVNO" />
                                        <asp:BoundField DataField="INVNO" HeaderText="INVOICE NO" SortExpression="INVNO" />
                                        <asp:BoundField DataField="INVAMOUNT" HeaderText="INVOICE AMOUNT" SortExpression="INVAMOUNT" /> 
                                        <asp:BoundField DataField="INVDATE" HeaderText="INVOICE DATE" DataFormatString="{0:dd/MM/yyyy}" SortExpression="INVDATE" />                                   
                                    </Columns>
                                </asp:GridView>
                    </fieldset> 
                    <fieldset>
                         <legend>Bill Return</legend>  
                            <div class="m clear"></div>
                            <div style="overflow:auto;">
                                <asp:GridView ID="gvBillReturnDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    PagerStyle-CssClass="pgr" DataKeyNames="JobId" DataSourceID="BillReturnSqlDataSource"
                                    AllowPaging="True" AllowSorting="True" PageSize="20" PagerSettings-Position="TopAndBottom" >
                                    <%--OnPreRender="gvBillReturnDetaill_PreRender" OnRowDataBound="gvBillReturnDetail_RowDataBound">--%>
                                    <%-- OnRowCommand="gvJobDetail_RowCommand">--%>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex +1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BS Job No" SortExpression="JobRefNo" Visible="false">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkJobRefNo" CommandName="select" runat="server" Text='<%#Eval("JobRefNo") %>'
                                                    CommandArgument='<%#Bind("JobId") + Bind("BJVlid") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" SortExpression="JobRefNo" Visible="false"/>
                                        <asp:BoundField DataField="CustName" HeaderText="Customer Name" SortExpression="CustName" />
                                        <asp:BoundField DataField="INVNO" HeaderText="INVoice NO" SortExpression="INVNO" />
                                        <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />
                                        <asp:TemplateField HeaderText="Reason">
                                            <ItemTemplate>
                                                 <asp:Label ID="lblReturnReason" runat="server" Text='<%#Bind("Reason") %>' Visible="false"></asp:Label>
                                                <asp:DropDownList ID="ddlBillReason" runat="server" Enabled="false" ForeColor="Black" Visible="false">
                                                    <asp:ListItem Value="0" Text="-Select-"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="GST Invoice Related"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="Teriff/Contract"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="SOP Contract"></asp:ListItem>
                                                    <asp:ListItem Value="4" Text="Dispatch/Wrong Attension"></asp:ListItem>
                                                    <asp:ListItem Value="5" Text="Warehouse"></asp:ListItem>
                                                    <asp:ListItem Value="6" Text="Shipping Line"></asp:ListItem>
                                                    <asp:ListItem Value="7" Text="CFS"></asp:ListItem>
                                                    <asp:ListItem Value="8" Text="Other"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="lblBillReason" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="Reason" HeaderText="Reason" SortExpression="Reason"  Visible="false"/>
                                        <asp:BoundField DataField="RemarkCust" HeaderText="Customer Remark" SortExpression="RemarkCust" />
                                        <asp:BoundField DataField="BillReturnBy" HeaderText="Bill Return By" SortExpression="BillReturnBy" />
                                        <asp:BoundField DataField="BillReturnDate" HeaderText="Bill Return Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="BillReturnDate" />
                                        <asp:BoundField DataField="RemarkBSEnd" HeaderText="Remark BSEnd" SortExpression="RemarkBSEnd" />
                                        <asp:BoundField DataField="ChangesDate" HeaderText="Changes Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ChangesDate" />
                                        <asp:BoundField DataField="NewDispatchDate" HeaderText="New Dispatch Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="NewDispatchDate" />
                                        <asp:BoundField DataField="updDate" HeaderText="Bill Update Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="updDate" />
                                        <asp:BoundField DataField="BillCompleteBy" HeaderText="Bill Completed By" SortExpression="BillCompleteBy" />

                                    </Columns>
                                    <PagerTemplate>
                                        <asp1:GridViewPager runat="server" />
                                    </PagerTemplate>
                                </asp:GridView>
                            </div>
                        </fieldset> 
                    <div>
                        <asp:SqlDataSource ID="DataSourceBillDetails" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="BS_GetCustBillDetail" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="JobId"/>
                            </SelectParameters>
                          </asp:SqlDataSource>

                         <asp:SqlDataSource ID="BillReturnSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="BS_GetJobBillReturnDetail" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="JobId"/>
                                <%--<asp:SessionParameter Name="CustUserId" SessionField="CustUserId" />                                
                                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />--%>
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>           

                </ContentTemplate>
            </AjaxToolkit:TabPanel>
                
            </AjaxToolkit:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

