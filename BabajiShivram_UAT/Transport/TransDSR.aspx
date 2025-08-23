<%@ Page Title="DSR NBCPL" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
        CodeFile="TransDSR.aspx.cs" Inherits="Transport_TransDSR" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlJobTracking" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upnlJobTracking" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError_Job" runat="server"></asp:Label>
            </div>
            <fieldset>
                <legend>Transport DSR</legend>
                <div class="m clear">
                    <asp:Panel ID="Panel1" runat="server">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                        </div>
                    </asp:Panel>
                    <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click"><asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" /></asp:LinkButton>
                </div>
                <div class="clear"></div>
                <asp:GridView ID="gvTransDSR" runat="server" AutoGenerateColumns="True" CssClass="table"
                    Width="1500px" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="TransRequestID"
                    DataSourceID="DataSourceTransDSR" CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField HeaderText="Ref No" DataField="TRRefNo" Visible="false" />
                        <asp:BoundField HeaderText="Job No" DataField="JobRefNo" ItemStyle-Width="120px" />
                        <asp:BoundField DataField="CustName" HeaderText="Customer" SortExpression="CustName" />
                        <asp:BoundField DataField="RequestType" HeaderText="Type" Visible="false" />
                        <asp:BoundField DataField="LocationFrom" HeaderText="Location" SortExpression="LocationFrom" />
                        <asp:BoundField DataField="Destination" HeaderText="Destination" SortExpression="Destination" />
                        <asp:BoundField DataField="NoOfPkgs" HeaderText="Pkgs" SortExpression="NoOfPkgs"
                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="GrossWeight" HeaderText="Weight (Kgs)" SortExpression="GrossWeight"
                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="Count20" HeaderText="20" SortExpression="Count20" />
                        <asp:BoundField DataField="Count40" HeaderText="40" SortExpression="Count40" />
                        <asp:BoundField DataField="CountLCL" HeaderText="LCL" SortExpression="CountLCL" />
                        <asp:BoundField DataField="DeliveryType" HeaderText="Type" SortExpression="DeliveryType" />
                        <asp:BoundField DataField="RequestedBy" HeaderText="Created By" SortExpression="RequestedBy" />
                        <asp:BoundField DataField="RequestDate" HeaderText="Created Date" SortExpression="RequestDate" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="Instruction" HeaderText="Billing Instruction" SortExpression="Instruction" />
                        <asp:TemplateField HeaderText="KAM">
                        <ItemTemplate>
                            <asp:Label ID="lblKAM" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                        </asp:TemplateField>--%>
                    </Columns>
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="DataSourceTransDSR" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TRS_rptDSR" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

