<%@ Page Title="SKF Pre-Alert" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="SKF_PreAlertDoc.aspx.cs" Inherits="Reports_SKF_PreAlertDoc" EnableEventValidation="false" Culture="en-GB" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
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
    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div class="clear"></div>
            <fieldset><legend>SKF PreAlert Document</legend>
            <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:datafilter id="DataFilter1" runat="server" />
                        </div>
                        <div class="fleft" style="margin-left: 30px; padding-top: 3px;">
                            <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
                                <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
            <div class="clear"></div>
                <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="false" CssClass="table"
                    AllowPaging="True" AllowSorting="True" Width="100%" PageSize="20" PagerSettings-Position="TopAndBottom"
                    OnRowCommand="gvReport_RowCommand" DataSourceID="DataSourceReport">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DocumentID" SortExpression="DocumentID">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkView" CommandName="view" runat="server" Text='<%#Eval("DocumentID") %>' CommandArgument='<%#Eval("lid") %>' ToolTip="Export Invoice Detail" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="DocumentID" HeaderText="DocumentID" SortExpression="DocumentID" Visible="false" />
                        <asp:BoundField DataField="DocumentDateTime" HeaderText="DocumentDateTime" SortExpression="DocumentDateTime" />
                        <asp:BoundField DataField="InvoiceReferenceNo" HeaderText="InvoiceNo" SortExpression="InvoiceReferenceNo" />
                        <asp:BoundField DataField="InvoiceReferenceDate" HeaderText="InvoiceDate" SortExpression="InvoiceReferenceDate" DataFormatString="{0:dd/MM/yyyy}"/>
                        <asp:BoundField DataField="SupplierPartyName" HeaderText="SupplierPartyName" SortExpression="SupplierPartyName" />
                        <asp:BoundField DataField="ExporterPartyName" HeaderText="ExporterPartyName" SortExpression="ExporterPartyName" />
                        <asp:BoundField DataField="ImporterPartyName" HeaderText="ImporterPartyName" SortExpression="ImporterPartyName" />
                        <asp:BoundField DataField="ShipToPartyName" HeaderText="ShipToPartyName" SortExpression="ShipToPartyName" />
                        <asp:BoundField DataField="IncotermsCode" HeaderText="Incoterms" SortExpression="IncotermsCode" />
                        <asp:BoundField DataField="NetWeightMeasure" HeaderText="NetWeight" SortExpression="NetWeightMeasure" />
                        <asp:BoundField DataField="NetWeightMeasureUnitCode" HeaderText="NetWeightUnitCode" SortExpression="NetWeightMeasureUnitCode"/>
                        <asp:BoundField DataField="GrossWeightMeasure" HeaderText="GrossWeightMeasure" SortExpression="GrossWeightMeasure" />
                        <asp:BoundField DataField="GrossWeightMeasureUnitCode" HeaderText="GrossWeightUnitCode" SortExpression="GrossWeightMeasureUnitCode" />
                        <asp:BoundField DataField="ShipUnitQuantity" HeaderText="ShipUnitQuantity" SortExpression="ShipUnitQuantity" />
                        <asp:BoundField DataField="PackagingLabelID" HeaderText="PackagingLabelID" SortExpression="PackagingLabelID" />
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>  
            <div>
                <asp:SqlDataSource ID="DataSourceReport" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="SF_GetPreAlertDoc" SelectCommandType="StoredProcedure">
            </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

