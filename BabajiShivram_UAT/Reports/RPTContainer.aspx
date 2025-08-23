<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RPTContainer.aspx.cs" Inherits="Reports_RPTContainer" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1"/>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="updPanel" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="updPanel" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center" style="vertical-align: top">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div class="clear"></div>
            <div>
            <fieldset class="fieldset-AutoWidth"><legend>Container Report</legend>
            <div class="clear">
                <asp:Panel ID="pnlFilter" runat="server">
                    <div class="fleft">
                        <uc1:DataFilter ID="DataFilter1" runat="server" />
                    </div>
                    <div class="fleft" style="margin-left:20px; padding-top:3px;">
                        <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                            <asp:Image ID="Image1" runat="server" ImageUrl="images/Excel.jpg" ToolTip="Export To Excel" />
                        </asp:LinkButton>
                    </div>
                </asp:Panel>
            </div>
            <div class="clear">
            </div>
            <div>
                <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                    PagerStyle-CssClass="pgr" OnPreRender="gvJobDetail_PreRender" OnRowCommand="gvJobDetail_RowCommand"
                    DataKeyNames="JobId" DataSourceID="JobDetailSqlDataSource" AllowPaging="True" AllowSorting="True" 
                    PagerSettings-Position="TopAndBottom" PageSize="20" Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="BS Job No" SortExpression="JobRefNo">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkJobRefNo" CommandName="select" runat="server" Text='<%#Eval("JobRefNo") %>'
                                    CommandArgument='<%#Eval("JobId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" Visible="false" />
                        <%--<asp:BoundField DataField="CurrentStatus" HeaderText="Status" Visible="false" />--%>
                       <%-- <asp:TemplateField HeaderText="Status" SortExpression="CurrentStatus">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkStatus" CommandName="StatusRedirect" CommandArgument='<%#Eval("JobId") %>' runat="server" Text='<%#Eval("CurrentStatus") %>' ToolTip='<%#Eval("DailyProgress") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
			            <%--<asp:BoundField DataField="JobSubStatus" HeaderText="Status" SortExpression="JobSubStatus" />--%>
                        <asp:BoundField DataField="CustRefNo" HeaderText="Cust Ref No" SortExpression="CustRefNo" />
                        <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                        <asp:BoundField DataField="Consignee" HeaderText="Consignee" SortExpression="Consignee" />
                        <asp:BoundField DataField="Port" HeaderText="Port" SortExpression="Port" />
                        <asp:BoundField DataField="Mode" HeaderText="Mode" SortExpression="Mode" /> 
                        <asp:BoundField DataField="ContainerNo" HeaderText="Container No" SortExpression="ContainerNo" />
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </div>
            </fieldset>
            </div>
            <div>
                <asp:SqlDataSource ID="JobDetailSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetJobDetailForCont" SelectCommandType="StoredProcedure" 
			        DataSourceMode="DataSet" EnableCaching="true" CacheDuration="300" CacheKeyDependency="MyCacheDependency">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
