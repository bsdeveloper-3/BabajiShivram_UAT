<%@ Page Title="Transporter Wise Business" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TPWiseSummary.aspx.cs"
    Inherits="Transport_TPWiseSummary" Culture="en-GB" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <asp:UpdatePanel ID="upnlWeeklyTrip" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false" ForeColor="Red"></asp:Label>
            </div>
            <div>
                <fieldset>
                    <legend>Transporter Wise Vehicle Placed Details</legend>
                    <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
                        <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>

                    <asp:GridView ID="gvTPWiseBusiness" runat="server" CssClass="table" DataSourceID="DataSourceTPWsSummary"
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
                <asp:SqlDataSource ID="DataSourceTPWsSummary" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TR_rptTransporterWiseVehicleCount" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

