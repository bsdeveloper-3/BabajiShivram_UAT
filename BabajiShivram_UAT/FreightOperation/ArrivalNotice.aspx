<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
    CodeFile="ArrivalNotice.aspx.cs" Inherits="FreightOperation_ArrivalNotice" Title="Cargo Arrival Notice"
    Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="gvPager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <script type="text/javascript">

        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_beginRequest(beginRequest);

        function beginRequest() {
            prm._scrollPosition = null;
        }

    </script>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPanelDetail" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPanelDetail" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="ValSummaryFreightDetail" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false"></asp:ValidationSummary>
                 <asp:ValidationSummary ID="Valsummarycan" runat="server" ShowMessageBox="True"
                ShowSummary="False" ValidationGroup="CanRequired" CssClass="errorMsg" EnableViewState="false"></asp:ValidationSummary>
                <asp:HiddenField ID="hdnWeight" runat="server" />
                <asp:HiddenField ID="hdnVolume" runat="server" />
                <asp:HiddenField ID="hdnUploadPath" runat="server" />
                <asp:HiddenField ID="hdnIsGST" runat="server" Value="0" />
                <asp:HiddenField ID="hdnIsStateGST" runat="server" Value="0" />
                <asp:HiddenField ID="hdnModeId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnCountryCode" runat="server" />
            </div>
            <div class="clear">
            </div>
            <fieldset>
                <legend>CAN</legend>
                <div class="m clear">
                    <asp:Button ID="btnUpdate" runat="server" Text="Save" OnClick="btnSubmit_Click" ValidationGroup="Required"></asp:Button>
                    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CausesValidation="False"
                        Text="Cancel"></asp:Button>
                </div>
                <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
                    <tr>
                        <td>Job No
                        </td>
                        <td>
                            <asp:Label ID="lblJobNo" runat="server"></asp:Label>
                        </td>
                        <td>Booking Month
                        </td>
                        <td>
                            <asp:Label ID="lblBookingMonth" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>IGM No
                            <asp:RequiredFieldValidator ID="RfvIGMNo" runat="server" ControlToValidate="txtIGMNo"
                                Display="Dynamic" SetFocusOnError="true" Text="*" ErrorMessage="Please Enter IGM Number."
                                ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtIGMNo" runat="server" TabIndex="1"></asp:TextBox>
                        </td>
                        <td>IGM Date
                            <AjaxToolkit:CalendarExtender ID="calIGMDate" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgIGMDate" PopupPosition="BottomRight"
                                TargetControlID="txtIGMDate">
                            </AjaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtIGMDate" runat="server" Width="100px" TabIndex="2"></asp:TextBox>
                            <asp:Image ID="imgIGMDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                                runat="server" />
                            <AjaxToolkit:MaskedEditExtender ID="MskIGMDate" TargetControlID="txtIGMDate" Mask="99/99/9999"
                                MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                            </AjaxToolkit:MaskedEditExtender>
                            <AjaxToolkit:MaskedEditValidator ID="MskIGMHBLDate" ControlExtender="MskIGMDate"
                                ControlToValidate="txtIGMDate" IsValidEmpty="false" InvalidValueMessage="IGM Date is invalid"
                                InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true" Display="Dynamic"
                                EmptyValueMessage="Please Enter IGM Date" EmptyValueBlurredText="Required" MinimumValueMessage="Invalid IGM Date"
                                MaximumValueMessage="Invalid IGM Date" MinimumValue="01/01/2015" MaximumValue="01/01/2026"
                                runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Item No
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemNo" runat="server" TabIndex="3"></asp:TextBox>
                        </td>
                        <td>
                            <asp:LinkButton ID="lnkCreateCANPdf" Text="Create CAN" runat="server" OnClick="lnkCreateCANPdf_Click"></asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="lnkEmailToCustomer" Text="Email CAN To Customer" runat="server"
                                Visible="false"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td>ATA
                            <AjaxToolkit:CalendarExtender ID="calATA" runat="server" Enabled="True" EnableViewState="False"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgATADate" PopupPosition="BottomRight"
                                TargetControlID="txtATA">
                            </AjaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtATA" runat="server" Width="100px" TabIndex="4"></asp:TextBox>
                            <asp:Image ID="imgATADate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                                runat="server" />
                            <AjaxToolkit:MaskedEditExtender ID="MaskEdtATA" TargetControlID="txtIGMDate" Mask="99/99/9999"
                                MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                            </AjaxToolkit:MaskedEditExtender>
                            <AjaxToolkit:MaskedEditValidator ID="MskValidATA" ControlExtender="MaskEdtATA" ControlToValidate="txtATA"
                                IsValidEmpty="false" InvalidValueMessage="ATA Date is invalid" InvalidValueBlurredMessage="Invalid Date"
                                SetFocusOnError="true" Display="Dynamic" EmptyValueMessage="Please Enter ATA Date"
                                EmptyValueBlurredText="Required" MinimumValueMessage="Invalid ATA Date" MaximumValueMessage="Invalid ATA Date"
                                MinimumValue="01/01/2015" runat="Server" ValidationGroup="Required"></AjaxToolkit:MaskedEditValidator>
                        </td>
                        <td>Upload CAN PDF
                        </td>
                        <td>
                            <asp:FileUpload ID="fuCANCopy" runat="server" TabIndex="5" />
                        </td>
                    </tr>

                    <asp:Panel ID="pnlSea" runat="server" Visible="false">
                        <tr>
                            <td>Container 20"
                            </td>
                            <td>
                                <asp:Label ID="lblCon20" runat="server"></asp:Label>
                            </td>
                            <td>Container 40"
                            </td>
                            <td>
                                <asp:Label ID="lblCon40" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>LCL (CBM)
                            </td>
                            <td>
                                <asp:Label ID="lblVolume" runat="server"></asp:Label>
                            </td>
                            <td>Container Type
                            </td>
                            <td>
                                <asp:Label ID="lblContainerTypeName" runat="server"></asp:Label>
                                <asp:HiddenField ID="hdnContainerTypeId" Value="0" runat="server" />
                            </td>
                        </tr>
                    </asp:Panel>
                    <asp:Panel ID="pnlAir" runat="server" Visible="false">
                        <tr>
                            <td>No of Packages
                            </td>
                            <td>
                                <asp:Label ID="lblnoOfPkgs" runat="server"></asp:Label>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                    </asp:Panel>
                    <tr>
                        <td>Gross Weight (Kgs)
                        </td>
                        <td>
                            <asp:Label ID="lblGrossWT" runat="server"></asp:Label>
                        </td>
                        <td>Chargeable Weight (Kgs)
                        </td>
                        <td>
                            <asp:Label ID="lblChargeWT" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Place of Supply
                        </td>
                        <td>
                            <asp:Label ID="lblState" runat="server"></asp:Label>
                        </td>
                        <td>GSTN
                        </td>
                        <td>
                            <asp:Label ID="lblGSTN" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Remark
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark" Width="80%" TextMode="MultiLine" TabIndex="6" MaxLength="400" runat="server" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <div id="GSTMessage" style="margin-left: 100px;">
                <asp:Label ID="lblGSTMessage" runat="server" CssClass="errorMsg"></asp:Label>
            </div>
            <fieldset>
                <div align="center">
                    <asp:Label ID="lblInvoiceError" runat="server" EnableViewState="false"></asp:Label>
                </div>
                <div>
                    <asp:Button ID="btnSaveInvoice" runat="server" Text="Save" OnClick="btnSaveInvoice_Click" TabIndex="14" ValidationGroup="CanRequired"/>
                    <%-- &nbsp;&nbsp; Tax Rate %&nbsp;&nbsp;--%>
                    <asp:TextBox ID="txtTaxRate" runat="server" AutoPostBack="true" OnTextChanged="ddInvoice_SelectedIndexChanged"
                        Width="80px" TabIndex="7" Enabled="false" Visible="false"></asp:TextBox>
                </div>
                <legend>Invoice</legend>
                <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
                    <tr>
                        <td>Invoice Item
                        </td>
                        <td>
                            <asp:DropDownList ID="ddInvoice" runat="server" DataSourceID="dataSourceInvoiceMS"
                                TabIndex="8" DataValueField="lid" DataTextField="FieldName" AppendDataBoundItems="true"
                                AutoPostBack="true" OnTextChanged="ddInvoice_SelectedIndexChanged" Width="150px">
                                <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;
                            <asp:Label ID="lblUOM" runat="server"></asp:Label>&nbsp; <a id="lnkDataTooltip" href="#"
                                data-tooltip="" runat="server">
                                <img src="../Images/info-icon.png" width="14px" height="14px" alt="info" /></a>
                        </td>
                        <td>
                            <asp:Label ID="lblRate" Text="Rate" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRate" runat="server" AutoPostBack="true" OnTextChanged="ddInvoice_SelectedIndexChanged"
                                TabIndex="9" Width="80px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Currency
                        </td>
                        <td>
                            <asp:DropDownList ID="ddCurrency" runat="server" DataSourceID="dataSourceCurrency"
                                AppendDataBoundItems="true" DataValueField="lId" DataTextField="Currency" TabIndex="10">
                                <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="txtUSDRate" runat="server" placeholder="USD Rate" Width="80px" Visible="false"
                                TabIndex="10" AutoPostBack="true" OnTextChanged="ddInvoice_SelectedIndexChanged"></asp:TextBox>
                        </td>
                        <td>Exchange Rate (Rs)
                        </td>
                        <td>
                            <asp:TextBox ID="txtExchangeRate" runat="server" Width="50px" AutoPostBack="true"
                                OnTextChanged="ddInvoice_SelectedIndexChanged" TabIndex="11"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Min Unit
                        </td>
                        <td>
                            <asp:TextBox ID="txtMinUnit" runat="server" AutoPostBack="true" OnTextChanged="ddInvoice_SelectedIndexChanged"
                                TabIndex="12" Width="80px"></asp:TextBox>
                        </td>
                        <td>Min Amount
                        </td>
                        <td>
                            <asp:TextBox ID="txtMinAmount" runat="server" AutoPostBack="true" OnTextChanged="ddInvoice_SelectedIndexChanged"
                                TabIndex="13" Width="80px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Amount (Rs)
                        </td>
                        <td>
                            <asp:Label ID="lblAmount" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblTaxName" runat="server" Text="Tax"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblTaxAmount" runat="server"></asp:Label>
                            <asp:HiddenField ID="hdnIsTaxRequired" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>Total Amount (Rs)
                        </td>
                        <td>
                            <asp:Label ID="lblTotalAmount" runat="server"></asp:Label>
                        </td>
                        <td>CAN
                               <asp:RequiredFieldValidator ID="RFVcan" runat="server" ControlToValidate="rblIsCAN"
                                Display="Dynamic" SetFocusOnError="true" Text="*" ErrorMessage="Please select CAN Yes/No."
                                ValidationGroup="CanRequired"></asp:RequiredFieldValidator>
                         </td><%--Implemented new CAN redio button--%>
                         <td>
                              <asp:RadioButtonList ID="rblIsCAN" runat="server" RepeatDirection="Horizontal">
                                     <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                     <asp:ListItem Text="No" Value="0"></asp:ListItem>
                               </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
                <div class="m">
                </div>
                <div>
                    <asp:GridView ID="gvCanInvoice" runat="server" DataSourceID="SqlDataSourceCanInvoice"
                        DataKeyNames="InvoiceItemId" Width="100%" AllowPaging="True" PageSize="40" AllowSorting="true"
                        PagerStyle-CssClass="pgr" PagerSettings-Position="TopAndBottom" OnRowCommand="gvCanInvoice_RowCommand"
                        CssClass="table" AutoGenerateColumns="False">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CHECK">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelect_CheckedChanged" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FieldName" HeaderText="INVOICE ITEM" />
                            <asp:BoundField DataField="SacCode" HeaderText="HSN/SAC" SortExpression="SacCode" ReadOnly="true" />
                            <asp:BoundField DataField="ReportHeader" HeaderText="REPORT HEADER" />
                            <asp:BoundField DataField="UnitOfMeasurement" HeaderText="UOM" />
                            <asp:BoundField DataField="Rate" HeaderText="RATE" SortExpression="Rate" />
                            <%--<asp:BoundField DataField="Currency" HeaderText="Currency" SortExpression="Currency"/>
                        <asp:BoundField DataField="ExchangeRate" HeaderText="Exchange Rate" SortExpression="ExchangeRate"/>--%>
                            <asp:TemplateField HeaderText="CURRENCY" SortExpression="Currency">
                                <ItemTemplate>
                                    <asp:Label ID="lblCurrency" runat="server" Text='<%#Eval("Currency") + " - " + Eval("ExchangeRate")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:BoundField DataField="TaxRequired" HeaderText="Taxable" SortExpression="TaxRequired"/>--%>
                            <asp:TemplateField HeaderText="AMOUNT (Rs)" SortExpression="Amount">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmount" runat="server" Text='<%#Eval("Amount")%>' />
                                    <%--<asp:BoundField DataField="Amount" HeaderText="AMOUNT (Rs)" SortExpression="Amount" ReadOnly="true"/>--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:BoundField DataField="TaxPercentage" HeaderText="TAX %" SortExpression="TaxPercentage" />
                          <asp:BoundField DataField="Tax14" HeaderText="SERVICE TAX" SortExpression="Service Tax" DataFormatString="{0:f2}" />
                        <asp:BoundField DataField="SBC" HeaderText="SBC" SortExpression="Service Tax"  DataFormatString="{0:f2}" />--%>
                            <%--<asp:BoundField DataField="SBC" HeaderText="SBC" SortExpression="SBC"/>
                            <asp:BoundField DataField="TaxAmount" HeaderText="TAX (Rs)" SortExpression="TaxAmount" />--%>
                            <asp:BoundField DataField="CGstTax" HeaderText="CGST %" SortExpression="CGstTax" ReadOnly="true" />
                            <asp:BoundField DataField="CGstTaxAmount" HeaderText="CGST (Rs)" SortExpression="CGstTaxAmount" ReadOnly="true" />
                            <asp:BoundField DataField="SGstTax" HeaderText="SGST %" SortExpression="SGstTax" ReadOnly="true" />
                            <asp:BoundField DataField="SGstTaxAmount" HeaderText="SGST (Rs)" SortExpression="SGstTaxAmount" ReadOnly="true" />
                            <asp:BoundField DataField="IGstTax" HeaderText="IGST %" SortExpression="IGstTax" ReadOnly="true" />
                            <asp:BoundField DataField="IGstTaxAmount" HeaderText="IGST (Rs)" SortExpression="IGstTaxAmount" ReadOnly="true" />
                            <asp:TemplateField HeaderText="TOTAL (Rs)" SortExpression="TotalAmount">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotal" runat="server" Text='<%#Eval("TotalAmount") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="IsCAN" HeaderText="IsCAN" SortExpression="IsCAN" ReadOnly="true" />
                            <asp:BoundField DataField="dtDate" HeaderText="DATE" DataFormatString="{0:dd/MM/yyyy}"
                                SortExpression="dtDate" />
                            <asp:BoundField DataField="CreatedBy" HeaderText="USER" SortExpression="CreatedBy" />
                            <asp:TemplateField HeaderText="REMOVE">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkRemove" runat="server" Text="Remove" CommandName="Remove"
                                        CommandArgument='<%#Eval("lid") %>' OnClientClick="return confirm('Are you sure wants to Remove Invoice Item?');"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerTemplate>
                            <gvPager:GridViewPager ID="GridViewPager1" runat="server" />
                        </PagerTemplate>
                    </asp:GridView>
                </div>
                <asp:SqlDataSource ID="dataSourceInvoiceMS" runat="server" ConnectionString="<%$ ConnectionStrings:ConBsImport %>"
                    SelectCommand="FOP_GetInvoiceFieldMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="dataSourceCurrency" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetCurrencyMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </fieldset>
            <asp:SqlDataSource ID="SqlDataSourceCanInvoice" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="FOP_GetCANInvoiceDetail" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="EnqId" SessionField="EnqId" />
                </SelectParameters>
            </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
