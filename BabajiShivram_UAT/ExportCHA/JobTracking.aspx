<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JobTracking.aspx.cs" MasterPageFile="~/MasterPage.master"
    Inherits="JobTracking" EnableEventValidation="false" Title="Shipment Tracking" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../Images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional"
        RenderMode="Inline">
        <ContentTemplate>
            <div align="center" style="vertical-align: top">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div class="clear"></div>
            <div>
                <fieldset class="fieldset-AutoWidth">
                    <legend>Shipment Tracking</legend>
                    <div class="clear"></div>
                    <div>
                        <asp:Panel ID="pnlFilter" runat="server">
                            <div class="fleft">
                                <uc1:DataFilter ID="DataFilter1" runat="server" />

                            </div>
                            <div class="fleft" style="margin-left: 30px; padding-top: 3px;">
                                <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                            <asp:Image ID="Image1" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
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
                                            CommandArgument='<%#Eval("JobId")+ ","+ Eval("StatusId") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" SortExpression="JobRefNo" Visible="false" />
                                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                                
                                <%--<asp:TemplateField HeaderText="Status" SortExpression="Status">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkStatus" CommandName="StatusRedirect" CommandArgument='<%#Eval("JobId")+ ","+ Eval("StatusId") %>'
                                            runat="server" Text='<%#Eval("Status") %>' ToolTip='<%#Eval("DailyProgress") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                
                                <asp:BoundField DataField="CustRefNo" HeaderText="Cust Ref No" SortExpression="CustRefNo" />
                                <asp:BoundField DataField="CustName" HeaderText="Customer Name" SortExpression="CustName" />
                                <asp:BoundField DataField="Shipper" HeaderText="Shipper" SortExpression="Shipper" />
                                <asp:BoundField DataField="Consignee" HeaderText="Consignee" SortExpression="Consignee" />
                                <asp:BoundField DataField="Port" HeaderText="Port" SortExpression="Port" />
                                <asp:BoundField DataField="Mode" HeaderText="Mode" SortExpression="Mode" />
                                <asp:BoundField DataField="SBNo" HeaderText="SB No" SortExpression="SBNo" />
                                <asp:BoundField DataField="SBDate" HeaderText="SB Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="SBDate" />
                                <asp:BoundField DataField="MAWBNo" HeaderText="MBL No" SortExpression="MAWBNo" />
                                <asp:BoundField DataField="HAWBNo" HeaderText="HBL No" SortExpression="HAWBNo" />
                                <asp:BoundField DataField="NoOfPackages" HeaderText="Pkgs" SortExpression="NoOfPackages" />
                                <asp:BoundField DataField="GrossWT" HeaderText="Gross Weight" SortExpression="GrossWT" />
                                <asp:BoundField DataField="ChkPreDate" HeaderText="Checklist Prepared Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ChkPreDate" />
                                <asp:BoundField DataField="ChecklistPreparedBy" HeaderText="Checklist Prepared By" SortExpression="ChecklistPreparedBy" />
                                <asp:BoundField DataField="jobCreateDate" HeaderText="Job Created Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="jobCreateDate" />
                                <asp:BoundField DataField="jobCreateBy" HeaderText="Job Created By" SortExpression="jobCreateBy" />
                                <asp:BoundField DataField="ShippingLineDate" HeaderText="Document Hand Over To Shipping Line Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ShippingLineDate" />
                                <asp:BoundField DataField="LEODate" HeaderText="Supretendent LEO Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LEODate" />
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
                    SelectCommand="EX_GetJobDetailForExportCHA" SelectCommandType="StoredProcedure"
                    DataSourceMode="DataSet">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
