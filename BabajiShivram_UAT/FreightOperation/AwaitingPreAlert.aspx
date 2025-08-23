<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AwaitingPreAlert.aspx.cs" 
    Inherits="FreightOperation_AwaitingPreAlert" Title="Awaiting Agent PreAlert" Culture="en-GB"%>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="updateOperation" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="updateOperation" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
    <fieldset><legend>Agent Pre-Alert</legend>
        <div class="m clear">
        <div class="fleft">
            <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
            </asp:LinkButton>
            &nbsp;
        </div>
        <div class="fleft">
            <uc1:DataFilter ID="DataFilter1" runat="server" />
        </div>
    </div>
    <div class="clear">
    </div>
    <div>
        <asp:GridView ID="gvFreight" runat="server" DataSourceID="GridViewSqlDataSource" DataKeyNames="EnqId" Width="100%" 
            AllowPaging="True" PageSize="20" AllowSorting="true" PagerStyle-CssClass="pgr" PagerSettings-Position="TopAndBottom"
            CssClass="table" AutoGenerateColumns="False" OnRowCommand="gvFreight_RowCommand" OnPreRender="gvFreight_PreRender">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Job No" SortExpression="FRJobNo">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkViewFreight" runat="Server" Text='<%#Eval("FRJobNo") %>' CommandName="navigate"
                         CommandArgument='<%#Eval("EnqId") +";"+ Eval("ModeName")%>' CausesValidation="false" ></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="FRJobNo" HeaderText="Job No" Visible="false"/>
                <asp:BoundField DataField="ENQRefNo" HeaderText="Enquiry No" SortExpression="ENQRefNo"/>
                <asp:BoundField DataField="EnquiryUser" HeaderText="Freight SPC" SortExpression="EnquiryUser"/>
                <asp:BoundField DataField="ETA" HeaderText="ETA" SortExpression="ETA" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField DataField="HBLNo" HeaderText="HBL" SortExpression="HBLNo"/>
                <asp:BoundField DataField="AgentName" HeaderText="Agent" SortExpression="AgentName"/>
                <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer"/>
                <asp:BoundField DataField="Consignee" HeaderText="Consignee" SortExpression="Consignee"/>
                <asp:BoundField DataField="ModeName" HeaderText="Mode" SortExpression="ModeName"/>
                <asp:BoundField DataField="CountOf20" HeaderText="20" SortExpression="CountOf20"/>
                <asp:BoundField DataField="CountOf40" HeaderText="40" SortExpression="CountOf40"/>
                <asp:BoundField DataField="LCLVolume" HeaderText="LCL(CBM)" SortExpression="LCLVolume"/>
                <asp:BoundField DataField="NoOfPackages" HeaderText="Pkgs" SortExpression="NoOfPackages"/>
                <asp:BoundField DataField="GrossWeight" HeaderText="Gross WT" SortExpression="GrossWeight"/>
                <%--<asp:BoundField DataField="TermsName" HeaderText="Terms" SortExpression="TermsName"/>--%>
                <%--<asp:BoundField DataField="TypeName" HeaderText="Type" SortExpression="TypeName"/>--%>
                <%--<asp:BoundField DataField="CountryName" HeaderText="Country" SortExpression="CountryName"/>--%>
                <asp:BoundField DataField="LoadingPortName" HeaderText="Loading Port" SortExpression="LoadingPortName"/>
                <asp:BoundField DataField="PortOfDischargedName" HeaderText="Port of Discharged" SortExpression="PortOfDischargedName"/>
            </Columns>
            <PagerTemplate>
                <asp:GridViewPager ID="GridViewPager1" runat="server" />
            </PagerTemplate>
        </asp:GridView>
        <asp:SqlDataSource ID="GridViewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="FOP_GetAwaitingAgentPreAlert" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    </fieldset>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
