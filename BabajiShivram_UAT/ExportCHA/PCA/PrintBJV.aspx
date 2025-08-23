<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintBJV.aspx.cs" Inherits="PCA_PrintBJV" Culture="en-GB" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BJV Detail</title>
    
    <script type="text/javascript">
        function CallPrint() {
            //Get the print button and put it into a variable
            var printButton = document.getElementById("btnPrint");

            //Set the print button visibility to 'hidden' 
            printButton.style.visibility = 'hidden';

            //Print the page content
            window.print()

            //Set the print button to 'visible' again 
            //[Delete this line if you want it to stay hidden after printing]
            printButton.style.visibility = 'visible';
        }
    </script>
    <STYLE TYPE='text/css'>
        P.pagebreakhere {page-break-after: always}
    </STYLE>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Button ID="btnPrint" runat="server" Text="Print"
                OnClientClick="Javascript:CallPrint()" />
        <div>
            <asp:Repeater ID="rptBJVDetails" runat="server" OnItemDataBound="rptBJVDetails_ItemDataBound">
                <ItemTemplate>
                    <div>
                        Job No - &nbsp;<asp:Label ID="lblJobRefNo" Text ='<%# Eval("JobRefNo")%>' runat="server"></asp:Label>
                        &nbsp;<asp:Label ID="lbljobDate" Text ='<%# Eval("CreatedDate", "{0:dd/MM/yyyy}")%>' runat="server"></asp:Label>
                    </div>
                   <div style="float:right">
                       <%# DateTime.Now.ToShortDateString() %> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                   </div>
                     <div>
                        A/C Name - &nbsp;<asp:Label ID="lblCustomer" runat="server" Text='<%#Eval("Customer")%>'></asp:Label>
                    </div>
                   
                     <br />
                    
                    <asp:GridView ID="gvjobexpenseDetail" runat="server" AutoGenerateColumns="False" ShowFooter="true"
                            CssClass="table" Width="99%" CellPadding="4">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="DATE" DataField="VCHDATE" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />                                        
                                    <asp:BoundField HeaderText="VCH No" DataField="VCHNO" />
                                    <asp:BoundField HeaderText="Narration" DataField="NARRATION"/>
                                    <asp:BoundField HeaderText="Cheque No" DataField="CHQNO" />
                                    <asp:BoundField HeaderText="Debit" DataField="DEBITAMT" />
                                    <asp:BoundField HeaderText="Credit" DataField="CREDITAMT"/>
                                </Columns>
                            </asp:GridView>                            
                    <br /><br />
                    
                </ItemTemplate>
                
            </asp:Repeater>
                
            <asp:SqlDataSource ID="DataSourceExpenseDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="GetBJVDetails" SelectCommandType="StoredProcedure">
                <SelectParameters>
                <asp:Parameter Name="JobId" DefaultValue="0" />                                
                </SelectParameters>                                  
            </asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
