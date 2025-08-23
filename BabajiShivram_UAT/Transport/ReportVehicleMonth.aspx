<%@ Page Title="Vehicle Trip" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
     CodeFile="ReportVehicleMonth.aspx.cs" Inherits="Transport_ReportVehicleMonth" EnableEventValidation="false" Culture="en-GB" %>
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
            <fieldset><legend>Vehicle Type wise Trip</legend>
                <div>
                &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="lnkXls" runat="server" OnClick="lnkXls_Click" data-tooltip="&nbsp; Export To Excel">
                <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" />
                </asp:LinkButton>
                <asp:DropDownList ID="ddBranch" runat="server" DataSourceID="GridviewSqlDataSource" DataTextField="BranchName"
                    DataValueField="lid" AppendDataBoundItems="true" AutoPostBack="true">
                    <asp:ListItem Text="Branch Name" Value="0" Selected="True"></asp:ListItem>
                 </asp:DropDownList>
                <asp:GridView ID="gvTransportMonth" runat="server" CssClass="table" DataSourceID="DataSourceVehicleMonth" 
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
            <div>
                <asp:SqlDataSource ID="DataSourceVehicleMonth" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TR_rptVehicleMonth" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        <asp:ControlParameter ControlID="ddBranch" Name="BranchId" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="GridviewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetAllBranch" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
