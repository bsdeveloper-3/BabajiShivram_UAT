<%@ Page Title="Payment Rejected" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" EnableEventValidation="false"
        CodeFile="PendingTransRejectedView.aspx.cs" Inherits="AccountTransport_PendingTransRejectedView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upFillDetails" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upFillDetails" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <div align="center">
            <asp:Label ID="lblMessage" runat="server" CssClass="errorMsg" EnableViewState="false"></asp:Label>
            <asp:HiddenField ID="hdnInvoiceId" runat="server" />
            <asp:HiddenField ID="hdnOperationType" runat="server" />
        </div>
        <fieldset><legend>Transporter Payment Rejected</legend>
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
        <asp:GridView ID="gvDetail" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="RequestId"
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
                        <asp:LinkButton ID="lnkJobRefNo" CommandName="select" runat="server" Text='<%#Eval("JobRefNo") %>'
                            CommandArgument='<%#Eval("RequestId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="JobRefNo" HeaderText="Job No" Visible="False" />
                <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" SortExpression="InvoiceNo" />
                <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" SortExpression="InvoiceDate" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName" />
                <asp:BoundField DataField="ExpenseTypeName" HeaderText="Expense Type" SortExpression="ExpenseTypeName" />
                <asp:BoundField DataField="CustName" HeaderText="Customer" SortExpression="Customer" />
                <asp:BoundField DataField="PaidTo" HeaderText="Transporter" SortExpression="PaidTo" />
                <asp:BoundField DataField="PaymentRequiredDate" HeaderText="Pay Reqrd Date" SortExpression="PaymentRequiredDate" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="Amount" HeaderText="Total Value" SortExpression="TotalAmount" />
                <%--<asp:BoundField DataField="AdvanceAmt" HeaderText="Advance Amount" SortExpression="AdvanceAmt" />--%>
                <asp:BoundField DataField="RejectedRemark" HeaderText="Remark" SortExpression="RejectedRemark" />
                <asp:BoundField DataField="RejectedBy" HeaderText="User" SortExpression="RejectedBy" />
                <asp:BoundField DataField="updDate" HeaderText="Date" SortExpression="updDate" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:TemplateField HeaderText="Cancel">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkCancel" CommandName="Cancel" runat="server" Text="Cancel" 
                            CommandArgument='<%#Eval("RequestId") %>' Visible="true" />
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
                                    <td>Payment Type</td>
                                    <td>
                                        <asp:Label ID="lblPaymentType1" runat="server"></asp:Label>
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
                                    <td>Status</td>
                                    <td>
                                        <asp:Label ID="lblStatus1" runat="server"></asp:Label>
                                    </td>
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
                                        <asp:Button ID="btnCancelJob" runat="server" Text="Cancel Invoice" OnClick="btnCancelJob_Click" ValidationGroup="ValidateCancel" 
                                            OnClientClick="return confirm('Sure to Cancel Payment Request?');"/>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        <div>
            <asp:SqlDataSource ID="InvoiceSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="TRS_GetPendingInvoiceForReject" SelectCommandType="StoredProcedure">
                <SelectParameters>
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                <asp:Parameter Name="CompanyID" DefaultValue="12" />
            </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


