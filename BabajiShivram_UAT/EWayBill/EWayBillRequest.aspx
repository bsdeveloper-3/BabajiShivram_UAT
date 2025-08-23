<%@ Page Title="Generate Eway Bill" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="EWayBillRequest.aspx.cs" Inherits="EWayBill_EWayBillRequest" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
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
        function OnJobSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnJobId').value = results.JobId;

            $("[id*=chkAllProduct]").live("click", function () {
            var chkHeader = $(this);
            var grid = $(this).closest("table");
            $("input[type=checkbox]", grid).each(function () {
                if (chkHeader.is(":checked")) {
                    $(this).attr("checked", "checked");
                    $("td", $(this).closest("tr")).addClass("selected");
                } else {
                    $(this).removeAttr("checked");
                    $("td", $(this).closest("tr")).removeClass("selected");
                }
            });
        });
        }

        function GridSelectAllColumn(spanChk) {
            var oItem = spanChk.children;
            var theBox = (spanChk.type == "checkbox") ? spanChk : spanChk.children.item[0];
            xState = theBox.checked;
            elm = theBox.form.elements;

            for (i = 0; i < elm.length; i++) {
                if (elm[i].type === 'checkbox' && elm[i].checked != xState)
                    elm[i].click();
            }
        }
        
    </script>
    <asp:UpdatePanel ID="upJobDetail" runat="server">
        <ContentTemplate>
        <div style="text-align:center;"> <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label></div>
            <asp:HiddenField ID="hdnJobId" runat="server" Value="0" />
            <asp:HiddenField ID="hdnTransMode" runat="server" Value="0" />
            <asp:HiddenField ID="hdnDeliveryTypeId" runat="server" Value="0" />
            <asp:HiddenField ID="hdnIsInvalidData" Value="0" runat="server" />
        <fieldset><legend>Babaji Job No / BOE Number</legend>
        <div class="m clear"></div>
            <div>
                <asp:TextBox ID="txtJobNumber" Width="160px" runat="server" ToolTip="Enter Job/BOE Number." CssClass="SearchTextbox" 
                    placeholder="Search Job" TabIndex="1" AutoPostBack="true" OnTextChanged="txtJobNumber_TextChanged"></asp:TextBox>
                
                <asp:Button ID="btnShowJob" Text="Show Job Detail" runat="server" OnClick="btnShowJob_Click" />
                    &nbsp;&nbsp;<span><b>No of Vehicle Placed</b></span> &nbsp;&nbsp;
                <asp:TextBox ID="txtVehicleCount" runat="server" Text="0" AutoPostBack="true" 
                    MaxLength="2" Width="50px" OnTextChanged="txtVehicleCount_TextChanged"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RFVVehicleCount" runat="server" InitialValue="0" ErrorMessage="Required"
                    ValidationGroup="Required1" ControlToValidate="txtVehicleCount" SetFocusOnError="true"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompValNumber" runat="server" ControlToValidate="txtVehicleCount"
                    Operator="DataTypeCheck" SetFocusOnError="true" Type="Integer" Text="Invalid Vehicle Count" ErrorMessage="Invalid Vehicle Count"
                    Display="Dynamic" ValidationGroup="Required1"></asp:CompareValidator>
                <asp:Button ID="btnAPIEWay" Text="Generate Eway Bill Number" CausesValidation="false" runat="server" OnClick="btnAPIEWay_Click" Visible="false" />
                <div id="divwidthJob" runat="server">
                </div>
                <cc1:AutoCompleteExtender ID="JobDetailExtender" runat="server" TargetControlID="txtJobNumber"
                    CompletionListElementID="divwidthJob" ServicePath="~/WebService/JobNumberAutoComplete.asmx"
                    ServiceMethod="GetJobListForDelivery" MinimumPrefixLength="2" BehaviorID="divwidthJob"
                    ContextKey="1" UseContextKey="True" OnClientItemSelected="OnJobSelected" 
                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                </cc1:AutoCompleteExtender>
            </div> 
            
            <div>
                <asp:LinkButton ID="lnkEWayHistory" runat="server" CausesValidation="false" />
                <cc1:ModalPopupExtender ID="ModalPopupHistory" runat="server" TargetControlID="lnkEWayHistory"
                        PopupControlID="Panel21">
                    </cc1:ModalPopupExtender>
                <asp:Panel ID="Panel21" Style="display: none" runat="server">
                    <fieldset>
                            <legend>Eway Bill History</legend>
                            <div class="header">
                                <div class="fleft">
                                    <asp:LinkButton ID="lnkPopupClose" runat="server" Text="Close History" OnClick="lnkPopupClose_Click"
                                        data-tooltip="&nbsp; Close History">
                                    </asp:LinkButton>
                                </div>
                                <div class="fright">
                                    <asp:ImageButton ID="imgPopupClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="lnkPopupClose_Click"
                                        ToolTip="Close" />
                                </div>
                            </div>
                            <div class="clear"></div>
                            <div id="Div1" runat="server" style="max-height: 550px; overflow: auto;">
                                <asp:GridView ID="gvEwayHistory" runat="server" CssClass="table" AutoGenerateColumns="false"
                                    AllowPaging="false" DataSourceID="DSEwayHistory" OnRowCommand="gvEwayHistory_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex +1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Eway Bill No">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEwayBill" runat="server" Text='<%#Eval("EWayBillNo") %>'
                                                    CommandName="PrintEwayBill" CommandArgument='<%#Eval("EWayBillNo") %>' ></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No"  />
                                        <asp:BoundField DataField="BillGenDate" HeaderText="Created Date" />
                                        <asp:BoundField DataField="ValidityDate" HeaderText="Validity Date" />
                                        <asp:BoundField DataField="StatusName" HeaderText="Status" />
                                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                </asp:Panel>
                <asp:SqlDataSource ID="DSEwayHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                     SelectCommand="TR_GetEWayByJobId" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hdnJobId" PropertyName="Value" Name="JobId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>           
        </fieldset>
        <fieldset><legend> Job Detail </legend>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
            <tr>
                <td>
                    <b> Tax Applicable </b>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblIGST" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" 
                            OnSelectedIndexChanged="rblIGST_SelectedIndexChanged" CausesValidation="false" >
                        <asp:ListItem Text="IGST" Value="1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="CGST/SGST" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td>
                    <b> Group By HSN </b>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblGroupHSN" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" 
                            OnSelectedIndexChanged="rblGroupHSN_SelectedIndexChanged" CausesValidation="false" >
                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                        <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    BS Job No.
                </td>
                <td>
                    <asp:Label ID="lblJobRefNo" runat="server"></asp:Label>
                </td>
                <td>
                    Delivery Type
                </td>
                <td>
                    <asp:Label ID="lblDeliveryType" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Total Container
                </td>
                <td>
                    <asp:Label ID="lblContainerCount" runat="server"></asp:Label>
                    <asp:Label ID="lblContainerType" runat="server"></asp:Label>
                    &nbsp;&nbsp;<asp:Label ID="lblNoOfInvoices" runat="server" Text="0"></asp:Label>
                    <span>Invoice</span> 
                </td>
                <td>
                    Packages
                </td>
                <td>
                    <asp:Label ID="lblNoOfPackages" runat="server"></asp:Label>
                    &nbsp;<asp:Label ID="lblPackageType" runat="server"></asp:Label>
                    &nbsp; & Weight (Kgs) <asp:Label ID="lblGrossWeight" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Description
                </td>
                <td>
                    <asp:Label ID="lblShortDesc" runat="server"></asp:Label>
                </td>
                <td>
                    Truck Request Date
                </td>
                <td>
                    <asp:Label ID="lblTruckReqDate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Port
                </td>
                <td>
                    <asp:Label ID="lblDeliveryFrom" runat="server"></asp:Label>
                </td>
                <td>
                    CFS
                </td>
                <td>
                    <asp:Label ID="lblCFS" runat="server"></asp:Label>
                </td>
            </tr>
            
            <tr>
                <td>Transportation By </td>
                <td>
                    <asp:Label ID="lblTransportationBy" runat="server"></asp:Label>
                </td>
                <td>
                    Transaction Type
                </td>
                <td>
                    <asp:DropDownList ID="ddTransactionType" runat="server" Enabled="false">
                        <asp:ListItem Text="Regular" Value="1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Bill To - Ship To" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Bill From - Dispatch From" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Combination of 2 and 3" Value="4"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        </fieldset>

        <fieldset><legend> Transaction Detail </legend>
            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
            <tr>
                <td>Transaction Type</td>
                <td>
                    <asp:DropDownList ID="ddTransType" runat="server" Width="120px" AutoPostBack="true"
                        OnSelectedIndexChanged="ddTransType_SelectedIndexChanged" Font-Bold="true">
                        <asp:ListItem Text="Inward" Value="I"></asp:ListItem>
                        <asp:ListItem Text="Outward" Value="O"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    Sub Type
                    <asp:RequiredFieldValidator ID="rfvSubType" runat="server" InitialValue="0" SetFocusOnError="true"
                        ControlToValidate="ddSubType" ErrorMessage="Required" ValidationGroup="Required1"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:DropDownList ID="ddSubType" runat="server" Font-Bold="true">
                        <asp:ListItem Value="1" Text="Supply"></asp:ListItem>
                        <asp:ListItem Value="2" Text="Import" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="9" Text="SKD/CKD/Lots"></asp:ListItem>
                        <asp:ListItem Value="6" Text="Job work Returns"></asp:ListItem>
                        <asp:ListItem Value="7" Text="Sales Return"></asp:ListItem>
                        <asp:ListItem Value="12" Text="Exhibition or Fairs"></asp:ListItem>
                        <asp:ListItem Value="5" Text="For Own Use"></asp:ListItem>
                        <asp:ListItem Value="8" Text="Others"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Document Type
                </td>
                <td>
                    <asp:DropDownList ID="ddDocType" runat="server" Font-Bold="true">
                        <asp:ListItem value="INV" Text="Tax Invoice"></asp:ListItem>
                        <asp:ListItem value="BIL" Text="Bill of Supply"></asp:ListItem>
                        <asp:ListItem value="BOE" Text="Bill of Entry" Selected="True"></asp:ListItem>
                        <asp:ListItem value="CHL" Text="Delivery Challan"></asp:ListItem>
                        <asp:ListItem value="CNT" Text="Credit Note"></asp:ListItem>
                        <asp:ListItem value="OTH" Text="Others"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    Document No
                </td>
                <td>
                    <asp:TextBox ID="txtBOENo" runat="server" MaxLength="100" Placeholder="Doc No" Width="120px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RFVDocNo" runat="server" InitialValue="" ControlToValidate="txtBOENo" 
                           ErrorMessage="Required" Text="Doc Number Required" ValidationGroup="Required1" Display="Dynamic"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    Transporter
                    <asp:RequiredFieldValidator ID="RFVTransporterName" runat="server" InitialValue="0" ControlToValidate="ddTrasnporter" 
                      Display="Dynamic" ErrorMessage="*" Text="Required" ValidationGroup="Required1" SetFocusOnError="true"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:DropDownList ID="ddTrasnporter" runat="server" Font-Bold="true">
                    <asp:ListItem Value="0" Text="--Select Transporter--" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="27AAACN1163G1ZR" Text="NAVBHARAT CLEARING AGENTS PVT.LTD."></asp:ListItem>
                    <asp:ListItem Value="27AAFFN5296A1ZA" Text="NAV JEEVAN AGENCY"></asp:ListItem>
                </asp:DropDownList>
                    <%--<asp:TextBox ID="txtTransporterName" Text="NAVBHARAT CLEARING AGENTS PVT.LTD." runat="server"></asp:TextBox>--%>
                </td>
                <td>
                  Document Date
                </td>
                <td>
                    <asp:TextBox ID="txtBOEDate" runat="server" Placeholder="DD/MM/YYYY" Width="100px" Enabled="false" ></asp:TextBox>
                    <asp:Image ID="imgDocDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                   <cc1:CalendarExtender ID="CalExtDocDate" runat="server" EnableViewState="False" FirstDayOfWeek="Sunday" 
                        Format="dd/MM/yyyy" TargetControlID="txtBOEDate" PopupButtonID="imgDocDate"></cc1:CalendarExtender>
                    <asp:RequiredFieldValidator ID="RFValDocDate" runat="server" InitialValue="" ControlToValidate="txtBOEDate" 
                           ErrorMessage="Doc Date Required" Text="Doc Date Required" ValidationGroup="Required1"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <%--<tr>
                <td>
                    E-Way User GSTIN
                    <asp:RequiredFieldValidator ID="RFVUserGSTIN" runat="server" InitialValue="" ControlToValidate="txtUserGSTIN" 
                      Display="Dynamic" ErrorMessage="*" Text="Required" ValidationGroup="Required1" SetFocusOnError="true"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtUserGSTIN" Text="27AAACN1163G1ZR" runat="server"></asp:TextBox>
                </td>
                <td></td>
                <td></td>
            </tr>--%>
           
        </table>
        </fieldset>

        <fieldset><legend>Bill From / Supplier</legend>
            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                <thead style="background-color:#337AB7; color:white;">
                    <th>SUPPLIER NAME</th>
                    <th>SUPPLIER GSTIN</th>
                    <th>SUPPLIER STATE</th>
                </thead>
                <tr style="text-align:center;">
                    <td>
                        <asp:TextBox ID="txtBillFromName" runat="server" placeholder="Supplier Name" Width="80%"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RFVBillFromGSTIN" runat="server" InitialValue="" ControlToValidate="txtBillFromGSTIN" 
                            ErrorMessage="*" Text="Required" ValidationGroup="Required1" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtBillFromGSTIN" runat="server" Text="URP" placeholder="Supplier GSTIN"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RFVBillFromState" runat="server" InitialValue="0" ControlToValidate="ddBillFromState" 
                            ErrorMessage="*" Text="Required" ValidationGroup="Required1" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        <asp:DropDownList ID="ddBillFromState" runat="server"></asp:DropDownList>
                    </td>
                </tr>
            </table>
        </fieldset>
        
        <fieldset><legend>Ship From / Dispatch From</legend>
            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                <thead style="background-color:#337AB7; color:white;">
                    <th>ADDRESS</th>
                    <th>PLACE</th>
                    <th>PIN CODE</th>
                    <th>STATE</th>
                </thead>
                <tr>
                    <td>
                        <asp:TextBox ID="txtDespatchFromAddr1" runat="server" placeholder="Address Line 1"></asp:TextBox>
                        <asp:TextBox ID="txtDespatchFromAddr2" runat="server" placeholder="Address Line 2"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDespatchFromPlace" runat="server" placeholder="Place"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RFVDespatchFromPin" runat="server" InitialValue="" ControlToValidate="txtDespatchFromPin" 
                            ErrorMessage="*" Text="Required" ValidationGroup="Required1" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtDespatchFromPin" runat="server" Text="999999" placeholder="Pin Code"></asp:TextBox>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddDespatchFromState" runat="server"></asp:DropDownList>
                    </td>
                </tr>
            </table>
        </fieldset>

        <fieldset><legend>Bill To / Consignee</legend>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
            <thead style="background-color:#337AB7; color:white;">
                <th>CONSIGNEE NAME</th>
                <th>CONSIGNEE GSTIN</th>
                <th>STATE</th>
            </thead>
            <tr style="text-align:center;">
                <td>
                    <asp:TextBox ID="txtBillToName" runat="server" placeholder="Consignee Name" Width="80%"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="RFVConsigneeGSTIN" runat="server" InitialValue="" ControlToValidate="txtBillToGSTIN" 
                           ErrorMessage="*" Text="Required" ValidationGroup="Required1" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox ID="txtBillToGSTIN" runat="server" placeholder="Consignee GSTIN"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="RFVState" runat="server" InitialValue="0" ControlToValidate="ddBillToState" 
                           ErrorMessage="*" Text="Required" ValidationGroup="Required1" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:DropDownList ID="ddBillToState" runat="server"></asp:DropDownList>
                </td>
            </tr>
        </table>
        </fieldset>
        <fieldset><legend>Ship To / Delivery Detail</legend>
            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                <thead style="background-color:#337AB7; color:white;">
                    <th>DISPATCH DATE</th>
                    <th>ADDRESS</th>
                    <th>CITY</th>
                    <th>PIN CODE</th>
                    <th>STATE</th>
                    <th>DISTANCE KM</th>
                </thead>
                <tr>
                    <td>
                        <asp:RequiredFieldValidator ID="RFVDispatchDate" runat="server" InitialValue="" ControlToValidate="txtTransportDate" 
                           ErrorMessage="*" Text="Required" ValidationGroup="Required1"></asp:RequiredFieldValidator>

                        <asp:TextBox ID="txtTransportDate" runat="server" Width="100px" placeholder="Dispatch Date"></asp:TextBox>
                        <cc1:CalendarExtender ID="CalTransportDate" runat="server" EnableViewState="False" FirstDayOfWeek="Sunday" 
                            Format="dd/MM/yyyy" TargetControlID="txtTransportDate"></cc1:CalendarExtender>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDeliveryAddress" runat="server" placeholder="Address" MaxLength="100"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDeliveryDestination" runat="server" Placeholder="City" MaxLength="50"></asp:TextBox>
                    </td>
                    <td> 
                       <asp:RequiredFieldValidator ID="RFVPinCode" runat="server" InitialValue="" ControlToValidate="txtPinCode" 
                           ErrorMessage="*" Text="Required" ValidationGroup="Required1"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtPinCode" runat="server" Width="80px" placeholder="Pin Code" MaxLength="6"></asp:TextBox>
                    </td>
                    <td>                       
                        <asp:RequiredFieldValidator ID="RFVStateTo" runat="server" InitialValue="0" ControlToValidate="ddState" 
                           ErrorMessage="*" Text="Required" ValidationGroup="Required1"></asp:RequiredFieldValidator>
                        <asp:DropDownList ID="ddState" runat="server"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RFVDistance" runat="server" InitialValue="" ControlToValidate="txtDistance" 
                           ErrorMessage="*" Text="Required" ValidationGroup="Required1"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompValDistance" runat="server" ControlToValidate="txtDistance"
                           Operator="DataTypeCheck" SetFocusOnError="true" Type="Integer" Text="Invalid Number" ErrorMessage="Invalid Number"
                           Display="Dynamic" ValidationGroup="Required1"></asp:CompareValidator>
                        <asp:TextBox ID="txtDistance" runat="server" Width="100px" MaxLength="4" placeholder="Distance KM"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </fieldset>

        <fieldset style="width:90%;"><legend>Vehicle No / Part B</legend>
            <asp:Label ID="lblErrorWizard" runat="server" CssClass="errorMsg" EnableViewState="false"></asp:Label>
            <asp:Wizard ID="wzDelivery" runat="server" DisplaySideBar="false" OnNextButtonClick="wzDelivery_NextButtonClick"
               OnPreviousButtonClick="wzDelivery_PreviousButtonClick"  OnFinishButtonClick="wzDelivery_FinishButtonClick" >
            <StartNavigationTemplate>
                <div style="float: left; padding-top: 3px; padding-left: 3px">
                <asp:Button ID="btnWizardNext" runat="server" Text="Next Vehicle" CommandName="MoveNext"
                    Width="100px" CausesValidation="true" ValidationGroup="Required1" />
                </div>
            </StartNavigationTemplate>
            <StepNavigationTemplate>
                <div style="float: left; padding-top: 3px; padding-left: 3px">
                <asp:Button ID="btnWizardPrev1" runat="server" Text="Prev Vehicle" CommandName="MovePrevious"
                    Width="100px" />
                <asp:Button ID="btnWizardNext1" runat="server" Text="Next Vehicle" CommandName="MoveNext"
                    Width="100px" ValidationGroup="Required1" />
                </div>
            </StepNavigationTemplate>
            <FinishNavigationTemplate>
                <div style="float: left; padding-top: 3px; padding-left: 3px">
                <asp:Button ID="btnWizardPrev2" runat="server" Text="Prev Vehicle" CommandName="MovePrevious"
                    Width="100px" />
                <%--<asp:Label ID="lblFinalMessage" Text="Review & Generate Eway Bill" runat="server"></asp:Label>--%>
                    
                </div>
            </FinishNavigationTemplate>
            <WizardSteps>
                <asp:WizardStep ID="WizardStep1" runat="server" Title="Vehicle 1" StepType="Start">
                <fieldset><legend>Vehicle 1</legend>
                   <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                       <tr>
                           <td>
                               Vehicle No
                           </td>
                            <td>
                                <asp:TextBox ID="txtVehicleNo1" runat="server" placeholder="Vehicle No" style="text-transform: uppercase"></asp:TextBox>
                                <a href="#" data-tooltip="Valid No: MH46AB1234  or MH46A1234">
                                <img src="../Images/info-icon.png" width="14px" height="14px" alt="Vehicle No Format" /></a>
                                <%--<asp:RegularExpressionValidator ID="REVVehicleNo1" runat="server" ControlToValidate="txtVehicleNo1"
                                    ValidationExpression="^[A-Z a-z]{2}[0-9]{2}[A-Z a-z]{1,2}[0-9]{3,4}$" ErrorMessage="Invalid No" ValidationGroup="Required1"></asp:RegularExpressionValidator>--%>
                            </td>
                           <td>
                                Transport Doc No
                            </td>
                           <td>
                               <asp:TextBox ID="txtTransDocNo1" runat="server" placeholder="Trans Doc No"></asp:TextBox>
                           </td>
                        </tr>
                    </table>
                </fieldset>
                <div id="divProduct1">
                    <fieldset><legend> Product Detail </legend>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" DataKeyNames="ProductId" CssClass="table" 
                        width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" CellPadding="4" AllowPaging="True" PageSize="1000">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkAllProduct" align="center" Text="All" ToolTip="Check All"
                                        runat="server" onclick="GridSelectAllColumn(this);" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk1" runat="server" ToolTip="Check"></asp:CheckBox>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="HSN" DataField="HSN" />
                            <asp:BoundField HeaderText="Unit" DataField="UnitOfProduct" />
                            <asp:TemplateField HeaderText="Avbl Qty">
                                <ItemTemplate>
                                    <asp:Label ID="lblQuantity" runat="server" Text='<%#Bind("Quantity") %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Dispatch Qty">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDispatchQuantity" runat="server" Text='<%#Bind("Quantity") %>' Width="80px"
                                       OnTextChanged="txtDispatchQuantity_TextChanged" AutoPostBack="true" ></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDescription" runat="server" Text='<%#Bind("Description") %>' Enabled="false" TextMode="MultiLine" Width="200px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Value" DataField="AssessableValue" />
                            <asp:BoundField HeaderText="Tax Rate" DataField="GSTDutyRate" />
                            <asp:BoundField HeaderText="CGST" DataField="CGSTAmount" DataFormatString="{0:F}" />
                            <asp:BoundField HeaderText="SGST" DataField="SGSTAmount" DataFormatString="{0:F}" />
                            <asp:BoundField HeaderText="IGST" DataField="IGSTAmount" DataFormatString="{0:F}" />
                            <asp:BoundField HeaderText="Cess" DataField="CessAmount"  DataFormatString="{0:F}" />
                        </Columns>
                    </asp:GridView>
                    </fieldset>
                </div>
                </asp:WizardStep>
                
                <asp:WizardStep ID="WizardStep2" runat="server" Title="Vehicle 2">
                   <fieldset><legend>Vehicle 2</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                       <tr>
                            <td>
                               Vehicle No
                           </td>
                            <td>
                                <asp:TextBox ID="txtVehicleNo2" runat="server" placeholder="Vehicle No" style="text-transform: uppercase"></asp:TextBox>
                                <a href="#" data-tooltip="Valid No: MH46AB1234  or MH46A1234">
                                <img src="../Images/info-icon.png" width="14px" height="14px" alt="Vehicle No Format" /></a>
                                <%--<asp:RegularExpressionValidator ID="REVVehicleNo2" runat="server" ControlToValidate="txtVehicleNo2"
                                    ValidationExpression="^[A-Z a-z]{2}[0-9]{2}[A-Z a-z]{1,2}[0-9]{3,4}$" ErrorMessage="Invalid No" ValidationGroup="Required1"></asp:RegularExpressionValidator>--%>
                            </td>
                           <td>
                                Transport Doc No
                            </td>
                           <td>
                               <asp:TextBox ID="txtTransDocNo2" runat="server" placeholder="Trans Doc No"></asp:TextBox>
                           </td>                         
                        </tr>
                    </table>
                    </fieldset>
                   <div id="divProduct2">
                    <fieldset><legend> Product Detail </legend>
                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="false" DataKeyNames="ProductId" CssClass="table" 
                        width="100%"  AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" CellPadding="4" AllowPaging="True" PageSize="1000">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkAllProduct" align="center" Text="All" ToolTip="Check All"
                                    runat="server" onclick="GridSelectAllColumn(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chk1" runat="server" ToolTip="Check"></asp:CheckBox>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                            
                        <asp:BoundField HeaderText="HSN" DataField="HSN" />
                        <asp:BoundField HeaderText="Unit" DataField="UnitOfProduct" />
                        <asp:TemplateField HeaderText="Avbl Qty">
                            <ItemTemplate>
                                <asp:Label ID="lblQuantity" runat="server" Text='<%#Bind("Quantity") %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Dispatch Qty">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDispatchQuantity" runat="server" Text='<%#Bind("Quantity") %>' Width="80px"
                                    OnTextChanged="txtDispatchQuantity_TextChanged" AutoPostBack="true" ></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDescription" runat="server" Text='<%#Bind("Description") %>' Enabled="false" TextMode="MultiLine" Width="200px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Value" DataField="AssessableValue" />
                        <asp:BoundField HeaderText="Tax Rate" DataField="GSTDutyRate" />
                        <asp:BoundField HeaderText="CGST" DataField="CGSTAmount" />
                        <asp:BoundField HeaderText="SGST" DataField="SGSTAmount" />
                        <asp:BoundField HeaderText="IGST" DataField="IGSTAmount" />
                        <asp:BoundField HeaderText="Cess" DataField="CessAmount" />
                    </Columns>
                    </asp:GridView>
                    </fieldset>
                   </div>
               </asp:WizardStep>
                
                <asp:WizardStep ID="WizardStep3" runat="server" Title="Vehicle 3">
                  <fieldset><legend>Vehicle 3</legend>
                     <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                       <tr>
                            <td>
                               Vehicle No
                           </td>
                            <td>
                                <asp:TextBox ID="txtVehicleNo3" runat="server" placeholder="Vehicle No" style="text-transform: uppercase"></asp:TextBox>
                                <a href="#" data-tooltip="Valid No: MH46AB1234  or MH46A1234">
                                <img src="../Images/info-icon.png" width="14px" height="14px" alt="Vehicle No Format" /></a>
                                <%--<asp:RegularExpressionValidator ID="REVVehicleNo3" runat="server" ControlToValidate="txtVehicleNo3"
                                    ValidationExpression="^[A-Z a-z]{2}[0-9]{2}[A-Z a-z]{1,2}[0-9]{3,4}$" ErrorMessage="Invalid No" ValidationGroup="Required1"></asp:RegularExpressionValidator>--%>
                            </td>
                           <td>
                                Transport Doc No
                           </td>
                           <td>
                               <asp:TextBox ID="txtTransDocNo3" runat="server" placeholder="Trans Doc No"></asp:TextBox>
                           </td>
                        </tr>
                    </table>
                   </fieldset>
                    <div id="divProduct3">
                    <fieldset><legend> Product Detail </legend>
                        <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="false" DataKeyNames="ProductId" CssClass="table" 
                            width="100%"  AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" CellPadding="4" AllowPaging="True" PageSize="1000">
                            <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkAllProduct" align="center" Text="All" ToolTip="Check All"
                                        runat="server" onclick="GridSelectAllColumn(this);" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk1" runat="server" ToolTip="Check"></asp:CheckBox>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:BoundField HeaderText="HSN" DataField="HSN" />
                            <asp:BoundField HeaderText="Unit" DataField="UnitOfProduct" />
                            <asp:TemplateField HeaderText="Avbl Qty">
                                <ItemTemplate>
                                    <asp:Label ID="lblQuantity" runat="server" Text='<%#Bind("Quantity") %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Dispatch Qty">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDispatchQuantity" runat="server" Text='<%#Bind("Quantity") %>' Width="80px"
                                       OnTextChanged="txtDispatchQuantity_TextChanged" AutoPostBack="true" ></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDescription" runat="server" Text='<%#Bind("Description") %>' Enabled="false" TextMode="MultiLine" Width="200px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Value" DataField="AssessableValue" />
                            <asp:BoundField HeaderText="Tax Rate" DataField="GSTDutyRate" />
                            <asp:BoundField HeaderText="CGST" DataField="CGSTAmount" />
                            <asp:BoundField HeaderText="SGST" DataField="SGSTAmount" />
                            <asp:BoundField HeaderText="IGST" DataField="IGSTAmount" />
                            <asp:BoundField HeaderText="Cess" DataField="CessAmount" />
                        </Columns>
                        </asp:GridView>
                    </fieldset>
                   </div>
               </asp:WizardStep>
                
                <asp:WizardStep ID="WizardStep4" runat="server" Title="Vehicle 4">
                   <fieldset><legend>Vehicle 4</legend>
                     <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                       <tr>
                            <td>
                               Vehicle No
                           </td>
                            <td>
                                <asp:TextBox ID="txtVehicleNo4" runat="server" placeholder="Vehicle No" style="text-transform: uppercase"></asp:TextBox>
                                <a href="#" data-tooltip="Valid No: MH46AB1234  or MH46A1234">
                                <img src="../Images/info-icon.png" width="14px" height="14px" alt="Vehicle No Format" /></a>
                                <%--<asp:RegularExpressionValidator ID="REVVehicleNo4" runat="server" ControlToValidate="txtVehicleNo4"
                                    ValidationExpression="^[A-Z a-z]{2}[0-9]{2}[A-Z a-z]{1,2}[0-9]{3,4}$" ErrorMessage="Invalid No" ValidationGroup="Required1"></asp:RegularExpressionValidator>--%>
                            </td>
                           <td>
                                Transport Doc No
                           </td>
                           <td>
                               <asp:TextBox ID="txtTransDocNo4" runat="server" placeholder="Trans Doc No"></asp:TextBox>
                           </td>
                        </tr>
                    </table>
                   </fieldset>
                    <div id="divProduct4">
                    <fieldset><legend> Product Detail </legend>
                        <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="false" DataKeyNames="ProductId" CssClass="table" 
                            width="100%"  AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" CellPadding="4" AllowPaging="True" PageSize="1000">
                            <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkAllProduct" align="center" Text="All" ToolTip="Check All"
                                        runat="server" onclick="GridSelectAllColumn(this);" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk1" runat="server" ToolTip="Check"></asp:CheckBox>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:BoundField HeaderText="HSN" DataField="HSN" />
                            <asp:BoundField HeaderText="Unit" DataField="UnitOfProduct" />
                            <asp:TemplateField HeaderText="Avbl Qty">
                                <ItemTemplate>
                                    <asp:Label ID="lblQuantity" runat="server" Text='<%#Bind("Quantity") %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Dispatch Qty">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDispatchQuantity" runat="server" Text='<%#Bind("Quantity") %>' Width="80px"
                                       OnTextChanged="txtDispatchQuantity_TextChanged" AutoPostBack="true" ></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDescription" runat="server" Text='<%#Bind("Description") %>' Enabled="false" TextMode="MultiLine" Width="200px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Value" DataField="AssessableValue" />
                            <asp:BoundField HeaderText="Tax Rate" DataField="GSTDutyRate" />
                            <asp:BoundField HeaderText="CGST" DataField="CGSTAmount" />
                            <asp:BoundField HeaderText="SGST" DataField="SGSTAmount" />
                            <asp:BoundField HeaderText="IGST" DataField="IGSTAmount" />
                            <asp:BoundField HeaderText="Cess" DataField="CessAmount" />
                        </Columns>
                        </asp:GridView>
                    </fieldset>
                   </div>
               </asp:WizardStep>
                
                <asp:WizardStep ID="WizardStep5" runat="server" Title="Vehicle 5">
                   <fieldset><legend>Vehicle 5</legend>
                     <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                       <tr>
                            <td>
                               Vehicle No
                           </td>
                            <td>
                                <asp:TextBox ID="txtVehicleNo5" runat="server" placeholder="Vehicle No" style="text-transform: uppercase"></asp:TextBox>
                                <a href="#" data-tooltip="Valid No: MH46AB1234  or MH46A1234">
                                <img src="../Images/info-icon.png" width="14px" height="14px" alt="Vehicle No Format" /></a>
                                <%--<asp:RegularExpressionValidator ID="REVVehicleNo5" runat="server" ControlToValidate="txtVehicleNo5"
                                    ValidationExpression="^[A-Z a-z]{2}[0-9]{2}[A-Z a-z]{1,2}[0-9]{3,4}$" ErrorMessage="Invalid No" ValidationGroup="Required1"></asp:RegularExpressionValidator>--%>
                            </td>
                           <td>
                               Transport Doc No
                           </td>
                           <td>
                               <asp:TextBox ID="txtTransDocNo5" runat="server" placeholder="Trans Doc No"></asp:TextBox>
                           </td>
                        </tr>
                    </table>
                    </fieldset>
                    <div id="divProduct5">
                    <fieldset><legend> Product Detail </legend>
                    <asp:GridView ID="GridView5" runat="server" AutoGenerateColumns="false" DataKeyNames="ProductId" CssClass="table" 
                        width="100%"  AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" CellPadding="4" AllowPaging="True" PageSize="1000">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkAllProduct" align="center" Text="All" ToolTip="Check All"
                                        runat="server" onclick="GridSelectAllColumn(this);" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk1" runat="server" ToolTip="Check"></asp:CheckBox>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:BoundField HeaderText="HSN" DataField="HSN" />
                            <asp:BoundField HeaderText="Unit" DataField="UnitOfProduct" />
                            <asp:TemplateField HeaderText="Avbl Qty">
                                <ItemTemplate>
                                    <asp:Label ID="lblQuantity" runat="server" Text='<%#Bind("Quantity") %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Dispatch Qty">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDispatchQuantity" runat="server" Text='<%#Bind("Quantity") %>' Width="80px"
                                       OnTextChanged="txtDispatchQuantity_TextChanged" AutoPostBack="true" ></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDescription" runat="server" Text='<%#Bind("Description") %>' Enabled="false" TextMode="MultiLine" Width="200px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Value" DataField="AssessableValue" />
                            <asp:BoundField HeaderText="Tax Rate" DataField="GSTDutyRate" />
                            <asp:BoundField HeaderText="CGST" DataField="CGSTAmount" />
                            <asp:BoundField HeaderText="SGST" DataField="SGSTAmount" />
                            <asp:BoundField HeaderText="IGST" DataField="IGSTAmount" />
                            <asp:BoundField HeaderText="Cess" DataField="CessAmount" />
                        </Columns>
                    </asp:GridView>
                </fieldset>
                   </div>
               </asp:WizardStep>
                
                <asp:WizardStep ID="WizardStep6" runat="server" Title="Summary" StepType="Step">
                   <fieldset><legend>Vehicle 6</legend>
                     <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                       <tr>
                            <td>
                               Vehicle No
                           </td>
                            <td>
                                <asp:TextBox ID="txtVehicleNo6" runat="server" placeholder="Vehicle No" style="text-transform: uppercase"></asp:TextBox>
                                <a href="#" data-tooltip="Valid No: MH46AB1234  or MH46A1234">
                                <img src="../Images/info-icon.png" width="14px" height="14px" alt="Vehicle No Format" /></a>
                                <%--<asp:RegularExpressionValidator ID="REVVehicleNo6" runat="server" ControlToValidate="txtVehicleNo6"
                                    ValidationExpression="^[A-Z a-z]{2}[0-9]{2}[A-Z a-z]{1,2}[0-9]{3,4}$" ErrorMessage="Invalid No" ValidationGroup="Required1"></asp:RegularExpressionValidator>--%>
                            </td>
                           <td>
                               Transport Doc No
                           </td>
                           <td>
                               <asp:TextBox ID="txtTransDocNo6" runat="server" placeholder="Trans Doc No"></asp:TextBox>
                           </td>
                        </tr>
                    </table>
                     </fieldset>
                       <div id="divProduct6">
                    <fieldset><legend> Product Detail </legend>
                    <asp:GridView ID="GridView6" runat="server" AutoGenerateColumns="false" DataKeyNames="ProductId" CssClass="table" 
                        width="100%"  AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" CellPadding="4" AllowPaging="True" PageSize="1000">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkAllProduct" align="center" Text="All" ToolTip="Check All"
                                        runat="server" onclick="GridSelectAllColumn(this);" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk1" runat="server" ToolTip="Check"></asp:CheckBox>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:BoundField HeaderText="HSN" DataField="HSN" />
                            <asp:BoundField HeaderText="Unit" DataField="UnitOfProduct" />
                            <asp:TemplateField HeaderText="Avbl Qty">
                                <ItemTemplate>
                                    <asp:Label ID="lblQuantity" runat="server" Text='<%#Bind("Quantity") %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Dispatch Qty">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDispatchQuantity" runat="server" Text='<%#Bind("Quantity") %>' Width="80px"
                                       OnTextChanged="txtDispatchQuantity_TextChanged" AutoPostBack="true" ></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDescription" runat="server" Text='<%#Bind("Description") %>' Enabled="false" TextMode="MultiLine" Width="200px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Value" DataField="AssessableValue" />
                            <asp:BoundField HeaderText="Tax Rate" DataField="GSTDutyRate" />
                            <asp:BoundField HeaderText="CGST" DataField="CGSTAmount" />
                            <asp:BoundField HeaderText="SGST" DataField="SGSTAmount" />
                            <asp:BoundField HeaderText="IGST" DataField="IGSTAmount" />
                            <asp:BoundField HeaderText="Cess" DataField="CessAmount" />
                        </Columns>
                    </asp:GridView>
                </fieldset>
                    </div>
               </asp:WizardStep>
                
                <asp:WizardStep ID="WizardStep7" runat="server" Title="Summary" StepType="Step">
                    <fieldset><legend>Vehicle 7</legend>
                     <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                       <tr>
                           <td>
                               Vehicle No
                           </td>
                            <td>
                                <asp:TextBox ID="txtVehicleNo7" runat="server" placeholder="Vehicle No" style="text-transform: uppercase"></asp:TextBox>
                                <a href="#" data-tooltip="Valid No: MH46AB1234  or MH46A1234">
                                <img src="../Images/info-icon.png" width="14px" height="14px" alt="Vehicle No Format" /></a>
                                <%--<asp:RegularExpressionValidator ID="REVVehicleNo7" runat="server" ControlToValidate="txtVehicleNo7"
                                    ValidationExpression="^[A-Z a-z]{2}[0-9]{2}[A-Z a-z]{1,2}[0-9]{3,4}$" ErrorMessage="Invalid No" ValidationGroup="Required1"></asp:RegularExpressionValidator>--%>
                            </td>
                           <td>
                               Transport Doc No
                           </td>
                           <td>
                               <asp:TextBox ID="txtTransDocNo7" runat="server" placeholder="Trans Doc No"></asp:TextBox>
                           </td>
                        </tr>
                    </table>
                    </fieldset>
                     <div id="divProduct7">
                    <fieldset><legend> Product Detail </legend>
                    <asp:GridView ID="GridView7" runat="server" AutoGenerateColumns="false" DataKeyNames="ProductId" CssClass="table" 
                        width="100%"  AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" CellPadding="4" AllowPaging="True" PageSize="1000">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkAllProduct" align="center" Text="All" ToolTip="Check All"
                                        runat="server" onclick="GridSelectAllColumn(this);" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk1" runat="server" ToolTip="Check"></asp:CheckBox>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:BoundField HeaderText="HSN" DataField="HSN" />
                            <asp:BoundField HeaderText="Unit" DataField="UnitOfProduct" />
                            <asp:TemplateField HeaderText="Avbl Qty">
                                <ItemTemplate>
                                    <asp:Label ID="lblQuantity" runat="server" Text='<%#Bind("Quantity") %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Dispatch Qty">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDispatchQuantity" runat="server" Text='<%#Bind("Quantity") %>' Width="80px"
                                       OnTextChanged="txtDispatchQuantity_TextChanged" AutoPostBack="true" ></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDescription" runat="server" Text='<%#Bind("Description") %>' Enabled="false" TextMode="MultiLine" Width="200px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Value" DataField="AssessableValue" />
                            <asp:BoundField HeaderText="Tax Rate" DataField="GSTDutyRate" />
                            <asp:BoundField HeaderText="CGST" DataField="CGSTAmount" />
                            <asp:BoundField HeaderText="SGST" DataField="SGSTAmount" />
                            <asp:BoundField HeaderText="IGST" DataField="IGSTAmount" />
                            <asp:BoundField HeaderText="Cess" DataField="CessAmount" />
                        </Columns>
                    </asp:GridView>
                </fieldset>
                    </div>
               </asp:WizardStep>
                
                <asp:WizardStep ID="WizardStep8" runat="server" Title="Summary" StepType="Step">
                    <fieldset><legend>Vehicle 8</legend>
                     <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                       <tr>
                            <td>
                               Vehicle No
                           </td>
                            <td>
                                <asp:TextBox ID="txtVehicleNo8" runat="server" placeholder="Vehicle No" style="text-transform: uppercase"></asp:TextBox>
                                <a href="#" data-tooltip="Valid No: MH46AB1234  or MH46A1234">
                                <img src="../Images/info-icon.png" width="14px" height="14px" alt="Vehicle No Format" /></a>
                                <%--<asp:RegularExpressionValidator ID="REVVehicleNo8" runat="server" ControlToValidate="txtVehicleNo8"
                                    ValidationExpression="^[A-Z a-z]{2}[0-9]{2}[A-Z a-z]{1,2}[0-9]{3,4}$" ErrorMessage="Invalid No" ValidationGroup="Required1"></asp:RegularExpressionValidator>--%>
                            </td>
                           <td>
                               Transport Doc No
                           </td>
                           <td>
                               <asp:TextBox ID="txtTransDocNo8" runat="server" placeholder="Trans Doc No"></asp:TextBox>
                           </td>
                        </tr>
                    </table>
                     </fieldset>
                    <div id="divProduct8">
                    <fieldset><legend> Product Detail </legend>
                    <asp:GridView ID="GridView8" runat="server" AutoGenerateColumns="false" DataKeyNames="ProductId" CssClass="table" 
                        width="100%"  AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" CellPadding="4" AllowPaging="True" PageSize="1000">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkAllProduct" align="center" Text="All" ToolTip="Check All"
                                        runat="server" onclick="GridSelectAllColumn(this);" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk1" runat="server" ToolTip="Check"></asp:CheckBox>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:BoundField HeaderText="HSN" DataField="HSN" />
                            <asp:BoundField HeaderText="Unit" DataField="UnitOfProduct" />
                            <asp:TemplateField HeaderText="Avbl Qty">
                                <ItemTemplate>
                                    <asp:Label ID="lblQuantity" runat="server" Text='<%#Bind("Quantity") %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Dispatch Qty">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDispatchQuantity" runat="server" Text='<%#Bind("Quantity") %>' Width="80px"
                                       OnTextChanged="txtDispatchQuantity_TextChanged" AutoPostBack="true" ></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDescription" runat="server" Text='<%#Bind("Description") %>' Enabled="false" TextMode="MultiLine" Width="200px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Value" DataField="AssessableValue" />
                            <asp:BoundField HeaderText="Tax Rate" DataField="GSTDutyRate" />
                            <asp:BoundField HeaderText="CGST" DataField="CGSTAmount" />
                            <asp:BoundField HeaderText="SGST" DataField="SGSTAmount" />
                            <asp:BoundField HeaderText="IGST" DataField="IGSTAmount" />
                            <asp:BoundField HeaderText="Cess" DataField="CessAmount" />
                        </Columns>
                    </asp:GridView>
                </fieldset>
                    </div>
               </asp:WizardStep>
                
                <asp:WizardStep ID="WizardStep9" runat="server" Title="Summary" StepType="Step">
                    <fieldset><legend>Vehicle 9</legend>
                     <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                       <tr>
                           <td>
                               Vehicle No
                           </td>
                            <td>
                                <asp:TextBox ID="txtVehicleNo9" runat="server" placeholder="Vehicle No" style="text-transform: uppercase"></asp:TextBox>
                                <a href="#" data-tooltip="Valid No: MH46AB1234  or MH46A1234">
                                <img src="../Images/info-icon.png" width="14px" height="14px" alt="Vehicle No Format" /></a>
                                <%--<asp:RegularExpressionValidator ID="REVVehicleNo9" runat="server" ControlToValidate="txtVehicleNo9"
                                    ValidationExpression="^[A-Z a-z]{2}[0-9]{2}[A-Z a-z]{1,2}[0-9]{3,4}$" ErrorMessage="Invalid No" ValidationGroup="Required1"></asp:RegularExpressionValidator>--%>
                            </td>
                           <td>
                               Transport Doc No
                           </td>
                           <td>
                               <asp:TextBox ID="txtTransDocNo9" runat="server" placeholder="Trans Doc No"></asp:TextBox>
                           </td>
                        </tr>
                    </table>
                     </fieldset>
                    <div id="divProduct9">
                    <fieldset><legend> Product Detail </legend>
                    <asp:GridView ID="GridView9" runat="server" AutoGenerateColumns="false" DataKeyNames="ProductId" CssClass="table" 
                        width="100%"  AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" CellPadding="4" AllowPaging="True" PageSize="1000">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkAllProduct" align="center" Text="All" ToolTip="Check All"
                                        runat="server" onclick="GridSelectAllColumn(this);" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk1" runat="server" ToolTip="Check"></asp:CheckBox>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:BoundField HeaderText="HSN" DataField="HSN" />
                            <asp:BoundField HeaderText="Unit" DataField="UnitOfProduct" />
                            <asp:TemplateField HeaderText="Avbl Qty">
                                <ItemTemplate>
                                    <asp:Label ID="lblQuantity" runat="server" Text='<%#Bind("Quantity") %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Dispatch Qty">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDispatchQuantity" runat="server" Text='<%#Bind("Quantity") %>' Width="80px"
                                       OnTextChanged="txtDispatchQuantity_TextChanged" AutoPostBack="true" ></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDescription" runat="server" Text='<%#Bind("Description") %>' Enabled="false" TextMode="MultiLine" Width="200px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Value" DataField="AssessableValue" />
                            <asp:BoundField HeaderText="Tax Rate" DataField="GSTDutyRate" />
                            <asp:BoundField HeaderText="CGST" DataField="CGSTAmount" />
                            <asp:BoundField HeaderText="SGST" DataField="SGSTAmount" />
                            <asp:BoundField HeaderText="IGST" DataField="IGSTAmount" />
                            <asp:BoundField HeaderText="Cess" DataField="CessAmount" />
                        </Columns>
                    </asp:GridView>
                    </fieldset>
                    </div>
               </asp:WizardStep>
                
                <asp:WizardStep ID="WizardStep10" runat="server" Title="Summary" StepType="Step">
                    <fieldset><legend>Vehicle 10</legend>
                     <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                       <tr>
                            <td>
                               Vehicle No
                            </td>
                            <td>
                                <asp:TextBox ID="txtVehicleNo10" runat="server" placeholder="Vehicle No" style="text-transform: uppercase"></asp:TextBox>
                                <a href="#" data-tooltip="Valid No: MH46AB1234  or MH46A1234">
                                <img src="../Images/info-icon.png" width="14px" height="14px" alt="Vehicle No Format" /></a>
                                <%--<asp:RegularExpressionValidator ID="REVVehicleNo10" runat="server" ControlToValidate="txtVehicleNo10"
                                    ValidationExpression="^[A-Z a-z]{2}[0-9]{2}[A-Z a-z]{1,2}[0-9]{3,4}$" ErrorMessage="Invalid No" ValidationGroup="Required1"></asp:RegularExpressionValidator>--%>
                            </td>
                            <td>
                               Transport Doc No
                            </td>
                            <td>
                               <asp:TextBox ID="txtTransDocNo10" runat="server" placeholder="Trans Doc No"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                     </fieldset>
                    <div id="divProduct10">
                    <fieldset><legend> Product Detail </legend>
                    <asp:GridView ID="GridView10" runat="server" AutoGenerateColumns="false" DataKeyNames="ProductId" CssClass="table"  
                        width="100%"  AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" CellPadding="4" AllowPaging="True" PageSize="1000">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkAllProduct" align="center" Text="All" ToolTip="Check All"
                                        runat="server" onclick="GridSelectAllColumn(this);" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk1" runat="server" ToolTip="Check"></asp:CheckBox>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:BoundField HeaderText="HSN" DataField="HSN" />
                            <asp:BoundField HeaderText="Unit" DataField="UnitOfProduct" />
                            <asp:TemplateField HeaderText="Avbl Qty">
                                <ItemTemplate>
                                    <asp:Label ID="lblQuantity" runat="server" Text='<%#Bind("Quantity") %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Dispatch Qty">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDispatchQuantity" runat="server" Text='<%#Bind("Quantity") %>' Width="80px"
                                       OnTextChanged="txtDispatchQuantity_TextChanged" AutoPostBack="true" ></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDescription" runat="server" Text='<%#Bind("Description") %>' Enabled="false" TextMode="MultiLine" Width="200px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Value" DataField="AssessableValue" />
                            <asp:BoundField HeaderText="Tax Rate" DataField="GSTDutyRate" />
                            <asp:BoundField HeaderText="CGST" DataField="CGSTAmount" />
                            <asp:BoundField HeaderText="SGST" DataField="SGSTAmount" />
                            <asp:BoundField HeaderText="IGST" DataField="IGSTAmount" />
                            <asp:BoundField HeaderText="Cess" DataField="CessAmount" />
                        </Columns>
                    </asp:GridView>
                </fieldset>
                    </div>
               </asp:WizardStep>

                <asp:WizardStep ID="WizardStepFinish" runat="server" Title="Summary" StepType="Finish" AllowReturn="true">
                    <div class="content">Review & Generate Eway Bill</div>
                </asp:WizardStep>
            </WizardSteps>
            </asp:Wizard>

            <div id="divDatasource">
            <asp:SqlDataSource ID="DataSourceBalance" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="GetConsolidateBalance" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="JobIDList" SessionField="ConsolidateJob" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="DataSourcePreConsolidate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetBalanceByConsolidateId" SelectCommandType="StoredProcedure" EnableCaching="false" >
                <SelectParameters>
                    <asp:SessionParameter Name="ConsolidateId" SessionField="ConsolidateId" />
                    <asp:SessionParameter Name="JobIDList" SessionField="ConsolidateJob" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
        </fieldset>
        
        <div style="overflow:auto;">
            <fieldset><legend> EWay Bill Detail </legend>
                <asp:GridView ID="gvProductDispatch" runat="server" AutoGenerateColumns="false" DataKeyNames="ProductId,VehicleID" CssClass="table" 
                    width="100%"  AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" CellPadding="4" AllowPaging="True" PageSize="1000">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Vehicle No" DataField="Vehicle No" /> 
                        <%--<asp:BoundField HeaderText="Trans Date" DataField="Trans Date" /> --%>
                        <asp:BoundField HeaderText="HSN" DataField="HSN" /> 
                        <asp:BoundField HeaderText="Unit" DataField="Unit" /> 
                        <asp:BoundField HeaderText="Qty" DataField="Qty" /> 
                        <asp:BoundField HeaderText="Value" DataField="AssessableValue" /> 
                        <asp:BoundField HeaderText="Product" DataField="Product" /> 
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <asp:TextBox ID="txtEwayDescription" Text='<%# BIND("Description")%>' Enabled="false" runat="server" TextMode="MultiLine" Width="200px" ></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Tax Rate" DataField="GSTDutyRate" /> 
                        <asp:BoundField HeaderText="CGST Amt" DataField="CGSTAmount" /> 
                        <asp:BoundField HeaderText="SGST Amt" DataField="SGSTAmount" /> 
                        <asp:BoundField HeaderText="IGST Amt" DataField="IGSTAmount" /> 
                        <asp:BoundField HeaderText="Cess Amt" DataField="CessAmount" /> 
                        <%--<asp:BoundField HeaderText="Doc No" DataField="Doc No" /> 
                        <asp:BoundField HeaderText="Doc Date" DataField="Doc Date" /> --%>
                        <%--<asp:BoundField HeaderText="TO_GSTIN" DataField="TO_GSTIN" /> --%>
                        <%--<asp:BoundField HeaderText="To_Address1" DataField="To_Address1" /> 
                        <asp:BoundField HeaderText="To_Place" DataField="To_Place" /> --%>
                        <%--<asp:BoundField HeaderText="To_Pin_Code	" DataField="To_Pin_Code" /> --%>
                        <%--<asp:BoundField HeaderText="To_State" DataField="To_State" /> 
                        <asp:BoundField HeaderText="Distance km" DataField="Distance km" /> --%>
                        <asp:BoundField HeaderText="Trans Name" DataField="Trans Name" /> 
                        <asp:BoundField HeaderText="Trans ID" DataField="Trans ID" /> 
                        <asp:BoundField HeaderText="Trans Doc No" DataField="Trans Doc No" /> 
                    </Columns>
                </asp:GridView>
            </fieldset>
        </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

