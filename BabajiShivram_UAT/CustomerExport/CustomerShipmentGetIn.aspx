<%@ Page Title="Shipment Get In" Language="C#" MasterPageFile="~/ExportCustomerMaster.master" AutoEventWireup="true" CodeFile="CustomerShipmentGetIn.aspx.cs"
    Inherits="CustomerExport_CustomerShipmentGetIn" Culture="en-GB" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlShipmentGetIn"
            runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../Images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upnlShipmentGetIn" runat="server" UpdateMode="Conditional"
        RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" />
            </div>
            <div class="clear">
            </div>
            <fieldset>
                <legend>Pending Shipment Get In</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                        </div>
                        <div class="fleft" style="margin-left: 30px; padding-top: 3px;">
                            <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                            <asp:Image ID="Image2" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
                <div class="clear">
                </div>

                <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                    PagerStyle-CssClass="pgr" DataKeyNames="JobId" AllowPaging="True" AllowSorting="True" PageSize="20"
                    PagerSettings-Position="TopAndBottom" Width="100%" OnRowDataBound="gvJobDetail_RowDataBound"
                    DataSourceID="PendingJobForShipmentGetIn" OnPreRender="gvJobDetail_PreRender">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No"/>
                        <asp:BoundField DataField="CustRefNo" HeaderText="Cust Ref No" SortExpression="CustRefNo" ReadOnly="true" />
                        <asp:BoundField DataField="Shipper" HeaderText="Shipper" SortExpression="Shipper" ReadOnly="true" />
                        <asp:BoundField DataField="Consignee" HeaderText="Consignee" SortExpression="Consignee" ReadOnly="true" />
                        <asp:BoundField DataField="TransMode" HeaderText="Mode" SortExpression="TransMode" ReadOnly="true" />
                        <asp:BoundField DataField="PortOfLoading" HeaderText="Port Of Loading" SortExpression="PortOfLoading" ReadOnly="true" />
                        <asp:BoundField DataField="PortOfDischarge" HeaderText="Port Of Discharge" SortExpression="PortOfDischarge" ReadOnly="true" />
                        <asp:BoundField DataField="SBNo" HeaderText="SB No" SortExpression="SBNo" ReadOnly="true" />
                        <asp:BoundField DataField="SBDate" HeaderText="SB Date" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                        <asp:BoundField DataField="MarkAppraisingValue" HeaderText="To be Marked/Appraising" SortExpression="MarkAppraisingValue" ReadOnly="true" />
                        <asp:BoundField DataField="LEODate" HeaderText="Superintendent LEO Date" SortExpression="LEODate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>

            </fieldset>
            <div>
                <asp:SqlDataSource ID="PendingJobForShipmentGetIn" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CC_GetShipmentGetInByCustId" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="CustUserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

