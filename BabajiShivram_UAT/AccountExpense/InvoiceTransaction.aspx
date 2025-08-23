<%@ Page Title="Vendor Invoice Bank Transaction" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="InvoiceTransaction.aspx.cs" Inherits="AccountExpense_InvoiceTransaction" EnableEventValidation="false" Culture="en-GB"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Scriptmanager id="ScriptManager1" runat="server"></asp:Scriptmanager>
    <fieldset><legend>Vendor Invoice - Fund Transfer</legend>
        <div align="center">
            <asp:Label ID="lblMessage" runat="server" CssClass="errorMsg" EnableViewState="false" ></asp:Label>
        </div>
        
        <div class="clear">
            <asp:Panel ID="pnlFilter" runat="server">
                <div class="fleft">
                    <uc1:DataFilter ID="DataFilter1" runat="server" />
                </div>
                <div class="fleft" style="margin-left:30px; padding-top:3px;">
                    <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                        <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnCheckStatus" runat="server" Text="Check Current Status" OnClick="btnCheckStatus_Click" />
                </div>
            </asp:Panel>
        </div>
        <div class="m clear"></div>
        <asp:GridView ID="gvDetail" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="lid"
            AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataSourceID="InvoiceSqlDataSource" 
            AllowPaging="True" AllowSorting="True" PageSize="80" PagerSettings-Position="TopAndBottom"
            Width="100%" OnRowCommand="gvDetail_RowCommand" OnRowDataBound="gvDetail_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Job No" SortExpression="JobRefNo">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkJobRefNo" CommandName="select" runat="server" Text='<%#Eval("FARefNo") %>'
                            CommandArgument='<%#Eval("InvoiceId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="FARefNo" HeaderText="Job No" Visible="False" />
                <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" SortExpression="InvoiceNo"/>
                <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" SortExpression="InvoiceDate" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />
                <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" SortExpression="VendorName" />
                <asp:BoundField DataField="UniqueReferenceNo" HeaderText="UTR No" SortExpression="UniqueReferenceNo" />
                <asp:BoundField DataField="RespStatus" HeaderText="Status" SortExpression="RespStatus" />
                <asp:BoundField DataField="CreditorAccountNo" HeaderText="AccountNo" SortExpression="CreditorAccountNo" />
                <asp:BoundField DataField="CreditorAccountIFSC" HeaderText="IFSC" SortExpression="CreditorAccountIFSC" />
                <asp:BoundField DataField="ReqDate" HeaderText="Request Date" SortExpression="ReqDate" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField DataField="RespDate" HeaderText="Update Date" SortExpression="RespDate" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField DataField="UserName" HeaderText="User" SortExpression="UserName"/>
                <asp:TemplateField HeaderText="Move">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkMove" CommandName="MoveToBankTransfer" runat="server" Text="Move To Payment" 
                            OnClientClick="return confirm('Are you sure to Move Bank Transfer Tab?')"
                            CommandArgument='<%# Eval("PaymentRequestId") %>' Visible="false" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status">
    <ItemTemplate>
        <asp:LinkButton ID="lnkCheckStatus" CommandName="CheckStatus" runat="server" Text="Check Status" 
            OnClientClick="return confirm('Are you sure to Check Status?')"
            CommandArgument='<%# Eval("ReqReferenceNo") %>' />
    </ItemTemplate>
</asp:TemplateField>
            </Columns>
            <PagerTemplate>
                <asp:GridViewPager runat="server" />
            </PagerTemplate>
        </asp:GridView>
        </fieldset>
    <div id="popupStatus">
<asp:LinkButton ID="lnkModelPopup21" runat="server" />
<cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="lnkModelPopup21"
    PopupControlID="Panel21">
</cc1:ModalPopupExtender>
<asp:Panel ID="Panel21" Style="display: none" runat="server">
    <fieldset>
        <legend>
            <asp:Label ID="lblPopupMessage" runat="server" Text="Payment Status"></asp:Label>
        </legend>
            
            <div class="header">
            <div class="fleft"></div>
            <div class="fright">
                <asp:ImageButton ID="ImageButton1" ImageUrl="~/Images/delete.gif" runat="server"
                    OnClick="btnCancelPopup_Click" ToolTip="Close" />
            </div>
            </div>
            <div class="clear"></div>
            <div id="Div1" runat="server" style="max-height: 550px; overflow: auto;">
            <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
                <tr>
                    <td>
                        Status
                    </td>
                    <td>
                        <asp:Label ID="lblCHKStatus" runat="server"></asp:Label>
                    </td>
                    <td>
                        Status Date
                    </td>
                    <td>
                        <asp:Label ID="lblCHKStatusDate" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        UTR No
                    </td>
                    <td>
                        <asp:Label ID="lblCHKStatusUTR" runat="server"></asp:Label>
                    </td>
                    <td></td>
                    <td>
                        <asp:Label ID="lblCHKStatusUTRDate" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Message
                    </td>
                    <td colspan="3">
                        <asp:Label ID="lblCHKStatusMessage" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </fieldset>
</asp:Panel>
</div>
        <div>
            <asp:SqlDataSource ID="InvoiceSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="AC_GetAPIBankTransaction" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="UserId" SessionField="UserId" />
                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
</asp:Content>
