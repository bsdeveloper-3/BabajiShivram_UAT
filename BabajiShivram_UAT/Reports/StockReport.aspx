<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="StockReport.aspx.cs"
    Inherits="Reports_StockReport" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1"></cc1:ToolkitScriptManager>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upStockReport" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upStockReport" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            </div>

            <fieldset>
                <legend>Stock Report</legend>
                <div>
                    <table>
                        <tr>
                            <td>Customer
                                <%--<asp:RequiredFieldValidator ID="RFVCustomer" runat="server" ControlToValidate="ddCustomer" InitialValue="0"
                                ErrorMessage="*" Text="Required" ValidationGroup="validateReport" Display="Dynamic"></asp:RequiredFieldValidator>---%>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCustomer" runat="server" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged" AutoPostBack="true"
                                    DataTextField="CustName" DataValueField="lid" DataSourceID="CustNameDataSource" AppendDataBoundItems="true">
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    <%--<asp:ListItem Text="SAVITA OIL TECHNOLOGIES LIMITED" Value="17445"></asp:ListItem>
                                    <asp:ListItem Text="SAVITA POLYMERS LTD" Value="17457"></asp:ListItem>--%>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkPortXls" runat="server" OnClick="lnkPortXls_Click">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                                </asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>

                <div class="clear">
                    <asp:GridView ID="gvStockReport" runat="server" AutoGenerateColumns="False" CssClass="table"
                    DataSourceID="StockReportSqlDataSource" OnRowDataBound="gvStockReport_RowDataBound" >
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="BS Job No">
                                <ItemTemplate>
                                    <asp:Label ID="lblJobNumber" Text='<%#Eval("JobRefNo") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                           <asp:TemplateField HeaderText="Customer Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblDescription" Text='<%#Eval("CustName") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="BE Type">
                                <ItemTemplate>
                                    <asp:Label ID="lblBEType" Text='<%#Eval("BETypeName") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description">
                                <ItemTemplate>
                                    <asp:Label ID="lblDescription" Text='<%#Eval("Description") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Quantity" >
                                <ItemTemplate>
                                    <asp:Label ID="lblQuantity" Text='<%#Eval("Quantity") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Delivery" >
                                <ItemTemplate>
                                    <asp:Label ID="lblQuantity" Text='<%#Eval("Delivery") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Balance">
                                <ItemTemplate>
                                    <asp:Label ID="lblBalance" Text='<%#Eval("Balance") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="BOE Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblExportJob" Text='<%#Eval("BOEDate") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="BOE No">
                                <ItemTemplate>
                                    <asp:Label ID="lblExportJob" Text='<%#Eval("BOENo") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ExBond BE Type">
                                <ItemTemplate>
                                    <asp:Label ID="lblExportJob" Text='<%#Eval("ExBETypeName") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ExBond Job No">
                                <ItemTemplate>
                                    <asp:Label ID="lblExportJob" Text='<%#Eval("ExBondJobRefNo") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ExBond Description">
                                <ItemTemplate>
                                    <asp:Label ID="lblExportJob" Text='<%#Eval("ExBondDescription") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ExBond Delivery">
                                <ItemTemplate>
                                    <asp:Label ID="lblExportJob" Text='<%#Eval("ExDelivery") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ExBond Balance">
                                <ItemTemplate>
                                    <asp:Label ID="lblExportDesc" Text='<%#Eval("EXBalance") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ExBond BOE Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblExportBEType" Text='<%#Eval("ExBOEDate") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ExBond BOE No">
                                <ItemTemplate>
                                    <asp:Label ID="lblExportQuantity" Text='<%#DefaultVal(Eval("ExBOENo").ToString()) %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                        </Columns>
                    </asp:GridView>
                </div>

                <div id="divSqlDataSource">
                <asp:SqlDataSource ID="StockReportSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="StockReport" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:ControlParameter ControlID="ddlCustomer" PropertyName="SelectedValue" Name="CustId" DefaultValue="0" />
                        <asp:SessionParameter Name="FinYr" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="CustNameDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="BS_GetCustomerInfo" SelectCommandType="StoredProcedure">
                </asp:SqlDataSource>
                </div>
            </fieldset>


        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

