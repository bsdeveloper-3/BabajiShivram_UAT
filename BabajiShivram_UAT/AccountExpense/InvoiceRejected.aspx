<%@ Page Title="Invoice Rejected" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="InvoiceRejected.aspx.cs" Inherits="AccountExpense_InvoiceRejected" EnableEventValidation="false" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<meta http-equiv="refresh" content="180">
    <asp:Scriptmanager id="ScriptManager1" runat="server"></asp:Scriptmanager>
    <asp:UpdatePanel ID="upFillDetails" runat="server">
        <ContentTemplate>
    <fieldset><legend>Payment Rejected</legend>
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
            Width="100%" OnRowCommand="gvDetail_RowCommand" OnRowDataBound="gvDetail_RowDataBound">
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
                <asp:BoundField DataField="ExpenseTypeName" HeaderText="Expense Type"/>
                <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" />
                <asp:BoundField DataField="InvoiceModeName" HeaderText="Type" SortExpression="InvoiceModeName" />
                <asp:BoundField DataField="RIMType" HeaderText="RIM/NonRIM" SortExpression="RIMType" />
                <asp:BoundField DataField="InvoiceTypeName" HeaderText="Invoice Type"/>
                <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                <asp:BoundField DataField="BillTypeName" HeaderText="Vendor Type" SortExpression="BillTypeName" />
                <%--<asp:BoundField DataField="VendorGSTNo" HeaderText="Supplier GSTIN No" SortExpression="VendorGSTNo" />--%>
                <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" SortExpression="VendorName" />
                <%--<asp:BoundField DataField="PaymentTerms" HeaderText="Vendor Payment Terms In Days" SortExpression="PaymentTerms" />--%>
                <asp:BoundField DataField="PaymentRequiredDate" HeaderText="Payment Required Date" SortExpression="PaymentRequiredDate" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField DataField="TotalAmount" HeaderText="Total Value" SortExpression="TotalAmount" />
                <%--<asp:BoundField DataField="AdvanceReceived" HeaderText="Advance Received" SortExpression="AdvanceReceived" />--%>
                <asp:BoundField DataField="AdvanceAmount" HeaderText="Advance" SortExpression="AdvanceAmount" />
                <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" />
                <asp:BoundField DataField="KAM" HeaderText="KAM" SortExpression="KAM" />
                <asp:BoundField DataField="InvoiceUser" HeaderText="Invoice User" SortExpression="InvoiceUser" />
                <asp:BoundField DataField="UserName" HeaderText="Rejected By" SortExpression="UserName" />
                <asp:BoundField DataField="StatusDate" HeaderText="Rejected Date" SortExpression="StatusDate" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField DataField="Aging" HeaderText="Aging" SortExpression="Aging" />
                <asp:TemplateField HeaderText="Cancel">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkCancel" CommandName="Cancel" runat="server" Text="Cancel" 
                            CommandArgument='<%#Eval("InvoiceId") %>' Visible="false" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerTemplate>
                <asp:GridViewPager runat="server" />
            </PagerTemplate>
        </asp:GridView>
        </fieldset>
             <%--  START : MODAL POP-UP FOR Cancel EXPENSE  --%>

            <div>
                <asp:LinkButton ID="lnkCancelExp" runat="server"></asp:LinkButton>
            </div>

            <div id="divOpExpense">
                <cc1:ModalPopupExtender ID="CancelModalPopupExtender" runat="server" CacheDynamicResults="false" PopupControlID="panCancelJob" 
                    CancelControlID="imgbtnCancelExp" TargetControlID="lnkCancelExp" BackgroundCssClass="modalBackground" DropShadow="true">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="panCancelJob" runat="server" CssClass="ModalPopupPanel" Width="600px">
                    <div class="header">
                        <div class="fleft">
                            <asp:Label ID="lblPopupName" runat="server"></asp:Label>

                        </div>
                        <div class="fright">
                            <asp:ImageButton ID="imgbtnCancelExp" ImageUrl="../Images/delete.gif" runat="server"
                                ToolTip="Close" />
                        </div>
                    </div>
                    <div id="Div2" runat="server" style="max-height: 250px; overflow: auto; padding: 5px">
                        <div>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <div class="m">
                                    <center>
                                        <asp:Label ID="lblError_CancelExp" runat="server"></asp:Label>
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
                                    <td>Remark</td>
                                    <td>
                                        <asp:Label ID="lblRemark1" runat="server"></asp:Label>
                                    </td>
                                    <td>Created By</td>
                                    <td>
                                        <asp:Label ID="lblCreatedBy1" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <asp:RequiredFieldValidator ID="RFVReason" runat="server" ControlToValidate="txtCancelReason" SetFocusOnError="true" Display="Dynamic"
                            ForeColor="Red" ErrorMessage="Enter Cancel Remark" ValidationGroup="ValidateCancel" Font-Bold="true"></asp:RequiredFieldValidator>
                        <div>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>Remark                                      
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCancelReason" runat="server" TextMode="MultiLine" Rows="4" MaxLength="200" Width="300px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button ID="btnCancelJob" runat="server" Text="Cancel Invoice" OnClick="btnCancelJob_Click" ValidationGroup="ValidateCancel" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </asp:Panel>
            </div>

        <div>
            <asp:SqlDataSource ID="InvoiceSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="AC_GetPendingInvoiceForReject" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="UserId" SessionField="UserId" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
</ContentTemplate>
            </asp:UpdatePanel>
</asp:Content>


