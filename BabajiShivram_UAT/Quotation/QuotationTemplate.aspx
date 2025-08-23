<%@ Page Title="Quotation Template" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="QuotationTemplate.aspx.cs" Inherits="Quotation_QuotationTemplate" %>

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

    <style type="text/css">
        td.NestedPadding {
            padding-left: 1px;
        }

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
                <asp:HiddenField ID="hdnHtmlContent" runat="server" />
            </div>
            <div class="clear"></div>
            <div id="dvDraftSection" runat="server">
                <%--<fieldset>
                    <div>
                      <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td style="width: 25%">Choose Quotation Format
                                </td>
                                <td style="width: 75%">
                                    <asp:RadioButtonList ID="rblCustBabajiQuote" runat="server" RepeatDirection="Horizontal" Width="50%" ValidationGroup="vgCustBabajiQuote"
                                        AutoPostBack="true" OnSelectedIndexChanged="rblCustBabajiQuote_SelectedIndexChanged">
                                        <asp:ListItem Selected="True" Text="Normal Quotation" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="RFQ/Tender" Value="2"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td>Select Branch</td>
                                <td>
                                    <asp:DropDownList ID="ddlBabajiBranch" runat="server" Width="50%" TabIndex="2">
                                    </asp:DropDownList>
                                </td>
                            </tr>

                        </table>
                    </div>
                </fieldset>--%>
                <fieldset>
                    <legend>Add Draft Quotation</legend>

                    <div id="dvBabajiQuote" runat="server">
                        <div id="dvDraftHeader" runat="server">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <%--      <tr>
                                    <td style="border-right: none">Customer
                                <asp:RequiredFieldValidator ID="rfvOrgName" runat="server" ControlToValidate="txtCustomer" Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Please Enter Customer." Text="*" ValidationGroup="vgDraftQuote" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>&nbsp; Existing &nbsp;
                                    <asp:DropDownList ID="ddCustomer" runat="server" TabIndex="3" Width="25%" AutoPostBack="true" OnSelectedIndexChanged="ddCustomer_SelectedIndexChanged">
                                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                        &nbsp; OR &nbsp; (New) &nbsp;
                                    <asp:HiddenField ID="hdnCustId" runat="server" Value="0" />
                                        <asp:TextBox ID="txtCustomer" runat="server" Width="45%" MaxLength="250" TabIndex="4" placeholder="Enter Customer Name"></asp:TextBox>
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
                                        <asp:TextBox ID="txtAddressLine1" runat="server" TabIndex="5" Width="84%" MaxLength="55"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-right: none">Address Line 2
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAddressLine2" runat="server" TabIndex="6" Width="84%" MaxLength="55"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-right: none">Address Line 3
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAddressLine3" runat="server" TabIndex="6" Width="84%" MaxLength="55"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-right: none">Kind Attention
                                  <asp:RequiredFieldValidator ID="rfvattn" runat="server" ControlToValidate="txtKindAttn" Display="Dynamic" SetFocusOnError="true"
                                      ErrorMessage="Please Enter Kind Attn." Text="*" ValidationGroup="vgDraftQuote" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlPrefix" runat="server" Width="70px" TabIndex="7">
                                            <asp:ListItem Text="Mr." Selected="True" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Ms." Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Mrs." Value="3"></asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp;
                                <asp:TextBox ID="txtKindAttn" runat="server" MaxLength="250" Width="21%" TabIndex="8"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-right: none">Subject 
                                   <asp:RequiredFieldValidator ID="rfvsubject" runat="server" ControlToValidate="txtSubject" Display="Dynamic" SetFocusOnError="true"
                                       ErrorMessage="Please Enter Subject." Text="*" ValidationGroup="vgDraftQuote" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSubject" runat="server" MaxLength="250" Width="84%" TabIndex="9"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-right: none">Payment Terms
                           
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTerms" runat="server" TabIndex="10" Width="84%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Select division
                                    <asp:RequiredFieldValidator ID="rfvdivision" runat="server" ControlToValidate="ddlDivision" Display="Dynamic" SetFocusOnError="true"
                                        ErrorMessage="Please Select Division." Text="*" ValidationGroup="vgDraftQuote" ForeColor="Red" InitialValue="0"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlDivision" runat="server" AppendDataBoundItems="true" DataSourceID="DataSourceService" Width="31%"
                                            DataTextField="sName" DataValueField="ServicesId" TabIndex="11">
                                            <asp:ListItem Selected="True" Value="0" Text="-Select-"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td>Enter Template For
                                        <asp:RequiredFieldValidator ID="rfvtemplatefor" runat="server" ControlToValidate="txtTemplateFor" Display="Dynamic" SetFocusOnError="true"
                                        ErrorMessage="Please Enter Template For." Text="*" ValidationGroup="vgDraftQuote" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTemplateFor" runat="server" MaxLength="200" Width="70%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Generate Line Items For</td>
                                    <td>
                                        <asp:DropDownList ID="ddlQuoteForDept" runat="server" Width="31%" DataSourceID="DataSourceQuoteCatg" DataTextField="sName" TabIndex="12"
                                            DataValueField="lid" AutoPostBack="true" OnSelectedIndexChanged="ddlQuoteForDept_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <%--     <tr>
                                    <td>Select Mode</td>
                                    <td>
                                        <asp:CheckBoxList ID="cblModes" runat="server" RepeatDirection="Horizontal" AppendDataBoundItems="true" TabIndex="13" Width="31%"
                                            DataSourceID="DataSourceModes" DataTextField="sName" DataValueField="lid">
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Select Terms & Condition (Added in quotation)</td>
                                    <td>
                                        <asp:DropDownList ID="ddlTermCondition" runat="server" Width="31%" AppendDataBoundItems="true" DataSourceID="DataSourceTermCondition"
                                            DataTextField="sName" DataValueField="lid" TabIndex="14">
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
                                <tr>
                                    <td style="border-right: none">Include Texture?</td>
                                    <td>
                                        <asp:DropDownList ID="ddlIncludeDesc" runat="server" Width="70px" TabIndex="16" AutoPostBack="true" OnSelectedIndexChanged="ddlIncludeDesc_OnSelectedIndexChanged">
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
                                <%--  <tr>
                                    <td style="width: 25%">Copy Charges From An Existing Quotation ?
                                    </td>
                                    <td style="width: 75%">
                                        <asp:DropDownList ID="ddlCopyQuote" Width="70px" runat="server" AutoPostBack="true" TabIndex="18"
                                             Visible="false" OnSelectedIndexChanged="ddlCopyQuote_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Text="NO" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="YES" Value="1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>--%>
                                <%--  <tr id="trCopyQuoteLists" runat="server">
                                    <td>Select Quotation
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlExistQuotes" runat="server" TabIndex="19" AutoPostBack="true" OnSelectedIndexChanged="ddlExistQuotes_SelectedIndexChanged"
                                            DataSourceID="DataSourceExistQuotes" DataTextField="CustWsQuoteNo" DataValueField="lid" AppendDataBoundItems="true" Width="60%">
                                            <asp:ListItem Selected="True" Value="0" Text="-Select-"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>--%>
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
                                                <asp:CheckBox ID="chkLumpSum" runat="server" ToolTip="Prepare Lump-Sum for quote." TabIndex="20" Text="PREPARE LUMP-SUM" AutoPostBack="true" OnCheckedChanged="chkLumpSum_CheckedChanged" />
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
                                                            <asp:TemplateField HeaderText="LumpSum Amount" ItemStyle-Width="42%">
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
                            </div>
                        </fieldset>

                    </div>

                    <div class="m clear">
                        <asp:Button ID="btnSave" Text="Save Quotation" OnClick="btnSave_Click" runat="server" ValidationGroup="vgDraftQuote" />
                        &nbsp;
                    <asp:Button ID="btnDraftQuote" Text="Draft Quote" OnClick="btnDraftQuote_Click" Visible="false" runat="server" ValidationGroup="vgDraftQuote" />
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
                <asp:HiddenField ID="LinkButton1" runat="server"></asp:HiddenField>
            </div>
            <!--Document for Doc Upload - END -->

            <!--Document for Doc Upload-->
            <div id="divTexture">
                <AjaxToolkit:ModalPopupExtender ID="mpeTexture" runat="server" CacheDynamicResults="false"
                    PopupControlID="pnlTexture" TargetControlID="lnkTexture" BackgroundCssClass="modalBackground" DropShadow="true">
                </AjaxToolkit:ModalPopupExtender>
                <asp:Panel ID="pnlTexture" runat="server" CssClass="ModalPopupPanel" Width="800px" Style="border-radius: 10px">
                    <div class="header">
                        <div class="fleft">
                            <img alt="" width="20" height="18" src="../Images/Error.png" />
                            &nbsp;&nbsp;
                           Include Texture
                        </div>
                        <div class="fright">
                            <asp:ImageButton ID="imgbtnDelTexture" ImageUrl="~/Images/delete.gif" runat="server"
                                ToolTip="Close" />
                        </div>
                    </div>
                    <div class="m">
                    </div>
                    <!-- Lists Of All Documents -->
                    <div id="Div1" runat="server" style="max-height: 500px; overflow: auto; padding: 5px">
                        <asp:HiddenField ID="hdnTextureValue" runat="server" />
                        <div>
                            <asp:TextBox ID="txtHTMLContent" runat="server" TextMode="MultiLine" CssClass="mceEditor" TabIndex="12" />
                        </div>
                    </div>

                    <div class="m clear">
                    </div>
                </asp:Panel>
            </div>
            <div>
                <asp:HiddenField ID="lnkTexture" runat="server"></asp:HiddenField>
            </div>
            <!--Document for Doc Upload - END -->
        </ContentTemplate>

    </asp:UpdatePanel>

</asp:Content>

