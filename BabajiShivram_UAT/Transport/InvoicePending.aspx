<%@ Page Title="Invoice Pending" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="InvoicePending.aspx.cs"
    Inherits="Transport_InvoicePending" Culture="en-GB" EnableEventValidation="false" %>

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
                    <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
                        <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                </div>
                <fieldset>
                    <legend>Invoice Pending Report</legend>
                    <asp:GridView ID="gvInvoicePending" runat="server" CssClass="table" DataSourceID="DataSourceInvoicePending"
                        Width="99%" AutoGenerateColumns="true" AllowSorting="true" AllowPaging="true" PagerSettings-Position="TopAndBottom" PageSize="80">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </fieldset>
            </div>
            <div>
                <asp:SqlDataSource ID="DataSourceInvoicePending" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TR_rptInvoicePending" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

