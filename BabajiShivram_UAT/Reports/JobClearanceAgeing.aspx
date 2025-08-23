<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JobClearanceAgeing.aspx.cs" EnableEventValidation="false" 
 MasterPageFile="~/MasterPage.master" Inherits="Reports_JobClearanceAgeing" Title="Job Clearance Aging Detail" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
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
        <asp:Label ID="lblMessage" runat="server" CssClass="info" Visible="false"></asp:Label>
    </div>
    <fieldset><legend>Job Clearance Aging Report</legend>   
    <div class="clear">
            
            <div class="fleft">
                <asp:LinkButton ID="lnkPortJobXls" runat="server" OnClick="lnkPortJobXls_Click">
                <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
            </div>
        
    </div>
    <div class="clear">
    </div>
    
    <asp:GridView ID="gvJobDetailAgeing" runat="server" AutoGenerateColumns="True" CssClass="table" AllowSorting="true" 
            AllowPaging="True" PagerStyle-CssClass="pgr" PageSize="20" PagerSettings-Position="TopAndBottom" 
            Width="100%" DataSourceID="JobDetailAgeingSqlDataSource" OnPreRender="gvJobDetailAgeing_PreRender">
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
        <asp:SqlDataSource ID="JobDetailAgeingSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            DataSourceMode="DataSet" EnableCaching="true" CacheDuration="600" SelectCommand="ds_BSAgeingDaysDetails" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="RangeLow" Type="int32" SessionField="MISRangeLow"/>
                <asp:SessionParameter Name="RangeHigh" Type="int32" SessionField="MISRangeHigh"/>
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                <asp:SessionParameter Name="BranchId" SessionField="PendingBranchId" />
                <asp:SessionParameter Name="CustId" SessionField="PendingCustId" />
                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>