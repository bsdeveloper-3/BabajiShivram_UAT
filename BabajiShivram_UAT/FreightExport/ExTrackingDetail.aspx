<%@ Page Title="Tracking Details" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ExTrackingDetail.aspx.cs"
    Inherits="FreightExport_ExTrackingDetail" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upUpdateDetail" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upUpdateDetail" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:HiddenField ID="hdnWeight" runat="server" />
                <asp:HiddenField ID="hdnVolume" runat="server" />
                <asp:HiddenField ID="hdnModeId" runat="server" />
                <asp:HiddenField ID="hdnLoadingPortId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnPortOfDischargedId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnBranchEmail" runat="server" />
                <asp:HiddenField ID="hdnIsGST" runat="server" Value="0" />
                <asp:HiddenField ID="hdnIsStateGST" runat="server" Value="0" />
                <asp:HiddenField ID="hdnGSTNo" runat="server" Value="0" />
                <asp:HiddenField ID="hdnStateCode" runat="server" Value="0" />
                <asp:HiddenField ID="hdnPrevPOS" runat="server" Value="0" />
                <asp:HiddenField ID="hdnCurrentPOS" runat="server" Value="0" />
                <asp:HiddenField ID="hdnCountryCode" runat="server" />
                <asp:HiddenField ID="hdnFRJobNo" runat="server" />
            </div>
            <div class="clear">
            </div>

            <AjaxToolkit:Accordion ID="Accordion1" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                ContentCssClass="accordionContent" runat="server" SelectedIndex="0" FadeTransitions="true"
                SuppressHeaderPostbacks="true" TransitionDuration="250" FramesPerSecond="40"
                RequireOpenedPane="false" AutoSize="None">
                <Panes>
                    <AjaxToolkit:AccordionPane ID="accBooking" runat="server">
                        <Header>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Booking Detail</Header>
                        <Content>
                            <%-- Booking Detail--%>
                            <asp:FormView ID="FVFreightDetail" runat="server" DataKeyNames="EnqId" Width="99%" OnDataBound="FVFreightDetail_DataBound">
                                <ItemTemplate>
                                    <div class="m clear">
                                        <asp:Button ID="btnEditFreightDetail" runat="server"
                                            Text="Edit" OnClick="btnEditFreightDetail_Click"/>
                                       
                                        <asp:Button ID="btnBack" runat="server" Text="Back To Tracking" OnClick="btnBack_Click" />
                                       
                                    </div>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                        <tr>
                                            <td><b>Job No.</b>
                                            </td>
                                            <td>
                                                <b><asp:Label ID="lblFRJobNo" Text='<%#Eval("FRJobNo")%>' runat="server"></asp:Label></b>
                                            </td>
                                            <td>
                                                 <b>CHA Job No.</b>
                                             </td>
                                            <td>
                                                <b><%# Eval("ExJobRefNo")%></b>
                                            </td>                                            
                                        </tr>
                                        <tr>
                                            <td>ENQ Ref No
                                            </td>
                                            <td>
                                                <asp:Label ID="Label2" Text='<%#Eval("ENQRefNo")%>' runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                 ENQ Date
                                             </td>
                                            <td>
                                                <asp:Label ID="Label3" Text='<%#Eval("ENQDate")%>' runat="server"></asp:Label>
                                            </td>                                            
                                        </tr>
                                        <tr>
                                            <td>Freight SPC
                                            </td>
                                            <td>
                                                <%# Eval("EnquiryUser")%>
                                            </td>
                                            <td>Booking Date
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("BookingDate","{0:dd/MM/yyyy}")%>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Freight Mode
                                            </td>
                                            <td>
                                                <%# Eval("ModeName")%>
                                            </td>
                                           <td>Sales Representative
                                            </td>
                                            <td>
                                                <%# Eval("SalesRepName")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Customer
                                            </td>
                                            <td>
                                                <%# Eval("Customer")%>
                                            </td>
                                             <td>Branch
                                            </td>
                                            <td>
                                                <%# Eval("BranchName")%>
                                            </td>
                                        </tr>
                                         <tr>
                                            <td>Division
                                            </td>
                                            <td>
                                                <%# Eval("DivisionName")%>
                                            </td>
                                            <td>Plant
                                            </td>
                                            <td>
                                                <%# Eval("PlantName")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Shipper
                                            </td>

                                            <td>
                                                <%# Eval("ShipperName")%>
                                            </td>
                                            <td>Country
                                            </td>
                                            <td>
                                                <%# Eval("CountryName")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Shipper Address
                                            </td>
                                            <td colspan="3">
                                                <%# Eval("ShipperAddress")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Consignee
                                            </td>
                                            <td>
                                                <%# Eval("Consignee")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Enquiry Value</td>
                                            <td>
                                                <%# Eval("EnquiryValue")%>
                                            </td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                         <tr>
                                            <td>Consignee Address
                                            </td>
                                            <td colspan="3">
                                                <%# Eval("ConsigneeAddress")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>No of Packages
                                            </td>
                                            <td>
                                                <%#Eval("NoOfPackages")%>
                                                <%#Eval("PackageType")%>
                                            </td>
                                            <td>Terms
                                            </td>
                                            <td>
                                                <%# Eval("TermsName")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Gross Weight (Kgs)
                                            </td>
                                            <td>
                                                <%#Eval("GrossWeight")%>
                                            </td>
                                            <td>Chargeable Weight (Kgs)
                                            </td>
                                            <td>
                                                <asp:Label ID="lblChargeableWeight" Text='<%#Eval("ChargeableWeight")%>' runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <asp:Panel ID="pnlSea" runat="server" Visible="false">
                                            <tr>
                                                <td>Container 20"
                                                </td>
                                                <td>
                                                    <%#Eval("CountOf20")%>
                                                </td>
                                                <td>Container 40"
                                                </td>
                                                <td>
                                                    <%#Eval("CountOf40")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>LCL (CBM)
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblLCLVolume" Text='<%#Eval("LCLVolume")%>' runat="server"></asp:Label>
                                                </td>
                                                <td>LCL/FCL
                                                </td>
                                                <td>
                                                    <%# Eval("ContainerTypeName")%>
                                                    &nbsp;
                                                    <%# Eval("ContainerSubType")%>
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                        <tr>
                                            <td>Port of Loading
                                            </td>
                                            <td>
                                                <%# Eval("LoadingPortName")%>
                                            </td>
                                            <td>Port of Discharged
                                            </td>
                                            <td>
                                                <%# Eval("PortOfDischargedName")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Invoice No
                                            </td>
                                            <td>
                                                <%# Eval("InvoiceNo")%>
                                            </td>
                                            <td>Invoice Date
                                            </td>
                                            <td>
                                                <%# Eval("InvoiceDate","{0:dd/MM/yyyy}")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>PO No
                                            </td>
                                            <td>
                                                <%# Eval("PONumber")%>
                                            </td>
                                            <td>Overseas Agent
                                            </td>
                                            <td>
                                                <%# Eval("AgentName")%>
                                            </td>
                                        </tr>
                                       
                                        <tr>
                                            <td>Place Of Supply
                                            </td>
                                            <td>
                                                <%# Eval("StateName")%>
                                            </td>
                                            <td>GSTN
                                            </td>
                                            <td>
                                                <%# Eval("ConsigneeGSTN")%>
                                            </td>
                                        </tr>
                                       
                                        
                                        <tr>
                                            <td>Booking Details
                                            </td>
                                            <td colspan="3">
                                                <%# Eval("BookingDetails")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Cust Ref No
                                            </td>
                                            <td colspan="3">
                                                <%#Eval("CustRefNo")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Cargo Description
                                            </td>
                                            <td colspan="3">
                                                <%#Eval("CargoDescription")%>
                                            </td>
                                        </tr>
                                        <%-- <tr>
                                            <td>Debit Note
                                            </td>
                                            <td>
                                                <%# Eval("DebitNoteAmount")%>
                                                <asp:LinkButton ID="lnkCreateDebitNote" Text="Create Debit Note" runat="server"  OnClick="lnkCreateDebitNote_Click"></asp:LinkButton>
                                            </td>
                                            <td>Debit Note Remark
                                            </td>
                                            <td>
                                                <%# Eval("DebitNoteRemark")%>
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td>Transporation By</td>
                                            <td> <%# Eval("TransportBy") %> </td>
                                            <td> CHA By</td>
                                            <td> <%# Eval("CHABy") %></td>
                                        </tr>
                                        <tr>
                                            <td>Created By
                                            </td>
                                            <td>
                                                 <%# Eval("EnqDetailBy")%>
                                            </td>
                                            <td>Created Date
                                            </td>
                                            <td>
                                                <%# Eval("EnqDetailDate", "{0:dd/MM/yyyy}")%>
                                            </td>
                                        </tr>
                                        <asp:Panel ID="pnlCancel" runat="server" Visible="false">
                                            <tr id="trDisplayCancel" runat="server" visible="TRUE">
                                                <td>Job Cancel Reason</td>
                                                <td>
                                                    <%--<asp:Label ID="lblCancelReason" runat="server"></asp:Label>--%>
                                                    <%# Eval("REASON")%>
                                                </td>
                                                <td>Job Cancel Remark</td>
                                                <td>
                                                    <%--<asp:Label ID="lblCancelRemark" runat="server"></asp:Label>--%>
                                                    <%# Eval("REMARK")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Job Cancel By</td>
                                                <td>
                                                    <%# Eval("sName") %>
                                                </td>
                                                <td>Job Cancel Date</td>
                                                <td>
                                                    <%# Eval("dtDate") %>
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                    </table>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div class="m clear">
                                        <asp:Button ID="btnUpdateFreightDetail" runat="server"
                                            Text="Update" ValidationGroup="FreightRequired" TabIndex="18" OnClick="btnUpdateFreightDetail_Click" />
                                        <%-- OnClick="btnUpdateFreightDetail_Click"--%>
                                        <asp:Button ID="btnCancelFreightDetail" runat="server"
                                            CausesValidation="False" Text="Cancel" TabIndex="19" OnClick="btnCancelFreightDetail_Click" />
                                       
                                    </div>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                        <tr id="trJobCancel" runat="server">
                                            <td>Job Cancel Allow</td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="ddlCancelAllow" runat="server" OnSelectedIndexChanged="ddlCancelAllow_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="trCancel" runat="server" visible="false">
                                            <td>Cancel Reason <span id="sp" runat="server" visible="true" style="color: red">*</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlReason" runat="server">
                                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Job Cancel By Customer" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Job Cancel by User" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>Cancel Remark <span id="Span1" runat="server" visible="true" style="color: red">*</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCancelRemark" runat="server" TextMode="MultiLine" Text='<%# Eval("REMARK")%>'></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Job No.
                                               <%-- <asp:RequiredFieldValidator ID="RFVJobNo" runat="server" ControlToValidate="txtJobNo"
                                                    Display="Dynamic" InitialValue="" SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Job No"
                                                    ValidationGroup="FreightRequired"></asp:RequiredFieldValidator>--%>
                                            </td>
                                            <td>
                                                <%#Eval("FRJobNo")%>
                                                <%--<asp:Label ID="lblJobNo" runat="server" Text='<%#Eval("FRJobNo")%>'></asp:Label>--%>
                                               
                                                <%--<asp:TextBox ID="txtJobNo" runat="server" Text='<%#Eval("FRJobNo")%>' TabIndex="1"
                                                    Enabled="false"></asp:TextBox>--%>
                                            </td>
                                            <td>Branch
                                                <asp:RequiredFieldValidator ID="RFVBranch" runat="server" ControlToValidate="ddBranch"
                                                    Display="Dynamic" InitialValue="0" SetFocusOnError="true" Text="*" ErrorMessage="Please Select Branch"
                                                    ValidationGroup="FreightRequired"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddBranch" runat="server" SelectedValue='<%#Eval("BranchId") %>'
                                                    Enabled="false" ForeColor="Black" TabIndex="2">
                                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Mumbai - Cargo Complex" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Mumbai - Nhava Sheva" Value="3"></asp:ListItem>
                                                    <asp:ListItem Text="Delhi" Value="5"></asp:ListItem>
                                                    <asp:ListItem Text="Bangalore" Value="20"></asp:ListItem>
                                                    <asp:ListItem Text="Hyderabad" Value="23"></asp:ListItem>
                                                    <asp:ListItem Text="Jaipur" Value="15"></asp:ListItem>
                                                    <asp:ListItem Text="Ahmedabad" Value="13"></asp:ListItem>
                                                    <asp:ListItem Text="Ankleshwar" Value="16"></asp:ListItem>
                                                    <asp:ListItem Text="Chennai" Value="6"></asp:ListItem>
                                                    <asp:ListItem Text="Gandhidham" Value="8"></asp:ListItem>
                                                    <asp:ListItem Text="Vizag" Value="14"></asp:ListItem>
                                                    <asp:ListItem Text="Kolkata" Value="7"></asp:ListItem>
                                                    <asp:ListItem Text="Punjab" Value="28"></asp:ListItem>
                                                    <asp:ListItem Text="USA" Value="35"></asp:ListItem>
                                                    <asp:ListItem Text="HAZIRA" Value="26"></asp:ListItem>


                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Freight Mode
                                            </td>
                                            <td>
                                                <%# Eval("ModeName")%>
                                                <%--<asp:DropDownList ID="ddFreightMode" runat="server" Width="30%" SelectedValue='<%#Eval("lMode") %>'
                                                    Enabled="false" TabIndex="3" ForeColor="Black">
                                                    <asp:ListItem Text="Air" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Sea" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Breakbulk" Value="3"></asp:ListItem>
                                                </asp:DropDownList>--%>
                                            </td>
                                            <td>Terms
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddTerms" runat="server" DataSourceID="dataSourceTermsMS" DataValueField="lid"
                                                    DataTextField="sName" SelectedValue='<%#Eval("TermsId") %>' TabIndex="4" Width="100px">
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="dataSourceTermsMS" runat="server" ConnectionString="<%$ ConnectionStrings:ConBsImport %>"
                                                    SelectCommand="FR_GetTermsMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Port of Loading
                                            </td>
                                            <td>
                                                <%#Eval("LoadingPortName") %>
                                                <%--<asp:TextBox ID="txtPortLoading" runat="server" Text='<%#Eval("LoadingPortName") %>'
                                                    TabIndex="4" Enabled="false" ForeColor="Black"></asp:TextBox>--%>
                                               <%-- <div id="divwidthLoadingPort">
                                                </div>
                                                <AjaxToolkit:AutoCompleteExtender ID="AutoCompletePortLoading" runat="server" TargetControlID="txtPortLoading"
                                                    CompletionListElementID="divwidthLoadingPort" ServicePath="../WebService/PortOfLoadingAutoComplete.asmx"
                                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthLoadingPort"
                                                    ContextKey="1267" UseContextKey="True" OnClientItemSelected="OnPortOfLoadingSelected"
                                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                                </AjaxToolkit:AutoCompleteExtender>--%>
                                            </td>
                                            <td>Port of Discharged
                                               <%-- <asp:RequiredFieldValidator ID="RFVDischarged" runat="server" ValidationGroup="FreightRequired"
                                                    SetFocusOnError="true" InitialValue="" ControlToValidate="txtPortOfDischarged"
                                                    Text="*" ErrorMessage="Please Select Port of Discharged"> </asp:RequiredFieldValidator>--%>
                                            </td>
                                            <td>
                                                <%#Eval("PortOfDischargedName") %>
                                                <%--<asp:TextBox ID="txtPortOfDischarged" runat="server" Text='<%#Eval("PortOfDischargedName") %>'
                                                    TabIndex="4" Enabled="false" ForeColor="Black"></asp:TextBox>
                                                <div id="divwidthDischargPort">
                                                </div>
                                                <AjaxToolkit:AutoCompleteExtender ID="AutoCompletePortOfDischarged" runat="server"
                                                    TargetControlID="txtPortOfDischarged" CompletionListElementID="divwidthDischargPort"
                                                    ServicePath="../WebService/PortOfLoadingAutoComplete.asmx" ServiceMethod="GetCompletionList"
                                                    MinimumPrefixLength="2" BehaviorID="divwidthDischargPort" ContextKey="7268" UseContextKey="True"
                                                    OnClientItemSelected="OnPortOfDischargedSelected" CompletionListCssClass="AutoExtender"
                                                    CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                                    DelimiterCharacters="" Enabled="True">
                                                </AjaxToolkit:AutoCompleteExtender>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>No of Packages
                                                <asp:CompareValidator ID="CompValPackgs" runat="server" ControlToValidate="txtNoOfPkgs"
                                                    Operator="DataTypeCheck" SetFocusOnError="true" Type="Integer" ErrorMessage="Invalid No of Packages"
                                                    Display="Dynamic" ValidationGroup="FreightRequired"></asp:CompareValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNoOfPkgs" runat="server" Width="25%" MaxLength="8" Text='<%#Eval("NoOfPackages") %>'
                                                    TabIndex="4"></asp:TextBox>
                                                &nbsp<%--;<asp:DropDownList ID="ddPackageType" runat="server" Width="100px">
                                                </asp:DropDownList>--%>
                                            </td>
                                            <td>Customer
                                               <asp:RequiredFieldValidator ID="RFVCustName" runat="server" ValidationGroup="FreightRequired"
                                                    SetFocusOnError="true" ControlToValidate="txtCustomer" Text="*" ErrorMessage="Please Enter Customer Name"
                                                    InitialValue=""> </asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCustomer" runat="server" Text='<%#Eval("Customer") %>' TabIndex="4" Enabled="false"
                                                    Width="90%" OnTextChanged="txtCustomer_TextChanged" AutoPostBack="true" ></asp:TextBox>
                                                <div id="divwidthCust">
                                                </div>
                                                <AjaxToolkit:AutoCompleteExtender ID="CustomerExtender" runat="server" TargetControlID="txtCustomer"
                                                    CompletionListElementID="divwidthCust" ServicePath="../WebService/CustomerAutoComplete.asmx"
                                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCust"
                                                    ContextKey="4317" UseContextKey="True" CompletionListCssClass="AutoExtender"
                                                    CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                                    FirstRowSelected="true">
                                                </AjaxToolkit:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                         <tr>
                                            <td>Division
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="FreightRequired"
                                                    SetFocusOnError="true" ControlToValidate="ddDivision" Text="*" ErrorMessage="Please Select Agent Name"
                                                    InitialValue="0"> </asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddDivision" runat="server" Width="30%" 
                                                     OnSelectedIndexChanged="ddDivision_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                </asp:DropDownList> 
                                            </td>
                                            <td>Plant
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="FreightRequired"
                                                    SetFocusOnError="true" ControlToValidate="ddPlant" Text="*" ErrorMessage="Please Select Agent Name"
                                                    InitialValue="0"> </asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddPlant" runat="server" Width="30%" > 
                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem> 
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Gross Weight (Kgs)
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGrossWeight" runat="server" Width="25%" Text='<%#Eval("GrossWeight") %>'
                                                    TabIndex="5"></asp:TextBox>
                                                <asp:CompareValidator ID="ComValGrossWT" runat="server" ControlToValidate="txtGrossWeight"
                                                    Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" ErrorMessage="Invalid Gross Weight"
                                                    Display="Dynamic" ValidationGroup="FreightRequired"></asp:CompareValidator>
                                            </td>
                                            <td>Chargeable Weight (Kgs)
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtChargWeight" runat="server" Width="25%" Text='<%#Eval("ChargeableWeight") %>'
                                                    TabIndex="5"></asp:TextBox>
                                                <asp:CompareValidator ID="CompValChargeWeight" runat="server" ControlToValidate="txtChargWeight"
                                                    Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" ErrorMessage="Invalid Chargeable Weight"
                                                    Display="Dynamic" ValidationGroup="FreightRequired"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>LCL (CBM)
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLCLVolume" runat="server" Text='<%#Eval("LCLVolume") %>' Width="25%"
                                                    TabIndex="6"></asp:TextBox>
                                            </td>
                                            <td>LCL/FCL
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddContainerType" runat="server" Width="30%" SelectedValue='<%#Eval("ContainerType") %>'
                                                    TabIndex="6">
                                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="FCL" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="LCL" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddSubType" runat="server" SelectedValue='<%#Bind("ContainerSubType") %>'
                                                    Width="100px" TabIndex="6">
                                                    <asp:ListItem Text="-- Sub Type--" Value=""></asp:ListItem>
                                                    <asp:ListItem Text="GP" Value="GP"></asp:ListItem>
                                                    <asp:ListItem Text="HD" Value="HD"></asp:ListItem>
                                                    <asp:ListItem Text="HQ" Value="HQ"></asp:ListItem>
                                                    <asp:ListItem Text="OT" Value="OT"></asp:ListItem>
                                                    <asp:ListItem Text="FR" Value="FR"></asp:ListItem>
                                                    <asp:ListItem Text="FB" Value="FB"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Container 20"
                                                <asp:CompareValidator ID="CompValCon20" runat="server" ControlToValidate="txtCont20"
                                                    Operator="DataTypeCheck" SetFocusOnError="true" Type="Integer" ErrorMessage="Invalid Count Of 20"
                                                    Display="Dynamic" ValidationGroup="RequiredBooking"></asp:CompareValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCont20" Width="20%" runat="server" Text='<%#Eval("CountOf20") %>'
                                                    type="number" TabIndex="6"></asp:TextBox>
                                            </td>
                                            <td>Container 40"
                                                <asp:CompareValidator ID="ComVal40" runat="server" ControlToValidate="txtCont40"
                                                    Operator="DataTypeCheck" SetFocusOnError="true" Type="Integer" ErrorMessage="Invalid Count Of 40"
                                                    Display="Dynamic" ValidationGroup="RequiredBooking"></asp:CompareValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCont40" Width="20%" runat="server" Text='<%#Eval("CountOf40") %>'
                                                    type="number" TabIndex="6"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Invoice No
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtInvoiceNo" runat="server" Text='<%#Eval("InvoiceNo")%>' TabIndex="7"></asp:TextBox>
                                            </td>
                                            <td>Invoice Date
                                                <AjaxToolkit:CalendarExtender ID="calInvoiceDate" runat="server" Enabled="True" EnableViewState="False"
                                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgInvDate" PopupPosition="BottomRight"
                                                    TargetControlID="txtInvoiceDate">
                                                </AjaxToolkit:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtInvoiceDate" runat="server" Text='<%# Eval("InvoiceDate","{0:dd/MM/yyyy}")%>'
                                                    Width="100px" TabIndex="8"></asp:TextBox>
                                                <asp:Image ID="imgInvDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                                                    runat="server" />
                                                <AjaxToolkit:MaskedEditExtender ID="MskExtInvoiceDate" TargetControlID="txtInvoiceDate"
                                                    Mask="99/99/9999" MessageValidatorTip="true" MaskType="Date" AutoComplete="false"
                                                    runat="server">
                                                </AjaxToolkit:MaskedEditExtender>
                                                <AjaxToolkit:MaskedEditValidator ID="MskValInvoiceDate" ControlExtender="MskExtInvoiceDate"
                                                    ControlToValidate="txtInvoiceDate" IsValidEmpty="true" InvalidValueMessage="Invoice Date is invalid"
                                                    InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                                    EmptyValueMessage="Invoice Date Required" MinimumValueMessage="Invalid Date"
                                                    MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2025"
                                                    runat="Server" ValidationGroup="FreightRequired"></AjaxToolkit:MaskedEditValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>PO No
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPONumber" runat="server" Text='<%#Eval("PONumber")%>' TextMode="MultiLine"
                                                    TabIndex="9"></asp:TextBox>
                                            </td>
                                            <td>Overseas Agent
                                                <%--<asp:RequiredFieldValidator ID="RFVAgentID" runat="server" ValidationGroup="FreightRequired"
                                                    SetFocusOnError="true" ControlToValidate="ddAgent" Text="*" ErrorMessage="Please Select Agent Name"
                                                    InitialValue="0"> </asp:RequiredFieldValidator>--%>
                                            </td>
                                            <td>
                                                <%#Eval("AgentName")%>
                                                <%--<asp:TextBox ID="txtOverseasAgent" runat="server" Text='<%#Eval("AgentName")%>' TabIndex="10"></asp:TextBox>--%>
                                               <%-- <asp:DropDownList ID="ddAgent" runat="server" Width="200px" Enabled="false" ></asp:DropDownList>--%>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Consignee
                                                <%--<asp:RequiredFieldValidator ID="RFVConsignee" runat="server" ValidationGroup="FreightRequired"
                                                    SetFocusOnError="true" ControlToValidate="txtConsignee" Text="*" ErrorMessage="Please Enter Consignee Name"
                                                    InitialValue=""> </asp:RequiredFieldValidator>--%>
                                            </td>
                                            <td>
                                                <%#Eval("Consignee")%>
                                               <%-- <asp:TextBox ID="txtConsignee" runat="server" Text='<%#Eval("Consignee")%>' TabIndex="11" Width="200px" Enabled="false" ForeColor="Black"></asp:TextBox>
                                                <div id="divwidthConsignee">
                                                </div>
                                                <AjaxToolkit:AutoCompleteExtender ID="AutoCompleteConsignee" runat="server" TargetControlID="txtConsignee"
                                                    CompletionListElementID="divwidthConsignee" ServicePath="../WebService/FreightConsigneeAutoComplete.asmx"
                                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthConsignee"
                                                    ContextKey="1990" UseContextKey="True" CompletionListCssClass="AutoExtender"
                                                    CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                                    FirstRowSelected="true">
                                                </AjaxToolkit:AutoCompleteExtender>--%>
                                            </td>
                                            <td>Shipper
                                            </td>
                                            <td>
                                                <%#Eval("ShipperName")%>
                                                <%--<asp:TextBox ID="txtShipper" runat="server" Text='<%#Eval("ShipperName")%>' TabIndex="12" Width="200px" Enabled="false" ForeColor="Black"></asp:TextBox>
                                                <div id="divwidthShipper">
                                                </div>
                                                <AjaxToolkit:AutoCompleteExtender ID="AutoCompleteShipper" runat="server" TargetControlID="txtShipper"
                                                    CompletionListElementID="divwidthShipper" ServicePath="~/WebService/FreightShipperAutoComplete.asmx"
                                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthShipper"
                                                    ContextKey="1991" UseContextKey="True" CompletionListCssClass="AutoExtender"
                                                    CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                                    FirstRowSelected="true">
                                                </AjaxToolkit:AutoCompleteExtender>--%>
                                            </td>
                                        </tr>                                       
                                        <tr>
                                            <td>Consignee Address
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtConsigneeAddress" runat="server" Text='<%#Eval("ConsigneeAddress")%>'
                                                    TextMode="MultiLine" TabIndex="13" Enabled="false" ForeColor="Black"></asp:TextBox>
                                            </td>
                                            <td>Shipper Address
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtShipperAddress" runat="server" Text='<%#Eval("ShipperAddress")%>'
                                                    TextMode="MultiLine" TabIndex="14" Enabled="false" ForeColor="Black"></asp:TextBox>
                                            </td>
                                        </tr>
                                         <tr>
                                            <td>Place Of Supply
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddConsigneeState" runat="server" Width="180px" TabIndex="11" Enabled="false" ForeColor="Black"
                                                    AutoPostBack="true">
                                                    <%--OnSelectedIndexChanged="ddConsigneeState_OnSelectedIndexChanged"--%>
                                                </asp:DropDownList>
                                                <%--<asp:RequiredFieldValidator ID="RFVConsigneeState" runat="server" ControlToValidate="ddConsigneeState" Display="Dynamic"
                                                    InitialValue="0" SetFocusOnError="true" Text="*" ErrorMessage="Please Select Place Of Delivery" ValidationGroup="FreightRequired"> </asp:RequiredFieldValidator>--%>
                                            </td>
                                            <td>GSTN
                                              <%--  <asp:RequiredFieldValidator ID="RFVGSTN" runat="server" ControlToValidate="txtGSTN" Display="Dynamic"
                                                    InitialValue="" SetFocusOnError="true" Text="*" ErrorMessage="Please Enter GSTN" ValidationGroup="RequiredBooking"> </asp:RequiredFieldValidator>--%>
                                            </td>
                                            <td>
                                                <%#Eval("ConsigneeGSTN")%>
                                               <%-- <asp:TextBox ID="txtGSTN" runat="server" Text='<%#Eval("ConsigneeGSTN")%>' MaxLength="20"
                                                    AutoPostBack="true" Enabled="false" ForeColor="Black"></asp:TextBox>--%>
                                                <%--OnTextChanged="txtGSTN_TextChanged"--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Booking Details
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtBookingDetails" runat="server" MaxLength="800" TextMode="MultiLine"
                                                    Text='<%# Eval("BookingDetails")%>' Width="90%" Height="50px" TabIndex="15"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Booking Info Date
                                                <AjaxToolkit:CalendarExtender ID="CalBookingDt" runat="server" Enabled="True" EnableViewState="False"
                                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgBookDate" PopupPosition="BottomRight"
                                                    TargetControlID="txtBookingDate">
                                                </AjaxToolkit:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBookingDate" runat="server" Text='<%# Eval("BookingDate","{0:dd/MM/yyyy}")%>'
                                                    Width="100px" TabIndex="16"></asp:TextBox>
                                                <asp:Image ID="imgBookDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                                                    runat="server" />
                                                <AjaxToolkit:MaskedEditExtender ID="MskExtBookingDate" TargetControlID="txtBookingDate"
                                                    Mask="99/99/9999" MessageValidatorTip="true" MaskType="Date" AutoComplete="false"
                                                    runat="server">
                                                </AjaxToolkit:MaskedEditExtender>
                                                <AjaxToolkit:MaskedEditValidator ID="MskValBookingDate" ControlExtender="MskExtBookingDate"
                                                    ControlToValidate="txtBookingDate" IsValidEmpty="false" InvalidValueMessage="Booking Date is invalid"
                                                    InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                                    EmptyValueMessage="Please Enter Booking Info Date" EmptyValueBlurredText="Required"
                                                    MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015"
                                                    MaximumValue="01/01/2025" runat="Server" ValidationGroup="FreightRequired"></AjaxToolkit:MaskedEditValidator>
                                            </td>
                                            <td>Cargo Description
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCargoDescription" runat="server" Text='<%#Eval("CargoDescription")%>'
                                                    TextMode="MultiLine" MaxLength="200" TabIndex="16"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <%--<tr>
                                            <td>Debit Note Amount
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDebitNoteAmount" runat="server" Text='<%#Eval("DebitNoteAmount")%>'></asp:TextBox>
                                            </td>
                                            <td>Debit Note Remark
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDebitNoteRemark" runat="server" Text='<%#Eval("DebitNoteRemark")%>'></asp:TextBox>
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td>Transporation By</td>
                                            <td> 
                                                <asp:RadioButtonList ID="rdlbtnTransport" runat="server"  RepeatDirection="Horizontal" SelectedValue='<%#Eval("TransportId") %>' OnSelectedIndexChanged="rdlbtnTransport_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Text="Babaji" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Customer" Value="2"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td><asp:Label ID="lblTransName" runat="server" Text="Transporter Name"></asp:Label> </td>
                                            <td><asp:TextBox ID="txtTransportBy" runat="server" Text='<%#Eval("TransportName") %>' Visible="false" Placeholder="Transporter Name"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td> CHA By</td>
                                            <td>
                                                <asp:RadioButtonList ID="rdlbtnCHABy" runat="server"  RepeatDirection="Horizontal" SelectedValue='<%#Eval("CHAById") %>' OnSelectedIndexChanged="rdlbtnCHABy_SelectedIndexChanged" AutoPostBack="true" >
                                                    <asp:ListItem Text="Babaji" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Other" Value="2"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td><asp:Label ID="lblChaName" runat="server" Text="Transporter Name"></asp:Label></td>
                                            <td><asp:TextBox ID="txtCHABy" runat="server" Visible="false" Text='<%#Eval("CHAName1") %>' Placeholder="CHA Name"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </EditItemTemplate>
                            </asp:FormView>
                        </Content>
                    </AjaxToolkit:AccordionPane>

                    <AjaxToolkit:AccordionPane ID="accAgentInvoice" runat="server">
                        <Header>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Operation</Header>
                        <Content>
                            <%--<asp:FormView ID="fvAgentInvoice" runat="server" DataKeyNames="EnqId" Width="99%">--%>
                            <asp:FormView ID="fvOperation" runat="server" DataKeyNames="EnqId" Width="99%">
                                <ItemTemplate>
                                    <div class="m clear">
                                        <%--    <asp:Button ID="btnEditAgentInvoice" runat="server" OnClick="btnEditAgentInvoice_Click"
                                            Text="Edit" />--%>
                                        <asp:Button ID="btnEditOperation" runat="server" Text="Edit" OnClick="btnEditOperation_Click" />
                                        <%--OnClick="btnEditAgentInvoice_Click" --%>
                                    </div>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                        <tr>
                                            <td>SB No
                                            </td>
                                            <td>
                                                <%# Eval("SBNO") %>
                                            </td>
                                            <td>SB Date
                                            </td>
                                            <td>
                                                <%# Eval("SBDate", "{0:dd/MM/yyyy}")%>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>Container Pickup Date
                                            </td>
                                            <td>
                                                <%# Eval("ContainerPickDate", "{0:dd/MM/yyyy}") %>
                                            </td>
                                            <td>Customs Permission Date
                                            </td>
                                            <td>
                                                <%# Eval("CustomDate", "{0:dd/MM/yyyy}")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Stuffing Date
                                            </td>
                                            <td>
                                                <%# Eval("StuffingDate", "{0:dd/MM/yyyy}")%>
                                            </td>
                                            <td>CLP Date
                                            </td>
                                            <td>
                                                <span>
                                                    <%#Eval("CLPDate", "{0:dd/MM/yyyy}") %>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Carting Date
                                            </td>
                                            <td>
                                                 <span>
                                                    <%#Eval("CartingDate", "{0:dd/MM/yyyy}") %>
                                                </span>
                                            </td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                ASI By
                                            </td>
                                            <td>
                                                <%# Eval("ASIBy") %>
                                            </td>
                                            <td>ASI Date</td>
                                            <td>
                                                 <span>
                                                    <%#Eval("ASIDate", "{0:dd/MM/yyyy}") %>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Created By
                                            </td>
                                            <td>
                                                 <%# Eval("OperCreatedBy")%>
                                            </td>
                                            <td>Created Date
                                            </td>
                                            <td>
                                                 <%# Eval("OperCreatedDt", "{0:dd/MM/yyyy}")%>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div class="m clear">
                                        <asp:Button ID="btnUpdateOperation" runat="server" Text="Update" ValidationGroup="AgentINVRequired" 
                                            TabIndex="12" OnClick="btnUpdateOperation_Click" />
                                        <%----%>
                                        <asp:Button ID="btnCancelOperation" runat="server"
                                            CausesValidation="False" Text="Cancel" TabIndex="13" OnClick="btnCancelOperatione_Click" />                                        
                                    </div>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                        <tr>
                                            <td>SB No
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSBNo" runat="server" Text='<%#Eval("SBNO")%>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvSBNO" runat="server" ControlToValidate="txtSBNo" ValidationGroup="Required" ErrorMessage="Enter the SB NO.">*
                                                </asp:RequiredFieldValidator>
                                            </td>

                                            <td>SB Date                            
                                                 <AjaxToolkit:CalendarExtender ID="calChequeDate" runat="server" Enabled="True" EnableViewState="False"
                                                     FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgSBDate" PopupPosition="BottomRight"
                                                     TargetControlID="txtSBDate">
                                                 </AjaxToolkit:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSBDate" runat="server" Text='<%# Eval("SBDate","{0:dd/MM/yyyy}")%>'></asp:TextBox>

                                                <asp:Image ID="imgSBDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                <AjaxToolkit:MaskedEditExtender ID="MskExtSBDate" TargetControlID="txtSBDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                    MaskType="Date" AutoComplete="false" runat="server">
                                                </AjaxToolkit:MaskedEditExtender>
                                                <AjaxToolkit:MaskedEditValidator ID="MskValSBDate" ControlExtender="MskExtSBDate" ControlToValidate="txtSBDate" IsValidEmpty="true"
                                                    InvalidValueMessage="SB Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                                    EmptyValueMessage="Required" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2025"
                                                    runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                                                <asp:RequiredFieldValidator ID="rfvSBDate" runat="server" ControlToValidate="txtSBDate" ValidationGroup="Required" ErrorMessage="Enter the SB Date">*
                                                </asp:RequiredFieldValidator>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>Container Pickup Date
                                 
                                                <AjaxToolkit:CalendarExtender ID="calContPickDate" runat="server" Enabled="True" EnableViewState="False"
                                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgContPickDate" PopupPosition="BottomRight"
                                                    TargetControlID="txtContPickDate">
                                                </AjaxToolkit:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtContPickDate" runat="server" Text='<%# Eval("ContainerPickDate","{0:dd/MM/yyyy}")%>'></asp:TextBox>

                                                <asp:Image ID="imgContPickDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                <AjaxToolkit:MaskedEditExtender ID="MskContPickDate" TargetControlID="txtContPickDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                    MaskType="Date" AutoComplete="false" runat="server">
                                                </AjaxToolkit:MaskedEditExtender>
                                                <AjaxToolkit:MaskedEditValidator ID="MskValContPickDate" ControlExtender="MskContPickDate" ControlToValidate="txtContPickDate" IsValidEmpty="true"
                                                    InvalidValueMessage="Container Pickup Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                                    EmptyValueMessage="Required" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2025"
                                                    runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                                                <asp:RequiredFieldValidator ID="rfvContainerDate" runat="server" ControlToValidate="txtContPickDate" ValidationGroup="Required" ErrorMessage="Enter the Container Pickup date">*
                                                </asp:RequiredFieldValidator>
                                            </td>
                                            <td>Customs Permission Date
                            
                                                 <AjaxToolkit:CalendarExtender ID="calCustPerDate" runat="server" Enabled="True" EnableViewState="False"
                                                     FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgCustPermDate" PopupPosition="BottomRight"
                                                     TargetControlID="txtCustomPermiDate">
                                                 </AjaxToolkit:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCustomPermiDate" runat="server" Text='<%# Eval("CustomDate","{0:dd/MM/yyyy}")%>'></asp:TextBox>

                                                <asp:Image ID="imgCustPermDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                <AjaxToolkit:MaskedEditExtender ID="MskCustPerDate" TargetControlID="txtCustomPermiDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                    MaskType="Date" AutoComplete="false" runat="server">
                                                </AjaxToolkit:MaskedEditExtender>
                                                <AjaxToolkit:MaskedEditValidator ID="MskValCustPerDate" ControlExtender="MskCustPerDate" ControlToValidate="txtCustomPermiDate" IsValidEmpty="true"
                                                    InvalidValueMessage="Customs Permission Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                                    EmptyValueMessage="Required" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2025"
                                                    runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                                                <asp:RequiredFieldValidator ID="rfvCustomDate" runat="server" ControlToValidate="txtCustomPermiDate" ValidationGroup="Required" ErrorMessage="Enter Custom Permission date">*
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Stuffing Date
                            
                                                 <AjaxToolkit:CalendarExtender ID="calStuffingDate" runat="server" Enabled="True" EnableViewState="False"
                                                     FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgStuffingDate" PopupPosition="BottomRight"
                                                     TargetControlID="txtStuffingDate">
                                                 </AjaxToolkit:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStuffingDate" runat="server" Text='<%# Eval("StuffingDate","{0:dd/MM/yyyy}")%>'></asp:TextBox>

                                                <asp:Image ID="imgStuffingDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                <AjaxToolkit:MaskedEditExtender ID="MskStuffingDate" TargetControlID="txtStuffingDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                    MaskType="Date" AutoComplete="false" runat="server">
                                                </AjaxToolkit:MaskedEditExtender>
                                                <AjaxToolkit:MaskedEditValidator ID="MskValStuffingDate" ControlExtender="MskStuffingDate" ControlToValidate="txtStuffingDate" IsValidEmpty="true"
                                                    InvalidValueMessage="Customs Permission Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                                    EmptyValueMessage="Required" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2025"
                                                    runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                                                <asp:RequiredFieldValidator ID="rfvStuffingDate" runat="server" ControlToValidate="txtStuffingDate" ValidationGroup="Required" ErrorMessage="Enter Stuffing date">*
                                                </asp:RequiredFieldValidator>
                                            </td>
                                            <td>CLP Date
                            
                                               <AjaxToolkit:CalendarExtender ID="calCLPDate" runat="server" Enabled="True" EnableViewState="False"
                                                   FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgCLPDate" PopupPosition="BottomRight"
                                                   TargetControlID="txtCLPDate">
                                               </AjaxToolkit:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCLPDate" runat="server" Text='<%# Eval("CLPDate","{0:dd/MM/yyyy}")%>'></asp:TextBox>

                                                <asp:Image ID="imgCLPDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                <AjaxToolkit:MaskedEditExtender ID="MskCLPDate" TargetControlID="txtCLPDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                    MaskType="Date" AutoComplete="false" runat="server">
                                                </AjaxToolkit:MaskedEditExtender>
                                                <AjaxToolkit:MaskedEditValidator ID="MskValCLPDate" ControlExtender="MskCLPDate" ControlToValidate="txtCLPDate" IsValidEmpty="true"
                                                    InvalidValueMessage="CLP Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                                    EmptyValueMessage="Required" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2025"
                                                    runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>

                                                <asp:RequiredFieldValidator ID="rfvCLPDate" runat="server" ControlToValidate="txtCLPDate" ValidationGroup="Required" ErrorMessage="Enter CLP date">*
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                             <td>Carting Date
                            
                                               <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" EnableViewState="False"
                                                   FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgCartingDate" PopupPosition="BottomRight"
                                                   TargetControlID="txtCartingDate">
                                               </AjaxToolkit:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCartingDate" runat="server" Text='<%# Eval("CartingDate","{0:dd/MM/yyyy}")%>'></asp:TextBox>

                                                <asp:Image ID="imgCartingDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                <AjaxToolkit:MaskedEditExtender ID="MskCartingDate" TargetControlID="txtCartingDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                    MaskType="Date" AutoComplete="false" runat="server">
                                                </AjaxToolkit:MaskedEditExtender>
                                                <AjaxToolkit:MaskedEditValidator ID="MaskedEditValidator2" ControlExtender="MskCartingDate" ControlToValidate="txtCartingDate" IsValidEmpty="true"
                                                    InvalidValueMessage="Carting Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                                    EmptyValueMessage="Required" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2025"
                                                    runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                                            </td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                             <td>ASI By                           
                                             
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtASIBy" runat="server" Text='<%# Eval("ASIBy","{0:dd/MM/yyyy}")%>'></asp:TextBox>                                           
                                            </td>
                                             <td>ASI Date
                            
                                               <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" EnableViewState="False"
                                                   FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgASIDate" PopupPosition="BottomRight"
                                                   TargetControlID="txtASIDate">
                                               </AjaxToolkit:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtASIDate" runat="server" Text='<%# Eval("ASIDate","{0:dd/MM/yyyy}")%>'></asp:TextBox>

                                                <asp:Image ID="imgASIDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                <AjaxToolkit:MaskedEditExtender ID="MskASIDate" TargetControlID="txtASIDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                    MaskType="Date" AutoComplete="false" runat="server">
                                                </AjaxToolkit:MaskedEditExtender>
                                                <AjaxToolkit:MaskedEditValidator ID="MaskedEditValidator3" ControlExtender="MskASIDate" ControlToValidate="txtASIDate" IsValidEmpty="true"
                                                    InvalidValueMessage="ASI Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                                    EmptyValueMessage="Required" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2025"
                                                    runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </EditItemTemplate>
                            </asp:FormView>
                            <div class="m clear">
                            </div>

                        </Content>
                    </AjaxToolkit:AccordionPane>

                    <AjaxToolkit:AccordionPane ID="accVGMForm13" runat="server">
                        <Header>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;VGM/Form13</Header>
                        <Content>
                            <%--<asp:FormView ID="fvAgentInvoice" runat="server" DataKeyNames="EnqId" Width="99%">--%>
                            <asp:FormView ID="fvVGMForm13" runat="server" DataKeyNames="EnqId" Width="99%">
                                <ItemTemplate>
                                    <div class="m clear">
                                        <%--    <asp:Button ID="btnEditAgentInvoice" runat="server" OnClick="btnEditAgentInvoice_Click"
                                            Text="Edit" />--%>
                                        <asp:Button ID="btnEditVGMForm13" runat="server" Text="Edit" OnClick="btnEditVGMForm13_Click" />
                                        <%--OnClick="btnEditAgentInvoice_Click" --%>
                                    </div>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                        <tr>
                                            <td>SB No
                                            </td>
                                            <td>
                                                <%# Eval("SBNO") %>
                                            </td>
                                            <td>SB Date
                                            </td>
                                            <td>
                                                <%# Eval("SBDate", "{0:dd/MM/yyyy}")%>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>VGM Date
                                            </td>
                                            <td>
                                                <%# Eval("VGMDate", "{0:dd/MM/yyyy}") %>
                                            </td>
                                            <td>Form13 Date
                                            </td>
                                            <td>
                                                <%# Eval("Form13Date", "{0:dd/MM/yyyy}")%>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Created By
                                            </td>
                                            <td>
                                                 <%# Eval("VGMCreatedBy")%>
                                            </td>
                                            <td>Created Date
                                            </td>
                                            <td>
                                                 <%# Eval("VGMCreatedDt", "{0:dd/MM/yyyy}")%>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div class="m clear">
                                        <asp:Button ID="btnUpdateVGMForm13" runat="server"
                                            Text="Update" ValidationGroup="AgentINVRequired" TabIndex="12" OnClick="btnUpdateVGMForm13_Click"  />
                                        
                                        <asp:Button ID="btnCancelVGMForm13" runat="server"
                                            CausesValidation="False" Text="Cancel" TabIndex="13" OnClick="btnCancelVGMForm13_Click"/>
                                       
                                    </div>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                        <tr>
                                            <td>SB No
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSBNo" runat="server" Text='<%#Eval("SBNO")%>'></asp:TextBox>
                                              <%--  <asp:RequiredFieldValidator ID="rfvSBNO" runat="server" ControlToValidate="txtSBNo" ValidationGroup="Required" ErrorMessage="Enter the SB NO.">*
                                                </asp:RequiredFieldValidator>--%>
                                            </td>

                                            <td>SB Date
                                                 <AjaxToolkit:CalendarExtender ID="calChequeDate" runat="server" Enabled="True" EnableViewState="False"
                                                     FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgSBDate" PopupPosition="BottomRight"
                                                     TargetControlID="txtSBDate">
                                                 </AjaxToolkit:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSBDate" runat="server" Text='<%# Eval("SBDate","{0:dd/MM/yyyy}")%>'></asp:TextBox>

                                                <asp:Image ID="imgSBDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                <AjaxToolkit:MaskedEditExtender ID="MskExtSBDate" TargetControlID="txtSBDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                    MaskType="Date" AutoComplete="false" runat="server">
                                                </AjaxToolkit:MaskedEditExtender>
                                                <AjaxToolkit:MaskedEditValidator ID="MskValSBDate" ControlExtender="MskExtSBDate" ControlToValidate="txtSBDate" IsValidEmpty="true"
                                                    InvalidValueMessage="SB Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                                    EmptyValueMessage="Required" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2025"
                                                    runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                                                <asp:RequiredFieldValidator ID="rfvSBDate" runat="server" ControlToValidate="txtSBDate" ValidationGroup="Required" ErrorMessage="Enter the SB Date">*
                                                </asp:RequiredFieldValidator>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>VGM Date
                                                <AjaxToolkit:CalendarExtender ID="calVGMDate" runat="server" Enabled="True" EnableViewState="False"
                                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgVGMDate" PopupPosition="BottomRight"
                                                    TargetControlID="txtVGMDate">
                                                </AjaxToolkit:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtVGMDate" runat="server" Text='<%#Eval("VGMDate")%>'></asp:TextBox>
                                                <asp:Image ID="imgVGMDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                <AjaxToolkit:MaskedEditExtender ID="MskVGMDate" TargetControlID="txtVGMDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                    MaskType="Date" AutoComplete="false" runat="server">
                                                </AjaxToolkit:MaskedEditExtender>
                                                <AjaxToolkit:MaskedEditValidator ID="MskvalVGMDate" ControlExtender="MskVGMDate" ControlToValidate="txtVGMDate" IsValidEmpty="true"
                                                    InvalidValueMessage="VGM Date invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                                    EmptyValueMessage="Required" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2025"
                                                    runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                                               <%-- <asp:RequiredFieldValidator ID="rfvContainerDate" runat="server" ControlToValidate="txtVGMDate" ValidationGroup="Required" ErrorMessage="Enter the VGM date">*
                                                </asp:RequiredFieldValidator>--%>
                                            </td>
                                            <td>Form 13 Date
                                                 <AjaxToolkit:CalendarExtender ID="calForm13Date" runat="server" Enabled="True" EnableViewState="False"
                                                     FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgForm13Date" PopupPosition="BottomRight"
                                                     TargetControlID="txtForm13Date">
                                                 </AjaxToolkit:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtForm13Date" runat="server" Text='<%#Eval("Form13Date")%>'></asp:TextBox>

                                                <asp:Image ID="imgForm13Date" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                <AjaxToolkit:MaskedEditExtender ID="MskForm13Date" TargetControlID="txtForm13Date" Mask="99/99/9999" MessageValidatorTip="true"
                                                    MaskType="Date" AutoComplete="false" runat="server">
                                                </AjaxToolkit:MaskedEditExtender>
                                                <AjaxToolkit:MaskedEditValidator ID="MaskedEditValidator1" ControlExtender="MskForm13Date" ControlToValidate="txtForm13Date" IsValidEmpty="true"
                                                    InvalidValueMessage="Form13 Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                                    EmptyValueMessage="Required" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2025"
                                                    runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtForm13Date" ValidationGroup="Required" ErrorMessage="Enter the Form13 date">*
                                                </asp:RequiredFieldValidator>--%>
                                            </td>
                                        </tr>
                                    </table>
                                </EditItemTemplate>
                            </asp:FormView>
                            <div class="m clear">
                            </div>

                        </Content>
                    </AjaxToolkit:AccordionPane>

                    <AjaxToolkit:AccordionPane ID="acccustPreAlert" runat="server">
                        <Header>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Customer PreAlert</Header>
                        <Content>
                            <%--<asp:FormView ID="fvAgentInvoice" runat="server" DataKeyNames="EnqId" Width="99%">--%>
                            <asp:FormView ID="fvCustPreAlert" runat="server" DataKeyNames="EnqId" Width="99%">
                                <ItemTemplate>
                                    <div class="m clear">
                                        <%--    <asp:Button ID="btnEditAgentInvoice" runat="server" OnClick="btnEditAgentInvoice_Click"
                                            Text="Edit" />--%>
                                        <asp:Button ID="btnEditCustPreAlert" runat="server" Text="Edit" OnClick="btnEditCustPreAlert_Click"/>
                                        <%--OnClick="btnEditAgentInvoice_Click" --%>
                                    </div>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                        <tr>
                                            <td>MBL No
                                            </td>
                                            <td>
                                                <%# Eval("MBLNo") %>
                                            </td>
                                            <td>MBL Date
                                            </td>
                                            <td>
                                                <%# Eval("MBLDate", "{0:dd/MM/yyyy}")%>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>HBL No
                                            </td>
                                            <td>
                                                <%# Eval("HBLNo") %>
                                            </td>
                                            <td>HBL Date
                                            </td>
                                            <td>
                                                <%# Eval("HBLDate", "{0:dd/MM/yyyy}")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Flight Schedule Detail
                                            </td>
                                            <td>
                                                <%#Eval("FlightScheduleDetail")%>
                                            </td>
                                            <td>
                                                LEO Date
                                            </td>
                                            <td>
                                                <%# Eval("LEODate", "{0:dd/MM/yyyy}")%>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Created By
                                            </td>
                                            <td>
                                                 <%# Eval("PreCreatedBy")%>
                                            </td>
                                            <td>Created Date
                                            </td>
                                            <td>
                                                 <%# Eval("PreCreatedDt", "{0:dd/MM/yyyy}")%>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div class="m clear">
                                        <asp:Button ID="btnUpdateCustPreAlert" runat="server" Text="Update" ValidationGroup="AgentINVRequired" 
                                            TabIndex="12" OnClick="btnUpdateCustPreAlert_Click" />
                                                                                
                                        <asp:Button ID="btnCancelCustPreAlert" runat="server"
                                            CausesValidation="False" Text="Cancel" TabIndex="13" OnClick="btnCancelCustPreAlert_Click" />
                                       
                                    </div>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                        <tr>
                                            <td>MBL NO</td>
                                            <td>
                                                <asp:TextBox ID="txtMBLNo" runat="server" Text='<%# Eval("MBLNo") %>'></asp:TextBox>                                                                                      

                                                <%--        <asp:RangeValidator ID="rngvMBLNo" runat="server"  ControlToValidate="txtMBLNo" ValidationGroup="Required"
                                  ErrorMessage="Enter the MBL No">*</asp:RangeValidator>--%>

                                                <%--  <asp:RegularExpressionValidator ID="revMBLNo" runat="server" ErrorMessage="Please Enter 7 digit MBL Number."
                                    Text="*" ValidationExpression="^[A-Z]{10}$" ControlToValidate="txtMBLNo" Display="Dynamic"
                                    ValidationGroup="Required" SetFocusOnError="true"></asp:RegularExpressionValidator>--%>
                                            </td>
                                            <td>MBL Date
                                                <AjaxToolkit:CalendarExtender ID="calMBLDate" runat="server" Enabled="True" EnableViewState="False"
                                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgMBLDate" PopupPosition="BottomRight"
                                                    TargetControlID="txtMBLDate">
                                                </AjaxToolkit:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMBLDate" runat="server" Text='<%#Eval("MBLDate")%>'></asp:TextBox>
                                                <asp:Image ID="imgMBLDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                <AjaxToolkit:MaskedEditExtender ID="MskMBLDate" TargetControlID="txtMBLDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                    MaskType="Date" AutoComplete="false" runat="server">
                                                </AjaxToolkit:MaskedEditExtender>
                                                <AjaxToolkit:MaskedEditValidator ID="MskvalMBLDate" ControlExtender="MskMBLDate" ControlToValidate="txtMBLDate" IsValidEmpty="true"
                                                    InvalidValueMessage="MBL Date invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                                    EmptyValueMessage="Required" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2025"
                                                    runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                                                <asp:RequiredFieldValidator ID="rfvMBLDate" runat="server" ControlToValidate="txtMBLDate" ValidationGroup="Required" ErrorMessage="Enter the MBL date">*
                                                </asp:RequiredFieldValidator>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>HBL No
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtHBLNo" runat="server" Text='<%# Eval("HBLNo") %>'></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="rfvHBLNo" runat="server" ControlToValidate="txtHBLNo" ValidationGroup="Required" ErrorMessage="Enter the HBL No">*
                            </asp:RequiredFieldValidator>--%>                                              

                                                <%--     <asp:RegularExpressionValidator ID="REVHBLNo" runat="server" ErrorMessage="Please Enter 7 digit HBL Number."
                                    Text="*" ValidationExpression="^[0-9]{7}$" ControlToValidate="txtHBLNo" Display="Dynamic"
                                    ValidationGroup="Required" SetFocusOnError="true"></asp:RegularExpressionValidator>--%>                                           

                                            </td>
                                            <td>HBL Date
                                                  <AjaxToolkit:CalendarExtender ID="calHBLDate" runat="server" Enabled="True" EnableViewState="False"
                                                      FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgHBLDate" PopupPosition="BottomRight"
                                                      TargetControlID="txtHBLDate">
                                                  </AjaxToolkit:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtHBLDate" runat="server" Text='<%#Eval("HBLDate")%>'></asp:TextBox>

                                                <asp:Image ID="imgHBLDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />

                                                <AjaxToolkit:MaskedEditExtender ID="MskHBLDate" TargetControlID="txtHBLDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                    MaskType="Date" AutoComplete="false" runat="server">
                                                </AjaxToolkit:MaskedEditExtender>
                                                <AjaxToolkit:MaskedEditValidator ID="MskvalHBLDate" ControlExtender="MskHBLDate" ControlToValidate="txtHBLDate" IsValidEmpty="true"
                                                    InvalidValueMessage="HBL Date invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                                    EmptyValueMessage="Required" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2025"
                                                    runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                                                <asp:RequiredFieldValidator ID="rfvHBLDate" runat="server" ControlToValidate="txtHBLDate" ValidationGroup="Required" ErrorMessage="Enter the HBL date">*
                                                </asp:RequiredFieldValidator>
                                        </tr>
                                        <tr>
                                            <td>
                                                Flight Schedule Details
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFlightSchedule" runat="server" TextMode="MultiLine" Text='<%#Eval("FlightScheduleDetail")%>'> </asp:TextBox>
                                            </td>
                                            <td>
                                                LEO Date
                                            </td>
                                            <td>
                                                 <asp:TextBox ID="txtLeoDate" runat="server" Text='<%#Eval("LEODate", "{0:dd/MM/yyyy}")%>'></asp:TextBox>

                                                 <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" EnableViewState="False"
                                                      FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgLeoDate" PopupPosition="BottomRight"
                                                      TargetControlID="txtLeoDate">
                                                  </AjaxToolkit:CalendarExtender>

                                                <asp:Image ID="imgLeoDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />

                                                <AjaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtHBLDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                    MaskType="Date" AutoComplete="false" runat="server">
                                                </AjaxToolkit:MaskedEditExtender>
                                                <AjaxToolkit:MaskedEditValidator ID="MaskedEditValidator1" ControlExtender="MskHBLDate" ControlToValidate="txtLeoDate" IsValidEmpty="true"
                                                    InvalidValueMessage="Leo Date invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                                    EmptyValueMessage="Required" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2025"
                                                    runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </EditItemTemplate>
                            </asp:FormView>
                            <div class="m clear">
                            </div>

                        </Content>
                    </AjaxToolkit:AccordionPane>

                    <AjaxToolkit:AccordionPane ID="accShippedOnBoard" runat="server">
                        <Header>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Shipped OnBoard</Header>
                        <Content>
                            <%--<asp:FormView ID="fvAgentInvoice" runat="server" DataKeyNames="EnqId" Width="99%">--%>
                            <asp:FormView ID="fvShippedOnBoard" runat="server" DataKeyNames="EnqId" Width="99%">
                                <ItemTemplate>
                                    <div class="m clear">
                                        <%--    <asp:Button ID="btnEditAgentInvoice" runat="server" OnClick="btnEditAgentInvoice_Click"
                                            Text="Edit" />--%>
                                        <asp:Button ID="btnEditShippedOnBoard" runat="server" Text="Edit" OnClick="btnEditShippedOnBoard_Click"/>
                                        <%--OnClick="btnEditAgentInvoice_Click" --%>
                                    </div>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                        <tr>
                                            <td>MBL No
                                            </td>
                                            <td>
                                                <%# Eval("MBLNo") %>
                                            </td>
                                            <td>MBL Date
                                            </td>
                                            <td>
                                                <%# Eval("MBLDate", "{0:dd/MM/yyyy}")%>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>HBL No
                                            </td>
                                            <td>
                                                <%# Eval("HBLNo") %>
                                            </td>
                                            <td>HBL Date
                                            </td>
                                            <td>
                                                <%# Eval("HBLDate", "{0:dd/MM/yyyy}")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>On Board Date
                                            </td>
                                            <td>
                                                <%# Eval("ShipOnboardDate", "{0:dd/MM/yyyy}")%>
                                            </td>
                                            <td></td>
                                            <td></td>
                                        </tr>

                                        <tr>
                                            <td>Created By
                                            </td>
                                            <td>
                                                 <%# Eval("OnBoardCreatedBy")%>
                                            </td>
                                            <td>Created Date
                                            </td>
                                            <td>
                                                 <%# Eval("OnBoardCreatedDt", "{0:dd/MM/yyyy}")%>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div class="m clear">
                                        <asp:Button ID="btnUpdateAgentInvoice" runat="server"
                                            Text="Update" ValidationGroup="AgentINVRequired" TabIndex="12" OnClick="btnUpdateAgentInvoice_Click" />
                                        <%--OnClick="btnUpdateAgentInvoice_Click"--%>
                                        <asp:Button ID="btnCancelShipOnBoard" runat="server"
                                            CausesValidation="False" Text="Cancel" TabIndex="13" OnClick="btnCancelShipOnBoard_Click" />
                                        <%--OnClick="btnCancelAgentInvoice_Click"--%>
                                    </div>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                        <tr>
                                            <td>MBL NO</td>
                                            <td>
                                                <%# Eval("MBLNo") %>
                                            </td>
                                            <td>MBL Date
                         
                                            </td>
                                            <td>
                                                <%# Eval("MBLDate", "{0:dd/MM/yyyy}")%>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>HBL No
                                            </td>
                                            <td>
                                                <%# Eval("HBLNo") %>
                                            </td>
                                            <td>HBL Date
                             
                                            </td>
                                            <td>
                                                <%# Eval("HBLDate", "{0:dd/MM/yyyy}")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>On Board Date
                                                  <AjaxToolkit:CalendarExtender ID="calOnBoardDate" runat="server" Enabled="True" EnableViewState="False"
                                                      FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgOnBoardDate" PopupPosition="BottomRight"
                                                      TargetControlID="txtOnBoardDate">
                                                  </AjaxToolkit:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOnBoardDate" runat="server" Text='<%#Eval("ShipOnboardDate")%>'></asp:TextBox>

                                                <asp:Image ID="imgOnBoardDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />

                                                <AjaxToolkit:MaskedEditExtender ID="MskOnBoardDate" TargetControlID="txtOnBoardDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                    MaskType="Date" AutoComplete="false" runat="server">
                                                </AjaxToolkit:MaskedEditExtender>
                                                <AjaxToolkit:MaskedEditValidator ID="MskvalHBLDate" ControlExtender="MskOnBoardDate" ControlToValidate="txtOnBoardDate" IsValidEmpty="true"
                                                    InvalidValueMessage="OnBoard Date invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                                    EmptyValueMessage="Required" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2025"
                                                    runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                                               <%-- <asp:RequiredFieldValidator ID="rfvOnBoardDate" runat="server" ControlToValidate="txtOnBoardDate" ValidationGroup="Required" ErrorMessage="Enter the OnBoard date">*
                                                </asp:RequiredFieldValidator>--%>
                                            </td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </EditItemTemplate>
                            </asp:FormView>
                            <div class="m clear">
                            </div>

                        </Content>
                    </AjaxToolkit:AccordionPane>
                    <ajaxToolkit:AccordionPane ID="accContainer" runat="server">
                        <Header>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Container</Header>
                        <Content>
                            
                                <fieldset>
                                    <legend>Container Detail</legend>
                                    <asp:GridView ID="gvContainer" runat="server" AllowPaging="true" CssClass="table"
                                        PagerStyle-CssClass="pgr" AutoGenerateColumns="false" DataKeyNames="lid" Width="100%"
                                        PageSize="40" DataSourceID="DataSourceContainer"
                                        AllowSorting="true">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sl">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex +1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Container No" SortExpression="ContainerNo">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblContainerNo" runat="server" Text='<%#Eval("ContainerNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtEditContainerNo" runat="server" Text='<%#Eval("ContainerNo") %>'
                                                        MaxLength="11" Width="100px"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="REVGridContainer" runat="server" ControlToValidate="txtEditContainerNo"
                                                        ValidationGroup="valGridContainer" SetFocusOnError="true" ErrorMessage="Enter 11 Digit Container No."
                                                        Display="Dynamic" ValidationExpression="^[a-zA-Z0-9]{11}$"></asp:RegularExpressionValidator>
                                                    <asp:RequiredFieldValidator ID="RFVGridContainer" runat="server" ControlToValidate="txtEditContainerNo"
                                                        ValidationGroup="valGridContainer" SetFocusOnError="true" ErrorMessage="*" Display="Dynamic">
                                                    </asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblType" runat="server" Text='<%#Eval("ContainerTypeName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList ID="ddEditContainerType" runat="server" SelectedValue='<%#Eval("ContainerType") %>'
                                                        Width="80px">
                                                        <asp:ListItem Text="FCL" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="LCL" Value="2"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Size">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSize" runat="server" Text='<%#Eval("ContainerSizeName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList ID="ddEditContainerSize" runat="server" SelectedValue='<%#Eval("ContainerSize") %>'
                                                        Width="80px">
                                                        <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="20" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="40" Value="2"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="User">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblContrUser" runat="server" Text='<%#Eval("UserName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblContrDate" runat="server" Text='<%#Eval("updDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:CommandField ButtonType="Link" ShowEditButton="true" ShowDeleteButton="true"
                                                ValidationGroup="valGridContainer" />--%>
                                        </Columns>
                                    </asp:GridView>
                                </fieldset>
                            
                            <div>
                                <asp:SqlDataSource ID="DataSourceContainer" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="FOP_GetContainerDetailExport" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </Content>
                    </ajaxToolkit:AccordionPane>
                    <AjaxToolkit:AccordionPane ID="accDocument" runat="server">
                        <Header>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Document</Header>
                        <Content>
                            <fieldset>
                                <legend>Upload</legend>
                                <table border="0" cellpadding="0" cellspacing="0" width="99%" bgcolor="white">
                                    <tr>
                                        <td width="110px" align="center">Document Name
                                            <asp:RequiredFieldValidator ID="RFVDocName" runat="server" ControlToValidate="ddl_DocumentType"
                                                Display="Dynamic" ValidationGroup="validateDocument" SetFocusOnError="true" Text="*"
                                                ErrorMessage="Enter Document Name."></asp:RequiredFieldValidator>
                                        </td>
                                        <td width="50px" align="center">
                                            <%--<asp:TextBox ID="txtDocName" runat="server"></asp:TextBox>--%>
                                            <asp:DropDownList ID="ddl_DocumentType" runat="server" DataSourceID="FrDocTypeSqlDataSource"
                                                DataTextField="Sname" DataValueField="lid">
                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="hdnUploadPath" runat="server" />
                                        </td>
                                        <td align="center">Attachment
                                            <asp:RequiredFieldValidator ID="RFVAttach" runat="server" ControlToValidate="fuDocument"
                                                Display="Dynamic" ValidationGroup="validateDocument" SetFocusOnError="true" Text="*"
                                                ErrorMessage="Attach File For Upload."></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:FileUpload ID="fuDocument" runat="server" />
                                            <asp:Button ID="btnFileUpload" runat="server" Text="Upload" ValidationGroup="validateDocument"
                                                OnClick="btnFileUpload_Click" />
                                            <%-- OnClick="btnFileUpload_Click" --%>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <fieldset>
                                <legend>Download</legend>
                                <asp:GridView ID="gvFreightDocument" runat="server" AutoGenerateColumns="False" Width="99%"
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
                                        <asp:TemplateField HeaderText="View">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkViewDoc11" runat="server" Text="View" CommandName="ViewDoc"
                                                    CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Download">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                                    CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remove">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnlRemoveDocument" runat="server" Text="Remove" CommandName="RemoveDocument"
                                                    CommandArgument='<%#Eval("DocId") %>' OnClientClick="return confirm('Are you sure to remove document?');"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </fieldset>
                            <asp:SqlDataSource ID="FreightDocumentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="FR_GetUploadedDocument" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                                </SelectParameters>
                            </asp:SqlDataSource>

                            <fieldset id="fsRepository" runat="server" visible="false">
                                <legend>Billing Repository</legend>
                                <asp:Button ID="btnShowBillingRepository" runat="server" Visible="false" Text="Show Billing Repository" OnClick="btnShowBillingRepository_Click" />
                                <asp:GridView ID="gvBillingRepository" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" PagerStyle-CssClass="pgr" CellPadding="4" AllowPaging="True" AllowSorting="True"
                                    OnRowCommand="gvBillingRepository_RowCommand" PageSize="20">
                                    <Columns>
                                        <asp:TemplateField HeaderText="SI">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Name" HeaderText="Document" />
                                        <asp:BoundField DataField="CreationTime" HeaderText="Date" />
                                        <asp:TemplateField HeaderText="Download">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownloadRepo" runat="server" Text="Download" CommandName="DownloadRepo"
                                                    CommandArgument='<%#Eval("FullName") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </fieldset>
                        </Content>
                    </AjaxToolkit:AccordionPane>

                       <%-- Billing Advice--%>
                    <AjaxToolkit:AccordionPane ID="accBillingAdvice" runat="server">
                        <Header>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Billing Advice</Header>
                        <Content>
                            <fieldset>
                                <legend>Agent Details </legend>
                                <asp:FormView ID="fvAdvice" runat="server" DataKeyNames="EnqId" Width="99%">
                                    <ItemTemplate>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                            <tr>
                                                <td>Agent Invoice Received?
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblAgentINVRcvd" Text='<%#GetBooleanToYesNo(Eval("IsAgentInvoiceRcvd"))%>'
                                                        runat="server"></asp:Label>
                                                </td>
                                                <td>Sent To Billing Dept
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSenToBillDep" Text='<%#GetBooleanToYesNo(Eval("IsFileSentToBilling"))%>'
                                                        runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <%--<tr>
                                            <td>File Sent To Billing Date
                                            </td>
                                            <td>
                                                <%# Eval("FileSentToBillingDate", "{0:dd/MM/yyyy}")%>
                                            </td>
                                            <td>Remark
                                            </td>
                                            <td>
                                                <%# Eval("AdviceRemark")%>
                                            </td>
                                        </tr>--%>
                                            <tr>
                                                <td>Created By
                                                </td>
                                                <td>
                                                    <%# Eval("AdviceCreatedBy")%>
                                                </td>
                                                <td>Created Date
                                                </td>
                                                <td>
                                                    <%# Eval("AdviceCreatedDate", "{0:dd/MM/yyyy}")%>
                                                </td>
                                            </tr>
                                            <%--<tr>
                                                <td>Agent Name</td>
                                                <td><%# Eval("AgentInvoiceName")%></td>
                                                <td>Invoice Received Date</td>
                                                <td><%# Eval("InvoiceReceivedDate")%></td>
                                            </tr>
                                            <tr>
                                                <td>Agent/Vendor Invoice No</td>
                                                <td><%# Eval("AgentInvoiceNo")%></td>
                                                <td>Invoice Date</td>
                                                <td><%# Eval("AgentInvoiceDate")%></td>
                                            </tr>
                                            <tr>
                                                <td>Invoice Amount</td>
                                                <td><%# Eval("AgentInvoiceAmount")%></td>
                                                <td>Invoice Currency</td>
                                                <td><%# Eval("Currency")%></td>
                                            </tr>
                                            <tr>
                                                <td>Agent Remark</td>
                                                <td><%# Eval("AgentInvoiceRemark")%></td>
                                                <td>Agent Invoice
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="lnkAgentInvoice" runat="server" Text="Download" CommandName="Download"
                                                        CommandArgument='<%#Eval("DocPath") %>'
                                                        OnClick="lbnAgentInvoice_Click">
                                                    </asp:LinkButton>
                                                </td>
                                            </tr>--%>
                                        </table>
                                    </ItemTemplate>

                                </asp:FormView>
                            </fieldset>

                            <fieldset id="BillingScrutiny">
                                <legend>Billing Scrutiny </legend>
                                <asp:GridView ID="gvbillingscrutiny" runat="server" AutoGenerateColumns="False"
                                    CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceBillingScrutiny"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                                    <%--OnRowDataBound="gvbillingscrutiny_RowDataBound"--%>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Status" DataField="Status" />
                                        <asp:BoundField HeaderText="Sent By Billing Advice" DataField="SentBy" />
                                        <asp:BoundField HeaderText="Billing Advice Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Received By Billing Scrutiny" DataField="ReceivedBy" />
                                        <asp:BoundField HeaderText="Billing Scrutiny Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Scrutiny Completed Date" DataField="ScrutinyDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Scrutiny Completed By" DataField="ScrutinyCompletedBy" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                            <fieldset id="DraftInvoice" runat="server">
                                <legend>Draft Invoice</legend>
                                <asp:Label ID="lblConsolidated" runat="server"></asp:Label>
                                <asp:GridView ID="gvDraftInvoice" runat="server" AutoGenerateColumns="False"
                                    CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceDraftinvoice"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                                    <%--OnRowDataBound="gvDraftInvoice_RowDataBound"--%>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Status" DataField="Status" />
                                        <asp:BoundField HeaderText="Sent By Billing Scrutiny" DataField="SentBy" />
                                        <asp:BoundField HeaderText="Billing Scrutiny Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Received By Draft Invoice" DataField="ReceivedBy" />
                                        <asp:BoundField HeaderText="Draft Invoice Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Draft Invoice Completed Date" DataField="DraftInvoiceDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Draft Invoice Completed By" DataField="FAUserName" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Consolidated Job No" DataField="ConsolidatedjobNo" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                            <fieldset id="DraftCheck" runat="server">
                                <legend>Draft Check</legend>
                                <asp:GridView ID="gvDraftcheck" runat="server" AutoGenerateColumns="False"
                                    CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceDraftCheck"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Status" DataField="Status" />
                                        <asp:BoundField HeaderText="Sent By Draft Invoice" DataField="SentBy" />
                                        <asp:BoundField HeaderText="Draft Invoice Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Received By Draft Check" DataField="ReceivedBy" />
                                        <asp:BoundField HeaderText="Draft Check Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Draft Check Completed Date" DataField="DraftCheckDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                            <fieldset id="FinalInvoiceTyping" runat="server">
                                <legend>Final Invoice Typing</legend>
                                <asp:GridView ID="gvFinaltyping" runat="server" AutoGenerateColumns="False"
                                    CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceFinalTyping"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Status" DataField="Status" />
                                        <asp:BoundField HeaderText="Sent By Draft Check" DataField="SentBy" />
                                        <asp:BoundField HeaderText="Draft Check Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Received By Final Typing" DataField="ReceivedBy" />
                                        <asp:BoundField HeaderText="Final Typing Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Final Typing Completed Date" DataField="FinalTypingDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Final Typing Completed by" DataField="FAUserName" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Comment" DataField="Comment" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                            <fieldset id="FinalInvoiceCheck" runat="server">
                                <legend>Final Invoice Check</legend>
                                <asp:GridView ID="gvfinalcheck" runat="server" AutoGenerateColumns="False"
                                    CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceFinalCheck"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Status" DataField="Status" />
                                        <asp:BoundField HeaderText="Sent By Final Typing" DataField="SentBy" />
                                        <asp:BoundField HeaderText="Final Typing Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Received By Final Check" DataField="ReceivedBy" />
                                        <asp:BoundField HeaderText="Final Check Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Final Check Completed Date" DataField="FinalCheckDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                            <fieldset id="Billdispatch" runat="server">
                                <legend>Bill Dispatch</legend>
                                <asp:GridView ID="gvbilldispatch" runat="server" AutoGenerateColumns="False" CssClass="table" Width="99%"
                                    PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceBillDispatch"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Status" DataField="Status" />
                                        <asp:BoundField HeaderText="Sent By Final Check" DataField="SentBy" />
                                        <asp:BoundField HeaderText="Final Check Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Received By Bill Dispatch" DataField="ReceivedBy" />
                                        <asp:BoundField HeaderText="Bill Dispatch Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Bill Dispatch Completed Date" DataField="BillDispatchDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                            <fieldset id="BillRejection" runat="server">
                                <legend>Bill Rejection</legend>
                                <asp:GridView ID="gvBillrejection" runat="server" AutoGenerateColumns="False" CssClass="table" Width="99%" PagerStyle-CssClass="pgr"
                                    DataKeyNames="lId" DataSourceID="DataSourceBillRejection" CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Stage" DataField="Stage" />
                                        <asp:BoundField HeaderText="Rejected by" DataField="RejectedBy" />
                                        <asp:BoundField HeaderText="Bill Rejection Date" DataField="RejectedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Reason" DataField="Reason" />
                                        <asp:BoundField HeaderText="Remark" DataField="Remark" />
                                        <asp:BoundField HeaderText="Followup Date" DataField="FollowupDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Followup Remark" DataField="FollowupRemark" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>


                            <fieldset>
                                <legend>E-Bill Dispatch</legend>
                                <asp:Button ID="btnMailResend" runat="server" Text="Resend Mail" OnClick="btnMailResend_Click" />
                                <asp:GridView ID="GridViewBillingDept" runat="server" AutoGenerateColumns="False"
                                    CssClass="table" Width="100%" PagerStyle-CssClass="pgr" DataKeyNames="lid" CellPadding="4"
                                    AllowPaging="True" AllowSorting="True" PageSize="20" DataSourceID="BillingDeptSqlDataSource">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DocumentName" HeaderText="Billing Dispatch" SortExpression="DocumentName" />
                                        <asp:BoundField DataField="IsCopy" HeaderText="Copy" SortExpression="IsCopy" />
                                        <asp:BoundField DataField="IsOriginal" HeaderText="Original" SortExpression="IsOriginal" />
                                        <asp:BoundField DataField="CreatedBy" HeaderText="User Name" SortExpression="CreatedBy" />
                                    </Columns>
                                </asp:GridView>

                                <asp:GridView ID="gvBillDispatchDocDetail" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table"
                                    OnRowCommand="gvBillDispatchDocDetail_RowCommand"
                                    CellPadding="4" PagerStyle-CssClass="pgr" PageSize="20" PagerSettings-Position="TopAndBottom">
                                    <Columns>
                                        <%--DataSourceID="SqlDataSourceBillDispatchDoc"--%>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DocName" HeaderText="Document Name" SortExpression="DocName" />
                                        <asp:BoundField DataField="UserName" HeaderText="Upload By" />
                                        <asp:BoundField DataField="UploadedDate" HeaderText="Upload Date" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:TemplateField HeaderText="Download">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                                    CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                                <asp:GridView ID="GridViewMailDetail" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table"
                                    DataSourceID="SqlDataSourceMailDetail" OnRowCommand="GridViewMailDetail_RowCommand"
                                    CellPadding="4" PagerStyle-CssClass="pgr" PageSize="20" PagerSettings-Position="TopAndBottom">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="SentTo" HeaderText="Mail Send To" SortExpression="SentTo" />
                                        <asp:BoundField DataField="SentCC" HeaderText="Mail Send Cc" SortExpression="SentCC" />
                                        <asp:TemplateField HeaderText="Subject">
                                            <ItemTemplate>
                                                <div style="word-wrap: break-word; width: 200px; white-space: normal;">
                                                    <asp:Label ID="lblRemark" runat="server" Text='<%#Bind("Subject") %>'></asp:Label>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Message">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownload" runat="server" Text="View Meassage" CommandName="Download"
                                                    CommandArgument='<%#Eval("Message") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="SendBy" HeaderText="Mail Send By" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="SendDate" HeaderText="Mail Send Date" />
                                    </Columns>
                                </asp:GridView>

                                <div id="divPreAlertEmail">
                                    <AjaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" CacheDynamicResults="false"
                                        DropShadow="False" PopupControlID="PanelEmail1" TargetControlID="LinkButton1">
                                    </AjaxToolkit:ModalPopupExtender>

                                    <asp:Panel ID="PanelEmail1" runat="server" CssClass="ModalPopupPanel">
                                        <div class="header">
                                            <div class="fleft">
                                                Customer Email
                                            </div>
                                            <div class="fright">
                                                <asp:ImageButton ID="ImageButton1" ImageUrl="~/Images/delete.gif" runat="server" OnClick="btnEMailCancel_Click" ToolTip="Close" />
                                            </div>
                                        </div>
                                        <div id="dvMail" runat="server" style="max-height: 900px; max-width: 850px;">
                                            <div id="dvMailSend" runat="server" style="max-height: 900px; max-width: 750px;">
                                                <div id="div1" runat="server" style="margin-left: 10px;">
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <asp:LinkButton ID="LinkButton1" runat="server"></asp:LinkButton>
                                    <!--Customer Email Draft End -->
                                </div>

                                <div id="divResendEmail">
                                    <AjaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" CacheDynamicResults="false"
                                        DropShadow="False" PopupControlID="PanelEmail" TargetControlID="LinkButton1">
                                    </AjaxToolkit:ModalPopupExtender>

                                    <asp:Panel ID="PanelEmail" runat="server" CssClass="ModalPopupPanel">
                                        <div class="header">
                                            <div class="fleft">
                                                Customer Email 
                                            </div>
                                            <div class="fright">
                                                <asp:ImageButton ID="ImageClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="ImageClose_Click" ToolTip="Close" />
                                            </div>
                                        </div>
                                        <div class="m"></div>
                                        <div id="Div3" runat="server" style="max-height: 600px; max-width: 850px; overflow: auto;">
                                            <asp:Label ID="lblError1" runat="server"></asp:Label>
                                            <div id="Div4" runat="server" style="max-height: 600px; max-width: 750px;">
                                                <asp:Button ID="btnSendEmail1" runat="server" Text="Send Email" OnClick="btnSendEmail_Click" ValidationGroup="mailRequired"
                                                    OnClientClick="if (!Page_ClientValidate('mailRequired')){ return false; } this.disabled = true; this.value = 'Processing...';" UseSubmitBehavior="false" />
                                                <%--<asp:Label ID="lblStatus" runat="server" Text="abc"></asp:Label>--%><br />
                                                <div class="m">
                                                    <asp:Label ID="Label1" runat="server" EnableViewState="false"></asp:Label>
                                                    Email To:&nbsp;<asp:TextBox ID="txtMailTo" runat="server" Width="85%"></asp:TextBox><br />
                                                    Email CC:&nbsp;<asp:TextBox ID="txtMailCC1" runat="server" Width="85%"></asp:TextBox><br />
                                                    Subject:&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtSubject1" runat="server" Width="85%"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtSubject1" SetFocusOnError="true"
                                                        Text="*" ErrorMessage="Subject Required" ValidationGroup="mailRequired"></asp:RequiredFieldValidator>
                                                    <hr style="border-top: 1px solid #8c8b8b" />
                                                </div>
                                                <div id="divPreviewEmailBillDispatch" runat="server" style="margin-left: 10px;">
                                                </div>
                                                <fieldset style="width: 700px;">
                                                    <legend>Document Attachment</legend>
                                                    <asp:GridView ID="gvDocAttach" runat="server" AutoGenerateColumns="False" Width="100%"
                                                        DataKeyNames="DocId" CssClass="table"
                                                        CellPadding="4" PagerStyle-CssClass="pgr" PageSize="20" PagerSettings-Position="TopAndBottom">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Sl">
                                                                <ItemTemplate>
                                                                    <%#Container.DataItemIndex + 1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Check">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkAttach" runat="server" Checked="true" />
                                                                    <asp:HiddenField ID="hdnDocPath" runat="server" Value='<%#Eval("DocPath") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="DocName" HeaderText="Document Name" SortExpression="DocName" />
                                                            <asp:BoundField DataField="UserName" HeaderText="Uploaded By" />
                                                            <asp:BoundField DataField="UploadedDate" HeaderText="Uploaded Date" DataFormatString="{0:dd/MM/yyyy}" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </fieldset>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <asp:LinkButton ID="LinkButton2" runat="server"></asp:LinkButton>
                                    <!--Customer Email Draft End -->
                                </div>
                                <div>
                                    <asp:SqlDataSource ID="BillingDeptSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                        SelectCommand="GetPCDDocumentByWorkFlow" SelectCommandType="StoredProcedure">
                                        <SelectParameters>
                                            <asp:SessionParameter Name="JobId" SessionField="EnqId" />
                                            <asp:Parameter Name="DocumentForType" DefaultValue="4" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                    <asp:SqlDataSource ID="SqlDataSourceMailDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                        SelectCommand="GetBillDispatchMailDetail" SelectCommandType="StoredProcedure">
                                        <SelectParameters>
                                            <asp:SessionParameter Name="JobId" SessionField="EnqId" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                    <asp:SqlDataSource ID="SqlDataSourceBillDispatchDoc" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                        SelectCommand="GetBillDispatchDocDetail" SelectCommandType="StoredProcedure">
                                        <SelectParameters>
                                            <asp:SessionParameter Name="JobId" SessionField="EnqId" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>
                            </fieldset>

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
                        </Content>
                    </AjaxToolkit:AccordionPane>
                  
                    <%-- Delivery--%>
            <AjaxToolkit:AccordionPane ID="AccordionPane1" runat="server">
                <Header>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Delivery</Header>
                <Content>

                    <fieldset>
                            <legend>Update Delivery Detail</legend>
                            <asp:Button ID="btnJobDelivery" Text="Update" OnClick="btnJobDelivery_Click" runat="server" ValidationGroup="RequiredDelivery" />
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>Job Delivery Date
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtJobDeliveryDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="calJobDeliveryDate" runat="server" Enabled="True"
                                            EnableViewState="False" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgJobDeliveryDate"
                                            PopupPosition="BottomRight" TargetControlID="txtJobDeliveryDate">
                                        </AjaxToolkit:CalendarExtender>
                                        <asp:Image ID="imgJobDeliveryDate" ImageAlign="Top" ImageUrl="Images/btn_calendar.gif"
                                            runat="server" />
                                        <AjaxToolkit:MaskedEditExtender ID="MskExtDeliveryDate" TargetControlID="txtJobDeliveryDate" Mask="99/99/9999"
                                            MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                                        </AjaxToolkit:MaskedEditExtender>
                                        <AjaxToolkit:MaskedEditValidator ID="MskValDeliveryDate" ControlExtender="MskExtDeliveryDate" ControlToValidate="txtJobDeliveryDate"
                                            IsValidEmpty="true" InvalidValueMessage="Delivery Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true"
                                            MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2014" MaximumValue="31/12/2025"
                                            runat="Server" ValidationGroup="RequiredDelivery" Display="Dynamic"></AjaxToolkit:MaskedEditValidator>
                                        <td>Delivery Instruction
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDeliveryIns" runat="server"></asp:TextBox>
                                        </td>
                                </tr>
                                <tr>
                                    <td>Truck Request Date
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTruckRequestDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                                        <asp:Image ID="imgTruckReqDate" ImageAlign="Top" ImageUrl="Images/btn_calendar.gif"
                                            runat="server" />
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True"
                                            EnableViewState="False" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgTruckReqDate"
                                            PopupPosition="BottomRight" TargetControlID="txtTruckRequestDate">
                                        </AjaxToolkit:CalendarExtender>
                                        <AjaxToolkit:MaskedEditExtender ID="MskExtTruckDate" TargetControlID="txtTruckRequestDate" Mask="99/99/9999"
                                            MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                                        </AjaxToolkit:MaskedEditExtender>
                                        <AjaxToolkit:MaskedEditValidator ID="MskValTruckDate" ControlExtender="MskExtTruckDate" ControlToValidate="txtTruckRequestDate"
                                            IsValidEmpty="true" InvalidValueMessage="Truck Request Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true"
                                            MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2014" 
                                            MaximumValue='<%#DateTime.Now.ToString("dd/MM/yyyy") %>'
                                            runat="Server" ValidationGroup="RequiredDelivery" Display="Dynamic"></AjaxToolkit:MaskedEditValidator>
                                    </td>
                                    <td>Delivery Address
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDeliveryAddress" TextMode="MultiLine" MaxLength="100" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Delivery Destination
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDeliveryDestination" runat="server" MaxLength="100"></asp:TextBox>
                                    </td>
                                    <td>Transportation By
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTransportationBy" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>

                    <fieldset class="fieldset-AutoWidth">
                            <legend>Customer Delivery Detail</legend>
                            <asp:GridView ID="GridViewDelivery" runat="server" AutoGenerateColumns="False" CssClass="table"
                                Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                                DataSourceID="DataSourceDelivery" OnRowCommand="GridViewDelivery_RowCommand" OnRowDataBound="GridViewDelivery_RowDataBound"
                                CellPadding="4" AllowPaging="True" AllowSorting="True" PagerSettings-Position="TopAndBottom" PageSize="40">
                                <Columns>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server"
                                                Text="Edit" Font-Underline="true"></asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" Width="45" runat="server"
                                                Text="Update" Font-Underline="true" ValidationGroup="GridDelivery"></asp:LinkButton>
                                            <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" Width="39" CausesValidation="false"
                                                runat="server" Text="Cancel" Font-Underline="true"></asp:LinkButton>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Container No" DataField="ContainerNo" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Packages" DataField="NoOfPackages" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Vehicle No" DataField="VehicleNo" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Transporter" DataField="TransporterName" ReadOnly="true" />
                                    <asp:BoundField HeaderText="LR No" DataField="LRNo" ReadOnly="true" />
                                    <asp:BoundField HeaderText="LR Date" DataField="LRDate" DataFormatString="{0:dd/MM/yyyy}"
                                        ReadOnly="true" />
                                    <asp:BoundField HeaderText="Dispatch Date" DataField="DispatchDate" DataFormatString="{0:dd/MM/yyyy}"
                                        ReadOnly="true" />
                                    <asp:TemplateField HeaderText="Cargo Delivered Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDeliveryDate" runat="server" Text='<%# Eval("DeliveryDate","{0:dd/MM/yyyy}")%>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtDeliveryDate" runat="server" Width="100px" Text='<%# Bind("DeliveryDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                            <AjaxToolkit:CalendarExtender ID="calBOEDate" runat="server" Enabled="True" EnableViewState="False"
                                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDeliveryDate" PopupPosition="BottomRight"
                                                TargetControlID="txtDeliveryDate">
                                            </AjaxToolkit:CalendarExtender>
                                            <asp:Image ID="imgDeliveryDate" ImageAlign="Top" ImageUrl="Images/btn_calendar.gif"
                                                runat="server" />
                                            <asp:RequiredFieldValidator ID="reqvDeliveryDate" runat="server" ErrorMessage="Please Enter Delivery Date."
                                                Text="*" Display="Dynamic" ValidationGroup="GridDelivery" ControlToValidate="txtDeliveryDate"></asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="ComValDeliveryDate" runat="server" ControlToValidate="txtDeliveryDate"
                                                Display="Dynamic" ErrorMessage="Invalid Delivery Date." Type="Date" Text="*" CultureInvariantValues="false"
                                                Operator="DataTypeCheck" ValidationGroup="GridDelivery"></asp:CompareValidator>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Empty Container Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmptyContainerDate" runat="server" Text='<%# Eval("EmptyContRetrunDate","{0:dd/MM/yyyy}")%>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEmptyContainerDate" runat="server" Width="100px" Text='<%# Bind("EmptyContRetrunDate","{0:dd/MM/yyyy}") %>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                            <AjaxToolkit:CalendarExtender ID="calEmptyContainerDate" runat="server" Enabled="true"
                                                EnableViewState="false" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgtxtEmptyContainerDate"
                                                PopupPosition="BottomRight" TargetControlID="txtEmptyContainerDate">
                                            </AjaxToolkit:CalendarExtender>
                                            <asp:Image ID="imgtxtEmptyContainerDate" ImageAlign="top" ImageUrl="Images/btn_calendar.gif"
                                                runat="server" />
                                            <asp:CompareValidator ID="ComValEmptyDate" runat="server" ControlToValidate="txtEmptyContainerDate"
                                                Display="Dynamic" ErrorMessage="Invalid Empty Container Date." Type="Date" Text="*" CultureInvariantValues="false"
                                                Operator="DataTypeCheck" ValidationGroup="GridDelivery"></asp:CompareValidator>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="LR Attachment">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton3" runat="server" Text='<%#Eval("PODAttachmentPath") %>'
                                                CommandName="Download" CommandArgument='<%#Eval("PODDownload") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:FileUpload ID="fuPODAttchment" runat="server" />
                                            <asp:HiddenField ID="hdnPODPath" runat="server" Value='<%#Eval("PODDownload") %>' />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Babaji Challan Copy">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkChallanDownload" runat="server" Text='<%#Eval("BabajiChallanCopyFile") %>'
                                                CommandName="Download" CommandArgument='<%#Eval("BabajiChallanCopyPath") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Damage Copy">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDamageDownload" runat="server" Text='<%#Eval("DamagedImageFile") %>'
                                                CommandName="Download" CommandArgument='<%#Eval("DamagedImagePath") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cargo Recvd Person Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCargoRecvdby" runat="server" Text='<%#Eval("CargoReceivedBy") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtCargoRecvdby" runat="server" Width="100px" Text='<%# Bind("CargoReceivedBy")%>'></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="N Form No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNFormNo" runat="server" Text='<%#Eval("NFormNo") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtNFormNo" runat="server" Width="100px" Text='<%# Bind("NFormNo")%>'></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="N Form Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNFormDate" runat="server" Text='<%#Eval("NFormDate","{0:dd/MM/yyyy}") %>' placeholder="dd/mm/yyyy"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtNFormDate" runat="server" Width="100px" Text='<%# Bind("NFormDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                            <AjaxToolkit:CalendarExtender ID="calNFormDate" runat="server" Enabled="true" EnableViewState="false"
                                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgNFormDate" PopupPosition="BottomRight"
                                                TargetControlID="txtNFormDate">
                                            </AjaxToolkit:CalendarExtender>
                                            <asp:Image ID="imgNFormDate" ImageAlign="top" ImageUrl="Images/btn_calendar.gif"
                                                runat="server" />
                                            <asp:CompareValidator ID="ComValNFormDate" runat="server" ControlToValidate="txtNFormDate"
                                                Display="Dynamic" ErrorMessage="Invalid N Form Date." Type="Date" Text="*" CultureInvariantValues="false"
                                                Operator="DataTypeCheck" ValidationGroup="GridDelivery"></asp:CompareValidator>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="N Closing Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNClosingDate" runat="server" Text='<%#Eval("NClosingDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtNClosingDate" runat="server" Width="100px" Text='<%# Bind("NClosingDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                            <AjaxToolkit:CalendarExtender ID="calNClosingDate" runat="server" Enabled="true"
                                                EnableViewState="false" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgNClosingDate"
                                                PopupPosition="BottomRight" TargetControlID="txtNClosingDate">
                                            </AjaxToolkit:CalendarExtender>
                                            <asp:Image ID="imgNClosingDate" ImageAlign="top" ImageUrl="Images/btn_calendar.gif"
                                                runat="server" />
                                            <asp:CompareValidator ID="ComValNClosingDate" runat="server" ControlToValidate="txtNClosingDate"
                                                Display="Dynamic" ErrorMessage="Invalid N Closing Date." Type="Date" Text="*" CultureInvariantValues="false"
                                                Operator="DataTypeCheck" ValidationGroup="GridDelivery"></asp:CompareValidator>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="S Form No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSFormNo" runat="server" Text='<%#Eval("SFormNo") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtSFormNo" runat="server" Width="100px" Text='<%# Bind("SFormNo")%>'></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="S Form Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSFormDate" runat="server" Text='<%#Eval("SFormDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtSFormDate" runat="server" Width="100px" Text='<%# Bind("SFormDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                            <AjaxToolkit:CalendarExtender ID="calSFormDate" runat="server" Enabled="true" EnableViewState="false"
                                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgSFormDate" PopupPosition="BottomRight"
                                                TargetControlID="txtSFormDate">
                                            </AjaxToolkit:CalendarExtender>
                                            <asp:Image ID="imgSFormDate" ImageAlign="top" ImageUrl="Images/btn_calendar.gif"
                                                runat="server" />
                                            <asp:CompareValidator ID="ComValSFormDate" runat="server" ControlToValidate="txtSFormDate"
                                                Display="Dynamic" ErrorMessage="Invalid S Form Date." Type="Date" Text="*" CultureInvariantValues="false"
                                                Operator="DataTypeCheck" ValidationGroup="GridDelivery"></asp:CompareValidator>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="S Form Closing Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSClosingDate" runat="server" Text='<%#Eval("SClosingDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtSClosingDate" runat="server" Width="100px" Text='<%# Bind("SClosingDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                            <AjaxToolkit:CalendarExtender ID="calSClosingDate" runat="server" Enabled="true"
                                                EnableViewState="false" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgSClosingDate"
                                                PopupPosition="BottomRight" TargetControlID="txtSClosingDate">
                                            </AjaxToolkit:CalendarExtender>
                                            <asp:Image ID="imgSClosingDate" ImageAlign="top" ImageUrl="Images/btn_calendar.gif"
                                                runat="server" />
                                            <asp:CompareValidator ID="ComValSClosingDate" runat="server" ControlToValidate="txtSClosingDate"
                                                Display="Dynamic" ErrorMessage="Invalid S Closing Date." Type="Date" Text="*" CultureInvariantValues="false"
                                                Operator="DataTypeCheck" ValidationGroup="GridDelivery"></asp:CompareValidator>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Octroi Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOctroiAmount" runat="server" Text='<%#Eval("OctroiAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtOctroiAmount" runat="server" Width="100px" Text='<%# Bind("OctroiAmount")%>'></asp:TextBox>
                                            <asp:CompareValidator ID="CompValOctroiAmount" runat="server" ControlToValidate="txtOctroiAmount"
                                                Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" ErrorMessage="Invalid Octroi Amount."
                                                Display="Dynamic" ValidationGroup="GridDelivery"></asp:CompareValidator>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Octroi Receipt No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOctroiReceiptNo" runat="server" Text='<%#Eval("OctroiReceiptNo") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtOctroiReceiptNo" runat="server" Width="100px" Text='<%# Bind("OctroiReceiptNo")%>'></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Octroi Paid Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOctroiPaidDate" runat="server" Text='<%#Eval("OctroiPaidDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtOctroiPaidDate" runat="server" Width="100px" Text='<%# Bind("OctroiPaidDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                            <AjaxToolkit:CalendarExtender ID="calOctroiPaidDate" runat="server" Enabled="true"
                                                EnableViewState="false" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgOctroiPaidDate"
                                                PopupPosition="BottomRight" TargetControlID="txtOctroiPaidDate">
                                            </AjaxToolkit:CalendarExtender>
                                            <asp:Image ID="imgOctroiPaidDate" ImageAlign="top" ImageUrl="Images/btn_calendar.gif"
                                                runat="server" />
                                            <asp:CompareValidator ID="ComValOctroiPaidDate" runat="server" ControlToValidate="txtOctroiPaidDate"
                                                Display="Dynamic" ErrorMessage="Invalid Octroi Paid Date." Type="Date" Text="*" CultureInvariantValues="false"
                                                Operator="DataTypeCheck" ValidationGroup="GridDelivery"></asp:CompareValidator>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Road Permit No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRoadPermitNo" runat="server" Text='<%#Eval("RoadPermitNo") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtRoadPermitNo" runat="server" Width="100px" Text='<%# Bind("RoadPermitNo")%>' MaxLength="100"></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Road Permit Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRoadPermitDate" runat="server" Text='<%#Eval("RoadPermitDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtRoadPermitDate" runat="server" Width="100px" Text='<%# Bind("RoadPermitDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                            <AjaxToolkit:CalendarExtender ID="calRoadPermitDate" runat="server" Enabled="true"
                                                EnableViewState="false" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgRoadPermitDate"
                                                PopupPosition="BottomRight" TargetControlID="txtRoadPermitDate">
                                            </AjaxToolkit:CalendarExtender>
                                            <asp:Image ID="imgRoadPermitDate" ImageAlign="top" ImageUrl="Images/btn_calendar.gif"
                                                runat="server" />
                                            <asp:CompareValidator ID="ComValRoadPermitDate" runat="server" ControlToValidate="txtRoadPermitDate"
                                                Display="Dynamic" ErrorMessage="Invalid Road Permit Date." Type="Date" Text="*" CultureInvariantValues="false"
                                                Operator="DataTypeCheck" ValidationGroup="GridDelivery"></asp:CompareValidator>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Challan No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChallanNo" runat="server" Text='<%#Eval("BabajiChallanNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Challan Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChalanDate" runat="server" Text='<%#Eval("BabajiChallanDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="User Name" DataField="UserName" ReadOnly="true" />
                                </Columns>
                            </asp:GridView>
                        </fieldset>

                    </Content>
            </AjaxToolkit:AccordionPane>
            
            <AjaxToolkit:AccordionPane ID="AccordionPane2" runat="server">
                <Header>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Truck Request</Header>
                    
                <Content>
                      <asp:HiddenField ID="hdnTransReqId" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnConsolidateId" runat="server" Value="0" />
                    <div style="overflow: scroll;">
                            <fieldset>
                                <legend>Truck Request Details</legend>
                                <asp:GridView ID="gvTruckRequest" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                                    DataSourceID="DataSourceTruckRequest" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false"
                                    PageSize="20" OnRowCommand="gvTruckRequest_RowCommand" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="TRRefNo" HeaderText="Ref No" ReadOnly="true" />--%>
                                        <asp:TemplateField HeaderText="Ref No">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkRefNo" runat="server" Text='<%#Eval("TRRefNo") %>' CommandName="Select"
                                                    CommandArgument='<%#Eval("TRRefNo") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="JobRefNo" HeaderText="Job No" ReadOnly="true" />
                                        <asp:BoundField DataField="RequestDate" HeaderText="Truck Request Date" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="CustName" HeaderText="Customer Name" ReadOnly="true" />
                                        <asp:BoundField DataField="VehiclePlaceDate" HeaderText="Vehicle Place Require Date" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="LocationFrom" HeaderText="Location" ReadOnly="true" />
                                        <asp:BoundField DataField="Destination" HeaderText="Destination" ReadOnly="true" />
                                        <asp:BoundField DataField="GrossWeight" HeaderText="Gross Weight (Kgs)" ReadOnly="true" />
                                        <asp:BoundField DataField="Count20" HeaderText="Cont 20'" ReadOnly="true" />
                                        <asp:BoundField DataField="Count40" HeaderText="Cont 40'" ReadOnly="true" />
                                        <asp:BoundField DataField="DelExportType_Value" HeaderText="Delivery Type" ReadOnly="true" />
                                        <asp:BoundField DataField="DelExportType_Value" HeaderText="Export Type" ReadOnly="true" />
                                        <asp:BoundField DataField="Dimension" HeaderText="Dimension" ReadOnly="true" />
                                        <asp:BoundField DataField="PickupPincode" HeaderText="PickUp Pincode" ReadOnly="True" />
                                        <asp:BoundField DataField="PickupCity" HeaderText="PickUp City" ReadOnly="True" />
                                        <asp:BoundField DataField="PickupState" HeaderText="PickUp State" ReadOnly="True" />                                 
                                        <asp:BoundField DataField="DropPincode" HeaderText="Drop Pincode" ReadOnly="True" />
                                        <asp:BoundField DataField="DropCity" HeaderText="Drop City" ReadOnly="True" />
                                        <asp:BoundField DataField="DropState" HeaderText="Drop State" ReadOnly="True" /> 
                                        <asp:TemplateField HeaderText="Packing List">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnPackingList" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20"
                                                    ToolTip="Click to view documents." CommandName="PackingDocs" CommandArgument='<%# Bind("lId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy" ReadOnly="true" />
                                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" SortExpression="CreatedDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="UpdatedBy" HeaderText="Updated By" SortExpression="UpdatedBy" ReadOnly="true" />
                                        <asp:BoundField DataField="UpdatedDate" HeaderText="Updated Date" SortExpression="UpdatedDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                    </Columns>
                                </asp:GridView>

                                <fieldset>
                                    <div id="dvtruckDetail" runat="server" visible="false">
                                    <legend >Fill Job Detail</legend>
                                    
                                        <table id="tblTruckRequest" border="0" cellpadding="0" cellspacing="0" bgcolor="white"  runat="server">
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Button ID="btnSubmit" runat="server" Text="Save" ValidationGroup="Required" OnClick="btnSubmit_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Transport Ref No</td>
                                                <td>
                                                    <asp:Label ID="lblTransRefNo" runat="server" Font-Bold="true"></asp:Label>
                                                </td>
                                                <td>Job Number
                                                </td>
                                                <td>
                                                    <%-- <asp:TextBox ID="txtJobNumber" Width="160px" runat="server" ToolTip="Enter Job Number."
                                                CssClass="SearchTextbox" placeholder="Search" TabIndex="1" AutoPostBack="true"></asp:TextBox>--%>
                                                    <asp:Label ID="lblJobNumber" runat="server" Font-Bold="true"></asp:Label>
                                                    <div id="divwidthCust_Loc" runat="server">
                                                    </div>
                                                </td>
                                            </tr>
                                            <%--<tr>
                                        <td>Customer
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCustomer" runat="server" Enabled="false" Width="290px"></asp:Label>
                                            <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblConsigneeShipper_Title" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblShipper" runat="server"></asp:Label>
                                            <asp:Label ID="lblConsignee" runat="server" Enabled="false" Width="290px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Branch</td>
                                        <td>
                                            <asp:Label ID="lblBranch" runat="server"></asp:Label>
                                        </td>
                                        <td>Port</td>
                                        <td>
                                            <asp:Label ID="lblPort" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>Total No of Containers
                                        </td>
                                        <td>
                                            <asp:Label ID="lblNoOfContainers" runat="server"></asp:Label>
                                        </td>
                                        <td>No of Packages
                                        </td>
                                        <td>
                                            <asp:Label ID="lblNoOfPackgs" runat="server"></asp:Label>
                                            <asp:Label ID="lblPackageType" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Sum of 20
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSum20" runat="server"></asp:Label>
                                        </td>
                                        <td>Sum of 40
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSum40" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Gross Weight</td>
                                        <td>
                                            <asp:Label ID="lblGrossWt" runat="server"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblBOEType_Title" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBOEType" runat="server"></asp:Label>
                                        </td>
                                    </tr>--%>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblType_Title" runat="server" Text="Delivery Type"></asp:Label>
                                                    <asp:RequiredFieldValidator ID="RFVDeliveryType" ValidationGroup="Required" runat="server" Display="Dynamic"
                                                        ControlToValidate="ddDeliveryType" InitialValue="0" ErrorMessage="Please Select Delivery Type"
                                                        Text="*"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlExportType" runat="server" TabIndex="2" AutoPostBack="true" OnSelectedIndexChanged="ddlExportType_SelectedIndexChanged">
                                                        <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Factory Stuff" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Doc Stuff" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="On Wheel" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="Excise Sealing" Value="4"></asp:ListItem>
                                                    </asp:DropDownList>

                                                    <asp:DropDownList ID="ddDeliveryType" runat="server" TabIndex="2" AutoPostBack="true" OnSelectedIndexChanged="ddDeliveryType_SelectedIndexChanged">
                                                        <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Loaded" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="DeStuff" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="LCL" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="Break Bulk" Value="4"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>Delivery Destination
                                            <asp:RequiredFieldValidator ID="rfvDestination" runat="server" ControlToValidate="txtDestination" SetFocusOnError="true"
                                                Text="*" ErrorMessage="Please Enter Delivery Destination" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDestination" runat="server" MaxLength="100" TabIndex="3" Width="250px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Dimension</td>
                                                <td>
                                                    <asp:TextBox ID="txtDimension" runat="server" TextMode="MultiLine" ToolTip="Enter Dimension"
                                                        TabIndex="4" PlaceHolder="Dimension" Width="250px" Visible="true"></asp:TextBox>
                                                </td>
                                                <td>Vehicle Place Require Date
                                <AjaxToolkit:CalendarExtender ID="calVehiclePlaceDate" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="imgVehiclePlaceDate"
                                    Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtVehiclePlaceDate">
                                </AjaxToolkit:CalendarExtender>
                                                    <AjaxToolkit:MaskedEditExtender ID="meeVehiclePlaceDate" TargetControlID="txtVehiclePlaceDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                        MaskType="Date" AutoComplete="false" runat="server">
                                                    </AjaxToolkit:MaskedEditExtender>
                                                    <AjaxToolkit:MaskedEditValidator ID="mevVehiclePlaceDate" ControlExtender="meeVehiclePlaceDate" ControlToValidate="txtVehiclePlaceDate" IsValidEmpty="false"
                                                        InvalidValueMessage="Vehicle Place Require Date is invalid" InvalidValueBlurredMessage="Invalid Vehicle Place Require Date" SetFocusOnError="true"
                                                        MinimumValueMessage="Invalid Vehicle Place Require Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="31/12/2025"
                                                        runat="Server" ValidationGroup="Required" EmptyValueMessage="Please enter vehicle place require date." EmptyValueBlurredText="*"></AjaxToolkit:MaskedEditValidator>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtVehiclePlaceDate" runat="server" Width="125px" placeholder="dd/mm/yyyy" TabIndex="5" ToolTip="Enter Vehicle Place Require Date."></asp:TextBox>
                                                    <asp:Image ID="imgVehiclePlaceDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                </td>
                                            </tr>
                                            <tr runat="server">
                                            <td runat="server">
                                            </td>
                                            <td runat="server"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Pincode&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; City&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; State
                                            </td>
                                            <td runat="server"></td>
                                            <td runat="server">
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Pincode&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; City&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; State
                                            </td>
                                          </tr>
                                        <tr runat="server">
                                          <td runat="server">Enter PickUpPincode</td>
                                           <td runat="server">
                                           <asp:TextBox ID="txtpickPincode" runat="server" AutoPostBack="True" CssClass="SearchTextbox" ValidationGroup="Required" placeholder="Search" TabIndex="3" ToolTip="Enter Pincode" Width="100px" OnTextChanged="txtpickPincode_TextChanged" ></asp:TextBox>
                                           <asp:HiddenField ID="hdnPincodeId" runat="server" Value="0" />&nbsp;
                                            &nbsp;&nbsp;<asp:TextBox ID="txtpickCity" runat="server" Enabled="False" Width="100px"></asp:TextBox>
                                            &nbsp;<asp:TextBox ID="txtpickState" runat="server" Enabled="False" Width="100px"></asp:TextBox>
                      
                                           </td>
                                           <td runat="server">Enter DropPincode</td>
                                           <td runat="server">
                                             <asp:TextBox ID="txtdropPincode" runat="server" AutoPostBack="True" CssClass="SearchTextbox" ValidationGroup="Required" placeholder="Search" TabIndex="3" ToolTip="Enter Pincode" Width="100px" OnTextChanged="txtdropPincode_TextChanged" ></asp:TextBox>
                                                <asp:HiddenField ID="hdnpinid" runat="server" Value="0" />&nbsp;
                                                  &nbsp;&nbsp;<asp:TextBox ID="txtdropCity" runat="server" Enabled="False" Width="100px"></asp:TextBox>
                                               &nbsp;<asp:TextBox ID="txtdropState" runat="server" Enabled="False" Width="100px"></asp:TextBox>                    
                                        </td>
                                      </tr>
                                            <tr>
                                                <td>Remark</td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtRemark1" runat="server" TextMode="MultiLine" Rows="3" Width="900px" TabIndex="6"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                            <td runat="server">
                                            <asp:Label ID="lblEmpty_Letter" runat="server" Text="Empty Letter"  ValidationGroup="Required"  Visible="False"></asp:Label>
                                            <asp:RequiredFieldValidator ID="rfvLoadedDocuments" runat="server" ControlToValidate="loadedDocuments" SetFocusOnError="True"
                                            Text="*" ErrorMessage="Please select a document to upload"></asp:RequiredFieldValidator>
                                            </td>    
                                            <td runat="server">                                 
                                            <div class="file-upload">                                  
                                            <label for="FileUpload1" class="file-upload-label"> </label>                               
                                            <asp:FileUpload ID="loadedDocuments" runat="server" CssClass="file-upload-input" Visible="False"/> 
                                            </div>
                                            </td>
                                        </tr>
                                             <tr>
                                                    <td>
                                                        Packing List
                                                    </td>
                                                    <td colspan="3"><asp:FileUpload ID="FileUpload1" runat="server" />
                                                        <asp:Button ID="btnSaveDocument" Text="Save Document" runat="server" OnClick="btnSaveDocument_Click" />
                                                    </td>
                                                </tr>
                                        </table>
                                        </div>
                                        <br />
                                        <br />
                                        <div>
                                            <asp:Repeater ID="rptDocument" runat="server" OnItemCommand="rptDocument_ItemCommand">
                                                <HeaderTemplate>
                                                    <table class="table" border="0" cellpadding="0" cellspacing="0" style="width: 50%">
                                                        <tr bgcolor="#FF781E">
                                                            <th>Sl
                                                            </th>
                                                            <th>Document Name
                                                            </th>
                                                            <th>Action
                                                            </th>
                                                        </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <%#Container.ItemIndex +1%>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="LinkButton4" Text='<%#DataBinder.Eval(Container.DataItem,"DocumentName") %>'
                                                                CommandArgument='<%# Eval("DocPath") %>' CausesValidation="false" runat="server"
                                                                Width="200px" CommandName="DownloadFile"></asp:LinkButton>
                                                            &nbsp;
                                                    <asp:HiddenField ID="hdnDocLid" Value='<%#DataBinder.Eval(Container.DataItem,"PkId") %>'
                                                        runat="server"></asp:HiddenField>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="LinkButton5" CommandName="Delete" ToolTip="Delete" Width="39" CausesValidation="false"
                                                                runat="server" Text="Delete" Font-Underline="true" OnClientClick="return confirm('Are you sure you want to remove this document?')"></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </div>
                                </fieldset>
                            </fieldset>
                        
                        
                            <fieldset>
                                <legend>Consolidate Job Detail</legend>
                                <asp:GridView ID="gvTransportJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="TransReqId" PagerStyle-CssClass="pgr"
                                    AllowPaging="True" AllowSorting="True" Width="100%" PageSize="20" PagerSettings-Position="TopAndBottom" DataSourceID="DataSourceTransportJobDetail">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex +1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TRRefNo" HeaderText="TR Ref No" />
                                        <asp:BoundField DataField="JobRefNo" HeaderText="Job Ref No" />
                                        <asp:BoundField DataField="CustName" HeaderText="Customer" />
                                        <asp:BoundField DataField="VehiclePlaceDate" HeaderText="Vehicle Place Date" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="LocationFrom" HeaderText="From" />
                                        <asp:BoundField DataField="Destination" HeaderText="Destination" />
                                        <asp:BoundField DataField="NoOfPkgs" HeaderText="No Of Pkgs" />
                                        <asp:BoundField DataField="GrossWeight" HeaderText="Gross Weight" />
                                        <asp:BoundField DataField="Count20" HeaderText="Cont 20" />
                                        <asp:BoundField DataField="Count40" HeaderText="Cont 40" />
                                        <asp:BoundField DataField="DeliveryType" HeaderText="Delivery Type" />
                                        <asp:BoundField DataField="PlanningDate" HeaderText="Planning Date" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="LRNo" HeaderText="LR No" />
                                        <asp:BoundField DataField="LRDate" HeaderText="LR Date" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="BabajiChallanNo" HeaderText="Challan No" />
                                        <asp:BoundField DataField="BabajiChallanDate" HeaderText="Challan Date" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="RequestedBy" HeaderText="Requested By" />
                                        <asp:BoundField DataField="RequestedDate" HeaderText="Request Date" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="UnloadingDate" HeaderText="Unloading Date" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="ReportingDate" HeaderText="Reporting Date" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="ContReturnDate" HeaderText="Cont Return Date" DataFormatString="{0:dd/MM/yyyy}" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>
                        
                        
                            <fieldset>
                                <legend>Vehicle Rate Detail</legend>
                                <asp:GridView ID="GridViewVehicle" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                                    DataSourceID="DataSourceRate" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false"
                                    PageSize="20" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TransporterName" HeaderText="Transporter" SortExpression="TransporterName" ReadOnly="true" />
                                        <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" SortExpression="VehicleNo" ReadOnly="true" />
                                        <asp:BoundField DataField="VehicleTypeName" HeaderText="Vehicle Type" SortExpression="VehicleTypeName" ReadOnly="true" />
                                        <asp:BoundField DataField="LRNo" HeaderText="LRNo" SortExpression="LRNo" ReadOnly="true" />
                                        <asp:BoundField DataField="LRDate" HeaderText="LRDate" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LRDate" ReadOnly="true" />
                                        <asp:BoundField DataField="ChallanNo" HeaderText="ChallanNo" SortExpression="ChallanNo" ReadOnly="true" />
                                        <asp:BoundField DataField="ChallanDate" HeaderText="Challan Date" SortExpression="ChallanDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="Rate" HeaderText="Freight Rate" SortExpression="Rate" ReadOnly="true" ItemStyle-HorizontalAlign="Right" Visible="false" />
                                        <asp:BoundField DataField="Advance" HeaderText="Advance (%)" SortExpression="Advance" ReadOnly="true" ItemStyle-HorizontalAlign="Right" Visible="false" />
                                        <asp:BoundField DataField="AdvanceAmount" HeaderText="AdvanceAmount" SortExpression="AdvanceAmount" ReadOnly="true" ItemStyle-HorizontalAlign="Right" Visible="false" />
                                        <asp:BoundField DataField="MarketBillingRate" HeaderText="Market Rate" SortExpression="MarketBillingRate" ReadOnly="true" ItemStyle-HorizontalAlign="Right" Visible="false" />
                                        <asp:BoundField DataField="FreightAmount" HeaderText="Freight Amt" SortExpression="FreightAmount" ReadOnly="true" Visible="false" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="DetentionAmount" HeaderText="Detention Amt" SortExpression="DetentionAmount" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="VaraiExpense" HeaderText="Varai Exp" SortExpression="VaraiExpense" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="EmptyContRecptCharges" HeaderText="Empty Cont Charges" SortExpression="EmptyContRecptCharges" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy" ReadOnly="true" />
                                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" SortExpression="CreatedDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="UpdatedBy" HeaderText="Updated By" SortExpression="UpdatedBy" ReadOnly="true" />
                                        <asp:BoundField DataField="UpdatedDate" HeaderText="Updated Date" SortExpression="UpdatedDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>
                        
                        
                
                            <asp:SqlDataSource ID="DataSourceTransportJobDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="TR_GetConsolidateJobDetail" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <%--<asp:ControlParameter ControlID="hdnConsolidateId" PropertyName="Value" Name="ConsolidateID" />--%>
                                    <asp:ControlParameter ControlID="hdnTransReqId" PropertyName="Value" Name="TransReqId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:SqlDataSource ID="DataSourceTruckRequest" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="TR_GetTruckRequestByJobId" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <%--<asp:ControlParameter ControlID="hdnTransReqId" PropertyName="Value" Name="TransportId" />--%>
                                    <asp:SessionParameter Name="JobId" SessionField="EnqId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:SqlDataSource ID="DataSourceRate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="TR_GetTransRateDetail" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="hdnTransReqId" PropertyName="Value" Name="TransReqId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </Content>
                </AjaxToolkit:AccordionPane>
                    <ajaxToolkit:AccordionPane ID="accExpense" runat="server" Visible="false">
                        <Header>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Vendor Expense</Header>
                        <Content>
                            <fieldset id="fldFundRequest" runat="server" Visible="false">
                                <legend>Allow Vendor Payment for Billed Job</legend>
                                <span>Allow Vendor Payment ?</span>
                                <asp:RadioButtonList ID="rblFundRequest" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                                <br />
                                <asp:Button ID="btnAllowFundRequest" runat="server" OnClick="btnAllowFundRequest_Click" CssClass="btn"
                                            Text="Save Vendor Payment Status" />
                            </fieldset>
                        </Content>
                    </ajaxToolkit:AccordionPane>
                </Panes>
            </AjaxToolkit:Accordion>

                  
             
            <div id="divSqlDataSource">
                <asp:SqlDataSource ID="DataSourceBillingScrutiny" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="FR_GetBillingScrutinyById" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <%--<asp:SessionParameter Name="JobId" SessionField="JobId" />--%>
                        <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="DataSourceDraftinvoice" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="FR_GetDraftInvoiceById" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="DataSourceDraftCheck" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="FR_GetDraftCheckById" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="DataSourceFinalTyping" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="FR_GetFinalTypingById" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="DataSourceFinalCheck" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="FR_GetFinalCheckById" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="DataSourceBillDispatch" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="FR_GetBillDispatchById" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="DataSourceBillRejection" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="FR_GetBillRejectionById" SelectCommandType="StoredProcedure">
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
                        <asp:QueryStringParameter Name="JobType" DbType="String" DefaultValue='Export' />
                        <asp:SessionParameter Name="JobMode" SessionField="JobMode" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
            <div>
                <asp:SqlDataSource ID="DataSourceDelivery" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetDeliveryDetail" SelectCommandType="StoredProcedure" UpdateCommand="updJobDeliveryDetail"
                                    UpdateCommandType="StoredProcedure" OnUpdated="DataSourceDelivery_Updated">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                        <asp:Parameter Name="TransitType" DefaultValue="1" />

                                    </SelectParameters>
                                    <UpdateParameters>
                                        <asp:Parameter Name="lid" Type="int32" />
                                        <asp:Parameter Name="DeliveryDate" DbType="datetime" />
                                        <asp:Parameter Name="EmptyContRetrunDate" DbType="datetime" />
                                        <asp:Parameter Name="PODAttachment" DbType="String" />
                                        <asp:Parameter Name="CargoReceivedby" DbType="string" />
                                        <asp:Parameter Name="NFormNo" DbType="string" />
                                        <asp:Parameter Name="NFormDate" DbType="datetime" />
                                        <asp:Parameter Name="NClosingDate" DbType="datetime" />
                                        <asp:Parameter Name="SFormNo" DbType="string" />
                                        <asp:Parameter Name="SFormDate" DbType="datetime" />
                                        <asp:Parameter Name="SClosingDate" DbType="datetime" />
                                        <asp:Parameter Name="OctroiAmount" DbType="decimal" />
                                        <asp:Parameter Name="OctroiReceiptNo" DbType="String" />
                                        <asp:Parameter Name="OctroiPaidDate" DbType="datetime" />
                                        <asp:Parameter Name="RoadPermitNo" DbType="String" />
                                        <asp:Parameter Name="RoadPermitDate" DbType="datetime" />
                                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                        <asp:Parameter Name="OutPut" Type="int32" Direction="Output" />
                                    </UpdateParameters>
                                </asp:SqlDataSource>
                                <asp:SqlDataSource ID="DataSourceWarehouse" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetDeliveryDetail" SelectCommandType="StoredProcedure"
                                    UpdateCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                        <asp:Parameter Name="TransitType" DefaultValue="2" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

