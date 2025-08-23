<%@ Page Title="Payment Receipt" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
        CodeFile="PendingInvoicePaymentReceipt.aspx.cs" Inherits="AccountExpense_PendingInvoicePaymentReceipt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <fieldset><legend>Upload Payment Receipt</legend>
        <div align="center">
            <asp:Label ID="lblMessage" runat="server" CssClass="errorMsg" EnableViewState="false" ></asp:Label>
            <asp:HiddenField ID="hdnInvoiceId" runat="server" />
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
                </div>
            </asp:Panel>
        </div>
        <div class="m clear"></div>
        <asp:GridView ID="gvDetail" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="InvoiceId"
            AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataSourceID="InvoiceSqlDataSource" 
            AllowPaging="True" AllowSorting="True" PageSize="40" PagerSettings-Position="TopAndBottom"
            Width="100%" OnRowCommand="gvDetail_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Job No" SortExpression="FARefNo">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkJobRefNo" CommandName="select" runat="server" Text='<%#Eval("FARefNo") %>'
                            CommandArgument='<%#Eval("InvoiceId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="FARefNo" HeaderText="Job No" Visible="False" />
                <asp:BoundField DataField="ExpenseTypeName" HeaderText="Type" />
                <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" />
                <asp:BoundField DataField="InvoiceTypeName" HeaderText="Invoice Type" SortExpression="InvoiceTypeName" />
                <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" SortExpression="VendorName" />
                <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                <asp:BoundField DataField="Consignee" HeaderText="Consignee" SortExpression="Consignee" />
                <asp:BoundField DataField="TotalAmount" HeaderText="Total Value" SortExpression="TotalAmount" />
                <asp:BoundField DataField="InstrumentDate" HeaderText="Payment Date" SortExpression="InstrumentDate" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField DataField="RequestedBy" HeaderText="Requested By" SortExpression="RequestedBy" />
            </Columns>
            <PagerTemplate>
                <asp:GridViewPager runat="server" />
            </PagerTemplate>
        </asp:GridView>
        </fieldset>

            <%--  START : MODAL POP-UP FOR Upload-  --%>

            <div>
                <asp:LinkButton ID="lnkRejectExp" runat="server"></asp:LinkButton>
            </div>

            <div id="divOpExpense">
                <cc1:ModalPopupExtender ID="RejectModalPopupExtender" runat="server" CacheDynamicResults="false" PopupControlID="panRejectJob" 
                    CancelControlID="imgbtnRejectExp" TargetControlID="lnkRejectExp" BackgroundCssClass="modalBackground" DropShadow="true">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="panRejectJob" runat="server" CssClass="ModalPopupPanel" Width="600px">
                    <div class="header">
                        <div class="fleft">
                            <asp:Label ID="lblPopupName" runat="server" Text="Upload Receipt"></asp:Label>

                        </div>
                        <div class="fright">
                            <asp:ImageButton ID="imgbtnRejectExp" ImageUrl="../Images/delete.gif" runat="server"
                                ToolTip="Close" />
                        </div>
                    </div>
                    <div id="Div2" runat="server" style="max-height: 250px; overflow: auto; padding: 5px">
                        <div>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <div class="m">
                                    <center>
                                        <asp:Label ID="lblError_RejectExp" runat="server"></asp:Label>
                                    </center>
                                </div>
                                <tr>
                                    <td>BS Job Number</td>
                                    <td>
                                        <asp:Label ID="lblJobNumber" runat="server"></asp:Label>
                                    </td>
                                    <td>Branch</td>
                                    <td>
                                        <asp:Label ID="lblBranch1" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Payment Type</td>
                                    <td>
                                        <asp:Label ID="lblPaymentType1" runat="server"></asp:Label>
                                    </td>
                                    <td>Expense Type</td>
                                    <td>
                                        <asp:Label ID="lblexpenseType1" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Amount</td>
                                    <td>
                                        <asp:Label ID="lblAmount1" runat="server"></asp:Label>
                                    </td>
                                    <td>Paid To</td>
                                    <td>
                                        <asp:Label ID="lblPaidTo1" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Created By</td>
                                    <td>
                                        <asp:Label ID="lblCreatedBy1" runat="server"></asp:Label>
                                    </td>
                                    <td>Created Date</td>
                                    <td>
                                        <asp:Label ID="lblCreatedDate1" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <asp:RequiredFieldValidator ID="RFVUpload" runat="server" ControlToValidate="fuReceipt" SetFocusOnError="true" Display="Dynamic"
                            ForeColor="Red" ErrorMessage="Receipt Required" ValidationGroup="ValidateExpense" Font-Bold="true"></asp:RequiredFieldValidator>
                        <div>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>Remark                                      
                                    </td>
                                    <td>
                                        <asp:FileUpload ID="fuReceipt" runat="server"></asp:FileUpload>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button ID="btnUploadReceipt" runat="server" Text="Upload Receipt" OnClick="btnUploadReceipt_Click" ValidationGroup="ValidateExpense" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        <div>
            <asp:SqlDataSource ID="InvoiceSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="AC_GetPendingInvoiceForPaymentReceipt" SelectCommandType="StoredProcedure">
            </asp:SqlDataSource>
        </div>
</asp:Content>


