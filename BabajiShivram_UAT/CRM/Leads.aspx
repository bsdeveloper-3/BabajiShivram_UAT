<%@ Page Title="Leads" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="Leads.aspx.cs" Inherits="CRM_Leads" EnableEventValidation="false" Culture="en-GB" %>
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

        .tdLead {
            margin-bottom: 15px;
        }
    </style>
    <%--    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>--%>
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
                <asp:ValidationSummary ID="vsFollowUp" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="vgFollowUp" CssClass="errorMsg" />
            </div>
            <div class="clear">
            </div>
            <asp:Button ID="btnNewLead" runat="server" Text="New Lead" OnClick="btnNewLead_Click" />
            <asp:Button ID="btnAddEnquiry" runat="server" Text="Existing Customer Enquiry" OnClick="btnAddEnquiry_Click" />
            <div class="clear"></div>
            <fieldset>
                <legend>Leads</legend>
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
                        <asp:TemplateField HeaderText="" ItemStyle-Width="2%">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnFollowup" runat="server" CausesValidation="false" CommandName="followup"
                                    ImageAlign="AbsMiddle" ImageUrl="~/Images/edit.png" Width="16" Height="18" ToolTip="Add follow up for lead."
                                    CommandArgument='<%#Eval("lid") + ";" + Eval("LeadRefNo") %>' Style="padding-bottom: 5px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Lead Ref No" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="left" ItemStyle-CssClass="tdLead">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkLead" runat="server" ToolTip="Click to add lead status." Text='<%#Eval("LeadRefNo") %>'
                                    CommandName="select" CommandArgument='<%#Eval("lid")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="LeadRefNo" HeaderText="Lead Ref No" SortExpression="LeadRefNo" ReadOnly="true" ItemStyle-Width="10%" Visible="false" />
                        <asp:BoundField DataField="CompanyName" HeaderText="Company" SortExpression="CompanyName" ReadOnly="true" ItemStyle-Width="20%" />
                        <asp:BoundField DataField="LeadStageName" HeaderText="Current Status" SortExpression="LeadStageName" ReadOnly="true" ItemStyle-Width="10%" />
                        <asp:TemplateField HeaderText="Remark">
                            <ItemTemplate>
                                <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                <asp:Label ID="lblRemarkView" runat="server" Text='<%#Eval("Remark") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" ReadOnly="true" ItemStyle-Width="30%" />--%>
                        <asp:BoundField DataField="LeadSourceName" HeaderText="Source" SortExpression="LeadSourceName" ReadOnly="true" ItemStyle-Width="5%" />
                        <asp:BoundField DataField="ContactName" HeaderText="Contact Name" SortExpression="ContactName" ReadOnly="true" ItemStyle-Width="13%" />
                        <asp:BoundField DataField="MobileNo" HeaderText="Mobile No" SortExpression="MobileNo" ReadOnly="true" ItemStyle-Width="5%" />
                        <asp:BoundField DataField="RfqReceived" HeaderText="RFQ Received?" SortExpression="RfqReceived" ReadOnly="true" />
                        <asp:BoundField DataField="CreatedBy" HeaderText="Lead Owner" SortExpression="CreatedBy" ReadOnly="true" ItemStyle-Width="5%" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LeadDate" ReadOnly="true" ItemStyle-Width="5%" />
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="DataSourceLeads" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_GetAllLeads" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>

            <!-- Follow up -->
            <cc1:ModalPopupExtender ID="mpeFollowup" runat="server" CacheDynamicResults="false"
                PopupControlID="pnlFollowup" TargetControlID="hdnFollowup" BackgroundCssClass="modalBackground" DropShadow="true">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pnlFollowup" runat="server" CssClass="ModalPopupPanel" Width="700px" Height="300px" Style="border-radius: 6px">
                <div class="header">
                    <div class="fleft">
                        &nbsp;
                            FOLLOW UP -
                            <asp:Label ID="lblLeadNo" runat="server" Font-Bold="true" Font-Underline="true"></asp:Label>
                        <asp:HiddenField ID="hdnLeadId" runat="server" Value="0" />
                    </div>
                    <div class="fright">
                        <asp:ImageButton ID="imgbtnClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgbtnClose_Click" ToolTip="Close" />
                    </div>
                </div>
                <div class="clear" style="text-align: center">
                    <asp:Label ID="lblError_Followup" runat="server" EnableViewState="false"></asp:Label>
                </div>
                <div class="m clear">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>FollowUp Date
                                    <cc1:CalendarExtender ID="calFollowupDate" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="imgFollowupDate"
                                        Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtFollowupDate">
                                    </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="meeFollowupDate" TargetControlID="txtFollowupDate" Mask="99/99/9999" MessageValidatorTip="true"
                                    MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="mevFollowupDate" ControlExtender="meeFollowupDate" ControlToValidate="txtFollowupDate" IsValidEmpty="true"
                                    InvalidValueMessage="FollowUp Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true"
                                    MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2001" MaximumValue="31/12/2030"
                                    EmptyValueBlurredText="*" EmptyValueMessage="Please Enter FollowUp Date" runat="Server" ValidationGroup="vgFollowUp"></cc1:MaskedEditValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFollowupDate" runat="server" Width="100px" placeholder="dd/mm/yyyy" TabIndex="1"></asp:TextBox>
                                <asp:Image ID="imgFollowupDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>Remark (if any)</td>
                            <td>
                                <asp:TextBox ID="txtRemark" runat="server" Rows="2" TextMode="MultiLine" TabIndex="2"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Is Active?</td>
                            <td>
                                <asp:CheckBox ID="chkIsActive" runat="server" TabIndex="3" Text="Yes" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnAddFollowup" runat="server" OnClick="btnAddFollowup_Click" ValidationGroup="vgFollowUp" Text="Save" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="m clear">
                </div>
                <div id="Div3" runat="server" style="max-height: 250px; overflow: auto; padding: 5px">
                    <asp:GridView ID="gvFollowupHistory" runat="server" CssClass="table" AutoGenerateColumns="false"
                        PagerStyle-CssClass="pgr" DataKeyNames="lid" AllowPaging="True" AllowSorting="True" PageSize="20"
                        PagerSettings-Position="TopAndBottom" Width="100%"
                        DataSourceID="DataSourceFollowup">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Date" HeaderText="Followup Date" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="Remark" HeaderText="Remark" />
                            <asp:BoundField DataField="Status" HeaderText="Status" />
                            <asp:BoundField DataField="ActiveStatus" HeaderText="Active" />
                            <asp:BoundField DataField="CreatedBy" HeaderText="Created By" />
                            <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="UpdatedBy" HeaderText="Updated By" Visible="false" />
                            <asp:BoundField DataField="UpdatedDate" HeaderText="Updated Date" DataFormatString="{0:dd/MM/yyyy}" Visible="false" />
                        </Columns>
                    </asp:GridView>
                    <div>
                        <asp:SqlDataSource ID="DataSourceFollowup" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="CRM_GetFollowupHistoryByLead" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hdnLeadId" Name="LeadId" PropertyName="Value" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </div>
            </asp:Panel>
            <asp:HiddenField ID="hdnFollowup" runat="server"></asp:HiddenField>
            <!-- Follow up -->

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

