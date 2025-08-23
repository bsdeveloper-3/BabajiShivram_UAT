<%@ Page Title="Inventory Stock History" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="StockHistory.aspx.cs" 
Inherits="Inventory_StockHistory"  Culture="en-GB" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<fieldset><legend>  Stock Inward Details  </legend>
 <!-- Filter Content Start-->
        <div class="fleft">
        <uc1:DataFilter ID="DataFilter1" runat="server" />
        </div>
    <!-- Filter Content END-->
            <div>
                <asp:LinkButton ID="lnkIMSXls" runat="server" OnClick="lnkIMSXls_Click">
                <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
            </asp:LinkButton>
            </div>
    <div class="clear" ></div>
      <asp:GridView ID="GrvStockHistory" runat="server" AutoGenerateColumns="False"
            DataSourceID="DataSorceStorckHistory"  PagerStyle-CssClass="pgr"
            CssClass="table" CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:BoundField DataField="CategoryName" HeaderText="Category" SortExpression="CategoryName" />
                 <asp:BoundField DataField="ItemName" HeaderText="Item Type" SortExpression="ItemName" />
                 <asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity" />
                 <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" SortExpression="VendorName" />
                 <asp:BoundField DataField="BillNO" HeaderText="Bill NO" SortExpression="BillNO" />
                 <asp:BoundField DataField="BillAmount" HeaderText="Bill Amount" SortExpression="BillAmount" />
                 <asp:BoundField DataField="BillDate" HeaderText="Bill Date" SortExpression="BillDate"  DataFormatString="{0:dd/MM/yyyy}" />
                 <asp:BoundField DataField="dtDate" HeaderText="Stock Entry Date" SortExpression="dtDate" DataFormatString="{0:dd/MM/yyyy hh:mm tt}"/>
            </Columns>
        </asp:GridView>
   </fieldset>
    <asp:SqlDataSource ID="DataSorceStorckHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
        SelectCommand="IMS_getStockHistory" SelectCommandType="StoredProcedure" >
        <SelectParameters>
            <asp:SessionParameter Name="UserId" SessionField="UserId" />
            <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

