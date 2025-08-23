<%@ Page Title="Quotation Lists" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AdminLevelQuote.aspx.cs"
    Inherits="Quotation_AdminLevelQuote" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <style type="text/css">
        .modalBackground {
            background-color: black;
            filter: alpha(opacity=90);
            opacity: 0.5;
        }

        .modalPopup1 {
            border-radius: 5px;
            background-color: #FFFFFF;
            border-width: 1px;
            border-style: solid;
            border-color: black;
            padding-top: 5px;
            padding-left: 3px;
            width: 600px;
            height: 300px;
        }
    </style>
    <div>
        <asp:ValidationSummary ID="vsAddStatus" runat="server" ShowMessageBox="true" ShowSummary="false"
            ValidationGroup="vgAddStatus" />
    </div>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upQuotedDraft" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); opacity: .8;">
                    <img alt="progress" src="../Images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upQuotedDraft" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>

            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" />
            </div>

            <fieldset>
                <legend>Quotations</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                        </div>
                        <div class="fleft" style="margin-left: 30px; padding-top: 3px;">
                            <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click" Visible="false">
                                <asp:Image ID="Image2" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
                <br />
                <asp:GridView ID="gvQuotationDetails" runat="server" CssClass="table" AutoGenerateColumns="false"
                    PagerStyle-CssClass="pgr" DataKeyNames="lid" AllowPaging="True" AllowSorting="True" PageSize="20"
                    PagerSettings-Position="TopAndBottom" OnRowCommand="gvQuotationDetails_RowCommand" Width="100%"
                    OnPreRender="gvQuotationDetails_PreRender" DataSourceID="DataSourceQuotedDetail2" OnRowDataBound="gvQuotationDetails_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Quotation Ref No" SortExpression="QuoteRefNo">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEditQuotation" CommandName="GetQuote" ToolTip="Edit Quotation" runat="server"
                                    Text='<%#Eval("QuoteRefNo") %>' CommandArgument='<%#Eval("lid") + ";" + Eval("ApprovalStatusId") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="QuoteRefNo" HeaderText="Quotation Ref No" SortExpression="QuoteRefNo" ReadOnly="true" Visible="false" />
                        <asp:TemplateField HeaderText="Status" SortExpression="ApprovalStatus">
                            <ItemTemplate>
                                <asp:LinkButton ID="lblStatus" runat="server" Text='<%# Eval("ApprovalStatus") %>' CommandArgument='<%#Eval("lid") %>' Enabled="false"
                                    CommandName="getstatus" ToolTip="Update Status For Quotation."></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" SortExpression="">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnDownloadQuote" runat="server" CausesValidation="false" CommandName="DownloadQuote"
                                    ImageAlign="AbsMiddle" ImageUrl="~/Images/pdf2.png" Width="16" Height="16" ToolTip="Download Quotation in PDF Format."
                                    CommandArgument='<%#Eval("QuotePath") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Related Documents" SortExpression="">
                            <ItemTemplate>
                                <asp:LinkButton ID="lblRelatedDocuments" runat="server" Text="Show Documents" CommandArgument='<%#Eval("lid") %>'
                                    CommandName="getdoc" ToolTip="Show Related Documents For Quotation."></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CustomerName" HeaderText="Customer" SortExpression="CustomerName" ReadOnly="true" />
                        <asp:BoundField DataField="QuotedFormat" HeaderText="Quotation Type" SortExpression="QuotedFormat" ReadOnly="true" />
                        <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category" ReadOnly="true" />
                        <asp:BoundField DataField="Mode" HeaderText="Mode" SortExpression="Mode" ReadOnly="true" />
                        <asp:BoundField DataField="CreatedBy" HeaderText="Quoted By" SortExpression="CreatedBy" ReadOnly="true" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="Quoted On" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                        <asp:BoundField DataField="ModifiedBy" HeaderText="Modified By" SortExpression="ModifiedBy" ReadOnly="true" />
                        <asp:BoundField DataField="ModifiedDate" HeaderText="Modified On" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                        <asp:BoundField DataField="ApprovalStatus" HeaderText="Status" SortExpression="ApprovalStatus" ReadOnly="true" Visible="false" />

                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="DataSourceQuotedDetail2" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="BS_GetDraftQuotationLists" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>

            <!-- Update Status of Quotation-->
            <div id="divDocument">
                <cc1:ModalPopupExtender ID="ModalPopupDocument" runat="server" CacheDynamicResults="false"
                    PopupControlID="Panel2Document" TargetControlID="lnkDummy" BackgroundCssClass="modalBackground" DropShadow="true">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="Panel2Document" runat="server" CssClass="ModalPopupPanel" Width="500px">
                    <div class="header">
                        <div class="fleft">
                            Update Status of Quotation
                        </div>
                        <div class="fright">
                            <asp:ImageButton ID="imgClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="btnCancelPopup_Click"
                                ToolTip="Close" />
                        </div>
                    </div>
                    <div class="m">
                    </div>

                    <div id="Div2" runat="server" style="max-height: 200px; overflow: auto; padding: 5px">
                        <asp:HiddenField ID="hdnQuotationId" runat="server" />
                        <div align="center">
                            <asp:Label ID="lbError_Popup" runat="server" Visible="true"></asp:Label>
                        </div>
                        <div>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>Select Status
                                      <asp:RequiredFieldValidator ID="rfvstatus" runat="server" ControlToValidate="ddlStatus" Display="Dynamic" SetFocusOnError="true"
                                          ErrorMessage="Please Select Status." Text="*" ValidationGroup="vgAddStatus" InitialValue="0" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlStatus" runat="server" AppendDataBoundItems="true" DataSourceID="DataSourceStatus"
                                            DataTextField="sName" DataValueField="lid" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="0" Text="-Select-"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Remark (if any)
                                      <asp:RequiredFieldValidator ID="rfvrem" runat="server" ControlToValidate="txtRemark" SetFocusOnError="true" Display="Dynamic"
                                          ErrorMessage="Enter Remark." Text="*" ValidationGroup="vgAddStatus"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Rows="3" Width="250px">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_OnClick" CausesValidation="true" ValidationGroup="vgAddStatus"
                                            Text="Save Status" />
                                    </td>
                                </tr>
                            </table>
                            <asp:GridView ID="gvStatusHistory" runat="server" CssClass="table" PagerStyle-CssClass="pgr"
                                AllowSorting="true" AutoGenerateColumns="false" AllowPaging="true" DataSourceID="DataSourceStatusHistory"
                                PageSize="30" PagerSettings-Position="TopAndBottom" Style="white-space: normal"
                                DataKeyNames="lid" Width="98%">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Status" DataField="StatusName" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Remark" DataField="Remark" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Modified By" DataField="ModifiedBy" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Modified Date" DataField="ModifiedDate" ReadOnly="true" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div>
                <asp:LinkButton ID="lnkDummy" runat="server" Text=""></asp:LinkButton>
                <asp:SqlDataSource ID="DataSourceStatusHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="BS_GetQuotationStatusHistory" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="QuotationId" SessionField="QuotationId" DbType="Int32" />
                        <asp:SessionParameter Name="UserId" SessionField="UserId" DbType="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceStatus" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="BS_GetQuotationStatus" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </div>
            <!-- Update Status of Quotation - END -->

            <!-- Document for Doc Upload-->
            <div id="divContractCopy">
                <cc1:ModalPopupExtender ID="mpeContractCopy" runat="server" CacheDynamicResults="false"
                    PopupControlID="pnlContractCopy" TargetControlID="lnkbtnContractCopy" BackgroundCssClass="modalBackground" DropShadow="true">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pnlContractCopy" runat="server" CssClass="ModalPopupPanel" Width="500px" Style="border-radius: 10px">
                    <div class="header">
                        <div class="fleft">
                            UPLOAD DOCUMENTS 
                        </div>
                        <div class="fright">
                            <asp:ImageButton ID="imgbtnContract" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgbtnContract_Click"
                                ToolTip="Close" />
                        </div>
                    </div>
                    <div class="m">
                    </div>
                    <!-- Lists Of All Documents -->
                    <div id="Div3" runat="server" style="max-height: 250px; overflow: auto; padding: 5px">
                        <div class="clear" style="text-align: center">
                            <asp:Label ID="lblErrorContract" runat="server" EnableViewState="false"></asp:Label>
                        </div>
                        <div>
                            <asp:GridView ID="gvContractCopy" runat="server" CssClass="table" AutoGenerateColumns="false"
                                PagerStyle-CssClass="pgr" DataKeyNames="lid" AllowPaging="True" AllowSorting="True" PageSize="20"
                                PagerSettings-Position="TopAndBottom" OnRowCommand="gvContractCopy_RowCommand" Width="100%"
                                DataSourceID="DataSourceContractDocuments">
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
                            <asp:SqlDataSource ID="DataSourceContractDocuments" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="BS_GetQuotationDocDetail" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="QuotationId" SessionField="QuotationId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </div>

                    <div class="m clear">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr id="trStartDt" runat="server">
                                <td>Contract Start Date
                                     <cc1:CalendarExtender ID="calStartDt" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="imgStartDt"
                                         Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtContractStartDt">
                                     </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="meeStartDt" TargetControlID="txtContractStartDt" Mask="99/99/9999" MessageValidatorTip="true"
                                        MaskType="Date" AutoComplete="false" runat="server">
                                    </cc1:MaskedEditExtender>
                                    <cc1:MaskedEditValidator ID="mevStartDt" ControlExtender="meeStartDt" ControlToValidate="txtContractStartDt" IsValidEmpty="false"
                                        InvalidValueMessage="Contract Start Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true"
                                        MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2001" MaximumValue="31/12/2030"
                                        EmptyValueBlurredText="*" EmptyValueMessage="Please Enter Contract Start Date."
                                        runat="Server" ValidationGroup="ContractReq"></cc1:MaskedEditValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtContractStartDt" runat="server" Width="100px" placeholder="dd/mm/yyyy" TabIndex="1"></asp:TextBox>
                                    <asp:Image ID="imgStartDt" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                </td>
                            </tr>
                            <tr id="trEndDt" runat="server">
                                <td>Contract End Date
                                     <cc1:CalendarExtender ID="calEndDt" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="imgEndDt"
                                         Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtContractEndDt">
                                     </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="meeEndDt" TargetControlID="txtContractEndDt" Mask="99/99/9999" MessageValidatorTip="true"
                                        MaskType="Date" AutoComplete="false" runat="server">
                                    </cc1:MaskedEditExtender>
                                    <cc1:MaskedEditValidator ID="mevEndDt" ControlExtender="meeEndDt" ControlToValidate="txtContractEndDt" IsValidEmpty="false"
                                        InvalidValueMessage="Contract End Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true"
                                        MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2001" MaximumValue="31/12/2030"
                                        EmptyValueBlurredText="*" EmptyValueMessage="Please Enter Contract End Date."
                                        runat="Server" ValidationGroup="ContractReq"></cc1:MaskedEditValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtContractEndDt" runat="server" Width="100px" placeholder="dd/mm/yyyy" TabIndex="2"></asp:TextBox>
                                    <asp:Image ID="imgEndDt" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>Upload Contract Copy
                                </td>
                                <td>
                                    <asp:FileUpload ID="fuUploadContractCopy" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="btnAddContractCopy" runat="server" OnClick="btnAddContractCopy_OnClick" ValidationGroup="ContractReq" Text="Save Document" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="m clear">
                    </div>
                </asp:Panel>
            </div>
            <div>
                <asp:HiddenField ID="lnkbtnContractCopy" runat="server"></asp:HiddenField>
            </div>
            <!-- Document for Doc Upload - END -->

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

