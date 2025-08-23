<%@ Page Title="Vehicle Unloading Pending" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PendingUnloading.aspx.cs" Inherits="Transport_PendingUnloading" Culture="en-GB" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <asp:UpdatePanel ID="upnlPendingUnloading" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false" ForeColor="Red"></asp:Label>
                <asp:ValidationSummary ID="csRequiredFields" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="vgRequired" CssClass="errorMsg" />
            </div>
            <div>
                <div class="m clear">
                    Report Type                               
                    <asp:DropDownList ID="ddlReportType" runat="server" Width="300px" AutoPostBack="true" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="1" Text="Analysis Report"></asp:ListItem>
                        <asp:ListItem Selected="False" Value="2" Text="Detailed Report"></asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;
                    <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
                        <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                </div>
                <fieldset>
                    <legend>Movement Pending Report</legend>
                    <asp:GridView ID="gvMovementPending" runat="server" CssClass="table" DataSourceID="DataSourceMovementPending"
                        Width="99%" AutoGenerateColumns="true" AllowSorting="true" AllowPaging="true" PagerSettings-Position="TopAndBottom" PageSize="80">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <%--<asp:GridView ID="gvMovementPending" runat="server" CssClass="table" DataSourceID="DataSourceMovementPending"
                        Width="99%" AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" PagerSettings-Position="TopAndBottom" PageSize="100">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Job Ref No" DataField="Job Ref No" ReadOnly="true" />
                            <asp:BoundField HeaderText="Customer" DataField="Customer" ReadOnly="true" />
                            <asp:BoundField HeaderText="Vehicle No" DataField="Vehicle No" ReadOnly="true" />
                            <asp:BoundField HeaderText="Vehicle Type" DataField="Vehicle Type" ReadOnly="true" />
                            <asp:BoundField HeaderText="Transporter" DataField="Transporter" ReadOnly="true" />
                            <asp:BoundField HeaderText="Delivery From" DataField="Delivery From" ReadOnly="true" />
                            <asp:BoundField HeaderText="Delivery Point" DataField="Delivery Point" ReadOnly="true" />
                            <asp:BoundField HeaderText="Delivery Type" DataField="Delivery Type" ReadOnly="true" />
                            <asp:BoundField HeaderText="Empty Validity Date" DataField="Empty Validity Date" ReadOnly="true" />
                            <asp:BoundField HeaderText="Dispatch Date" DataField="Dispatch Date" ReadOnly="true" />
                            <asp:BoundField HeaderText="Customer Ref No" DataField="Customer Ref No" ReadOnly="true" />
                            <asp:BoundField HeaderText="Created By" DataField="Created By" ReadOnly="true" />
                            <asp:BoundField HeaderText="Created Date" DataField="Created Date" ReadOnly="true" />
                            <asp:BoundField HeaderText="Aging" DataField="Aging" ReadOnly="true" />
                        </Columns>
                    </asp:GridView>--%>
                </fieldset>
            </div>
            <div>
                <asp:SqlDataSource ID="DataSourceMovementPending" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TR_rptPendingVehicleUnloading" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        <asp:ControlParameter ControlID="ddlReportType" Name="ReportType" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

