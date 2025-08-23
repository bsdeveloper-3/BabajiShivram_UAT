<%@ Page Title="Pending Job" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
        CodeFile="PendingExportJob.aspx.cs" Inherits="ExportCHA_PendingExportJob" Culture="en-GB"%>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingJob" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPendingJob" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
        <div align="center">
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
        </div>
        <div class="clear"></div>
        <fieldset><legend>Job Creation</legend>
        <div class="clear">
        <asp:Panel ID="pnlFilter" runat="server">
            <div class="fleft"><uc1:DataFilter ID="DataFilter1" runat="server" /></div>
            <div class="fleft" style="margin-left:30px; padding-top:3px;">
                <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click" CssClass="fright">
                    <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
            </div>
        </asp:Panel>
        </div>
        <div class="clear"></div>            
        <asp:GridView ID="gvPreAlert" runat="server" AutoGenerateColumns="False" DataKeyNames="JobId" CssClass="table"
            PagerStyle-CssClass="pgr" PageSize="20" AllowPaging="True" AllowSorting="True" PagerSettings-Position="TopAndBottom"
            DataSourceID="GridviewSqlDataSource" OnRowCommand="gvPreAlert_RowCommand" 
            OnRowDataBound="gvPreAlert_RowDataBound" OnPreRender="gvPreAlert_PreRender">
            <Columns>
            <asp:TemplateField HeaderText="Sl">
                <ItemTemplate>
                    <%#Container.DataItemIndex +1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Create Job">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkCreate" CommandName="select" runat="server" Text="Create Job"
                        CommandArgument='<%#Eval("JobId") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="JobRefNo" HeaderText="Job No" SortExpression="JobRefNo" />
            <asp:BoundField DataField="ShipperName" HeaderText="Shipper" SortExpression="ShipperName" />
            <asp:BoundField DataField="PortOfLoading" HeaderText="Port" SortExpression="PortOfLoading" />
            <asp:BoundField DataField="TransMode" HeaderText="Mode" SortExpression="TransMode" />
            <asp:BoundField DataField="BuyerName" HeaderText="Buyer Name" SortExpression="BuyerName"/>
            <asp:BoundField DataField="PreAlertDate" HeaderText="PreAlert Date" SortExpression="PreAlertDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
            <asp:TemplateField HeaderText="Aging" SortExpression="PreAlertAgeing" >
                <ItemTemplate>
                    <asp:Label ID="lblAging" runat="server" Text='<%#Eval("PreAlertAgeing") %>' ToolTip="Today – PreAlert Date"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            </Columns>
            <PagerTemplate>
                <asp:GridViewPager runat="server" />
            </PagerTemplate>
        </asp:GridView>
        </fieldset>
        <div>
        <asp:SqlDataSource ID="GridviewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="EX_GetPendingJobForUser" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
            </SelectParameters>
        </asp:SqlDataSource>
        </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

