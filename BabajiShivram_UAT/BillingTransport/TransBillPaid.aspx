<%@ Page Title="Bill Payment Detail" Language="C#" MasterPageFile="~/TransportMaster.master" AutoEventWireup="true" CodeFile="TransBillPaid.aspx.cs" 
        Inherits="BillingTransport_TransBillPaid" EnableEventValidation="false" Culture="en-GB" %>

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
                    <asp:GridView ID="gvVendorJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                        PagerStyle-CssClass="pgr" DataKeyNames="BillID" AllowPaging="True" AllowSorting="True" Width="1330px" 
                        PageSize="20" PagerSettings-Position="TopAndBottom" DataSourceID="DataSourceVendorJobs"
                        OnRowCommand="gvVendorJobDetail_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="BS Job No" SortExpression="JobRefNo"
                                ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnUpdatedVehicle" runat="server" Text='<%#Eval("JobRefNo") %>'
                                        ToolTip="Submit Bill" CommandName="select" CommandArgument='<%#Eval("TransReqID") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:BoundField HeaderText="BS Job No" DataField="JobRefNo"/>
                            <%--<asp:BoundField HeaderText="Transporter" DataField="Transporter"/>--%>
                            <asp:BoundField HeaderText="Bill Amount" DataField="BillAmount"/>
                            <asp:BoundField HeaderText="Bill Date" DataField="BillDate" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField HeaderText="Delivery From" DataField="LocationFrom" SortExpression="LocationFrom"/>
                            <asp:BoundField HeaderText="Bill Type" DataField="BillType"/>
                            <asp:BoundField HeaderText="Paid Amount" DataField="PaidAmount"/>
                            <asp:BoundField HeaderText="TDS" DataField="TDSTotalAmount"/>
                            <asp:BoundField HeaderText="UTR No" DataField="InstrumentNo" SortExpression="LocationFrom"/>
                            <asp:BoundField HeaderText="UTR Date" DataField="InstrumentDate" DataFormatString="{0:dd/MM/yyyy}" />
                        </Columns>
                        <PagerTemplate>
                            <asp:GridViewPager ID="GridViewPager1" runat="server" />
                        </PagerTemplate>
                    </asp:GridView>
                    <asp:SqlDataSource ID="DataSourceVendorJobs" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="TRS_GetPaidBillByVendorId" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:SessionParameter SessionField="CID" Name="VendorId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

