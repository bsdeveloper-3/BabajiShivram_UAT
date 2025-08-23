<%@ Page Title="OOC Completed-Pending Dispatch" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PendingDispatchReport.aspx.cs" Inherits="Reports_PendingDispatchReport" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:scriptmanager runat="server" id="ScriptManager1" />
    <div>
        <asp:updateprogress id="updProgress" associatedupdatepanelid="upPendingchecklist" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:updateprogress>
    </div>
    <asp:updatepanel id="upPendingchecklist" runat="server" updatemode="Conditional"
        rendermode="Inline">
        <ContentTemplate>
            <div align="center" style="vertical-align: top">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div class="clear"></div>
            <div>
                <fieldset class="fieldset-AutoWidth">
                    <legend>OOC Completed - Pending Dispatch</legend>
                    <div class="clear"></div>
                    <div>
                        <asp:Panel ID="pnlFilter" runat="server">
                            <div class="fleft">
                                <uc1:datafilter id="DataFilter1" runat="server" />
                            </div>
                            <div class="fleft" style="margin-left: 30px; padding-top: 3px;">
                                <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                                </asp:LinkButton>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="clear">
                    </div>
                    <div>
                        <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                            PagerStyle-CssClass="pgr" OnPreRender="gvJobDetail_PreRender" 
                            DataKeyNames="lId" DataSourceID="JobDetailSqlDataSource" AllowPaging="True" AllowSorting="True"
                            PagerSettings-Position="TopAndBottom" PageSize="20" Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" />
                                <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                                <asp:BoundField DataField="ETA" HeaderText="ETA" SortExpression="ETA" DataFormatString="{0:dd/MM/yyyy}"/>
                                <asp:BoundField DataField="BOENo" HeaderText="BOE No" SortExpression="BOENo" />
                                <asp:BoundField DataField="BOEDate" HeaderText="BOE Date" SortExpression="BOEDate" DataFormatString="{0:dd/MM/yyyy}"/>
                                <asp:BoundField DataField="OOCDate" HeaderText="OOC Date" SortExpression="OOCDate" DataFormatString="{0:dd/MM/yyyy}"/>
                                <asp:BoundField DataField="BranchName" HeaderText="Branch Name" SortExpression="BranchName" />
                                <asp:BoundField DataField="sName" HeaderText="KAM Name" SortExpression="sName" />
                                <asp:BoundField DataField="StatusName" HeaderText="Job Activity Status" SortExpression="StatusName" />
                               
                                <%--<asp:BoundField DataField="DailyProgress" HeaderText="Job Activity Remarks" SortExpression="DailyProgress" />--%>
                                    <%--<asp:Label ID="lblRemark" runat="server" Text='<%#Bind("DailyProgress") %>'></asp:Label>--%>
                                    <asp:TemplateField HeaderText="Job Activity Remarks">
                                    <ItemTemplate>
                                         <div style="word-wrap: break-word; width: 400px; white-space:normal;">
                                      <asp:Label ID="lblRemark" runat="server" Text='<%#Bind("DailyProgress") %>'></asp:Label>--%>
                                             </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:BoundField DataField="BETypeName" HeaderText="BOE Type" SortExpression="BETypeName" />

                                <asp:BoundField DataField="Count20" HeaderText="Count20" SortExpression="Count20" />
                                <asp:BoundField DataField="Count40" HeaderText="Count40" SortExpression="Count40" />
                                <asp:BoundField DataField="CountLCL" HeaderText="CountLCL" SortExpression="LCL" ReadOnly="true"/>
                                <asp:BoundField DataField="NoOfPackages" HeaderText="No Of Packages" SortExpression="Count40" />
                                <asp:BoundField DataField="GrossWT" HeaderText="Gross Weight" SortExpression="Count20" />
                                
                                
                                <asp:BoundField DataField="DispatchDate" HeaderText="Dispatch Date" SortExpression="DispatchDate" DataFormatString="{0:dd/MM/yyyy}"/>
                                <asp:BoundField DataField="BALContainer" HeaderText="BAL Container" SortExpression="BALContainer" />
                                <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />
                                <asp:BoundField DataField="AGING" HeaderText="AGING" SortExpression="AGING" />
                                
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
                    SelectCommand="rptSPendingDispatch" SelectCommandType="StoredProcedure"
                    DataSourceMode="DataSet" EnableCaching="true" CacheDuration="45" CacheKeyDependency="MyCacheDependency">
                </asp:SqlDataSource>
            </div>
            
        </ContentTemplate>
    </asp:updatepanel>
</asp:Content>

