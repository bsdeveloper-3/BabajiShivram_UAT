<%@ Page Title="Warehouse Detail" Language="C#" MasterPageFile="~/ExportCustomerMaster.master" AutoEventWireup="true" 
    CodeFile="CustomerWarehouse.aspx.cs" Inherits="CustomerExport_CustomerWarehouse" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <style type="text/css">
        .modalBackground {
            background-color: black;
            filter: alpha(opacity=90);
            opacity: 0.5;
        }
        .modalPopup1 {
            border-radius: 5px;
            background-color: #FFFFFF;
            border-width: 1px;
            border-style: solid;
            border-color: black;
            padding-top: 5px;
            padding-left: 3px;
            width: 600px;
            height: 300px;
        }
    </style>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upJobDetail" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); opacity: .8;">
                    <img alt="progress" src="../Images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upJobDetail" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>

            <div align="center">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            </div>

            <fieldset>
                <legend>Pending Warehouse</legend>
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

                <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                    PagerStyle-CssClass="pgr" DataKeyNames="JobId" AllowPaging="True" AllowSorting="True" PageSize="20"
                    PagerSettings-Position="TopAndBottom" OnRowCommand="gvJobDetail_RowCommand" Width="100%"
                    DataSourceID="PendingJobForWarehouse" OnPreRender="gvJobDetail_PreRender" OnRowDataBound="gvJobDetail_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" />
                        <asp:BoundField DataField="CustRefNo" HeaderText="Cust Ref No" SortExpression="CustRefNo" ReadOnly="true" />
                        <asp:BoundField DataField="Shipper" HeaderText="Shipper" SortExpression="Shipper" ReadOnly="true" />
                        <asp:BoundField DataField="Consignee" HeaderText="Consignee" SortExpression="Consignee" ReadOnly="true" />
                        <asp:BoundField DataField="TransMode" HeaderText="Mode" SortExpression="TransMode" ReadOnly="true" />
                        <asp:BoundField DataField="PortOfDischarge" HeaderText="Port Of Discharge" SortExpression="PortOfDischarge" ReadOnly="true" />
                        <asp:BoundField DataField="VehiclePlaced" HeaderText="Vehicle Placed" SortExpression="VehiclePlaced" ReadOnly="true" />
                        <asp:BoundField DataField="TransporterName" HeaderText="Transporter Name" SortExpression="TransporterName" ReadOnly="true" />
                        <asp:TemplateField HeaderText="Delivered Items">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDeliveredItems" CommandName="GetDeliveredItems" runat="server"
                                    CommandArgument='<%#Eval("JobId") + ";" + Eval("TransModeId") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="RequestDate" HeaderText="Request Date" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                        <asp:BoundField DataField="RequestBy" HeaderText="Request By" SortExpression="RequestBy" ReadOnly="true" />
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>

            </fieldset>
            <div>
                <asp:SqlDataSource ID="PendingJobForWarehouse" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CC_GetPendingWarehouseByCustId" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="CustUserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>

            <div id="divDeliveredItems">
                <asp:LinkButton ID="modelPopup1" runat="server" Visible="false" />
                <AjaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="modelPopup1" BackgroundCssClass="modalBackground"
                    PopupControlID="Panel2" DropShadow="true">
                </AjaxToolkit:ModalPopupExtender>
                <asp:Panel ID="Panel2" runat="server" Style="display: none">
                        <fieldset>
                                <legend>
                                    <asp:Label ID="lblDeliveredSubject" runat="server"></asp:Label></b>
                                    <div class="header">
                                        <div class="fleft">
                                            <asp:LinkButton ID="lnkDeliveredItems" runat="server" OnClick="lnkDeliveredItems_Click" ToolTip="Export To Excel">
                                                <asp:Image ID="imgExportDelItemsToExcel" runat="server" ImageUrl="~/images/Excel.jpg" Style="margin-top: 5px" />
                                            </asp:LinkButton>
                                        </div>
                                        <div class="fright">
                                        <asp:ImageButton ID="imgbtnDeliveredItems" ImageUrl="~/Images/delete.gif" runat="server" OnClick="btnCancelPopup_Click1" ToolTip="Close" />
                                        </div>
                                </div>
                            <div class="clear"></div>

                        <asp:Panel ID="Panel4" runat="server" Width="100%" ScrollBars="Auto" BorderStyle="Solid" BorderWidth="1px">
                            <asp:GridView ID="GridViewVehicle" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                                Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                                DataSourceID="DataSourceVehicle" CellPadding="4" AllowPaging="True" AllowSorting="True"
                                PageSize="20" OnRowCommand="GridViewVehicle_RowCommand" OnRowDataBound="GridViewVehicle_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Container No" DataField="ContainerNo" />
                                    <asp:BoundField HeaderText="Packages" DataField="NoOfPackages" />
                                    <asp:BoundField HeaderText="Vehicle No" DataField="VehicleNo" />
                                    <asp:BoundField HeaderText="Transporter" DataField="TransporterName" />
                                    <asp:BoundField HeaderText="LR No" DataField="LRNo" />
                                    <asp:BoundField HeaderText="LR Date" DataField="LRDate" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField HeaderText="Dispatch Date" DataField="DispatchDate" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:TemplateField HeaderText="LR Attachment">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDownload" runat="server" Text='<%#Eval("PODAttachmentPath") %>'
                                                CommandName="Download" CommandArgument='<%#Eval("PODDownload") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="User" DataField="UserName" />
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                </asp:Panel>
                <div>
                    <asp:SqlDataSource ID="DataSourceVehicle" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="EX_GetDeliveryDetail" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:SessionParameter Name="JobId" SessionField="JobId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

