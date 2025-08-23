<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RequestRate.aspx.cs" Inherits="Transport_RequestRate" 
    MasterPageFile="~/MasterPage.master" Title="Transport Request Received" EnableEventValidation="false" Culture="en-GB" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional"
        RenderMode="Inline">
        <ContentTemplate>
    <div align="center">
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
    </div>
    <div class="m clear">
    <fieldset><legend>Pending Rate Detail</legend>
    <div class="m clear">
        <asp:Panel ID="pnlFilter" runat="server">
            <div class="fleft">
                <uc1:DataFilter ID="DataFilter1" runat="server" />
            </div>
        </asp:Panel>
    </div>
    <div class="clear">
    </div>
    
    <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="lId" 
        PagerStyle-CssClass="pgr" AllowPaging="True" AllowSorting="True" Width="100%" PageSize="20" PagerSettings-Position="TopAndBottom"
        OnRowDataBound="gvJobDetail_RowDataBound" OnRowCommand="gvJobDetail_RowCommand" 
        OnPreRender="gvJobDetail_PreRender" DataSourceID="TruckRequestSqlDataSource">
        <Columns>
            <asp:TemplateField HeaderText="Sl">
                <ItemTemplate>
                    <%#Container.DataItemIndex +1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Ref No" SortExpression="JobRefNo">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkRefNo" CommandName="select" runat="server" Text='<%#Eval("TRRefNo") %>'
                        CommandArgument='<%#Eval("lId") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="TRRefNo" HeaderText="Ref No" Visible="false" />
            <asp:BoundField DataField="CustName" HeaderText="Customer" SortExpression="CustName" />
            <%--<asp:BoundField DataField="RequestType" HeaderText="Type"/>--%>
            <asp:BoundField DataField="JobRefNo" HeaderText="Job No"/>
            <asp:BoundField DataField="VehiclePlaced" HeaderText="Placed" SortExpression="VehiclePlaced" />
            <asp:BoundField DataField="DeliveryFrom" HeaderText="Location" SortExpression="DeliveryFrom" />
            <asp:BoundField DataField="DeliveryTo" HeaderText="Destination" SortExpression="DeliveryTo" />
            <asp:BoundField DataField="Packages" HeaderText="Pkg" SortExpression="Packages" 
               ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"/>
            <asp:BoundField DataField="VehicleNo" HeaderText="VehicleNo" SortExpression="VehicleNo" />
            <asp:BoundField DataField="DeliveryType" HeaderText="Delivery Type" SortExpression="DeliveryType" />
            <asp:BoundField DataField="DispatchDate" HeaderText="Dispatch Date" SortExpression="DispatchDate" DataFormatString="{0:dd/MM/yyyy}"/>
            
        </Columns>
        <PagerTemplate>
            <asp:GridViewPager ID="GridViewPager1" runat="server" />
        </PagerTemplate>
    </asp:GridView>
    </fieldset>
    <div>
        <asp:SqlDataSource ID="TruckRequestSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetRateRequest" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
