<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Customer_CustomerFieldReport.aspx.cs" Inherits="Reports_Customer_CustomerFieldReport" 
MasterPageFile="~/CustomerMaster.master" Title="Report" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="content1" runat="server">
 <div align="center" >
            <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
        </div>
        
        <div class="m clear"></div>
                <asp:GridView ID="GridViewJobReport" runat="server" AutoGenerateColumns="True" CssClass="table"
                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr"
                    CellPadding="4" AllowPaging="True" PageSize="40">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
</asp:Content>

