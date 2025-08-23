<%@ Page Title="Bill - Rejected" Language="C#" MasterPageFile="~/TransportMaster.master" AutoEventWireup="true" 
  CodeFile="RejectedVendorBill.aspx.cs" Inherits="BillingTransport_RejectedVendorBill" EnableEventValidation="false" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlJobWiseVehicleDetail"
            runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upnlJobWiseVehicleDetail" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lberror" Text="" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <fieldset style="min-height: 380px; margin-top: 0px">
                <legend>Job Details</legend>
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
                    <asp:GridView ID="gvVendorRejectedBill" runat="server" AutoGenerateColumns="False" CssClass="table"
                        PagerStyle-CssClass="pgr" DataKeyNames="TransReqID" AllowPaging="True" AllowSorting="True" Width="1330px" 
                        PageSize="20" PagerSettings-Position="TopAndBottom" DataSourceID="DataSourceRejectedJobs"
                        OnRowCommand="gvVendorRejectedBill_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="BS Job No" SortExpression="JobRefNo"
                                ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnUpdatedVehicle" runat="server" Text='<%#Eval("JobRefNo") %>'
                                        ToolTip="Submit Bill" CommandName="select" CommandArgument='<%#Eval("TransReqID") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="BS Job No" DataField="JobRefNo" Visible="false" />
                            <asp:BoundField HeaderText="Customer" DataField="CustName"/>
                            <asp:BoundField HeaderText="Delivery From" DataField="LocationFrom" ReadOnly="true"/>
                            <asp:BoundField HeaderText="Total Vehicle" DataField="TotalVehiclePlaced" />
                            
<%--                            <asp:BoundField HeaderText="Destination" DataField="DeliveryDestination" ReadOnly="true"
                                ItemStyle-Width="9%" />--%>
                            <%--<asp:BoundField HeaderText="Dispatch Date" DataField="LastDispatchDate" ReadOnly="true"
                                DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="7%" />--%>
                            <%--<asp:TemplateField HeaderText="Aging" ItemStyle-Width="2%">
                                <ItemTemplate>
                                    <asp:Label ID="lblAging" runat="server" Text='<%#Eval("Aging") %>' ToolTip="Today – Dispatch Date"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <%--<asp:TemplateField HeaderText="Freight Total" ItemStyle-Width="4%">
                                <ItemTemplate>
                                    <asp:Label ID="lblFreightRate" runat="server" Text='<%#Eval("FrightTotal") %>' ToolTip="Freight Total of updated vehicles."></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Detention Total" ItemStyle-Width="5%">
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
                            <asp:TemplateField HeaderText="Empty Off Loading Total" ItemStyle-Width="4%">
                                <ItemTemplate>
                                    <asp:Label ID="lblEmptyOffLoadingTotal" runat="server" Text='<%#Eval("OffLoadingTotal") %>'
                                        ToolTip="Empty Off Loading Total of updated vehicles."></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tempo Union Total" ItemStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="lblTempoUnionTotal" runat="server" Text='<%#Eval("UnionTotal") %>'
                                        ToolTip="Tempo Union Total of updated vehicles."></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total" ItemStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotal" runat="server" Text='<%#Eval("TotalAmount") %>' ToolTip="Total amount."></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                        </Columns>
                        <PagerTemplate>
                            <asp:GridViewPager ID="GridViewPager1" runat="server" />
                        </PagerTemplate>
                    </asp:GridView>
                    <asp:SqlDataSource ID="DataSourceRejectedJobs" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="TRS_GetRejectBillByVendorId" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <%--<asp:SessionParameter SessionField="VendorId" Name="UserId" />--%>
                            <asp:SessionParameter SessionField="CID" Name="VendorId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


