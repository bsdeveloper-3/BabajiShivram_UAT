<%@ Page Title="Customer Credit Reminder" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
        CodeFile="CustomerCreditReminder.aspx.cs" Inherits="Service_CustomerCreditReminder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "../images/minus.png");
        });
        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "../images/plus.png");
            $(this).closest("tr").next().remove();
        });
    </script>
    <div>
        <fieldset><legend>Out Standing Payment Reminder</legend>
        <asp:GridView ID="gvCustomers" runat="server" AutoGenerateColumns="False" CssClass="table"
            DataKeyNames="CustomerName" OnRowDataBound="OnRowDataBound" >
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <img alt="" style="cursor: pointer" src="../images/plus.png" />

                        <asp:Panel ID="pnlHistory" runat="server" Style="display: none">
                            <asp:GridView ID="gvCredit" runat="server" AutoGenerateColumns="false" CssClass="table"
                                CellPadding="4" ForeColor="#333333" GridLines="None">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="OSSentDate" HeaderText="OS Sent Date" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="TotalDue" HeaderText="Total Due" />
                                    <asp:BoundField DataField="OverDue" HeaderText="Over Due" />
                                    <asp:BoundField DataField="DisputedAmount" HeaderText="Disputed Amt" />
                                    <asp:BoundField DataField="BSCCPLRemark" HeaderText="Remark" />
                                    <asp:BoundField DataField="ClientRemark" HeaderText="Client Remark" />
                                </Columns>
                                <EditRowStyle BackColor="#999999" />
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                            </asp:GridView>
                        </asp:Panel>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                <asp:BoundField DataField="OSSentDate" HeaderText="OS Sent Date" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField DataField="TotalDue" HeaderText="Total Due" />
                <asp:BoundField DataField="OverDue" HeaderText="Over Due" />
                <asp:BoundField DataField="DisputedAmount" HeaderText="Disputed Amt" />
                <asp:BoundField DataField="BSCCPLRemark" HeaderText="Remark"></asp:BoundField>
                <asp:BoundField DataField="ClientRemark" HeaderText="Client Remark"></asp:BoundField>
            </Columns>
            
        </asp:GridView>
        </fieldset>
    </div>
</asp:Content>

