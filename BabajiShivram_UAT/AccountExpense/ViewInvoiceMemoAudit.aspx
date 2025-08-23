<%@ Page Title="Invoice Memo Audit" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="ViewInvoiceMemoAudit.aspx.cs" Inherits="AccountExpense_ViewInvoiceMemoAudit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <div align="center">
        <asp:Label ID="lblMessage" runat="server" CssClass="errorMsg" Font-Size="Large"></asp:Label>
    </div>
    <fieldset>
        <div class="fleft">
            <asp:Button ID="btnRejectMemo" runat="server" Text="Reject Memo" OnClick="btnRejectMemo_Click" OnClientClick="return confirm('Sure to Reject Memo ?');" />
            <asp:Button ID="btnApproveAuditMemo" runat="server" Text="Audit Invoice Memo For Payment" OnClick="btnApproveAuditMemo_Click" OnClientClick="return confirm('Sure to Complete Audit ?');" />
        </div>
        <div class="clear"></div>
    <fieldset>
        <legend>Invoice Memo Detail</legend>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
            <tr>
                <td>
                    Memo Ref No
                </td>
                <td >
                    <asp:Label ID="lblMemoRefNo" runat="server"></asp:Label>
                </td>
                <td>
                    Vendor
                </td>
                <td>
                    <asp:Label ID="lblVendorName" runat="server" ></asp:Label>
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
                </td>
            </tr>
            <tr>
                <td>Remark</td>
                <td>
                    <asp:TextBox ID="txtRemark" runat="server" MaxLength="200" Columns="3" TextMode="MultiLine"></asp:TextBox>

                </td>
                <td>
                    Profit Excel
                </td>
                <td>
                    <asp:FileUpload ID="fuProfitDoc" runat="server" />
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset><legend>Job Detail</legend>
        <div class="m clear"></div>
        <asp:Button ID="btnCheckBJVExpense" runat="server" Text="Check BJV Expense" OnClick="btnCheckBJVExpense_Click" OnClientClick="return confirm('Sure to Check BJV Detail ?');" />
        <asp:GridView ID="gvDetail" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="InvoiceMemoId"
            AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataSourceID="InvoiceSqlDataSource" 
            PageSize="50" AllowPaging="True" AllowSorting="True" PagerSettings-Position="TopAndBottom" Width="100%" 
            OnRowCommand="gvDetail_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PayMemoRefNo" HeaderText="Memo Ref No" SortExpression="PayMemoRefNo" Visible="false" />
                <asp:BoundField DataField="FARefNo" HeaderText="Job No" />
                <asp:TemplateField HeaderText="View BJV" SortExpression="FARefNo">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBJVNo" CommandName="ViewBJV" runat="server" Text="View BJV"
                            CommandArgument='<%#Eval("FARefNo") %>' ToolTip="Check BJV Detail" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="BilledStatus" HeaderText="Billed ?" SortExpression="BilledStatus" />
                <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" SortExpression="InvoiceNo" />
                <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" SortExpression="InvoiceDate" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName" />
                <%--<asp:BoundField DataField="StatusDate" HeaderText="Status Date" SortExpression="StatusDate" DataFormatString="{0:dd/MM/yyyy}"/>--%>
                <asp:BoundField DataField="ExpenseTypeName" HeaderText="Expense Type" SortExpression="ExpenseTypeName" />
                <asp:BoundField DataField="JobProfit" HeaderText="Job Profit" SortExpression="JobProfit" />
                <asp:BoundField DataField="BJVJBNumber" HeaderText="JB No" SortExpression="BJVJBNumber" />
                <asp:BoundField DataField="ToPay" HeaderText="Net Payable" SortExpression="ToPay" />
                <asp:BoundField DataField="TDSTotalAmount" HeaderText="TDS" SortExpression="TDSTotalAmount" />
                <asp:BoundField DataField="OtherDeduction" HeaderText="Deduction" SortExpression="OtherDeduction" />
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
                <asp:Label ID="lblRemarkView" runat="server" Text='<%#Eval("Remark") %>'></asp:Label>
                </div>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField HeaderText="User" DataField="UserName" />
        <asp:BoundField HeaderText="Date" DataField="CreatedDate" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" />
        </Columns>
        </asp:GridView>
        </fieldset>
    </fieldset>
    <!---------------------BJV Detail Start --------------------->
    <div id="BJV_Popup">
<asp:LinkButton ID="lnkModelPopupBJV" runat="server" />
<cc1:ModalPopupExtender ID="ModalPopupExtenderBJV" runat="server" TargetControlID="lnkModelPopupBJV"
    PopupControlID="PanelBJV">
</cc1:ModalPopupExtender>
<asp:Panel ID="PanelBJV" Style="display: none" runat="server">
    <fieldset>
        <legend>BJV DETAIL</legend>
        <div class="header">
            <div class="fright">
                <asp:ImageButton ID="btnCancelBJVPopup2" ImageUrl="~/Images/delete.gif" runat="server"
                    OnClick="btnCancelBJVPopup2_Click" ToolTip="Close" />
            </div>
        </div>
        <div class="clear"></div>
        <div id="Div1" runat="server" style="max-height: 560px; overflow: auto;">
            <asp:GridView ID="gvBJVDetail" runat="server" CssClass="table" AutoGenerateColumns="false" AllowPaging="false"
                OnPreRender="gvBJVDetail_PreRender">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex +1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Ref" HeaderText="Job No" />
                    <asp:BoundField DataField="Type" HeaderText="Type" />
                    <asp:BoundField DataField="BookName" HeaderText="Book Name" />
                    <asp:BoundField DataField="billno" HeaderText="Bill No" />
                    <asp:BoundField DataField="billdate" HeaderText="Bill Date" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="par_code" HeaderText="Party Code" />
                    <asp:BoundField DataField="Debit" HeaderText="Debit" />
                    <asp:BoundField DataField="Credit" HeaderText="Credit" />
                    <asp:TemplateField HeaderText="Narration">
                    <ItemTemplate>
                        <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                        <asp:Label ID="lblNarration" runat="server" 
                            Text='<%# Eval("narration")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </fieldset>
</asp:Panel>
</div>
    <!----------------------------------------------------------->
    <div>
        <asp:SqlDataSource ID="InvoiceSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="INV_GetInvoiceByMemoId" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="InvoiceMemoID" SessionField="InvoiceMemoAuditId" />
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceStatusHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="INV_GetMemoHistory" SelectCommandType="StoredProcedure">
            <SelectParameters>
            <asp:SessionParameter Name="InvoiceMemoId" SessionField="InvoiceMemoAuditId" />
            <asp:SessionParameter Name="UserId" SessionField="UserId" />                
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
</asp:Content>


