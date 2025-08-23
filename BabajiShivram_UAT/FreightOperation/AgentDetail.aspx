<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AgentDetail.aspx.cs" Inherits="FreightOperation_AgentDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp1" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <script src="../JS/GridViewCellEdit.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript" src="../JS/CheckBoxListPCDDocument.js"></script>

    <asp:UpdatePanel ID="upPanelDetail" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="ValSummaryDetail" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="RequiredInvoice" CssClass="errorMsg" EnableViewState="false" />
                <asp:HiddenField ID="hdnTypeId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnUploadPath" runat="server" />

            </div>
            <div id="dvAgentInvoice" runat="server">
                    <fieldset id="Agentfield" runat="server">
                        <legend>Agent Invoice Detail</legend>
                        <div>
                            <asp:Button ID="btnSaveAgentInvoice" runat="server" Text="Save" OnClick="btnSaveAgentInvoice_Click" ValidationGroup="RequiredInvoice" />
                            <asp:Button ID="btnCancelInvoice" runat="server" OnClick="btnCancelInvoice_Click" CausesValidation="False"
                                Text="Cancel" TabIndex="20" />
                        </div>
                        <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
                            <tr>
                                <td>Job No
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lblJobNo" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Agent Name
                            <asp:RequiredFieldValidator ID="RFVName" runat="server" ControlToValidate="ddAgent" Display="Dynamic"
                                InitialValue="0" SetFocusOnError="true" Text="*" ErrorMessage="Please Select Agent Name." ValidationGroup="RequiredInvoice"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddAgent" runat="server"></asp:DropDownList>
                                    <%--<asp:TextBox ID="txtAgentName" runat="server"></asp:TextBox>--%>
                                </td>
                                <td>Invoice Received Date
                            <AjaxToolkit:CalendarExtender ID="CalRcvdDate" runat="server" Enabled="True" EnableViewState="false"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgRcvdDate" PopupPosition="BottomRight"
                                TargetControlID="txtReceivedDate">
                            </AjaxToolkit:CalendarExtender>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReceivedDate" runat="server" Width="100px"></asp:TextBox>
                                    <asp:Image ID="imgRcvdDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                    <%--<AjaxToolkit:MaskedEditExtender ID="MskExtRcvdDate" TargetControlID="txtReceivedDate" Mask="99/99/9999" MessageValidatorTip="true"
                                        MaskType="Date" AutoComplete="false" runat="server">
                                    </AjaxToolkit:MaskedEditExtender>
                                    <AjaxToolkit:MaskedEditValidator ID="MskValRcvdDate" ControlExtender="MskExtRcvdDate" ControlToValidate="txtReceivedDate" IsValidEmpty="false"
                                        InvalidValueMessage="Bill Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                        EmptyValueMessage="Please Enter Invoice Rcvd Date" EmptyValueBlurredText="Required" MinimumValueBlurredText="Invalid Date" MinimumValueMessage="Invalid Received Date"
                                        MaximumValueBlurredMessage="Invalid Date" MaximumValueMessage="Invalid Received Date" MinimumValue="01/01/2016" MaximumValue="01/01/2020"
                                        runat="Server" ValidationGroup="RequiredInvoice"></AjaxToolkit:MaskedEditValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>Agent/Vendor Invoice No
                            <asp:RequiredFieldValidator ID="RFVInvNo" runat="server" ControlToValidate="txtInvoiceNo" Display="Dynamic"
                                SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Invoice No." ValidationGroup="RequiredInvoice"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtInvoiceNo" runat="server"></asp:TextBox>
                                </td>
                                <td>Invoice Date
                            <AjaxToolkit:CalendarExtender ID="calInvoiceDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgInvoiceDate" PopupPosition="BottomRight"
                                TargetControlID="txtInvoiceDate">
                            </AjaxToolkit:CalendarExtender>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtInvoiceDate" runat="server" Width="100px"></asp:TextBox>
                                    <asp:Image ID="imgInvoiceDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                    <%--<AjaxToolkit:MaskedEditExtender ID="MskExtInvoiceDate" TargetControlID="txtInvoiceDate" Mask="99/99/9999" MessageValidatorTip="true"
                                        MaskType="Date" AutoComplete="false" runat="server">
                                    </AjaxToolkit:MaskedEditExtender>
                                    <AjaxToolkit:MaskedEditValidator ID="MskValInvoiceDate" ControlExtender="MskExtInvoiceDate" ControlToValidate="txtInvoiceDate" IsValidEmpty="false"
                                        InvalidValueMessage="Invoice Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                        EmptyValueMessage="Please Enter Invoice Date" EmptyValueBlurredText="Required" MinimumValueMessage="Invalid Date"
                                        MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="01/01/2020"
                                        runat="Server" ValidationGroup="RequiredInvoice"></AjaxToolkit:MaskedEditValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>Invoice Amount
                            <asp:RequiredFieldValidator ID="RFVInvoiceAmount" runat="server" ControlToValidate="txtInvoiceAmount" Display="Dynamic" InitialValue=""
                                SetFocusOnError="true" Text="*" ErrorMessage="Required" ValidationGroup="RequiredInvoice"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="CompValAmount" runat="server" ControlToValidate="txtInvoiceAmount"
                                        Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" ErrorMessage="Invalid Amount"
                                        Display="Dynamic" ValidationGroup="RequiredInvoice"></asp:CompareValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtInvoiceAmount" runat="server" MaxLength="15"></asp:TextBox>
                                </td>
                                <td>Invoice Currency
                           <asp:RequiredFieldValidator ID="RFVCurrency" runat="server" ControlToValidate="ddCurrency" Display="Dynamic" InitialValue="0"
                               SetFocusOnError="true" Text="*" ErrorMessage="Please Select Invoice Currency." ValidationGroup="RequiredInvoice"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddCurrency" runat="server" DataSourceID="dataSourceCurrency" AppendDataBoundItems="true"
                                        DataValueField="lId" DataTextField="Currency">
                                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Agent Invoice
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="fuDocument" Display="Dynamic"
                                SetFocusOnError="true" Text="*" ErrorMessage="Please Upload Agent Invoice Copy." ValidationGroup="RequiredInvoice"></asp:RequiredFieldValidator>
                                </td>
                                <td colspan="3">
                                    <asp:FileUpload ID="fuDocument" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>Remark
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtInvoiceRemark" TextMode="MultiLine" Width="90%" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <div>
                            <div>
                                <asp:GridView ID="GridViewInvoiceDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" DataKeyNames="lId" DataSourceID="DataSourceInvoiceDetail" CellPadding="4"
                                    OnRowCommand="GridViewInvoiceDetail_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Agent" DataField="AgentInvoiceName" />
                                        <asp:BoundField HeaderText="Invoice No" DataField="AgentInvoiceNo" />
                                        <asp:BoundField HeaderText="Invoice Date" DataField="AgentInvoiceDate" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField HeaderText="Invoice Amount" DataField="AgentInvoiceAmount" />
                                        <asp:BoundField HeaderText="Currency" DataField="Currency" />
                                        <asp:TemplateField HeaderText="Download">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownload" runat="server" Text='<%#Eval("DocName") %>' CommandName="Download"
                                                    CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <%--<fieldset>
                                <legend>Download Document</legend>
                                <div>
                                <asp:GridView ID="gvFreightDocument" runat="server" AutoGenerateColumns="False" Width="99%"
                                    DataKeyNames="DocId" DataSourceID="FreightDocumentSqlDataSource" CssClass="table"
                                    CellPadding="4" PagerStyle-CssClass="pgr"
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
                                                <asp:LinkButton ID="lnlRemoveDocument" runat="server" Text="Remove" CommandName="RemoveDocument"
                                                    CommandArgument='<%#Eval("DocId") %>' OnClientClick="return confirm('Are you sure to remove document?');"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            </fieldset>--%>
                            
                            <asp:SqlDataSource ID="dataSourceCurrency" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetCurrencyMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                            <asp:SqlDataSource ID="DataSourceInvoiceDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="FOP_GetAgentInvoiceDetail" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:SqlDataSource ID="FreightDocumentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="FR_GetAgentInvoiceDocument" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:SessionParameter Name="EnqId" SessionField="EnqId" />

                        </SelectParameters>
                    </asp:SqlDataSource>
                        </div>
                    </fieldset>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

