<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PendingQuotation.aspx.cs"
    Inherits="Quotation_PendingQuotation" Culture="en-GB" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" EnablePartialRendering="true" />
    <script type="text/javascript" src="../tinymce/jscripts/tiny_mce/tiny_mce.js"></script>
    <script type="text/javascript">
        tinyMCE.init({
            mode: "specific_textareas",
            editor_selector: "mceEditor",
            width: '100%',
            theme: "advanced",
            height: '300px',
            plugins: "safari,spellchecker,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,imagemanager,filemanager",
            theme_advanced_buttons1: "newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,styleselect,formatselect,fontselect,fontsizeselect",
            theme_advanced_buttons2: "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,cleanup,code,|,forecolor,backcolor,tablecontrols,print,ltr,rtl,|,fullscreen, absolute,spellchecker,abbr,del",
            theme_advanced_buttons3: "",
            theme_advanced_toolbar_location: "top",
            theme_advanced_toolbar_align: "left",
            theme_advanced_statusbar_location: "bottom",
            theme_advanced_resizing: false,
            template_external_list_url: "js/template_list.js",
            external_link_list_url: "js/link_list.js",
            external_image_list_url: "js/image_list.js",
            media_external_list_url: "js/media_list.js"
        });
    </script>
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
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingQuotation2" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upPendingQuotation2" runat="server" UpdateMode="Conditional" RenderMode="Inline">
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
                <fieldset id="fsEnquiryInfo" runat="server">
                    <legend>Enquiry Info</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <col width="25%" />
                        <col width="85%" />
                        <tr>
                            <td>Lead Ref No</td>
                            <td>
                                <asp:TextBox ID="txtLeadRefNo" runat="server" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Enquiry Ref No
                            </td>
                            <td>
                                <asp:TextBox ID="txtEnquiryRefNo" runat="server" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Services</td>
                            <td>
                                <asp:TextBox ID="txtServices" runat="server" Enabled="false" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Notes</td>
                            <td>
                                <asp:HiddenField ID="hdnEnquiryId" runat="server" Value="0" />
                                <asp:TextBox ID="txtEnquiryNotes" runat="server" Rows="5" Width="800px" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>Add Draft Quotation</legend>

                    <div id="dvBabajiQuote" runat="server">
                        <div id="dvDraftHeader" runat="server">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td style="width: 25%">Quotation Ref No</td>
                                    <td style="width: 75%">
                                        <asp:TextBox ID="lblQuoteRefNo" Enabled="false" Width="25%" runat="server" Style="background-color: rgba(46, 90, 95, 0.09);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Branch</td>
                                    <td>
                                        <asp:DropDownList ID="ddlBabajiBranch" runat="server" Width="26%" TabIndex="2">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Sales Person Name
                                    <asp:RequiredFieldValidator ID="rfvSalesPerson" runat="server" ControlToValidate="txtSalesPerson" Display="Dynamic" SetFocusOnError="true"
                                        ErrorMessage="Please Enter Sales Person Name." Text="*" ValidationGroup="vgDraftQuote" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSalesPerson" runat="server" CssClass="SearchTextbox" Width="38%" placeholder="Search" TabIndex="3"></asp:TextBox>
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
                                </tr>
                                <tr>
                                    <td>KAM</td>
                                    <td>
                                        <asp:TextBox ID="txtKAM" runat="server" CssClass="SearchTextbox" Width="38%" placeholder="Search" TabIndex="4"></asp:TextBox>
                                        <asp:HiddenField ID="hdnKAMId" runat="server" Value="0" />
                                        <div id="divwidthKAM">
                                        </div>
                                        <AjaxToolkit:AutoCompleteExtender ID="KAMExtender" runat="server" TargetControlID="txtKAM"
                                            CompletionListElementID="divwidthKAM" ServicePath="../WebService/UserAutoComplete.asmx"
                                            ServiceMethod="GetUserCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthKAM"
                                            ContextKey="7166" UseContextKey="True" OnClientItemSelected="OnKAMSelected"
                                            CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                            CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                        </AjaxToolkit:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Quotation Format</td>
                                    <td>
                                        <asp:TextBox ID="lblQuoteFormat" Text="Normal Quotation" Width="25%" Enabled="false" Style="background-color: rgba(46, 90, 95, 0.09);" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-right: none">Customer
                                        <asp:RequiredFieldValidator ID="rfvOrgName" runat="server" ControlToValidate="txtCustomer" Display="Dynamic" SetFocusOnError="true"
                                            ErrorMessage="Please Enter Customer." Text="*" ValidationGroup="vgDraftQuote" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddCustomer" Visible="false" runat="server" TabIndex="3" Width="25%" AutoPostBack="true" OnSelectedIndexChanged="ddCustomer_SelectedIndexChanged">
                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                        </asp:DropDownList>

                                        <asp:HiddenField ID="hdnCustId" runat="server" Value="0" />
                                        <asp:TextBox ID="txtCustomer" runat="server" Width="60%" MaxLength="250" Style="background-color: rgba(46, 90, 95, 0.09);" placeholder="Search Customer Name"></asp:TextBox>
                                        <div id="divwidthCust">
                                        </div>
                                        <AjaxToolkit:AutoCompleteExtender ID="CustomerExtender" runat="server" TargetControlID="txtCustomer"
                                            CompletionListElementID="divwidthCust" ServicePath="~/WebService/CustomerAutoComplete.asmx"
                                            ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCust"
                                            ContextKey="4317" UseContextKey="True" OnClientItemSelected="OnCustomerSelected"
                                            CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                            CompletionListHighlightedItemCssClass="AutoExtenderHighlight" FirstRowSelected="true">
                                        </AjaxToolkit:AutoCompleteExtender>
                                    </td>
                                </tr>
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
                                    <td>Generate Line Items For</td>
                                    <td>
                                        <asp:TextBox ID="lblQuoteGeneratedFor" Visible="false" runat="server" Enabled="false" Style="background-color: rgba(46, 90, 95, 0.09);" Width="84%"></asp:TextBox>
                                        <asp:DropDownList ID="ddlQuoteForDept" runat="server" Width="31%" DataSourceID="DataSourceQuoteCatg" DataTextField="sName" TabIndex="12"
                                            DataValueField="lid" AutoPostBack="true" OnSelectedIndexChanged="ddlQuoteForDept_OnSelectedIndexChanged">
                                        </asp:DropDownList>
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
                                <tr>
                                    <td>Include Transportation Charges? </td>
                                    <td>
                                        <asp:DropDownList ID="ddlIncludeTransportChg" runat="server" AutoPostBack="true" Width="70px" TabIndex="15"
                                            OnSelectedIndexChanged="ddlIncludeTransportChg_OnSelectedIndexChanged">
                                            <asp:ListItem Selected="True" Text="YES" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="NO" Value="2"></asp:ListItem>
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
                                                <asp:CheckBox ID="chkLumpSum" runat="server" ToolTip="Prepare Lump-Sum for quote." Text="PREPARE LUMP-SUM" AutoPostBack="true" OnCheckedChanged="chkLumpSum_CheckedChanged" />
                                                <span style="text-align: right"></span>
                                                <asp:Label ID="lblTotal2" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblMinTotal2" runat="server" Visible="false"></asp:Label>
                                            </div>
                                            <asp:UpdatePanel ID="upnlChargeGrid_Pending" runat="server">
                                                <ContentTemplate>
                                                    <div>
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
                                                                        <asp:CheckBox ID="chkItemForLumpSum" runat="server" AutoPostBack="true" OnCheckedChanged="chkItemForLumpSum_OnCheckedChanged" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Particulars" ItemStyle-Width="45%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblParticulars" runat="server" Text='<%#Eval("Particulars") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Charges Applicable" ItemStyle-Width="40%">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtChargesApp" runat="server" Width="200px"></asp:TextBox>
                                                                        <asp:RegularExpressionValidator ID="revChargesApp" runat="server" ControlToValidate="txtChargesApp" SetFocusOnError="true"
                                                                            Display="Dynamic" ValidationExpression="^[0-9]\d*(\.\d+)?$" ErrorMessage="Only numbers allowed!"></asp:RegularExpressionValidator>
                                                                        <asp:DropDownList ID="ddlRanges" runat="server" AppendDataBoundItems="true" Width="250px">
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="LumpSum Amount" ItemStyle-Width="40%">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtLumpsumAmount" Width="100px" runat="server"></asp:TextBox>
                                                                        <asp:RegularExpressionValidator ID="revLumpSum" runat="server" ControlToValidate="txtLumpsumAmount" SetFocusOnError="true"
                                                                            Display="Dynamic" ValidationExpression="^[0-9]\d*(\.\d+)?$" ErrorMessage="Only numbers allowed!"></asp:RegularExpressionValidator>
                                                                        <asp:DropDownList ID="ddlRanges_LumpSum" runat="server" AppendDataBoundItems="true" Width="250px">
                                                                        </asp:DropDownList>
                                                                        <%--  OnTextChanged="txtLumpsumAmount_OnTextChanged" AutoPostBack="true"--%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="CategoryId" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCategoryId" runat="server" Text='<%#Eval("CategoryId") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="lid" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLid" runat="server" Text='<%#Eval("lid") %>'></asp:Label>
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
                                OnPageIndexChanging="gvTransportChg_PageIndexChanging" PageSize="20" PagerSettings-Position="TopAndBottom"
                                DataKeyNames="lid" OnRowCommand="gvTransportChg_RowCommand" OnRowUpdating="gvTransportChg_RowUpdating"
                                OnRowDeleting="gvTransportChg_RowDeleting" OnRowEditing="gvTransportChg_RowEditing" Width="100%"
                                OnRowCancelingEdit="gvTransportChg_RowCancelingEdit" CellPadding="3" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                                <AlternatingRowStyle BackColor="White" ForeColor="#333333" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Particulars" ItemStyle-Width="60%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblParticulars" runat="server" Text='<%#Eval("Particulars") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtParticulars" runat="server" Text='<%#Eval("Particulars") %>' Width="90%" TabIndex="1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvParticulars" runat="server" ControlToValidate="txtParticulars" SetFocusOnError="true" Display="Dynamic"
                                                ErrorMessage="Please Enter Particulars." Text="*" ForeColor="Red" ValidationGroup="vgEditTransp"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtParticulars_Footer" runat="server" Text='<%#Eval("Particulars") %>' Width="90%" TabIndex="1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvaddpart" runat="server" ControlToValidate="txtParticulars_Footer" SetFocusOnError="true" Display="Dynamic"
                                                ErrorMessage="Please Enter Particulars." Text="*" ForeColor="Red" ValidationGroup="vgAddTransp"></asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                        <ItemStyle Width="60%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Charges Applicable" ItemStyle-Width="60%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChargesApplicable" runat="server" Text='<%#Eval("ChargesApplicable") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtChargesApplicable" runat="server" Text='<%#Eval("ChargesApplicable") %>' TextMode="MultiLine" Width="90%" TabIndex="1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvChargesApplicable" runat="server" ControlToValidate="txtChargesApplicable" SetFocusOnError="true" Display="Dynamic"
                                                ErrorMessage="Please Enter Charges Applicable." Text="*" ForeColor="Red" ValidationGroup="vgEditTransp"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtChargesApplicable_Footer" runat="server" Text='<%#Eval("ChargesApplicable") %>' Width="90%" TabIndex="1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvaddChargesApplicable" runat="server" ControlToValidate="txtChargesApplicable_Footer" SetFocusOnError="true" Display="Dynamic"
                                                ErrorMessage="Please Enter Charges Applicable." Text="*" ForeColor="Red" ValidationGroup="vgAddTransp"></asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                        <ItemStyle Width="60%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit/Delete" ItemStyle-Width="18%">
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
                                        <ItemStyle Width="18%" />
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
                    <div id="dvCustomerQuote" runat="server">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>Quotation Ref No</td>
                                <td>
                                    <asp:TextBox ID="lblQuoteRefNo2" runat="server" Enabled="false" Style="background-color: rgba(46, 90, 95, 0.09);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Branch</td>
                                <td>
                                    <asp:DropDownList ID="ddlBabajiBranch2" runat="server" Width="50%" TabIndex="2">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Quotation Format</td>
                                <td>
                                    <asp:TextBox ID="lblQuoteFormat2" Text="RFQ/Tender Quotation" Enabled="false" Style="background-color: rgba(46, 90, 95, 0.09);" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="border-right: none">Customer                           
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlTenderCustomer" runat="server" Visible="false" TabIndex="3" Width="25%" AutoPostBack="true" OnSelectedIndexChanged="ddlTenderCustomer_SelectedIndexChanged">
                                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    </asp:DropDownList>

                                    <asp:HiddenField ID="hdnTenderCustomer" runat="server" Value="0" />
                                    <asp:Label ID="txtTenderCustomer" runat="server" Width="25%" MaxLength="250" Style="background-color: rgba(46, 90, 95, 0.09);" placeholder="Search Customer Name."></asp:Label>

                                </td>
                            </tr>
                            <tr>
                                <td>Select division
                                    <asp:RequiredFieldValidator ID="rfvdiv2" runat="server" ControlToValidate="ddlDivision2" Display="Dynamic" SetFocusOnError="true"
                                        ErrorMessage="Please Select Division." Text="*" ValidationGroup="vgDraftQuote" ForeColor="Red" InitialValue="0"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlDivision2" runat="server" AppendDataBoundItems="true" DataSourceID="DataSourceService"
                                        DataTextField="sName" DataValueField="ServicesId">
                                        <asp:ListItem Selected="True" Value="0" Text="-Select-"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Notes</td>
                                <td>
                                    <asp:TextBox ID="txtNotes" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <fieldset>
                            <legend>Documents</legend>
                            <div>
                                <div>
                                    <asp:FileUpload ID="fuRFQDoc" runat="server" />

                                    <asp:Button ID="btnSaveDoc" runat="server" OnClick="btnSaveDoc_OnClick" CausesValidation="false" Text="Save Document" />
                                </div>
                                <asp:GridView ID="gvRFQDocuments" runat="server" CssClass="table" AutoGenerateColumns="false"
                                    PagerStyle-CssClass="pgr" DataKeyNames="lid" AllowPaging="True" AllowSorting="True" PageSize="20"
                                    PagerSettings-Position="TopAndBottom" OnRowCommand="gvRFQDocuments_RowCommand" Width="50%"
                                    DataSourceID="DataSourceRfqDocuments">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex +1%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Document">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownloadDoc" CommandName="download" ToolTip="Download document" runat="server"
                                                    Text='<%#Eval("DocumentName") %>' CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDelete" CommandName="deletedoc" ToolTip="Delete document" runat="server"
                                                    Text="Delete" CommandArgument='<%#Eval("lid") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div>
                                <asp:SqlDataSource ID="DataSourceRfqDocuments" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="BS_GetQuotationDocDetail" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="QuotationId" SessionField="QuotationId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </fieldset>
                    </div>

                    <div class="m clear">
                        <asp:Button ID="btnSave" Text="Save Quotation" OnClick="btnSave_Click" runat="server" ValidationGroup="vgDraftQuote" />
                        &nbsp;
                    <asp:Button ID="btnDraftQuote" Text="Draft Quote" OnClick="btnDraftQuote_Click" runat="server" ValidationGroup="vgDraftQuote" />
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



