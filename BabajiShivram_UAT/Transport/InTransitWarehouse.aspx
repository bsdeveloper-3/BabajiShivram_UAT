<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InTransitWarehouse.aspx.cs" Inherits="InTransitWarehouse" 
    MasterPageFile="~/MasterPage.master" Title="Trans Warehouse Delivery" EnableEventValidation="false" Culture="en-GB"%>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div align="center">
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
    </div>
    <div class="clear">
    </div>
    <fieldset>
        <legend>Trans Warehouse Delivery</legend>
    <div class="clear">
        <asp:Panel ID="pnlFilter" runat="server">
            <div class="fleft">
                <uc1:DataFilter ID="DataFilter1" runat="server" />
            </div>
            <div class="fleft" style="margin-left:30px; padding-top:3px;">
                <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                    <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
            </div>
        </asp:Panel>
    </div>
    <div class="clear">
    </div>
  
    <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
        PagerStyle-CssClass="pgr" DataKeyNames="JobId" AllowPaging="True" AllowSorting="True" Width="100%"
        PageSize="20" PagerSettings-Position="TopAndBottom" 
        OnPreRender="gvJobDetail_PreRender" DataSourceID="TransitWarehouseSqlDataSource" OnRowCommand="gvJobDetail_RowCommad">
        <Columns>
            <asp:TemplateField HeaderText="Sl">
                <ItemTemplate>
                    <%#Container.DataItemIndex +1 %>
                </ItemTemplate>
            </asp:TemplateField>
           
            <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" SortExpression="JobRefNo"   />
         
	<%--<asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />--%>
            <asp:BoundField DataField="Consignee" HeaderText="Consignee" SortExpression="Consignee" />
            <asp:BoundField DataField="Mode" HeaderText="Mode" SortExpression="Mode" />
            <asp:BoundField DataField ="Port" HeaderText="Port" SortExpression="Port" />
            <asp:BoundField DataField="ExamineDate" HeaderText="Examine Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ExamineDate" />
            <asp:BoundField DataField="OutOfChargeDate" HeaderText="Out Of Charge Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="OutOfChargeDate" />
            <asp:BoundField DataField ="WarehouseName" HeaderText="Warehouse" SortExpression="WarehouseName" />
            <asp:BoundField DataField="TransportationByBabaji" HeaderText="Transport By" SortExpression="TransportationByBabaji" />
            <asp:BoundField DataField="DeliveryDestination" HeaderText="Delivery Destination" SortExpression="DeliveryDestination" />
            
            <asp:TemplateField HeaderText="Move To Customer Place">
                 <ItemTemplate>
                 <asp:LinkButton ID="lnkMove" Text="Update Dispatch Details" CommandName="Move" CommandArgument='<%#Eval("JobId") %>' runat="server"></asp:LinkButton>
                 <%--<asp:LinkButton ID="lnkMove" Text="Update Dispatch Details" CommandName="Move" OnClientClick="return confirm('Are you sure wants to Move the Job ?');" CommandArgument='<%#Eval("JobId") %>' runat="server"></asp:LinkButton>--%>
                 </ItemTemplate>
            </asp:TemplateField>
                     
        </Columns>
        <PagerTemplate>
            <asp:GridViewPager ID="GridViewPager1" runat="server" />
        </PagerTemplate>
    </asp:GridView>
    </fieldset>
    <div>
        <asp:SqlDataSource ID="TransitWarehouseSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="GetTransitWarehouseJobForUser" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
</asp:Content>

