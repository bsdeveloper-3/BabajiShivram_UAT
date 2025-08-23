<%@ Page Title="Quotation" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CustQuote.aspx.cs"
    Inherits="CRM_CustQuote" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" EnablePartialRendering="true" />
    <script type="text/javascript" src="../tinymce/jscripts/tiny_mce/tiny_mce.js"></script>
    <script type="text/javascript">
        function OnSalesPersonSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=hdnSalesPersonId.ClientID %>').value = results.Userid;
        }

        function OnKAMSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=hdnKAMId.ClientID %>').value = results.Userid;
        }
    </script>
    <style type="text/css">
        .modalBackground {
            background-color: black;
            filter: alpha(opacity=90);
            opacity: 0.5;
        }

        .gridview tr:nth-child(even) {
            background: #F7F6F3;
        }

        table th {
            border: 1px solid rgba(93, 123, 157, 0.33);
        }

        .checkbox input:checked + i, .toggle input:checked + i {
            border-color: rgba(0,0,0,8);
        }

        input#ctl00_ContentPlaceHolder1_chkLumpSum {
            width: 20px;
            height: 16px;
        }
    </style>
    <script type="text/javascript">
        function OnCustomerSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            //alert('Customer ' + results.ClientName + ' already exists..!! Please select the same from drop down.');
            $get('ctl00_ContentPlaceHolder1_hdnCustId').value = results.ClientId;
        }

        function OnCustomerSelected_Tender(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            alert('Customer ' + results.ClientName + ' already exists..!! Please select the same from drop down.');
            $get('ctl00_ContentPlaceHolder1_txtTenderCustomer').value = '';
        }
    </script>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upDraftQuotation" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upDraftQuotation" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center" id="dvErrorSection" runat="server">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="Valsummary" runat="server" ShowMessageBox="true" ShowSummary="false"
                    ValidationGroup="vgRequired" />
                <asp:ValidationSummary ID="valDraftQuote" runat="server" ShowMessageBox="true" ShowSummary="false"
                    ValidationGroup="vgDraftQuote" />
                <asp:ValidationSummary ID="vsLumpSumRange" runat="server" ShowMessageBox="true" ShowSummary="false"
                    ValidationGroup="vgLumpSumRange" />
                <asp:HiddenField ID="hdnHtmlContent" runat="server" />
                <asp:HiddenField ID="hdnEnquiryId" runat="server" Value="0" />
            </div>
            <div class="clear"></div>
            <fieldset>
                <div>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>Customer
                            </td>
                            <td>
                                <asp:HiddenField ID="hdnCustId" runat="server" Value="0" />
                                <asp:Label ID="lblCustomer" runat="server" Width="350px" MaxLength="250"></asp:Label>
                            </td>
                            <td>Select Branch
                                <asp:RequiredFieldValidator ID="rfvBranch" runat="server" ControlToValidate="ddlBabajiBranch" Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Please Select Branch." Text="*" ValidationGroup="vgDraftQuote" ForeColor="Red" InitialValue="0"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlBabajiBranch" runat="server" Width="260px" TabIndex="2">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Sales Person Name
                                    <asp:RequiredFieldValidator ID="rfvSalesPerson" runat="server" ControlToValidate="txtSalesPerson" Display="Dynamic" SetFocusOnError="true"
                                        ErrorMessage="Please Enter Sales Person Name." Text="*" ValidationGroup="vgDraftQuote" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSalesPerson" runat="server" CssClass="SearchTextbox" Width="235px" placeholder="Search Sales Person Name" TabIndex="3"></asp:TextBox>
                                <asp:HiddenField ID="hdnSalesPersonId" runat="server" Value="0" />
                                <div id="divwidthSalesRep">
                                </div>
                                <AjaxToolkit:AutoCompleteExtender ID="SalesRepExtender" runat="server" TargetControlID="txtSalesPerson"
                                    CompletionListElementID="divwidthSalesRep" ServicePath="../WebService/UserAutoComplete.asmx"
                                    ServiceMethod="GetUserCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthSalesRep"
                                    ContextKey="7164" UseContextKey="True" OnClientItemSelected="OnSalesPersonSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </AjaxToolkit:AutoCompleteExtender>
                            </td>
                            <td>KAM</td>
                            <td>
                                <asp:TextBox ID="txtKAM" runat="server" CssClass="SearchTextbox" Width="250px" placeholder="Search KAM" TabIndex="4"></asp:TextBox>
                                <asp:HiddenField ID="hdnKAMId" runat="server" Value="0" />
                                <div id="divwidthKAM">
                                </div>
                                <AjaxToolkit:AutoCompleteExtender ID="KAMExtender" runat="server" TargetControlID="txtKAM"
                                    CompletionListElementID="divwidthKAM" ServicePath="../WebService/UserAutoComplete.asmx"
                                    ServiceMethod="GetUserCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthKAM"
                                    ContextKey="7164" UseContextKey="True" OnClientItemSelected="OnKAMSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </AjaxToolkit:AutoCompleteExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>Is RFQ Quote?</td>
                            <td>
                                <asp:CheckBox ID="chkRFQQuote" runat="server" AutoPostBack="true" OnCheckedChanged="chkRFQQuote_CheckedChanged" />
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </div>
            </fieldset>
            <fieldset id="fsNormalQuote" runat="server">
                <legend>Add Draft Quotation</legend>
                <div id="dvDraftHeader" runat="server">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <colgroup>
                            <col width="30%" />
                            <col width="70%" />
                        </colgroup>
                        <tr>
                            <td style="border-right: none">Address Line 1
                                        <asp:RequiredFieldValidator ID="rfvorgadd" runat="server" ControlToValidate="txtAddressLine1" Display="Dynamic" SetFocusOnError="true"
                                            ErrorMessage="Please Enter Address." Text="*" ValidationGroup="vgDraftQuote" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddressLine1" runat="server" TabIndex="7" Width="84%" MaxLength="55"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="border-right: none">Address Line 2
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddressLine2" runat="server" TabIndex="8" Width="84%" MaxLength="55"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="border-right: none">Address Line 3
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddressLine3" runat="server" TabIndex="9" Width="84%" MaxLength="55"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="border-right: none">Kind Attention
                                  <asp:RequiredFieldValidator ID="rfvattn" runat="server" ControlToValidate="txtKindAttn" Display="Dynamic" SetFocusOnError="true"
                                      ErrorMessage="Please Enter Kind Attn." Text="*" ValidationGroup="vgDraftQuote" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPrefix" runat="server" Width="70px" TabIndex="10">
                                    <asp:ListItem Text="Mr." Selected="True" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Ms." Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Mrs." Value="3"></asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;
                                <asp:TextBox ID="txtKindAttn" runat="server" MaxLength="250" Width="21%" TabIndex="11"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="border-right: none">Subject 
                                   <asp:RequiredFieldValidator ID="rfvsubject" runat="server" ControlToValidate="txtSubject" Display="Dynamic" SetFocusOnError="true"
                                       ErrorMessage="Please Enter Subject." Text="*" ValidationGroup="vgDraftQuote" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSubject" runat="server" MaxLength="250" Width="84%" TabIndex="12"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="border-right: none">Payment Terms
                           
                            </td>
                            <td>
                                <asp:TextBox ID="txtTerms" runat="server" TabIndex="13" Width="84%"></asp:TextBox>
                            </td>
                        </tr>
                        <%-- <tr>
                                    <td>Select division
                                    <asp:RequiredFieldValidator ID="rfvdivision" runat="server" ControlToValidate="ddlDivision" Display="Dynamic" SetFocusOnError="true"
                                        ErrorMessage="Please Select Division." Text="*" ValidationGroup="vgDraftQuote" ForeColor="Red" InitialValue="0"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlDivision" runat="server" AppendDataBoundItems="true" DataSourceID="DataSourceService" Width="31%"
                                            DataTextField="sName" DataValueField="ServicesId" TabIndex="14">
                                            <asp:ListItem Selected="True" Value="0" Text="-Select-"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>--%>
                        <tr>
                            <td>Generate Line Items For
                                    <asp:RequiredFieldValidator ID="rfvLineItemsFor" runat="server" ControlToValidate="ddlBabajiBranch" Display="Dynamic" SetFocusOnError="true"
                                        ErrorMessage="Please Select Line Items For." Text="*" ValidationGroup="vgDraftQuote" ForeColor="Red" InitialValue="0"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlQuoteForDept" runat="server" Width="31%" DataSourceID="DataSourceQuoteCatg" DataTextField="sName" TabIndex="15"
                                    DataValueField="lid" AutoPostBack="true" OnSelectedIndexChanged="ddlQuoteForDept_OnSelectedIndexChanged" AppendDataBoundItems="true">
                                    <asp:ListItem Selected="True" Value="0" Text="-Select"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <%-- <tr>
                                <td>Select Mode</td>
                                <td>
                                    <asp:CheckBoxList ID="cblModes" runat="server" RepeatDirection="Horizontal" AppendDataBoundItems="true" TabIndex="16" Width="31%"
                                        DataSourceID="DataSourceModes" DataTextField="sName" DataValueField="lid">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>--%>
                        <tr>
                            <td>Select Terms & Condition (Added in quotation)</td>
                            <td>
                                <asp:DropDownList ID="ddlTermCondition" runat="server" Width="31%" AppendDataBoundItems="true" DataSourceID="DataSourceTermCondition"
                                    DataTextField="sName" DataValueField="lid" TabIndex="17">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <%-- <tr>
                                <td>Include Transportation Charges? </td>
                                <td>
                                    <asp:DropDownList ID="ddlIncludeTransportChg" runat="server" AutoPostBack="true" Width="70px" TabIndex="18"
                                        OnSelectedIndexChanged="ddlIncludeTransportChg_OnSelectedIndexChanged">
                                        <asp:ListItem Selected="True" Text="YES" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="NO" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="border-right: none">Include Texture?</td>
                                <td>
                                    <asp:DropDownList ID="ddlIncludeDesc" runat="server" Width="70px" TabIndex="19" AutoPostBack="true" OnSelectedIndexChanged="ddlIncludeDesc_OnSelectedIndexChanged">
                                        <asp:ListItem Text="-Select-" Selected="True" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="NO" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="YES" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>--%>
                    </table>
                    <asp:SqlDataSource ID="DataSourceQuoteCatg" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="BS_GetQuotationCategory" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="DataSourceModes" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="BS_GetQuoteMode" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="DataSourceService" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="BS_GetServicesMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                </div>
                <fieldset>
                    <legend>Charges Applicable</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td style="width: 25%">Copy Charges From An Existing Quotation ?
                            </td>
                            <td style="width: 75%">
                                <asp:DropDownList ID="ddlCopyQuote" Width="70px" runat="server" AutoPostBack="true" TabIndex="20" OnSelectedIndexChanged="ddlCopyQuote_SelectedIndexChanged">
                                    <asp:ListItem Selected="True" Text="NO" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="YES" Value="1"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="trCopyQuoteLists" runat="server">
                            <td>Select Quotation
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlExistQuotes" runat="server" TabIndex="21" AutoPostBack="true" OnSelectedIndexChanged="ddlExistQuotes_SelectedIndexChanged"
                                    DataSourceID="DataSourceExistQuotes" DataTextField="CustWsQuoteNo" DataValueField="lid" AppendDataBoundItems="true" Width="60%">
                                    <asp:ListItem Selected="True" Value="0" Text="-Select-"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="border: none">
                            <td style="width: 43%; background-color: #5D7B9D" colspan="2">
                                <div style="overflow: auto; height: 250px">
                                    <asp:CheckBoxList ID="chkParticulars" runat="server" CellPadding="4" CellSpacing="2" AutoPostBack="true" OnSelectedIndexChanged="chkParticulars_SelectedChanged"
                                        RepeatLayout="Table" CausesValidation="True" RepeatColumns="2" BackColor="ScrollBar" CssClass="gridview">
                                    </asp:CheckBoxList>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="2" style="width: 55%">
                                <div>
                                    <div style="padding: 5px; font-weight: 600">
                                        <asp:CheckBox ID="chkLumpSum" runat="server" ToolTip="Prepare Lump-Sum for quote." TabIndex="22" Text="PREPARE LUMP-SUM" AutoPostBack="true" OnCheckedChanged="chkLumpSum_CheckedChanged" />
                                        <span style="text-align: right"></span>
                                    </div>
                                    <asp:UpdatePanel ID="upnlChargeGrid" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="gvGenerateCharge" runat="server" ShowFooter="True" AllowSorting="True" Style="border-collapse: initial; border: 1px solid #5D7B9D"
                                                AutoGenerateColumns="False" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None" OnRowCommand="gvGenerateCharge_RowDataCommand">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText=" " ItemStyle-Width="8%">
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnUp" CommandName="Up" ToolTip="UP" Text="&uArr;" ForeColor="White" Height="20px" Font-Bold="true" BackColor="#E07200" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                            <asp:Button ID="btnDown" CommandName="Down" ToolTip="Down" Text="&dArr;" ForeColor="White" Height="20px" Font-Bold="true" BackColor="#E07200" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="RowNumber" HeaderText="Sl" ItemStyle-Width="2%" />
                                                    <asp:TemplateField HeaderText="ChargeId" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblChargeId" runat="server" Text='<%#Eval("ChargeId") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" ItemStyle-Width="1%">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkItemForLumpSum" runat="server" AutoPostBack="true" OnCheckedChanged="chkItemForLumpSum_OnCheckedChanged" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Particulars" ItemStyle-Width="50%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblParticulars" runat="server" Text='<%#Eval("Particulars") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Charges Applicable" ItemStyle-Width="42%">
                                                        <ItemTemplate>
                                                            &nbsp;
                                                                <asp:TextBox ID="txtChargesApp" runat="server" Width="100px" TabIndex="1"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="revChargesApp" runat="server" ControlToValidate="txtChargesApp" SetFocusOnError="true"
                                                                Display="Dynamic" ValidationExpression="^[0-9]\d*(\.\d+)?$" ErrorMessage="Only numbers allowed!"></asp:RegularExpressionValidator>
                                                            <asp:DropDownList ID="ddlRanges" runat="server" AppendDataBoundItems="true" Width="250px" TabIndex="2">
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="LumpSum Amount" ItemStyle-Width="42%" Visible="false">
                                                        <ItemTemplate>
                                                            &nbsp;
                                                                <asp:TextBox ID="txtLumpsumAmount" runat="server" TabIndex="3"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="revLumpSum" runat="server" ControlToValidate="txtLumpsumAmount" SetFocusOnError="true"
                                                                Display="Dynamic" ValidationExpression="^[0-9]\d*(\.\d+)?$" ErrorMessage="Only numbers allowed!"></asp:RegularExpressionValidator>
                                                            <asp:DropDownList ID="ddlRanges_LumpSum" runat="server" AppendDataBoundItems="true" TabIndex="4" Width="250px">
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="CategoryId" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCategoryId" runat="server" Text='<%#Eval("CategoryId") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Delete Row">
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnDeleteCharge" runat="server" OnClick="btnDeleteCharge_OnClick" CausesValidation="false" Text="Delete" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EditRowStyle BackColor="#999999" />
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div>
                        <asp:SqlDataSource ID="DataSourceExistQuotes" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="BS_GetExistingQuotations" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="DataSourceTermCondition" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="BS_GetTermConditionMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                        <%--  <asp:SqlDataSource ID="DataSourceLeadInfo" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="CRM_GetLeads_Quotation" SelectCommandType="StoredProcedure"></asp:SqlDataSource>--%>
                        <asp:SqlDataSource ID="DataSourceLeadInfo" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="CRM_GetLeadByFinYear" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="DataSourceEnquiry" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="CRM_GetOtherEnquiryByCustId" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hdnCustId" Name="CustomerId" PropertyName="Value" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </fieldset>
                <fieldset id="trTransportCharges" runat="server">
                    <legend>Transportation Charges</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>
                                <asp:GridView ID="gvTransportationCharges" runat="server" ShowFooter="True" AutoGenerateColumns="False" Width="99%" TabIndex="21"
                                    OnRowCreated="gvTransportationCharges_RowCreated" CellPadding="4" ForeColor="#333333" GridLines="None" Style="border-collapse: initial; border: 1px solid #5D7B9D">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="RowNumber" HeaderText="Sr.No." ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Particulars" ItemStyle-Width="45%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtParticulars" runat="server" TextMode="MultiLine" Rows="2" Width="95%"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Charges Applicable" ItemStyle-Width="45%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtChargesApp" runat="server" TextMode="MultiLine" Rows="2" Width="95%"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDeleteRow" runat="server" CausesValidation="false" OnClick="lnkDeleteRow_Click" Text="Delete" Font-Underline="true"></asp:LinkButton>
                                            </ItemTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Button ID="btnAddTransportCharges" CausesValidation="false" runat="server" Text="+" OnClick="btnAddTransportCharges_Click" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EditRowStyle BackColor="#999999" />
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <div class="m clear">
                    <asp:Button ID="btnSave" Text="Save Quotation" OnClick="btnSave_Click" runat="server" ValidationGroup="vgDraftQuote" />
                    &nbsp;
                    <asp:Button ID="btnDraftQuote" Text="Draft Quote" OnClick="btnDraftQuote_Click" runat="server" ValidationGroup="vgDraftQuote" />
                </div>

            </fieldset>
            <fieldset id="fsRfqQuote" runat="server">
                <legend>Quote</legend>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <colgroup>
                        <col width="30%" />
                        <col width="70%" />
                    </colgroup>
                    <tr>
                        <td>Notes <span style="color: red">*</span>
                            <asp:RequiredFieldValidator ID="rfvNotes" runat="server" ControlToValidate="txtNotes" Display="Dynamic" SetFocusOnError="true"
                                ErrorMessage="Please Enter Notes." Text="*" ValidationGroup="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" Rows="3" Width="85%" TabIndex="7"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Add documents (if any)</td>
                        <td colspan="3">
                            <div id="dvUploadNewFile2" runat="server" style="max-height: 200px; overflow: auto;">
                                <asp:FileUpload ID="fuDocument2" runat="server" ViewStateMode="Enabled" TabIndex="8" />
                                <asp:Button ID="btnSaveDocument2" Text="Add" runat="server" OnClick="btnSaveDocument2_Click" />
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
                        </td>
                    </tr>
                </table>
            </fieldset>

            <!--Document for Doc Upload-->
            <div id="divApproval">
                <AjaxToolkit:ModalPopupExtender ID="mpeApproval" runat="server" CacheDynamicResults="false"
                    PopupControlID="pnlApprovalPopup" TargetControlID="LinkButton1" BackgroundCssClass="modalBackground" DropShadow="true">
                </AjaxToolkit:ModalPopupExtender>
                <asp:Panel ID="pnlApprovalPopup" runat="server" CssClass="ModalPopupPanel" Width="400px" Style="border-radius: 10px">
                    <div class="header">
                        <div class="fleft">
                            <img alt="" width="20" height="18" src="../Images/Error.png" />
                            &nbsp;&nbsp;
                            CONFIRMATION MESSAGE
                        </div>
                        <div class="fright">
                            <asp:ImageButton ID="imgbtnApproval" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgbtnApproval_Click"
                                ToolTip="Close" />
                        </div>
                    </div>
                    <div class="m">
                    </div>
                    <!-- Lists Of All Documents -->
                    <div id="Div3" runat="server" style="max-height: 200px; overflow: auto; padding: 5px">
                        <asp:HiddenField ID="HiddenField1" runat="server" />
                        <asp:HiddenField ID="HiddenField2" runat="server" />
                        <div>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white" style="background-color: beige; border-radius: 5px; border-collapse: initial">
                                <tr style="text-align: center; font-weight: 600; background-color: navajowhite">
                                    <td colspan="2">Charges in some line items are below minimum range..!!
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">Yes -  to send the draft quotation to concerned authority for approval
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">No - to modify the quotation
                                    </td>
                                </tr>

                            </table>
                            <div style="text-align: center; padding-top: 10px">
                                <asp:Button ID="btnYes" runat="server" OnClick="btnYes_OnClick" CausesValidation="false" Text="YES" Width="40px" />
                                &nbsp;&nbsp;
                                 <asp:Button ID="btnNo" runat="server" OnClick="btnNo_OnClick" CausesValidation="false" Text="NO" Width="40px" />
                            </div>
                        </div>
                    </div>

                    <div class="m clear">
                    </div>
                </asp:Panel>
            </div>
            <div>
                <asp:HiddenField ID="LinkButton1" runat="server"></asp:HiddenField>
            </div>
            <!--Document for Doc Upload - END -->

            <asp:SqlDataSource ID="DataSourceOtherEnquiryRFQQuote" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="CRM_GetOtherEnquiryByCustId" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hdnCustId" Name="CustomerId" PropertyName="Value" />
                </SelectParameters>
            </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
