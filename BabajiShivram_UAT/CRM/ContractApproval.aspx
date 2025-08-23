<%@ Page Title="Contract Approval" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ContractApproval.aspx.cs"
    Inherits="CRM_ContractApproval" Culture="en-GB" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" %>

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
    <script type="text/javascript">
        function GridSelectAllColumn(spanChk) {
            var oItem = spanChk.children;
            var theBox = (spanChk.type == "checkbox") ? spanChk : spanChk.children.item[0]; xState = theBox.checked;
            elm = theBox.form.elements;

            for (i = 0; i < elm.length; i++) {
                if (elm[i].type === 'checkbox' && elm[i].checked != xState)
                    elm[i].click();
            }
        }
    </script>
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
                <asp:ValidationSummary ID="vsRejectContract" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="vgRejectContract" CssClass="errorMsg" />
            </div>
            <div class="clear">
            </div>
            <div class="clear"></div>
            <fieldset class="fieldset-AutoWidth">
                <legend>Contract Approval</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:datafilter id="DataFilter1" runat="server" />
                    </asp:Panel>
                    <asp:Button ID="btnApprove" runat="server" Text="Approve" TabIndex="1" OnClick="btnApprove_Click" />
                    <asp:Button ID="btnReject" runat="server" Text="Reject" TabIndex="2" OnClick="btnReject_Click" />
                    <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
                        <asp:Image ID="imgExcel" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                    <div class="fright">
                        <asp:TextBox ID="lblRejectedSymbol" runat="server" Enabled="false" Style="background-color: #ff000059" Width="50px"></asp:TextBox>
                        Rejected
                    </div>
                </div>
                </div>
                <div class="clear">
                </div>
                <asp:GridView ID="gvLeads" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr" DataSourceID="DataSourceLeads"
                    DataKeyNames="lid,QuotationId" OnRowCommand="gvLeads_RowCommand" AllowPaging="True" AllowSorting="True" CssClass="table" PageSize="20"
                    PagerSettings-Position="TopAndBottom" OnRowDataBound="gvLeads_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkboxSelectAll" align="center" ToolTip="Check All" runat="server" onclick="GridSelectAllColumn(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chk1" runat="server" ToolTip="Check"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" SortExpression="">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnDownloadQuote" runat="server" CausesValidation="false" CommandName="DownloadQuote"
                                    ImageAlign="AbsMiddle" ImageUrl="~/Images/pdf2.png" Width="16" Height="16" ToolTip="Download Quotation in PDF Format."
                                    CommandArgument='<%#Eval("QuotePath") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Lead Ref No">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkLead" runat="server" ToolTip="Click to view lead." Text='<%#Eval("LeadRefNo") %>'
                                    CommandName="viewlead" CommandArgument='<%#Eval("lid")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="LeadRefNo" HeaderText="Lead Ref No" ReadOnly="true" Visible="false" />
                        <asp:BoundField DataField="QuotationStatus" HeaderText="Quote Status" ReadOnly="true" Visible="false" />
                        <asp:BoundField DataField="CompanyName" HeaderText="Company" SortExpression="CompanyName" ReadOnly="true" />
                        <asp:BoundField DataField="Services" HeaderText="Product" SortExpression="Services" ReadOnly="true" />
                        <asp:BoundField DataField="LeadSourceName" HeaderText="Source" SortExpression="LeadSourceName" ReadOnly="true" />
                        <asp:BoundField DataField="ContactName" HeaderText="Contact Name" SortExpression="ContactName" ReadOnly="true" />
                        <asp:BoundField DataField="MobileNo" HeaderText="Mobile No" SortExpression="MobileNo" ReadOnly="true" />
                        <asp:BoundField DataField="RfqReceived" HeaderText="RFQ Recd?" SortExpression="RfqReceived" ReadOnly="true" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="LeadCreatedBy" HeaderText="Lead Owner" SortExpression="LeadCreatedBy" ReadOnly="true" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="CreatedDate" ReadOnly="true" />
                        <asp:BoundField DataField="KYCCreatedDate" HeaderText="KYC Register" DataFormatString="{0:dd/MM/yyyy}" SortExpression="KYCCreatedDate" ReadOnly="true" />
                        <asp:BoundField DataField="Aging" HeaderText="Aging" SortExpression="Aging" ReadOnly="true" />
                        <asp:BoundField DataField="Remark" HeaderText="Rejection Reason" SortExpression="Remark" ReadOnly="true" />
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="DataSourceLeads" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_GetContractForApproval" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>

            <!-- Remark for Contract Rejection-->
            <cc1:modalpopupextender id="mpeContractRejection" runat="server" cachedynamicresults="false"
                popupcontrolid="pnlContractRejection" targetcontrolid="lnkDummy" backgroundcssclass="modalBackground" dropshadow="true">
            </cc1:modalpopupextender>
            <asp:Panel ID="pnlContractRejection" runat="server" CssClass="ModalPopupPanel" Width="500px">
                <div class="header">
                    <div class="fleft">
                        <asp:Label ID="lblPopup_Title" runat="server" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblQuoteRefNo" runat="server"></asp:Label>
                    </div>
                    <div class="fright">
                        <asp:ImageButton ID="imgClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgClose_Click"
                            ToolTip="Close" />
                    </div>
                </div>
                <div class="m">
                </div>

                <div id="Div2" runat="server" style="max-height: 200px; overflow: auto; padding: 5px">
                    <div align="center">
                        <asp:Label ID="lbError_Popup" runat="server" Visible="true"></asp:Label>
                    </div>
                    <div>
                        <asp:GridView ID="gvRejectContract" runat="server" CssClass="table" AutoGenerateColumns="false" Width="99%" Style="white-space: normal; border: 1px solid #cccc; border-radius: 3px">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRowNumber" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lead Id" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLeadId" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lead Ref No" ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLeadRefNo" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomer" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btnDeleteRow" runat="server" OnClick="btnDeleteRow_Click" Text="Remove" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quote Id" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="hdnQuoteId" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <asp:Panel ID="pnlRejection" runat="server">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>Reason
                                    <asp:RequiredFieldValidator ID="rfvRemark" runat="server" ControlToValidate="txtRemark" SetFocusOnError="true" Display="Dynamic"
                                        ErrorMessage="Enter Reason" Text="*" ValidationGroup="vgRejectContract"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Rows="3" Width="350px">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <br />
                            <tr>
                                <td colspan="2" style="text-align: center">
                                    <asp:Button ID="btnRejectContract" runat="server" OnClick="btnRejectContract_Click" CausesValidation="true" ValidationGroup="vgRejectContract" Text="Reject" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlApproval" runat="server" Style="text-align: center">
                        <asp:Button ID="btnApproveContract" runat="server" OnClick="btnApproveContract_Click" CausesValidation="false" Text="Approve" />
                    </asp:Panel>
                </div>
            </asp:Panel>
            <div>
                <asp:HiddenField ID="lnkDummy" runat="server" />
                <asp:SqlDataSource ID="DataSourceStatusHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="BS_GetQuotationStatusHistory" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:ControlParameter Name="QuotationId" ControlID="hdnQuotationId" PropertyName="Value" DbType="Int32" />
                        <asp:SessionParameter Name="UserId" SessionField="UserId" DbType="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
            <!-- Remark for Contract Rejection-->

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

