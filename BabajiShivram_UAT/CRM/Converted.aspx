<%@ Page Title="Active Leads" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Converted.aspx.cs"
    Inherits="CRM_Converted" Culture="en-GB" EnableEventValidation="false" %>

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
            </div>
            <div class="clear"></div>
            <fieldset class="fieldset-AutoWidth">
                <legend>Lead - Active</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                        </div>
                        <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
                            <asp:Image ID="imgExcel" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                        </asp:LinkButton>
                        <div class="fright">
                            <asp:TextBox ID="lblRejectedSymbol" runat="server" Enabled="false" Style="background-color: #ff000059" Width="50px"></asp:TextBox>
                            No Services Added
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
                        <asp:TemplateField HeaderText="" ItemStyle-Width="5%">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnFollowup" runat="server" CausesValidation="false" CommandName="followup"
                                    ImageAlign="AbsMiddle" ImageUrl="~/Images/edit.png" Width="18" Height="19" ToolTip="Add follow up for lead"
                                    CommandArgument='<%#Eval("lid") + ";" + Eval("LeadRefNo") %>' Style="padding-bottom: 5px" />
                                <asp:ImageButton ID="imgbtnStatus" runat="server" CausesValidation="false" CommandName="status"
                                    ImageAlign="AbsMiddle" ImageUrl="~/Images/BlackStatus.png" Width="18" Height="20" ToolTip="Change lead status"
                                    CommandArgument='<%#Eval("lid") + ";" + Eval("LeadRefNo")  + ";" + Eval("Services")%>' Style="padding-bottom: 5px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Lead Ref No">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkLead" runat="server" ToolTip="View Lead" Text='<%#Eval("LeadRefNo") %>'
                                    CommandName="select" CommandArgument='<%#Eval("lid") + ";" + Eval("RfqReceived")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="LeadRefNo" HeaderText="Lead Ref No" SortExpression="LeadRefNo" ReadOnly="true" Visible="false" />
                        <asp:BoundField DataField="CompanyName" HeaderText="Company Name" SortExpression="CompanyName" ReadOnly="true" />
                        <asp:BoundField DataField="LeadStageName" HeaderText="Lead Status" SortExpression="LeadStageName" ReadOnly="true" />
                        <asp:BoundField DataField="LeadSourceName" HeaderText="Source" SortExpression="LeadSourceName" ReadOnly="true" />
                        <%--<asp:BoundField DataField="Services" HeaderText="Services" SortExpression="Services" ReadOnly="true" Visible="false" />--%>
                        <asp:BoundField DataField="ContactName" HeaderText="Contact Name" SortExpression="ContactName" ReadOnly="true" />
                        <asp:BoundField DataField="MobileNo" HeaderText="Mobile No" SortExpression="MobileNo" ReadOnly="true" />
                        <%--<asp:BoundField DataField="RfqReceived" HeaderText="RFQ Received?" SortExpression="RfqReceived" ReadOnly="true" />--%>
                        <asp:BoundField DataField="CreatedBy" HeaderText="Lead Owner" SortExpression="CreatedBy" ReadOnly="true" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LeadDate" ReadOnly="true" />
                        <asp:BoundField DataField="VisitDt" HeaderText="Last Visit Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="VisitDt" ReadOnly="true" />
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="DataSourceLeads" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_GetAllOpportunities" SelectCommandType="StoredProcedure">
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
                                <asp:ControlParameter ControlID="hdnLeadId_ForStatus" Name="LeadId" PropertyName="Value" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </div>
            </asp:Panel>
            <asp:HiddenField ID="hdnFollowup" runat="server"></asp:HiddenField>
            <!-- Follow up -->

            <!-- Lead Status -->
            <cc1:ModalPopupExtender ID="mpeLeadStatus" runat="server" CacheDynamicResults="false"
                PopupControlID="pnlLeadStatus" TargetControlID="hdnLeadStatus" BackgroundCssClass="modalBackground" DropShadow="true">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pnlLeadStatus" runat="server" CssClass="ModalPopupPanel" Width="800px" Style="border-radius: 6px">
                <div class="header">
                    <div class="fleft">
                        &nbsp;
                            LEAD STATUS - 
                            <asp:Label ID="lblLeadNo_ForStatus" runat="server" Font-Bold="true" Font-Underline="true"></asp:Label>
                        <asp:HiddenField ID="hdnLeadId_ForStatus" runat="server" Value="0" />
                    </div>
                    <div class="fright">
                        <asp:ImageButton ID="imgClose_Forstatus" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgClose_Forstatus_Click" ToolTip="Close" />
                    </div>
                </div>
                <div class="clear" style="text-align: center">
                    <asp:Label ID="lblError_ForStatus" runat="server" EnableViewState="false"></asp:Label>
                </div>
                <div class="m clear" runat="server" style="max-height: 200px; overflow: auto; padding: 5px">
                    <table border="0" cellpadding="0" cellspacing="0" bgcolor="white">
                        <tr>
                            <td>Status
                                <asp:RequiredFieldValidator ID="rfvstatus" runat="server" ControlToValidate="ddlStatus" Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Please Select Status." Text="*" ValidationGroup="vgAddStatus" InitialValue="0" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server" TabIndex="31" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"
                                    AutoPostBack="true">
                                    <asp:ListItem Selected="True" Value="0" Text="-Select-"></asp:ListItem>
                                    <%--<asp:ListItem Value="2" Text="Follow Up"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Under Progress"></asp:ListItem>--%>
                                    <asp:ListItem Value="18" Text="Expected To Close"></asp:ListItem>
                                    <asp:ListItem Value="19" Text="Active Follow up"></asp:ListItem>
                                    <asp:ListItem Value="21" Text="Passive Follow up"></asp:ListItem>
                                    <asp:ListItem Value="20" Text="No response"></asp:ListItem>
                                    <asp:ListItem Value="9" Text="Lost"></asp:ListItem>
                                    <asp:ListItem Value="16" Text="Cold"></asp:ListItem>
                                    <asp:ListItem Value="7" Text="Quote"></asp:ListItem>
                                    
                                    
                                </asp:DropDownList>
                            </td>
                            <td id="tdDate" runat="server" visible="false">
                                Target Month
                            </td>
                            <td>
                                <cc1:CalendarExtender ID="calCloseDate" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="txtCloseDate"
                                    Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtCloseDate">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="meeCloseDate" TargetControlID="txtCloseDate" Mask="99/99/9999" MessageValidatorTip="true"
                                    MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="mevCloseDate" ControlExtender="meeCloseDate" ControlToValidate="txtCloseDate" IsValidEmpty="true"
                                    InvalidValueMessage="Expected Close Date is invalid" InvalidValueBlurredMessage="Invalid Expected Close Date" SetFocusOnError="true"
                                    MinimumValueMessage="Invalid Expected Close Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="31/12/2025"
                                    runat="Server"></cc1:MaskedEditValidator>
                                <asp:TextBox ID="txtCloseDate" runat="server" Width="125px" placeholder="dd/mm/yyyy" ToolTip="Enter Expected Close Date." Visible="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Remark
                                <asp:RequiredFieldValidator ID="rfvremark" runat="server" ControlToValidate="txtRemark_Forstatus" SetFocusOnError="true" Display="Dynamic"
                                    ErrorMessage="Enter Remark." Text="*" ValidationGroup="vgAddStatus"></asp:RequiredFieldValidator>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtRemark_Forstatus" runat="server" TextMode="MultiLine" Rows="3" Width="500px" TabIndex="32">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnSaveStatus" runat="server" OnClick="btnSaveStatus_Click" CausesValidation="true" ValidationGroup="vgAddStatus"
                                    Text="Save Status" TabIndex="33" />
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="gvLeadStatusHistory" runat="server" CssClass="table" PagerStyle-CssClass="pgr"
                        AllowSorting="true" AutoGenerateColumns="false" AllowPaging="true" DataSourceID="DataSourceLeadStatusHistory"
                        PageSize="30" PagerSettings-Position="TopAndBottom" Style="white-space: normal"
                        DataKeyNames="lid" Width="98%">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Status" DataField="LeadStageName" ReadOnly="true" />
                            <asp:BoundField HeaderText="Remark" DataField="Remark" ReadOnly="true" />
                            <asp:BoundField HeaderText="Active/Inactive" DataField="Status" ReadOnly="true" />
                            <asp:BoundField HeaderText="Modified By" DataField="UpdatedBy" ReadOnly="true" />
                            <asp:BoundField HeaderText="Modified Date" DataField="UpdateOn" ReadOnly="true" />
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="DataSourceLeadStatusHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="CRM_GetLeadStageHistory" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter Name="LeadId" ControlID="hdnLeadId_ForStatus" PropertyName="Value" DbType="Int32" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
                <div class="m clear">
                </div>
            </asp:Panel>
            <asp:HiddenField ID="hdnLeadStatus" runat="server"></asp:HiddenField>
            <!-- Lead Status -->

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

