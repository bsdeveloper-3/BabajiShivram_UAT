<%@ Page Title="Cheque/Invoice Pending" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="PendingChequeInvoice.aspx.cs" Inherits="AccountExpense_PendingChequeInvoice" EnableEventValidation="false" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager id="ScriptManager1" runat="server" scriptmode="Release"> </asp:ScriptManager>    
     <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upUpdatePanel" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upUpdatePanel" runat="server" RenderMode="Inline">
        <ContentTemplate>
        <fieldset><legend>Issued Cheque/ Invoice Pending</legend>
        <div align="center">
            <asp:Label ID="lblMessage" runat="server" CssClass="errorMsg" EnableViewState="false" ></asp:Label>
            <asp:HiddenField ID="hdnChequeId" runat="server" Value="0" />
            <asp:HiddenField ID="hdnInvoiceId" runat="server" Value="0" />
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
        <asp:GridView ID="gvDetail" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="ChequeID,InvoiceId"
            AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataSourceID="ChequeSqlDataSource" 
            AllowPaging="True" AllowSorting="True" PageSize="40" PagerSettings-Position="TopAndBottom"
            Width="100%" OnRowCommand="gvDetail_RowCommand" OnRowDataBound="gvDetail_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Job No" SortExpression="JobRefNo">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkJobRefNo" CommandName="select" runat="server" Text='<%#Eval("JobRefNo") %>'
                           CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="JobRefNo" HeaderText="Job No" Visible="False" />
                <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                <asp:BoundField DataField="Consignee" HeaderText="Consignee" SortExpression="Consignee" />
                <asp:BoundField DataField="ChequeNo" HeaderText="Cheque No" SortExpression="ChequeNo"/>
                <asp:BoundField DataField="ChequeDate" HeaderText="Cheque Date" SortExpression="ChequeDate" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField DataField="AccountName" HeaderText="Account Name" SortExpression="AccountName"/>
                <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark"/>
                <asp:TemplateField HeaderText="Cancel">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkCancel" CommandName="Cancel" runat="server" Text="Cancel Cheque" 
                            CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' Visible="false" />
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

        <div id="divOpExpenseExp">
            <cc1:ModalPopupExtender ID="CancelModalPopupExp" runat="server" CacheDynamicResults="false" PopupControlID="panCancelExp" 
                CancelControlID="imgbtnCancelExp" TargetControlID="lnkCancelExp" BackgroundCssClass="modalBackground" DropShadow="true">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="panCancelExp" runat="server" CssClass="ModalPopupPanel" Width="600px">
                <div class="header">
                    <div class="fleft">
                        <asp:Label ID="lblPopupNameExp" runat="server"></asp:Label>
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
                                <td>Cheque Number</td>
                                <td>
                                    <asp:Label ID="lblChequeNo" runat="server"></asp:Label>
                                </td>
                                <td>Cheque Date</td>
                                <td>
                                    <asp:Label ID="lblChequeDate" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Bank Name</td>
                                <td>
                                    <asp:Label ID="lblBankName" runat="server"></asp:Label>
                                </td>
                                <td>Bank Account</td>
                                <td>
                                    <asp:Label ID="lblBankAccountName" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Job Number</td>
                                <td>
                                    <asp:Label ID="lblJobNumber" runat="server"></asp:Label>
                                </td>
                                <td>Consignee</td>
                                <td>
                                    <asp:Label ID="lblConsigneeName" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Amount</td>
                                <td>
                                    <asp:Label ID="lblAmount" runat="server"></asp:Label>
                                </td>
                                <td>Paid To</td>
                                <td>
                                    <asp:Label ID="lblPaidTo" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Remark</td>
                                <td>
                                    <asp:Label ID="lblRemark" runat="server"></asp:Label>
                                </td>
                                <td>Created By</td>
                                <td>
                                    <asp:Label ID="lblCreatedBy" runat="server"></asp:Label>
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
                                    <asp:TextBox ID="txtCancelReason" runat="server" TextMode="MultiLine" Rows="3" MaxLength="200" Width="300px"></asp:TextBox>
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
            <asp:SqlDataSource ID="ChequeSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="AC_GetPendingChequeForInvoice" SelectCommandType="StoredProcedure">
            </asp:SqlDataSource>
        </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


