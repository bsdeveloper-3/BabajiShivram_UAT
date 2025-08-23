<%@ Page Title="Expense Booking" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="ExpenseBooking.aspx.cs" Inherits="AccountExpense_ExpenseBooking" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:ScriptManager id="ScriptManager1" runat="server" scriptmode="Release"> </asp:ScriptManager>
    <script type="text/javascript">
        function OnJobSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnJobId').value = results.JobId;
        }
        function OnChargeCodeSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnChargeCode').value = results.Charge_Code;
        }
        function OnPayToSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnPayToCode').value = results.Par_Code;
        }
         function OnVendorSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnVendorCode').value = results.Vendor_Code;
        }
        
         function OnUserSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=hdnAGApprovalID.ClientID %>').value = results.Userid;
        }
    </script>
    <asp:UpdatePanel ID="upFillDetails" runat="server">
        <ContentTemplate>
            <div>
                <div align="center">
                    <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                    <asp:ValidationSummary ID="vsFillDetails" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
                </div>
                <fieldset>
                    <legend>Expense Booking</legend>
                    <div class="m clear">
                        <asp:Button ID="btnSaveBooking" Text="Save & Submit To Account" runat="server" ValidationGroup="Required" OnClick="btnSaveBooking_Click" />
                        <asp:Button ID="btnCancel" Text="Cancel" CausesValidation="false" runat="server" OnClick="btnCancel_Click" />
                        <asp:HiddenField ID="hdnJobId" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnCustomerId" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnPayToCode" runat="server" Value="" />
                        <asp:HiddenField ID="hdnVendorCode" runat="server" Value="" />
                        <asp:HiddenField ID="hdnChargeCode" runat="server" Value="" />
                        <asp:HiddenField ID="hdnChequeIssueID" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnAGApprovalID" runat="server" Value="0" />
                    </div>
                    <fieldset>
                    <legend>Job Detail</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td width="20%">Job Number
                                <asp:RequiredFieldValidator ID="rfvJobNo" runat="server" ValidationGroup="Required"
                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtJobNumber"
                                    Text="*" ErrorMessage="Please Select Job Number."></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtJobNumber" Width="160px" runat="server" ToolTip="Enter Job Number."
                                    CssClass="SearchTextbox" placeholder="Search" AutoPostBack="true"
                                    OnTextChanged="txtJobNumber_TextChanged"></asp:TextBox>
                                <div id="divwidthCust_Loc" runat="server">
                                </div>
                                <cc1:autocompleteextender id="JobDetailExtender" runat="server" TargetControlID="txtJobNumber"
                                    completionlistelementid="divwidthCust_Loc" ServicePath="~/WebService/JobNumberAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCust_Loc"
                                    ContextKey="4567" usecontextkey="True" OnClientItemSelected="OnJobSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:autocompleteextender>
                            </td>
                            <td>
                                Customer
                            </td>
                            <td>
                                <asp:Label ID="lblCustomer" runat="server" Width="290px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Expense For
                            </td>
                            <td>
                                <asp:DropDownList ID="ddRIMorAG" runat="server" OnSelectedIndexChanged="ddRIMorAG_OnSelectedIndexChanged" AutoPostBack="True">
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="RIM" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="AG" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                Pay Mode
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPaymentType" runat="server" AppendDataBoundItems="true" DataSourceID="DataSourcePaymentType" AutoPostBack="true"
                                    DataTextField="sName" DataValueField="lid" ToolTip="Select Type Of Payment." OnSelectedIndexChanged="ddlPaymentType_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Paid To
                            </td>
                            <td>
                            <asp:TextBox ID="txtPayTo" runat="server" MaxLength="100" CssClass="SearchTextbox" placeholder="Search" ></asp:TextBox>
                                <div id="divPayTo" runat="server">
                                </div>
                                <cc1:autocompleteextender id="AutocompletePayto" runat="server" targetcontrolid="txtPayTo"
                                    completionlistelementid="divPayTo" servicepath="~/WebService/FAPayToAutoComplete.asmx"
                                    servicemethod="GetCompletionList" minimumprefixlength="2" behaviorid="divPayTo"
                                    contextkey="3261" usecontextkey="True" onclientitemselected="OnPayToSelected"
                                    completionlistcssclass="AutoExtender" completionlistitemcssclass="AutoExtenderList"
                                    completionlisthighlighteditemcssclass="AutoExtenderHighlight">
                                </cc1:autocompleteextender>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                    </fieldset>

                    <asp:Panel ID="pnlChequeIsssued" runat="Server" visible="False">
                    <fieldset>
                    <legend>Cheque Detail</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>
                                Cheque No
                            </td>
                            <td>
                                <asp:Label ID="lblChequeNo" runat="server"></asp:Label>
                            </td>
                            <td>
                                Cheque Date
                            </td>
                            <td>
                                <asp:Label ID="lblChequeDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Bank Name
                            </td>
                            <td>
                                <asp:Label ID="lblBankName" runat="server"></asp:Label>
                            </td>
                            <td>
                                Paid To
                            </td>
                            <td >
                                <asp:Label ID="lblChequePaidTo" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Cheque Issued By
                            </td>
                            <td >
                                <asp:Label ID="lblChequeIssuedBy" runat="server"></asp:Label>
                            </td>
                            <td>
                                Cheque Issued Date
                            </td>
                            <td>
                                <asp:Label ID="lblChequeIssuedDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    </fieldset>
                    </asp:Panel>
                    <asp:Panel ID="pnlRIM" runat="Server" visible="False">
                    <fieldset>
                    <legend>RIM - Expense Detail</legend>
                        <%--<asp:Button ID="btnAddExpense" runat="server" Text="Add Expense" OnClick="btnAddExpense_Click" />--%>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>
                                Charge Code
                            </td>
                            <td >
                                <asp:TextBox ID="txtChargeCode" runat="server" CssClass="SearchTextbox" placeholder="Search" ></asp:TextBox>
                                <div id="divwidthCharge" runat="server">
                                </div>
                                <cc1:autocompleteextender id="AutocompleteChargeCode" runat="server" TargetControlID="txtChargeCode"
                                    completionlistelementid="divwidthChargeCode" ServicePath="~/WebService/FAPayToAutoComplete.asmx"
                                    ServiceMethod="GetFAChargeCode" MinimumPrefixLength="2" BehaviorID="divwidthCharge"
                                    ContextKey="3299" usecontextkey="True" OnClientItemSelected="OnChargeCodeSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:autocompleteextender>
                            </td>
                            <td>
                                GSTN
                            </td>
                            <td>
                                <asp:TextBox ID="txtGSTIN" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Invoice No
                            </td>
                            <td >
                                <asp:TextBox ID="txtInvoiceNo" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Invoice Date
                            </td>
                            <td>
                                <asp:TextBox ID="txtInvoiceDate" runat="server" Width="100px"></asp:TextBox>
                                <cc1:CalendarExtender ID="calInvoiceDate" runat="server" Enabled="True" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy"
                                    PopupPosition="BottomRight" TargetControlID="txtInvoiceDate">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Invoice Taxable Amount
                            </td>
                            <td >
                                <asp:TextBox ID="txtInvoiceTaxAmount" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                IGST %
                            </td>
                            <td>
                                <asp:TextBox ID="txtIGST" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                CGST %
                            </td>
                            <td>
                                <asp:TextBox ID="txtCGST" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                SGST %
                            </td>
                            <td>
                                <asp:TextBox ID="txtSGST" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Invoice Total Amount
                            </td>
                            <td >
                                <asp:TextBox ID="txtInvoiceTotalAmount" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Receipt Number
                            </td>
                            <td>
                                <asp:TextBox ID="txtReceiptNumber" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Receipt Date
                            </td>
                            <td >
                                <asp:TextBox ID="txtReceiptDate" runat="server"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalExtReceiptDate" runat="server" Enabled="True" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy"
                                    PopupPosition="BottomRight" TargetControlID="txtReceiptDate">
                                </cc1:CalendarExtender>
                            </td>
                            <td>
                                Vendor Name
                            </td>
                            <td>
                                <asp:TextBox ID="txtVendorName" runat="server" MaxLength="100" CssClass="SearchTextbox" placeholder="Search" ></asp:TextBox>
                                <div id="divVendor" runat="server">
                                </div>
                                <cc1:autocompleteextender id="PatToExtender" runat="server" targetcontrolid="txtVendorName"
                                    completionlistelementid="divVendor" servicepath="~/WebService/FAPayToAutoComplete.asmx"
                                    servicemethod="GetFAVendorCode" minimumprefixlength="2" behaviorid="divVendor"
                                    contextkey="9811" usecontextkey="True" onclientitemselected="OnVendorSelected"
                                    completionlistcssclass="AutoExtender" completionlistitemcssclass="AutoExtenderList"
                                    completionlisthighlighteditemcssclass="AutoExtenderHighlight">
                                </cc1:autocompleteextender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Narration
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtNarration" runat="server" TextMode="MultiLine" Columns="3" MaxLength="200"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    </fieldset>
                    </asp:Panel>
                    <asp:Panel ID="pnlAG" runat="Server" visible="False">
                    <fieldset>
                    <legend>AGENCY - Expense Detail</legend>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>
                                Charge Code
                            </td>
                            <td>
                                <asp:TextBox ID="txtAGChargeName" runat="server" CssClass="SearchTextbox" placeholder="Search" 
                                    AutoPostBack="true"></asp:TextBox>
                                <div id="divAGCharge" runat="server">
                                </div>
                                <cc1:autocompleteextender id="Autocompleteextender1" runat="server" TargetControlID="txtAGChargeName"
                                    completionlistelementid="divAGCharge" ServicePath="~/WebService/FAPayToAutoComplete.asmx"
                                    ServiceMethod="GetFAChargeCode" MinimumPrefixLength="2" BehaviorID="divAGCharge"
                                    ContextKey="1473" usecontextkey="True" OnClientItemSelected="OnChargeCodeSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:autocompleteextender>
                            </td>
                            <td>
                                Agency Amount
                            </td>
                            <td>
                                <asp:TextBox ID="txtAGAmount" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Charge To Party
                            </td>
                            <td >
                                <asp:RadioButtonList ID="rdAGChargeToParty" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                Approval Attachment
                            </td>
                            <td>
                                <asp:FileUpload ID="FUAGApproveAttach" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Charge Amount
                            </td>
                            <td >
                                <asp:TextBox ID="txtAGChargeAmount" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Approving Authority
                            </td>
                            <td>
                                <asp:TextBox ID="txtApproveAuthor" runat="server" CssClass="SearchTextbox" placeholder="Search" TabIndex="18"></asp:TextBox>
                                
                                <div id="divwidthUser">
                                </div>
                                <cc1:AutoCompleteExtender ID="SalesRepExtender" runat="server" TargetControlID="txtApproveAuthor"
                                    CompletionListElementID="divwidthSalesRep" ServicePath="../WebService/UserAutoComplete.asmx"
                                    ServiceMethod="GetUserCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthUser"
                                    ContextKey="1088" UseContextKey="True" OnClientItemSelected="OnUserSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Service Rendered
                            </td>
                            <td>
                                <asp:DropDownList ID="ddServiceRendered" runat="server" DataSourceID="DataSourceService" 
                                    DataTextField="sName" DataValueField="lid" AppendDataBoundItems="true">
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Other Service" Value="-1"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                Service Remarks
                            </td>
                            <td>
                                <asp:TextBox ID="txtServiceRemarks" runat="server" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    </fieldset>
                    </asp:Panel>
                    <asp:Panel ID="pnlGrid" runat="server" Visible="true">
                        <div id="Div2" runat="server" style="overflow: auto;">   
                            <asp:GridView ID="gvPaymentDetail" runat="server" CssClass="table" AutoGenerateColumns="true"
                                AllowPaging="false">
                                <%--<Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>--%>
                            </asp:GridView>
                            </div>
                    </asp:Panel>
                </fieldset>
                <div id="divModel">
                    <asp:LinkButton ID="lnkModelPopup21" runat="server" />
                    <cc1:ModalPopupExtender ID="ModalPopupCheque" runat="server" TargetControlID="lnkModelPopup21"
                        PopupControlID="Panel21">
                    </cc1:ModalPopupExtender>
                    <asp:Panel ID="Panel21" Style="display: none" runat="server">
                        <fieldset>
                            <legend>
                                <asp:Label ID="lbl2" Text="Cheque Issued" runat="server"></asp:Label></legend>
                            <div class="header">
                                <div class="fleft">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </div>
                                <div class="fright">
                                    <asp:ImageButton ID="imgCancelPopup" ImageUrl="~/Images/delete.gif" runat="server"
                                        OnClick="btnCancelPopup_Click" ToolTip="Close" />
                                </div>
                            </div>
                            <div class="clear"></div>
                            <div id="Div1" runat="server" style="max-height: 550px; overflow: auto;">
                                <asp:GridView ID="gvChequeDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataSourceID="ChequeSqlDataSource" 
                                    AllowPaging="True" AllowSorting="True" PageSize="40" PagerSettings-Position="TopAndBottom" 
                                    DataKeyNames="ChequeIssueID" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex +1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Job No">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkChequeSelect" Text='<%#Bind("JobRefNo") %>' CommandArgument='<%#Bind("ChequeIssueId") %>' 
                                                    runat="server" OnClick="lnkChequeSelect"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="JobRefNo" HeaderText="Job No" />--%>
                                        <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                                        <asp:BoundField DataField="PayTo" HeaderText="PayTo" SortExpression="PayTo" />
                                        <asp:BoundField DataField="ChequeNo" HeaderText="Cheque No" SortExpression="ChequeNo" />
                                        <asp:BoundField DataField="ChequeDate" HeaderText="Cheque Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ChequeDate" />
                                        <asp:BoundField DataField="BankName" HeaderText="Bank Name" SortExpression="BankName" />
                                        <asp:BoundField DataField="ChequeIssuedBy" HeaderText="Issued By" SortExpression="IssuedBy" />
                                        <asp:BoundField DataField="ChequeIssueDate" HeaderText="Issue Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="IssueDate" />
                                    </Columns>
                                    <PagerTemplate>
                                        <asp:GridViewPager runat="server" />
                                    </PagerTemplate>
                                </asp:GridView>
                            </div>
                        </fieldset>
                        <asp:SqlDataSource ID="ChequeSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="AC_FAGetInstrumentHistory" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                <asp:ControlParameter ControlID="txtJobNumber" Name="JobRefNo" PropertyName="Text" ConvertEmptyStringToNull="false" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </asp:Panel>
                </div>
                <div>
                <div>
                    <asp:SqlDataSource ID="DataSourceExpense" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="AC_GetRequestTypeMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="DataSourcePaymentType" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="AC_GetPaymentTypeMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="DataSourceService" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="BS_GetQuotationCategory" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                </div>
            </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

