<%@ Page Title="Memo Approval" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TransMemoApproval.aspx.cs" 
        Inherits="AccountTransport_TransMemoApproval" EnableEventValidation="false" Culture="en-GB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Scriptmanager id="ScriptManager1" runat="server" scriptmode="Release"> </asp:Scriptmanager>
    <div align="center">
        <asp:Label ID="lblMessage" runat="server" CssClass="errorMsg" Font-Size="Large"></asp:Label>
    </div>
    <div align="center">
        
    </div>
    <fieldset>
        
    <fieldset>
        <legend>Transporter Memo Detail</legend>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
            <tr>
                <td>
                    Transporter
                </td>
                <td>
                    <asp:Label ID="lblTransporterName" runat="server"  Font-Bold="true" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    Memo Ref No
                </td>
                <td >
                    <asp:Label ID="lblMemoRefNo" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Total Amount
                </td>
                <td>
                    <asp:Label ID="lblTotalAmount" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                </td>
                <td>
                    <asp:Label ID="lblLTRefNo" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Remark
                </td>
                <td>
                    <asp:TextBox ID="txtRemark" runat="server" MaxLength="100" TextMode="MultiLine"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RFVRemark" ValidationGroup="Required" runat="server" Display="Dynamic"
                        ControlToValidate="txtRemark" InitialValue="" ErrorMessage="Required" Text="*"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:Button ID="btnApproveMemo" runat="server" Text="Approve Transport Memo" OnClick="btnApproveMemo_Click" 
                       ValidationGroup="Required" OnClientClick="return confirm('Sure to Approve Memo ?');" />
                </td>
                <td>
                    <asp:Button ID="btnRejectMemo" runat="server" Text="Reject Transport Memo" OnClick="btnRejectMemo_Click" 
                        ValidationGroup="Required" OnClientClick="return confirm('Sure to Reject Memo ?');" />
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset><legend runat="server">Job Detail &nbsp;
        <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
            <asp:Image ID="imgExcel" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
        </asp:LinkButton>
        </legend>
        <asp:GridView ID="gvDetail" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="MemoId"
            AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataSourceID="InvoiceSqlDataSource" 
             AllowPaging="True" AllowSorting="True" PagerSettings-Position="TopAndBottom" Width="100%" 
            OnRowCommand="gvDetail_RowCommand">
        <Columns>
            <asp:TemplateField HeaderText="Sl">
                <ItemTemplate>
                    <%#Container.DataItemIndex +1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="PayMemoRefNo" HeaderText="Memo Ref No" SortExpression="PayMemoRefNo" Visible="false" />
            <asp:BoundField DataField="JobRefNo" HeaderText="Job No" SortExpression="JobRefNo"/>
            <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer"/>
            <asp:BoundField DataField="TansBilledStatus" HeaderText="Billed ?" SortExpression="TansBilledStatus" />
            <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" SortExpression="InvoiceNo" />
            <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" SortExpression="InvoiceDate" DataFormatString="{0:dd/MM/yyyy}"/>
            <%--<asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName" />--%>
            <%--<asp:BoundField DataField="StatusDate" HeaderText="Audit Date" SortExpression="StatusDate" DataFormatString="{0:dd/MM/yyyy}"/>--%>
            <asp:BoundField DataField="ExpenseTypeName" HeaderText="Expense Type" SortExpression="ExpenseTypeName" />
            <asp:BoundField DataField="BJVJBNumber" HeaderText="JB No" SortExpression="BJVJBNumber" />
            <asp:BoundField DataField="Amount" HeaderText="Bill Amt" SortExpression="Amount" />
            <asp:BoundField DataField="DetentionCharges_T26" HeaderText="Detention" SortExpression="DetentionCharges_T26" />
            <asp:BoundField DataField="AdvanceAmt" HeaderText="Adv Paid" SortExpression="AdvanceAmt" />
            <asp:BoundField DataField="TDSTotalAmount" HeaderText="TDS" SortExpression="TDSTotalAmount" />
            <asp:BoundField DataField="OtherDeduction" HeaderText="Deduction" SortExpression="OtherDeduction" />
            <asp:BoundField DataField="ToPay" HeaderText="Net Payable" SortExpression="ToPay" />
            <asp:TemplateField HeaderText="Download">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                        CommandArgument='<%#Bind("RequestId") %>'></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="View">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkView" runat="server" Text="View" CommandName="View" 
                        CommandArgument='<%#Bind("RequestId") %>'></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <%--<asp:BoundField DataField="ProfitLossPercent" HeaderText="P&L %" SortExpression="ProfitLossPercent" />
            <asp:BoundField DataField="PROFIT" HeaderText="Is Profit" SortExpression="PROFIT" />--%>
            <%--<asp:BoundField DataField="SellValue" HeaderText="Sell Value" SortExpression="SellValue" />--%>
            <%--<asp:BoundField DataField="RequestBy" HeaderText="User" SortExpression="RequestBy" />
            <asp:BoundField DataField="dtDate" HeaderText="Date" SortExpression="Date" DataFormatString="{0:dd/MM/yyyy}"/>--%>
        </Columns>
    </asp:GridView>
    </fieldset>
    <fieldset>
        <legend>History</legend>
        <asp:GridView ID="gvMemoHistory" runat="server" AutoGenerateColumns="False"
            CssClass="table" Width="90%" PagerStyle-CssClass="pgr"
            DataKeyNames="lId" DataSourceID="DataSourceStatusHistory" CellPadding="4"
            AllowPaging="True" AllowSorting="True" PageSize="40">
            <Columns>
        <asp:TemplateField HeaderText="Sl">
            <ItemTemplate>
                <%#Container.DataItemIndex + 1 %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField HeaderText="Status" DataField="StatusName" />
        <%--<asp:BoundField HeaderText="Remark" DataField="sRemark" />--%>
        <asp:TemplateField HeaderText="Remark">
            <ItemTemplate>
                <div style="word-wrap: break-word; width: 400px; white-space:normal;">
                <asp:Label ID="lblRemarkView" runat="server" Text='<%#Eval("sRemark") %>'></asp:Label>
                </div>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField HeaderText="User" DataField="UserName" />
        <asp:BoundField HeaderText="Date" DataField="CreatedDate" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" />
        </Columns>
        </asp:GridView>
        </fieldset>
    </fieldset>
    <div>
        <asp:SqlDataSource ID="InvoiceSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TRS_GetInvoiceByMemoId" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="MemoId" SessionField="MemoApproveId" />
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceStatusHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TRS_GetMemoHistory" SelectCommandType="StoredProcedure">
            <SelectParameters>
            <asp:SessionParameter Name="MemoId" SessionField="MemoApproveId" />
            <asp:SessionParameter Name="UserId" SessionField="UserId" />                
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
</asp:Content>

