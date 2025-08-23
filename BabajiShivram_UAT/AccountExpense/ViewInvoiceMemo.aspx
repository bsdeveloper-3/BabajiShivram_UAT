<%@ Page Title="Invoice Memo Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
        CodeFile="ViewInvoiceMemo.aspx.cs" Inherits="AccountExpense_ViewInvoiceMemo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <div align="center">
        <asp:Label ID="lblMessage" runat="server" CssClass="errorMsg" Font-Size="Large"></asp:Label>
    </div>
    <fieldset> <legend>Memo Detail</legend>
        <div class="clear"></div>
    <fieldset>
        <legend>Invoice Memo Detail</legend>
        <asp:Button ID="btnUpdateMemoRemark" runat="server" Text="Update Memo Remark" OnClick="btnUpdateMemoRemark_Click" OnClientClick="return confirm('Sure to Update Memo ?');" />
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
                    Document
                </td>
                <td>
                    <asp:LinkButton ID="lnkViewDoc" runat="server" OnClick="lnkViewDoc_Click" Text="View Doc"></asp:LinkButton>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:FileUpload ID="fuProfitDoc" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Remark
                </td>
                <td colspan="4">
                    <asp:TextBox ID="txtRemark" runat="server" MaxLength="200" Columns="3" TextMode="MultiLine"></asp:TextBox>
                    <%--<asp:Label ID="lblRemark" runat="server" ></asp:Label>--%>
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset><legend>Job Detail</legend>
        <div class="m clear"></div>
        <asp:Button ID="btnCheckBJVExpense" runat="server" Text="Check BJV Expense" OnClick="btnCheckBJVExpense_Click" OnClientClick="return confirm('Sure to Check BJV Detail ?');" />
        <asp:GridView ID="gvDetail" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="InvoiceMemoId"
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
                <%--<asp:BoundField DataField="FARefNo" HeaderText="Job No" />--%>
                <asp:TemplateField HeaderText="Job No" SortExpression="FARefNo">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBJVAdjustment" CommandName="ViewAdjustment" runat="server" Text='<%#Eval("FARefNo") %>'
                            CommandArgument='<%#Eval("FARefNo") %>' ToolTip="Check Payment Detail" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="View BJV" SortExpression="FARefNo">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBJVNo" CommandName="ViewBJV" runat="server" Text="View BJV"
                            CommandArgument='<%#Eval("FARefNo") %>' ToolTip="Check BJV Detail" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                <asp:BoundField DataField="BilledStatus" HeaderText="Billed ?" SortExpression="BilledStatus" />
                <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" SortExpression="InvoiceNo" />
                <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" SortExpression="InvoiceDate" DataFormatString="{0:dd/MM/yyyy}"/>
<%--                <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName" />
                <asp:BoundField DataField="updDate" HeaderText="Status Date" SortExpression="updDate" DataFormatString="{0:dd/MM/yyyy hh:mm tt}"/>--%>
                <asp:BoundField DataField="ExpenseTypeName" HeaderText="Expense Type" SortExpression="ExpenseTypeName" />
                <asp:BoundField DataField="BJVJBNumber" HeaderText="JB No" SortExpression="BJVJBNumber" />
                <asp:BoundField DataField="InvoiceAmount" HeaderText="Invoice Value" SortExpression="InvoiceAmount" />
                <%--<asp:BoundField DataField="TaxAmount" HeaderText="Invoice Taxbl Value" SortExpression="TaxAmount" />--%>
                <asp:BoundField DataField="JobProfit" HeaderText="Job Profit" SortExpression="JobProfit" />
                <%--<asp:BoundField DataField="VendorSellValue" HeaderText="Sell Value" SortExpression="VendorSellValue"/>
                <asp:BoundField DataField="JobMargin" HeaderText="Margin" SortExpression="JobMargin" DataFormatString="{0:F}"/>
                <asp:BoundField DataField="PercentMargin" HeaderText="Margin %" SortExpression="PercentMargin" DataFormatString="{0:F}"/>--%>
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
    <!-------------------- BJV ADJUSTMENT Start ---------------->
    <div id="BJV_PopupAdjustm">
        <asp:LinkButton ID="lnkModelPopup21" runat="server" />
        <cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="lnkModelPopup21"
                    PopupControlID="Panel21">
                </cc1:ModalPopupExtender>
        <asp:Panel ID="Panel21" Style="display: none" runat="server">
        <fieldset>
            <legend>Payment Detail</legend>
            <div class="header">
                <div class="fright">
                    <asp:ImageButton ID="imgModelClose" ImageUrl="~/Images/delete.gif" runat="server"
                        OnClick="btnCancelBJVPopup_Click" ToolTip="Close" />
                </div>
            </div>
                <div class="clear"></div>
            <div id="Div3" runat="server" style="max-height: 560px; overflow: auto;">
            <asp:GridView ID="gvBJVAdjustment" runat="server" CssClass="table" AutoGenerateColumns="false" AllowPaging="false">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex +1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Ref" HeaderText="Job No" />
                    <asp:BoundField DataField="BillNo" HeaderText="Bill No" />
                    <asp:BoundField DataField="PAR_CODE" HeaderText="PAR_CODE" />
                    <asp:BoundField DataField="ADJNO" HeaderText="ADJ NO" />
                    <asp:BoundField DataField="ADJDATE" HeaderText="ADJ DATE" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="ADJAMT" HeaderText="Amount" />
                </Columns>
            </asp:GridView>
            </div>
        </fieldset>
    </asp:Panel>
    </div>
    <!---------------------BJV ADJUSTMENT END------------->
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
                <asp:SessionParameter Name="InvoiceMemoID" SessionField="InvoiceMemoViewId" />
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceStatusHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="INV_GetMemoHistory" SelectCommandType="StoredProcedure">
            <SelectParameters>
            <asp:SessionParameter Name="InvoiceMemoId" SessionField="InvoiceMemoViewId" />
            <asp:SessionParameter Name="UserId" SessionField="UserId" />                
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
</asp:Content>
