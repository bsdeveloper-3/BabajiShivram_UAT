<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master"
    CodeFile="FinalApprovedBills.aspx.cs" Inherits="Transport_FinalApprovedBills"
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
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlFinalBills" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upnlFinalBills" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                ShowSummary="False" ValidationGroup="vgTransp" />
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <fieldset id="fsDeliveryDetails" runat="server">
                <legend>Approved Bills</legend>
                <div id="dvFilter" runat="server">
                    <div class="m clear">
                        <asp:Panel ID="pnlFilter" runat="server">
                            <div class="fleft">
                                <uc1:DataFilter ID="DataFilter1" runat="server" />
                            </div>
                            <div style="margin-left: 2px;">
                                <%--  <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                                    <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                                </asp:LinkButton>--%>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <div class="m clear">
                </div>
                <asp:GridView ID="gvApprovedBills" runat="server" AutoGenerateColumns="False" CssClass="table"
                    PagerStyle-CssClass="pgr" DataKeyNames="lid,TransporterId,VehicleNo" AllowPaging="True"
                    AllowSorting="True" Width="100%" PageSize="20" PagerSettings-Position="TopAndBottom"
                    DataSourceID="DataSourceApprovedBills" OnPreRender="gvApprovedBills_PreRender"
                    OnRowCommand="gvApprovedBills_RowCommand" OnRowDataBound="gvApprovedBills_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delivery Detail" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDeliveryDetail" runat="server" Text="Show Details" CommandName="selectvehicles"
                                    ToolTip="Shows Vehicle Wise Delivery details." CommandArgument='<%#Eval("lid") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Invoice Copy" ItemStyle-Width="1%">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnDwnInvoice" runat="server" ImageUrl="~/Images/file.gif"
                                    CommandArgument='<%# Eval("DocumentPath") %>' CommandName="downloaddoc" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="BS Job No" DataField="JobRefNo" ReadOnly="true" ItemStyle-Width="13%" />
                        <asp:BoundField HeaderText="Transporter" DataField="TransporterName" ReadOnly="true"
                            ItemStyle-Width="15%" />
                        <asp:TemplateField HeaderText="No of Vehicles" SortExpression="NoOfVehicle" ItemStyle-Width="2%"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblNoOfVehicle" runat="server" Text='<%#Eval("TotalVehicle") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Delivery From" DataField="DeliveryFrom" ReadOnly="true"
                            ItemStyle-Width="10%" />
                        <asp:BoundField HeaderText="Destination" DataField="DeliveryPoint" ReadOnly="true"
                            ItemStyle-Width="9%" />
                        <asp:BoundField HeaderText="Dispatch Date" DataField="LastDispatchDate" ReadOnly="true"
                            DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="4%" />
                        <asp:TemplateField HeaderText="Freight Total" ItemStyle-Width="2%">
                            <ItemTemplate>
                                <asp:Label ID="lblFreightRate" runat="server" Text='<%#Eval("FrightTotal") %>' ToolTip="Freight Total of updated vehicles."></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Detention Total" ItemStyle-Width="1%">
                            <ItemTemplate>
                                <asp:Label ID="lblDetentionTotal" runat="server" Text='<%#Eval("DetentionTotal") %>'
                                    ToolTip="Detention Total of updated vehicles."></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Warai Total" ItemStyle-Width="2%">
                            <ItemTemplate>
                                <asp:Label ID="lblWaraiTotal" runat="server" Text='<%#Eval("WaraiTotal") %>' ToolTip="Warai Total of updated vehicles."></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Empty Off Loading Total" ItemStyle-Width="3%">
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
                        <asp:TemplateField HeaderText="Total" ItemStyle-Width="5%">
                            <ItemTemplate>
                                <asp:Label ID="lblTotal" runat="server" Text='<%#Eval("TotalAmount") %>' ToolTip="Total amount."></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager ID="GridViewPager1" runat="server" />
                    </PagerTemplate>
                </asp:GridView>
                <asp:SqlDataSource ID="DataSourceApprovedBills" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TR_GetFinalBillApproved" SelectCommandType="StoredProcedure">
                </asp:SqlDataSource>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
