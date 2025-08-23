<%@ Page Title="Job tracking" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="JobTracking.aspx.cs" Inherits="ContMovement_JobTracking" EnableEventValidation="false" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlCMJobTracking" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upnlCMJobTracking" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center" style="vertical-align: top">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div class="clear"></div>
            <div>
                <fieldset class="fieldset-AutoWidth">
                    <legend>Job Tracking</legend>
                    <div class="clear"></div>
                    <div>
                        <asp:Panel ID="pnlFilter" runat="server">
                            <div class="fleft">
                                <uc1:DataFilter ID="DataFilter1" runat="server" />
                            </div>
                            <div class="fleft" style="margin-left: 5px; padding-top: 3px;">
                                <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                                    <asp:Image ID="imgExport" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
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
                                            CommandArgument='<%#Eval("JobId") + ";" + Eval("JobRefNo")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" Visible="false" />
                                <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                                <asp:BoundField DataField="Division" HeaderText="Division" SortExpression="Division" />
                                <asp:BoundField DataField="Plant" HeaderText="Plant" SortExpression="Plant" />
                                <asp:BoundField DataField="JobCreationDate" HeaderText="Job Creation Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="JobCreationDate" />
                                <asp:BoundField DataField="JobCreatedBy" HeaderText="Job Created By" SortExpression="JobCreatedBy" />
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
                    SelectCommand="CM_GetJobtracking" SelectCommandType="StoredProcedure" DataSourceMode="DataSet" EnableCaching="true"
                    CacheDuration="300" CacheKeyDependency="MyCacheDependency">
                    <SelectParameters>
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

