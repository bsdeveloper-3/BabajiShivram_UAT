<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DocumentToShippingLine.aspx.cs"
    Inherits="ExportCHA_DocumentToShippingLine" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <style type="text/css">
        .modalBackground {
            background-color: black;
            filter: alpha(opacity=90);
            opacity: 0.5;
        }
    </style>
    <div align="center">
        <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" />
    </div>
    <div class="clear">
    </div>
    <fieldset>
        <legend>Job Detail</legend>
        <div class="m clear">
            <asp:Button ID="btnSubmit" Text="Save" runat="server" OnClick="btnSubmit_Click"
                ValidationGroup="Required" TabIndex="7" />
            <asp:Button ID="btnCancel" Text="Cancel" CausesValidation="false"
                runat="server" OnClick="btnCancel_Click" TabIndex="8" />
        </div>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
            <tr>
                <td>BS Job No.
                </td>
                <td>
                    <asp:Label ID="lblJobRefNo" runat="server"></asp:Label>
                </td>
                <td>Cust Ref No
                </td>
                <td>
                    <asp:Label ID="lblCustRefNo" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Customer
                </td>
                <td>
                    <asp:Label ID="lblCustName" runat="server"></asp:Label>
                </td>
                <td>Consignee
                </td>
                <td>
                    <asp:Label ID="lblConsigneeName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Shipper
                </td>
                <td>
                    <asp:Label ID="lblShipper" runat="server"></asp:Label>
                </td>
                <td>Mode
                </td>
                <td>
                    <asp:Label ID="lblMode" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Port Of Loading
                </td>
                <td>
                    <asp:Label ID="lblPOL" runat="server"></asp:Label>
                </td>
                <td>Port Of Discharge
                </td>
                <td>
                    <asp:Label ID="lblPOD" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Country Consignment
                </td>
                <td>
                    <asp:Label ID="lblCountryConsgn" runat="server"></asp:Label>
                </td>
                <td>Destination Country  
                </td>
                <td>
                    <asp:Label ID="lblDestCountry" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Forwarder Name</td>
                <td>
                    <asp:Label ID="lblForwarderName" runat="server"></asp:Label>
                </td>
                <td>No Of Packages
                </td>
                <td>
                    <asp:Label ID="lblNoOfPkg" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Gross Weight
                </td>
                <td>
                    <asp:Label ID="lblGrossWT" runat="server"></asp:Label>
                </td>
                <td>Net WT
                </td>
                <td>
                    <asp:Label ID="lblNetWT" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>SB No
                </td>
                <td>
                    <asp:Label ID="lblSBNo" runat="server"></asp:Label>
                </td>
                <td>SB Date
                </td>
                <td>
                    <asp:Label ID="lblSBDate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>LEO Date
                </td>
                <td>
                    <asp:Label ID="lblLEODate" runat="server"></asp:Label>
                </td>
                <td>Document Hand Over To Shipping Line Date
                    <cc1:CalendarExtender ID="calDocHandOverDate" runat="server" Enabled="True" EnableViewState="False"
                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDocHandOverDate"
                        PopupPosition="BottomRight" TargetControlID="txtDocHandOverDate">
                    </cc1:CalendarExtender>
                    <cc1:MaskedEditExtender ID="MEditDocHandOverDate" TargetControlID="txtDocHandOverDate" Mask="99/99/9999" MessageValidatorTip="true"
                        MaskType="Date" AutoComplete="false" runat="server">
                    </cc1:MaskedEditExtender>
                    <cc1:MaskedEditValidator ID="MEditValDocHandOverDate" ControlExtender="MEditDocHandOverDate" ControlToValidate="txtDocHandOverDate" IsValidEmpty="false"
                        EmptyValueMessage="Enter Document Hand Over To Shipping Line Date" EmptyValueBlurredText="*" InvalidValueMessage="Mark Date is invalid"
                        SetFocusOnError="true" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid"
                        MinimumValue="01/01/2014" MaximumValue="31/12/2025" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtDocHandOverDate" runat="server" Width="100px" MaxLength="10" TabIndex="1" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:Image ID="imgDocHandOverDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                        runat="server" />
                </td>
            </tr>
            <tr>
                <td>Exporter Copy
                  <%-- <asp:RequiredFieldValidator ID="rfvExporterCopy" runat="server" ControlToValidate="fuExporterCopy" SetFocusOnError="true" Display="Dynamic" ForeColor="Red"
                       ErrorMessage="Please browse Exporter Copy" Text="*" ValidationGroup="Required"></asp:RequiredFieldValidator>--%>
                </td>
                <td>
                    <asp:FileUpload ID="fuExporterCopy" runat="server" />
                </td>
                <td>VGM Copy
                  <%-- <asp:RequiredFieldValidator ID="rfvVGMCopy" runat="server" ControlToValidate="fuVGMCopy" SetFocusOnError="true" Display="Dynamic" ForeColor="Red"
                       ErrorMessage="Please browse VGM Copy" Text="*" ValidationGroup="Required"></asp:RequiredFieldValidator>--%>
                </td>
                <td>
                    <asp:FileUpload ID="fuVGMCopy" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Informed To Freight Forwarded Date
                     <cc1:CalendarExtender ID="calFreightDate" runat="server" Enabled="True" EnableViewState="False"
                         FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgFreightDate"
                         PopupPosition="BottomRight" TargetControlID="txtFreightForwarderDate">
                     </cc1:CalendarExtender>
                    <cc1:MaskedEditExtender ID="MEditFreightFrDate" TargetControlID="txtFreightForwarderDate" Mask="99/99/9999"
                        MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                    </cc1:MaskedEditExtender>
                    <cc1:MaskedEditValidator ID="MEditValFreightFrDate" ControlExtender="MEditFreightFrDate" ControlToValidate="txtFreightForwarderDate" IsValidEmpty="false"
                        EmptyValueMessage="Enter Informed To Freight Forwarded Date" EmptyValueBlurredText="*" InvalidValueMessage="Freight Forwarded Date is invalid" SetFocusOnError="true"
                        MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid"
                        MinimumValue="01/01/2014" MaximumValue="31/12/2025" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtFreightForwarderDate" runat="server" Width="100px" MaxLength="10" TabIndex="3" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:Image ID="imgFreightDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                </td>
                <td>Forwarder Person Name
                   <asp:RequiredFieldValidator ID="rfvForwarderPerson" runat="server" ControlToValidate="txtForwarderPerson" SetFocusOnError="true" Display="Dynamic" ForeColor="Red"
                       ErrorMessage="Please Enter Forwarder Person Name" Text="*" ValidationGroup="Required"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtForwarderPerson" runat="server" Width="200px" TabIndex="4"></asp:TextBox>
                    &nbsp;
                     <asp:TextBox ID="txtForwardToEmail" runat="server" Width="250px" placeholder="Forward Email To (Mention email of person)"></asp:TextBox>
                    <asp:HiddenField ID="hdnForwardToBabaji" runat="server" />
                    <asp:RegularExpressionValidator ID="RegExeditEmail" runat="server" ControlToValidate="txtForwardToEmail"
                        ErrorMessage="Please Enter Valid Email. Enter semi colon ; For Multiple Email."
                        Text="*" ValidationExpression="((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([;])*)*"
                        ValidationGroup="Required" Display="Dynamic" SetFocusOnError="true"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblFreightFwds" Text="Sent Email To" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:DataList ID="rptFreightDeptList" runat="server" RepeatColumns="2" RepeatDirection="Horizontal">
                        <ItemTemplate>
                            <div class="clear">
                            </div>
                            <div style="float: left;">
                                <asp:CheckBox ID="chkSelect" Text='<%#DataBinder.Eval(Container.DataItem,"sName") %>'
                                    runat="server" TabIndex="29" />
                                <asp:Label ID="lblFreightOpEmail" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"sEmail") %>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:DataList>
                </td>
                <td>Sent Email To Customers
                    <asp:HiddenField ID="hdnCustId" runat="server" Value="0" />
                </td>
                <td>
                    <asp:Repeater ID="rptCustomerEmails" runat="server">
                        <ItemTemplate>
                            <div class="clear">
                            </div>
                            <div style="float: left;">
                                <asp:CheckBox ID="chkSelect" Text='<%#DataBinder.Eval(Container.DataItem,"sName") %>'
                                    runat="server" TabIndex="29" />
                                <asp:Label ID="lblFreightOpEmail" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"sEmail") %>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            <tr style="text-align: right">
                <td colspan="4">
                    <asp:LinkButton ID="lnkbtnShowEmailFormat" runat="server" Text="View Shipment Get In Email Draft"
                        OnClick="lnkbtnShowEmailFormat_Click"></asp:LinkButton></td>

            </tr>
        </table>
        <asp:SqlDataSource ID="DataSourceFreightOp" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
            SelectCommand="FR_GetEnqTransferUser" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceCustEmails" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
            SelectCommand="EX_GetCustomerEmailsList" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="hdnCustId" PropertyName="Value" Name="CustomerId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <!--Customer Email Draft Start -->
        <div id="divPreAlertEmail">
            <cc1:ModalPopupExtender ID="ModalPopupEmail" runat="server" CacheDynamicResults="false"
                DropShadow="False" PopupControlID="Panel2Email" TargetControlID="lnkDummy" BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>

            <asp:Panel ID="Panel2Email" runat="server" CssClass="ModalPopupPanel">
                <div class="header">
                    <div class="fleft">
                        Shipment Get In Intimation Mail
                    </div>
                    <div class="fright">
                        <asp:ImageButton ID="imgEmailClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="btnEMailCancel_Click" ToolTip="Close" />
                    </div>
                </div>
                <div class="m"></div>
                <div id="DivABC" runat="server" style="max-height: 600px; max-width: 780px; overflow: auto;">
                    <div class="m" style="padding: 10px">
                        <asp:Label ID="lblPopMessageEmail" runat="server" EnableViewState="false"></asp:Label>
                        Email To: &nbsp;&nbsp;&nbsp; 
                        <asp:TextBox ID="lblCustomerEmail" runat="server" Width="85%" Enabled="false"></asp:TextBox><br />
                        Email CC: &nbsp;&nbsp;
                        <asp:TextBox ID="txtMailCC" runat="server" Width="85%" Enabled="false"></asp:TextBox>
                        Subject : &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="txtSubject" runat="server" Width="85%" Enabled="false"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvSubject" runat="server" ControlToValidate="txtSubject" SetFocusOnError="true"
                            Text="*" ErrorMessage="Subject Required" ValidationGroup="mailRequired"></asp:RequiredFieldValidator>
                        <hr style="border-top: 1px solid #8c8b8b; margin-top: 10px" />
                    </div>
                    <div id="divPreviewEmail" runat="server" style="margin-left: 10px; padding: 10px; margin-right: 10px; margin-bottom: 20px; border: 1px solid #293452">
                    </div>
                </div>
            </asp:Panel>
            <asp:LinkButton ID="lnkDummy" runat="server"></asp:LinkButton>
            <!--Customer Email Draft End -->
        </div>
    </fieldset>
</asp:Content>

