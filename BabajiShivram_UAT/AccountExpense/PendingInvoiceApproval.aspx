<%@ Page Title="Pending Approval" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="PendingInvoiceApproval.aspx.cs" Inherits="AccountExpense_PendingInvoiceApproval" EnableEventValidation="false" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<meta http-equiv="refresh" content="180">
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
        
        <fieldset><legend>Invoice Approval</legend>
        
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
                <asp:TemplateField HeaderText="" ItemStyle-Width="8%">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgbtnApproveJob" runat="server" ImageUrl="~/Images/success.png" OnClientClick="return confirm('Are you sure to approve Invoice?');"
                            CommandArgument='<%#Eval("InvoiceId") %>' CommandName="ApproveJob" Width="17px" Height="17px" ToolTip="Approve Invoice."
                            Style="padding-right: 1px; margin-right: 0px; padding-left: 1px"></asp:ImageButton>
                        <asp:ImageButton ID="imgbtnHoldJob" runat="server" ImageUrl="~/Images/hold.png"
                            CommandArgument='<%#Eval("InvoiceId") %>' CommandName="Hold" Width="18px" Height="18px" ToolTip="Hold Invoice."
                            Style="padding-right: 1px; margin-right: 0px; padding-left: 1px"></asp:ImageButton>
                        <asp:ImageButton ID="imgRejectJob" runat="server" ImageUrl="~/Images/Reject.jpg"
                            CommandArgument='<%#Eval("InvoiceId") %>' CommandName="Reject" Width="18px" Height="18px" ToolTip="Reject Invoice."
                            Style="padding-right: 1px; margin-right: 0px; padding-left: 1px"></asp:ImageButton>
                        <asp:ImageButton ID="imgbtnDocument" runat="server" ImageUrl="~/Images/file.gif" Width="17px" Height="18px"
                            CommandArgument='<%#Eval("InvoiceId") %>' CommandName="DownloadDoc" ToolTip="Download Documents." Style="padding-right: 0px; margin-right: 0px; padding-left: 1px" />
                        <asp:ImageButton ID="imgbtnJobDetail" runat="server" ImageUrl="~/Images/history.png"
                            CommandArgument='<%#Eval("InvoiceId") %>' CommandName="History" Width="18px" Height="18px" ToolTip="Payment History."
                                Style="padding-right: 1px; margin-right: 0px; padding-left: 1px"></asp:ImageButton>       
                        </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Job No" SortExpression="FARefNo">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkJobRefNo" CommandName="select" runat="server" Text='<%#Eval("FARefNo") %>'
                            CommandArgument='<%#Eval("InvoiceId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="FARefNo" HeaderText="Job No" Visible="False" />
                <asp:TemplateField HeaderText="View BJV" SortExpression="FARefNo">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBJVNo" CommandName="ViewBJV" runat="server" Text="View BJV"
                            CommandArgument='<%#Eval("FARefNo") %>' ToolTip="Check BJV Detail" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ExpenseTypeName" HeaderText="Expense Type" SortExpression="ExpenseTypeName"/>
                <asp:BoundField DataField="InvoiceCurrencyAmt" HeaderText="Total Value (INR)" SortExpression="InvoiceCurrencyAmt" />
                <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" SortExpression="VendorName" />
                <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" />
                <asp:BoundField DataField="ImmediatePayment" HeaderText="Immediate Pay" SortExpression="ImmediatePayment"/>
                <asp:BoundField DataField="AdvanceReceived" HeaderText="Adv Rcvd" SortExpression="AdvanceReceived" />
                <asp:BoundField DataField="AdvanceAmount" HeaderText="Adv Amt" SortExpression="AdvanceAmount" />
                <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="Status"/>
                <asp:BoundField DataField="BilledStatus" HeaderText="Billed ?" SortExpression="BilledStatus" />
                <asp:BoundField DataField="InvoiceTypeName" HeaderText="Invoice Type" SortExpression="InvoiceTypeName"/>
                <asp:BoundField DataField="InvoiceModeName" HeaderText="Type" SortExpression="InvoiceModeName" />
                <asp:BoundField DataField="PaymentTerms" HeaderText="Pay Terms Days" SortExpression="PaymentTerms" />
                <asp:BoundField DataField="PaymentRequiredDate" HeaderText="Pay Required Date" SortExpression="PaymentRequiredDate" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="TotalAmount" HeaderText="Total Value" SortExpression="TotalAmount" />
                <asp:BoundField DataField="CurrencyName" HeaderText="Currency" SortExpression="CurrencyName" />
                <asp:BoundField DataField="InvoiceCurrencyExchangeRate" HeaderText="ExchangeRate" SortExpression="InvoiceCurrencyExchangeRate" />
                <%--<asp:BoundField DataField="Consignee" HeaderText="Consignee" SortExpression="Consignee" />--%>
                <%--<asp:BoundField DataField="VendorGSTNo" HeaderText="Supplier GSTIN No" SortExpression="VendorGSTNo" />--%>
            </Columns>
            <PagerTemplate>
                <asp:GridViewPager runat="server" />
            </PagerTemplate>
        </asp:GridView>
        </fieldset>
        
        <%--  START : MODAL POP-UP FOR Approve- 1/Reject -2/Hold-3 EXPENSE  --%>

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
                            <asp:Label ID="lblPopupName" runat="server"></asp:Label>

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
                        <asp:RequiredFieldValidator ID="RFVReason" runat="server" ControlToValidate="txtReason" SetFocusOnError="true" Display="Dynamic"
                            ForeColor="Red" ErrorMessage="Enter Remark" ValidationGroup="ValidateExpense" Font-Bold="true"></asp:RequiredFieldValidator>
                        <div>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>Remark                                      
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" Rows="4" MaxLength="200" Width="300px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button ID="btnApproveJob" runat="server" Text="Approve" OnClick="btnApproveJob_OnClick" Visible="false" ValidationGroup="ValidateExpense" />
                                        <asp:Button ID="btnRejectJob" runat="server" Text="Reject" OnClick="btnRejectJob_OnClick" Visible="false" ValidationGroup="ValidateExpense" />
                                        <asp:Button ID="btnHoldJob" runat="server" Text="Hold" OnClick="btnHoldJob_OnClick" Visible="false" ValidationGroup="ValidateExpense" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <%--  START : MODAL POP-UP FOR Details/History Expense  --%>
            <div>
                <asp:LinkButton ID="lnkHistory" runat="server" Enabled="false"></asp:LinkButton>
            </div>

           <div id="divDetailsHistory">
                <cc1:ModalPopupExtender ID="ModalPopupDetailHistory" runat="server" CacheDynamicResults="false" PopupControlID="panJobDetails" 
                    CancelControlID="imgbtnJobDetails" TargetControlID="lnkHistory" BackgroundCssClass="modalBackground" DropShadow="true">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="panJobDetails" runat="server" CssClass="ModalPopupPanel" Width="1100px">
                    <div class="header">
                        <div class="fleft">
                            Job Payment History
                        </div>
                        <div class="fright">
                            <asp:ImageButton ID="imgbtnJobDetails" ImageUrl="../Images/delete.gif" runat="server"
                                ToolTip="Close" />
                        </div>
                    </div>          
                    <div id="Div4" runat="server" style="max-height: 250px; overflow: auto; padding: 5px">
                        <div>
                            <asp:GridView ID="grdJobHistory" runat="server" CssClass="table" AutoGenerateColumns="false"
                                PagerStyle-CssClass="pgr" DataKeyNames="InvoiceID" AllowPaging="True" AllowSorting="True" PageSize="20"
                                PagerSettings-Position="TopAndBottom" Width="100%">
                                <%--DataSourceID="DataSourceDocuments">--%>
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                 
                                    <asp:BoundField DataField="FARefNo" HeaderText="BS Job No" SortExpression="FARefNo"/>
                                    <asp:BoundField DataField="ConsigneeName" HeaderText="Consignee" SortExpression="Consignee"/>
                                    <asp:BoundField DataField="PaymentTypeName" HeaderText="Payment Type" SortExpression="PaymentTypeName"/>
                                    <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName"/>
                                    <asp:BoundField DataField="ExpenseTypeName" HeaderText="Expense Type" SortExpression="ExpenseTypeName"/>
                                    <asp:BoundField DataField="TotalAmount" HeaderText="Total Amt" SortExpression="TotalAmount" />
                                    <asp:BoundField DataField="VendorName" HeaderText="Paid To" SortExpression="VendorName" ReadOnly="true"/>
                                    <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy"/>
                                    <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss tt}" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>
            </div>

            <%--  START : MODAL POP-UP FOR Document  --%>

            <div>
                <asp:LinkButton ID="lnkDocumentPupup" runat="server" Enabled="false"></asp:LinkButton>
            </div>

           <div id="divDetailsDocument">
                <cc1:ModalPopupExtender ID="ModalPopupDetailDocument" runat="server" CacheDynamicResults="false" PopupControlID="panJobDocument" 
                    CancelControlID="imgbtnJobDetails" TargetControlID="lnkDocumentPupup" BackgroundCssClass="modalBackground" DropShadow="true">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="panJobDocument" runat="server" CssClass="ModalPopupPanel" Width="800px">
                    <div class="header">
                        <div class="fleft">
                            Invoice Document
                        </div>
                        <div class="fright">
                            <asp:ImageButton ID="imgbtnJobDcouemtn" ImageUrl="../Images/delete.gif" runat="server"
                                ToolTip="Close" />
                        </div>
                    </div>          
                    <div id="Div1" runat="server" style="max-height: 250px; overflow: auto; padding: 5px">
                        <div>
                            <asp:GridView ID="gvDocument" runat="server" CssClass="table" AutoGenerateColumns="false"
                                PagerStyle-CssClass="pgr" DataKeyNames="InvoiceID" AllowPaging="True" AllowSorting="True" PageSize="20"
                                OnRowCommand="gvDocument_RowCommand" PagerSettings-Position="TopAndBottom" Width="100%">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                 
                                    <asp:BoundField DataField="DocumentName" HeaderText="Document Name" SortExpression="DocumentName"/>
                                    <asp:BoundField DataField="FileName" HeaderText="File Name" SortExpression="FileName"/>
                                    <asp:TemplateField HeaderText="Download">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                                CommandArgument='<%#Eval("lid") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="View">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkView" runat="server" Text="View" CommandName="View" 
                                                CommandArgument='<%#Eval("lid") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>
            </div>

            <div id="BJV_Popup">
            <asp:LinkButton ID="lnkModelPopup21" runat="server" />
                <cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="lnkModelPopup21"
                        PopupControlID="Panel21">
                    </cc1:ModalPopupExtender>
                <asp:Panel ID="Panel21" Style="display: none" runat="server">
                        <fieldset>
                            <legend>BJV DETAIL</legend>
                            <div class="header">
                                <div class="fright">
                                    <asp:ImageButton ID="ImageButton1" ImageUrl="~/Images/delete.gif" runat="server"
                                        OnClick="btnCancelBJVPopup_Click" ToolTip="Close" />
                                </div>
                            </div>
                            <div class="clear"></div>
                            <div id="Div3" runat="server" style="max-height: 560px; overflow: auto;">
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
        <div>
            <asp:SqlDataSource ID="InvoiceSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="AC_GetPendingInvoiceForApproval" SelectCommandType="StoredProcedure">
            </asp:SqlDataSource>
        </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


