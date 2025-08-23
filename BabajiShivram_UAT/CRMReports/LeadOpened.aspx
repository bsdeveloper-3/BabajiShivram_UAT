<%@ Page Title="New Leads Opened" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="LeadOpened.aspx.cs"
    Inherits="CRMReports_LeadOpened" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
    </asp:ScriptManager>
    <style type="text/css">
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
                <asp:HiddenField ID="hdnCustId" runat="server" Value="0" />
            </div>
            <div class="clear">
                <asp:DropDownList ID="ddlMonth" runat="server" TabIndex="1" Width="250px" ToolTip="Filter monthly user data" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                    <asp:ListItem Text="Financial Year" Selected="True" Value="0"></asp:ListItem>
                    <asp:ListItem Text="January" Value="1"></asp:ListItem>
                    <asp:ListItem Text="February" Value="2"></asp:ListItem>
                    <asp:ListItem Text="March" Value="3"></asp:ListItem>
                    <asp:ListItem Text="April" Value="4"></asp:ListItem>
                    <asp:ListItem Text="May" Value="5"></asp:ListItem>
                    <asp:ListItem Text="June" Value="6"></asp:ListItem>
                    <asp:ListItem Text="July" Value="7"></asp:ListItem>
                    <asp:ListItem Text="August" Value="8"></asp:ListItem>
                    <asp:ListItem Text="September" Value="9"></asp:ListItem>
                    <asp:ListItem Text="October" Value="10"></asp:ListItem>
                    <asp:ListItem Text="November" Value="11"></asp:ListItem>
                    <asp:ListItem Text="December" Value="12"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <fieldset>
                <legend>Leads Opened</legend>
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
                    DataKeyNames="lid" AllowPaging="True" AllowSorting="True" CssClass="table" PageSize="20" PagerSettings-Position="TopAndBottom">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="LeadRefNo" HeaderText="Lead Ref No" SortExpression="LeadRefNo" ReadOnly="true" />
                        <asp:BoundField DataField="CompanyName" HeaderText="Company" SortExpression="CompanyName" ReadOnly="true" />
                        <asp:BoundField DataField="LeadStageName" HeaderText="Current Status" SortExpression="LeadStageName" ReadOnly="true" />
                        <asp:BoundField DataField="LeadSourceName" HeaderText="Source" SortExpression="LeadSourceName" ReadOnly="true" />
                        <asp:BoundField DataField="ContactName" HeaderText="Contact Name" SortExpression="ContactName" ReadOnly="true" />
                        <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" ReadOnly="true" />
                        <asp:BoundField DataField="MobileNo" HeaderText="Mobile No" SortExpression="MobileNo" ReadOnly="true" />
                        <asp:BoundField DataField="RfqReceived" HeaderText="RFQ Received" SortExpression="RfqReceived" ReadOnly="true" />
                        <asp:BoundField DataField="CreatedBy" HeaderText="Lead Owner" SortExpression="CreatedBy" ReadOnly="true" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LeadDate" ReadOnly="true" />
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="DataSourceLeads" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_GetDashboardLeads" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlMonth" PropertyName="SelectedValue" Name="MonthId" DefaultValue="0" />
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        <asp:ControlParameter ControlID="hdnCustId" PropertyName="Value" Name="CompanyId" DefaultValue="0" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


