<%@ Page Title="Location Head Wise Services" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="LocationHead.aspx.cs"
    Inherits="CRMReports_LocationHead" Culture="en-GB" %>

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
            <div align="center" >
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:HiddenField ID="hdnCustId" runat="server" Value="0" />
            </div>
            <fieldset>
                <legend>Product Wise Business Leads Potential</legend>
                <div class="clear">
                    <div class="fleft" style="margin-right: 5px;">
                        Service :
                            <asp:DropDownList ID="ddlService" runat="server" DataSourceID="DataSourceService" DataTextField="sName" DataValueField="ServicesId"
                                AppendDataBoundItems="true" >
                                <asp:ListItem Value="0" Text="-Select-" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                        &nbsp;
                        Service Location:
                            <asp:DropDownList ID="ddlLocation" runat="server" DataSourceID="DataSourceServiceLocation" DataTextField="BranchName" DataValueField="lid"
                                AppendDataBoundItems="true">
                                <asp:ListItem Value="0" Text="-Select-" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                         &nbsp;
                            <asp:DropDownList ID="ddCust" runat="server" AppendDataBoundItems="true">
                                <asp:ListItem Value="0" Text="-Select-" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Registered Customer" ></asp:ListItem>
                                <asp:ListItem Value="2" Text="Non-Registered Customer" ></asp:ListItem>
                            </asp:DropDownList>
                        &nbsp;
                            <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit"/>
                            <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
                                <asp:Image ID="imgExcel" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>
                    </div>
                    <asp:Panel ID="pnlFilter" runat="server" VISIBLE="FALSE">
                        <div class="fleft">
                            <uc1:datafilter id="DataFilter1" runat="server" />
                        </div>
                    </asp:Panel>
                </div>
                <div class="clear">
                </div>
                <asp:GridView ID="gvLocationHead" runat="server" AutoGenerateColumns="true" Width="100%" PagerStyle-CssClass="pgr" DataSourceID="DataSourceLocationHead"
                    AllowPaging="True" AllowSorting="True" CssClass="table" PageSize="20" PagerSettings-Position="TopAndBottom">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>
            <div>
                <%--<asp:SqlDataSource ID="DataSourceLocationHead" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_rptLocationHead" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlSalesPerson" PropertyName="SelectedValue" Name="UserId" DefaultValue="0" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>--%>
                <asp:SqlDataSource ID="DataSourceLocationHead" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_rptLocationHeadByService" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlService" PropertyName="SelectedValue" Name="ServiceId" DefaultValue="0" />
                        <asp:ControlParameter ControlID="ddlLocation" PropertyName="SelectedValue" Name="serviceLocationId" DefaultValue="0" />
                        <asp:ControlParameter ControlID="ddCust" PropertyName="SelectedValue" Name="KycCust" DefaultValue="0" />
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
                 <asp:SqlDataSource ID="DataSourceService" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="BS_GetServicesMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceServiceLocation" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetAllBranch" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <%--CRM_GetAllUsers--%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

