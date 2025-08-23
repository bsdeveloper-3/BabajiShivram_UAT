<%@ Page Title="Vehicle Fuel Summary" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="ReportVehicleExpense.aspx.cs" Inherits="Transport_ReportVehicleExpense" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPanel" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPanel" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <fieldset><legend>Vehicle Daily Expense Detail</legend>
                <div>
                &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="lnkXls" runat="server" OnClick="lnkXls_Click" data-tooltip="&nbsp; Export To Excel">
                <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" />
                </asp:LinkButton>
                
                <asp:GridView ID="gvTransportMonth" runat="server" CssClass="table" DataSourceID="DataSourceExpense" 
                    Width="99%" AutoGenerateColumns="true" AllowSorting="true">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex +1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                </asp:GridView>
                </div>
            </fieldset>
            <asp:SqlDataSource ID="DataSourceExpense" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="TR_rptVehicleFuel" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                </SelectParameters>
            </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

