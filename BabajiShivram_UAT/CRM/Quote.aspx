<%@ Page Title="Quote" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Quote.aspx.cs"
    Inherits="CRM_Quote" Culture="en-GB" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
    </asp:ScriptManager>
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
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlLeads" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../Images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upnlLeads" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="vsLeads" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="vgRequired" CssClass="errorMsg" />
                <asp:ValidationSummary ID="vsAddStatus" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="vgAddStatus" CssClass="errorMsg" />
                <asp:ValidationSummary ID="vsRFQ" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="vgRFQ" CssClass="errorMsg" />
                <asp:HiddenField ID="hdnExistEnquiry" runat="server" Value="0" />
            </div>
            <div class="clear">
            </div>
            <div class="clear"></div>
            <fieldset class="fieldset-AutoWidth">
                <legend>Quote</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                        </div>
                        <div class="fleft" style="margin-left: 2px;">
                            <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
                                <asp:Image ID="imgExcel" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
                <div class="clear">
                </div>
                <asp:GridView ID="gvLeads" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr" DataSourceID="DataSourceLeads"
                    DataKeyNames="lid" OnRowCommand="gvLeads_RowCommand" AllowPaging="True" AllowSorting="True" CssClass="table" PageSize="20"
                    PagerSettings-Position="TopAndBottom" OnRowDataBound="gvLeads_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" SortExpression="">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnDownloadQuote" runat="server" CausesValidation="false" CommandName="DownloadQuote"
                                    ImageAlign="AbsMiddle" ImageUrl="~/Images/pdf2.png" Width="16" Height="16" ToolTip="Download Quotation in PDF Format."
                                    CommandArgument='<%#Eval("QuotationId") + ";" +Eval("QuotePath") %>' />
                                <asp:ImageButton ID="imgbtnEditQuote" runat="server" CausesValidation="false" CommandName="EditQuote"
                                    ImageAlign="AbsMiddle" ImageUrl="~/Images/edit.png" Width="16" Height="16" ToolTip="Edit Quote"
                                    CommandArgument='<%#Eval("QuotationId") + ";" + Eval("StatusId")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Lead Ref No">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkLead" runat="server" ToolTip="Click to view lead." Text='<%#Eval("LeadRefNo") %>'
                                    CommandName="viewlead" CommandArgument='<%#Eval("lid")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Quote Status" SortExpression="QuotationStatus">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnStatus" runat="server" Text='<%# Eval("QuotationStatus") %>' CommandArgument='<%#Eval("QuotationId") +";" + Eval("LeadRefNo") + ";" + Eval("EnquiryId") + ";" + Eval("ExistCustEnquiry")%>'
                                    CommandName="getstatus" ToolTip="Update Status For Quotation."></asp:LinkButton>
                                <asp:LinkButton ID="lnkAddQuote" runat="server" Text="Add Quote" OnClick="popup_Click" CommandName="addquote"
                                    CommandArgument='<%# Eval("LeadRefNo") + ";" +Eval("lid")%>'
                                    ToolTip="Add quote"></asp:LinkButton><%-- CommandArgument='<%#Eval("lid") + ";" + Eval("RfqReceived") + ";" + Eval("ExistCustEnquiry") + ";" + Eval("EnquiryId")%>'--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="LeadRefNo" HeaderText="Lead Ref No" ReadOnly="true" Visible="false" />
                        <asp:BoundField DataField="QuotationStatus" HeaderText="Quote Status" ReadOnly="true" SortExpression="QuotationStatus" Visible="false" />
                        <asp:BoundField DataField="CompanyName" HeaderText="Company" SortExpression="CompanyName" ReadOnly="true" />
                        <asp:BoundField DataField="Services" HeaderText="Product" SortExpression="Services" ReadOnly="true" />
                        <asp:BoundField DataField="LeadSourceName" HeaderText="Source" SortExpression="LeadSourceName" ReadOnly="true" />
                        <asp:BoundField DataField="ContactName" HeaderText="Contact Name" SortExpression="ContactName" ReadOnly="true" />
                        <asp:BoundField DataField="MobileNo" HeaderText="Mobile No" SortExpression="MobileNo" ReadOnly="true" />
                        <asp:BoundField DataField="RfqReceived" HeaderText="RFQ Received?" SortExpression="RfqReceived" ReadOnly="true" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="LeadCreatedBy" HeaderText="Lead Owner" SortExpression="LeadCreatedBy" ReadOnly="true" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="CreatedDate" ReadOnly="true" />
                        <asp:BoundField DataField="KYCCreatedDate" HeaderText="KYC Register" DataFormatString="{0:dd/MM/yyyy}" SortExpression="KYCCreatedDate" ReadOnly="true" />
                        <asp:BoundField DataField="Aging" HeaderText="Aging" SortExpression="Aging" ReadOnly="true" />
                        <asp:BoundField DataField="CustomerQuote" HeaderText="Cust Quote" ReadOnly="true" SortExpression="CustomerQuote" />
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="DataSourceLeads" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_GetPendingQuote" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>

            <!-- Update Status of Quotation-->
            <cc1:ModalPopupExtender ID="ModalPopupDocument" runat="server" CacheDynamicResults="false"
                PopupControlID="Panel2Document" TargetControlID="lnkDummy" BackgroundCssClass="modalBackground" DropShadow="true">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="Panel2Document" runat="server" CssClass="ModalPopupPanel" Width="500px">
                <div class="header">
                    <div class="fleft">
                        Update Status of Quotation -
                        <asp:Label ID="lblLeadRefNo" runat="server"></asp:Label>
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
                        <asp:HiddenField ID="hdnEnquiryId" runat="server" Value="0" />
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
                                <td id="tdTargetMonth" runat="server" visible="false">Target Month
                                </td>
                                <td>
                                    <%--<cc1:CalendarExtender ID="calCloseDate" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="txtTargetDt"
                                    Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtCloseDate">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="meeCloseDate" TargetControlID="txtCloseDate" Mask="99/99/9999" MessageValidatorTip="true"
                                    MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="mevCloseDate" ControlExtender="meeCloseDate" ControlToValidate="txtTargetDt" IsValidEmpty="true"
                                    InvalidValueMessage="Expected Close Date is invalid" InvalidValueBlurredMessage="Invalid Expected Close Date" SetFocusOnError="true"
                                    MinimumValueMessage="Invalid Expected Close Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="31/12/2025"
                                    runat="Server"></cc1:MaskedEditValidator>--%>
                                <asp:TextBox ID="txtTargetDt" runat="server" Width="125px" placeholder="dd/mm/yyyy" ToolTip="Enter Expected Close Date." Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Remark (if any)
                                      <asp:RequiredFieldValidator ID="rfvrem" runat="server" ControlToValidate="txtRemark" SetFocusOnError="true" Display="Dynamic"
                                          ErrorMessage="Enter Remark." Text="*" ValidationGroup="vgAddStatus"></asp:RequiredFieldValidator>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Rows="3" Width="400px">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
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
            <div>
                <asp:LinkButton ID="lnkDummy" runat="server" Text=""></asp:LinkButton>
                <asp:SqlDataSource ID="DataSourceStatusHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="BS_GetQuotationStatusHistory" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:ControlParameter Name="QuotationId" ControlID="hdnQuotationId" PropertyName="Value" DbType="Int32" />
                        <asp:SessionParameter Name="UserId" SessionField="UserId" DbType="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceStatus" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="BS_GetQuotationStatus" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </div>
            <!-- Update Status of Quotation-->

            <!--For Add Quote-->
                <cc1:ModalPopupExtender ID="mpeRFQStatus" runat="server" CacheDynamicResults="false"
                PopupControlID="pnlRFQStatus" TargetControlID="hdnLeadId" BackgroundCssClass="modalBackground" DropShadow="true">
            </cc1:ModalPopupExtender>

            <asp:panel ID="pnlRFQStatus" runat="server" CssClass="ModalPopupPanel" Width="800px" Style="border-radius:6px">
                <div class="header">
                    <div class="fleft">
                        &nbsp;
                            RFQ RECEIVED - 
                        <asp:Label ID="lbl_LeadRefNo" runat="server" Font-Bold="true" Font-Underline="true"></asp:Label>
                    </div>
                    <div class="fright">
                        <asp:ImageButton ID="imgClose_Forstatus" ImageUrl="~/Images/delete.gif" runat="server"  ToolTip="Close" OnClick="imgClose_Forstatus_Click" />
                    </div>
                </div>
                <div class="clear" style="text-align:center">
                    <asp:Label ID="lblLeaderror" runat="server" EnableViewState="false"></asp:Label>
                </div>
                <div class="m clear" runat="server" style="max-height:200px; overflow:auto; padding:5px">
                    <table border="0" cellpadding="0" cellspacing="0" bgcolor:"white">
                        <tr>
                           <%-- <td>
                                RFQ Received &nbsp; &nbsp; &nbsp;
                                <asp:RequiredFieldValidator ID="rfvRFQReceived" runat="server" ControlToValidate="ddlRFQ" Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Please select RFQ" Text="*" ValidationGroup="vgRFQ" InitialValue="0" ForeColor="Red" EnableClientScript="true"></asp:RequiredFieldValidator>
                            </td>--%>
                            <td colspan="2">
                                <%--<asp:DropDownList ID="ddlRFQ" runat="server" >
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                </asp:DropDownList>--%>

                                <asp:RadioButtonList ID="ddlRFQ" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="RFQ Received" Value="1" />
                                    <asp:ListItem Text="Standard Quote" Value="2" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td> &nbsp;</td>
                        </tr>
                       
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnok" runat="server" ValidationGroup="vgRFQ" Text="OK" OnClick="btnok_Click" CausesValidation="true" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                            </td>
                        </tr>
                        <caption>
                            <br />
                        </caption>
                    </table>
                </div>
            </asp:panel>
            <!--For Add Quote-->
            <asp:HiddenField ID="hdnLeadId" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

