<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AgentInvoice.aspx.cs" 
    Inherits="FreightOperation_AgentInvoice" Title="Billing & Back Office Details" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <AjaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" />
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
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="ValSummaryDetail" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
            </div>
            <div class="clear"></div>
            <fieldset><legend>Billing Detail</legend>
                <div class="m clear">
                    <asp:Button ID="btnSaveBilling" runat="server" Text="Save" OnClick="btnSaveBilling_Click" ValidationGroup="Required" />
                    <asp:Button ID="btnCancelBilling" runat="server" OnClick="btnCancelBilling_Click" CausesValidation="False"
                        Text="Cancel" TabIndex="20" />
                </div>
                <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
                    <tr>
                        <td>
                            Job No
                        </td>
                        <td>
                            <asp:Label ID="lblJobNo" runat="server"></asp:Label>
                        </td>
                        <td>
                            Agent Invoice Completed?
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rdlAgentInvoice" RepeatDirection="Horizontal" runat="server" Enabled="false">
                                <asp:ListItem Text="NO" Value="false" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="YES" Value="true"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            File Received for Billing
                            <%--<AjaxToolkit:CalendarExtender ID="CalRcvdDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgRcvdDate" PopupPosition="BottomRight"
                                TargetControlID="txtFileRcvdDate">
                            </AjaxToolkit:CalendarExtender>--%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFileRcvdDate" runat="server" Width="100px"></asp:TextBox>
                            <%--<asp:Image ID="imgRcvdDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server"/>
                            <AjaxToolkit:MaskedEditExtender ID="MskExtRcvdDate" TargetControlID="txtFileRcvdDate" Mask="99/99/9999" MessageValidatorTip="true" 
                                MaskType="Date" AutoComplete="false" runat="server"></AjaxToolkit:MaskedEditExtender>
                            <AjaxToolkit:MaskedEditValidator ID="MskValRcvdDate" ControlExtender="MskExtRcvdDate" ControlToValidate="txtFileRcvdDate" IsValidEmpty="false" 
                                InvalidValueMessage="Received Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                EmptyValueMessage="Please Enter File Received Date" EmptyValueBlurredText="Required" MinimumValueBlurredText="Invalid Date" MinimumValueMessage="Invalid File Rcvd Date" 
                                MaximumValueMessage="Invalid File Rcvd Date" MaximumValueBlurredMessage="Invalid Date"
                                Runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>--%>
                        </td>
                        <td>
                            Bill Amount
                            <%--<asp:RequiredFieldValidator ID="RFVBillAmount" runat="server" ControlToValidate="txtBillAmount" Display="Dynamic" 
                                SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Bill Amount." ValidationGroup="Required"></asp:RequiredFieldValidator>--%>
                            <asp:CompareValidator ID="ComValBillAmount" runat="server" ControlToValidate="txtBillAmount"
                                Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" ErrorMessage="Invalid Bill Amount"
                                Display="Dynamic" ValidationGroup="Required"></asp:CompareValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBillAmount" runat="server" MaxLength="15"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Bill Number
                            <%--<asp:RequiredFieldValidator ID="RFVBillNo" runat="server" ControlToValidate="txtBillNumber" Display="Dynamic" 
                             SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Bill No." ValidationGroup="Required"></asp:RequiredFieldValidator>--%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBillNumber" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            Bill Date
                            <AjaxToolkit:CalendarExtender ID="CalBilDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgBillDate" PopupPosition="BottomRight"
                                TargetControlID="txtBillDate">
                            </AjaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBillDate" runat="server" Width="100px"></asp:TextBox>
                           <asp:Image ID="imgBillDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server"/>
                            <AjaxToolkit:MaskedEditExtender ID="MskExtBillDate" TargetControlID="txtBillDate" Mask="99/99/9999" MessageValidatorTip="true" 
                                MaskType="Date" AutoComplete="false" runat="server"></AjaxToolkit:MaskedEditExtender>
                            <AjaxToolkit:MaskedEditValidator ID="MskValBillDate" ControlExtender="MskExtBillDate" ControlToValidate="txtBillDate" IsValidEmpty="true" 
                                InvalidValueMessage="Bill Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                EmptyValueMessage="Please Enter Bill Date" EmptyValueBlurredText="Required" MinimumValueBlurredText="Invalid Date" MinimumValueMessage="Invalid Bill Date" 
                                MaximumValueBlurredMessage="Invalid Date" MaximumValueMessage="Invalid Bill Date" MinimumValue="01/01/2016" MaximumValue="01/01/2025" 
                                Runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Dispatch Date
                        </td>
                        <td>
                            <asp:Label ID="lblDispatchDate" runat="server" ></asp:Label>
                        </td>
                        <td>
                            Remark
                        </td>
                        <td>
                            <asp:TextBox ID="txtBillingRemark" TextMode="MultiLine" Width="90%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <fieldset><legend>Agent Invoice Detail</legend>
                <div class="m clear">
                    <asp:Button ID="btnSaveAgentInvoice" runat="server" Text="Save Invoice" OnClick="btnSaveAgentInvoice_Click" ValidationGroup="RequiredInvoice" />
                    <asp:Button ID="btnCancelInvoice" runat="server" OnClick="btnCancelInvoice_Click" CausesValidation="False"
                        Text="Cancel" TabIndex="20" />
                </div>
                <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
                    <tr>
                        <td>
                            JB Number
                        </td>
                        <td>
                            <asp:TextBox ID="txtJBNumber" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            JB Date
                            <AjaxToolkit:CalendarExtender ID="CalJBDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgJBDate" PopupPosition="BottomRight"
                                TargetControlID="txtJBDate">
                            </AjaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtJBDate" runat="server" Width="100px"></asp:TextBox>
                           <asp:Image ID="imgJBDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server"/>
                            <AjaxToolkit:MaskedEditExtender ID="MskExtJBDate" TargetControlID="txtJBDate" Mask="99/99/9999" MessageValidatorTip="true" 
                                MaskType="Date" AutoComplete="false" runat="server"></AjaxToolkit:MaskedEditExtender>
                            <AjaxToolkit:MaskedEditValidator ID="MskValJBDate" ControlExtender="MskExtJBDate" ControlToValidate="txtJBDate" IsValidEmpty="true" 
                                InvalidValueMessage="JB Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                EmptyValueMessage="Please Enter JB Date" EmptyValueBlurredText="Required" MinimumValueMessage="Invalid JB Date" MinimumValueBlurredText="Invalid Date" 
                                MaximumValueMessage="Invalid JB Date" MaximumValueBlurredMessage="Invalid Date" MinimumValue="01/01/2016"
                                Runat="Server" ValidationGroup="RequiredInvoice"></AjaxToolkit:MaskedEditValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Agent Name
                            <asp:RequiredFieldValidator ID="RFVName" runat="server" ControlToValidate="ddAgent" Display="Dynamic" 
                             InitialValue="0" SetFocusOnError="true" Text="*" ErrorMessage="Please Select Agent Name." ValidationGroup="RequiredInvoice"></asp:RequiredFieldValidator> 
                        </td>
                        <td>
                            <asp:DropDownList ID="ddAgent" runat="server"></asp:DropDownList>
                           <%--<asp:TextBox ID="txtAgentName" runat="server"></asp:TextBox>--%>
                        </td>
                        <td>
                            Invoice Rcvd Date
                            <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgInvRcvdDate" PopupPosition="BottomRight"
                                TargetControlID="txtReceivedDate">
                            </AjaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtReceivedDate" runat="server" Width="100px"></asp:TextBox>
                           <asp:Image ID="imgInvRcvdDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server"/>
                            <AjaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtReceivedDate" Mask="99/99/9999" MessageValidatorTip="true" 
                                MaskType="Date" AutoComplete="false" runat="server"></AjaxToolkit:MaskedEditExtender>
                            <AjaxToolkit:MaskedEditValidator ID="MaskedEditValidator1" ControlExtender="MaskedEditExtender1" ControlToValidate="txtReceivedDate" IsValidEmpty="true" 
                                InvalidValueMessage="Invoice Rcvd Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                EmptyValueMessage="Please Enter Invoice Rcvd Date" EmptyValueBlurredText="Required" MinimumValueBlurredText="Invalid Date" MinimumValueMessage="Invalid Received Date" 
                                MaximumValueBlurredMessage="Invalid Date" MaximumValueMessage="Invalid Received Date" MinimumValue="01/01/2016" MaximumValue="01/01/2025" 
                                Runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Agent/Vendor Invoice No
                            <asp:RequiredFieldValidator ID="RFVInvNo" runat="server" ControlToValidate="txtInvoiceNo" Display="Dynamic" 
                             SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Invoice No." ValidationGroup="RequiredInvoice"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtInvoiceNo" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            Invoice Date
                            <AjaxToolkit:CalendarExtender ID="calInvoiceDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgInvoiceDate" PopupPosition="BottomRight"
                                TargetControlID="txtInvoiceDate">
                            </AjaxToolkit:CalendarExtender>
                        </td>
                        <td>
                           <asp:TextBox ID="txtInvoiceDate" runat="server" Width="100px"></asp:TextBox>
                           <asp:Image ID="imgInvoiceDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server"/>
                            <AjaxToolkit:MaskedEditExtender ID="MskExtInvoiceDate" TargetControlID="txtInvoiceDate" Mask="99/99/9999" MessageValidatorTip="true" 
                                MaskType="Date" AutoComplete="false" runat="server"></AjaxToolkit:MaskedEditExtender>
                            <AjaxToolkit:MaskedEditValidator ID="MskValInvoiceDate" ControlExtender="MskExtInvoiceDate" ControlToValidate="txtInvoiceDate" IsValidEmpty="true" 
                                InvalidValueMessage="Invoice Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                EmptyValueMessage="Please Enter Invoice Date" EmptyValueBlurredText="Required" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2025" 
                                Runat="Server" ValidationGroup="RequiredInvoice"></AjaxToolkit:MaskedEditValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Invoice Amount
                            <%--<asp:RequiredFieldValidator ID="RFVAmount" runat="server" ControlToValidate="txtInvoiceAmount" Display="Dynamic" 
                                SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Invoice Amount." ValidationGroup="Required"></asp:RequiredFieldValidator>--%>
                            <asp:CompareValidator ID="CompValAmount" runat="server" ControlToValidate="txtInvoiceAmount"
                                Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" ErrorMessage="Invalid Amount"
                                Display="Dynamic" ValidationGroup="RequiredInvoice"></asp:CompareValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtInvoiceAmount" runat="server" MaxLength="15"></asp:TextBox>
                        </td>
                        <td>
                           Invoice Currency
                           <%--<asp:RequiredFieldValidator ID="RFVCurrency" runat="server" ControlToValidate="ddCurrency" Display="Dynamic" InitialValue="0" 
                            SetFocusOnError="true" Text="*" ErrorMessage="Please Select Invoice Currency." ValidationGroup="Required"></asp:RequiredFieldValidator>--%>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddCurrency" runat="server" DataSourceID="dataSourceCurrency" AppendDataBoundItems="true"
                                DataValueField="lId" DataTextField="Currency">
                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                             </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Remark
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtInvoiceRemark" TextMode="MultiLine" Width="90%" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <div class="m clear">
                <div>
                <asp:GridView ID="GridViewInvoiceDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                    Width="100%" DataKeyNames="lId" DataSourceID="DataSourceInvoiceDetail" CellPadding="4"
                    OnRowUpdating="GridViewInvoiceDetail_RowUpdating">
                    <Columns>
                        <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server"
                                        Text="Edit" Font-Underline="true"></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" Width="45" runat="server"
                                        Text="Update" Font-Underline="true" ValidationGroup="GridInvoiceRequired">
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" Width="39" CausesValidation="false"
                                        runat="server" Text="Cancel" Font-Underline="true"></asp:LinkButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="JB Number">
                            <ItemTemplate>
                                <asp:Label ID="lblJBNumber" runat="server" Text='<%# Eval("JBNumber")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtJBNumber" runat="server" Text='<%# Bind("JBNumber")%>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RFVJBNumber" runat="server" ControlToValidate="txtJBNumber"
                                    SetFocusOnError="true" Text="*" ErrorMessage="Please Enter JB Number" Display="Dynamic"
                                    ValidationGroup="GridInvoiceRequired"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="JB Date">
                            <ItemTemplate>
                                <asp:Label ID="lblJBDate" runat="server" Text='<%# Eval("JBDate","{0:dd/MM/yyyy}")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtJBDate" runat="server" Width="80px" Text='<%# bind("JBDate","{0:dd/MM/yyyy}")%>'></asp:TextBox>
                                <AjaxToolkit:CalendarExtender ID="calJBDate" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                    Format="dd/MM/yyyy" PopupButtonID="imgJBDate" PopupPosition="BottomRight" TargetControlID="txtJBDate">
                                </AjaxToolkit:CalendarExtender>
                                <asp:Image ID="imgJBDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                                    runat="server" />
                                <asp:RequiredFieldValidator ID="RFVJBDate" runat="server" ControlToValidate="txtJBDate"
                                    SetFocusOnError="true" Text="*" ErrorMessage="Please Enter JB Date" Display="Dynamic"
                                    ValidationGroup="GridInvoiceRequired"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Agent">
                            <ItemTemplate>
                                <asp:Label ID="lblAgentInvoiceName" runat="server" Text='<%# Eval("AgentInvoiceName")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtAgentInvoiceName" runat="server" Text='<%# bind("AgentInvoiceName")%>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RFVAgentName" runat="server" ControlToValidate="txtAgentInvoiceName"
                                    SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Agent Name" Display="Dynamic"
                                    ValidationGroup="GridInvoiceRequired"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Invoice No">
                            <ItemTemplate>
                                <asp:Label ID="lblAgentInvoiceNo" runat="server" Text='<%# Eval("AgentInvoiceNo")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtAgentInvoiceNo" runat="server" Text='<%# bind("AgentInvoiceNo")%>'
                                    Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RFVAgentInvNo" runat="server" ControlToValidate="txtAgentInvoiceNo"
                                    SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Invoice No" Display="Dynamic"
                                    ValidationGroup="GridInvoiceRequired"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Invoice Date">
                            <ItemTemplate>
                                <asp:Label ID="lblAgentInvoicDate" runat="server" Text='<%# Eval("AgentInvoiceDate","{0:dd/MM/yyyy}")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtAgentInvoiceDate" runat="server" Width="80px" Text='<%# bind("AgentInvoiceDate","{0:dd/MM/yyyy}")%>'></asp:TextBox>
                                <AjaxToolkit:CalendarExtender ID="calAGInvDate" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                    Format="dd/MM/yyyy" PopupButtonID="imgAgInvDate" PopupPosition="BottomRight"
                                    TargetControlID="txtAgentInvoiceDate"></AjaxToolkit:CalendarExtender>
                                <asp:Image ID="imgAgInvDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                                    runat="server" />
                                <asp:RequiredFieldValidator ID="RFVAgentInvDate" runat="server" ControlToValidate="txtAgentInvoiceDate"
                                    SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Invoice Date" Display="Dynamic"
                                    ValidationGroup="GridInvoiceRequired"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Invoice Amount">
                            <ItemTemplate>
                                <asp:Label ID="lblInvoicAmount" runat="server" Text='<%# Eval("AgentInvoiceAmount")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtInvoicAmount" runat="server" Width="80px" Text='<%# bind("AgentInvoiceAmount")%>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RFVAgentInvAmt" runat="server" ControlToValidate="txtInvoicAmount"
                                    SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Invoice Amount" Display="Dynamic"
                                    ValidationGroup="GridInvoiceRequired"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Currency">
                            <ItemTemplate>
                                <asp:Label ID="lblCurrency" runat="server" Text='<%# Eval("Currency")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddAgentCurrency" runat="server" DataSourceID="dataSourceCurrency"
                                    AppendDataBoundItems="true" DataValueField="lId" DataTextField="Currency" SelectedValue='<%#Bind("InvoiceCurrencyId") %>'>
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RFVAgentCurrency" runat="server" ControlToValidate="ddAgentCurrency"
                                    SetFocusOnError="true" InitialValue="0" Text="*" ErrorMessage="Please Select Invoice Currency"
                                    Display="Dynamic" ValidationGroup="GridInvoiceRequired"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                </div>
                </div>
                </fieldset>
                <asp:SqlDataSource ID="dataSourceCurrency" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetCurrencyMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceInvoiceDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="FOP_GetAgentInvoiceDetail" SelectCommandType="StoredProcedure"
                    UpdateCommand="FOP_updAgentInvoice" UpdateCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>