<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomerReport.aspx.cs" 
    Inherits="Reports_CustomerReport" MasterPageFile="~/CustomerMaster.master" Title="Report" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div align="center">
        <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
    </div>
    <fieldset>
    <legend>Job Report</legend>
    <div>
        <asp:Button ID="btnSubmit" Text="Download To Excel" runat="server" OnClick="lnkexport_Click" TabIndex="2" />
    </div>
    <div class="m"> 
    </div>
    <%--<div class="fright">
        <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
            <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
        </asp:LinkButton>
    </div>--%>
      <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
        <tr>
            <td>
                Shipment Type
            </td>
            <td class="m">
                <asp:DropDownList ID="ddShipmentType" runat="server" AutoPostBack="true" TabIndex="1">
                    <asp:ListItem Value="1" Text="Cleared" ></asp:ListItem>
                    <asp:ListItem Value="0" Text="Un-Cleared" ></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
          
    </table>
    </fieldset>    
            <asp:GridView ID="gvReportField" runat="server" AutoGenerateColumns="true" Width="100%" CssClass="table"
                CellPadding="4" PagerSettings-Position="TopAndBottom" 
               DataSourceID="ReportSqlDataSource" Visible="false">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %> 
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField DataField="InvoiceNoDate" HtmlEncode="False" />--%>
                </Columns>
            </asp:GridView>
        <div id="divSqlDataSource">
            <asp:SqlDataSource ID="ReportSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="rptCustomerUserReport" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter SessionField="CustUserId" Name="CustomerUserId" />
                    <asp:ControlParameter ControlID="ddShipmentType" Name="DeliveryStatus" PropertyName="SelectedValue" />
                </SelectParameters>
                </asp:SqlDataSource>
        </div>
</asp:Content>
