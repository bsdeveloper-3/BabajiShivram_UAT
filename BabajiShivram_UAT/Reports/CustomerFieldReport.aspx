<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomerFieldReport.aspx.cs" 
 MasterPageFile="~/MasterPage.master" Inherits="Reports_CustomerFieldReport" Title="Additional Field Report"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
        <div align="center" >
            <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
        </div>
        <div>    
            <div class="m clear">
                   <asp:Button ID="btnShowReport" Text="Show Report" OnClick="btnShowReport_Click" runat="server" />
                   <%--<asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False"  />--%>
                            
                        </div>     
            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                <tr>
                    <td>
                        Customer 
                     </td>
                    <td >
                        <asp:DropDownList ID="ddCustomer" runat="server" CssClass="DropDownBox">
                            </asp:DropDownList>
                    </td>
                    
                </tr>
            </table>
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
        </div>
</asp:Content>