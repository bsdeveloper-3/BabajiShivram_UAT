<%@ Page Title="Job Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EditExpJobDetail.aspx.cs"
    Inherits="ExportCHA_EditExpJobDetail" Culture="en-GB" EnableEventValidation="false"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <script type="text/css">
        .Tab .ajax__tab_header {white-space:nowrap !important;}
    </script>

    <AjaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
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
    <script type="text/javascript">
        function OnCustomerSelected(source, eventArgs) {
            // alert(eventArgs.get_value());
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnCustId').value = results.ClientId;
        }
        $addHandler
       (
           $get('ctl00_ContentPlaceHolder1_Tabs_TabPanelJobDetail_accJobDetail_content_FVJobDetail_txtCustomer'), 'keyup',
           function () {
               $get('ctl00_ContentPlaceHolder1_hdnCustId').value = '0';
               $get('ctl00_ContentPlaceHolder1_Tabs_TabPanelJobDetail_ddlShipper').value = '';
           }
       );

        function OnShipperSelected(source, eventArgs) {
            var resultsA = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnShipperId').value = resultsA.ShipperId;
        }
        $addHandler
       (
           $get('ctl00_ContentPlaceHolder1_Tabs_TabPanelJobDetail_ddlShipper'), 'keyup',
           function () {
               $get('ctl00_ContentPlaceHolder1_hdnShipperId').value = '0';
           }
       );

        function OnPortOfDischargeSelected(source, eventArgs) {
            var resultsPOD = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnDischargePortId').value = resultsPOD.PortOfLoadingId;
        }

        function OnPortOfLoadingSelected(source, eventArgs) {
            var resultsPOL = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnLoadingPortId').value = resultsPOL.PortId;
        }

        function OnDestCountrySelected(source, eventArgs) {
            var resultsDC = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnDestCountryId').value = resultsDC.CountryId;
        }

        function OnCountrySelected(source, eventArgs) {
            var resultsC = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnCountryId').value = resultsC.CountryId;
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
                <asp:HiddenField ID="hdnPreAlertId" runat="server" />
                <asp:HiddenField ID="hdnCustId" runat="server" />
                <asp:HiddenField ID="hdnLoadingPortId" Value="0" runat="server" />
                <asp:HiddenField ID="hdnDischargePortId" Value="0" runat="server" />
                <asp:HiddenField ID="hdnCustDocFolder" runat="server" />
                <asp:HiddenField ID="hdnJobFileDir" runat="server" />
                <asp:HiddenField ID="hdnDocPath" runat="server" />
                <asp:HiddenField ID="hdnShipperId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnCountryId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnDestCountryId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnUploadPath" runat="server" />

                <asp:ValidationSummary ID="ValSummaryJobDetail" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
                <asp:ValidationSummary ID="ValSummarySbPrepare" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="vgChecklist" CssClass="errorMsg" EnableViewState="false" />
                <asp:ValidationSummary ID="ValSummaryFiling" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="ExamineRequired" CssClass="errorMsg" EnableViewState="false" />
                <asp:ValidationSummary ID="valSummaryShipment" runat="server" ShowMessageBox="true"
                    ShowSummary="false" CssClass="errorMsg" ValidationGroup="valContainer" EnableViewState="false" />
                <asp:ValidationSummary ID="valUpdateDelivery" runat="server" ShowMessageBox="true"
                    ShowSummary="false" CssClass="errorMsg" ValidationGroup="GridDeliveryRequired" EnableViewState="false" />
            </div>
            <div class="clear"></div>

            <AjaxToolkit:TabContainer runat="server" ID="Tabs" ActiveTabIndex="0" CssClass="Tab"
                Width="100%" OnClientActiveTabChanged="ActiveTabChanged12"
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
                                                        <asp:Button ID="btnEditJobDetail" runat="server" CommandName="Edit" Text="Edit" OnClick="btnEditJobDetail_OnClick" />
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
                                                            <td>Buyer Name
                                                            </td>
                                                            <td>
                                                                <%# Eval("BuyerName")%>
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
                                                            <td>ADC
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblADC" runat="server" Text='<%#GetBooleanToYesNo(Eval("IsADC")) %>'></asp:Label>
                                                            </td>
                                                            <td>Haze Cargo
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblHaze" runat="server" Text='<%#GetBooleanToYesNo(Eval("IsHaze")) %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Dimension</td>
                                                            <td>
                                                                <%# Eval("Dimension")%>
                                                            </td>
                                                            <td>Job Remark</td>
                                                            <td><%# Eval("JobRemark")%></td>
                                                        </tr>

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

                                                        <%--      <tr>
                                                            <td>Octroi Applicable
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblOctroi" runat="server" Text='<%#GetBooleanToYesNo(Eval("IsOctroi")) %>'></asp:Label>

                                                            </td>
                                                            <td>S Form Applicable
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblSFrom" runat="server" Text='<%#GetBooleanToYesNo(Eval("IsSForm")) %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>N Form Applicable
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblNForm" runat="server" Text='<%#GetBooleanToYesNo(Eval("IsNForm")) %>'></asp:Label>
                                                            </td>
                                                            <td>Road Permit Applicable
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblRoadPermit" runat="server" Text='<%#GetBooleanToYesNo(Eval("IsRoadPermit")) %>'></asp:Label>
                                                            </td>
                                                        </tr>--%>
                                                    </table>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div class="m clear">
                                                        <asp:Button ID="btnUpdateJobDetail" runat="server" CommandName="Update" OnClick="btnUpdateJob_Click"
                                                            Text="Update" ValidationGroup="RequiredJob" TabIndex="9" />
                                                        <asp:Button ID="btnUpdateCancelJob" runat="server" CausesValidation="False" CommandName="Cancel"
                                                            Text="Cancel" TabIndex="10" />
                                                    </div>
                                                    <table id="tblJobDetail" runat="server" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                                        <tr id="trJobCancel" runat="server">
                                                            <td>Job Cancel Allow</td>
                                                            <td colspan="3">
                                                                <asp:DropDownList ID="ddlCancelAllow" runat="server" OnSelectedIndexChanged="ddlCancelAllow_SelectedIndexChanged" AutoPostBack="true">
                                                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <%--<td colspan="2" id="trMsg" runat="server">
                                                                <asp:Label ID="lblMsg" runat="server">Please enter cancel Remark and select cancel Reason</asp:Label>
                                                            </td>--%>
                                                        </tr>
                                                        <tr id="trCancel" runat="server" visible="false">
                                                            <td>Cancel Reason <span id="sp" runat="server" visible="true" style="color:red">*</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlReason" runat="server"  >
                                                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="Job Cancel By Customer" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="Job Cancel by User" Value="2"></asp:ListItem>
                                                                    <%--<asp:ListItem Text="C" Value="3"></asp:ListItem>
                                                                    <asp:ListItem Text="D" Value="4"></asp:ListItem>--%>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>Cancel Remark <span id="Span1" runat="server" visible="true" style="color:red">*</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCancelRemark" runat="server" TextMode="MultiLine" Text='<%# Eval("REMARK")%>'></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>BS Job No.
                                                            </td>
                                                            <td>
                                                                <b>
                                                                    <asp:Label ID="lblJobRefNo" runat="server" Text=' <%# Bind("JobRefNo") %>'></asp:Label>
                                                                </b>
                                                            </td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Babaji Branch
                                                                <asp:RequiredFieldValidator ID="rfvBranch" runat="server" ValidationGroup="RequiredJob"
                                                                    Display="Dynamic" SetFocusOnError="true" InitialValue="0" ControlToValidate="ddlBabajiBranch"
                                                                    Text="*" ErrorMessage="Please Select Babaji Branch"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlBabajiBranch" runat="server" Width="50%" TabIndex="2" DataSourceID="DataSourceBranch"
                                                                    DataTextField="BranchName" DataValueField="BranchId" SelectedValue='<%#Eval("BabajiBranchId") %>'>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>Cust Ref No.
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCustRefNo" MaxLength="100" TabIndex="8" ToolTip="Enter Customer Reference No"
                                                                    runat="server" Width="28%" Text='<%# Bind("CustRefNo") %>'></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Customer Name
                                                                <asp:RequiredFieldValidator ID="rfvCustomerName" ValidationGroup="RequiredJob" SetFocusOnError="true"
                                                                    runat="server" Display="Dynamic" ControlToValidate="txtCustomer" ErrorMessage="Please Enter Customer Name"
                                                                    Text="*"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCustomer" Width="95%" runat="server" ToolTip="Enter Customer Name." Text='<%#Bind("Customer") %>'
                                                                    CssClass="SearchTextbox" placeholder="Search" TabIndex="3" AutoPostBack="true" OnTextChanged="txtCustomer_TextChanged"></asp:TextBox>
                                                                <div id="divwidthCust" runat="server">
                                                                </div>
                                                                <AjaxToolkit:AutoCompleteExtender ID="CustomerExtender" runat="server" TargetControlID="txtCustomer"
                                                                    CompletionListElementID="divwidthCust" ServicePath="~/WebService/CustomerAutoComplete.asmx"
                                                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCust"
                                                                    ContextKey="1234" UseContextKey="True" OnClientItemSelected="OnCustomerSelected"
                                                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                                                </AjaxToolkit:AutoCompleteExtender>
                                                            </td>
                                                            <td>Shipper Name
                                                                 <asp:RequiredFieldValidator ID="rfvShipperName" ValidationGroup="RequiredJob" runat="server"
                                                                     SetFocusOnError="true" Display="Dynamic" ControlToValidate="ddlShipper" ErrorMessage="Please Select Shipper Name"
                                                                     Text="*" InitialValue="0"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlShipper" TabIndex="4" runat="server" Width="60%" DataSourceID="DataSourceShipper"
                                                                    DataTextField="ShipperName" DataValueField="ShipperId">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Customer Division/Branch
                                                                <asp:RequiredFieldValidator ID="RFVDivision" runat="server" ValidationGroup="RequiredJob" SetFocusOnError="true"
                                                                    InitialValue="0" ControlToValidate="ddDivision" Text="*" ErrorMessage="Please Select Customer Division"> </asp:RequiredFieldValidator>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddDivision" runat="server" AutoPostBack="true" Width="99%"
                                                                    TabIndex="5" OnSelectedIndexChanged="ddDivision_SelectedIndexChanged">
                                                                    <asp:ListItem Value="0" Text="-Select-"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>Customer Plant
                                                                 <asp:RequiredFieldValidator ID="RFVPlant" runat="server" ValidationGroup="RequiredJob" SetFocusOnError="true"
                                                                     InitialValue="0" ControlToValidate="ddPlant" Text="*" ErrorMessage="Please Select Customer Plant"> </asp:RequiredFieldValidator>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddPlant" runat="server" TabIndex="6" AutoPostBack="true" Width="60%">
                                                                    <%--SelectedValue='<%#Eval("PlantId") %>'--%>
                                                                    <asp:ListItem Value="0" Text="-Select-"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Consignee Name
                                                                <asp:RequiredFieldValidator ID="rfvConsignee" ValidationGroup="RequiredJob" runat="server"
                                                                    SetFocusOnError="true" Display="Dynamic" ControlToValidate="txtConsignee" ErrorMessage="Please Enter Consignee"
                                                                    Text="*"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtConsignee" runat="server" TabIndex="7" Width="95%" ToolTip="Enter Consignee." Text='<%#Eval("ConsigneeName") %>'></asp:TextBox>
                                                                <asp:HiddenField ID="hdnConsigneeId" runat="server" Value="0" />
                                                            </td>
                                                            <td>Mode
                                                                <asp:RequiredFieldValidator ID="rfvMode" ValidationGroup="RequiredJob" runat="server"
                                                                    SetFocusOnError="true" Display="Dynamic" ControlToValidate="ddlMode" InitialValue="0"
                                                                    ErrorMessage="Please Select Mode." Text="*"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlMode" runat="server" TabIndex="9" Width="30%" AutoPostBack="true" SelectedValue='<%#Eval("TransModeId") %>'
                                                                    OnSelectedIndexChanged="ddlMode_OnSelectedIndexChanged">
                                                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="Air" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="Sea" Value="2"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:HiddenField ID="hdnMode" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Port of Loading
                                                                <asp:RequiredFieldValidator ID="rfvPortOfDischarge" ValidationGroup="RequiredJob" runat="server"
                                                                    SetFocusOnError="true" Display="Dynamic" ControlToValidate="txtPortOfLoading"
                                                                    ErrorMessage="Please Select Port of Loading" Text="*"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPortOfLoading" Width="95%" runat="server" TabIndex="13" CssClass="SearchTextbox" Text='<%#Eval("PortOfLoading") %>'
                                                                    placeholder="Search" ToolTip="Enter Port Of Loading." MaxLength="100" />
                                                                <div id="divwidthLoadingPort">
                                                                </div>
                                                                <AjaxToolkit:AutoCompleteExtender ID="AutoCompletePortLoading" runat="server" TargetControlID="txtPortOfLoading"
                                                                    CompletionListElementID="divwidthLoadingPort" ServicePath="~/WebService/PortAutoComplete.asmx"
                                                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthLoadingPort"
                                                                    ContextKey="1234" UseContextKey="True" OnClientItemSelected="OnPortOfLoadingSelected"
                                                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                                                </AjaxToolkit:AutoCompleteExtender>
                                                            </td>
                                                            <td>Port of Discharge
                                                                <asp:RequiredFieldValidator ID="RFVPortDischarge" ValidationGroup="RequiredJob" runat="server"
                                                                    SetFocusOnError="true" Display="Dynamic" ControlToValidate="txtPortOfDischarge"
                                                                    ErrorMessage="Please Select Port of Discharge" Text="*"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPortOfDischarge" Width="58%" runat="server" TabIndex="14" CssClass="SearchTextbox" Text='<%#Eval("PortOfDischarge") %>'
                                                                    placeholder="Search" ToolTip="Enter Port Of Discharge." AutoPostBack="true" MaxLength="100" />
                                                                <div id="divwidthDischargePort">
                                                                </div>
                                                                <AjaxToolkit:AutoCompleteExtender ID="DischargePortExtender" runat="server" TargetControlID="txtPortOfDischarge"
                                                                    CompletionListElementID="divwidthDischargePort" ServicePath="~/WebService/PortOfLoadingAutoComplete.asmx"
                                                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthDischargePort"
                                                                    ContextKey="1234" UseContextKey="True" OnClientItemSelected="OnPortOfDischargeSelected"
                                                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                                                </AjaxToolkit:AutoCompleteExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Consignment Country
                                                                <asp:RequiredFieldValidator ID="rfvcountryConsign" runat="server" ValidationGroup="RequiredJob"
                                                                    SetFocusOnError="true" ControlToValidate="txtCountry" Text="*" ErrorMessage="Please Select Consignment Country"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCountry" runat="server" CssClass="SearchTextbox" placeholder="Search" Text='<%#Eval("ConsignmentCountry") %>'
                                                                    Width="95%" ToolTip="Enter Consignment Country." TabIndex="15"></asp:TextBox>
                                                                <div id="divwidthCountry">
                                                                </div>
                                                                <AjaxToolkit:AutoCompleteExtender ID="CountryExtender" runat="server" TargetControlID="txtCountry"
                                                                    CompletionListElementID="divwidthCountry" ServicePath="../WebService/CountryAutoComplete.asmx"
                                                                    ServiceMethod="GetCountryCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCountry"
                                                                    ContextKey="4244" UseContextKey="True" OnClientItemSelected="OnCountrySelected"
                                                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                                                </AjaxToolkit:AutoCompleteExtender>
                                                            </td>
                                                            <td>Destination Country
                                                                <asp:RequiredFieldValidator ID="rfvDestinationCountry" runat="server" ValidationGroup="RequiredJob"
                                                                    SetFocusOnError="true" ControlToValidate="txtDestinationCountry" Text="*" ErrorMessage="Please Select Destination Country"> </asp:RequiredFieldValidator>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtDestinationCountry" runat="server" CssClass="SearchTextbox" placeholder="Search" Text='<%#Eval("DestinationCountry") %>'
                                                                    Width="58%" ToolTip="Enter Final Destination Country" TabIndex="16"></asp:TextBox>
                                                                <div id="divwidthDestCountry">
                                                                </div>
                                                                <AjaxToolkit:AutoCompleteExtender ID="DestCountryExtender" runat="server" TargetControlID="txtDestinationCountry"
                                                                    CompletionListElementID="divwidthDestCountry" ServicePath="../WebService/CountryAutoComplete.asmx"
                                                                    ServiceMethod="GetCountryCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthDestCountry"
                                                                    ContextKey="4244" UseContextKey="True" OnClientItemSelected="OnDestCountrySelected"
                                                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                                                </AjaxToolkit:AutoCompleteExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Type of Export</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlExportType" Width="50%" runat="server" DataSourceID="DataSourceExportType"
                                                                    DataTextField="sName" DataValueField="lid" TabIndex="21" ToolTip="Select Shipping Bill Type" SelectedValue='<%#Eval("ExportTypeId") %>'
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlExportType_OnSelectedIndexchanged">
                                                                    <asp:ListItem Selected="True" Text="-- Select Export Type --" Value="0"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>Shipping Bill Type
                                                                    <asp:RequiredFieldValidator ID="rfvShippingbillType" runat="server" ControlToValidate="ddlShippingBillType"
                                                                        InitialValue="0" SetFocusOnError="true" ErrorMessage="Please Select Shipping Bill Type"
                                                                        Display="Dynamic" Text="*" ValidationGroup="RequiredJob"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlShippingBillType" Width="40%" runat="server" SelectedValue='<%#Eval("ShippingBillTypeId") %>'
                                                                    TabIndex="17" ToolTip="Select Shipping Bill Type" AutoPostBack="true">
                                                                    <asp:ListItem Selected="True" Text="-- Select Bill Type --" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="W – Duty free – Commercial" Value="5"></asp:ListItem>
                                                                    <asp:ListItem Text="F – NFEI" Value="6"></asp:ListItem>
                                                                    <asp:ListItem Text="EPCG – White" Value="7"></asp:ListItem>
                                                                    <asp:ListItem Text="G – Green" Value="8"></asp:ListItem>
                                                                    <asp:ListItem Text="D – DEEC White" Value="9"></asp:ListItem>
                                                                    <asp:ListItem Text="E – DEEC Drawback" Value="10"></asp:ListItem>
                                                                    <asp:ListItem Text="Re-Export" Value="11"></asp:ListItem>
                                                                    <asp:ListItem Text="I – EPCG Drawback" Value="12"></asp:ListItem>
                                                                    <asp:ListItem Text="X – Ex-Bond" Value="13"></asp:ListItem>
                                                                    <asp:ListItem Text="Section 74" Value="14"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Priority
                                                                <asp:RequiredFieldValidator ID="rfvPriority" ValidationGroup="RequiredJob" runat="server"
                                                                    Display="Dynamic" ControlToValidate="ddlPriority" InitialValue="0" ErrorMessage="Please Select Job Priority"
                                                                    Text="*"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlPriority" runat="server" CssClass="DropDownBox" Width="50%" SelectedValue='<%#Eval("PriorityId") %>'
                                                                    ToolTip="Select Priority" TabIndex="18">
                                                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="Normal" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="High" Value="2"></asp:ListItem>
                                                                    <asp:ListItem Text="Intense" Value="3"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>Transport By
                                                                 <asp:RequiredFieldValidator ID="rfvTransportBy" ValidationGroup="RequiredJob" runat="server"
                                                                     SetFocusOnError="true" Display="dynamic" ControlToValidate="ddlTransportBy" InitialValue="0"
                                                                     ErrorMessage="Please Select Transport By" Text="*"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlTransportBy" runat="server" TabIndex="10" ToolTip="Select Transport By" Width="40%"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlTransportBy_OnSelectedIndexChanged">
                                                                    <asp:ListItem Selected="True" Text="-Select-" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="Babaji Shivram" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="Customer" Value="2"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr id="trPickUpDetails" runat="server">
                                                            <td>Pickup Location From                                     
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPickupLocation" runat="server" Width="95%" ToolTip="Enter Pickup Location From." Text='<%# Eval("PickUpFrom")%>' TabIndex="11"></asp:TextBox>
                                                            </td>
                                                            <td>To 
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtLocationTo" MaxLength="100" ToolTip="Enter Destination To." TabIndex="12" Text='<%# Eval("Destination")%>'
                                                                    runat="server" Width="58%"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr id="trPickUpPersonDetails" runat="server">
                                                            <td>Pick Up Date
                                                                 <AjaxToolkit:CalendarExtender ID="calPickupDate" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="imgPickUpDate"
                                                                     Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtPickUpDate">
                                                                 </AjaxToolkit:CalendarExtender>
                                                                <AjaxToolkit:MaskedEditExtender ID="meePickupDate" TargetControlID="txtPickUpDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                                    MaskType="Date" AutoComplete="false" runat="server">
                                                                </AjaxToolkit:MaskedEditExtender>
                                                                <AjaxToolkit:MaskedEditValidator ID="mevPickUpDate" ControlExtender="meePickupDate" ControlToValidate="txtPickUpDate" IsValidEmpty="true"
                                                                    InvalidValueMessage="Pick Up Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true"
                                                                    MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2014" MaximumValue="31/12/2025"
                                                                    runat="Server" ValidationGroup="RequiredJob"></AjaxToolkit:MaskedEditValidator>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPickUpDate" runat="server" Width="100px" placeholder="dd/mm/yyyy" TabIndex="13" Text='<%# Eval("PickupDate","{0:dd/MM/yyyy}")%>'></asp:TextBox>
                                                                <asp:Image ID="imgPickUpDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                            </td>
                                                            <td>Pick Up Person Name
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPickupPersonName" runat="server" ToolTip="Pickup Person Name." TabIndex="14" Width="40%" Text='<%# Eval("PickupPersonName")%>'></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr id="trPickUpPersonDetails2" runat="server">
                                                            <td>Mobile No
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPickupMobileNo" runat="server" TabIndex="15" Text='<%#Eval("PickupPhoneNo") %>'></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Product Description
                                                                <asp:RequiredFieldValidator ID="rfvProductDesc" ValidationGroup="RequiredJob" runat="server"
                                                                    SetFocusOnError="true" Display="Dynamic" ControlToValidate="txtProductDesc" ErrorMessage="Please Enter Product Description"
                                                                    Text="*"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtProductDesc" runat="server" Width="95%" TabIndex="19" ToolTip="Enter Product Description." Text='<%#Eval("ProductDesc") %>'></asp:TextBox>
                                                            </td>
                                                            <td>Buyer Name
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBuyerName" runat="server" TabIndex="20" Width="58%" ToolTip="Enter Buyer Name." Text='<%#Eval("BuyerName") %>'></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>No Of Packages
                                                                <asp:RequiredFieldValidator ID="rfvNoOfPackages" runat="server" ControlToValidate="txtNoOfPackages"
                                                                    SetFocusOnError="true" ErrorMessage="Please Enter No Of Packages" Display="Dynamic"
                                                                    Text="*" ValidationGroup="RequiredJob"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtNoOfPackages" runat="server" TabIndex="24" ToolTip="Enter No Of Packages."
                                                                    MaxLength="8" Width="46%" Text='<%#Eval("NoOfPackages") %>'></asp:TextBox>
                                                                <asp:CompareValidator ID="CompValPackages" runat="server" ControlToValidate="txtNoOfPackages"
                                                                    Operator="DataTypeCheck" SetFocusOnError="true" Type="Integer" ErrorMessage="Invalid Number Of Packages."
                                                                    Display="Dynamic" ValidationGroup="RequiredJob"></asp:CompareValidator>
                                                            </td>
                                                            <td>Package Type
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlPackageType" runat="server" DataSourceID="DataSourcePackageType" SelectedValue='<%#Eval("PackageTypeId") %>'
                                                                    Width="40%" TabIndex="25" ToolTip="Select Package Type" DataTextField="sName"
                                                                    DataValueField="lId">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Forward To Babaji Freight?
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlForwardedBy" runat="server" TabIndex="21" ToolTip="Select Forward To Babaji Shivram" Width="50%"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlForwardedBy_OnSelectedIndexChanged">
                                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>Forwarder Name
                                                              <asp:RequiredFieldValidator ID="rfvForwardedName" ValidationGroup="RequiredJob" runat="server"
                                                                  SetFocusOnError="true" Display="Dynamic" ControlToValidate="txtForwardedName" ErrorMessage="Please Enter Forwarded Name."
                                                                  Text="*"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td id="td_NonBabajiForwarder" runat="server">
                                                                <asp:TextBox ID="txtForwardedName" runat="server" TabIndex="22" ToolTip="Enter Forwarded Name." Text='<%#Eval("ForwarderName") %>'
                                                                    Width="58%"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Container Loaded
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlContainerLoaded" Width="50%" runat="server" TabIndex="23"
                                                                    ToolTip="Select Container Loaded">
                                                                    <asp:ListItem Selected="True" Text="-Select-" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="FCL" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="LCL" Value="2"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>Seal No
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtSealNo" MaxLength="100" Width="30%" TabIndex="26" ToolTip="Enter Seal No." Text='<%#Eval("SealNo") %>'
                                                                    runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Gross Weight (in Kgs)
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtGrossWT" runat="server" MaxLength="10" Width="46%" TabIndex="27" Text='<%#Eval("GrossWT") %>'
                                                                    ToolTip="Enter Gross Weight"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="revGrossWeight" runat="server" ControlToValidate="txtGrossWT"
                                                                    SetFocusOnError="true" ErrorMessage="Invalid Gross Weight." Display="Dynamic"
                                                                    ValidationGroup="Required" ValidationExpression="^[0-9]\d{0,13}(\.\d{1,3})?$"></asp:RegularExpressionValidator>
                                                            </td>
                                                            <td>Net Weight (in Kgs)
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtNetWT" runat="server" MaxLength="10" TabIndex="28" Width="30%" Text='<%#Eval("NetWT") %>'
                                                                    ToolTip="Enter Net Weight"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="revNetWT" runat="server" ControlToValidate="txtNetWT"
                                                                    SetFocusOnError="true" ErrorMessage="Invalid Net Weight." Display="Dynamic" ValidationGroup="RequiredJob"
                                                                    ValidationExpression="^[0-9]\d{0,13}(\.\d{1,3})?$"></asp:RegularExpressionValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>MBL/MAWBL No
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtMblNo" runat="server" MaxLength="50" Text='<%#Eval("MAWBNo") %>' Width="46%"></asp:TextBox>
                                                            </td>
                                                            <td>MBL/MAWBL Date
                                                                <AjaxToolkit:CalendarExtender ID="calMAWB" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="imgMAWBDate"
                                                                    Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtMAWBDate">
                                                                </AjaxToolkit:CalendarExtender>
                                                                <AjaxToolkit:MaskedEditExtender ID="MskExtMAWBDate" TargetControlID="txtMAWBDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                                    MaskType="Date" AutoComplete="false" runat="server">
                                                                </AjaxToolkit:MaskedEditExtender>
                                                                <AjaxToolkit:MaskedEditValidator ID="MskValMAWBDate" ControlExtender="MskExtMAWBDate" ControlToValidate="txtMAWBDate" IsValidEmpty="true"
                                                                    InvalidValueMessage="MBL/MAWBL Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true"
                                                                    MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2014" MaximumValue="31/12/2025"
                                                                    runat="Server" ValidationGroup="RequiredJob"></AjaxToolkit:MaskedEditValidator>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtMAWBDate" runat="server" Width="110px" placeholder="dd/mm/yyyy" Text='<%#Eval("MAWBDate","{0:dd/MM/yyyy}")%>'></asp:TextBox>
                                                                <asp:Image ID="imgMAWBDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>HBL/HAWBL No
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtHblNo" runat="server" MaxLength="50" Text='<%#Eval("HAWBNo") %>' Width="46%"></asp:TextBox>
                                                            </td>
                                                            <td>HBL/HAWBL Date
                                                                <AjaxToolkit:CalendarExtender ID="calHAWB" runat="server" EnableViewState="False" FirstDayOfWeek="Sunday"
                                                                    Format="dd/MM/yyyy" PopupButtonID="imgHAWBDate" PopupPosition="BottomRight" TargetControlID="txtHAWBDate">
                                                                </AjaxToolkit:CalendarExtender>
                                                                <AjaxToolkit:MaskedEditExtender ID="MskExtHAWBDate" TargetControlID="txtHAWBDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                                    MaskType="Date" AutoComplete="false" runat="server">
                                                                </AjaxToolkit:MaskedEditExtender>
                                                                <AjaxToolkit:MaskedEditValidator ID="MskValHAWBDate" ControlExtender="MskExtHAWBDate" ControlToValidate="txtHAWBDate" IsValidEmpty="true"
                                                                    InvalidValueMessage="HBL/HAWBL Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true"
                                                                    MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2014" MaximumValue="31/12/2025"
                                                                    runat="Server" ValidationGroup="RequiredJob"></AjaxToolkit:MaskedEditValidator>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtHAWBDate" runat="server" Width="110px" placeholder="dd/mm/yyyy" Text='<%#Eval("HAWBDate","{0:dd/MM/yyyy}")%>'></asp:TextBox>
                                                                <asp:Image ID="imgHAWBDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>ADC
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkADC" runat="server" Checked='<%#CheckNullBooleanToTrueFalse(Eval("IsADC")) %>' Text="Yes" />
                                                            </td>
                                                            <td>Haze Cargo
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkHaze" runat="server" Checked='<%#CheckNullBooleanToTrueFalse(Eval("IsHaze")) %>' Text="Yes"></asp:CheckBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Dimension Length
                                                                <asp:RequiredFieldValidator ID="rfvDimension" runat="server" ControlToValidate="txtDimension"
                                                                    SetFocusOnError="true" ErrorMessage="Please Enter Dimension." Display="Dynamic"
                                                                    Text="*" ValidationGroup="RequiredJob"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtDimension" runat="server" MaxLength="10" TabIndex="34" Width="37%" Text='<%#Eval("Dimension")%>'
                                                                    ToolTip="Enter Dimension."></asp:TextBox>
                                                            </td>
                                                            <td>Job Remark</td>
                                                            <td>
                                                                <asp:TextBox ID="txtJobRemark" runat="server" TabIndex="35" Width="37%" Text='<%#Eval("JobRemark")%>'
                                                                    ToolTip="Enter Job Remark." TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <%--     <tr>
                                                            <td>Octroi Applicable
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkOctroi" runat="server" Checked='<%#CheckNullBooleanToTrueFalse(Eval("IsOctroi")) %>' />
                                                            </td>
                                                            <td>S Form Applicable
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkSFrom" runat="server" Checked='<%#CheckNullBooleanToTrueFalse(Eval("IsSForm")) %>'></asp:CheckBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>N Form Applicable
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkNFrom" runat="server" Checked='<%#CheckNullBooleanToTrueFalse(Eval("IsNForm")) %>'></asp:CheckBox>
                                                            </td>
                                                            <td>Road Permit Applicable
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkRoadPermit" runat="server" Checked='<%#CheckNullBooleanToTrueFalse(Eval("IsRoadPermit")) %>'></asp:CheckBox>
                                                            </td>
                                                        </tr>--%>
                                                    </table>
                                                </EditItemTemplate>
                                            </asp:FormView>

                                            <asp:SqlDataSource ID="DataSourcePackageType" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                                                SelectCommand="GetPackageType" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                                            <asp:SqlDataSource ID="DataSourceShippingBillType" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                                                SelectCommand="EX_GetEX_ShippingBillTypeMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                                            <asp:SqlDataSource ID="DataSourceBranch" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                                                SelectCommand="EX_GetAllBranchMS" SelectCommandType="StoredProcedure">
                                                <SelectParameters>
                                                    <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                            <asp:SqlDataSource ID="DataSourceExportType" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                                                SelectCommand="EX_GetExportTypeMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                                            <asp:SqlDataSource ID="DataSourceShipper" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                                                SelectCommand="EX_GetCustWsShipperDetail" SelectCommandType="StoredProcedure">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="hdnCustId" Name="CustId" PropertyName="Value" DbType="Int32" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>

                                        </Content>
                                    </AjaxToolkit:AccordionPane>
                                    <AjaxToolkit:AccordionPane ID="accSBPrepare" runat="server">
                                        <Header>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; SB Preparation Detail</Header>
                                        <Content>
                                            <asp:FormView ID="FVSBPrepare" runat="server" Width="99%" DataKeyNames="lid" DataSourceID="DataSourceJobDetail" OnDataBound="FVSBPrepare_DataBound" OnItemCommand="FVSBPrepare_ItemCommand">
                                                <HeaderStyle Font-Bold="True" />
                                                <ItemTemplate>
                                                    <div class="m clear">
                                                        <asp:Button ID="btnEditPrepareDetail" runat="server" CommandName="Edit" Text="Edit" OnClick="btnEditPrepareDetail_OnClick" />
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
                                                <EditItemTemplate>
                                                    <div class="m clear">
                                                        <asp:Button ID="btnUpdatePrepareDetail" runat="server" CommandName="Update" OnClick="btnUpdatePrepareDetail_OnClick"
                                                            Text="Update" ValidationGroup="Required" TabIndex="9" />
                                                        <asp:Button ID="btnUpdatePrepareCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                                            Text="Cancel" TabIndex="10" />
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
                                                                <asp:LinkButton ID="lnkEditChecklistDoc_JobDetail" runat="server"
                                                                    OnClick="lnkEditChecklistDoc_JobDetail_Click"></asp:LinkButton>
                                                                <asp:HiddenField ID="hdnEditChecklistDocPath" runat="server" />
                                                                <asp:Label ID="lblDwnloadChecklst" runat="server" Text="OR"></asp:Label>
                                                                <asp:FileUpload ID="fuUploadChecklist" runat="server" />
                                                                <asp:HiddenField ID="hdnCheckListPath" runat="server" />
                                                            </td>
                                                            <td>FOB Value 
                                                                <asp:RequiredFieldValidator ID="rfvFOBValue" runat="server" ControlToValidate="txtFOBValue"
                                                                    ValidationGroup="vgChecklist" SetFocusOnError="True" ErrorMessage="Enter FOB Value."
                                                                    ForeColor="Red" Text="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                                                <asp:CompareValidator ID="cvFOBValue" runat="server" ControlToValidate="txtFOBValue"
                                                                    Display="Dynamic" SetFocusOnError="true" Type="Double" Operator="DataTypeCheck"
                                                                    ErrorMessage="Invalid FOB Value." ValidationGroup="vgChecklist"></asp:CompareValidator>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtFOBValue" runat="server" TabIndex="1" MaxLength="15" Width="30%" Text='<%#Eval("FOBValue") %>'></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>CIF Value 
                                                                <asp:RequiredFieldValidator ID="rfvCIFValue" runat="server" ControlToValidate="txtCIFValue"
                                                                    ValidationGroup="vgChecklist" SetFocusOnError="True" ErrorMessage="Enter CIF Value."
                                                                    ForeColor="Red" Text="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                                                <asp:CompareValidator ID="cvCIFValue" runat="server" ControlToValidate="txtCIFValue"
                                                                    Display="Dynamic" SetFocusOnError="true" Type="Double" Operator="DataTypeCheck"
                                                                    ErrorMessage="Invalid CIF Value." ValidationGroup="vgChecklist"></asp:CompareValidator>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCIFValue" runat="server" TabIndex="2" MaxLength="15" Width="30%" Text='<%#Eval("CIFValue") %>'></asp:TextBox>
                                                            </td>
                                                            <td>Remark
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="txtRemark" runat="server" TabIndex="3" Text='<%#Eval("ChecklistRemark") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EditItemTemplate>
                                            </asp:FormView>
                                        </Content>
                                    </AjaxToolkit:AccordionPane>
                                    <AjaxToolkit:AccordionPane ID="accSBFiling" runat="server">
                                        <Header>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; SB Filing / Custom Process Detail</Header>
                                        <Content>
                                            <asp:FormView ID="FVFilingCustomProcess" runat="server" Width="99%" DataKeyNames="lid" DataSourceID="DataSourceJobDetail" OnDataBound="FVFilingCustomProcess_DataBound" OnItemCommand="FVFilingCustomProcess_ItemCommand">
                                                <HeaderStyle Font-Bold="True" />
                                                <ItemTemplate>
                                                    <div class="m clear">
                                                        <asp:Button ID="btnEditFilingDetail" runat="server" CommandName="Edit" Text="Edit" OnClick="btnEditFilingDetail_OnClick" />
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
                                                            <td>Supretendent LEO Date
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
                                                <EditItemTemplate>
                                                    <div class="m clear">
                                                        <asp:Button ID="btnUpdateFilingDetail" runat="server" CommandName="Update" OnClick="btnUpdateFilingDetail_OnClick"
                                                            Text="Update" ValidationGroup="FilingRequired" TabIndex="9" />
                                                        <asp:Button ID="btnUpdateFilingCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                                            Text="Cancel" TabIndex="10" />
                                                    </div>
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                                        <AjaxToolkit:CalendarExtender ID="calRegisterationDate" runat="server" Enabled="True" EnableViewState="False"
                                                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgRegistrationDate"
                                                            PopupPosition="BottomRight" TargetControlID="txtRegistrationDate">
                                                        </AjaxToolkit:CalendarExtender>
                                                        <AjaxToolkit:CalendarExtender ID="calExamineDate" runat="server" Enabled="True" EnableViewState="False"
                                                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgExamineDate" PopupPosition="BottomRight"
                                                            TargetControlID="txtExamineDate">
                                                        </AjaxToolkit:CalendarExtender>
                                                        <AjaxToolkit:CalendarExtender ID="calExamineReportDate" runat="server" Enabled="True" EnableViewState="False"
                                                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgExamineReport" PopupPosition="BottomRight"
                                                            TargetControlID="txtExamineReportDate">
                                                        </AjaxToolkit:CalendarExtender>
                                                        <AjaxToolkit:CalendarExtender ID="calLEODate" runat="server" Enabled="True" EnableViewState="False"
                                                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgLEODate"
                                                            PopupPosition="BottomRight" TargetControlID="txtLEODate">
                                                        </AjaxToolkit:CalendarExtender>
                                                        <tr>
                                                            <td>BS Job No.
                                                            </td>
                                                            <td>
                                                                <b><%# Eval("JobRefNo") %></b>
                                                            </td>
                                                            <td>SB No
                                                                 <asp:RegularExpressionValidator ID="REVSBNo" runat="server" ErrorMessage="Please Enter 7 digit SB Number."
                                                                     Text="*" ValidationExpression="^[0-9]{7}$" ControlToValidate="txtSBNo" Display="Dynamic"
                                                                     ValidationGroup="FilingRequired" SetFocusOnError="true"></asp:RegularExpressionValidator>
                                                                <asp:RequiredFieldValidator ID="reqvSBNo" runat="server" Text="*" ControlToValidate="txtSBNo" SetFocusOnError="true"
                                                                    ErrorMessage="Please Enter SB No." ValidationGroup="FilingRequired" Display="Dynamic"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtSBNo" runat="server" TabIndex="1" Width="30%" MaxLength="7" Text='<%# Bind("SBNo")%>'></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>SB Date
                                                                 <AjaxToolkit:CalendarExtender ID="calSBDate" runat="server" Enabled="True"
                                                                     FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgSBDate" PopupPosition="BottomRight"
                                                                     TargetControlID="txtSBDate">
                                                                 </AjaxToolkit:CalendarExtender>
                                                                <AjaxToolkit:MaskedEditExtender ID="MskExtSBDate" TargetControlID="txtSBDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                                    MaskType="Date" AutoComplete="false" runat="server">
                                                                </AjaxToolkit:MaskedEditExtender>
                                                                <AjaxToolkit:MaskedEditValidator ID="MskValSBDate" ControlExtender="MskExtSBDate" ControlToValidate="txtSBDate" IsValidEmpty="false"
                                                                    EmptyValueMessage="Please Enter SB Date." EmptyValueBlurredText="*" InvalidValueMessage="SB Date is invalid"
                                                                    InvalidValueBlurredMessage="Invalid Date" MinimumValueMessage="Invalid SB Date" MaximumValueMessage="Invalid SB Date"
                                                                    MinimumValue="01/07/2014" MaximumValue="31/12/2025"
                                                                    runat="Server" SetFocusOnError="true" ValidationGroup="FilingRequired"></AjaxToolkit:MaskedEditValidator>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtSBDate" runat="server" Width="80px" MaxLength="10" Text='<%# Bind("SBDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                                <asp:Image ID="imgSBDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                            </td>
                                                            <td>To be Mark/Appraising
                                                                 <asp:RequiredFieldValidator ID="rfvMarkAppraising" runat="server" Text="*" ControlToValidate="ddMarkAppraising" SetFocusOnError="true"
                                                                     InitialValue="0" ErrorMessage="Please Select Marked/Appraising." ValidationGroup="FilingRequired" Display="Dynamic"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td>

                                                                <asp:DropDownList ID="ddMarkAppraising" runat="server" Width="80px" SelectedValue='<%# Eval("MarkAppraisingId") %>'>
                                                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="Marked" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="Appraising" Value="2"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Mark/Passing Date
                                                                 <AjaxToolkit:CalendarExtender ID="calMarkPassingDate" runat="server" Enabled="True" EnableViewState="False"
                                                                     FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgMarkPassingdate"
                                                                     PopupPosition="BottomRight" TargetControlID="txtMarkPassingDate">
                                                                 </AjaxToolkit:CalendarExtender>
                                                                <AjaxToolkit:MaskedEditExtender ID="MEditMarkPassingDate" TargetControlID="txtMarkPassingDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                                    MaskType="Date" AutoComplete="false" runat="server">
                                                                </AjaxToolkit:MaskedEditExtender>
                                                                <AjaxToolkit:MaskedEditValidator ID="MEditValMarkPassingDate" ControlExtender="MEditMarkPassingDate" ControlToValidate="txtMarkPassingDate" IsValidEmpty="false"
                                                                    EmptyValueBlurredText="*" InvalidValueMessage="Mark Date is invalid" Display="Dynamic"
                                                                    SetFocusOnError="true" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid"
                                                                    MinimumValue="01/01/2014" MaximumValue="31/12/2025" runat="Server" ValidationGroup="FilingRequired"></AjaxToolkit:MaskedEditValidator>
                                                            </td>
                                                            <td>

                                                                <asp:TextBox ID="txtMarkPassingDate" runat="server" Width="100px" MaxLength="10" TabIndex="1" Text='<%# Eval("MarkPassingDate","{0:dd/MM/yyyy}") %>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                                <asp:Image ID="imgMarkPassingdate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                                                                    runat="server" />
                                                            </td>
                                                            <td>Registration Date
                                                                <AjaxToolkit:MaskedEditExtender ID="MEditRegistrationDate" TargetControlID="txtRegistrationDate" Mask="99/99/9999"
                                                                    MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                                                                </AjaxToolkit:MaskedEditExtender>
                                                                <AjaxToolkit:MaskedEditValidator ID="MEditValRegistrationDate" ControlExtender="MEditRegistrationDate" ControlToValidate="txtRegistrationDate" IsValidEmpty="true"
                                                                    EmptyValueMessage="Enter Registration Date." EmptyValueBlurredText="*" InvalidValueMessage="Registration Date is invalid" SetFocusOnError="true"
                                                                    MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid"
                                                                    MinimumValue="01/01/2014" MaximumValue="31/12/2025" runat="Server" ValidationGroup="FilingRequired"></AjaxToolkit:MaskedEditValidator>
                                                            </td>
                                                            <td>

                                                                <asp:TextBox ID="txtRegistrationDate" runat="server" Width="100px" MaxLength="10" TabIndex="2" Text='<%# Eval("RegisterationDate","{0:dd/MM/yyyy}") %>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                                <asp:Image ID="imgRegistrationDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                                                                    runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Examine Date
                                                                 <AjaxToolkit:MaskedEditExtender ID="MEditExamineDate" TargetControlID="txtExamineDate" Mask="99/99/9999"
                                                                     MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                                                                 </AjaxToolkit:MaskedEditExtender>
                                                                <AjaxToolkit:MaskedEditValidator ID="MEditValExamineDate" ControlExtender="MEditExamineDate" ControlToValidate="txtExamineDate" IsValidEmpty="true"
                                                                    EmptyValueMessage="Enter Examine Date" EmptyValueBlurredText="*" InvalidValueMessage="Examine Date is invalid" SetFocusOnError="true"
                                                                    MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid"
                                                                    MinimumValue="01/01/2014" MaximumValue="31/12/2025" runat="Server" ValidationGroup="FilingRequired"></AjaxToolkit:MaskedEditValidator>
                                                            </td>
                                                            <td>

                                                                <asp:TextBox ID="txtExamineDate" runat="server" Width="100px" MaxLength="10" Text='<%# Eval("ExamineDate","{0:dd/MM/yyyy}") %>' TabIndex="3" placeholder="dd/mm/yyyy"></asp:TextBox>
                                                                <asp:Image ID="imgExamineDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                            </td>
                                                            <td>Examine Report Date
                                                                 <AjaxToolkit:MaskedEditExtender ID="MEditExamineReportDate" TargetControlID="txtExamineReportDate" Mask="99/99/9999"
                                                                     MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                                                                 </AjaxToolkit:MaskedEditExtender>
                                                                <AjaxToolkit:MaskedEditValidator ID="MEditValExamineReportDate" ControlExtender="MEditExamineReportDate" ControlToValidate="txtExamineReportDate" IsValidEmpty="true"
                                                                    EmptyValueMessage="Enter Examine Report Date" EmptyValueBlurredText="*" InvalidValueMessage="Examine Report Date is invalid" SetFocusOnError="true"
                                                                    MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid"
                                                                    MinimumValue="01/01/2014" MaximumValue="31/12/2025" runat="Server" ValidationGroup="FilingRequired"></AjaxToolkit:MaskedEditValidator>
                                                            </td>
                                                            <td>

                                                                <asp:TextBox ID="txtExamineReportDate" runat="server" Width="100px" Text='<%# Eval("ExamineReportDate","{0:dd/MM/yyyy}") %>' TabIndex="4" placeholder="dd/mm/yyyy"></asp:TextBox>
                                                                <asp:Image ID="imgExamineReport" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                                                                    runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Supretendent LEO Date
                                                                <AjaxToolkit:MaskedEditExtender ID="MEditLEODate" TargetControlID="txtLEODate" Mask="99/99/9999"
                                                                    MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                                                                </AjaxToolkit:MaskedEditExtender>
                                                                <AjaxToolkit:MaskedEditValidator ID="MEditValLEODate" ControlExtender="MEditLEODate" ControlToValidate="txtLEODate" IsValidEmpty="true"
                                                                    EmptyValueMessage="Enter Supretendent LEO Date" EmptyValueBlurredText="*" InvalidValueMessage="Supretendent LEO Date is invalid" SetFocusOnError="true"
                                                                    MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid"
                                                                    MinimumValue="01/01/2014" MaximumValue="31/12/2025" runat="Server" ValidationGroup="FilingRequired"></AjaxToolkit:MaskedEditValidator>
                                                            </td>
                                                            <td>

                                                                <asp:TextBox ID="txtLEODate" runat="server" Width="100px" MaxLength="10" Text='<%# Eval("LEODate","{0:dd/MM/yyyy}") %>' TabIndex="5" placeholder="dd/mm/yyyy"></asp:TextBox>
                                                                <asp:Image ID="imgLEODate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                            </td>
                                                            <td>Remark
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtFilingRemark" runat="server" Width="200px" TabIndex="6" Text='<%# Eval("CustomRemark") %> ' TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EditItemTemplate>
                                            </asp:FormView>
                                        </Content>
                                    </AjaxToolkit:AccordionPane>
                                    <AjaxToolkit:AccordionPane ID="accForm13" runat="server">
                                        <Header>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Form 13 Detail</Header>
                                        <Content>
                                            <asp:FormView ID="FVForm13Detail" runat="server" Width="99%" DataKeyNames="lid" DataSourceID="DataSourceJobDetail" OnDataBound="FVForm13Detail_DataBound" OnItemCommand="FVForm13Detail_ItemCommand">
                                                <HeaderStyle Font-Bold="True" />
                                                <ItemTemplate>
                                                    <div class="m clear">
                                                        <asp:Button ID="btnEditForm13Detail" runat="server" CommandName="Edit" Text="Edit" OnClick="btnEditForm13Detail_OnClick" />
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
                                                <EditItemTemplate>
                                                    <div class="m clear">
                                                        <asp:Button ID="btnUpdateForm13Detail" runat="server" CommandName="Update" OnClick="btnUpdateForm13Detail_OnClick"
                                                            Text="Update" ValidationGroup="Required" TabIndex="9" />
                                                        <asp:Button ID="btnUpdateForm13Cancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                                            Text="Cancel" TabIndex="10" />
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
                                                                <AjaxToolkit:CalendarExtender ID="calForm13Date" runat="server" Enabled="True"
                                                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgForm13Date" PopupPosition="BottomRight"
                                                                    TargetControlID="txtForm13Date">
                                                                </AjaxToolkit:CalendarExtender>

                                                                <AjaxToolkit:MaskedEditExtender ID="MskExtForm13Date" TargetControlID="txtForm13Date" Mask="99/99/9999" MessageValidatorTip="true"
                                                                    MaskType="Date" AutoComplete="false" runat="server">
                                                                </AjaxToolkit:MaskedEditExtender>
                                                                <AjaxToolkit:MaskedEditValidator ID="MskValForm13Date" ControlExtender="MskExtForm13Date" ControlToValidate="txtForm13Date" IsValidEmpty="true"
                                                                    EmptyValueMessage="Please Enter Form 13 Date." EmptyValueBlurredText="*" InvalidValueMessage="Form 13 Date is invalid"
                                                                    InvalidValueBlurredMessage="Invalid Date" MinimumValueMessage="Invalid Form 13 Date" MaximumValueMessage="Invalid Form 13 Date"
                                                                    MinimumValue="01/07/2014" MaximumValue="31/12/2025"
                                                                    runat="Server" SetFocusOnError="true" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtForm13Date" runat="server" Width="80px" MaxLength="10" Text='<%# Bind("Form13Date","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                                <asp:Image ID="imgForm13Date" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                            </td>
                                                            <td>Transporter Hand Over Date
                                                                 <AjaxToolkit:MaskedEditExtender ID="MskExtTransHandOverDate" TargetControlID="txtTransHandOverDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                                     MaskType="Date" AutoComplete="false" runat="server">
                                                                 </AjaxToolkit:MaskedEditExtender>
                                                                <AjaxToolkit:MaskedEditValidator ID="MskValTransHandOverDate" ControlExtender="MskExtTransHandOverDate" ControlToValidate="txtTransHandOverDate" IsValidEmpty="true"
                                                                    EmptyValueMessage="Please Enter Transporter Hand Over Date." EmptyValueBlurredText="*" InvalidValueMessage="Transporter Hand Over Date is invalid"
                                                                    InvalidValueBlurredMessage="Invalid Date" MinimumValueMessage="Invalid Transporter Hand Over Date" MaximumValueMessage="Invalid Transporter Hand Over Date"
                                                                    MinimumValue="01/07/2014" MaximumValue="31/12/2025"
                                                                    runat="Server" SetFocusOnError="true" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                                                            </td>
                                                            <td>

                                                                <asp:TextBox ID="txtTransHandOverDate" runat="server" Width="80px" MaxLength="10" Text='<%# Bind("TransHandoverDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                                <AjaxToolkit:CalendarExtender ID="calTransHandOverDate" runat="server" Enabled="True"
                                                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgTransHandOverDate" PopupPosition="BottomRight"
                                                                    TargetControlID="txtTransHandOverDate">
                                                                </AjaxToolkit:CalendarExtender>
                                                                <asp:Image ID="imgTransHandOverDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Container Get IN Date
                                                                  <AjaxToolkit:MaskedEditExtender ID="MskExtContainerGetInDate" TargetControlID="txtContainerGetInDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                                      MaskType="Date" AutoComplete="false" runat="server">
                                                                  </AjaxToolkit:MaskedEditExtender>
                                                                <AjaxToolkit:MaskedEditValidator ID="MskValContainerGetInDate" ControlExtender="MskExtContainerGetInDate" ControlToValidate="txtContainerGetInDate" IsValidEmpty="true"
                                                                    EmptyValueMessage="Please Enter Container Get In Date." EmptyValueBlurredText="*" InvalidValueMessage="Container Get In Date is invalid"
                                                                    InvalidValueBlurredMessage="Invalid Date" MinimumValueMessage="Invalid Container Get In Date" MaximumValueMessage="Invalid Container Get In Date"
                                                                    MinimumValue="01/07/2014" MaximumValue="31/12/2025"
                                                                    runat="Server" SetFocusOnError="true" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtContainerGetInDate" runat="server" Width="80px" MaxLength="10" Text='<%# Bind("ContainerGetInDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                                <AjaxToolkit:CalendarExtender ID="calContGetInDate" runat="server" Enabled="True"
                                                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgContainerGetInDate" PopupPosition="BottomRight"
                                                                    TargetControlID="txtContainerGetInDate">
                                                                </AjaxToolkit:CalendarExtender>
                                                                <asp:Image ID="imgContainerGetInDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
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
                                                </EditItemTemplate>
                                            </asp:FormView>
                                        </Content>
                                    </AjaxToolkit:AccordionPane>
                                    <AjaxToolkit:AccordionPane ID="accShipmentGetIn" runat="server">
                                        <Header>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Shipment Get IN Detail</Header>
                                        <Content>
                                            <asp:FormView ID="FVShipmentGetIN" runat="server" Width="99%" DataKeyNames="lid" DataSourceID="DataSourceJobDetail" OnItemCommand="FVShipmentGetIN_ItemCommand" OnDataBound="FVShipmentGetIN_DataBound">
                                                <HeaderStyle Font-Bold="True" />
                                                <ItemTemplate>
                                                    <div class="m clear">
                                                        <asp:Button ID="btnEditShipmentDetail" runat="server" CommandName="Edit" Text="Edit" OnClick="btnEditShipmentDetail_OnClick" />
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
                                                <EditItemTemplate>
                                                    <div class="m clear">
                                                        <asp:Button ID="btnUpdateShipmentDetail" runat="server" CommandName="Update" OnClick="btnUpdateShipmentDetail_OnClick"
                                                            Text="Update" ValidationGroup="Required" TabIndex="9" />
                                                        <asp:Button ID="btnUpdateshipmentCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                                            Text="Cancel" TabIndex="10" />
                                                    </div>
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                                        <tr>
                                                            <td>BS Job No.
                                                            </td>
                                                            <td>
                                                                <b><%# Eval("JobRefNo") %></b>
                                                            </td>
                                                            <td>Document Hand Over To Shipping Line Date
                                                                 <AjaxToolkit:CalendarExtender ID="calDocHandOverDate" runat="server" Enabled="True" EnableViewState="False"
                                                                     FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDocHandOverDate"
                                                                     PopupPosition="BottomRight" TargetControlID="txtDocHandOverDate">
                                                                 </AjaxToolkit:CalendarExtender>
                                                                <AjaxToolkit:MaskedEditExtender ID="MEditDocHandOverDate" TargetControlID="txtDocHandOverDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                                    MaskType="Date" AutoComplete="false" runat="server">
                                                                </AjaxToolkit:MaskedEditExtender>
                                                                <AjaxToolkit:MaskedEditValidator ID="MEditValDocHandOverDate" ControlExtender="MEditDocHandOverDate" ControlToValidate="txtDocHandOverDate" IsValidEmpty="false"
                                                                    EmptyValueMessage="Enter Document Hand Over To Shipping Line Date" EmptyValueBlurredText="*" InvalidValueMessage="Mark Date is invalid"
                                                                    SetFocusOnError="true" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid"
                                                                    MinimumValue="01/01/2014" MaximumValue="31/12/2025" runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                                                            </td>
                                                            <td>

                                                                <asp:TextBox ID="txtDocHandOverDate" runat="server" Width="100px" MaxLength="10" Text='<%# Eval("ShippingLineDate","{0:dd/MM/yyyy}")%>' TabIndex="1" placeholder="dd/mm/yyyy"></asp:TextBox>
                                                                <asp:Image ID="imgDocHandOverDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                                                                    runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Exporter Copy
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="lnkEditDwnloadExporterCopy" runat="server" Enabled="true"
                                                                    OnClick="lnkEditDwnloadExporterCopy_Click"></asp:LinkButton>
                                                                <asp:HiddenField ID="hdnEditExporterCopy" runat="server" Value='<%#Eval("ExporterCopyPath") %>' />
                                                                &nbsp;
                                                                <asp:Label ID="lblDwnloadExporter" runat="server" Text="OR"></asp:Label>
                                                                <asp:FileUpload ID="fuExporterCopy" runat="server" />
                                                                <asp:HiddenField ID="hdnExporterCopyPath" runat="server" />
                                                            </td>
                                                            <td>VGM Copy
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="lnkDwnloadVGMCopy" runat="server" Enabled="true"
                                                                    OnClick="lnkDwnloadVGMCopy_Click"></asp:LinkButton>
                                                                <asp:HiddenField ID="hdnVGMCopy" runat="server" Value='<%#Eval("VGMCopyPath") %>' />
                                                                &nbsp;
                                                                <asp:Label ID="lblDwnloadVGMCopy" runat="server" Text="OR"></asp:Label>
                                                                <asp:FileUpload ID="fuVGMcopy" runat="server" />
                                                                <asp:HiddenField ID="hdnVGMCopyPath" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Freight Forwarded Date
                                                                <AjaxToolkit:CalendarExtender ID="calFreightDate" runat="server" Enabled="True" EnableViewState="False"
                                                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgFreightDate"
                                                                    PopupPosition="BottomRight" TargetControlID="txtFreightForwarderDate">
                                                                </AjaxToolkit:CalendarExtender>
                                                                <AjaxToolkit:MaskedEditExtender ID="MEditFreightFrDate" TargetControlID="txtFreightForwarderDate" Mask="99/99/9999"
                                                                    MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                                                                </AjaxToolkit:MaskedEditExtender>
                                                                <AjaxToolkit:MaskedEditValidator ID="MEditValFreightFrDate" ControlExtender="MEditFreightFrDate" ControlToValidate="txtFreightForwarderDate" IsValidEmpty="false"
                                                                    EmptyValueMessage="Enter Informed To Freight Forwarded Date" EmptyValueBlurredText="*" InvalidValueMessage="Freight Forwarded Date is invalid" SetFocusOnError="true"
                                                                    MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid"
                                                                    MinimumValue="01/01/2014" MaximumValue="31/12/2025" runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                                                            </td>
                                                            <td>

                                                                <asp:TextBox ID="txtFreightForwarderDate" runat="server" Width="100px" Text='<%# Eval("FreightForwardedDate","{0:dd/MM/yyyy}")%>' MaxLength="10" TabIndex="3" placeholder="dd/mm/yyyy"></asp:TextBox>
                                                                <asp:Image ID="imgFreightDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                            </td>
                                                            <td>Forwarder Person Name
                                                                 <asp:RequiredFieldValidator ID="rfvForwarderPerson" runat="server" ControlToValidate="txtForwarderPerson" SetFocusOnError="true" Display="Dynamic" ForeColor="Red"
                                                                     ErrorMessage="Please Enter Forwarder Person Name" Text="*" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td>

                                                                <asp:TextBox ID="txtForwarderPerson" runat="server" Text='<%# Eval("ForwarderPersonName") %>' Width="200px" TabIndex="4"></asp:TextBox>
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
                                                </EditItemTemplate>
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
                                    <td>Seal No</td>
                                    <td>
                                        <asp:TextBox ID="txtSealNo" runat="server"></asp:TextBox></td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset>
                            <legend>Container Detail</legend>
                            <div>
                                <asp:GridView ID="gvContainer" runat="server" AllowPaging="True" CssClass="table"
                                    AutoGenerateColumns="False" DataKeyNames="JobId,lid" Width="100%" PageSize="40"
                                    DataSourceID="DataSourceContainer" AllowSorting="true" OnRowUpdating="gvContainer_RowUpdating" OnRowDeleting="gvContainer_RowDeleting">
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
                                        <asp:TemplateField HeaderText="Seal No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSealNo" runat="server" Text='<%#Eval("SealNo") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtSealNo_Edit" runat="server" Text='<%#Eval("SealNo") %>'></asp:TextBox>
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
                                        <asp:CommandField ShowEditButton="True" ShowDeleteButton="true" ValidationGroup="valGridContainer"
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
                </AjaxToolkit:TabPanel>
                <!-- Invoice -->
                <AjaxToolkit:TabPanel ID="TabInvoice" runat="server" HeaderText="Invoice">
                    <ContentTemplate>
                        <div style="overflow: scroll;">
                            <%-- <fieldset class="fieldset-AutoWidth">
                                <legend>Invoice Detail</legend>
                                <asp:GridView ID="gvInvoiceProduct" runat="server" AutoGenerateColumns="False" AutoGenerateDeleteButton="false" CssClass="table"
                                    Width="99%" DataKeyNames="lId" DataSourceID="SqlDataSourceInvoice"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20">

                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex +1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Invoiceno" HeaderText="Invoice No" />
                                        <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" />
                                        <asp:BoundField DataField="ShipmentTerm" HeaderText="Shipment Term" />
                                        <asp:BoundField DataField="DBKAmount" HeaderText="DBK Amount" />
                                        <asp:BoundField DataField="LicenseNo" HeaderText="License No" />
                                        <asp:BoundField DataField="LicenseDate" HeaderText="License Date" />
                                        <asp:BoundField DataField="FreightAmount" HeaderText="Freight Amount" />
                                        <asp:BoundField DataField="InsuranceAmount" HeaderText="Insurance Amount" />

                                    </Columns>

                                </asp:GridView>
                            </fieldset>--%>
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
                                        <AjaxToolkit:MaskedEditExtender ID="meeIncDate_footer" TargetControlID="txtInvoiceDate" Mask="99/99/9999"
                                            MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                                        </AjaxToolkit:MaskedEditExtender>
                                            <AjaxToolkit:MaskedEditValidator ID="mevInvcDate_footer" ControlExtender="meeIncDate_footer"
                                                ControlToValidate="txtInvoiceDate" IsValidEmpty="false" InvalidValueBlurredMessage="Invalid Date"
                                                InvalidValueMessage="License Date is invalid" EmptyValueBlurredText="*" EmptyValueMessage="Please Enter Invoice Date."
                                                MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/1900"
                                                MaximumValue="01/01/2025" SetFocusOnError="true" runat="Server" ValidationGroup="vgAddInvoice"></AjaxToolkit:MaskedEditValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtInvoiceDate" runat="server" MaxLength="50" Width="65px" TabIndex="2"
                                                placeholder="dd/mm/yyyy"></asp:TextBox>
                                            <asp:Image ID="imgCalInvDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                                                runat="server" />
                                            <AjaxToolkit:CalendarExtender ID="calInvcDate" runat="server" Enabled="true" EnableViewState="false"
                                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtInvoiceDate"
                                                PopupButtonID="imgCalInvDate" PopupPosition="BottomRight">
                                            </AjaxToolkit:CalendarExtender>
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
                                            <AjaxToolkit:CalendarExtender ID="calLicenseDate_footer" runat="server" Enabled="true" EnableViewState="false"
                                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtLicenseDate"
                                                PopupButtonID="imgLicenseDate" PopupPosition="BottomRight">
                                            </AjaxToolkit:CalendarExtender>
                                            <AjaxToolkit:MaskedEditExtender ID="meeLicenseDate_footer" TargetControlID="txtLicenseDate"
                                                Mask="99/99/9999" MessageValidatorTip="true" MaskType="Date" AutoComplete="false"
                                                runat="server">
                                            </AjaxToolkit:MaskedEditExtender>
                                            <AjaxToolkit:MaskedEditValidator ID="mevLicenseDate_footer" ControlExtender="meeLicenseDate_footer"
                                                ControlToValidate="txtLicenseDate" IsValidEmpty="true" MinimumValueMessage="Invalid Date"
                                                MaximumValueMessage="Invalid Date" MinimumValue="01/01/1900" MaximumValue="01/01/2025"
                                                SetFocusOnError="true" runat="Server" EmptyValueBlurredText="*" ValidationGroup="vgAddInvoice"></AjaxToolkit:MaskedEditValidator>
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
                                                <AjaxToolkit:CalendarExtender ID="calInvcDate" runat="server" Enabled="true" EnableViewState="false"
                                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtInvoiceDate">
                                                </AjaxToolkit:CalendarExtender>
                                                <AjaxToolkit:MaskedEditExtender ID="meeIncDate" TargetControlID="txtInvoiceDate" Mask="99/99/9999"
                                                    MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                                                </AjaxToolkit:MaskedEditExtender>
                                                <AjaxToolkit:MaskedEditValidator ID="mevInvcDate" ControlExtender="meeIncDate" ControlToValidate="txtInvoiceDate"
                                                    IsValidEmpty="false" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="License Date is invalid"
                                                    EmptyValueBlurredText="*" EmptyValueMessage="Please enter Invoice Date." MinimumValueMessage="Invalid Date"
                                                    MaximumValueMessage="Invalid Date" MinimumValue="01/01/1900" MaximumValue="01/01/2025"
                                                    SetFocusOnError="true" runat="Server" ValidationGroup="vgEditInvoice"></AjaxToolkit:MaskedEditValidator>
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
                                                <AjaxToolkit:CalendarExtender ID="calLicenseDate" runat="server" Enabled="true" EnableViewState="false"
                                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtLicenseDate">
                                                </AjaxToolkit:CalendarExtender>
                                                <AjaxToolkit:MaskedEditExtender ID="meeLicenseDate" TargetControlID="txtLicenseDate" Mask="99/99/9999"
                                                    MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                                                </AjaxToolkit:MaskedEditExtender>
                                                <AjaxToolkit:MaskedEditValidator ID="mevLicenseDate" ControlExtender="meeLicenseDate" ControlToValidate="txtLicenseDate"
                                                    IsValidEmpty="true" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="License Date is invalid"
                                                    MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/1900"
                                                    MaximumValue="01/01/2025" SetFocusOnError="true" runat="Server" EmptyValueBlurredText="*"
                                                    ValidationGroup="vgEditInvoice"></AjaxToolkit:MaskedEditValidator>
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
                <!-- Daily Activity -->
                <AjaxToolkit:TabPanel runat="server" ID="TabPanelDailyActivity" HeaderText="Activity">
                    <ContentTemplate>
                        <div align="center">
                            <asp:ValidationSummary ID="valActivitySummary" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="RequiredActivity" />
                        </div>
                        <div class="clear"></div>
                        <fieldset id="fieldJobActivity" runat="server">
                            <legend>Add Job Activity</legend>
                            <div class="m clear">
                                <asp:Button ID="btnSaveActivity" runat="server" ValidationGroup="RequiredActivity" Text="Save" OnClick="btnSaveActivity_Click" TabIndex="5" />
                                <asp:Button ID="btnClearActivity" Text="Reset" runat="server" CausesValidation="false" OnClick="btnClearActivity_OnClick" TabIndex="6" />
                            </div>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>Current Status
                                     <asp:RequiredFieldValidator ID="RFVActStatus" runat="server" ErrorMessage="Please Select Current Status." Text="*" SetFocusOnError="true"
                                         ControlToValidate="ddActivityStatus" Display="Dynamic" InitialValue="0" ValidationGroup="RequiredActivity"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddActivityStatus" runat="server" DataSourceID="SqlDataSourceStatusMS" TabIndex="1" Enabled="false"
                                            DataTextField="StatusName" DataValueField="lid" AppendDataBoundItems="true">
                                            <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>Visible To Customer
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="RDLActivityCustomer" runat="server" RepeatDirection="Horizontal" TabIndex="4">
                                            <asp:ListItem Text="Yes" Value="True" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="False"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Daily Progress Details
                                    <asp:RequiredFieldValidator ID="RFVActProgress" runat="server" ErrorMessage="Please Enter Progress Detail." Text="*" SetFocusOnError="true"
                                        ControlToValidate="txtActivityProgress" Display="Dynamic" ValidationGroup="RequiredActivity"></asp:RequiredFieldValidator>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtActivityProgress" runat="server" TextMode="MultiLine" Rows="3" MaxLength="4000" TabIndex="2"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>

                        </fieldset>

                        <fieldset>
                            <legend>Job Activity</legend>
                            <div>
                                <asp:Button ID="btnActivityPrint" runat="server" Text="Print" OnClick="btnActivityPrint_Click" ToolTip="Print To PDF" />
                            </div>
                            <asp:GridView ID="gvDailyJob" runat="server" AutoGenerateColumns="False" CssClass="table"
                                AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lid"
                                Width="100%" PageSize="20" PagerSettings-Position="TopAndBottom" AllowPaging="true"
                                AllowSorting="true" DataSourceID="sqldtsJobdailyActivity" OnRowCommand="gvDailyJob_RowCommand"
                                OnPreRender="gvDailyJob_PreRender" OnRowDataBound="gvDailyJob_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex +1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="dtDate" HeaderText="Date & Time" SortExpression="dtDate"
                                        DataFormatString="{0:dd/MM/yyyy hh:mm tt}" ReadOnly="true" />
                                    <asp:TemplateField HeaderText="Current Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatusName" runat="server" Text='<%#Eval("StatusName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddStatusId" runat="server" DataSourceID="SqlDataSourceStatusMS"
                                                DataTextField="StatusName" DataValueField="lid" SelectedValue='<%#Eval("StatusId")%>' AppendDataBoundItems="true">
                                                <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Progress Detail">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProgress" runat="server" Text='<%#Eval("ShortProgress") %>'></asp:Label>
                                            <asp:HyperLink ID="lnkMoreProgress" NavigateUrl="#" Text="...More" runat="server"></asp:HyperLink>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtProgressDetail" runat="server" Text='<%#Eval("DailyProgress") %>'
                                                TextMode="MultiLine"></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Visible To Customer">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomerVisible" Text='<%#(Boolean.Parse(Eval("IsCustomerVisible").ToString())? "Yes" : "No") %>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:RadioButtonList ID="RDLCustomer" runat="server" RepeatDirection="Horizontal"
                                                SelectedValue='<%#Eval("IsCustomerVisible") %>'>
                                                <asp:ListItem Text="Yes" Value="True" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="False"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Document">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkActDownload" runat="server" Text="Download" CommandArgument='<%#Eval("DocumentPath") %>'
                                                CommandName="download"></asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName"
                                        ReadOnly="true" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkActEdit" runat="server" Text="Edit" CommandName="Edit"></asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lnkActUpdate" runat="server" Text="Update" CommandName="UpdateActivity"></asp:LinkButton>
                                            <asp:LinkButton ID="lnkActCancel" runat="server" Text="Cancel" CommandName="Cancel"></asp:LinkButton>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                </Columns>

                            </asp:GridView>
                        </fieldset>
                        <asp:SqlDataSource ID="sqldtsJobdailyActivity" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="EX_GetJobActivityDetailById" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="JobId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="SqlDataSourceStatusMS" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="EX_GetJobStatusMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                        <div class="clear"></div>
                        <fieldset id="flBill" runat="server">
                            <legend>Add Billing Status</legend>
                            <div class="m clear">
                                <asp:Button ID="btnSaveBillStatus" runat="server" ValidationGroup="RequiredStatus" Text="Save" OnClick="btnSaveBillStatus_Click" TabIndex="5" />
                                <asp:Button ID="btnClearBillStatus" Text="Reset" runat="server" CausesValidation="false" OnClick="btnClearBillStatus_OnClick" TabIndex="6" />
                            </div>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>Bill Status
                                     <asp:RequiredFieldValidator ID="RFVBillStatus1" runat="server" ErrorMessage="Please Select Bill Status." Text="*" SetFocusOnError="true"
                                         ControlToValidate="ddBillStatus" Display="Dynamic" InitialValue="0" ValidationGroup="RequiredStatus"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddBillStatus" runat="server" DataSourceID="SqlDataSourceBillStatusMS" TabIndex="1"
                                            DataTextField="BillStatusName" DataValueField="lid" AppendDataBoundItems="true">
                                            <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>Remark
                                    <asp:RequiredFieldValidator ID="RFVBillStatusRemark" runat="server" ErrorMessage="Please Enter Remark" Text="*" SetFocusOnError="true"
                                        ControlToValidate="txtRemark" Display="Dynamic" ValidationGroup="RequiredStatus"></asp:RequiredFieldValidator>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Rows="3" MaxLength="4000" TabIndex="2"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>

                        </fieldset>

                        <fieldset>
                            <legend>Billing Activity</legend>
                            <asp:GridView ID="gvBillingStatus" runat="server" AutoGenerateColumns="False" CssClass="table"
                                AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lid"
                                Width="100%" PageSize="20" PagerSettings-Position="TopAndBottom" AllowPaging="true"
                                AllowSorting="true" DataSourceID="DataSourceStatusDetail"
                                OnPreRender="gvBillingStatus_PreRender" OnRowDataBound="gvBillingStatus_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex +1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="dtDate" HeaderText="Date & Time" SortExpression="dtDate"
                                        DataFormatString="{0:dd/MM/yyyy hh:mm tt}" ReadOnly="true" />
                                    <asp:TemplateField HeaderText="Bill Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatusName" runat="server" Text='<%#Eval("BIllStatusName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remark">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRemark" runat="server" Text='<%#Eval("ShortRemark") %>'></asp:Label>
                                            <asp:HyperLink ID="lnkBillProgress" NavigateUrl="#" Text="...More" runat="server"></asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="UserName" HeaderText="User" SortExpression="UserName" ReadOnly="true" />
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                        <asp:SqlDataSource ID="DataSourceStatusDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="EX_GetBillingStatusDetail" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="JobId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="SqlDataSourceBillStatusMS" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="GetBillingStatusMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
                <!-- Field -->
                <AjaxToolkit:TabPanel runat="server" ID="TabField" HeaderText="Additional Field">
                    <ContentTemplate>
                        <div>
                            <fieldset>
                                <legend>Add Additional Field</legend>
                                <div class="m clear">
                                    <asp:Button ID="btnSave" Text="Save" OnClick="btnSave_Click" runat="server" />
                                </div>
                                <p>
                                    <asp:Table ID="CustomUITable" runat="server" border="0" CellPadding="0" CellSpacing="0" Width="100%" bgcolor="white">
                                    </asp:Table>
                                </p>
                            </fieldset>
                            <div class="clear">
                            </div>
                            <fieldset>
                                <legend>Job Additional Field Detail</legend>
                                <asp:GridView ID="GridViewJobField" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                                    DataSourceID="FieldSqlDataSource" CellPadding="4" AllowPaging="True" AllowSorting="True"
                                    PageSize="40">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FieldName" HeaderText="Field Name" />
                                        <asp:BoundField DataField="FieldValue" HeaderText="Field Value" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>
                            <div>
                                <asp:SqlDataSource ID="FieldSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="EX_GetJobFieldValues" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter SessionField="JobId" Name="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </div>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
                <!-- Delivery Details -->
                <AjaxToolkit:TabPanel runat="server" ID="TabPaneDelivery" HeaderText="Delivery">
                    <ContentTemplate>
                        <div>
                            <div>
                                <fieldset>
                                    <legend>Update Delivery Detail</legend>
                                    <asp:Button ID="btnUpdateJobDelivery" Text="Update" OnClick="btnUpdateJobDelivery_Click" runat="server" ValidationGroup="RequiredDelivery" />
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                        <tr>
                                            <td>Delivery Destination
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeliveryDestination" runat="server" MaxLength="100"></asp:TextBox>
                                            </td>
                                            <td>Transportation By
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rdlTransport" runat="server" RepeatDirection="Horizontal" TabIndex="9">
                                                    <asp:ListItem Text="Babaji" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Customer" Value="0"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                            <div style="overflow: scroll;">
                                <fieldset class="fieldset-AutoWidth">
                                    <legend>Delivery To Warehouse</legend>
                                    <asp:GridView ID="GridViewWarehouse" runat="server" AutoGenerateColumns="False" CssClass="table"
                                        Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                                        DataSourceID="DataSourceWarehouse" OnRowCommand="GridViewWarehouse_RowCommand" OnRowDataBound="GridViewWarehouse_RowDataBound"
                                        CellPadding="4" AllowPaging="True" AllowSorting="True" PagerSettings-Position="TopAndBottom" PageSize="40">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkedit1" runat="server" CommandName="Edit" ToolTip="Edit" Width="22" Text="Edit" Font-Underline="true"></asp:LinkButton>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:LinkButton ID="lnkupdate1" runat="server" ToolTip="update" CommandName="Update" Text="Update" Width="45" Font-Underline="true" ValidationGroup="GridDeliveryRequired"></asp:LinkButton>
                                                    <asp:LinkButton ID="lnkcancel1" runat="server" ToolTip="cancel" CommandName="Cancel" Text="Cancel" Width="39" Font-Underline="true"></asp:LinkButton>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
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
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtPackages" runat="server" Text='<%#Eval("NoOfPackages")%>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Vehicle No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVehicleNo" runat="server" Text='<%#Eval("VehicleNo")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtVehicleNo" runat="server" Text='<%#Eval("VehicleNo")%>'></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="Rfv_VehicleNo" runat="server" ControlToValidate="txtVehicleNo" ErrorMessage="Please Enter Vehicle No" ValidationGroup="GridDeliveryRequired"
                                                        SetFocusOnError="true" Text="*"></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Vehicle Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVehicleType" runat="server" Text='<%#Eval("vehiclename") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList ID="ddVehicleType" runat="server" DataSourceID="GetVehicleMSSqlDataSource"
                                                        DataTextField="sName" DataValueField="lid" SelectedValue='<%#Eval("vehicleId")%>' AppendDataBoundItems="true">
                                                        <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="reqVehicleType" runat="server" ErrorMessage="Please Select Vehicle Type." SetFocusOnError="true"
                                                        InitialValue="0" Text="*" Display="Dynamic" ValidationGroup="GridDeliveryRequired" ControlToValidate="ddVehicleType"></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Vehicle Received Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVehicleRcvdDate" runat="server" Text='<%#Eval("VehicleRcvdDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtVehicleRcvdDate" runat="server" Width="100px" Text='<%# Eval("VehicleRcvdDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                    <asp:Image ID="imgVehicleRcvdDate" ImageAlign="top" ImageUrl="../Images/btn_calendar.gif"
                                                        runat="server" />
                                                    <asp:RequiredFieldValidator ID="Rfv_VehicleRcvdDate" runat="server" ControlToValidate="txtVehicleRcvdDate" ErrorMessage="Please Enter Vehicle Received Date"
                                                        ValidationGroup="GridDeliveryRequired" SetFocusOnError="true" Text="*"></asp:RequiredFieldValidator>
                                                    <AjaxToolkit:CalendarExtender ID="calVehicleRcvdDate" runat="server" Enabled="true"
                                                        EnableViewState="false" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgVehicleRcvdDate"
                                                        PopupPosition="BottomRight" TargetControlID="txtVehicleRcvdDate">
                                                    </AjaxToolkit:CalendarExtender>
                                                    <asp:CompareValidator ID="ComValVehicleRcvdDate" runat="server" ControlToValidate="txtVehicleRcvdDate"
                                                        Display="Dynamic" Text="*" ErrorMessage="Invalid Vehicle Received Date." Type="Date" SetFocusOnError="false"
                                                        CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="GridDeliveryRequired"></asp:CompareValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Transporter Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTransporter" runat="server" Text='<%#Eval("TransporterName")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <%-- <asp:TextBox ID="txtTransporter" runat="server" Text='<%#Eval("TransporterName")%>'> </asp:TextBox>--%>
                                                    <asp:DropDownList ID="ddTransporter" runat="server" DataSourceID="DataSourceTransporter"
                                                        DataTextField="CustName" DataValueField="lid" SelectedValue='<%#Eval("TransporterId")%>' AppendDataBoundItems="true">
                                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvtransporter" runat="server" ControlToValidate="ddTransporter" ErrorMessage="Please Select Transporter."
                                                        ValidationGroup="GridDeliveryRequired" InitialValue="0" SetFocusOnError="true" Text="*"></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="LR No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLRNo" runat="server" Text='<%#Eval("LRNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtLRNo" runat="server" Text='<%#Eval("LRNo")%>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="LR Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLRDate" runat="server" Text='<%#Eval("LRDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtLRDate" runat="server" Width="100px" Text='<%# Bind("LRDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                    <AjaxToolkit:CalendarExtender ID="calLRDate" runat="server" Enabled="true" EnableViewState="false"
                                                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgLRDate"
                                                        PopupPosition="BottomRight" TargetControlID="txtLRDate">
                                                    </AjaxToolkit:CalendarExtender>
                                                    <asp:Image ID="imgLRDate" ImageAlign="top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                    <asp:CompareValidator ID="ComValLRDate" runat="server" ControlToValidate="txtLRDate"
                                                        Display="Dynamic" Text="*" ErrorMessage="Invalid LR Date." Type="Date" CultureInvariantValues="false"
                                                        Operator="DataTypeCheck" ValidationGroup="GridDeliveryRequired"></asp:CompareValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delivery Point">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDeliveryPoint" runat="server" Text='<%#Eval("DeliveryPoint") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtDeliveryPoint" runat="server" Width="100px" Text='<%# Eval("DeliveryPoint")%>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Dispatch Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDispatchDate" runat="server" Text='<%#Eval("DispatchDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtDispatchDate" runat="server" Width="100px" Text='<%# Eval("DispatchDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                    <AjaxToolkit:CalendarExtender ID="calDispatchDate" runat="server" Enabled="true"
                                                        EnableViewState="false" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDispatchDate"
                                                        PopupPosition="BottomRight" TargetControlID="txtDispatchDate">
                                                    </AjaxToolkit:CalendarExtender>
                                                    <asp:Image ID="imgDispatchDate" ImageAlign="top" ImageUrl="../Images/btn_calendar.gif"
                                                        runat="server" />
                                                    <asp:CompareValidator ID="ComValDispatchDate" runat="server" ControlToValidate="txtDispatchDate"
                                                        Display="Dynamic" Text="*" ErrorMessage="Invalid Dispatch Date." Type="Date" SetFocusOnError="true"
                                                        CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="GridDeliveryRequired"></asp:CompareValidator>
                                                    <asp:RequiredFieldValidator ID="reqvDispatchDate" runat="server" Display="Dynamic"
                                                        Text="*" ErrorMessage="Please Enter Dispatch Date." ValidationGroup="GridDeliveryRequired"
                                                        ControlToValidate="txtDispatchDate"></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delivery Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDeliveryDate" runat="server" Text='<%#Eval("DeliveryDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtDeliveryDate" runat="server" Width="100px" Text='<%# Eval("DeliveryDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                    <AjaxToolkit:CalendarExtender ID="calDeliveryDate" runat="server" Enabled="true"
                                                        EnableViewState="false" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDeliveryDate"
                                                        PopupPosition="BottomRight" TargetControlID="txtDeliveryDate">
                                                    </AjaxToolkit:CalendarExtender>
                                                    <asp:Image ID="imgDeliveryDate" ImageAlign="top" ImageUrl="../Images/btn_calendar.gif"
                                                        runat="server" />
                                                    <asp:CompareValidator ID="ComValDeliveryDate" runat="server" ControlToValidate="txtDeliveryDate"
                                                        Display="Dynamic" Text="*" ErrorMessage="Invalid Delivery Date." Type="Date" SetFocusOnError="true"
                                                        CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="GridDeliveryRequired"></asp:CompareValidator>
                                                    <asp:RequiredFieldValidator ID="rfvDeliveryDate" runat="server" Display="Dynamic"
                                                        Text="*" ErrorMessage="Please Enter Delivery Date." ValidationGroup="GridDeliveryRequired"
                                                        ControlToValidate="txtDeliveryDate"></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Road Permit No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRoadPermitNo" runat="server" Text='<%#Eval("RoadPermitNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtRoadPermitNo" runat="server" Width="100px" Text='<%#Eval("RoadPermitNo")%>' MaxLength="100"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="reqvRoadPermitNo" runat="server" Display="Dynamic"
                                                        Text="*" ErrorMessage="Please Enter Road Permit No." ValidationGroup="GridDeliveryRequired"
                                                        ControlToValidate="txtRoadPermitNo"></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Road Permit Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRoadPermitDate" runat="server" Text='<%#Eval("RoadPermitDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtRoadPermitDate" runat="server" Width="100px" Text='<%# Eval("RoadPermitDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                    <AjaxToolkit:CalendarExtender ID="calRoadPermitDate" runat="server" Enabled="true"
                                                        EnableViewState="false" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgRoadPermitDate"
                                                        PopupPosition="BottomRight" TargetControlID="txtRoadPermitDate">
                                                    </AjaxToolkit:CalendarExtender>
                                                    <asp:Image ID="imgRoadPermitDate" ImageAlign="top" ImageUrl="Images/btn_calendar.gif"
                                                        runat="server" />
                                                    <asp:CompareValidator ID="ComValRoadPermitDate" runat="server" ControlToValidate="txtRoadPermitDate"
                                                        Display="Dynamic" ErrorMessage="Invalid Road Permit Date." Type="Date" Text="*" CultureInvariantValues="false"
                                                        Operator="DataTypeCheck" ValidationGroup="GridDeliveryRequired"></asp:CompareValidator>
                                                    <asp:RequiredFieldValidator ID="reqvRoadPermitDate" runat="server" Display="Dynamic"
                                                        Text="*" ErrorMessage="Please Enter Road Permit Date" ValidationGroup="GridDeliveryRequired"
                                                        ControlToValidate="txtRoadPermitDate"></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cargo Received By">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCargoRecBy" runat="server" Text='<%#Eval("CargoReceivedBy") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtCargoRecBy" runat="server" Width="100px" Text='<%# Eval("CargoReceivedBy")%>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="POA Attachment Path">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDownload" runat="server" Text='<%#Eval("PODAttachmentPath") %>'
                                                        CommandName="Download" CommandArgument='<%#Eval("PODDownload") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:FileUpload ID="fuPODAttchment" runat="server" />
                                                    <asp:HiddenField ID="hdnPODPath" runat="server" Value='<%#Eval("PODDownload") %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="N Form No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNFormNo" runat="server" Text='<%#Eval("NFormNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtNFormNo" runat="server" Text='<%# Eval("NFormNo")%>'></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="reqvNFormNo" runat="server" Display="Dynamic"
                                                        Text="*" ErrorMessage="Please Enter N Form No" ValidationGroup="GridDeliveryRequired"
                                                        ControlToValidate="txtNFormNo"></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="N Form Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNFormDate" runat="server" Text='<%#Eval("NFormDate","{0:dd/MM/yyyy}") %>' placeholder="dd/mm/yyyy"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtNFormDate" runat="server" Width="100px" Text='<%# Eval("NFormDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                    <AjaxToolkit:CalendarExtender ID="calNFormDate" runat="server" Enabled="true" EnableViewState="false"
                                                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgNFormDate" PopupPosition="BottomRight"
                                                        TargetControlID="txtNFormDate">
                                                    </AjaxToolkit:CalendarExtender>
                                                    <asp:Image ID="imgNFormDate" ImageAlign="top" ImageUrl="Images/btn_calendar.gif"
                                                        runat="server" />
                                                    <asp:CompareValidator ID="ComValNFormDate" runat="server" ControlToValidate="txtNFormDate"
                                                        Display="Dynamic" ErrorMessage="Invalid N Form Date." Type="Date" Text="*" CultureInvariantValues="false"
                                                        Operator="DataTypeCheck" ValidationGroup="GridDeliveryRequired"></asp:CompareValidator>

                                                    <asp:RequiredFieldValidator ID="reqvNFormDate" runat="server" Display="Dynamic"
                                                        Text="*" ErrorMessage="Please Enter N Form Date" ValidationGroup="GridDeliveryRequired"
                                                        ControlToValidate="txtNFormDate"></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="N Closing Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNClosingDate" runat="server" Text='<%#Eval("NClosingDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtNClosingDate" runat="server" Width="100px" Text='<%# Eval("NClosingDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                    <AjaxToolkit:CalendarExtender ID="calNClosingDate" runat="server" Enabled="true"
                                                        EnableViewState="false" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgNClosingDate"
                                                        PopupPosition="BottomRight" TargetControlID="txtNClosingDate">
                                                    </AjaxToolkit:CalendarExtender>
                                                    <asp:Image ID="imgNClosingDate" ImageAlign="top" ImageUrl="Images/btn_calendar.gif"
                                                        runat="server" />
                                                    <asp:CompareValidator ID="ComValNClosingDate" runat="server" ControlToValidate="txtNClosingDate"
                                                        Display="Dynamic" ErrorMessage="Invalid N Closing Date." Type="Date" Text="*" CultureInvariantValues="false"
                                                        Operator="DataTypeCheck" ValidationGroup="GridDeliveryRequired"></asp:CompareValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="S Form No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSFormNo" runat="server" Text='<%#Eval("SFormNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtSFormNo" runat="server" Width="100px" Text='<%# Eval("SFormNo")%>'></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="reqvSFormNo" runat="server" Display="Dynamic"
                                                        Text="*" ErrorMessage="Please Enter S Form No" ValidationGroup="GridDeliveryRequired"
                                                        ControlToValidate="txtSFormNo"></asp:RequiredFieldValidator>
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
                                                        Operator="DataTypeCheck" ValidationGroup="GridDeliveryRequired"></asp:CompareValidator>
                                                    <asp:RequiredFieldValidator ID="reqvSFormDate" runat="server" Display="Dynamic"
                                                        Text="*" ErrorMessage="Please Enter S Form Date" ValidationGroup="GridDeliveryRequired"
                                                        ControlToValidate="txtSFormDate"></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="S Form Closing Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSClosingDate" runat="server" Text='<%#Eval("SClosingDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtSClosingDate" runat="server" Width="100px" Text='<%# Eval("SClosingDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                    <AjaxToolkit:CalendarExtender ID="calSClosingDate" runat="server" Enabled="true"
                                                        EnableViewState="false" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgSClosingDate"
                                                        PopupPosition="BottomRight" TargetControlID="txtSClosingDate">
                                                    </AjaxToolkit:CalendarExtender>
                                                    <asp:Image ID="imgSClosingDate" ImageAlign="top" ImageUrl="Images/btn_calendar.gif"
                                                        runat="server" />
                                                    <asp:CompareValidator ID="ComValSClosingDate" runat="server" ControlToValidate="txtSClosingDate"
                                                        Display="Dynamic" ErrorMessage="Invalid S Closing Date." Type="Date" Text="*" CultureInvariantValues="false"
                                                        Operator="DataTypeCheck" ValidationGroup="GridDeliveryRequired"></asp:CompareValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Octroi Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOctroiAmount" runat="server" Text='<%#Eval("OctroiAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtOctroiAmount" runat="server" Width="100px" Text='<%# Eval("OctroiAmount")%>'></asp:TextBox>
                                                    <asp:CompareValidator ID="CompValOctroiAmount" runat="server" ControlToValidate="txtOctroiAmount"
                                                        Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" ErrorMessage="Invalid Octroi Amount."
                                                        Display="Dynamic" ValidationGroup="GridDeliveryRequired"></asp:CompareValidator>
                                                    <asp:RequiredFieldValidator ID="reqvOctroiAmount" runat="server" Display="Dynamic"
                                                        Text="*" ErrorMessage="Please Enter Octroi Amount" ValidationGroup="GridDeliveryRequired"
                                                        ControlToValidate="txtOctroiAmount"></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Octroi Receipt No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOctroiReceiptNo" runat="server" Text='<%#Eval("OctroiReceiptNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtOctroiReceiptNo" runat="server" Width="100px" Text='<%# Eval("OctroiReceiptNo")%>'></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="reqvOctroiReceiptNo" runat="server" Display="Dynamic"
                                                        Text="*" ErrorMessage="Please Enter Octroi Receipt No" ValidationGroup="GridDeliveryRequired"
                                                        ControlToValidate="txtOctroiReceiptNo"></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Octroi Paid Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOctroiPaidDate" runat="server" Text='<%#Eval("OctroiPaidDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtOctroiPaidDate" runat="server" Width="100px" Text='<%# Eval("OctroiPaidDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                    <AjaxToolkit:CalendarExtender ID="calOctroiPaidDate" runat="server" Enabled="true"
                                                        EnableViewState="false" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgOctroiPaidDate"
                                                        PopupPosition="BottomRight" TargetControlID="txtOctroiPaidDate">
                                                    </AjaxToolkit:CalendarExtender>
                                                    <asp:Image ID="imgOctroiPaidDate" ImageAlign="top" ImageUrl="Images/btn_calendar.gif"
                                                        runat="server" />
                                                    <asp:CompareValidator ID="ComValOctroiPaidDate" runat="server" ControlToValidate="txtOctroiPaidDate"
                                                        Display="Dynamic" ErrorMessage="Invalid Octroi Paid Date." Type="Date" Text="*" CultureInvariantValues="false"
                                                        Operator="DataTypeCheck" ValidationGroup="GridDeliveryRequired"></asp:CompareValidator>

                                                    <asp:RequiredFieldValidator ID="reqvOctroiPaidDat" runat="server" Display="Dynamic"
                                                        Text="*" ErrorMessage="Please Enter Octroi Paid Date" ValidationGroup="GridDeliveryRequired"
                                                        ControlToValidate="txtOctroiPaidDate"></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Babaji Challan No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChallanNo" runat="server" Text='<%#Eval("BabajiChallanNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtChallanNo" runat="server" Text='<%#Eval("BabajiChallanNo")%>'></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="reqvChallanNo" runat="server" Display="Dynamic"
                                                        Text="*" ErrorMessage="Please Enter Babaji Challan No" ValidationGroup="GridDeliveryRequired"
                                                        ControlToValidate="txtChallanNo"></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Babaji Challan Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChalanDate" runat="server" Text='<%#Eval("BabajiChallanDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtChalanDate" runat="server" Text='<%#Eval("BabajiChallanDate","{0:dd/MM/yyyy}") %>'></asp:TextBox>
                                                    <AjaxToolkit:CalendarExtender ID="calchallandate" runat="server" Enabled="true" EnableViewState="false"
                                                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgchallandt" TargetControlID="txtChalanDate">
                                                    </AjaxToolkit:CalendarExtender>
                                                    <asp:Image ID="imgchallandt" runat="server" ImageUrl="~/Images/btn_calendar.gif" />
                                                    <asp:CompareValidator ID="comValchallandt" runat="server" ControlToValidate="txtChalanDate" Display="Dynamic"
                                                        Text="*" CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="GridDeliveryRequired" ErrorMessage="Invalid Babaji Challan Date." Type="Date"></asp:CompareValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Babaji Challan Copy">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkChallanDownload_Warehouse" runat="server" Text='<%#Eval("BabajiChallanCopyFile") %>'
                                                        CommandName="Download" CommandArgument='<%#Eval("BabajiChallanCopyPath") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:FileUpload ID="fuBCCAttchment_Warehouse" runat="server" />
                                                    <asp:HiddenField ID="hdnBCCPath_Warehouse" runat="server" Value='<%#Eval("BabajiChallanCopyPath") %>' />
                                                </EditItemTemplate>
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
                                                <%--   <EditItemTemplate>
                                                    <asp:DropDownList ID="ddLabourType" runat="server" DataSourceID="DataSourceTransporter"
                                                        DataTextField="CustName" DataValueField="lid" SelectedValue='<%#Eval("LabourTypeId")%>' AppendDataBoundItems="true">
                                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvLabourType" runat="server" ControlToValidate="ddLabourType" ErrorMessage="Please Select Labour Type."
                                                        ValidationGroup="GridDeliveryRequired" InitialValue="0" SetFocusOnError="true" Text="*"></asp:RequiredFieldValidator>
                                                </EditItemTemplate>--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Driver Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldrivername" runat="server" Text='<%#Eval("DriverName")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtdrivername" runat="server" Text='<%#Eval("DriverName")%>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Driver Phone no.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldriverphoneno" runat="server" Text='<%#Eval("DriverPhone")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtdriverphoneno" runat="server" Text='<%#Eval("DriverPhone")%>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="User Name" DataField="UserName" ReadOnly="true" />
                                            <asp:BoundField HeaderText="Date" DataField="DeliveryCreatedDate" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" ReadOnly="true" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkdelete1" runat="server" Text="Delete" ToolTip="Delete" CommandName="Delete1"
                                                        OnClientClick="return confirm('Are you sure wants to Delete?');"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
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
                                SelectCommand="EX_GetDeliveryDetail" SelectCommandType="StoredProcedure" UpdateCommand="EX_updJobDeliveryAsPerlid"
                                UpdateCommandType="StoredProcedure" OnUpdated="DataSourceWarehouse_Updated">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                </SelectParameters>
                                <UpdateParameters>
                                    <asp:Parameter Name="lid" Type="int32" />
                                    <asp:Parameter Name="NoOfPackages" Type="Int32" DefaultValue="0" />
                                    <asp:Parameter Name="VehicleNo" Type="string" />
                                    <asp:Parameter Name="VehicleType" Type="Int32" />
                                    <asp:Parameter Name="VehicleRcvdDate" DbType="datetime" />
                                    <asp:Parameter Name="TransporterID" Type="Int32" />
                                    <asp:Parameter Name="LRNo" Type="string" />
                                    <asp:Parameter Name="LRDate" DbType="datetime" />
                                    <asp:Parameter Name="DeliveryPoint" Type="string" />
                                    <asp:Parameter Name="DispatchDate" DbType="datetime" />
                                    <asp:Parameter Name="DeliveryDate" DbType="datetime" />
                                    <asp:Parameter Name="RoadPermitNo" DbType="String" />
                                    <asp:Parameter Name="RoadPermitDate" DbType="datetime" />
                                    <asp:Parameter Name="CargoReceivedBy" DbType="String" />
                                    <asp:Parameter Name="PODAttachment" DbType="String" />
                                    <asp:Parameter Name="NFormNo" DbType="string" />
                                    <asp:Parameter Name="NFormDate" DbType="datetime" />
                                    <asp:Parameter Name="NClosingDate" DbType="datetime" />
                                    <asp:Parameter Name="SFormNo" DbType="string" />
                                    <asp:Parameter Name="SFormDate" DbType="datetime" />
                                    <asp:Parameter Name="SClosingDate" DbType="datetime" />
                                    <asp:Parameter Name="OctroiAmount" DbType="decimal" />
                                    <asp:Parameter Name="OctroiReceiptNo" DbType="String" />
                                    <asp:Parameter Name="OctroiPaidDate" DbType="datetime" />
                                    <asp:Parameter Name="BabajiChallanNo" DbType="string" />
                                    <asp:Parameter Name="BabajiChallanDate" DbType="datetime" />
                                    <asp:Parameter Name="BabajiChallanCopyFile" DbType="string" />
                                    <%-- <asp:Parameter Name="DamagedImageFile" DbType="string" />
                                    <asp:Parameter Name="LabourTypeId" DbType="Int32" />--%>
                                    <asp:Parameter Name="DriverName" DbType="string" />
                                    <asp:Parameter Name="DriverPhone" DbType="string" />
                                    <asp:SessionParameter Name="lUser" SessionField="UserId" />
                                    <asp:Parameter Name="OutPut" Type="int32" Direction="Output" />
                                </UpdateParameters>
                            </asp:SqlDataSource>
                            <div class="m clear">
                            </div>
                        </div>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
                <!-- History -->
                <AjaxToolkit:TabPanel runat="server" ID="TabPanelHistory" HeaderText="History">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Job History</legend>
                            <asp:FormView ID="FVJobHistory" HeaderStyle-Font-Bold="true" runat="server" OnDataBound="FVJobHistory_DataBound"
                                Width="100%">
                                <ItemTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">

                                        <tr>
                                            <td>Job Created By
                                            </td>
                                            <td>
                                                <%# Eval("UserName") %>
                                            </td>
                                            <td>Job Created Date
                                            </td>
                                            <td>
                                                <%# Eval("JobCreatedDate", "{0:dd/MM/yyyy hh:mm tt}")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>SB Prepared By
                                            </td>
                                            <td>
                                                <%# Eval("ChecklistPreparedBy")%>
                                            </td>
                                            <td>SB Prepared Date
                                            </td>
                                            <td>
                                                <%# Eval("ChecklistDate", "{0:dd/MM/yyyy hh:mm tt}")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>SB Filed By
                                            </td>
                                            <td>
                                                <%# Eval("SBFiledBy")%>
                                            </td>
                                            <td>SB Filed Date
                                            </td>
                                            <td>
                                                <%# Eval("SBFiledDate", "{0:dd/MM/yyyy hh:mm tt}")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Custom Cleared By
                                            </td>
                                            <td>
                                                <%# Eval("CustomClearedBy")%>
                                            </td>
                                            <td>Custom Clearance Date
                                            </td>
                                            <td>
                                                <%# Eval("CustomClearedOn", "{0:dd/MM/yyyy hh:mm tt}")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Form 13 Cleared By
                                            </td>
                                            <td>
                                                <%# Eval("Form13ClearedBy")%>
                                            </td>
                                            <td>Form 13 Cleared Date
                                            </td>
                                            <td>
                                                <%# Eval("Form13ClearedOn", "{0:dd/MM/yyyy hh:mm tt}")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Shipment Get IN By
                                            </td>
                                            <td>
                                                <%# Eval("ShipmentClearedBy")%>
                                            </td>
                                            <td>Shipment Get IN Date
                                            </td>
                                            <td>
                                                <%# Eval("ShipmentClearedOn", "{0:dd/MM/yyyy hh:mm tt}")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Checklist Status
                                            </td>
                                            <td>
                                                <%# Eval("ChecklistStatusName")%>
                                            </td>
                                            <td>Download Checklist
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="lnkChecklistDoc" runat="server" Text="Not Uploaded" Enabled="false"
                                                    OnClick="lnkChecklistDoc_Click"></asp:LinkButton>
                                                <asp:HiddenField ID="hdnChecklistDocPath" runat="server" Value='<%#Eval("ChecklistDocPath") %>' />
                                            </td>
                                        </tr>

                                    </table>
                                    <div class="clear">
                                    </div>
                                </ItemTemplate>
                            </asp:FormView>
                        </fieldset>
                        <fieldset>
                            <legend>Checklist History</legend>
                            <div style="overflow: auto;">
                                <asp:GridView ID="gvCheckListHistory" runat="server" AutoGenerateColumns="False"
                                    CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="CheckListHistoryDataSource"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Status" DataField="Status" />
                                        <asp:BoundField HeaderText="Requested By" DataField="RequestedBy" />
                                        <asp:BoundField HeaderText="Requested Date" DataField="RequestedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Request Remark" DataField="RequestRemark" />
                                        <asp:BoundField HeaderText="Authorised By" DataField="AuthorisedBy" />
                                        <asp:BoundField HeaderText="Authorised Date" DataField="AuthorisedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Author Remark" DataField="AuthorRemark" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                        <div id="DivDataSourceHistory">
                            <asp:SqlDataSource ID="CheckListHistoryDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="EX_GetChecklistApprovalHistory" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
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
                                                    <legend>Shipment Get IN To PCA</legend>
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                                        <tr>
                                                            <td>Status
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblBackOfficeStatus" Text='<%#GetBooleanToCompletedPending(Eval("PCDToBackOffice"))%>'
                                                                    runat="server"></asp:Label>
                                                            </td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td>User Name
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblBackOfficeUser" runat="server" Text='<%#Eval("BackOfficeUser") %>'></asp:Label>
                                                            </td>
                                                            <td>Completed Date
                                                            </td>
                                                            <td>
                                                                <%# Eval("BackOfficeDate", "{0:dd/MM/yyyy}")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <fieldset>
                                                    <legend>PCA TO Customer</legend>
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                                        <tr>
                                                            <td>PCA TO Customer
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblPCDToCustomer" Text='<%#GetBooleanToYesNo(Eval("PCDRequired"))%>'
                                                                    runat="server"></asp:Label>
                                                            </td>
                                                            <td>Status
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label1" Text='<%#GetBooleanToCompletedPending(Eval("PCDToCustomer"))%>'
                                                                    runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>User Name
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblDispatchLocation" runat="server" Text='<%#Eval("PCDCustomerUser") %>'></asp:Label>
                                                            </td>
                                                            <td>Completed Date
                                                            </td>
                                                            <td>
                                                                <%# Eval("PCDCustomerDate", "{0:dd/MM/yyyy}")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">
                                                                <asp:LinkButton ID="lnkPCALetter" OnClick="lnkPCALetter_Click" runat="server" Text="Create PCA Letter"></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <fieldset>
                                                    <legend>Billing Advice</legend>
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                                        <tr>
                                                            <td>Status
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label2" Text='<%#GetBooleanToCompletedPending(Eval("bStatus"))%>'
                                                                    runat="server"></asp:Label>
                                                            </td>
                                                            <td>Scrutiny Status
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label3" Text='<%#GetBooleanToApprovedRejected(Eval("ApprovalStatus"))%>' runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>User Name
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label4" runat="server" Text='<%#Eval("BillingAdviceUser") %>'></asp:Label>
                                                            </td>
                                                            <td>Completed Date
                                                            </td>
                                                            <td>
                                                                <%# Eval("BillingAdviceDate", "{0:dd/MM/yyyy}")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <fieldset>
                                                    <legend>Billing Scrutiny</legend>
                                                    <asp:GridView ID="gvScrutinyHistory" runat="server" AutoGenerateColumns="False" CssClass="table"
                                                        Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceScrutinyHistory"
                                                        CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Sl">
                                                                <ItemTemplate>
                                                                    <%#Container.DataItemIndex + 1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="Status" DataField="Status" />
                                                            <asp:BoundField HeaderText="Requested By" DataField="RequestedBy" />
                                                            <asp:BoundField HeaderText="Request Date" DataField="RequestedDate" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" />
                                                            <asp:BoundField HeaderText="Request Remark" DataField="RequestRemark" />
                                                            <asp:BoundField HeaderText="Authorised By" DataField="AuthorisedBy" />
                                                            <asp:BoundField HeaderText="Authorise Date" DataField="AuthorisedDate" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" />
                                                            <asp:BoundField HeaderText="Author Remark" DataField="AuthorRemark" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <fieldset>
                                                    <legend>Billing Dept/Invoice</legend>
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                                        <tr>
                                                            <td>Billing Status
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label5" Text='<%#GetBooleanToCompletedPending(Eval("PCDToBilling"))%>' runat="server"></asp:Label>
                                                            </td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td>User Name
                                                            </td>
                                                            <td>
                                                                <%#Eval("BillingUser") %>
                                                            </td>
                                                            <td>Completed Date
                                                            </td>
                                                            <td>
                                                                <%# Eval("BillingDate", "{0:dd/MM/yyyy}")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">
                                                                <asp:GridView ID="gvPCDInvoice" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                    CssClass="table" PageSize="40" PagerSettings-Position="TopAndBottom" DataKeyNames="lId"
                                                                    PagerStyle-CssClass="pgr" AllowPaging="true" DataSourceID="PCDBilingInvoiceSqlDataSource">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Sl">
                                                                            <ItemTemplate>
                                                                                <%# Container.DataItemIndex +1%>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="DraftInvoiceDate" HeaderText="Draft Invoice Date" DataFormatString="{0:dd/MM/yyyy}" />
                                                                        <asp:BoundField DataField="CheckingDate" HeaderText="Checking Date" DataFormatString="{0:dd/MM/yyyy}" />
                                                                        <asp:BoundField DataField="FinalTypingDate" HeaderText="Final Typing Date" DataFormatString="{0:dd/MM/yyyy}" />
                                                                        <asp:BoundField DataField="GenerlisingDate" HeaderText="Generlising Date" DataFormatString="{0:dd/MM/yyyy}" />
                                                                        <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" SortExpression="InvoiceNo" />
                                                                        <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" DataFormatString="{0:dd/MM/yyyy}" />
                                                                        <asp:BoundField DataField="InvoiceAmount" HeaderText="Invoice Amount" SortExpression="InvoiceAmount" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>
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
                            <legend>Upload PCA Document</legend>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>PCA Document Type
                                        <asp:DropDownList ID="ddPCDDocument" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td>Document For
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddPCDDocFlowType" runat="server">
                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                            <%-- <asp:ListItem Text="Shipment Get In" Value="1"></asp:ListItem>--%>
                                            <asp:ListItem Text="PCA Customer" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Billing Advice" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Billing Dispatch" Value="4"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="m">
                                        <asp:FileUpload ID="fuPCDUpload1" runat="server" />
                                    </td>
                                    <td>
                                        <asp:CheckBoxList ID="chkDuplicate" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Original" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Copy" Value="2"></asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnPCDUpload" runat="server" Text="Upload" OnClick="btnPCDUpload_Click" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <%--<div class="m clear">
                        </div>
                        <fieldset>
                            <legend>Shipment Other Document</legend>
                            <asp:GridView ID="GridViewBackOfficeDoc" runat="server" AutoGenerateColumns="False"
                                CssClass="table" Width="100%" PagerStyle-CssClass="pgr" DataKeyNames="lid" CellPadding="4"
                                AllowPaging="True" AllowSorting="True" PageSize="20" DataSourceID="BackOfficeSqlDataSource" OnRowCommand="GridViewBackOfficeDoc_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DocumentName" HeaderText="Document Type" SortExpression="DocumentName" />
                                    <asp:BoundField DataField="CreatedBy" HeaderText="User Name" SortExpression="CreatedBy" />
                                    <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" SortExpression="CreatedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    <asp:TemplateField HeaderText="Download">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                                CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                        </fieldset>--%>
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
                <!-- Billing-->
                <AjaxToolkit:TabPanel runat="server" ID="TabBiiling" HeaderText="Billing Details">
                    <ContentTemplate>
                        <div>
                            <fieldset id="BillingInstruction" runat="server">
                                <legend>Billing Instruction</legend>
                                <div id="dvResult" runat="server" style="max-height: 550px; overflow: auto; text-align: center;">
                                    <br />
                                    <div align="center">
                                        <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" align="center" style="text-align: left;">
                                            <tr>
                                                <td><b>Allied Service</b></td>
                                                <td>
                                                    <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                                    <asp:Label ID="lblAlliedAgencyService" runat="server" Text='<%# Bind("AlliedAgencyService") %>'></asp:Label>
                                                        </div>
                                                </td>
                                                <td><b>Allied Remark</b></td>
                                                <td>
                                                    <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                                    <asp:Label ID="lblAlliedAgencyRemark" runat="server" Text='<%# Bind("AlliedAgencyRemark") %>'></asp:Label>
                                                        </div>
                                                </td>
                                                <td>
                                                    <b>Read By & Date</b>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblAlliedReadBy" runat="server"></asp:Label>
                                                </td>
                                                <td><b>Charge By & Date</b></td>
                                                <td>
                                                    <asp:Label ID="lblAlliedChargeBy" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>Other Service</b></td>
                                                <td>
                                                    <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                                    <asp:Label ID="lblOtherService" runat="server" ></asp:Label>
                                                        </div>
                                                </td>
                                                <td><b>Other Service remark</b></td>
                                                <td>
                                                    <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                                    <asp:Label ID="lblOtherServiceRemark" runat="server"></asp:Label>
                                                        </div>
                                                </td>
                                                <td>
                                                    <b>Read By & Date</b>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblOtherServiceReadBy" runat="server"></asp:Label>
                                                </td>
                                                <td><b>Charge By & Date</b></td>
                                                <td>
                                                    <asp:Label ID="lblOtherServiceChargeBy" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>Other Instruction</b></td>
                                                <td>
                                                    <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                                    <asp:Label ID="lblInstruction" runat="server"></asp:Label>
                                                    </div>
                                                </td>
                                                <td><b>Instruction Copy</b></td>
                                                <td>
                                                    <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                                    <asp:LinkButton ID="lnkInstructionCopy" runat="server" OnClick="lnkInstructionCopy_Click"></asp:LinkButton>
                                                        <asp:HiddenField id="hdnInstructionCopy" runat="server"/>
                                                    </div>
                                                </td>
                                                <td>
                                                    <b>Read By & Date</b>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblReadBy1" runat="server"></asp:Label>
                                                </td>
                                                <td><b>Charge By & Date</b></td>
                                                <td>
                                                    <asp:Label ID="lblChargeBy1" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>Other Instruction</b></td>
                                                <td>
                                                    <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                                    <asp:Label ID="lblInstruction1" runat="server"></asp:Label>
                                                    </div>
                                                </td>
                                                <td><b>Instruction Copy</b></td>
                                                <td>
                                                    <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                                    <asp:LinkButton ID="lnkInstructionCopy1" runat="server" OnClick="lnkInstructionCopy1_Click"></asp:LinkButton>
                                                        <asp:HiddenField id="hdnInstructionCopy1" runat="server"/>
                                                </div>
                                                    </td>
                                                <td>
                                                    <b>Read By & Date</b>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblReadBy2" runat="server"></asp:Label>
                                                </td>
                                                <td><b>Charge By & Date</b></td>
                                                <td>
                                                    <asp:Label ID="lblChargeBy2" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>Other Instruction</b></td>
                                                <td>
                                                    <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                                    <asp:Label ID="lblInstruction2" runat="server"></asp:Label>
                                                </div>
                                                </td>
                                                <td><b>Instruction Copy</b></td>
                                                <td>
                                                    <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                                    <asp:LinkButton ID="lnkInstructionCopy2" runat="server" OnClick="lnkInstructionCopy2_Click"></asp:LinkButton>
                                                        <asp:HiddenField id="hdnInstructionCopy2" runat="server" />
                                                    </div>
                                                    </td>
                                                <td>
                                                    <b>Read By & Date</b>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblReadBy3" runat="server"></asp:Label>
                                                </td>
                                                <td><b>Charge By & Date</b></td>
                                                <td>
                                                    <asp:Label ID="lblChargeBy3" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>Other Instruction</b></td>
                                                <td>
                                                    <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                                    <asp:Label ID="lblInstruction3" runat="server"></asp:Label>
                                                    </div>
                                                </td>
                                                <td><b>Instruction Copy</b></td>
                                                <td>
                                                    <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                                    <asp:LinkButton ID="lnkInstructionCopy3" runat="server" OnClick="lnkInstructionCopy3_Click"></asp:LinkButton>
                                                        <asp:HiddenField id="hdnInstructionCopy3" runat="server"/>
                                                    </div>
                                                </td>
                                                <td>
                                                    <b>Read By & Date</b>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblReadBy4" runat="server"></asp:Label>
                                                </td>
                                                <td><b>Charge By & Date</b></td>
                                                <td>
                                                    <asp:Label ID="lblChargeBy4" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>User Name</b></td>
                                                <td>
                                                    <asp:Label ID="lblUserId" runat="server"></asp:Label>
                                                </td>
                                                <td><b>User Date</b></td>
                                                <td>
                                                    <asp:Label ID="lblUserDate" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </fieldset>

                            <fieldset id="BillingScrutiny" runat="server">
                                <legend>Billing Scrutiny</legend>
                                <asp:Label ID="lblfreight" runat="server"></asp:Label>
                                <asp:GridView ID="gvbillingscrutiny" runat="server" AutoGenerateColumns="False"
                                    CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceBillingScrutiny"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40" OnRowDataBound="gvbillingscrutiny_RowDataBound">
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
                                        <asp:BoundField HeaderText="FreightJob" DataField="FreightjobNo" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                            <fieldset id="DraftInvoice" runat="server">
                                <legend>Draft Invoice</legend>
                                <asp:Label ID="lblConsolidated" runat="server"></asp:Label>
                                <asp:GridView ID="gvDraftInvoice" runat="server" AutoGenerateColumns="False"
                                    CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceDraftinvoice"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40" OnRowDataBound="gvDraftInvoice_RowDataBound"
                                    OnRowCommand="gvDraftInvoice_RowCommand">
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
                                        <asp:ButtonField Text="Next" ButtonType="Button" CommandName="DraftInvoiceNext" />
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
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40" OnRowCommand="gvFinaltyping_RowCommand">
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
                                        <asp:ButtonField Text="Next" ButtonType="Button" CommandName="FinalTypingNext" />
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
                                <asp:GridView ID="gvbilldispatch" runat="server" AutoGenerateColumns="False" CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceBillDispatch"
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
                                <asp:GridView ID="gvBillrejection" runat="server" AutoGenerateColumns="False" CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceBillRejection"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
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
                            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False"
                                CssClass="table" Width="100%" PagerStyle-CssClass="pgr" DataKeyNames="lid" CellPadding="4"
                                AllowPaging="True" AllowSorting="True" PageSize="20" DataSourceID="BillingDeptSqlDataSource1">
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
                                <AjaxToolkit:ModalPopupExtender ID="ModalPopupEmail" runat="server" CacheDynamicResults="false"
                                    DropShadow="False" PopupControlID="Panel2Email" TargetControlID="lnkDummy">
                                </AjaxToolkit:ModalPopupExtender>

                                <asp:Panel ID="Panel2Email" runat="server" CssClass="ModalPopupPanel">
                                    <div class="header">
                                        <div class="fleft">
                                            Customer Email
                                        </div>
                                        <div class="fright">
                                            <asp:ImageButton ID="imgEmailClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="btnEMailCancel_Click" ToolTip="Close" />
                                        </div>
                                    </div>
                                    <div id="dvMail" runat="server" style="max-height: 900px; max-width: 850px;">
                                        <div id="dvMailSend" runat="server" style="max-height: 900px; max-width: 750px;">
                                            <div id="divPreviewEmail" runat="server" style="margin-left: 10px;">
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:LinkButton ID="lnkDummy" runat="server"></asp:LinkButton>
                                <!--Customer Email Draft End -->
                            </div>

                            <div id="divResendEmail" >
                                <AjaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" CacheDynamicResults="false"
                                    DropShadow="False" PopupControlID="PanelEmail" TargetControlID="LinkButton2">
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
                                            <asp:Button ID="btnSendEmail" runat="server" Text="Send Email" OnClick="btnSendEmail_Click" ValidationGroup="mailRequired"
                                                OnClientClick="if (!Page_ClientValidate('mailRequired')){ return false; } this.disabled = true; this.value = 'Processing...';" UseSubmitBehavior="false" />
                                            <%--<asp:Label ID="lblStatus" runat="server" Text="abc"></asp:Label>--%><br />
                                            <div class="m">
                                                <asp:Label ID="lblPopMessageEmail" runat="server" EnableViewState="false"></asp:Label>
                                                Email To:&nbsp;<asp:TextBox ID="txtMailTo" runat="server" Width="85%"></asp:TextBox><br />
                                                Email CC:&nbsp;<asp:TextBox ID="txtMailCC" runat="server" Width="85%"></asp:TextBox><br />
                                                Subject:&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtSubject" runat="server" Width="85%"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvSubject" runat="server" ControlToValidate="txtSubject" SetFocusOnError="true"
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
                                <asp:SqlDataSource ID="BillingDeptSqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetPCDDocumentByWorkFlow" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    <asp:Parameter Name="DocumentForType" DefaultValue="4" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:SqlDataSource ID="SqlDataSourceBillDispatchDoc" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetBillDispatchDocDetail" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:SqlDataSource ID="SqlDataSourceMailDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetBillDispatchMailDetail" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            </div>
                        </fieldset>

                            <div id="div1">
                                <asp:SqlDataSource ID="DataSourceBillingScrutiny" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetBillingScrutinyById" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <asp:SqlDataSource ID="DataSourceDraftinvoice" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetDraftInvoiceById" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <asp:SqlDataSource ID="DataSourceDraftCheck" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetDraftCheckById" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <asp:SqlDataSource ID="DataSourceFinalTyping" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetFinalTypingById" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <asp:SqlDataSource ID="DataSourceFinalCheck" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetFinalCheckById" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <asp:SqlDataSource ID="DataSourceBillDispatch" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetBillDispatchById" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <asp:SqlDataSource ID="DataSourceBillRejection" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetBillRejectionById" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </div>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
                <!-- Billing-->
                <AjaxToolkit:TabPanel runat="server" ID="TABExpenseDetails" HeaderText="Expense Details">
                    <ContentTemplate>
                        <div>

                            <fieldset>
                                <legend>Job Expense Details</legend>

                                <fieldset>
                                    <legend>Job Expense Entry</legend>

                                    <asp:FormView ID="fvExpense" runat="server" DefaultMode="Insert" Width="100%"
                                        DataKeyNames="lId" OnDataBound="fvExpense_DataBound">
                                        <EmptyDataTemplate>
                                            <div>
                                                <asp:Button ID="btnNew" runat="server" Text="New Expense" OnClick="btnNewExpense_Click" TabIndex="13" />
                                                <asp:Button ID="btnNewCancel" Text="Back To Expense Detail" runat="server" CausesValidation="false"
                                                    OnClick="btnNewCancel_OnClick" TabIndex="14" />
                                            </div>
                                        </EmptyDataTemplate>
                                        <ItemTemplate>
                                            <div>
                                                <asp:Button ID="btnNew" runat="server" Text="New Expense" OnClick="btnNewExpense_Click" TabIndex="13" />
                                                <asp:Button ID="btnNewCancel" Text="Back To Expense Detail" runat="server" CausesValidation="false"
                                                    OnClick="btnNewCancel_OnClick" TabIndex="14" />
                                            </div>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <div>
                                                <asp:Button ID="btnAdd" runat="server" ValidationGroup="RequiredExp"
                                                    Text="Save" OnClick="btnAdd_Click" TabIndex="13" />
                                                <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="false"
                                                    OnClick="btnCancel_OnClick" TabIndex="14" />

                                            </div>
                                            <br />
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                                <tr>
                                                    <td>Charges For/Expense Type
                                                    <asp:RequiredFieldValidator ID="RFVType" runat="server" InitialValue="0" ControlToValidate="ddlExpenseType" SetFocusOnError="true"
                                                        Text="*" ErrorMessage="Please Select Charges For/Expense Type" Display="Dynamic" ValidationGroup="RequiredExp"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlExpenseType" runat="server" TabIndex="1">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>Expense Amount
                                                        <asp:RequiredFieldValidator ID="RFVEAmount" runat="server" ControlToValidate="txtEAmount" SetFocusOnError="true"
                                                            Text="*" ErrorMessage="Please Enter Expense Amount" Display="Dynamic" ValidationGroup="RequiredExp"></asp:RequiredFieldValidator>
                                                        <asp:CompareValidator ID="CompAmount" ControlToValidate="txtEAmount" runat="server" SetFocusOnError="true"
                                                            Operator="DataTypeCheck" Type="Double" ErrorMessage="Please Enter Valid Expense Amount" Text="Invalid Amount" ValidationGroup="Required"></asp:CompareValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEAmount" runat="server" MaxLength="12" TabIndex="2"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Receipt No.
                                                        <AjaxToolkit:CalendarExtender ID="calReceiptDate" runat="server" PopupButtonID="imgReceiptDate" TargetControlID="txtReceiptDate"
                                                            Enabled="true" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" EnableViewState="False" PopupPosition="BottomRight">
                                                        </AjaxToolkit:CalendarExtender>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtReceiptNo" runat="server" MaxLength="100" TabIndex="3"></asp:TextBox>
                                                    </td>
                                                    <td>Receipt Date
                                                        <AjaxToolkit:CalendarExtender ID="calChequeDate" runat="server" PopupButtonID="imgChequeDate" TargetControlID="txtChequeDate"
                                                            Enabled="true" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" EnableViewState="False" PopupPosition="BottomRight">
                                                        </AjaxToolkit:CalendarExtender>
                                                        <asp:CompareValidator ID="ComValETADate" runat="server" ControlToValidate="txtReceiptDate" Display="Dynamic" ValidationGroup="RequiredExp"
                                                            ErrorMessage="Invalid Receipt Date." Type="Date" Operator="DataTypeCheck" SetFocusOnError="true"></asp:CompareValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtReceiptDate" runat="server" Width="100px" TabIndex="4" placeholder="dd/mm/yyyy"></asp:TextBox>
                                                        <asp:Image ID="imgReceiptDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Receiptable / Non Receiptable
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddReceipt" runat="server" TabIndex="5">
                                                            <asp:ListItem Value="1" Text="Receipts"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="Non-Receipted"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>Location
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtLocation" runat="server" MaxLength="100" TabIndex="6"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Type Of Payment
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddPaymentType" runat="server" TabIndex="7"></asp:DropDownList>
                                                    </td>
                                                    <td>Receipt Amount
                                                        <asp:CompareValidator ID="CompValRecptAmnt" ControlToValidate="txtReceiptAmount" runat="server" SetFocusOnError="true"
                                                            Operator="DataTypeCheck" Type="Double" ErrorMessage="*" ValidationGroup="RequiredExp"></asp:CompareValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtReceiptAmount" runat="server" MaxLength="12" TabIndex="8"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <!-- -->
                                                <tr>
                                                    <td>Paid To
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPaidTo" runat="server" MaxLength="100" TabIndex="9"></asp:TextBox>
                                                    </td>
                                                    <td>Billable / No Billable
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddBillable" runat="server" TabIndex="10">
                                                            <asp:ListItem Text="Billable" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="No Billable" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Cheque No
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtChequeNo" runat="server" MaxLength="100" TabIndex="11"></asp:TextBox>
                                                    </td>

                                                    <td>Cheque Date
                                                        <asp:CompareValidator ID="CompValCheque" runat="server" ControlToValidate="txtChequeDate" Display="Dynamic" ValidationGroup="RequiredExp"
                                                            ErrorMessage="Invalid Cheque Date." Type="Date" Operator="DataTypeCheck" SetFocusOnError="true"></asp:CompareValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtChequeDate" runat="server" Width="100px" TabIndex="12" placeholder="dd/mm/yyyy"></asp:TextBox>
                                                        <asp:Image ID="imgChequeDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <div>
                                                <asp:Button ID="btnUpdate" runat="server" ValidationGroup="UpdRequired"
                                                    Text="Update" OnClick="btnUpdate_Click" TabIndex="13" />
                                                <asp:Button ID="btnUpdCancel" Text="Cancel" runat="server" CausesValidation="false"
                                                    OnClick="btnUpdCancel_OnClick" TabIndex="14" />
                                                <asp:HiddenField ID="hdnExpenseType" Value='<%#Bind("ExpenseTypeId") %>' runat="server" />
                                                <asp:HiddenField ID="hdnReceiptable" Value='<%#Bind("ReceiptTypeId") %>' runat="server" />
                                                <asp:HiddenField ID="hdnPaymentType" Value='<%#Bind("PaymentType") %>' runat="server" />
                                            </div>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                                <tr>
                                                    <td>Charges For/Expense Type
                                                        <asp:RequiredFieldValidator ID="RFVUpdType" runat="server" InitialValue="0" ControlToValidate="ddlUPDExpenseType" SetFocusOnError="true"
                                                            Text="*" ErrorMessage="Please Select Charges For/Expense Type" Display="Dynamic" ValidationGroup="UpdRequired"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlUPDExpenseType" runat="server" TabIndex="1">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>Expense Amount
                                                        <asp:RequiredFieldValidator ID="RFVEUPDAmount" runat="server" ControlToValidate="txtUPDExAmount" SetFocusOnError="true"
                                                            Text="*" ErrorMessage="Please Enter Expense Amount" Display="Dynamic" ValidationGroup="UpdRequired"></asp:RequiredFieldValidator>
                                                        <asp:CompareValidator ID="CompAmount" ControlToValidate="txtUPDExAmount" runat="server" SetFocusOnError="true"
                                                            Operator="DataTypeCheck" Type="Double" ErrorMessage="Please Enter Valid Expense Amount" Text="Invalid Amount" ValidationGroup="UpdRequired"></asp:CompareValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtUPDExAmount" Text='<%#Bind("Amount") %>' runat="server" MaxLength="12" TabIndex="2"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Receipt No.
                                                        <AjaxToolkit:CalendarExtender ID="calUPDReceiptDate" runat="server" PopupButtonID="imgUPDReceiptDate" TargetControlID="txtUPDReceiptDate"
                                                            Enabled="true" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" EnableViewState="False" PopupPosition="BottomRight">
                                                        </AjaxToolkit:CalendarExtender>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtUPDReceiptNo" Text='<%#Bind("ReceiptNo") %>' runat="server" MaxLength="100" TabIndex="3"></asp:TextBox>
                                                    </td>
                                                    <td>Receipt Date
              
                                                        <asp:CompareValidator ID="ComValUPDETADate" runat="server" ControlToValidate="txtUPDReceiptDate" Display="Dynamic" ValidationGroup="UpdRequired"
                                                            ErrorMessage="Invalid Receipt Date." Type="Date" Operator="DataTypeCheck" SetFocusOnError="true"></asp:CompareValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtUPDReceiptDate" Text='<%#Bind("ReceiptDate","{0:dd/MM/yyyy}") %>' runat="server" Width="100px" TabIndex="4" placeholder="dd/mm/yyyy"></asp:TextBox>
                                                        <asp:Image ID="imgUPDReceiptDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Receiptable / Non Receiptable
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddUPDReceipt" runat="server" TabIndex="5">
                                                            <asp:ListItem Value="1" Text="Receipts"></asp:ListItem>
                                                            <asp:ListItem Value="0" Text="Non-Receipted"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>Location
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtUPDLocation" Text='<%#Bind("Location") %>' runat="server" MaxLength="100" TabIndex="6"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Type Of Payment
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddUPDPaymentType" runat="server" TabIndex="7"></asp:DropDownList>
                                                    </td>
                                                    <td>Receipt Amount
                                                        <asp:CompareValidator ID="CompValUPDRecptAmnt" ControlToValidate="txtUPDReceiptAmount" runat="server" SetFocusOnError="true"
                                                            Operator="DataTypeCheck" Type="Double" ErrorMessage="*" ValidationGroup="UpdRequired"></asp:CompareValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtUPDReceiptAmount" Text='<%#Bind("ReceiptAmount") %>' runat="server" MaxLength="12" TabIndex="8"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <!-- -->
                                                <tr>
                                                    <td>Paid To
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtUPDPaidTo" Text='<%#Bind("PaidTo") %>' runat="server" MaxLength="100" TabIndex="9"></asp:TextBox>
                                                    </td>
                                                    <td>Billable / No Billable
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddUPDBillable" runat="server" TabIndex="10">
                                                            <asp:ListItem Text="Billable" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="No Billable" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Cheque No
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtUPDChequeNo" Text='<%#Bind("ChequeNo") %>' runat="server" MaxLength="100" TabIndex="11"></asp:TextBox>
                                                    </td>

                                                    <td>Cheque Date
                                                          <AjaxToolkit:CalendarExtender ID="calUPDChequeDate" runat="server" PopupButtonID="imgChequeDate" TargetControlID="txtUPDChequeDate"
                                                              Enabled="true" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" EnableViewState="False" PopupPosition="BottomRight">
                                                          </AjaxToolkit:CalendarExtender>
                                                        <asp:CompareValidator ID="CompValUPDCheque" runat="server" ControlToValidate="txtUPDChequeDate" Display="Dynamic" ValidationGroup="UpdRequired"
                                                            ErrorMessage="Invalid Cheque Date." Type="Date" Operator="DataTypeCheck" SetFocusOnError="true"></asp:CompareValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtUPDChequeDate" Text='<%#Bind("ChequeDate","{0:dd/MM/yyyy}") %>' runat="server" Width="100px" TabIndex="12" placeholder="dd/mm/yyyy"></asp:TextBox>
                                                        <asp:Image ID="imgChequeDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </EditItemTemplate>
                                    </asp:FormView>
                                </fieldset>
                                <fieldset>
                                    <legend>Job Expense Detail</legend>
                                    <div>
                                        <asp:Button ID="btnPrintJobExpense" runat="server" Text="Print" OnClick="btnPrintJobExpense_Click" ToolTip="Print To PDF" CausesValidation="false" />
                                    </div>
                                    <div id="content" style="overflow: scroll;">
                                        <asp:GridView ID="gvExpenseDetails" runat="server" AutoGenerateColumns="False" CssClass="table"
                                            Width="98%" PagerStyle-CssClass="pgr" DataKeyNames="lid" CellPadding="4" PageSize="40"
                                            PagerSettings-Position="TopAndBottom" AllowPaging="true"
                                            DataSourceID="DataJobExpenseDetails" OnRowCommand="gvExpenseDetails_RowCommand">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex +1%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit" Text="Edit" CommandName="Select" CommandArgument='<%#Eval("lId") %>'
                                                            runat="server" ToolTip="Edit Expense">
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ExpenseName" HeaderText="Expense Type" />
                                                <asp:BoundField DataField="Amount" HeaderText="Amount" />
                                                <asp:BoundField DataField="ReceiptNo" HeaderText="Receipt No" />
                                                <asp:BoundField DataField="PaidTo" HeaderText="Paid To" />
                                                <asp:BoundField DataField="ReceiptType" HeaderText="Receipt" />
                                                <asp:BoundField DataField="Location" HeaderText="Location" />
                                                <asp:BoundField DataField="PaymentTypeName" HeaderText="Type of Payment" />
                                                <asp:BoundField DataField="ReceiptAmount" HeaderText="Receipt Amount" />
                                                <asp:BoundField DataField="ReceiptDate" HeaderText="Receipt Date" DataFormatString="{0:dd/MM/yyyy}" />
                                                <asp:BoundField DataField="BillableNoBillable" HeaderText="Billable/NoBillable" />
                                                <asp:BoundField DataField="ChequeNo" HeaderText="Cheque No" />
                                                <asp:BoundField DataField="ChequeDate" HeaderText="Cheque Date" DataFormatString="{0:dd/MM/yyyy}" />
                                                <asp:BoundField DataField="UserName" HeaderText="User Name" />
                                                <asp:BoundField DataField="dtDate" HeaderText="Activity Date" DataFormatString="{0:dd/MM/yyyy}" />

                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </fieldset>
                                <div>
                                    <asp:SqlDataSource ID="DataJobExpenseDetails" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                        SelectCommand="EX_GetExpensesDetail" SelectCommandType="StoredProcedure">
                                        <SelectParameters>
                                            <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>
                            </fieldset>

                            <fieldset>
                                <legend>FA Expense Details</legend>
                                <asp:GridView ID="gvjobexpenseDetail" runat="server" AutoGenerateColumns="False" ShowFooter="true"
                                    CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataSourceID="DataSourceExpenseDetail"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40" OnRowDataBound="gvjobexpenseDetail_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="VCHDATE" DataField="VCHDATE" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="VCHNO" DataField="VCHNO" />
                                        <asp:BoundField HeaderText="CONTRANAME" DataField="CONTRANAME" />
                                        <asp:BoundField HeaderText="CHQNO" DataField="CHQNO" />
                                        <asp:BoundField HeaderText="CHQDATE" DataField="CHQDATE" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="DEBITAMT" DataField="DEBITAMT" />
                                        <asp:BoundField HeaderText="CREDITAMT" DataField="CREDITAMT">
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="AMOUNT" DataField="AMOUNT">
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="NARRATION" DataField="NARRATION" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                            <div id="div2">
                                <asp:SqlDataSource ID="DataSourceExpenseDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetBJVDetails" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </div>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
            </AjaxToolkit:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

