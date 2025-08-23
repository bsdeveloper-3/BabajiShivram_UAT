<%@ Page Title="Bill Tracking" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="TransBillTracking.aspx.cs" Inherits="Transport_TransBillTracking" EnableEventValidation="false" Culture="en-GB"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <style type="text/css">
        .form-control {
            border-top: 1px solid black;
            border-right: 1px solid black;
            border-bottom: 1px solid black;
            border-left: 1px solid black;
            border-radius: 3px;
            font-size: 9pt;
            padding: 2px 5px;
            margin-right: 5px;
            margin-top: 5px;
            font-family: Arial;
            padding-top: 5px;
            padding-bottom: 5px;
        }

        .modalBackground {
            background-color: black;
            filter: alpha(opacity=90);
            opacity: 0.5;
        }
    </style>
    <script type="text/javascript">
        function ActiveTabChanged12() {
            /* Clear Error Message on Tab change Event */
            document.getElementById('<%=lblError.ClientID%>').innerHTML = '';
            document.getElementById('<%=lblError.ClientID%>').className = '';
        }
    </script>
    <div>
        <asp:HiddenField ID="hdnPageValid" runat="server" />
        <asp:HiddenField ID="hdnFundRequestId" runat="server" Value="0" />
    </div>
    <div>
        <div align="center">
            <asp:Label ID="lblError" runat="server"></asp:Label>
            <asp:HiddenField ID="hdnNewPaymentLid" runat="server" Value="0" />
            <asp:HiddenField ID="hdnJobId" runat="server" Value="0" />
            <asp:ValidationSummary ID="vsPopup_FundRequest" runat="server" ShowMessageBox="True"
                ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" />
        </div>
    </div>
    <asp:UpdatePanel ID="upnlBillTracking" runat="server">
        <ContentTemplate>
            <cc1:TabContainer runat="server" ID="TabRequestRecd" ActiveTabIndex="0" CssClass="Tab"
                Width="100%" OnClientActiveTabChanged="ActiveTabChanged12" AutoPostBack="true">
                <cc1:TabPanel runat="server" ID="TabPanelNormalJob" TabIndex="0" HeaderText="Request">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Transport Request Detail</legend>
                            <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>Ref No.
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTRRefNo" runat="server" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td>Job No.
                                    </td>
                                    <td>
                                        <asp:Label ID="lblJobNo" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Truck Request Date
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTruckRequestDate" runat="server"></asp:Label>
                                    </td>
                                    <td>Customer Name
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCustName" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Vehicle Place Require Date
                                    </td>
                                    <td>
                                        <asp:Label ID="lblVehiclePlaceDate" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDispatch_Title" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDispatch_Value" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Location
                                    </td>
                                    <td>
                                        <asp:Label ID="lblLocationFrom" runat="server"></asp:Label>
                                    </td>
                                    <td>Destination
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDestination" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Dimension</td>
                                    <td>
                                        <asp:Label ID="lblDimension" runat="server"></asp:Label></td>
                                    <td>Gross Weight (Kgs)
                                    </td>
                                    <td>
                                        <asp:Label ID="lblGrossWeight" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Cont 20"
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCon20" runat="server"></asp:Label>
                                    </td>
                                    <td>Cont 40"
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCon40" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        PickUp Address
                                    </td>
                                    <td>
                                         <asp:Label ID="lblPickAdd" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        Drop Address
                                    </td>
                                    <td>
                                          <asp:Label ID="lblDropAdd" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Address Details
                                    </td>
                                    <td>
                                        Pincode&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; City&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp State
                                    </td>
                                    <td> Address Details
                                    </td>
                                    <td>
                                        Pincode&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;City &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp State
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        </td>
                                    <td>
                                         <asp:Label ID="lblpickPincode" runat="server">  </asp:Label>
                                          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
                                          <asp:Label ID="lblpickCity" runat="server"></asp:Label>
                                          &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
                                          <asp:Label ID="lblpickState" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        </td>
                                    <td>
                                          <asp:Label ID="lblDropPincode" runat="server"></asp:Label> 
                                          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                                          <asp:Label ID="lblDropCity" runat="server"> </asp:Label> 
                                          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
                                          <asp:Label ID="lblDropState" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblDelExportType_Title" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDelExportType_Value" runat="server"></asp:Label>
                                    </td>
                                    <td>Packing List Documents</td>
                                    <td>
                                        <asp:ImageButton ID="imgbtnPackingList" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20"
                                            ToolTip="Click to view documents." OnClick="imgbtnPackingList_Click" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel runat="server" ID="TabPanelVehicle" TabIndex="1" HeaderText="Vehicle">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Vehicle Rate Detail</legend>
                            <asp:Button ID="btnSaveRate" runat="server" Text="Save Detail" OnClick="btnSaveRate_Click" TabIndex="15" />
                            <asp:Button ID="btnFundRequest" runat="server" Text="Send Fund Request" OnClick="btnFundRequest_Click" TabIndex="16" />
                            <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>Transporter
                                        <asp:RequiredFieldValidator ID="rfvTransporter" runat="server" ControlToValidate="ddlTransporter" InitialValue="0"
                                            SetFocusOnError="true" Display="Dynamic" ErrorMessage="Please select transporter." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlTransporter" runat="server" Width="300px" TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="ddlTransporter_SelectedIndexChanged">
                                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>Vehicle Number
                                        <asp:RequiredFieldValidator ID="rfvVehicleNo" runat="server" ControlToValidate="txtVehicleNo" SetFocusOnError="true" Display="Dynamic"
                                            ErrorMessage="Please enter vehicle number." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegExVehicleNo" runat="server" ControlToValidate="txtVehicleNo"
                                            ValidationGroup="vgRequired" Text="Invalid No" Display="Dynamic" SetFocusOnError="true"
                                            ValidationExpression="^[a-z A-Z 0-9 . -]{8,16}$" ErrorMessage="Please Enter Valid Vehicle Number">
                                        </asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="rfvBabajiVehicleNo" runat="server" ControlToValidate="ddVehicleNo" InitialValue="0" SetFocusOnError="true" Display="Dynamic"
                                            ErrorMessage="Please select vehicle number." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVehicleNo" runat="server" TabIndex="2" MaxLength="10" Width="160px"></asp:TextBox>
                                        <asp:DropDownList ID="ddVehicleNo" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Vehicle Type
                                        <asp:RequiredFieldValidator ID="rfvVehicleType" runat="server" ControlToValidate="ddlVehicleType" InitialValue="0" SetFocusOnError="true" Display="Dynamic"
                                            ErrorMessage="Please select vehicle type." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlVehicleType" runat="server" Width="175px" TabIndex="3">
                                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>City                               
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCity" runat="server" TabIndex="4" MaxLength="10" Width="160px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Attach Collection Memo
                                        <asp:RequiredFieldValidator ID="rfvMemoCopy" runat="server" ControlToValidate="fuMemoDocument" Display="Dynamic" SetFocusOnError="true"
                                            ForeColor="Red" ErrorMessage="Please attach collection memo." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:FileUpload ID="fuMemoDocument" runat="server" TabIndex="5" Width="160px" />
                                    </td>
                                    <td>Freight Rate
                                        <asp:RequiredFieldValidator ID="rfvRate" runat="server" ControlToValidate="txtRate" SetFocusOnError="true" Display="Dynamic"
                                            ErrorMessage="Please enter freight rate." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                                        <%--  <asp:RegularExpressionValidator ID="revRate" runat="server" ValidationExpression="^[1-9]\d*(\.\d+)?$" ControlToValidate="txtRate"
                                    SetFocusOnError="true" Display="Dynamic" ErrorMessage="(Invalid rate)" ForeColor="Red"></asp:RegularExpressionValidator>--%>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRate" runat="server" TabIndex="6" Width="160px" AutoPostBack="true" OnTextChanged="txtRate_TextChanged"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Advance (%)
                              <%--  <asp:RegularExpressionValidator ID="revAdvance" runat="server" ValidationExpression="^[1-9]\d*(\.\d+)?$" ControlToValidate="txtRate"
                                    SetFocusOnError="true" Display="Dynamic" ErrorMessage="(Invalid advance)" ForeColor="Red"></asp:RegularExpressionValidator>--%>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAdvance" runat="server" TabIndex="7" Width="160px" AutoPostBack="true" OnTextChanged="txtAdvance_TextChanged"></asp:TextBox>
                                    </td>
                                    <td>Advance Amount</td>
                                    <td>
                                        <asp:TextBox ID="txtAdvanceAmount" runat="server" TabIndex="8" Enabled="false" Width="160px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Market Rate
                                        <%--<asp:RequiredFieldValidator ID="rfvMarketRate" runat="server" ControlToValidate="txtMarketBillingRate" SetFocusOnError="true" Display="Dynamic"
                                            ErrorMessage="Please enter market billing rate." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="rvMarketRate" runat="server" ControlToValidate="txtMarketBillingRate" Type="Double" ErrorMessage="(Invalid rate)"
                                            ForeColor="Red" SetFocusOnError="true" Display="Dynamic"></asp:RangeValidator>
                                    --%>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMarketBillingRate" runat="server" TabIndex="9" Width="160px"></asp:TextBox>
                                    </td>
                                    <td>Detention Amount
                                        <asp:RangeValidator ID="rvDetentionAmt" runat="server" ControlToValidate="txtDetentionAmount" Type="Double" ErrorMessage="(Invalid amount)"
                                            ForeColor="Red" SetFocusOnError="true" Display="Dynamic"></asp:RangeValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDetentionAmount" runat="server" Width="160px" TabIndex="10"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Varai Expense
                                <asp:RangeValidator ID="rvVaraiExp" runat="server" ControlToValidate="txtVaraiExp" Type="Double" ErrorMessage="(Invalid expense)"
                                    ForeColor="Red" SetFocusOnError="true" Display="Dynamic"></asp:RangeValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVaraiExp" runat="server" Width="160px" TabIndex="11"></asp:TextBox>
                                    </td>
                                    <td>Empty Cont Rcpt Charges
                                        <asp:RangeValidator ID="rvEmptyContRecptCharges" runat="server" ControlToValidate="txtEmptyContCharges" Type="Double" ErrorMessage="(Invalid charges)"
                                            ForeColor="Red" SetFocusOnError="true" Display="Dynamic"></asp:RangeValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmptyContCharges" runat="server" Width="160px" TabIndex="12"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Toll Charges
                                        <asp:RangeValidator ID="rvTollCharges" runat="server" ControlToValidate="txtTollCharges" Type="Double" ErrorMessage="(Invalid charges)"
                                            ForeColor="Red" SetFocusOnError="true" Display="Dynamic"></asp:RangeValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTollCharges" runat="server" Width="160px" TabIndex="13"></asp:TextBox>
                                    </td>
                                    <td>Other Charges
                                        <asp:RangeValidator ID="rvOtherCharges" runat="server" ControlToValidate="txtOtherCharges" Type="Double" ErrorMessage="(Invalid charges)"
                                            ForeColor="Red" SetFocusOnError="true" Display="Dynamic"></asp:RangeValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOtherCharges" runat="server" Width="160px" TabIndex="14"></asp:TextBox>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td>Select Option
                                <asp:RequiredFieldValidator ID="rfvSellingPrice" runat="server" ControlToValidate="rdlRequestReceive" SetFocusOnError="true" Display="Dynamic"
                                    ErrorMessage="Select Option." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rdlRequestReceive" runat="server" OnSelectedIndexChanged="rdlRequestReceive_SelectedIndexChanged"
                                            AutoPostBack="true">
                                            <asp:ListItem Text="As per Contract" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="As per Email" Value="2"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr id="trContractDetail" runat="server" visible="false">
                                    <td>Contract Price
                                <asp:RequiredFieldValidator ID="rfvPrice" runat="server" ControlToValidate="txtContractPrice" SetFocusOnError="true" Display="Dynamic"
                                    ErrorMessage="Please enter contract price." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtContractPrice" runat="server"></asp:TextBox>
                                    </td>
                                    <td>Contract Copy
                                    </td>
                                    <td>
                                        <asp:FileUpload ID="fuContractUploadCopy" runat="server" />
                                        <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator1" runat="server"
                                            ErrorMessage="Only PDF files are allowed!" Display="Dynamic" SetFocusOnError="true"
                                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" type="reset"
                                            ControlToValidate="fuContractUploadCopy" CssClass="text-red"></asp:RegularExpressionValidator>
                                    </td>
                                   
                                </tr>
                                <tr id="trEmailDetail" runat="server" visible="false">
                                    <td>Selling Price
                                <asp:RequiredFieldValidator ID="rfvSellPrice" runat="server" ControlToValidate="txtEmailSellPrice"
                                    SetFocusOnError="true" Display="Dynamic"
                                    ErrorMessage="Please enter Selling price." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmailSellPrice" runat="server"></asp:TextBox>
                                    </td>
                                    <td>Email Approval Copy
                               
                                    </td>
                                    <td>
                                        <asp:FileUpload ID="fuEmailApprovalCopy" runat="server" />
                                        <asp:RequiredFieldValidator ID="RFVFile" runat="server" ControlToValidate="fuEmailApprovalCopy" CausesValidation="true"
                                            InitialValue="" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Please upload email copy."
                                            ValidationGroup="vgRequired" Enabled="false"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator2" runat="server" CausesValidation="true"
                                            ErrorMessage="Only PDF files are allowed!" Display="Dynamic" SetFocusOnError="true"
                                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" type="reset"
                                            ControlToValidate="fuEmailApprovalCopy" CssClass="text-red"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Billing Instruction
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtBillingInstruction" SetFocusOnError="true" Display="Dynamic"
                                    ErrorMessage="Please enter billing instruction." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtBillingInstruction" runat="server" Width="650px" Rows="3" TextMode="MultiLine" TabIndex="18"></asp:TextBox>
                                    </td>
                                </tr>--%>
                            </table>
                            <fieldset>
                                <legend>Selling Rate Detail</legend>
                                <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                    <tr>
                                        <td>Selling Detail
                                <asp:RequiredFieldValidator ID="rfvSellingPrice" runat="server" ControlToValidate="rdlRequestReceive" SetFocusOnError="true" Display="Dynamic"
                                    ErrorMessage="Select Selling Detail." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rdlRequestReceive" runat="server" OnSelectedIndexChanged="rdlRequestReceive_SelectedIndexChanged"
                                                AutoPostBack="true" TabIndex="14" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="As per Contract" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="As per Email" Value="2"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr id="trContractDetail" runat="server" visible="false">
                                        <td>Selling Freight Rate
                                <asp:RequiredFieldValidator ID="rfvPrice" runat="server" ControlToValidate="txtContractPrice" SetFocusOnError="true" Display="Dynamic"
                                    ErrorMessage="Please enter selling freight price." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtContractPrice" runat="server" Width="160px" TabIndex="15"></asp:TextBox>
                                        </td>
                                        <td>Contract Copy
                                        </td>
                                        <td>
                                            <asp:FileUpload ID="fuContractUploadCopy" runat="server" />
                                            <asp:RegularExpressionValidator
                                                ID="RegularExpressionValidator1" runat="server"
                                                ErrorMessage="Only PDF files are allowed!" Display="Dynamic" SetFocusOnError="true"
                                                ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" type="reset"
                                                ControlToValidate="fuContractUploadCopy" CssClass="text-red"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr id="trEmailDetail" runat="server" visible="false">
                                        <td>Selling Freight Rate
                                <asp:RequiredFieldValidator ID="rfvSellPrice" runat="server" ControlToValidate="txtEmailSellPrice"
                                    SetFocusOnError="true" Display="Dynamic"
                                    ErrorMessage="Please enter Selling freight price." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEmailSellPrice" runat="server" Width="160px" TabIndex="16"></asp:TextBox>
                                        </td>
                                        <td>Email Approval Copy
                                        </td>
                                        <td>
                                            <asp:FileUpload ID="fuEmailApprovalCopy" runat="server" />
                                            <asp:RequiredFieldValidator ID="RFVFile" runat="server" ControlToValidate="fuEmailApprovalCopy" CausesValidation="true"
                                                InitialValue="" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Please upload email copy."
                                                ValidationGroup="vgRequired" Enabled="false"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator
                                                ID="RegularExpressionValidator2" runat="server" CausesValidation="true"
                                                ErrorMessage="Only PDF files are allowed!" Display="Dynamic" SetFocusOnError="true"
                                                ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" type="reset"
                                                ControlToValidate="fuEmailApprovalCopy" CssClass="text-red"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Billing Instruction
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtBillingInstruction" SetFocusOnError="true" Display="Dynamic"
                                    ErrorMessage="Please enter billing instruction." Text="*" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtBillingInstruction" runat="server" Width="650px" Rows="3" TextMode="MultiLine" TabIndex="18"></asp:TextBox>
                                        </td>
                                    </tr>

                                </table>
                            </fieldset>
                            <br />
                            <div style="width: 1250px; overflow-x: scroll">
                                <asp:GridView ID="GridViewVehicle" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                                    DataSourceID="DataSourceRate" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false" OnRowDataBound="GridViewVehicle_RowDataBound"
                                    PageSize="80" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF"
                                    OnRowCommand="GridViewVehicle_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkbtnUpdate" runat="server" CommandName="updateRow" Text="Edit" CommandArgument='<%#Bind("lid") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" ToolTip="Select to send fund request." />
                                                <asp:HiddenField ID="hdnFundReqId" runat="server" Value='<%#Bind("FundRequestId") %>' />
                                                <asp:HiddenField ID="hdnTransporterId" runat="server" Value='<%#Bind("TransporterId") %>' />
                                                <asp:HiddenField ID="hdnMemoPath" runat="server" Value='<%#Eval("MemoAttachment")%>' />
                                                <asp:HiddenField ID="hdnMemoCopyName" runat="server" Value='<%#Eval("MemoCopyName")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TransporterName" HeaderText="Transporter" SortExpression="TransporterName" ReadOnly="true" />
                                        <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" SortExpression="VehicleNo" ReadOnly="true" />
                                        <asp:BoundField DataField="VehicleTypeName" HeaderText="Vehicle Type" SortExpression="VehicleTypeName" ReadOnly="true" />
                                        <asp:BoundField DataField="LRNo" HeaderText="LRNo" SortExpression="LRNo" ReadOnly="true" Visible="false" />
                                        <asp:BoundField DataField="LRDate" HeaderText="LRDate" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LRDate" ReadOnly="true" Visible="false" />
                                        <asp:BoundField DataField="ChallanNo" HeaderText="ChallanNo" SortExpression="ChallanNo" ReadOnly="true" Visible="false" />
                                        <asp:BoundField DataField="ChallanDate" HeaderText="Challan Date" SortExpression="ChallanDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" Visible="false" />
                                        <asp:BoundField DataField="Rate" HeaderText="Freight Rate" SortExpression="Rate" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="Advance" HeaderText="Advance (%)" SortExpression="Advance" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="AdvanceAmount" HeaderText="AdvanceAmount" SortExpression="AdvanceAmount" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="MarketBillingRate" HeaderText="Market Billing Rate" SortExpression="MarketBillingRate" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="FreightAmount" HeaderText="Freight Amt" SortExpression="FreightAmount" ReadOnly="true" Visible="false" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="DetentionAmount" HeaderText="Detention Amt" SortExpression="DetentionAmount" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="VaraiExpense" HeaderText="Varai Exp" SortExpression="VaraiExpense" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="EmptyContRecptCharges" HeaderText="Empty Cont Charges" SortExpression="EmptyContRecptCharges" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="TollCharges" HeaderText="Toll Charges" SortExpression="TollCharges" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="OtherCharges" HeaderText="Other Charges" SortExpression="OtherCharges" ItemStyle-HorizontalAlign="Right" />
                                        <%--<asp:BoundField DataField="Instruction" HeaderText="Billing Intruction" SortExpression="Instruction" ReadOnly="true" />--%>
                                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy" ReadOnly="true" />
                                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" SortExpression="CreatedDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="UpdatedBy" HeaderText="Updated By" SortExpression="UpdatedBy" ReadOnly="true" />
                                        <asp:BoundField DataField="UpdatedDate" HeaderText="Updated Date" SortExpression="UpdatedDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:TemplateField HeaderText="Memo">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnMemoCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download collection memo copy."
                                                    CommandName="MemoCopy" CommandArgument='<%#Eval("MemoAttachment")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Download">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                                    CommandArgument='<%#Eval("EmailAttachment") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="TabPanelMovement" runat="server" TabIndex="2" HeaderText="Movement">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Movement History</legend>
                            <div>
                                <asp:GridView ID="gvMovementHistory" runat="server" AutoGenerateColumns="true" CssClass="table"
                                    Width="100%" PagerStyle-CssClass="pgr" DataSourceID="DataSourceMovementHistory"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="80">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>Vehicle Daily Status History</legend>
                            <div>
                                <asp:GridView ID="gvVehicleDailyStatus" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="TransReqId"
                                    DataSourceID="DataSourceDailyStatus" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false"
                                    PageSize="20" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" OnRowCommand="gvVehicleDailyStatus_RowCommand" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Vehicle No" DataField="VehicleNo" />
                                        <asp:BoundField HeaderText="Vehicle Type" DataField="VehicleType" />
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkbtnDailyStatus" runat="server" Font-Underline="true" CommandName="DailyStatus" Text='<%#Eval("CurrentStatus") %>'
                                                    CommandArgument='<%#Eval("VehicleNo") + ";" + Eval("VehicleType") + ";" + Eval("DeliveryFrom") + ";" + Eval("DeliveryTo") + ";" + Eval("DispatchDate","{0:dd/MM/yyyy}") + ";" + Eval("CustomerMail") + ";" + Eval("TransReqId") + ";" + Eval("JobRefNo") + ";" + Eval("CustName") + ";" + Eval("CustRefNo") + ";" + Eval("CurrentStatus") + ";" + Eval("StatusCreatedBy")  + ";" + Eval("EmailTo") + ";" + Eval("EmailCC")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Current Status" DataField="CurrentStatus" Visible="false" />
                                        <asp:BoundField HeaderText="CreatedBy" DataField="StatusCreatedBy" />
                                        <asp:BoundField HeaderText="Created Date" DataField="StatusCreatedDate" DataFormatString="{0:dd/MM/yyyy HH:mm:tt}" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <%-- START  : Pop-up For Daily Status --%>
                            <div>
                                <asp:HiddenField ID="hdnDailyStatus" runat="server" Value="0" />
                                <cc1:ModalPopupExtender ID="mpeDailyStatus" runat="server" TargetControlID="hdnDailyStatus" BackgroundCssClass="modalBackground" CancelControlID="imgClosePopup"
                                    PopupControlID="pnlDailyStatus" DropShadow="true">
                                </cc1:ModalPopupExtender>
                                <asp:Panel ID="pnlDailyStatus" runat="server" CssClass="modalPopup1" BackColor="#F5F5DC" Style="border-radius: 5px" Width="810px" Height="520px" BorderStyle="Solid" BorderWidth="2px">
                                    <div id="div1" runat="server">
                                        <table width="100%" style="border-bottom: 1px solid black">
                                            <tr>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td align="center"><b><u>Daily Status</u></b>
                                                    <span style="float: right">
                                                        <asp:ImageButton ID="imgClosePopup" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgClosePopup_Click" ToolTip="Close" />
                                                    </span>
                                                </td>
                                            </tr>
                                        </table>
                                        <div>
                                            <asp:Panel ID="pnlDailyStatus2" runat="server" Width="800px" Height="480px" ScrollBars="Auto">
                                                <div id="DivABC" runat="server" style="border: 1px solid black; margin: 5px; margin-top: 0px; border-radius: 4px; max-height: 620px; max-width: 780px;">
                                                    <div class="m" style="padding: 10px;">
                                                        <asp:Label ID="lblPopMessageEmail" runat="server" EnableViewState="false"></asp:Label>
                                                        <table border="0" width="100%">
                                                            <tr>
                                                                <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  <b>To : </b>
                                                                    <u>
                                                                        <asp:Label ID="lblCustomerEmail" runat="server" Font-Underline="true" Width="89%" Enabled="false" CssClass="cssStatus"></asp:Label></u>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b>CC :</b>
                                                                    <u>
                                                                        <asp:Label ID="txtMailCC" runat="server" Width="89%" Font-Underline="true" Enabled="false" CssClass="cssStatus"></asp:Label></u>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td><b>Subject :</b>
                                                                    <u>
                                                                        <asp:Label ID="txtSubject" runat="server" Width="89%" Enabled="false" Font-Underline="true" CssClass="cssStatus"></asp:Label></u>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <div id="divPreviewEmail" runat="server" style="padding: 10px; background-color: white; border-radius: 3px; margin-left: 10px; margin-right: 10px; margin-bottom: 20px; border: 1px solid black; border-style: ridge">
                                                    </div>
                                                    <div id="DivSendEmail" runat="server" style="text-align: right; margin-left: 350px">
                                                        <asp:Button ID="btnCancelEmailPp" runat="server" OnClick="btnCancelEmailPp_Click" Text="Cancel"></asp:Button>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                            <%-- END    : Pop-up For Daily Status --%>
                        </fieldset>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="TabPanelBillStatus" runat="server" TabIndex="3" HeaderText="Bill Detail">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Bill Detail</legend>
                            <div>
                                <asp:GridView ID="gvBillDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                                    DataSourceID="DataSourceBillDetail" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false"
                                    PageSize="80" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Transporter" DataField="Transporter" />
                                        <asp:BoundField HeaderText="Bill Number" DataField="BillNumber" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField HeaderText="Bill Submit Date" DataField="BillSubmitDate" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField HeaderText="Bill Date" DataField="BillDate" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField HeaderText="Billing Person" DataField="BillPersonName" />
                                        <asp:BoundField HeaderText="Bill Amount" DataField="BillAmount" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField HeaderText="Detention" DataField="DetentionAmount" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField HeaderText="Varai" DataField="VaraiAmount" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField HeaderText="Empty Cont Charges" DataField="EmptyContRcptCharges" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField HeaderText="Total" DataField="TotalAmount" ItemStyle-HorizontalAlign="Right" ItemStyle-Font-Bold="true" />
                                        <asp:BoundField HeaderText="Justification" DataField="Justification" ItemStyle-Width="35%" />
                                    </Columns>
                                </asp:GridView>
                                <br />
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>Bill Approval History</legend>
                            <div>
                                <asp:GridView ID="gvBillApprovalHistory" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                                    DataSourceID="DataSourceBillApprovalHistory" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false"
                                    PageSize="80" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="BillNumber" DataField="BillNumber" />
                                        <asp:BoundField HeaderText="Status" DataField="Status" />
                                        <asp:BoundField HeaderText="Remark" DataField="Remark" />
                                        <asp:BoundField HeaderText="Active/Inactive" DataField="ActiveStatus" Visible="false" />
                                        <asp:BoundField HeaderText="Created By" DataField="CreatedBy" />
                                        <asp:BoundField HeaderText="Created Date" DataField="CreatedDate" DataFormatString="{0:dd/MM/yyyy HH:mm:tt}" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>

                        <fieldset>
                            <legend>Vehicle Rate Detail</legend>
                            <div style="width: 1325px; overflow-x: auto">
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                                    DataSourceID="DataSourceRate" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false"
                                    PageSize="80" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TRRefNo" HeaderText="Ref No" ReadOnly="true" Visible="false" />
                                        <asp:BoundField DataField="JobRefNo" HeaderText="Job No" ReadOnly="true" Visible="false" />
                                        <asp:BoundField DataField="TransitType" HeaderText="Delivery To" ReadOnly="true" />
                                        <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" ReadOnly="true" />
                                        <asp:BoundField DataField="TransporterName" HeaderText="Transporter" ReadOnly="true" />
                                        <asp:BoundField DataField="MarketBillingRate" HeaderText="Market Rate" SortExpression="MarketBillingRate" ReadOnly="true" Visible="false" />
                                        <asp:BoundField DataField="Rate" HeaderText="Freight Rate" SortExpression="Rate" ReadOnly="true" />
                                        <asp:BoundField DataField="Advance" HeaderText="Advance" SortExpression="Advance" ReadOnly="true" Visible="false" />
                                        <asp:BoundField DataField="AdvanceAmount" HeaderText="Advance Amt" SortExpression="AdvanceAmount" ReadOnly="true" />
                                        <asp:BoundField DataField="DetentionAmount" HeaderText="Detention Amt" SortExpression="DetentionAmount" ReadOnly="true" />
                                        <asp:BoundField DataField="VaraiExpense" HeaderText="Varai Exp" SortExpression="VaraiExpense" ReadOnly="true" />
                                        <asp:BoundField DataField="EmptyContRecptCharges" HeaderText="Empty Cont Charges" SortExpression="EmptyContRecptCharges" ReadOnly="true" />
                                        <asp:BoundField DataField="TotalAmount" HeaderText="Total" SortExpression="TotalAmount" ReadOnly="true" />
                                        <asp:BoundField DataField="TollCharges" HeaderText="Toll Charges" SortExpression="TollCharges" ReadOnly="true" />
                                        <asp:BoundField DataField="OtherCharges" HeaderText="Other Charges" SortExpression="OtherCharges" ReadOnly="true" />
                                        <asp:BoundField DataField="VehicleTypeName" HeaderText="Type" ReadOnly="true" />
                                        <asp:BoundField DataField="LocationFrom" HeaderText="Delivery From" ReadOnly="true" />
                                        <asp:BoundField DataField="DeliveryPoint" HeaderText="Delivery Point" ReadOnly="true" />
                                        <asp:BoundField DataField="City" HeaderText="City" ReadOnly="true" />
                                        <asp:BoundField DataField="LRNo" HeaderText="LR No" SortExpression="LRNo" ReadOnly="true" />
                                        <asp:BoundField DataField="LRDate" HeaderText="LR Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LRDate" ReadOnly="true" />
                                        <asp:BoundField DataField="ChallanNo" HeaderText="Challan No" SortExpression="ChallanNo" ReadOnly="true" />
                                        <asp:BoundField DataField="ChallanDate" HeaderText="Challan Date" SortExpression="ChallanDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="UnloadingDate" HeaderText="Unloading Date" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                                        <%--<asp:BoundField DataField="Instruction" HeaderText="Billing Intruction" SortExpression="Instruction" ReadOnly="true" />--%>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>Selling Rate Detail</legend>
                            <div>
                                <div style="width: 1325px;overflow-x: scroll">
                                    <asp:GridView ID="gvSellingDetail" runat="server" AutoGenerateColumns="False" CssClass="table" Width="90%" AlternatingRowStyle-CssClass="alt"
                                        PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceSellingRate" OnRowCommand="gvSellingDetail_RowCommand"
                                        OnPreRender="gvSellingDetail_PreRender" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sl">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" ReadOnly="true" />
                                            <asp:BoundField DataField="TransporterName" HeaderText="Transporter" ReadOnly="true" />
                                            <asp:BoundField DataField="SellFreighRate" HeaderText="Selling Freight rate" ReadOnly="true" />
                                            <asp:BoundField DataField="SellDetentionAmount" HeaderText="Detention Amount" ReadOnly="true" />
                                            <asp:BoundField DataField="SellVaraiExpense" HeaderText="Varai Amount No" ReadOnly="true" />
                                            <asp:BoundField DataField="SellEmptyContRecptCharges" HeaderText="Empty Cont Amount" ReadOnly="true" />
                                            <asp:BoundField DataField="SellTollCharges" HeaderText="Toll Amount No" ReadOnly="true" />
                                            <asp:BoundField DataField="SellOtherCharges" HeaderText="Other Amount" ReadOnly="true" />
                                            <asp:TemplateField HeaderText="Detention Doc">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnDetentionCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download detention copy."
                                                        CommandName="DetentionCopy" CommandArgument='<%#Eval("DetentionDoc")%>' Visible='<%# DecideHereImg((string)Eval("DetentionDoc")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Varai Doc">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnVaraiCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download varai copy."
                                                        CommandName="varaiCopy" CommandArgument='<%#Eval("VaraiDoc")%>' Visible='<%# DecideHereImg((string)Eval("VaraiDoc")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Empty Cont Doc">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnemptyContCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download Empty cont copy."
                                                        CommandName="EmptyContCopy" CommandArgument='<%#Eval("EmptyContDoc")%>' Visible='<%# DecideHereImg((string)Eval("EmptyContDoc")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Toll Doc">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnTollCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download toll copy."
                                                        CommandName="TollCopy" CommandArgument='<%#Eval("TollDoc")%>' Visible='<%# DecideHereImg((string)Eval("TollDoc")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Other Doc">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnOtherCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download other copy."
                                                        CommandName="OtherCopy" CommandArgument='<%#Eval("OtherDoc")%>' Visible='<%# DecideHereImg((string)Eval("OtherDoc")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Email Approval Copy">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnEmailApprovalCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download Email Approval copy."
                                                        CommandName="EmailApprovalCopy" CommandArgument='<%#Eval("EmailAttachment")%>' Visible='<%# DecideHereImg((string)Eval("EmailAttachment")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Contract Copy">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnContractCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download contract copy."
                                                        CommandName="ContractCopy" CommandArgument='<%#Eval("ContractAttachment")%>' Visible='<%# DecideHereImg((string)Eval("ContractAttachment")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:BoundField DataField="Instruction" HeaderText="Billing Instruction" ReadOnly="true" />--%>
                                            <asp:BoundField DataField="Remark" HeaderText="Other Remark" ReadOnly="true" />
                                            <asp:BoundField DataField="SellDetail" HeaderText="Charge to Party" ReadOnly="true" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </fieldset>

                        <fieldset>
                            <legend>Bill Received Detail</legend>
                            <div>
                                <asp:GridView ID="gvBillReceived" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                                    DataSourceID="DataSourceBillReceived" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false"
                                    PageSize="20" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="BillNumber" DataField="BillNumber" />
                                        <asp:BoundField HeaderText="Sent User" DataField="SentUser" />
                                        <asp:BoundField HeaderText="Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField HeaderText="ReceivedBy" DataField="ReceivedBy" />
                                        <asp:BoundField HeaderText="ReceivedDate" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField HeaderText="Status" DataField="StatusName" />
                                        <asp:BoundField HeaderText="Cheque No" DataField="ChequeNo" />
                                        <asp:BoundField HeaderText="Cheque Date" DataField="ChequeDate" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField HeaderText="Hold Reason" DataField="HoldReason" />
                                        <asp:BoundField HeaderText="Release Date" DataField="ReleaseDate" DataFormatString="{0:dd/MM/yyyy}" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>Bill Received Status History</legend>
                            <div>
                                <asp:GridView ID="gvBillStatusHistory" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                                    DataSourceID="DataSourceBillStatusHistory" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false"
                                    PageSize="20" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Status" DataField="StatusName" />
                                        <asp:BoundField HeaderText="Cheque No" DataField="ChequeNo" />
                                        <asp:BoundField HeaderText="Cheque Date" DataField="ChequeDate" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField HeaderText="Hold Reason" DataField="HoldReason" />
                                        <asp:BoundField HeaderText="Release Date" DataField="ReleaseDate" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField HeaderText="CreatedBy" DataField="CreatedBy" />
                                        <asp:BoundField HeaderText="Created Date" DataField="CreatedDate" DataFormatString="{0:dd/MM/yyyy HH:mm:tt}" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="TabPanelDelivery" runat="server" TabIndex="4" HeaderText="Delivery">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Delivery Detail</legend>
                            <asp:GridView ID="gvVehicleDelivery" runat="server" AutoGenerateColumns="False" CssClass="table"
                                Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="TransporterId"
                                DataSourceID="DataSourceDelivery" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false"
                                PageSize="20" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Transporter" DataField="TransporterName" />
                                    <asp:BoundField HeaderText="Vehicle No" DataField="VehicleNo" />
                                    <asp:BoundField HeaderText="Vehicle Type" DataField="VehicleTypeName" />
                                    <asp:BoundField HeaderText="Total Packages" DataField="NoofPackages" />
                                    <asp:BoundField HeaderText="Container No" DataField="ContainerNo" />
                                    <asp:BoundField HeaderText="LR No" DataField="LRNo" />
                                    <asp:BoundField HeaderText="LR Date" DataField="LRDate" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField HeaderText="Challan No" DataField="ChallanNo" />
                                    <asp:BoundField HeaderText="Challan Date" DataField="ChallanDate" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField HeaderText="Location From" DataField="LocationFrom" />
                                    <asp:BoundField HeaderText="Delivery To" DataField="DeliveryPoint" />
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="TabPanelFund" runat="server" TabIndex="5" HeaderText="Fund Detail">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Fund Payment Detail</legend>
                            <div>
                                <asp:GridView ID="gvGetPaymentHistory" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                                    DataSourceID="DataSourcePaymentHistory" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false"
                                    PageSize="20" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="JobRefNo" DataField="JobRefNo" />
                                        <asp:BoundField HeaderText="Expense Type" DataField="ExpenseTypeName" />
                                        <asp:BoundField HeaderText="Payment Type" DataField="PaymentTypeName" />
                                        <asp:BoundField HeaderText="Amount" DataField="Amount" />
                                        <asp:BoundField HeaderText="Payment Type" DataField="PaymentTypeName" />
                                        <asp:BoundField HeaderText="Paid To" DataField="PaidTo" />
                                        <asp:BoundField HeaderText="Transporter" DataField="Transporter" />
                                        <asp:BoundField HeaderText="Vehicle No" DataField="VehicleNo" />
                                        <asp:BoundField HeaderText="CreatedBy" DataField="CreatedBy" />
                                        <asp:BoundField HeaderText="Created Date" DataField="CreatedOn" DataFormatString="{0:dd/MM/yyyy HH:mm:tt}" />
                                    </Columns>
                                </asp:GridView>
                                <br />
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>Fund Payment History</legend>
                            <div>
                                <asp:GridView ID="gvReqHistory" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceRequestHistory"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" />
                                        <asp:BoundField DataField="Remark" HeaderText="Remark" />
                                        <asp:BoundField DataField="CreatedBy" HeaderText="Updated By" />
                                        <asp:BoundField DataField="CreatedDate" HeaderText="Updated Date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss tt}" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <asp:SqlDataSource ID="DataSourceRequestHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="TR_GetPaymentStatusHistory" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </fieldset>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="TabPanelTripDetail" runat="server" TabIndex="6" HeaderText="Trip Detail">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Trip Detail</legend>
                            <div>
                                <asp:GridView ID="gvVehicleExpense" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" DataKeyNames="lId" DataSourceID="DataSourceVehicleExpense" CellPadding="4">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Vehicle No" DataField="VehicleNo" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Fuel" DataField="Fuel2" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Fuel Liter" DataField="Fuel2Liter" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Toll Charges" DataField="TollCharges" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Fine Without Cleaner" DataField="FineWithoutCleaner" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Xerox" DataField="Xerox" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Varai Unloading" DataField="VaraiUnloading" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Empty Container Receipt" DataField="EmptyContainerReceipt" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Parking/GatePass" DataField="ParkingGatePass" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Garage" DataField="Garage" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Bhatta" DataField="Bhatta" ReadOnly="true" />
                                        <asp:BoundField HeaderText="ODC/Overweight" DataField="AdditionalChargesForODCOverweight" ReadOnly="true" />
                                        <asp:BoundField HeaderText="OtherCharges" DataField="OtherCharges" ReadOnly="true" />
                                        <asp:BoundField HeaderText="NakaPassing/DamageContainer" DataField="NakaPassingDamageContainer" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Created By" DataField="CreatedBy" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Created Date" DataField="CreatedDate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="DataSourceVehicleExpense" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="TR_GetVehicleRateExpense" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </fieldset>
                    </ContentTemplate>
                </cc1:TabPanel>
            </cc1:TabContainer>
            <div id="dvModalPopup">
                <asp:HiddenField ID="hdnFundRequest" runat="server" Value="0" />
                <cc1:ModalPopupExtender ID="mpeFundRequest" runat="server" TargetControlID="hdnFundRequest" BackgroundCssClass="modalBackground" CancelControlID="imgClosePopup2"
                    PopupControlID="pnlFundRequest" DropShadow="true">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pnlFundRequest" runat="server" CssClass="modalPopup1" BackColor="#F5F5DC" Width="1100px" Height="600px" BorderStyle="Solid" BorderWidth="1px">
                    <div id="div2" runat="server">
                        <br />
                        <table width="100%">
                            <tr>
                                <td align="center"><b><u>FUND REQUEST</u></b>
                                    <span style="float: right">
                                        <asp:ImageButton ID="imgClosePopup2" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgClosePopup2_Click" ToolTip="Close" />
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblFundError" runat="server" Font-Bold="true" Font-Size="13px"></asp:Label>
                                    <asp:HiddenField ID="hdnRateDetailId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnMemoCopyPath" runat="server" />
                                </td>
                            </tr>
                        </table>
                        <div>
                            <asp:Panel ID="pnlFundRequest2" runat="server" Width="1060px" Height="530px" ScrollBars="Auto" Style="padding-left: 10px">
                                <fieldset>
                                    <div id="divInstruction" class="info" runat="server">
                                    </div>
                                    <div class="m clear">
                                        <asp:HiddenField ID="hdnTransporterId" runat="server" Value="0" />
                                        <asp:Button ID="btnSubmit" Text="Save" OnClick="btnSubmit_Click" runat="server" ValidationGroup="Required"
                                            TabIndex="7" />
                                        <asp:Button ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false" TabIndex="8"
                                            runat="server" />
                                    </div>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                        <tr>
                                            <td width="20%">Job Number
                                            </td>
                                            <td>
                                                <asp:Label ID="txtJobNumber" Width="160px" runat="server" ToolTip="Enter Job Number."
                                                    placeholder="Search" TabIndex="1" AutoPostBack="true" OnTextChanged="txtJobNumber_TextChanged"></asp:Label>
                                            </td>
                                            <td>Consignee
                                            </td>
                                            <td>
                                                <asp:Label ID="lblConsignee" runat="server" Enabled="false" Width="230px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Branch</td>
                                            <td>
                                                <asp:HiddenField ID="hdnBranchId" runat="server" Value="0" />
                                                <asp:Label ID="lblBranch" runat="server"></asp:Label>
                                            </td>
                                            <td>Type of Expense                           
                                            </td>
                                            <td>
                                                <asp:Label ID="lblExpenseType" runat="server" Text="Advance Payment"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Planning Date
                                                <cc1:CalendarExtender ID="calPlanningDate" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="imgPlanningDate"
                                                    Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtPlanningDate">
                                                </cc1:CalendarExtender>
                                                <cc1:MaskedEditExtender ID="meePlanningDate" TargetControlID="txtPlanningDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                    MaskType="Date" AutoComplete="false" runat="server">
                                                </cc1:MaskedEditExtender>
                                                <cc1:MaskedEditValidator ID="mevPlanningDate" ControlExtender="meePlanningDate" ControlToValidate="txtPlanningDate" IsValidEmpty="true"
                                                    InvalidValueMessage="Planning Date is invalid" InvalidValueBlurredMessage="Invalid Planning Date" SetFocusOnError="true"
                                                    MinimumValueMessage="Invalid Planning Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="31/12/2025"
                                                    runat="Server"></cc1:MaskedEditValidator>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPlanningDate" runat="server" Width="125px" placeholder="dd/mm/yyyy" TabIndex="3" ToolTip="Enter Planning Date."></asp:TextBox>
                                                <asp:Image ID="imgPlanningDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                            </td>
                                            <td>Type of Payment
                                                <asp:RequiredFieldValidator ID="rfvpaytype" runat="server" ValidationGroup="Required"
                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlPaymentType" InitialValue="0"
                                                    Text="*" ErrorMessage="Please Select Type of Payment."></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlPaymentType" runat="server" AppendDataBoundItems="true" DataSourceID="DataSourcePaymentType"
                                                    DataTextField="sName" DataValueField="lid" TabIndex="4" ToolTip="Select Type Of Payment.">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="trAmount" runat="server">
                                            <td>Freight Rate</td>
                                            <td>
                                                <asp:Label ID="lblFreightRate" runat="server"></asp:Label>
                                            </td>
                                            <td>Amount
                                            </td>
                                            <td>
                                                <asp:Label ID="txtAmount" runat="server" Width="160px" ToolTip="Enter Amount." TabIndex="6" OnTextChanged="txtAmount_OnTextChanged" AutoPostBack="true"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Paid To
                                            </td>
                                            <td>
                                                <asp:Label ID="txtPaidTo" runat="server" ToolTip="Enter Paid To." Width="290px" TabIndex="7" Enabled="false"></asp:Label>
                                            </td>
                                            <td>Bank Name</td>
                                            <td>
                                                <asp:Label ID="lblBankName" runat="server"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td>Account No</td>
                                            <td>
                                                <asp:Label ID="lblAccountNo" runat="server"></asp:Label>
                                            </td>
                                            <td>IFSC Code</td>
                                            <td>
                                                <asp:Label ID="lblIFSCCode" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Remark
                                        <asp:RequiredFieldValidator ID="rfvRemark" runat="server" ValidationGroup="Required"
                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtRemark"
                                            Text="*" ErrorMessage="Please Enter Remark."> </asp:RequiredFieldValidator>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtRemark" runat="server" Rows="3" TextMode="MultiLine" TabIndex="8"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <fieldset>
                                        <legend>Vehicle Detail</legend>
                                        <div>
                                            <asp:GridView ID="gvPopup_Vehicle" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr"
                                                DataKeyNames="lid" AllowPaging="false" AllowSorting="True" CssClass="table" PageSize="20" DataSourceID="SqlDataSourceVehicle" OnRowCommand="gvPopup_Vehicle_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Memo">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgbtnMemoCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download collection memo copy."
                                                                CommandName="MemoCopy" CommandArgument='<%#Eval("MemoAttachment")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" ReadOnly="true" />
                                                    <asp:BoundField DataField="VehicleTypeName" HeaderText="Vehicle Type" ReadOnly="true" />
                                                    <asp:BoundField DataField="LocationFrom" HeaderText="Location From" ReadOnly="true" />
                                                    <asp:BoundField DataField="DeliveryPoint" HeaderText="Delivery Point" ReadOnly="true" />
                                                    <asp:BoundField DataField="AdvanceAmount" HeaderText="Advance" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Rate" HeaderText="Vehicle Hire(Broker Rate)" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Balance" HeaderText="Balance" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="MarketBillingRate" HeaderText="Market Billing Rate" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="SavingAmt" HeaderText="Saving Amt" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                                </Columns>
                                                <PagerTemplate>
                                                    <asp:GridViewPager runat="server" />
                                                </PagerTemplate>
                                            </asp:GridView>
                                            <div>
                                                <asp:SqlDataSource ID="SqlDataSourceVehicle" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                                    SelectCommand="TR_GetTransRateDetailByTP" SelectCommandType="StoredProcedure">
                                                    <SelectParameters>
                                                        <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
                                                        <asp:ControlParameter ControlID="hdnTransporterId" Name="TransporterId" PropertyName="Value" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                            </div>
                                        </div>
                                    </fieldset>
                                    <fieldset>
                                        <legend>Add Documents</legend>
                                        <div id="dvUploadNewFile2" runat="server" style="max-height: 200px; overflow: auto;">
                                            <asp:FileUpload ID="fuDocument2" runat="server" />
                                            <asp:Button ID="btnSaveDocument2" Text="Save Document" runat="server" OnClick="btnSaveDocument2_Click" />
                                        </div>
                                        <br />
                                        <div>
                                            <asp:Repeater ID="rptDocument2" runat="server" OnItemCommand="rptDocument2_ItemCommand">
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
                                                            <asp:LinkButton ID="lnkDownload" Text='<%#DataBinder.Eval(Container.DataItem,"DocumentName") %>'
                                                                CommandArgument='<%# Eval("DocPath") %>' CausesValidation="false" runat="server"
                                                                Width="200px" CommandName="DownloadFile"></asp:LinkButton>
                                                            &nbsp;
                                                            <asp:HiddenField ID="hdnDocLid" Value='<%#DataBinder.Eval(Container.DataItem,"PkId") %>'
                                                                runat="server"></asp:HiddenField>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="lnkDelete" CommandName="Delete" ToolTip="Delete" Width="39" CausesValidation="false"
                                                                runat="server" Text="Delete" Font-Underline="true" OnClientClick="return confirm('Are you sure you want to remove this document?')"></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </fieldset>
                                </fieldset>
                            </asp:Panel>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="divDataSource">
        <asp:SqlDataSource ID="DataSourceDelivery" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetDeliveryDetail_TRId" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceRate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetTransRateDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqID" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <%-- <asp:SqlDataSource ID="DataSourceRate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetTransRateDetailByTP" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqID" SessionField="TRId" />
                <asp:SessionParameter Name="TransporterId" SessionField="TransporterId" />
            </SelectParameters>
        </asp:SqlDataSource>--%>
        <asp:SqlDataSource ID="DataSourceBillDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetTransBillDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceBillReceived" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetBillReceivedByTransReqId" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceBillApprovalHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetBillApprovalHistory" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceBillStatusHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetBillReceivedDetailHistory" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourcePaymentHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetPaymentDetails" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceSellingRate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetVehicleSellingDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqID" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceMovementHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetMovementHistory" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceDailyStatus" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetDailyStatusHistory" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceExpense" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="AC_GetRequestTypeMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourcePaymentType" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="AC_GetPaymentTypeMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>

    </div>
</asp:Content>



