<%@ Page Title="Transport Bill" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="TestBilling.aspx.cs" Inherits="Transport_TestBilling" Culture="en-GB" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
            <asp:Label ID="lblError" runat="server"></asp:Label>
        </div>
        <div class="m clear">
        <fieldset><legend>Billing Pending</legend>
        <div class="m clear">
            <asp:Panel ID="pnlFilter" runat="server">
                <div class="fleft">
                    <uc1:DataFilter ID="DataFilter1" runat="server" />
                </div>
            </asp:Panel>
        </div>
        <div class="clear">
        </div>
            <asp:GridView ID="GridViewVehicle" runat="server" AutoGenerateColumns="False" CssClass="table"
                Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="TransID"
                DataSourceID="DataSourceVehicle" CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20" 
                OnRowEditing="GridViewVehicle_RowEditing" OnRowCancelingEdit="GridViewVehicle_RowCancelingEdit"
                OnRowUpdating="GridViewVehicle_RowUpdating" OnRowCommand="GridViewVehicle_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ref No">
                        <ItemTemplate>
                        <asp:LinkButton ID="lnkSelect" CommandName="select" runat="server" Text='<%# BIND("TRRefNo")%>'
                                ToolTip="Click To Update Bill Detail"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Ref No" DataField="TRRefNo" ReadOnly="true" Visible="false" />
                    <asp:BoundField HeaderText="Job No" DataField="JobRefNo" ReadOnly="true" />
                    <asp:BoundField HeaderText="Transporter" DataField="Transporter" ReadOnly="true" />
                    <asp:BoundField HeaderText="Customer" DataField="CustName" ReadOnly="true" />
                    <asp:BoundField HeaderText="Pkg" DataField="Packages" ReadOnly="true" />
                    <asp:BoundField HeaderText="Vehicle No" DataField="VehicleNo" ReadOnly="true" />
                    <asp:BoundField HeaderText="Type" DataField="VehicleType" ReadOnly="true" />
                    <asp:BoundField HeaderText="From" DataField="DeliveryFrom" ReadOnly="true" />
                    <asp:BoundField HeaderText="To" DataField="DeliveryTo" ReadOnly="true" />
                    <asp:BoundField HeaderText="Delivery Type" DataField="DeliveryType" ReadOnly="true" />
                    <asp:TemplateField HeaderText="UnLoading Date" SortExpression="UnLoadingDate">
                        <ItemTemplate>
                            <asp:Label ID="lblUnLoadingDate" Text='<%# BIND("UnLoadingDate","{0:dd/MM/yyyy}")%>' runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Bill Status" DataField="BillStatusName" ReadOnly="true" />
                </Columns>
            </asp:GridView>
        </fieldset>
        <div>
        <asp:SqlDataSource ID="DataSourceVehicle" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetTransMovement" SelectCommandType="StoredProcedure">
        </asp:SqlDataSource>
    </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


