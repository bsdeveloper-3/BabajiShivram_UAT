<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MISBranchDetail.aspx.cs" 
    Inherits="ExportReports_MISBranchDetail" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" EnablePartialRendering="true" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblMessage" runat="server" CssClass="errorMsg" Visible="false"></asp:Label>
            </div>
            <div class="clear"></div>
            <fieldset>
                <legend>MIS Port</legend>
                <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
            <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
                <div class="clear">
                </div>
                <asp:GridView ID="gvBranchJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table" AllowSorting="true"
                    AllowPaging="True" PagerStyle-CssClass="pgr" PageSize="20" PagerSettings-Position="TopAndBottom"
                    Width="100%" DataSourceID="BranchJobDetailSqlDataSource" OnPreRender="gvBranchJobDetail_PreRender">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField DataField="Port" HeaderText="Port" SortExpression="Port" />--%>
                        <asp:BoundField DataField="BS Job No" HeaderText="BS Job No" SortExpression="BS Job No" />
                        <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                        <%--<asp:BoundField DataField="Consignee" HeaderText="Consignee" SortExpression="Consignee"/>--%>
                        <asp:BoundField DataField="Mode" HeaderText="Mode" SortExpression="Mode" />
                        <asp:BoundField DataField="NoOfPackages" HeaderText="Pkgs" SortExpression="NoOfPackages" />
                        <asp:BoundField DataField="GrossWt" HeaderText="Gross Weight" SortExpression="GrossWt" />
                        <asp:BoundField DataField="Count40" HeaderText="40 Con" SortExpression="Count40" />
                        <asp:BoundField DataField="Count20" HeaderText="20 Con" SortExpression="Count20" />
                        <asp:BoundField DataField="CountLCL" HeaderText="LCL" SortExpression="CountLCL" />
                        <asp:BoundField DataField="Job Created By" HeaderText="Job Created By" SortExpression="Job Created By" />
                        <asp:BoundField DataField="Job Date" HeaderText="Job Date" SortExpression="Job Date" DataFormatString="{0:dd/MM/yyyy}" />
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="BranchJobDetailSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="EX_rptBranchWsJobDetail" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="BabajiBranchId" SessionField="PendingBranchId" />
                        <asp:SessionParameter Name="BranchId" SessionField="PendingBranchId" ConvertEmptyStringToNull="true" DefaultValue="0" />
                        <asp:SessionParameter Name="CustId" SessionField="PendingCustId" ConvertEmptyStringToNull="true" DefaultValue="0" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

