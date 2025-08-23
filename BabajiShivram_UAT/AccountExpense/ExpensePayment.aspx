<%@ Page Title="Job Expense Payment Details" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ExpensePayment.aspx.cs"
    Inherits="AccountExpense_ExpensePayment" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <script type="text/javascript">
        function OnACCodeSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnACCodeId').value = results.AcCodeId;
        }

        function OnAcNameSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnACCodeId').value = results.AcCodeId;
        }

        function OnVendorCodeSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnVendorCodeId').value = results.AcCodeId;
        }

        function OnJobSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnJobId').value = results.JobId;
        }

    </script>

    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upExpPayment" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upExpPayment" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="vsAddExpense" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="vgAddExpense" CssClass="errorMsg" EnableViewState="false" />
                <asp:ValidationSummary ID="vsAddExpense_footer" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="vgAddExpense_footer" CssClass="errorMsg" EnableViewState="false" />
                <asp:HiddenField ID="hdnACCodeId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnJobId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnVendorCodeId" runat="server" Value="0" />
                <div id="divwidthCode" runat="server"></div>
                <div id="divwidthCust_Loc" runat="server"></div>
                <div id="divwidthName" runat="server"></div>
            </div>
            <div class="clear">
            </div>
            <fieldset class="fieldset-AutoWidth">
                <legend>Job Expense Payment Details</legend>

                <div class="m clear">
                    <asp:Button ID="btnSubmit" Text="Save" OnClick="btnSubmit_Click" runat="server" ValidationGroup="Required"
                        TabIndex="39" />
                    <asp:Button ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false" TabIndex="40"
                        runat="server" />
                </div>

                <fieldset>
                    <div>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>Voucher No
                                </td>
                                <td>
                                    <asp:TextBox ID="txtVoucherNo" runat="server" Enabled="false" Width="150px" ToolTip="Enter Voucher No."></asp:TextBox>
                                </td>
                                <td>Date</td>
                                <td>
                                    <asp:TextBox ID="txtDate" runat="server" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Vendor Code</td>
                                <td>
                                    <asp:TextBox ID="txtVendorCode" Width="150px" runat="server" ToolTip="Enter Voucher Code."
                                        CssClass="SearchTextbox" placeholder="Search" TabIndex="1" AutoPostBack="true" OnTextChanged="txtVendorCode_TextChanged"></asp:TextBox>
                                    <div id="divwidthVendorCode" runat="server">
                                    </div>
                                    <cc1:AutoCompleteExtender ID="VendorCodeExtender" runat="server" TargetControlID="txtVendorCode"
                                        CompletionListElementID="divwidthVendorCode" ServicePath="~/WebService/AccountCodeAutoComplete.asmx"
                                        ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthVendorCode"
                                        ContextKey="1234" UseContextKey="True" OnClientItemSelected="OnVendorCodeSelected"
                                        CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                        CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                    </cc1:AutoCompleteExtender>
                                    &nbsp;&nbsp;
                                     <asp:Label ID="lblVendorCodeName" runat="server" Width="180px"></asp:Label>
                                </td>
                                <td>Currency</td>
                                <td>
                                    <asp:DropDownList ID="ddCurrency" runat="server" DataSourceID="dataSourceCurrency" AppendDataBoundItems="true" TabIndex="2" ToolTip="Select Currency."
                                        DataValueField="lId" DataTextField="Currency" Width="200px">
                                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp; 
                                    Rate
                                    <asp:TextBox ID="txtRate" runat="server" Width="100px" TabIndex="3" ToolTip="Enter Rate."></asp:TextBox>
                                </td>
                            </tr>

                        </table>
                    </div>
                </fieldset>
                <fieldset>
                    <div>
                        <asp:GridView ID="gvExpenseDetails" runat="server" CssClass="table" ShowFooter="true" PagerStyle-CssClass="pgr"
                            AllowSorting="true" AutoGenerateColumns="false" AllowPaging="true"
                            OnPageIndexChanging="gvExpenseDetails_PageIndexChanging" PageSize="20" PagerSettings-Position="TopAndBottom"
                            DataKeyNames="lid" OnRowCommand="gvExpenseDetails_RowCommand" OnRowUpdating="gvExpenseDetails_RowUpdating"
                            OnRowDeleting="gvExpenseDetails_RowDeleting" OnRowEditing="gvExpenseDetails_RowEditing" Width="100%"
                            OnRowCancelingEdit="gvExpenseDetails_RowCancelingEdit" OnRowDataBound="gvExpenseDetails_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="2%">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="A/C Code" ItemStyle-Width="8%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAcCode" runat="server" Text='<%#Eval("ACCode") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblAcCode" runat="server" Text='<%#Eval("ACCode") %>'></asp:Label>
                                        <asp:TextBox ID="txtAcCode" runat="server" Text='<%#Eval("ACCode") %>' MaxLength="50" TabIndex="1" ToolTip="Enter Account Code."
                                            CssClass="SearchTextbox" placeholder="Search" AutoPostBack="true" OnTextChanged="txtAcCode_TextChanged" Visible="false"></asp:TextBox>
                                        <cc1:AutoCompleteExtender ID="AcCodeExtender" runat="server" TargetControlID="txtAcCode"
                                            CompletionListElementID="divwidthCode" ServicePath="~/WebService/AccountCodeAutoComplete.asmx"
                                            ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCode"
                                            ContextKey="1234" UseContextKey="True" OnClientItemSelected="OnACCodeSelected"
                                            CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                            CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                        </cc1:AutoCompleteExtender>
                                        <asp:RequiredFieldValidator ID="rfvAcCode" runat="server" ControlToValidate="txtAcCode" SetFocusOnError="true"
                                            Display="Dynamic" ErrorMessage="Please select A/C Code." Text="*" ValidationGroup="vgAddExpense"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="A/C Name" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAcName" runat="server" Text='<%#Eval("ACName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblAcName" runat="server" Text='<%#Eval("ACName") %>' Visible="false"></asp:Label>
                                        <asp:TextBox ID="txtAcName" runat="server" Text='<%#Eval("ACName") %>' Width="300px" MaxLength="50" TabIndex="2" ToolTip="Enter Account Name."
                                            CssClass="SearchTextbox" placeholder="Search" AutoPostBack="true" OnTextChanged="txtAcName_TextChanged"></asp:TextBox>
                                        <cc1:AutoCompleteExtender ID="AcNameExtender" runat="server" TargetControlID="txtAcName"
                                            CompletionListElementID="divwidthAcName" ServicePath="~/WebService/AccountNameAutoComplete.asmx"
                                            ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthAcName"
                                            ContextKey="1234" UseContextKey="True" OnClientItemSelected="OnAcNameSelected"
                                            CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                            CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                        </cc1:AutoCompleteExtender>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Job Ref No" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblJobNo" runat="server" Text='<%#Eval("JobRefNo") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblJobNo" runat="server" Text='<%#Eval("JobRefNo") %>'></asp:Label>
                                        <asp:TextBox ID="txtJobNo" runat="server" Text='<%#Eval("JobRefNo") %>' MaxLength="200" TabIndex="3" Width="200px" ToolTip="Enter Job Ref No."
                                            CssClass="SearchTextbox" placeholder="Search" AutoPostBack="true" Visible="false"></asp:TextBox>
                                        <cc1:AutoCompleteExtender ID="JobDetailExtender" runat="server" TargetControlID="txtJobNo"
                                            CompletionListElementID="divwidthCust_Loc" ServicePath="~/WebService/JobNumberAutoComplete.asmx"
                                            ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCust_Loc"
                                            ContextKey="4567" UseContextKey="True" OnClientItemSelected="OnJobSelected"
                                            CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                            CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                        </cc1:AutoCompleteExtender>
                                        <asp:RequiredFieldValidator ID="rfvJobNo" runat="server" ControlToValidate="txtJobNo" SetFocusOnError="true"
                                            Display="Dynamic" ErrorMessage="Please select Job Ref No." ValidationGroup="vgAddExpense"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Debit" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDebit" runat="server" Text='<%#Eval("Debit") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDebit" runat="server" Text='<%#Eval("Debit") %>' Width="100px" MaxLength="200" TabIndex="4"></asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Credit" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCredit" runat="server" Text='<%#Eval("Credit") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtCredit" runat="server" Text='<%#Eval("Credit") %>' Width="100px" MaxLength="200" TabIndex="5"></asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit/Delete" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server" Text="Edit" Font-Underline="true"></asp:LinkButton>
                                        &nbsp;<asp:LinkButton ID="lnkDelete" CommandName="Delete" ToolTip="Delete" Width="22" OnClientClick="return confirm('Sure to delete?');" runat="server" Text="Delete" Font-Underline="true"></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="lnkUpdate" ValidationGroup="vgAddExpense" CommandName="Update" ToolTip="Update" Width="35" runat="server" Text="Update" Font-Underline="true"></asp:LinkButton>
                                        &nbsp;<asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" Width="39" runat="server" Text="Cancel" Font-Underline="true"></asp:LinkButton>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <br />
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="padding-left: 200px; background-color: #cbcbdc;" bgcolor="white">
                            <tr>
                                <td style="width: 45px"></td>
                                <td style="width: 11px">
                                    <asp:TextBox ID="txtAcCode_footer" runat="server" Text='<%#Eval("ACCode") %>' MaxLength="50" TabIndex="1" ToolTip="Enter Account Code."
                                        CssClass="SearchTextbox" placeholder="Search" AutoPostBack="true" Width="105px" OnTextChanged="txtAcCode_footer_TextChanged"></asp:TextBox>
                                    <cc1:AutoCompleteExtender ID="AcCodeExtender" runat="server" TargetControlID="txtAcCode_footer"
                                        CompletionListElementID="divwidthCode" ServicePath="~/WebService/AccountCodeAutoComplete.asmx"
                                        ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCode"
                                        ContextKey="1234" UseContextKey="True" OnClientItemSelected="OnACCodeSelected"
                                        CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                        CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                    </cc1:AutoCompleteExtender>
                                    <asp:RequiredFieldValidator ID="rfvAcCode_footer" runat="server" ControlToValidate="txtAcCode_footer" SetFocusOnError="true"
                                        Display="Dynamic" ErrorMessage="Please select A/C Code." Text="*" ValidationGroup="vgAddExpense_footer"></asp:RequiredFieldValidator></td>
                                <td style="width: 28%">
                                    <asp:TextBox ID="txtAcName_footer" runat="server" Width="300px" Text='<%#Eval("ACName") %>' MaxLength="50" TabIndex="2" ToolTip="Enter Account Name."
                                        CssClass="SearchTextbox" placeholder="Search" AutoPostBack="true" OnTextChanged="txtAcName_footer_TextChanged"></asp:TextBox>
                                    <cc1:AutoCompleteExtender ID="AcNameExtender" runat="server" TargetControlID="txtAcName_footer"
                                        CompletionListElementID="divwidthAcName" ServicePath="~/WebService/AccountNameAutoComplete.asmx"
                                        ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthAcName"
                                        ContextKey="1234" UseContextKey="True" OnClientItemSelected="OnAcNameSelected"
                                        CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                        CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                    </cc1:AutoCompleteExtender>
                                </td>
                                <td style="width: 10%">
                                    <asp:TextBox ID="txtJobNo_footer" runat="server" Text='<%#Eval("JobRefNo") %>' MaxLength="200" TabIndex="3" ToolTip="Enter Job Ref No."
                                        CssClass="SearchTextbox" Width="145px" placeholder="Search" AutoPostBack="true"></asp:TextBox>
                                    <cc1:AutoCompleteExtender ID="JobDetailExtender" runat="server" TargetControlID="txtJobNo_footer"
                                        CompletionListElementID="divwidthCust_Loc" ServicePath="~/WebService/JobNumberAutoComplete.asmx"
                                        ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCust_Loc"
                                        ContextKey="4567" UseContextKey="True" OnClientItemSelected="OnJobSelected"
                                        CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                        CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                    </cc1:AutoCompleteExtender>
                                    <asp:RequiredFieldValidator ID="rfvJobNo_footer" runat="server" ControlToValidate="txtJobNo_footer" SetFocusOnError="true"
                                        Display="Dynamic" ErrorMessage="Please select Job Ref No." Text="*" ValidationGroup="vgAddExpense_footer"></asp:RequiredFieldValidator></td>
                                <td style="width: 14%">
                                    <asp:TextBox ID="txtDebit_footer" runat="server" Text='<%#Eval("Debit") %>' Width="140px" MaxLength="200" TabIndex="4" ToolTip="Enter debit."></asp:TextBox></td>
                                <td style="width: 10%">
                                    <asp:TextBox ID="txtCredit_footer" runat="server" Text='<%#Eval("Credit") %>' Width="140px" MaxLength="200" TabIndex="5" ToolTip="Enter credit."></asp:TextBox></td>
                                <td>
                                    <asp:LinkButton ID="lnkAdd" ValidationGroup="vgAddExpense_footer" OnClick="lnkAdd_OnClick" ToolTip="Add" Width="22" runat="server" Text="Add" Font-Underline="true" TabIndex="6"></asp:LinkButton></td>
                            </tr>
                        </table>
                    </div>
                </fieldset>
                <fieldset>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>Cheque No</td>
                            <td>
                                <asp:TextBox ID="txtChequeNo" Width="150px" runat="server" ToolTip="Enter Cheque No." TabIndex="4" MaxLength="10"></asp:TextBox>
                            </td>
                            <td>Cheque Date
                                <cc1:CalendarExtender ID="calChequeDate" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="imgChequeDate"
                                    Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtChequeDate">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="meeChequeDate" TargetControlID="txtChequeDate" Mask="99/99/9999" MessageValidatorTip="true"
                                    MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="mevChequeDate" ControlExtender="meeChequeDate" ControlToValidate="txtChequeDate" IsValidEmpty="true"
                                    InvalidValueMessage="Cheque Date is invalid" InvalidValueBlurredMessage="Invalid Cheque Date" SetFocusOnError="true"
                                    MinimumValueMessage="Invalid Cheque Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2014" MaximumValue="31/12/2025"
                                    runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChequeDate" runat="server" Width="100px" placeholder="dd/mm/yyyy" TabIndex="5" ToolTip="Enter Cheque Date."></asp:TextBox>
                                <asp:Image ID="imgChequeDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>Bank Name
                            </td>
                            <td>
                                <asp:TextBox ID="txtBankName" runat="server" ToolTip="Enter Bank Name." TabIndex="6" Width="220px"></asp:TextBox>
                            </td>
                            <td>Favouring</td>
                            <td>
                                <asp:TextBox ID="txtFavouring" runat="server" ToolTip="Enter Favouring." TabIndex="7" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Narration</td>
                            <td colspan="3">
                                <asp:TextBox ID="txtNarration" runat="server" ToolTip="Enter Narration." TabIndex="8" TextMode="MultiLine" Rows="3" Width="90%"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="DataSourceJobExpenseDetails" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetApprovedJobExpense" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="dataSourceCurrency" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetCurrencyMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

