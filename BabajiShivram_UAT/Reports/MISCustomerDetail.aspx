<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MISCustomerDetail.aspx.cs" Inherits="Reports_MISCustomerDetail"
 MasterPageFile="~/MasterPage.master" Title="MIS Customer Jobs" EnableEventValidation="false" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    <div align="center">
        <asp:Label ID="lblMessage" runat="server" CssClass="info" Visible="false"></asp:Label>
    </div>
    <div class="clear"></div>
        <fieldset>
        <legend>MIS</legend>    
        <asp:LinkButton ID="lnkPortJobXls" runat="server" OnClick="lnkPortJobXls_Click">
        <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
        </asp:LinkButton>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;<asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" />
    <div class="clear">
    </div>
    <asp:GridView ID="gvCustomerJobDetail" runat="server" AutoGenerateColumns="True" CssClass="table"  AllowPaging="True"
        PagerStyle-CssClass="pgr" Width="100%" PageSize="20" PagerSettings-Position="TopAndBottom" 
            DataSourceID="CustomerJobDetailSqlDataSource" OnPreRender="gvCustomerJobDetail_PreRender">
        <Columns>
            <asp:TemplateField HeaderText="Sl">
                <ItemTemplate>
                    <%#Container.DataItemIndex +1 %>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerTemplate>
            <asp:GridViewPager runat="server" />
        </PagerTemplate>
    </asp:GridView>
   </fieldset>
    <div>
        <asp:SqlDataSource ID="CustomerJobDetailSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="rptMISCustomerwiseDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="CustomerId" SessionField="MISCustomerId" />
                <asp:SessionParameter Name="TransMode" SessionField="MISTransMode" />
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    
</asp:Content>