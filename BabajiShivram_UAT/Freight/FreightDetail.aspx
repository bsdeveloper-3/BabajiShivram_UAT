<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FreightDetail.aspx.cs" Inherits="Freight_FreightDetail" 
        MasterPageFile="~/MasterPage.master" Title="Freight Details" Culture="en-GB" %>
  <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
  <%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .Tab .ajax__tab_header {white-space:nowrap !important;}
    </style>
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPanelDetail" runat="server">
            <ProgressTemplate>
                <div style="position:absolute;visibility:visible;border:none;z-index:100;width:90%;height:90%;background:#FAFAFA; filter: alpha(opacity=80);-moz-opacity:.8; opacity:.8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position:relative; top:40%;left:40%; "/>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <script src="../JS/jquery.min.js" type="text/javascript"></script>
    <script src="../JS/jquery-ui.js" type="text/javascript"></script>
    <link href="../JS/jquery-ui-1.8.7.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript">

        function ActiveTabChanged12() {
            /* Clear Error Message on Tab change Event */
            document.getElementById('<%=lblError.ClientID%>').innerHTML = '';
            document.getElementById('<%=lblError.ClientID%>').className = '';
        }
        // Reminder Type Checkbox list selection -Validation Required
        function ValidateCheckList(source, args) {
            var chkListMode = document.getElementById('<%= chkRemindMode.ClientID %>');
            var chkListinputs = chkListMode.getElementsByTagName("input");
            for (var i = 0; i < chkListinputs.length; i++) {
                if (chkListinputs[i].checked) {
                    args.IsValid = true;
                    return;
                }
            }
            args.IsValid = false;
        }
    </script>
        
    <script type="text/javascript">

        function ConfirmAward() {

            var ddFreightStatus =   document.getElementById('ctl00_ContentPlaceHolder1_EnquiryTabs_TabPanelFreightDetail_fvFreightStatus_ddFreightStatus');
            var hdnTypeId       =   document.getElementById('hdnTypeId');

            var StatusId        =   ddFreightStatus.options[ddFreightStatus.selectedIndex].value;
            var TypeId          =   $get('<%=hdnTypeId.ClientID%>').value

            // Import Awarded Enquiry
            if (StatusId == 3) {

                var strMessage = "Enquiry Awarded! This enquiry will be moved to Operation. Do you want to Update Enquiry ?";
                if (confirm(strMessage))
                { }
                else {
                    return false;
                }
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
    
    <script type="text/javascript">
        function ShowPopup(message) {
            $(function () {
                $("#dialog").html(message);
                $("#dialog").dialog({
                    title: "Message",
                    buttons: {
                        Close: function () {
                            $(this).dialog('close');
                        }
                    },
                    modal: true
                });
            });
        };
    </script>
    
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
            <div id="dialog" style="display: none"> </div>
            <div class="clear"></div>
            <AjaxToolkit:TabContainer runat="server" ID="EnquiryTabs" ActiveTabIndex="0" CssClass="Tab" CssTheme="None"
                Width="100%" OnClientActiveTabChanged="ActiveTabChanged12" AutoPostBack="false">
                <AjaxToolkit:TabPanel runat="server" ID="TabPanelFreightDetail" HeaderText="Freight Detail">
                    <ContentTemplate>
                        <fieldset id="fieldStatus" runat="server">
                            <legend>Freight Status</legend>
                            <asp:FormView ID="fvFreightStatus" runat="server" DataKeyNames="EnqId" Width="100%">
                            <HeaderStyle Font-Bold="True" />
                                <ItemTemplate>
                                    <div class="m clear">
                                        <asp:Button ID="btnStatusChange" runat="server" OnClick="btnStatusChange_Click" Text="Change Status" ToolTip="Update Enquiry Status" CausesValidation="false" />

                                        <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" ToolTip="Back To Enquiry" CausesValidation="false" />
                                    </div>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div class="m clear">
                                        <asp:Button ID="btnStatusUpdate" runat="server" OnClick="btnStatusUpdate_Click" Text="Update"
                                            ValidationGroup="validateStatus" OnClientClick="if(Page_ClientValidate('validateStatus')) return ConfirmAward(); return false;" />
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
                                                <asp:DropDownList ID="ddFreightStatus" runat="server" OnSelectedIndexChanged="ddFreightStatus_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                <asp:DropDownList ID="ddLostStaus" runat="server" Visible="false">
                                                    <asp:ListItem Text="--Lost Reason--" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Rate Issue upto 10%" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Rate Issue 10%-20%" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Rate Issue 20%-30%" Value="3"></asp:ListItem>
                                                    <asp:ListItem Text="Agent did not Quote rates" Value="4"></asp:ListItem>
                                                    <asp:ListItem Text="Quote Submitted Late" Value="5"></asp:ListItem>
                                                    <asp:ListItem Text="No Feedback" Value="6"></asp:ListItem>
                                                    <asp:ListItem Text="Others" Value="7"></asp:ListItem>
                                                </asp:DropDownList> 
                                                <asp:RequiredFieldValidator ID="RFVLostStatus" runat="server" ControlToValidate="ddLostStaus" Display="Dynamic" ValidationGroup="validateStatus"
                                                InitialValue="0" SetFocusOnError="true" Text="Required" Enabled="false"></asp:RequiredFieldValidator> 
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
                                            <td>
                                                <asp:TextBox ID="txtStatusRemark" runat="server" TextMode="MultiLine" Width="80%" MaxLength="800"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" id="lblEnqValue" Text="Enquiry Value" visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEnquiryValue" runat="server" visible="false" OnTextChanged="txtEnquiryValue_TextChanged" AutoPostBack="true" width="100px" ></asp:TextBox>
                                                <asp:Label runat="server" id="lblResult" ForeColor="Red" ></asp:Label>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEnquiryValue" Display="Dynamic" ValidationGroup="validateStatus"
                                                InitialValue="" SetFocusOnError="true" Text="Required"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEnquiryValue" ForeColor="Red"
                                                     ErrorMessage="Only numeric allowed."  ValidationExpression="^[0-9]*$" ValidationGroup="validateStatus">Please Enter Proper value
                                                </asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </EditItemTemplate>
                            </asp:FormView>
                            <div>
                            <asp:SqlDataSource ID="StatusHistorySqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="FR_GetStatusHistory" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            </div>
                            </fieldset>
                        <div style="overflow: scroll;">
                        <fieldset class="fieldset-AutoWidth">
                            <legend>Status History</legend>
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
                        </div>
                        <fieldset><legend>Freight Detail</legend>
                            <asp:FormView ID="FVFreightDetail" runat="server" DataKeyNames="EnqId" Width="100%" OnDataBound="FVFreightDetail_DataBound">
                                <HeaderStyle Font-Bold="True" />
                                <ItemTemplate>
                                    <div class="m clear">
                                        <asp:Button ID="btnFreightEdit" runat="server" OnClick="btnFreightEdit_Click" Text="Edit" CausesValidation="false" />
                                    </div>
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
                                                Ref No/Email Ref
                                            </td>
                                            <td>
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
                                                Enquiry Value
                                            </td>
                                            <td>
                                                <%#Eval("EnquiryValue")%>
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
                                                Project Emp
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
                                <EditItemTemplate>
                                    <div class="m clear">
                                        <asp:Button ID="btnFreightUpdate" runat="server" OnClick="btnFreightUpdate_Click"
                                            Text="Update" ValidationGroup="valUpdEnquiry" TabIndex="19" />
                                        <asp:Button ID="btnFreightCancel" runat="server" OnClick="btnFreightCancel_Click" CausesValidation="False"
                                            Text="Cancel" TabIndex="20" />
                                    </div>
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
                                               <%# Eval("ENQDate","{0:dd/MM/yyyyy}")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Freight Type
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddFreightType" runat="server" SelectedValue='<%#Eval("lType") %>' TabIndex="1">
                                                    <asp:ListItem Text="Import" Value="1" ></asp:ListItem>
                                                    <asp:ListItem Text="Export" Value="2" ></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                Freight Mode
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddFreightMode" runat="server" AutoPostBack="true" Width="40%" 
                                                   SelectedValue='<%#Eval("lMode") %>' OnSelectedIndexChanged="ddFreightMode_SelectedIndexChanged" TabIndex="2">
                                                    <asp:ListItem Text="Air" Value="1" ></asp:ListItem>
                                                    <asp:ListItem Text="Sea" Value="2" ></asp:ListItem>
                                                    <asp:ListItem Text="Breakbulk" Value="3" ></asp:ListItem>
                                                </asp:DropDownList>
                                                <%--<%#Eval("ModeName")%>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Customer
                                                <asp:RequiredFieldValidator ID="RFVCustName" runat="server" ValidationGroup="valUpdEnquiry" SetFocusOnError="true"
                                                    ControlToValidate="txtCustomer" Text="Required" ErrorMessage="Please Enter Customer Name" InitialValue=""> </asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCustomer" runat="server" Text='<%#Eval("Customer") %>' MaxLength="100" TabIndex="2"></asp:TextBox>
                                                
                                                <div id="divwidthCust">
                                                    </div>
                                                <AjaxToolkit:AutoCompleteExtender ID="CustomerExtender" runat="server" TargetControlID="txtCustomer"
                                                    CompletionListElementID="divwidthCust" ServicePath="../WebService/FreightCustomerAutoComplete.asmx"
                                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCust" 
                                                    ContextKey="4317" UseContextKey="True" CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight" FirstRowSelected="true">
                                                </AjaxToolkit:AutoCompleteExtender>
                                            </td>
                                            <td>
                                                Ref No/Email Ref
                                                <asp:RequiredFieldValidator ID="RFCRefNo" runat="server" ValidationGroup="valUpdEnquiry" SetFocusOnError="true"
                                                    ControlToValidate="txtCustRefNo" Text="Required" ErrorMessage="Please Enter Ref No" InitialValue=""> </asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCustRefNo" runat="server" Text='<%#Eval("CustRefNo") %>' MaxLength="400" TabIndex="3"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Shipper
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtShipper" runat="server" Text='<%#Eval("Shipper") %>' TabIndex="4"></asp:TextBox>
                                                <div id="divwidthShipper">
                                                </div>
                                                <AjaxToolkit:AutoCompleteExtender ID="AutoCompleteShipper" runat="server" TargetControlID="txtShipper"
                                                    CompletionListElementID="divwidthShipper" ServicePath="~/WebService/FreightShipperAutoComplete.asmx"
                                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthShipper" 
                                                    ContextKey="5556" UseContextKey="True" CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight" FirstRowSelected="true">
                                                </AjaxToolkit:AutoCompleteExtender>
                                            </td>
                                            <td>
                                                Consignee
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtConsignee" runat="server" Text='<%#Eval("Consignee") %>' TabIndex="5"></asp:TextBox>
                                                <div id="divwidthConsignee">
                                                </div>
                                                <AjaxToolkit:AutoCompleteExtender ID="AutoCompleteConsignee" runat="server" TargetControlID="txtConsignee"
                                                    CompletionListElementID="divwidthConsignee" ServicePath="../WebService/FreightConsigneeAutoComplete.asmx"
                                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthConsignee" 
                                                    ContextKey="4317" UseContextKey="True" CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight" FirstRowSelected="true">
                                                </AjaxToolkit:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Country
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCountry" runat="server" Text='<%#Eval("CountryName") %>' TabIndex="6"></asp:TextBox>
                                                <asp:HiddenField ID="hdnCountryId" runat="server" Value='<%#Eval("CountryId") %>' />
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
                                            <td>
                                                Enquiry Value
                                            </td>
                                            <td>
                                                <asp:Label ID="lblEnqVal" runat="server" Text='<%# Eval("EnquiryValue")%>' ></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Port of Loading
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPortLoading" runat="server" Text='<%#Eval("LoadingPortName") %>' TabIndex="7"></asp:TextBox>
                                                
                                                <div id="divwidthLoadingPort">
                                                </div>
                                                <AjaxToolkit:AutoCompleteExtender ID="AutoCompletePortLoading" runat="server" TargetControlID="txtPortLoading"
                                                    CompletionListElementID="divwidthLoadingPort" ServicePath="../WebService/PortOfLoadingAutoComplete.asmx"
                                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthLoadingPort"
                                                    ContextKey="1267" UseContextKey="True" OnClientItemSelected="OnPortOfLoadingSelected"
                                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                                </AjaxToolkit:AutoCompleteExtender>
                                            </td>
                                            <td>
                                                Port of Discharged
                                                <asp:RequiredFieldValidator ID="RFVDischarged" runat="server" ValidationGroup="validateEnquiry" SetFocusOnError="true"
                                                    InitialValue="" ControlToValidate="txtPortOfDischarged" Text="*" ErrorMessage="Please Select Port Of Discharged"> </asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPortOfDischarged" runat="server" Text='<%#Eval("PortOfDischargedName") %>'
                                                    TabIndex="8"></asp:TextBox>
                                                <div id="divwidthDischargPort">
                                                </div>
                                                <AjaxToolkit:AutoCompleteExtender ID="AutoCompletePortOfDischarged" runat="server" TargetControlID="txtPortOfDischarged"
                                                    CompletionListElementID="divwidthDischargPort" ServicePath="../WebService/PortOfLoadingAutoComplete.asmx"
                                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthDischargPort"
                                                    ContextKey="7268" UseContextKey="True" OnClientItemSelected="OnPortOfDischargedSelected"
                                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight" 
                                                    DelimiterCharacters="" Enabled="True">
                                                </AjaxToolkit:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Terms
                                            </td>
                                            <td>
                                                <%--<asp:DropDownList ID="ddTerms" runat="server" TabIndex="9" SelectedValue='<%#Eval("TermsId") %>'>
                                                <asp:ListItem Value="1" Text="EXW"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="DDU"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="DDP"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="CIF"></asp:ListItem>
                                                <asp:ListItem Value="5" Text="FOB"></asp:ListItem>
                                                <asp:ListItem Value="6" Text="FCA"></asp:ListItem>
                                                <asp:ListItem Value="7" Text="FAS"></asp:ListItem>
                                                <asp:ListItem Value="8" Text="DAP"></asp:ListItem>
                                                </asp:DropDownList>--%>
                                                <asp:DropDownList ID="ddTerms" runat="server" DataSourceID="dataSourceTermsMS" TabIndex="9"
                                                   SelectedValue='<%#Eval("TermsId") %>' DataValueField="lid" DataTextField="sName" AppendDataBoundItems="true" Width="50%">
                                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="dataSourceTermsMS" runat="server" ConnectionString="<%$ ConnectionStrings:ConBsImport %>"
                                                    SelectCommand="FR_GetTermsMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                                            </td>
                                            <td>
                                                <%--Agent--%>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAgent" runat="server" Text='<%#Eval("AgentName") %>' TabIndex="10" Visible="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <asp:Panel ID="pnlSeaUpdate" runat="server" Visible="false">
                                        <tr>
                                            <td>
                                                Container 20"
                                                <asp:CompareValidator ID="CompValCon20" runat="server" ControlToValidate="txtCont20"
                                                    Operator="DataTypeCheck" SetFocusOnError="true" Type="Integer" ErrorMessage="Invalid Count Of 20"
                                                    Display="Dynamic" ValidationGroup="valUpdEnquiry"></asp:CompareValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCont20" runat="server" Text='<%#Eval("CountOf20") %>' TabIndex="11"></asp:TextBox>
                                            </td>
                                            <td>
                                                Container 40"
                                                <asp:CompareValidator ID="ComVal40" runat="server" ControlToValidate="txtCont40"
                                                    Operator="DataTypeCheck" SetFocusOnError="true" Type="Integer" ErrorMessage="Invalid Count Of 40"
                                                    Display="Dynamic" ValidationGroup="valUpdEnquiry"></asp:CompareValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCont40" runat="server" Text='<%#Eval("CountOf40") %>' TabIndex="12"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                LCL (CBM)
                                                <asp:CompareValidator ID="CompValLCL" runat="server" ControlToValidate="txtLCLVolume"
                                                    Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" ErrorMessage="Invalid Volume Of LCL"
                                                    Display="Dynamic" ValidationGroup="valUpdEnquiry"></asp:CompareValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLCLVolume" runat="server" Text='<%#Eval("LCLVolume") %>' TabIndex="13"></asp:TextBox>
                                            </td>
                                            <td>
                                                Container Type
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddContainerType" runat="server" SelectedValue='<%#BIND("ContainerType") %>' Width="80px" TabIndex="13">
                                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="FCL" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="LCL" Value="2"></asp:ListItem>
                                                    </asp:DropDownList>
                                                <asp:DropDownList ID="ddSubType" runat="server" SelectedValue='<%#BIND("ContainerSubType") %>' Width="85px" TabIndex="13">
                                                    <asp:ListItem Text="Sub Type" Value=""></asp:ListItem>
                                                    <asp:ListItem Text="GP" Value="GP"></asp:ListItem>
                                                    <asp:ListItem Text="HD" Value="HD"></asp:ListItem>
                                                    <asp:ListItem Text="HQ" Value="HQ"></asp:ListItem>
                                                    <asp:ListItem Text="OT" Value="OT"></asp:ListItem>
                                                    <asp:ListItem Text="FR" Value="FR"></asp:ListItem>
                                                    <asp:ListItem Text="FB" Value="FB"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlAirUpdate" runat="server" Visible="false">
                                        <tr>
                                            <td>
                                                No of Packages
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNoOfPkgs" runat="server" Text='<%#Eval("NoOfPackages") %>' TabIndex="11"></asp:TextBox>
                                                <asp:CompareValidator ID="CompValPackgs" runat="server" ControlToValidate="txtNoOfPkgs"
                                                    Operator="DataTypeCheck" SetFocusOnError="true" Type="Integer" ErrorMessage="Invalid No Of Packages"
                                                    Display="Dynamic" ValidationGroup="valUpdEnquiry"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        </asp:Panel>
                                        <tr>
                                            <td>
                                                Gross Weight (Kgs)
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGrossWeight" runat="server" Text='<%#Eval("GrossWeight") %>' TabIndex="14"></asp:TextBox>
                                                <asp:CompareValidator ID="ComValGrossWT" runat="server" ControlToValidate="txtGrossWeight"
                                                    Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" ErrorMessage="Invalid Gross Weight"
                                                    Display="Dynamic" ValidationGroup="valUpdEnquiry"></asp:CompareValidator>
                                            </td>
                                            <td>
                                                Chargeable Weight (Kgs)
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtChargWeight" runat="server" Text='<%#Eval("ChargeableWeight") %>' TabIndex="15"></asp:TextBox>
                                                <asp:CompareValidator ID="CompValChargeWeight" runat="server" ControlToValidate="txtChargWeight"
                                                    Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" ErrorMessage="Invalid Chargeable Weight"
                                                    Display="Dynamic" ValidationGroup="valUpdEnquiry"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Is Dangerous Goods ?
                                            </td>
                                            <td>
                                            <asp:RadioButtonList ID="rdlGoodsType" runat="server" SelectedValue='<%#Bind("IsDangerousGood") %>' TabIndex="16" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="True" Text="Yes"></asp:ListItem>
                                                <asp:ListItem Value="False" Text="No"></asp:ListItem>
                                            </asp:RadioButtonList> 
                                        </td>
                                            <td>
                                                Sales representative
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddSalesRep" runat="server" TabIndex="17"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Remarks
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtRemark" runat="server" Text='<%#Eval("Remarks") %>' MaxLength="800" TabIndex="18" TextMode="MultiLine" Width="80%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        
                                    </table>
                            </EditItemTemplate>
                        </asp:FormView>
                        </fieldset>
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
                                        <asp:FileUpload ID="fuDocument" runat="server" /><asp:Button ID="btnUpload" runat="server"
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
                                    <asp:TemplateField HeaderText="Remove">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnlRemoveDocument" runat="server" Text="Remove" CommandName="RemoveDocument"
                                                CommandArgument='<%#Eval("DocId") %>' OnClientClick="return confirm('Are you sure to remove document?');" ></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
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
                                    <asp:SessionParameter Name="JobType" SessionField="JobType" />
                                    <asp:SessionParameter Name="JobMode" SessionField="JobMode" />
                                    <%--<asp:ControlParameter ControlID="hdnTypeId" Name="JobType" PropertyName="Value" />--%>
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
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
                                                    <asp:ListBox ID="lbAgentCompany" runat="server" SelectionMode="Multiple" Width="100%" Height="120px" TabIndex="21">
                                           
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
                                        <asp:ListBox ID="lbEmployee" runat="server" SelectionMode="Multiple" Width="40%" Height="150px" TabIndex="21">
                                            <asp:ListItem Text="Dhaval Davada" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Devendra Donde" Value="313"></asp:ListItem>
                                            <asp:ListItem Text="Hemali Patel" Value="177"></asp:ListItem>
                                            <asp:ListItem Text="Manish Radhakrishnan" Value="185"></asp:ListItem>
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

                <AjaxToolkit:TabPanel ID="TabContainer" runat="server" HeaderText="Container">
                    <ContentTemplate>
                        <fieldset>
                                <legend>Add Container</legend>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                    <tr>
                                        <td>Container No
                                            <asp:RequiredFieldValidator ID="RFVContainer" runat="server" ControlToValidate="txtContainerNo"
                                                ValidationGroup="valContainer" SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Container No"
                                                Display="Dynamic">
                                            </asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtContainerNo" runat="server" MaxLength="11"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="REVContainer" runat="server" ControlToValidate="txtContainerNo"
                                                ValidationGroup="valContainer" SetFocusOnError="true" ErrorMessage="Enter 11 Digit Container No."
                                                Display="Dynamic" ValidationExpression="^[a-zA-Z0-9]{11}$"></asp:RegularExpressionValidator>
                                        </td>
                                        <td>Container Type
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddContainerType" runat="server">
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
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnAddContainer" Text="Add Container" OnClick="btnAddContainer_Click"
                                                ValidationGroup="valContainer" runat="server" />
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                            </fieldset>
                            <div>
                                <fieldset>
                                    <legend>Container Detail</legend>
                                    <asp:GridView ID="gvContainer" runat="server" AllowPaging="true" CssClass="table"
                                        PagerStyle-CssClass="pgr" AutoGenerateColumns="false" DataKeyNames="lid" Width="100%"
                                        PageSize="20" DataSourceID="DataSourceContainer" OnRowDataBound="gvContainer_RowDataBound"
                                        AllowSorting="true" OnRowUpdating="gvContainer_RowUpdating" OnRowDeleting="gvContainer_RowDeleting">
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
                                            <asp:CommandField ButtonType="Link" ShowEditButton="true" ShowDeleteButton="true"
                                                ValidationGroup="valGridContainer" />
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
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
            </AjaxToolkit:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
