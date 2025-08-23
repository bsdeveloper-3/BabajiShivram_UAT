<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="SBPreparationDetail.aspx.cs" Inherits="ExportCHA_SBPreparationDetail"
    Title="Export Tracking" MaintainScrollPositionOnPostback="true" Culture="en-GB"
    EnableSessionState="True" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upJobDetail" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
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

        function CheckBoxValidation(oSrouce, args) {
            var myCheckBox = document.getElementById('<%= chkYes.ClientID %>');
            if (myCheckBox.checked) {

                args.IsValid = true;
            }
            else {
                args.IsValid = false;
            }


        }
    </script>

    <style type="text/css">
        .accordionHeader, .accordionHeaderSelected {
            background-position-x: 4px;
        }

        .accordionHeader {
            width: 50%;
        }
    </style>

    <asp:UpdatePanel ID="upJobDetail" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="vsEditContainer" runat="server" ShowMessageBox="True"
                ShowSummary="False" ValidationGroup="valContainer" CssClass="errorMsg" EnableViewState="false" />
            <asp:ValidationSummary ID="vsEditInvoice" runat="server" ShowMessageBox="True" ShowSummary="False"
                ValidationGroup="vgEditInvoice" CssClass="errorMsg" EnableViewState="false" />
            <asp:ValidationSummary ID="vsAddInvoice" runat="server" ShowMessageBox="True" ShowSummary="False"
                ValidationGroup="vgAddInvoice" CssClass="errorMsg" EnableViewState="false" />
            <asp:ValidationSummary ID="vsChecklist" runat="server" ShowMessageBox="True" ShowSummary="False"
                ValidationGroup="vgChecklist" CssClass="errorMsg" EnableViewState="false" />
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div align="center">
                <asp:Label ID="lblresult" runat="server" EnableViewState="false"></asp:Label>
            </div>

            <fieldset>
                <legend>Pending SB Preparation</legend>
                <cc1:Accordion ID="Accordion1" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" Width="95%"
                    ContentCssClass="accordionContent" runat="server" SelectedIndex="0" FadeTransitions="true"
                    SuppressHeaderPostbacks="true" TransitionDuration="250" FramesPerSecond="40"
                    RequireOpenedPane="false" AutoSize="None">
                    <Panes>
                        <cc1:AccordionPane ID="accJobDetail" runat="server">
                            <Header>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Job Detail
                        </Header>
                            <Content>
                                <asp:FormView ID="FVJobDetail" HeaderStyle-Font-Bold="true" runat="server" DataKeyNames="lid"
                                    Width="100%">
                                    <ItemTemplate>
                                        <div class="m clear">
                                            <asp:Button ID="btnBackButton" runat="server" UseSubmitBehavior="false" Text="Back"
                                                OnClick="btnBackButton_Click" CommandArgument="SBPreparation.aspx" CausesValidation="false" />
                                        </div>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                            <tr>
                                                <td>BS Job No.
                                                </td>
                                                <td>
                                                    <%# Eval("JobRefNo") %>
                                                </td>
                                                <td>Cust Ref No.
                                                </td>
                                                <td>
                                                    <%# Eval("CustRefNo") %>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Customer
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
                                                <td>Consignee
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
                                                <td>Buyer Name
                                                </td>
                                                <td>
                                                    <%# Eval("BuyerName")%>
                                                </td>
                                                <td>Type of Export
                                                </td>
                                                <td>
                                                    <%# Eval("ExportType")%>
                                                </td>
                                                
                                            </tr>
                                            <tr>
                                                <td>Product Desc
                                                </td>
                                                <td colspan="3">
                                                    <%# Eval("ProductDesc")%>
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
                                                <td>
                                                    Destination Country
                                                </td>
                                                <td>
                                                    <%#Eval("DestinationCountry") %>
                                                </td>
                                                
                                            </tr>
                                            <tr>
                                                <td>Package Type
                                                </td>
                                                <td>
                                                    <%# Eval("PackageType")%>
                                                </td>
                                                <td>
                                                    No Of Packages
                                                </td>
                                                <td>
                                                    <%# Eval("NoOfPackages")%>
                                                </td>
                                                
                                            </tr>
                                            <tr>
                                                <td>Shipping Bill Type
                                                </td>
                                                <td>
                                                    <%# Eval("ShippingBillType")%>
                                                </td>
                                                <td>Forwarder Name
                                                </td>
                                                <td>
                                                    <%# Eval("ForwarderName")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Gross Weight (Kgs)
                                                </td>
                                                <td>
                                                    <%# Eval("GrossWT")%>
                                                </td>
                                                <td>Container Loaded
                                                </td>
                                                <td>
                                                    <%# Eval("ContainerLoaded")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Net Weight (Kgs)
                                                </td>
                                                <td>
                                                    <%# Eval("NetWT")%>
                                                </td>
                                                <td>Transport By
                                                </td>
                                                <td>
                                                    <%# Eval("TransportBy")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:FormView>
                            </Content>
                        </cc1:AccordionPane>
                       
                        <cc1:AccordionPane ID="accInvceDetail" runat="server">
                            <Header>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Invoice Detail
                        </Header>
                            <Content>
                                <fieldset>
                                    <legend>Add Invoice</legend>
                                    <div style="padding-bottom: 5px">
                                        <asp:Button ID="btnAddInvoice" Text="Add Invoice" OnClick="btnAddInvoice_Click" ValidationGroup="vgAddInvoice"
                                            runat="server" />
                                        <asp:Button ID="btnCancelInvoice" Text="Cancel" OnClick="btnCancelInvoice_Click"
                                            CausesValidation="false" runat="server" />
                                    </div>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                        <tr>
                                            <td>Invoice No
                                        <asp:RequiredFieldValidator ID="rfvInvcNo" runat="server" ControlToValidate="txtInvoiceNo"
                                            SetFocusOnError="true" Display="Dynamic" ForeColor="Red" ErrorMessage="Please Enter Invoice No."
                                            Text="*" ValidationGroup="vgAddInvoice"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtInvoiceNo" runat="server" MaxLength="50" TabIndex="1"></asp:TextBox>
                                            </td>
                                            <td>Invoice Date
                                        <cc1:MaskedEditExtender ID="meeIncDate_footer" TargetControlID="txtInvoiceDate" Mask="99/99/9999"
                                            MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                                        </cc1:MaskedEditExtender>
                                                <cc1:MaskedEditValidator ID="mevInvcDate_footer" ControlExtender="meeIncDate_footer"
                                                    ControlToValidate="txtInvoiceDate" IsValidEmpty="false" InvalidValueBlurredMessage="Invalid Date"
                                                    InvalidValueMessage="License Date is invalid" EmptyValueBlurredText="*" EmptyValueMessage="Please Enter Invoice Date."
                                                    MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/1900"
                                                    MaximumValue="01/01/2025" SetFocusOnError="true" runat="Server" ValidationGroup="vgAddInvoice"></cc1:MaskedEditValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtInvoiceDate" runat="server" MaxLength="50" Width="65px" TabIndex="2"
                                                    placeholder="dd/mm/yyyy"></asp:TextBox>
                                                <asp:Image ID="imgCalInvDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                                                    runat="server" />
                                                <cc1:CalendarExtender ID="calInvcDate" runat="server" Enabled="true" EnableViewState="false"
                                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtInvoiceDate"
                                                    PopupButtonID="imgCalInvDate" PopupPosition="BottomRight">
                                                </cc1:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Invoice Value
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtInvoiceValue" runat="server" MaxLength="200" TabIndex="3"></asp:TextBox>
                                            </td>
                                            <td>Invoice Currency
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtInvoiceCurrency" runat="server" MaxLength="200" TabIndex="4"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Shipment Term
                                        <asp:RequiredFieldValidator ID="rfvshipmentTerm" runat="server" ControlToValidate="ddlShipmentTerm"
                                            SetFocusOnError="true" Display="Dynamic" ForeColor="Red" ErrorMessage="Please Select Shipment Term."
                                            InitialValue="0" Text="*" ValidationGroup="vgAddInvoice"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlShipmentTerm" runat="server" TabIndex="5" DataSourceID="DataSourceShipmentTerm" Width="40%"
                                                    DataTextField="sName" DataValueField="lid">
                                                </asp:DropDownList>
                                            </td>
                                            <td>DBK Amount
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDBKAmount" runat="server" MaxLength="200" TabIndex="6"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>License No
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLicenseNo" runat="server" MaxLength="200" TabIndex="7"></asp:TextBox>
                                            </td>
                                            <td>License Date
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLicenseDate" runat="server" Width="65px" TabIndex="8" placeholder="dd/mm/yyyy"></asp:TextBox>
                                                <asp:Image ID="imgLicenseDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                                                    runat="server" />
                                                <cc1:CalendarExtender ID="calLicenseDate_footer" runat="server" Enabled="true" EnableViewState="false"
                                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtLicenseDate"
                                                    PopupButtonID="imgLicenseDate" PopupPosition="BottomRight">
                                                </cc1:CalendarExtender>
                                                <cc1:MaskedEditExtender ID="meeLicenseDate_footer" TargetControlID="txtLicenseDate"
                                                    Mask="99/99/9999" MessageValidatorTip="true" MaskType="Date" AutoComplete="false"
                                                    runat="server">
                                                </cc1:MaskedEditExtender>
                                                <cc1:MaskedEditValidator ID="mevLicenseDate_footer" ControlExtender="meeLicenseDate_footer"
                                                    ControlToValidate="txtLicenseDate" IsValidEmpty="true" MinimumValueMessage="Invalid Date"
                                                    MaximumValueMessage="Invalid Date" MinimumValue="01/01/1900" MaximumValue="01/01/2025"
                                                    SetFocusOnError="true" runat="Server" EmptyValueBlurredText="*" ValidationGroup="vgAddInvoice"></cc1:MaskedEditValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Freight Amount
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFreightAmount" runat="server" MaxLength="200" TabIndex="9"></asp:TextBox>
                                            </td>

                                            <td>Insurance Amount
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtInsuranceAmount" runat="server" MaxLength="200" TabIndex="10"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                                <fieldset>
                                    <legend>Job Invoice Detail</legend>
                                    <asp:GridView ID="gvInvoiceDetail" runat="server" CssClass="table" PagerStyle-CssClass="pgr"
                                        AllowSorting="true" AutoGenerateColumns="false" AllowPaging="true" DataSourceID="DataSourceInvoice"
                                        OnPageIndexChanging="gvInvoiceDetail_PageIndexChanging" PageSize="20" PagerSettings-Position="TopAndBottom"
                                        DataKeyNames="JobId,lid" OnRowUpdating="gvInvoiceDetail_RowUpdating" OnRowDeleting="gvInvoiceDetail_RowDeleting"
                                        OnRowEditing="gvInvoiceDetail_RowEditing" Width="100%" OnRowCancelingEdit="gvInvoiceDetail_RowCancelingEdit">
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
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtInvoiceNo" runat="server" Text='<%#Eval("InvoiceNo") %>' TabIndex="11"
                                                        Width="100px" MaxLength="50"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvInvcNo" runat="server" ControlToValidate="txtInvoiceNo"
                                                        SetFocusOnError="true" Display="Dynamic" ForeColor="Red" ErrorMessage="Please enter invoice no."
                                                        Text="*" ValidationGroup="vgEditInvoice"></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Invoice Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblInvoiceDate" runat="server" Text='<%#Eval("InvoiceDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtInvoiceDate" runat="server" Width="70px" TabIndex="12" Text='<%#Eval("InvoiceDate","{0:dd/MM/yyyy}") %>'
                                                        placeholder="dd/mm/yyyy"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="calInvcDate" runat="server" Enabled="true" EnableViewState="false"
                                                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtInvoiceDate">
                                                    </cc1:CalendarExtender>
                                                    <cc1:MaskedEditExtender ID="meeIncDate" TargetControlID="txtInvoiceDate" Mask="99/99/9999"
                                                        MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                                                    </cc1:MaskedEditExtender>
                                                    <cc1:MaskedEditValidator ID="mevInvcDate" ControlExtender="meeIncDate" ControlToValidate="txtInvoiceDate"
                                                        IsValidEmpty="false" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="License Date is invalid"
                                                        EmptyValueBlurredText="*" EmptyValueMessage="Please enter Invoice Date." MinimumValueMessage="Invalid Date"
                                                        MaximumValueMessage="Invalid Date" MinimumValue="01/01/1900" MaximumValue="01/01/2025"
                                                        SetFocusOnError="true" runat="Server" ValidationGroup="vgEditInvoice"></cc1:MaskedEditValidator>
                                                    <asp:RequiredFieldValidator ID="rfvInvcDate" runat="server" ControlToValidate="txtInvoiceDate"
                                                        SetFocusOnError="true" Display="Dynamic" ForeColor="Red" ErrorMessage="Please enter invoice date."
                                                        Text="*" ValidationGroup="vgEditInvoice"></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Invoice Value">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblInvoiceValue" runat="server" Text='<%#Eval("InvoiceValue") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtInvoiceValue" runat="server" TabIndex="13" Text='<%#Eval("InvoiceValue") %>'
                                                        Width="70px" MaxLength="200"></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Invoice Currency">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblInvoiceCurrency" runat="server" Text='<%#Eval("InvoiceCurrency") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtInvoiceCurrency" runat="server" TabIndex="13" Text='<%#Eval("InvoiceCurrency") %>'
                                                        Width="90px" MaxLength="200"></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Shipment Term">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblShipmentTerm" runat="server" Text='<%#Eval("ShipmentTerm") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList ID="ddlShipmentTerm" runat="server" TabIndex="14" DataSourceID="DataSourceShipmentTerm"
                                                        DataTextField="sName" DataValueField="lid" SelectedValue='<%#Eval("ShipmentTermId") %>'
                                                        Width="90px">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvshipmentTerm" runat="server" ControlToValidate="ddlShipmentTerm"
                                                        SetFocusOnError="true" Display="Dynamic" ForeColor="Red" ErrorMessage="Please Select Shipment Term."
                                                        InitialValue="0" Text="*" ValidationGroup="vgEditInvoice"></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="DBK Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDBKAmount" runat="server" Text='<%#Eval("DBKAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtDBKAmount" runat="server" TabIndex="15" Text='<%#Eval("DBKAmount") %>'
                                                        Width="90px" MaxLength="200"></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="License No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLicenseNo" runat="server" Text='<%#Eval("LicenseNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtLicenseNo" runat="server" TabIndex="16" Text='<%#Eval("LicenseNo") %>'
                                                        Width="90px" MaxLength="200"></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="License Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLicenseDate" runat="server" Text='<%#Eval("LicenseDate","{0:dd/MM/yyyy}") %>'
                                                        placeholder="dd/mm/yyyy"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtLicenseDate" runat="server" Width="65px" Text='<%#Eval("LicenseDate","{0:dd/MM/yyyy}") %>'
                                                        TabIndex="17" placeholder="dd/mm/yyyy"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="calLicenseDate" runat="server" Enabled="true" EnableViewState="false"
                                                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtLicenseDate">
                                                    </cc1:CalendarExtender>
                                                    <cc1:MaskedEditExtender ID="meeLicenseDate" TargetControlID="txtLicenseDate" Mask="99/99/9999"
                                                        MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                                                    </cc1:MaskedEditExtender>
                                                    <cc1:MaskedEditValidator ID="mevLicenseDate" ControlExtender="meeLicenseDate" ControlToValidate="txtLicenseDate"
                                                        IsValidEmpty="true" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="License Date is invalid"
                                                        MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/1900"
                                                        MaximumValue="01/01/2025" SetFocusOnError="true" runat="Server" EmptyValueBlurredText="*"
                                                        ValidationGroup="vgEditInvoice"></cc1:MaskedEditValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Freight Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFreightAmount" runat="server" Text='<%#Eval("FreightAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtFreightAmount" runat="server" Text='<%#Eval("FreightAmount") %>'
                                                        Width="90px" TabIndex="18" MaxLength="200"></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Insurance Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblInsuranceAmount" runat="server" Text='<%#Eval("InsuranceAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtInsuranceAmount" runat="server" Text='<%#Eval("InsuranceAmount") %>'
                                                        Width="90px" MaxLength="200" TabIndex="19"></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Edit/Delete">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server"
                                                        TabIndex="20" Text="Edit" Font-Underline="true" ValidationGroup="vgEditInvoice"></asp:LinkButton>
                                                    &nbsp;<asp:LinkButton ID="lnkDelete" CommandName="Delete" ToolTip="Delete" Width="22"
                                                        TabIndex="21" OnClientClick="return confirm('Are you sure to delete this invoice detail?');"
                                                        runat="server" Text="Delete" Font-Underline="true"></asp:LinkButton>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" Width="40px" runat="server" TabIndex="20"
                                                        Text="Update" Font-Underline="true"></asp:LinkButton>
                                                    &nbsp;<asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" Width="40px" TabIndex="21"
                                                        runat="server" Text="Cancel" Font-Underline="true"></asp:LinkButton>
                                                </EditItemTemplate>
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
                                    </div>
                                </fieldset>
                            </Content>
                        </cc1:AccordionPane>
                         <cc1:AccordionPane ID="accChecklistUpload" runat="server">
                            <Header>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Checklist Copy Upload
                        </Header>
                            <Content>
                                <asp:HiddenField ID="hdnCheckListPath" runat="server" />
                                <div class="m clear">
                                    <asp:Button ID="btnAddChecklistCopy" runat="server" CssClass="btn" Text="Save"
                                        TabIndex="37" OnClick="btnAddChecklistCopy_OnClick" ValidationGroup="vgChecklist" />
                                    <asp:Button ID="btnCancelChecklist" runat="server" Text="Cancel" TabIndex="38" OnClick="btnCancelChecklist_Click"
                                        CausesValidation="false" />
                                </div>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                    <tr>
                                        <td>Checklist Prepared <span style="color: Red;">*</span>
                                            <asp:CustomValidator ID="CVAlCheckBox" runat="server" Text="" ErrorMessage="Please Confirm if Checklist is Prepared!"
                                                TabIndex="1" ClientValidationFunction="CheckBoxValidation" ValidationGroup="vgChecklist"
                                                Display="None"></asp:CustomValidator>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYes" runat="server" Text="Yes" />
                                        </td>
                                        <td>Customer Approval Required
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkClientApproval" runat="server" Text="Yes" TabIndex="31" Enabled="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>FOB Value 
                                        <asp:RequiredFieldValidator ID="rfvFOBValue" runat="server" ControlToValidate="txtFOBValue"
                                            ValidationGroup="vgChecklist" SetFocusOnError="True" ErrorMessage="Enter FOB Value."
                                            ForeColor="Red" Text="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cvFOBValue" runat="server" ControlToValidate="txtFOBValue"
                                                Display="Dynamic" SetFocusOnError="true" Type="Double" Operator="DataTypeCheck"
                                                ErrorMessage="Invalid FOB Value." ValidationGroup="vgChecklist"></asp:CompareValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFOBValue" runat="server" TabIndex="32" MaxLength="15"></asp:TextBox>
                                        </td>
                                        <td>CIF Value 
                                        <asp:RequiredFieldValidator ID="rfvCIFValue" runat="server" ControlToValidate="txtCIFValue"
                                            ValidationGroup="vgChecklist" SetFocusOnError="True" ErrorMessage="Enter CIF Value."
                                            ForeColor="Red" Text="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cvCIFValue" runat="server" ControlToValidate="txtCIFValue"
                                                Display="Dynamic" SetFocusOnError="true" Type="Double" Operator="DataTypeCheck"
                                                ErrorMessage="Invalid CIF Value." ValidationGroup="vgChecklist"></asp:CompareValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCIFValue" runat="server" TabIndex="33" MaxLength="15"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Upload Checklist
                                        <asp:RequiredFieldValidator ID="RFVChecklist" runat="server" ControlToValidate="fuChecklist"
                                            ErrorMessage="Please Upload Checklist!" Text="*" ValidationGroup="vgChecklist" SetFocusOnError="true">
                                        </asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:FileUpload ID="fuChecklist" runat="server" TabIndex="34" />
                                            <asp:HiddenField ID="HiddenField1" runat="server" />
                                        </td>
                                        <td>Download Checklist
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnkCheckListDoc" runat="server"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Remark
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtRemark" MaxLength="200" TextMode="MultiLine" runat="server" TabIndex="35"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </Content>
                        </cc1:AccordionPane>
                    </Panes>
                </cc1:Accordion>
            </fieldset>
            <asp:SqlDataSource ID="DataSourceShipmentTerm" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="EX_GetShipmentTerms" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<%-- <!-- Container -->
                <cc1:TabPanel ID="TabSeaContainer" runat="server" HeaderText="Container" Visible="true">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Add Container</legend>
                            <div style="padding-bottom: 5px">
                                <asp:Button ID="btnAddContainer" Text="Add Container" OnClick="btnAddContainer_Click"
                                    ValidationGroup="valContainer" runat="server" />
                                <asp:Button ID="btnCancelContainer" Text="Cancel" OnClick="btnCancelContainer_Click"
                                    CausesValidation="false" runat="server" />
                            </div>
                            <asp:Label ID="lblContainerMessage" runat="server" CssClass="errorMsg"></asp:Label>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>Container No
                                        <asp:RequiredFieldValidator ID="RFVContainer" runat="server" ControlToValidate="txtContainerNo"
                                            ValidationGroup="valContainer" SetFocusOnError="True" ErrorMessage="Enter Container No"
                                            Text="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtContainerNo" runat="server" MaxLength="11"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="REVContainer" runat="server" ControlToValidate="txtContainerNo"
                                            ValidationGroup="valContainer" SetFocusOnError="True" ErrorMessage="Enter 11 Digit Container No."
                                            Display="Dynamic" ValidationExpression="^[a-zA-Z0-9]{11}$"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>Container Type
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddContainerType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddContainerType_SelectedIndexChanged">
                                            <asp:ListItem Text="FCL" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="LCL" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Container Size
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddContainerSize" runat="server">
                                            <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="20" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="40" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="45" Value="3"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset>
                            <legend>Container Detail</legend>
                            <div>
                                <asp:GridView ID="gvContainer" runat="server" AllowPaging="True" CssClass="table"
                                    AutoGenerateColumns="False" DataKeyNames="JobId,lid" Width="100%" PageSize="40"
                                    DataSourceID="DataSourceContainer" AllowSorting="true" OnRowUpdating="gvContainer_RowUpdating">
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
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtEditContainerNo" runat="server" Text='<%#Eval("ContainerNo") %>'
                                                    MaxLength="11" Width="100px"></asp:TextBox><asp:RegularExpressionValidator ID="REVGridContainer"
                                                        runat="server" ControlToValidate="txtEditContainerNo" ValidationGroup="valGridContainer"
                                                        SetFocusOnError="true" ErrorMessage="Enter 11 Digit Container No." Display="Dynamic"
                                                        ValidationExpression="^[a-zA-Z0-9]{11}$"></asp:RegularExpressionValidator>
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
                                                    <asp:ListItem Text="45" Value="3"></asp:ListItem>
                                                </asp:DropDownList>
                                            </EditItemTemplate>
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
                                        <asp:CommandField ShowEditButton="True" ShowDeleteButton="false" ValidationGroup="valGridContainer"
                                            HeaderText="Edit" />
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
                </cc1:TabPanel>  --%>
