<%@ Page Title="Memo Audit" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="ViewTransMemoAudit.aspx.cs" Inherits="AccountTransport_ViewTransMemoAudit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div align="center">
        <asp:Label ID="lblMessage" runat="server" CssClass="errorMsg" Font-Size="Large"></asp:Label>
    </div>
    <fieldset>
        <div class="fleft">
            <asp:Button ID="btnRejectMemo" runat="server" Text="Reject Memo" OnClick="btnRejectMemo_Click" ValidationGroup="Required" OnClientClick="return confirm('Sure to Cancel Memo ?');" />
            <asp:Button ID="btnApproveAuditMemo" runat="server" Text="Audit Transport Memo For Mgmt Approval" ValidationGroup="Required" OnClick="btnApproveAuditMemo_Click" OnClientClick="return confirm('Sure to Complete Audit ?');" />
        </div>
        <div class="clear"></div>
    <fieldset>
        <legend>Transporter Memo Detail</legend>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
            <tr>
                <td>
                    Memo Ref No
                </td>
                <td >
                    <asp:Label ID="lblMemoRefNo" runat="server"></asp:Label>
                </td>
                <td>
                    Transporter
                </td>
                <td>
                    <asp:Label ID="lblTransporterName" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Total Amount
                </td>
                <td>
                    <asp:Label ID="lblTotalAmount" runat="server"></asp:Label>
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
                <td colspan="3">
                    <asp:TextBox ID="txtRemark" runat="server" MaxLength="100" TextMode="MultiLine"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RFVRemark" ValidationGroup="Required" runat="server" Display="Dynamic"
                        ControlToValidate="txtRemark" InitialValue="" ErrorMessage="Required" Text="*"></asp:RequiredFieldValidator>
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset><legend>Job Detail</legend>
        <div class="m clear"></div>
        <asp:GridView ID="gvDetail" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="MemoId"
            AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataSourceID="InvoiceSqlDataSource" 
            AllowPaging="True" AllowSorting="True" PagerSettings-Position="TopAndBottom" Width="100%" >
        <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PayMemoRefNo" HeaderText="Memo Ref No" SortExpression="PayMemoRefNo" Visible="false" />
                <asp:BoundField DataField="JobRefNo" HeaderText="Job No" />
                <asp:BoundField DataField="TansBilledStatus" HeaderText="Billed ?" SortExpression="TansBilledStatus" />
                <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" SortExpression="InvoiceNo" />
                <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" SortExpression="InvoiceDate" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName" />
                <asp:BoundField DataField="StatusDate" HeaderText="Status Date" SortExpression="StatusDate" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField DataField="ExpenseTypeName" HeaderText="Expense Type" SortExpression="ExpenseTypeName" />
                <asp:BoundField DataField="BJVJBNumber" HeaderText="JB No" SortExpression="BJVJBNumber" />
                <asp:BoundField DataField="ToPay" HeaderText="Net Payable" SortExpression="ToPay" />
                <asp:BoundField DataField="ProfitLossPercent" HeaderText="P&L %" SortExpression="ProfitLossPercent" />
                <asp:BoundField DataField="PROFIT" HeaderText="Is Profit" SortExpression="PROFIT" />
                <asp:BoundField DataField="SellValue" HeaderText="Sell Value" SortExpression="SellValue" />
                <asp:BoundField DataField="TDSTotalAmount" HeaderText="TDS" SortExpression="TDSTotalAmount" />
                <asp:BoundField DataField="AdvanceAmt" HeaderText="Adv Amt" SortExpression="AdvanceAmt" />
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
                <asp:SessionParameter Name="MemoId" SessionField="MemoAuditId" />
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceStatusHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TRS_GetMemoHistory" SelectCommandType="StoredProcedure">
            <SelectParameters>
            <asp:SessionParameter Name="MemoId" SessionField="MemoAuditId" />
            <asp:SessionParameter Name="UserId" SessionField="UserId" />                
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
</asp:Content>

