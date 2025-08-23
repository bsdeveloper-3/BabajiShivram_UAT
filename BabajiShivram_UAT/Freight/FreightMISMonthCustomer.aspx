<%@ Page Title="Customer Month Wise Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="FreightMISMonthCustomer.aspx.cs" Inherits="Freight_FreightMISMonthCustomer" EnableEventValidation="false" Culture="en-GB" %>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" ID="content1">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upReport" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <div class="clear"></div>
    <asp:UpdatePanel ID="upReport" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
                <div>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <tr>
                        <td>
                            Report Mode
                        </td>
                        <td>
                            <asp:DropDownList ID="ddMode" runat="server" AutoPostBack="true">
                                <asp:ListItem Value="0" Text="-ALL-" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="1" Text="-AIR-"></asp:ListItem>
                                <asp:ListItem Value="2" Text="-SEA-"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <div>
                <fieldset><legend>MIS - Customer Enquiry</legend>
                <div class="m clear">
                <div class="fleft">
                <asp:LinkButton ID="lnkExportCustomer" runat="server" OnClick="lnkExportCustomer_Click">
                    <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
                &nbsp;
                </div>
                </div>
                <div>
                    <asp:GridView ID="gvCustomerReport" runat="server" DataSourceID="DataSourceCustomer" 
                        AutoGenerateColumns="False" CssClass="table" AllowSorting="true">
                        <Columns>
                            <%--<asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:BoundField HeaderText="Customer" DataField="Customer" SortExpression="Customer" />
                            <asp:BoundField HeaderText="Status" DataField="Status" SortExpression="Status" />
                            <asp:BoundField HeaderText="Apr" DataField="Apr" SortExpression="Apr" />
                            <asp:BoundField HeaderText="May" DataField="May" SortExpression="May" />
                            <asp:BoundField HeaderText="Jun" DataField="Jun" SortExpression="Jun" />
                            <asp:BoundField HeaderText="Jul" DataField="Jul" SortExpression="Jul" />
                            <asp:BoundField HeaderText="Aug" DataField="Aug" SortExpression="Aug" />
                            <asp:BoundField HeaderText="Sep" DataField="Sep" SortExpression="Sep" />
                            <asp:BoundField HeaderText="Oct" DataField="Oct" SortExpression="Oct" />
                            <asp:BoundField HeaderText="Nov" DataField="Nov" SortExpression="Nov" />
                            <asp:BoundField HeaderText="Dec" DataField="Dec" SortExpression="Dec" />
                            <asp:BoundField HeaderText="Jan" DataField="Jan" SortExpression="Jan" />
                            <asp:BoundField HeaderText="Feb" DataField="Feb" SortExpression="Feb" />
                            <asp:BoundField HeaderText="Mar" DataField="Mar" SortExpression="Mar" />
                            <asp:BoundField HeaderText="Total" DataField="Total" SortExpression="Total" />
                        </Columns>
                    </asp:GridView>
                </div>
            </fieldset> 
            <asp:SqlDataSource ID="DataSourceCustomer" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommandType="StoredProcedure" SelectCommand="FR_MISCustomerEnquiry">
                <SelectParameters>
                    <asp:ControlParameter ControlID="ddMode" Name="ModeID" PropertyName="SelectedValue" />
                    <asp:SessionParameter Name="UserId" SessionField="UserId" />
                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                </SelectParameters>
            </asp:SqlDataSource> 
                </div>
        </ContentTemplate> 
    </asp:UpdatePanel> 
</asp:Content>

