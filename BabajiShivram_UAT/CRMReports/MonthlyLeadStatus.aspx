<%@ Page Title="Month Wise Lead Status" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MonthlyLeadStatus.aspx.cs"
    Inherits="CRMReports_MonthlyLeadStatus" Culture="en-GB" %>

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
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlMonthwsLeadStatus" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../Images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upnlMonthwsLeadStatus" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <fieldset>
                <div class="m clear">
                    <div class="fleft" style="margin-right: 5px;">
                        <asp:DropDownList ID="ddlUser" runat="server" DataSourceID="DataSourceSalesPerson" DataTextField="sName"
                            DataValueField="lid" AppendDataBoundItems="true" TabIndex="2" Width="250px" AutoPostBack="true" OnSelectedIndexChanged="ddlUser_SelectedIndexChanged">
                            <asp:ListItem Text="- Select Sales Person -" Value="0" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;
                         <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
                            <asp:Image ID="imgExcel" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                        </asp:LinkButton>
                    </div>
                </div>
                <legend>Month Wise Lead Status</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                        </div>
                    </asp:Panel>
                </div>
                <div class="clear">
                </div>
                <asp:GridView ID="gvLeads" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr" DataSourceID="DataSourceLeads"
                    DataKeyNames="MonthId" CssClass="table" OnPreRender="gvLeads_PreRender" OnRowCommand="gvLeads_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Month" HeaderText="Month" SortExpression="Month" ReadOnly="true" />
                        <asp:TemplateField HeaderText="MGMT Approval Pending">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnMGMTApproval" runat="server" Text='<%#Eval("MGMTApproval") %>' Font-Underline="false" CommandName="Status_1" CommandArgument='<%#Eval("MonthId") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Lead Approved">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnLeadApproved" runat="server" Text='<%#Eval("LeadApproved") %>' Font-Underline="false" CommandName="Status_2" CommandArgument='<%#Eval("MonthId") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rejected">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnRejected" runat="server" Text='<%#Eval("Rejected") %>' Font-Underline="false" CommandName="Status_3" CommandArgument='<%#Eval("MonthId") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Follow Up">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnFollowUp" runat="server" Text='<%#Eval("FollowUp") %>' Font-Underline="false" CommandName="Status_4" CommandArgument='<%#Eval("MonthId") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Under Progress">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnUnderProgress" runat="server" Text='<%#Eval("UnderProgress") %>' Font-Underline="false" CommandName="Status_5" CommandArgument='<%#Eval("MonthId") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Quote Pending">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnQuotePending" runat="server" Text='<%#Eval("QuotePending") %>' Font-Underline="false" CommandName="Status_6" CommandArgument='<%#Eval("MonthId") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="KYC Pending">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnKycPending" runat="server" Text='<%#Eval("KycPending") %>' Font-Underline="false" CommandName="Status_7" CommandArgument='<%#Eval("MonthId") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Contract Pending">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnContractPending" runat="server" Text='<%#Eval("ContractPending") %>' Font-Underline="false" CommandName="Status_8" CommandArgument='<%#Eval("MonthId") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnTotal" runat="server" Text='<%#Eval("Total") %>' Font-Underline="false" CommandName="Status_9" CommandArgument='<%#Eval("MonthId") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </fieldset>
            <asp:SqlDataSource ID="DataSourceLeads" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="CRM_rptMonthWiseLeadStatus" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:ControlParameter ControlID="ddlUser" PropertyName="SelectedValue" Name="UserId" DefaultValue="0" />
                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="DataSourceSalesPerson" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                SelectCommand="CRM_GetSalesPerson" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="UserId" SessionField="UserId" />
                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                </SelectParameters>
            </asp:SqlDataSource>

            <!-- Status of Lead-->
            <cc1:ModalPopupExtender ID="mpeLeadStatus" runat="server" CacheDynamicResults="false"
                PopupControlID="pnlLeadStatus" TargetControlID="lnkDummy" BackgroundCssClass="modalBackground" DropShadow="true">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pnlLeadStatus" runat="server" CssClass="ModalPopupPanel" Width="1300px">
                <div class="header">
                    <div class="fleft">
                        <asp:Label ID="lblStatusTitle" runat="server"></asp:Label>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkExport_LeadDetail" runat="server" OnClick="lnkExport_LeadDetail_Click">
                            <asp:Image ID="imgExpLeadDetail" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                        </asp:LinkButton>
                    </div>
                    <div class="fright">
                        <asp:ImageButton ID="imgClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgClose_Click" ToolTip="Close" />
                    </div>
                </div>
                <div class="m">
                </div>
                <div id="Div2" runat="server" style="width: 1280px; height: 500px; overflow: auto; padding: 5px">
                    <asp:HiddenField ID="hdnQuotationId" runat="server" />
                    <div align="center">
                        <asp:Label ID="lbError_Popup" runat="server" Visible="true"></asp:Label>
                        <asp:HiddenField ID="hdnMonthId" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnStageId" runat="server" Value="0" />
                    </div>
                    <asp:GridView ID="gvLeadDetail" runat="server" CssClass="table" AutoGenerateColumns="false" DataSourceID="DataSourceLeadDetail" Style="white-space: initial">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Lead Ref No" HeaderText="Lead Ref No" ItemStyle-Width="12%" />
                            <asp:BoundField DataField="Company Name" HeaderText="Company Name" ItemStyle-Width="25%" />
                            <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="25%" />
                            <asp:BoundField DataField="Lead Source" HeaderText="Lead Source" ItemStyle-Width="5%" />
                            <asp:BoundField DataField="Contact Name" HeaderText="Contact Name" ItemStyle-Width="5%" />
                            <asp:BoundField DataField="Email" HeaderText="Email" ItemStyle-Width="5%" />
                            <asp:BoundField DataField="Mobile No" HeaderText="Mobile No" ItemStyle-Width="5%" />
                            <asp:BoundField DataField="RFQ Received" HeaderText="RFQ Received" ItemStyle-Width="5%" />
                            <asp:BoundField DataField="Lead Owner" HeaderText="Lead Owner" ItemStyle-Width="8%" />
                            <asp:BoundField DataField="Lead Created" HeaderText="Lead Created" ItemStyle-Width="5%" />
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>
            <asp:HiddenField ID="lnkDummy" runat="server" Value=""></asp:HiddenField>
            <asp:SqlDataSource ID="DataSourceLeadDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="CRM_rptMonthWiseLeadStatusDetail" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:ControlParameter Name="StatusId" ControlID="hdnStageId" PropertyName="Value" DbType="Int32" DefaultValue="0" />
                    <asp:ControlParameter Name="MonthId" ControlID="hdnMonthId" PropertyName="Value" DbType="Int32" DefaultValue="0" />
                    <asp:ControlParameter ControlID="ddlUser" PropertyName="SelectedValue" Name="UserId" DefaultValue="0" />
                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" DbType="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
            <!-- Status of Lead-->

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

