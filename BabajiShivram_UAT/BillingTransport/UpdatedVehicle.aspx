<%@ Page Title="" Language="C#" MasterPageFile="~/TransportMaster.master" AutoEventWireup="true"
    CodeFile="UpdatedVehicle.aspx.cs" Inherits="BillingTransport_UpdatedVehicle"
    Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <style type="text/css">
        input[type="text" i]:disabled
        {
            background-color: rgba(211, 211, 211, 0.72);
            color: black;
        }
        table textarea
        {
            background-color: rgba(211, 211, 211, 0.72);
            color: black;
        }
        table.table
        {
            white-space: normal;
        }
    </style>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upShipment" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upShipment" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <fieldset style="min-height: 380px; margin-top: 0px; margin-bottom: 0px">
                <div align="center">
                    <asp:Label ID="lberror" Text="" runat="server" EnableViewState="false"></asp:Label>
                </div>
                <fieldset id="fsDeliveryDetails" runat="server">
                    <legend>Delivered Vehicle Detail</legend>
                    <div id="dvFilter" runat="server">
                        <div class="m clear">
                            <asp:Panel ID="pnlFilter" runat="server">
                                <div class="fleft">
                                    <uc1:DataFilter ID="DataFilter1" runat="server" />
                                </div>
                                <div style="margin-left: 2px;">
                                    <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                                        <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                                    </asp:LinkButton>
                                </div>
                            </asp:Panel>
                        </div>
                        <div class="clear">
                        </div>
                    </div>
                    <div>
                        <asp:GridView ID="gvVendorJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                            PagerStyle-CssClass="pgr" DataKeyNames="lId" AllowPaging="True" AllowSorting="True"
                            Width="1400px" PageSize="20" PagerSettings-Position="TopAndBottom" OnPreRender="gvVendorJobDetail_PreRender"
                            DataSourceID="DataSourceVendorJobs" OnRowCommand="gvVendorJobDetail_RowCommand"
                            OnRowDataBound="gvVendorJobDetail_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delivery Detail" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDeliveryDetail" runat="server" Text="Show Details" CommandName="selectvehicles"
                                            ToolTip="Shows Vehicle Wise Delivery details." CommandArgument='<%#Eval("lid") +"," +Eval("StatusID")%>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" SortExpression="Status" ItemStyle-Width="16%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--  <asp:BoundField HeaderText="Status" DataField="Status" ReadOnly="true" ItemStyle-Width="14%" />--%>
                                <asp:BoundField HeaderText="BS Job No" DataField="JobRefNo" ReadOnly="true" ItemStyle-Width="10%" />
                                <%--   <asp:BoundField HeaderText="Vehicle No" DataField="VehicleNo" ReadOnly="true" ItemStyle-Width="5%" />--%>
                                <asp:BoundField HeaderText="Customer" DataField="CustomerName" ReadOnly="true" ItemStyle-Width="14%" />
                                <asp:TemplateField HeaderText="No of Vehicles" SortExpression="NoOfVehicle" ItemStyle-Width="2%"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNoOfVehicle" runat="server" Text='<%#Eval("TotalVehicle") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Delivery From" DataField="DeliveryFrom" ReadOnly="true"
                                    ItemStyle-Width="9%" />
                                <asp:BoundField HeaderText="Destination" DataField="DeliveryPoint" ReadOnly="true"
                                    ItemStyle-Width="9%" />
                                <asp:BoundField HeaderText="Dispatch Date" DataField="LastDispatchDate" ReadOnly="true"
                                    DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="5%" />
                                <asp:TemplateField HeaderText="Freight Total" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFreightRate" runat="server" Text='<%#Eval("FrightTotal") %>' ToolTip="Freight Total of updated vehicles."></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Detention Total" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDetentionTotal" runat="server" Text='<%#Eval("DetentionTotal") %>'
                                            ToolTip="Detention Total of updated vehicles."></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Warai Total" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWaraiTotal" runat="server" Text='<%#Eval("WaraiTotal") %>' ToolTip="Warai Total of updated vehicles."></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Empty Off Loading Total" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmptyOffLoadingTotal" runat="server" Text='<%#Eval("OffLoadingTotal") %>'
                                            ToolTip="Empty Off Loading Total of updated vehicles."></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tempo Union Total" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTempoUnionTotal" runat="server" Text='<%#Eval("UnionTotal") %>'
                                            ToolTip="Tempo Union Total of updated vehicles."></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotal" runat="server" Text='<%#Eval("TotalAmount") %>' ToolTip="Total amount."></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Invoice Copy" ItemStyle-Width="11%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDownloadInvoice" Text='<%#(String.IsNullOrEmpty(Eval("DocumentPath").ToString()) ? "" : "Download")%>'
                                            CommandArgument='<%# Eval("DocumentPath") %>' CausesValidation="false" runat="server"
                                            CommandName="downloaddoc"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkDocUpload" runat="server" Text="Upload Invoice" CommandName="documentpopup"
                                            CommandArgument='<%#Eval("lid")+","+Eval("DeliveryId") %>' CausesValidation="false"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerTemplate>
                                <asp:GridViewPager ID="GridViewPager1" runat="server" />
                            </PagerTemplate>
                        </asp:GridView>
                        <asp:SqlDataSource ID="DataSourceVendorJobs" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="TR_GetDeliveredVehicleDetails" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter SessionField="VendorId" Name="UserId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                    <!--Document for Doc Upload-->
                    <div id="divDocument">
                        <cc1:ModalPopupExtender ID="ModalPopupDocument" runat="server" CacheDynamicResults="false"
                            DropShadow="False" PopupControlID="Panel2Document" TargetControlID="lnkDummy">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="Panel2Document" runat="server" CssClass="ModalPopupPanel" Width="400px">
                            <div class="header">
                                <div class="fleft">
                                    Upload Invoice Copy
                                </div>
                                <div class="fright">
                                    <asp:ImageButton ID="imgClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="btnCancelPopup_Click"
                                        ToolTip="Close" />
                                </div>
                            </div>
                            <div class="m">
                            </div>
                            <div id="Div1" runat="server" style="max-height: 200px; overflow: auto;">
                                <asp:HiddenField ID="hdnJobId" runat="server" />
                                <asp:HiddenField ID="hdnVehicleDeliveryId" runat="server" />
                                <!-- Lists Of All Documents -->
                                <div align="center">
                                    <asp:Label ID="lbError_Popup" runat="server" Visible="true"></asp:Label>
                                </div>
                            </div>
                            <!-- Add new Document -->
                            <div class="m clear">
                            </div>
                            <div id="dvUploadNewFile" runat="server" style="max-height: 200px; overflow: auto;
                                margin-left: 15px">
                                <asp:FileUpload ID="fuDocument" runat="server" />
                                <asp:Button ID="btnSaveDocument" Text="Save Document" runat="server" OnClick="btnSaveDocument_Click"
                                    CausesValidation="false" />
                            </div>
                            <div class="m clear">
                            </div>
                            <!--Document for BIlling Advice- END -->
                        </asp:Panel>
                    </div>
                    <div>
                        <asp:LinkButton ID="lnkDummy" runat="server" Text=""></asp:LinkButton>
                    </div>
                    <!--Document for Doc Upload - END -->
                </fieldset>
            </fieldset>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSaveDocument" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
