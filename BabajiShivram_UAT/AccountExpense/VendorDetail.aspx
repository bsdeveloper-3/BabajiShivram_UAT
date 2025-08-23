<%@ Page Title="FA- Vendor Details" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="VendorDetail.aspx.cs" Inherits="AccountExpense_VendorDetail" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPendingchecklist" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <div>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div class="clear"></div>
            <div>
                <fieldset class="fieldset-AutoWidth">
                    <legend>FA- Vendor Details</legend>
                    <div class="clear"></div>
                    <div>
                        <asp:Panel ID="pnlFilter" runat="server">
                            <div class="fleft">
                                <uc1:datafilter id="DataFilter1" runat="server" />
                            </div>
                            <div class="fleft" style="margin-left: 30px; padding-top: 3px;">
                                <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="images/Excel.jpg" ToolTip="Export To Excel" />
                                </asp:LinkButton>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="clear">
                    </div>
                    <div>
                        <asp:GridView ID="gvVendorDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                            PagerStyle-CssClass="pgr" OnPreRender="gvVendorDetail_PreRender" OnRowCommand="gvVendorDetail_RowCommand"
                            DataKeyNames="lId" DataSourceID="VendorSqlDataSource" AllowPaging="True" AllowSorting="True"
                            PagerSettings-Position="TopAndBottom" PageSize="20" Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Vendor Name" SortExpression="sName">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkVendorName" CommandName="select" runat="server" Text='<%#Eval("sName") %>'
                                            CommandArgument='<%#Eval("lid") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="sName" HeaderText="Vendor Name" Visible="false" />
                                <asp:BoundField DataField="sCode" HeaderText="Vendor Code" />
                                <asp:BoundField DataField="StateName" HeaderText="State" SortExpression="StateName" />
                                <asp:BoundField DataField="GSTIN" HeaderText="GST No" SortExpression="GSTIN" />
                                <asp:BoundField DataField="PanNo" HeaderText="Pan No" SortExpression="PanNo" />
                                <asp:BoundField DataField="CreditDays" HeaderText="Credit Days" SortExpression="CreditDays" />
                            </Columns>
                            <PagerTemplate>
                                <asp:GridViewPager runat="server" />
                            </PagerTemplate>
                        </asp:GridView>
                    </div>
                </fieldset>
            </div>
            <div>
                <asp:SqlDataSource ID="VendorSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="BJV_GetVendorMS" SelectCommandType="StoredProcedure"
                    DataSourceMode="DataSet" EnableCaching="true" CacheDuration="300" CacheKeyDependency="MyCacheDependency">
                </asp:SqlDataSource>
            </div>
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


