<%@ Page Title="Edit Quote" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EditQuote.aspx.cs"
    Inherits="CRM_EditQuote" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" EnablePartialRendering="true" />
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

        .modalPopup {
            border-radius: 5px;
            background-color: #FFFFFF;
            border-width: 1px;
            border-style: solid;
            border-color: black;
            padding-top: 5px;
            padding-left: 3px;
            width: 300px;
            height: 140px;
        }

        .tooltip {
            position: relative;
            display: inline-block;
            border-bottom: 1px dotted black;
        }

            .tooltip .tooltiptext {
                visibility: hidden;
                width: 120px;
                background-color: #555;
                color: #fff;
                text-align: center;
                border-radius: 6px;
                padding: 5px 0;
                position: absolute;
                z-index: 1;
                bottom: 125%;
                left: 50%;
                margin-left: -60px;
                opacity: 0;
                transition: opacity 1s;
            }

                .tooltip .tooltiptext::after {
                    content: "";
                    position: absolute;
                    top: 100%;
                    left: 50%;
                    margin-left: -5px;
                    border-width: 5px;
                    border-style: solid;
                    border-color: #555 transparent transparent transparent;
                }

            .tooltip:hover .tooltiptext {
                visibility: visible;
                opacity: 1;
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
            alert('Customer ' + results.ClientName + ' already exists..!! Please select the same from drop down.');
            $get('ctl00_ContentPlaceHolder1_txtCustomer').value = '';
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
                <asp:ValidationSummary ID="vsAddTransp" runat="server" ShowMessageBox="true" ShowSummary="false"
                    ValidationGroup="vgAddTransp" />
                <asp:ValidationSummary ID="vsEditTransp" runat="server" ShowMessageBox="true" ShowSummary="false"
                    ValidationGroup="vgEditTransp" />
            </div>
            <div class="clear"></div>
            <div id="dvDraftSection" runat="server">
                <fieldset>
                    <div>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>Quote Ref No</td>
                                <td>
                                    <asp:Label ID="lblQuoteRefNo" runat="server"></asp:Label>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
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
                        </table>
                    </div>
                </fieldset>

                <fieldset>
                    <legend>Add Draft Quotation</legend>

                    <div id="dvBabajiQuote" runat="server">
                        <div id="dvDraftHeader" runat="server">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td style="border-right: none">Address Line 1
                                        <asp:RequiredFieldValidator ID="rfvorgadd" runat="server" ControlToValidate="txtAddressLine1" Display="Dynamic" SetFocusOnError="true"
                                            ErrorMessage="Please Enter Address." Text="*" ValidationGroup="vgDraftQuote" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAddressLine1" runat="server" TabIndex="2" Width="84%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-right: none">Address Line 2
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAddressLine2" runat="server" TabIndex="3" Width="84%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-right: none">Address Line 3
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAddressLine3" runat="server" TabIndex="3" Width="84%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-right: none">Kind Attention
                                          <asp:RequiredFieldValidator ID="rfvattn" runat="server" ControlToValidate="txtKindAttn" Display="Dynamic" SetFocusOnError="true"
                                              ErrorMessage="Please Enter Kind Attn." Text="*" ValidationGroup="vgDraftQuote" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>

                                        <asp:TextBox ID="txtKindAttn" runat="server" MaxLength="250" Width="25%" TabIndex="4"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-right: none">Subject 
                                       <asp:RequiredFieldValidator ID="rfvsubject" runat="server" ControlToValidate="txtSubject" Display="Dynamic" SetFocusOnError="true"
                                           ErrorMessage="Please Enter Subject." Text="*" ValidationGroup="vgDraftQuote" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSubject" runat="server" MaxLength="250" Width="84%" TabIndex="5"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-right: none">Payment Terms
                               
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTerms" runat="server" TabIndex="6" Width="84%"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-right: none">Include Texture?</td>
                                    <td>
                                        <asp:DropDownList ID="ddlIncludeDesc" runat="server" Width="10%" TabIndex="7" AutoPostBack="true" OnSelectedIndexChanged="ddlIncludeDesc_OnSelectedIndexChanged">
                                            <asp:ListItem Text="-Select-" Selected="True" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="NO" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="YES" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="trTexture" runat="server">
                                    <td colspan="2">
                                        <asp:TextBox ID="txtHTMLContent" runat="server" TextMode="MultiLine" CssClass="mceEditor" TabIndex="8" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Select division
                                        <asp:RequiredFieldValidator ID="rfvdivision" runat="server" ControlToValidate="ddlDivision" Display="Dynamic" SetFocusOnError="true"
                                            ErrorMessage="Please Select Division." Text="*" ValidationGroup="vgDraftQuote" ForeColor="Red" InitialValue="0"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlDivision" runat="server" AppendDataBoundItems="true" DataSourceID="DataSourceService"
                                            DataTextField="sName" DataValueField="ServicesId" Width="25%">
                                            <asp:ListItem Selected="True" Value="0" Text="-Select-"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Quote Generated For</td>
                                    <td>
                                        <asp:TextBox ID="lblQuoteGeneratedFor" runat="server" Enabled="false" Style="background-color: rgba(46, 90, 95, 0.09);" Width="84%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Select Mode</td>
                                    <td>
                                        <asp:CheckBoxList ID="cblModes" runat="server" RepeatDirection="Horizontal" AppendDataBoundItems="true" TabIndex="9"
                                            DataSourceID="DataSourceModes" DataTextField="sName" DataValueField="lid" Width="25%">
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Select Terms & Condition (Added in quotation)</td>
                                    <td>
                                        <asp:DropDownList ID="ddlTermCondition" runat="server" Width="25%" AppendDataBoundItems="true" DataSourceID="DataSourceTermCondition"
                                            DataTextField="sName" DataValueField="lid" TabIndex="10">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <asp:SqlDataSource ID="DataSourceQuoteCatg" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="BS_GetQuotationCategory" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                            <asp:SqlDataSource ID="DataSourceModes" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="BS_GetQuoteMode" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                            <asp:SqlDataSource ID="DataSourceTermCondition" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="BS_GetTermConditionMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                            <asp:SqlDataSource ID="DataSourceService" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="BS_GetServicesMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                        </div>
                        <fieldset>
                            <legend>Charges Applicable</legend>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td colspan="3" style="width: 55%">
                                        <div>
                                            <div style="padding: 5px; font-weight: 600">
                                                <asp:CheckBox ID="chkLumpSum" runat="server" Enabled="false" Visible="false" />
                                                <span style="text-align: right"></span>
                                                <asp:Label ID="lblTotal2" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblMinTotal2" runat="server" Visible="false"></asp:Label>
                                            </div>
                                            <asp:UpdatePanel ID="upnlChargeGrid" runat="server">
                                                <ContentTemplate>
                                                    <div>
                                                        <asp:GridView ID="gvGenerateCharge" runat="server" ShowFooter="True" AllowSorting="True" Style="border-collapse: initial; border: 1px solid #5D7B9D"
                                                            AutoGenerateColumns="False" Width="100%" CellPadding="4" OnRowDataBound="gvGenerateCharge_RowDataBound" ForeColor="#333333" GridLines="None" DataKeyNames="lid"
                                                            OnRowCommand="gvGenerateCharge_RowDataCommand">
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Sl">
                                                                    <ItemTemplate>
                                                                        <%#Container.DataItemIndex +1%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="ChargeId" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblChargeId" runat="server" Text='<%#Eval("ChargeId") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="" ItemStyle-Width="1%">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkItemForLumpSum" runat="server" AutoPostBack="true" OnCheckedChanged="chkItemForLumpSum_OnCheckedChanged" Enabled="false" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Particulars" ItemStyle-Width="50%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblParticulars" runat="server" Text='<%#Eval("Particulars") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Charges Applicable" ItemStyle-Width="50%">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtChargesApp" runat="server" Width="200px" Text='<%#Eval("ChargesAmt") %>'></asp:TextBox>
                                                                        <%--   <asp:RegularExpressionValidator ID="revChargesApp" runat="server" ControlToValidate="txtChargesApp" SetFocusOnError="true"
                                                                            Display="Dynamic" ValidationExpression="^[0-9]\d*(\.\d+)?$" ErrorMessage="Only numbers allowed!"></asp:RegularExpressionValidator>--%>
                                                                        <asp:DropDownList ID="ddlRanges" runat="server" AppendDataBoundItems="true" Width="250px">
                                                                            <asp:ListItem Selected="True" Value="0" Text="-Select-"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="LumpSum Amount" ItemStyle-Width="50%">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtLumpsumAmount" Width="200px" runat="server" Text='<%#Eval("LumpSumAmt") %>'></asp:TextBox>
                                                                        <%--   <asp:RegularExpressionValidator ID="revLumpSum" runat="server" ControlToValidate="txtLumpsumAmount" SetFocusOnError="true"
                                                                            Display="Dynamic" ValidationExpression="^[0-9]\d*(\.\d+)?$" ErrorMessage="Only numbers allowed!"></asp:RegularExpressionValidator>--%>
                                                                        <asp:DropDownList ID="ddlRanges_LumpSum" runat="server" AppendDataBoundItems="true" Width="250px">
                                                                            <asp:ListItem Selected="True" Value="0" Text="-Select-"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <%--  OnTextChanged="txtLumpsumAmount_OnTextChanged" AutoPostBack="true"--%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="CategoryId" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCategoryId" runat="server" Text='<%#Eval("CategoryId") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Delete Row">
                                                                    <ItemTemplate>
                                                                        <asp:Button ID="btnDeleteCharge" runat="server" CommandArgument='<%#Eval("lid") %>' OnClick="btnDeleteCharge_OnClick" CausesValidation="false" Text="Delete" />
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
                                                        <div id="dvDisplayNote" runat="server" style="padding-top: 5px; color: red">
                                                            <b>Note*: <u>Line items marked with red color are charges which lie below minimum range..!!</u></b>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div>
                                        </div>
                                    </td>
                                </tr>

                            </table>
                        </fieldset>
                        <fieldset id="fsTransportCharges" runat="server">
                            <legend>Transportation Charges
                            </legend>
                            <asp:GridView ID="gvTransportChg" runat="server" ShowFooter="True" PagerStyle-CssClass="pgr" CssClass="gridview"
                                AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True" Style="white-space: normal; border-collapse: initial; border: 1px solid #5D7B9D"
                                OnPageIndexChanging="gvTransportChg_PageIndexChanging" PageSize="20" PagerSettings-Position="TopAndBottom" OnRowDataBound="gvTransportChg_OnDataBound"
                                DataKeyNames="lid" OnRowCommand="gvTransportChg_RowCommand" OnRowUpdating="gvTransportChg_RowUpdating"
                                OnRowDeleting="gvTransportChg_RowDeleting" OnRowEditing="gvTransportChg_RowEditing" Width="100%"
                                OnRowCancelingEdit="gvTransportChg_RowCancelingEdit" ShowHeader="true" CellPadding="3" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                                <AlternatingRowStyle BackColor="White" ForeColor="#333333" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl" ItemStyle-Width="3%" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle Width="3%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Particulars" ItemStyle-Width="60%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblParticulars" runat="server" Text='<%#Eval("Particulars") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtParticulars" runat="server" Text='<%#Eval("Particulars") %>' Width="90%" TabIndex="1" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvParticulars" runat="server" ControlToValidate="txtParticulars" SetFocusOnError="true" Display="Dynamic"
                                                ErrorMessage="Please Enter Particulars." Text="*" ForeColor="Red" ValidationGroup="vgEditTransp"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtParticulars_Footer" runat="server" Text='<%#Eval("Particulars") %>' Width="90%" TextMode="MultiLine" Rows="3" TabIndex="1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvaddpart" runat="server" ControlToValidate="txtParticulars_Footer" SetFocusOnError="true" Display="Dynamic"
                                                ErrorMessage="Please Enter Particulars." Text="*" ForeColor="Red" ValidationGroup="vgAddTransp"></asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                        <ItemStyle Width="60%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Charges Applicable" ItemStyle-Width="50%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChargesApplicable" runat="server" Text='<%#Eval("ChargesApplicable") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtChargesApplicable" runat="server" Text='<%#Eval("ChargesApplicable") %>' Width="90%" TextMode="MultiLine" Rows="3" TabIndex="1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvChargesApplicable" runat="server" ControlToValidate="txtChargesApplicable" SetFocusOnError="true" Display="Dynamic"
                                                ErrorMessage="Please Enter Charges Applicable." Text="*" ForeColor="Red" ValidationGroup="vgEditTransp"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtChargesApplicable_Footer" runat="server" Text='<%#Eval("ChargesApplicable") %>' Width="90%" TextMode="MultiLine" Rows="3" TabIndex="1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvaddChargesApplicable" runat="server" ControlToValidate="txtChargesApplicable_Footer" SetFocusOnError="true" Display="Dynamic"
                                                ErrorMessage="Please Enter Charges Applicable." Text="*" ForeColor="Red" ValidationGroup="vgAddTransp"></asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                        <ItemStyle Width="50%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit/Delete" ItemStyle-Width="35%">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server" Text="Edit" Font-Underline="true"></asp:LinkButton>
                                            &nbsp;<asp:LinkButton ID="lnkDelete" CommandName="Delete" ToolTip="Delete" Width="22" OnClientClick="return confirm('Sure to delete?');" runat="server" Text="Delete" Font-Underline="true"></asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" Width="39" runat="server" Text="Update" ValidationGroup="vgEditTransp" Font-Underline="true" TabIndex="2"></asp:LinkButton>
                                            &nbsp;<asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" Width="22" runat="server" Text="Cancel" Font-Underline="true" TabIndex="3"></asp:LinkButton>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:LinkButton ID="lnkAdd" CommandName="Insert" ToolTip="Add" Width="22" runat="server" Text="Add" ValidationGroup="vgAddTransp" Font-Underline="true" TabIndex="2"></asp:LinkButton>
                                        </FooterTemplate>
                                        <ItemStyle Width="35%" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Position="TopAndBottom" />
                                <PagerStyle CssClass="pgr" />
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
                        </fieldset>

                    </div>

                    <div class="m clear">
                        <asp:Button ID="btnSave" Text="Save Quotation" OnClick="btnSave_Click" runat="server" ValidationGroup="vgDraftQuote" />
                    </div>

                </fieldset>
            </div>


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
                <asp:LinkButton ID="LinkButton1" runat="server" Text=""></asp:LinkButton>
            </div>
            <!--Document for Doc Upload - END -->
        </ContentTemplate>

    </asp:UpdatePanel>

</asp:Content>

