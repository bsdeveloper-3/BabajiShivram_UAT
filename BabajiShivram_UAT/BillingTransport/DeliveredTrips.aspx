<%@ Page Title="Delivered Trip Details" Language="C#" MasterPageFile="~/TransportMaster.master" AutoEventWireup="true" CodeFile="DeliveredTrips.aspx.cs"
    Inherits="BillingTransport_DeliveredTrips" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <style type="text/css">
        input[type="text" i]:disabled {
            background-color: rgba(211, 211, 211, 0.72);
            color: black;
        }

        table textarea {
            background-color: rgba(211, 211, 211, 0.72);
            color: black;
        }

        table.table {
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
                                    <uc1:datafilter id="DataFilter1" runat="server" />
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
                            Width="1320px" PageSize="20" PagerSettings-Position="TopAndBottom" OnPreRender="gvVendorJobDetail_PreRender"
                            DataSourceID="DataSourceVendorJobs" OnRowCommand="gvVendorJobDetail_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Vehicle No" DataField="VehicleNo" ReadOnly="true" />
                                <asp:BoundField HeaderText="Destination" DataField="DeliveryPoint" ReadOnly="true" />
                                <asp:BoundField HeaderText="Dispatch Date" DataField="DispatchDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:TemplateField HeaderText="Detention">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDetentionTotal" runat="server" Text='<%#Eval("DetentionCharges") %>'
                                            ToolTip="Detention Total of updated vehicles."></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Warai">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWaraiTotal" runat="server" Text='<%#Eval("WaraiCharges") %>' ToolTip="Warai Total of updated vehicles."></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Off Loading">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmptyOffLoadingTotal" runat="server" Text='<%#Eval("OffLoadingCharges") %>'
                                            ToolTip="Empty Off Loading Total of updated vehicles."></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Union">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTempoUnionTotal" runat="server" Text='<%#Eval("UnionCharges") %>'
                                            ToolTip="Tempo Union Total of updated vehicles."></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Freight Rate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFreightRate" runat="server" Text='<%#Eval("FreightCharges") %>' ToolTip="Freight Total of updated vehicles."></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Rate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotal" runat="server" Text='<%#Eval("TotalAmount") %>' ToolTip="Total amount."></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Approved Rate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEditTotal" runat="server" Text='<%#(Eval("ApprovedAmount").ToString() == "0.00" ? "" :  Eval("ApprovedAmount"))%>'
                                            ToolTip="Edited Total Rate." Font-Bold="true"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--  <asp:TemplateField HeaderText="Invoice Copy" ItemStyle-Width="11%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDownloadInvoice" Text='<%#(String.IsNullOrEmpty(Eval("DocumentPath").ToString()) ? "" : "Download")%>'
                                            CommandArgument='<%# Eval("DocumentPath") %>' CausesValidation="false" runat="server"
                                            CommandName="downloaddoc"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkDocUpload" runat="server" Text="Upload Invoice" CommandName="documentpopup"
                                            CommandArgument='<%#Eval("lid")+","+Eval("DeliveryId") %>' CausesValidation="false"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
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
                        <cc1:modalpopupextender id="ModalPopupDocument" runat="server" cachedynamicresults="false"
                            dropshadow="False" popupcontrolid="Panel2Document" targetcontrolid="lnkDummy">
                        </cc1:modalpopupextender>
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
                            <div id="dvUploadNewFile" runat="server" style="max-height: 200px; overflow: auto; margin-left: 15px">
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

