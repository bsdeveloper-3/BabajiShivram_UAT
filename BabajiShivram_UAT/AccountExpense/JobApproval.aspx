<%@ Page Title="Job Expense Approval" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="JobApproval.aspx.cs" Inherits="AccountExpense_JobApproval" EnableEventValidation="false" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingForm13" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../Images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPendingForm13" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div class="clear">
            </div>
            <fieldset class="fieldset-AutoWidth">
                <legend>Expense Approval</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                        </div>
                        <div class="fleft" style="margin-left: 2px; padding-top: 3px;">

                            <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                                <asp:Image ID="Image1" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>

                            <div class="fright" style="padding-left: 40px">
                                <b>Total Amount : </b>
                                <asp:Label ID="lblTotalAmount" runat="server" Font-Underline="true" Font-Bold="true"></asp:Label>
                                &nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; 
                                <asp:TextBox ID="txtPendingFunds" runat="server" Width="3%" BackColor="LightGoldenrodYellow"></asp:TextBox>
                                Pending For Approval
                                &nbsp; &nbsp;
                                <asp:TextBox ID="txtHold" runat="server" Width="3%" BackColor="#87AFC7"></asp:TextBox>
                                Hold
                                &nbsp; &nbsp;
                                <asp:TextBox ID="txtReject" runat="server" Width="3%" BackColor="#E8ADAA"></asp:TextBox>
                                Reject
                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <div class="clear">
                </div>
                <asp:GridView ID="gvJobExpDetail" runat="server" AutoGenerateColumns="False" Width="2150px" PagerStyle-CssClass="pgr" OnRowDataBound="gvJobExpDetail_RowDataBound"
                    DataKeyNames="JobId" OnRowCommand="gvJobExpDetail_RowCommand" AllowPaging="True" Style="white-space: normal" AllowSorting="True" CssClass="table" PageSize="20"
                    PagerSettings-Position="TopAndBottom" DataSourceID="DataSourceJobExpenseDetails" OnPreRender="gvJobExpDetail_PreRender">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" ItemStyle-Width="8%">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnApproveJob" runat="server" ImageUrl="~/Images/success.png" OnClientClick="return confirm('Are you sure to approve Job Expense?');"
                                    CommandArgument='<%#Eval("lid") %>' CommandName="ApproveJob" Width="17px" Height="17px" ToolTip="Approve Job Expense."
                                    Style="padding-right: 1px; margin-right: 0px; padding-left: 1px"></asp:ImageButton>
                                <asp:ImageButton ID="imgbtnHoldJob" runat="server" ImageUrl="~/Images/hold.png"
                                    CommandArgument='<%#Eval("lid") %>' CommandName="Hold" Width="18px" Height="18px" ToolTip="Hold Job Expense."
                                    Style="padding-right: 1px; margin-right: 0px; padding-left: 1px"></asp:ImageButton>
                                <asp:ImageButton ID="imgbtnCancelJob" runat="server" ImageUrl="~/Images/cross.png"
                                    CommandArgument='<%#Eval("lid") %>' CommandName="Cancel" Width="18px" Height="18px" ToolTip="Cancel Job Expense."
                                    Style="padding-right: 1px; margin-right: 0px; padding-left: 1px"></asp:ImageButton>
                                <asp:ImageButton ID="imgRejectJob" runat="server" ImageUrl="~/Images/Reject.jpg"
                                    CommandArgument='<%#Eval("lid") %>' CommandName="Reject" Width="18px" Height="18px" ToolTip="Reject Job Expense."
                                    Style="padding-right: 1px; margin-right: 0px; padding-left: 1px"></asp:ImageButton>
                                <asp:ImageButton ID="imgbtnDocument" runat="server" ImageUrl="~/Images/file.gif" Width="17px" Height="18px"
                                    CommandArgument='<%#Eval("lid") %>' CommandName="DownloadDoc" ToolTip="Download Documents." Style="padding-right: 0px; margin-right: 0px; padding-left: 1px" />
                                <asp:ImageButton ID="imgbtnJobDetail" runat="server" ImageUrl="~/Images/history.png"
                                    CommandArgument='<%#Eval("ModuleId")  + "," + Eval("JobId") %>' CommandName="Details" Width="18px" Height="18px" ToolTip="Job History."
                                    Style="padding-right: 1px; margin-right: 0px; padding-left: 1px"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BS Job No" SortExpression="JobRefNo" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkJobRefNo" ToolTip="Add Job Expense Payment Details." CommandName="addpayment" runat="server" Text='<%#Eval("JobRefNo") %>'
                                    CommandArgument='<%#Eval("lid")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" ReadOnly="true" ItemStyle-Width="8%" Visible="false" />
                        <asp:BoundField DataField="Consignee" HeaderText="Consignee" SortExpression="Consignee" ReadOnly="true" ItemStyle-Width="10.3%" />
                        <asp:BoundField DataField="BranchName" HeaderText="Branch" SortExpression="BranchName" ReadOnly="true" ItemStyle-Width="6.3%" />                        
                        <asp:BoundField DataField="CompanyName" HeaderText="Company" ReadOnly="true" SortExpression="CompanyName" ItemStyle-Width="2%" />
                        <asp:BoundField DataField="PlanningDate" HeaderText="Planning Date" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" ItemStyle-Width="4.3%" />
                        <asp:BoundField DataField="PaymentTypeName" HeaderText="Payment Type" SortExpression="PaymentTypeName" ReadOnly="true" ItemStyle-Width="4.3%" />
                        <asp:BoundField DataField="ExpenseTypeName" HeaderText="Expense Type" SortExpression="ExpenseTypeName" ReadOnly="true" ItemStyle-Width="5.3%" />
                        <asp:BoundField DataField="AdvanceDetail" HeaderText="Advance Detail" SortExpression="AdvanceDetail" ReadOnly="true" ItemStyle-Width="4.3%" />
                        <asp:BoundField DataField="Total_Amnt" HeaderText="Total Amount" SortExpression="Total_Amnt" ReadOnly="true" ItemStyle-Width="4.3%" />
                        <asp:BoundField DataField="PaidTo" HeaderText="Paid To" SortExpression="PaidTo" ReadOnly="true" ItemStyle-Width="7.3%" />
                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy" ReadOnly="true" ItemStyle-Width="5.3%" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss tt}" ReadOnly="true" ItemStyle-Width="9.3%" />
                        <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" ReadOnly="true" ItemStyle-Width="11.3%" />
                        <asp:BoundField DataField="HoldDate" HeaderText="Hold On" DataFormatString="{0:dd/MM/yyyy HH:mm:ss tt}" ReadOnly="true" ItemStyle-Width="9.3%" />
                        <asp:BoundField DataField="HoldRemark" HeaderText="Hold Remark" ReadOnly="true" ItemStyle-Width="15.3%" />
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="DataSourceJobExpenseDetails" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetPaymentRequest" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="lUser" SessionField="UserId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>

            <%--  START : MODAL POP-UP FOR DOCUMENTS DOWNLOAD  --%>

            <div id="divContractCopy">
                <cc1:ModalPopupExtender ID="mpeContractCopy" runat="server" CacheDynamicResults="false"
                    PopupControlID="pnlContractCopy" CancelControlID="imgbtnDoc" TargetControlID="lnkbtnContractCopy" BackgroundCssClass="modalBackground" DropShadow="true">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pnlContractCopy" runat="server" CssClass="ModalPopupPanel" Width="350px">
                    <div class="header">
                        <div class="fleft">
                            Download Documents
                        </div>
                        <div class="fright">
                            <asp:ImageButton ID="imgbtnDoc" ImageUrl="../Images/delete.gif" runat="server"
                                ToolTip="Close" />
                        </div>
                    </div>
                    <div class="m">
                    </div>
                    <!-- Lists Of All Documents -->
                    <div id="Div3" runat="server" style="max-height: 250px; overflow: auto; padding: 5px">
                        <div>
                            <asp:GridView ID="gvDocuments" runat="server" CssClass="table" AutoGenerateColumns="false"
                                PagerStyle-CssClass="pgr" DataKeyNames="lid" AllowPaging="True" AllowSorting="True" PageSize="20"
                                PagerSettings-Position="TopAndBottom" OnRowCommand="gvDocuments_RowCommand" Width="100%"
                                DataSourceID="DataSourceDocuments">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Document">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDownloadDoc" CommandName="download" ToolTip="Download document" runat="server"
                                                Text='<%#Eval("FileName") %>' CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" CommandName="deletedoc" ToolTip="Delete document" runat="server"
                                                Text="Delete" CommandArgument='<%#Eval("lid") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div>
                            <asp:SqlDataSource ID="DataSourceDocuments" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="AC_GetExpenseDocDetails" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="PaymentId" SessionField="PaymentId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div>
                <asp:HiddenField ID="lnkbtnContractCopy" runat="server"></asp:HiddenField>
            </div>

            <%--  END   : MODAL POP-UP FOR DOCUMENTS DOWNLOAD  --%>

            <%--  -------------------------------------------------------------------------------------------------------------------------------%>

            <%--  START : MODAL POP-UP FOR HOLD EXPENSE  --%>

            <div id="divHoldExpense">
                <cc1:ModalPopupExtender ID="mpeHoldExpense" runat="server" CacheDynamicResults="false"
                    PopupControlID="pnlHoldExpense" CancelControlID="imgbtnHoldExp" TargetControlID="hdnHoldExp" BackgroundCssClass="modalBackground" DropShadow="true">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pnlHoldExpense" runat="server" CssClass="ModalPopupPanel" Width="600px">
                    <div class="header">
                        <div class="fleft">
                            <asp:Label ID="lblPopupName" runat="server"></asp:Label>
                            <asp:HiddenField ID="hdnRequestFor" runat="server" />
                        </div>
                        <div class="fright">
                            <asp:ImageButton ID="imgbtnHoldExp" ImageUrl="../Images/delete.gif" runat="server"
                                ToolTip="Close" />
                        </div>
                    </div>

                    <!-- Lists Of All Documents -->
                    <div id="Div1" runat="server" style="max-height: 250px; overflow: auto; padding: 5px">
                        <div>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">

                                <div class="m">
                                    <asp:Label ID="lblError_HoldExp" runat="server"></asp:Label>
                                </div>

                                <tr>
                                    <td>BS Job Number</td>
                                    <td>
                                        <asp:Label ID="lblBSJobNo" runat="server"></asp:Label>
                                    </td>
                                    <td>Branch</td>
                                    <td>
                                        <asp:Label ID="lblBranch" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Payment Type</td>
                                    <td>
                                        <asp:Label ID="lblPaymentType" runat="server"></asp:Label>
                                    </td>
                                    <td>Expense Type</td>
                                    <td>
                                        <asp:Label ID="lblExpenseType" runat="server"></asp:Label>
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
                        <br />
                        &nbsp;
                        <asp:RequiredFieldValidator ID="rfvReason" runat="server" ControlToValidate="txtReason" SetFocusOnError="true" Display="Dynamic"
                            ForeColor="Red" ErrorMessage="* Enter Reason" ValidationGroup="vgAddHoldExpense" Font-Bold="true"></asp:RequiredFieldValidator>
                        <div>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>Reason                                      
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" Rows="4" Width="500px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button ID="btnHoldJob" runat="server"
                                            OnClick="btnHoldJob_OnClick" Text="Hold Job" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="AC_GetExpenseDocDetails" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="PaymentId" SessionField="PaymentId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div>
                <asp:HiddenField ID="hdnHoldExp" runat="server"></asp:HiddenField>
            </div>

            <%--  END   : MODAL POP-UP FOR HOLD EXPENSE  --%>

            <%--  START : MODAL POP-UP FOR HOLD EXPENSE  --%>

            <div id="divRejectExpense">
                <cc1:ModalPopupExtender ID="RejectModalPopupExtender" runat="server" CacheDynamicResults="false" PopupControlID="panRejectJob" 
                    CancelControlID="imgbtnRejectExp" TargetControlID="hdnRejectExp" BackgroundCssClass="modalBackground" DropShadow="true">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="panRejectJob" runat="server" CssClass="ModalPopupPanel" Width="600px">
                    <div class="header">
                        <div class="fleft">
                            <asp:Label ID="lblREjectPopName" runat="server"></asp:Label>
                            <asp:HiddenField ID="hdnRequestForReject" runat="server" />
                        </div>
                        <div class="fright">
                            <asp:ImageButton ID="imgbtnRejectExp" ImageUrl="../Images/delete.gif" runat="server"
                                ToolTip="Close" />
                        </div>
                    </div>
                    <%--  <div class="m">
                        <asp:Label ID="lblError_RejectExp" runat="server"></asp:Label>
                    </div>--%>
                    <!-- Lists Of All Documents -->
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
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtRejectReason" SetFocusOnError="true" Display="Dynamic"
                            ForeColor="Red" ErrorMessage="* Enter Reason" ValidationGroup="vgRejectExpense" Font-Bold="true"></asp:RequiredFieldValidator>
                        <div>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>Reason                                      
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRejectReason" runat="server" TextMode="MultiLine" Rows="4" MaxLength="200" Width="500px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button ID="btnRejectJob" runat="server" Text="Reject Job" OnClick="btnRejectJob_OnClick" ValidationGroup="vgRejectExpense" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <%--<div>
                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="AC_GetExpenseDocDetails" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="PaymentId" SessionField="PaymentId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>--%>
                    </div>
                </asp:Panel>
            </div>
            <div>
                <asp:HiddenField ID="hdnRejectExp" runat="server"></asp:HiddenField>
            </div>

            <%--  END   : MODAL POP-UP FOR HOLD EXPENSE  --%>

              <%--  START : MODAL POP-UP FOR Details/History Expense  --%>

           <div id="divDetailsHistory">
                <cc1:ModalPopupExtender ID="ModalPopupDetailHistory" runat="server" CacheDynamicResults="false" PopupControlID="panJobDetails" 
                    CancelControlID="imgbtnJobDetails" TargetControlID="hdnJobDetailHistory" BackgroundCssClass="modalBackground" DropShadow="true">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="panJobDetails" runat="server" CssClass="ModalPopupPanel" Width="900px">
                    <div class="header">
                        <div class="fleft">
                            <asp:Label ID="lblDetailsPopName" runat="server"></asp:Label>
                            <asp:HiddenField ID="hdnReqForJobDetails" runat="server" />
                        </div>
                        <div class="fright">
                            <asp:ImageButton ID="imgbtnJobDetails" ImageUrl="../Images/delete.gif" runat="server"
                                ToolTip="Close" />
                        </div>
                    </div>          
                    <div id="Div4" runat="server" style="max-height: 250px; overflow: auto; padding: 5px">
                        <div>
                            <asp:GridView ID="grdJobDetails" runat="server" CssClass="table" AutoGenerateColumns="false"
                                PagerStyle-CssClass="pgr" DataKeyNames="lid" AllowPaging="True" AllowSorting="True" PageSize="20"
                                PagerSettings-Position="TopAndBottom" Width="100%">
                                <%--DataSourceID="DataSourceDocuments">--%>
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                 
                                    <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" SortExpression="JobRefNo" ReadOnly="true"/>
                                    <asp:BoundField DataField="ConsigneeName" HeaderText="Consignee Name" SortExpression="ConsigneeName" ReadOnly="true" />
                                    <asp:BoundField DataField="BranchName" HeaderText="Branch" SortExpression="BranchName" ReadOnly="true" />
                                    <asp:BoundField DataField="PaymentTypeName" HeaderText="Payment Type" SortExpression="PaymentTypeName" ReadOnly="true" />
                                    <asp:BoundField DataField="CurrentStatus" HeaderText="Status" SortExpression="CurrentStatus" ReadOnly="true" ItemStyle-Width="6.3%" />
                                    <asp:BoundField DataField="ExpenseTypeName" HeaderText="Expense Type" SortExpression="ExpenseTypeName" ReadOnly="true" />
                                    <asp:BoundField DataField="Total_Amnt" HeaderText="Total Amt" SortExpression="Total_Amnt" ReadOnly="true" />
                                    <asp:BoundField DataField="PaidTo" HeaderText="Paid To" SortExpression="PaidTo" ReadOnly="true" ItemStyle-Width="12%" />
                                    <%--<asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" ReadOnly="true" ItemStyle-Width="10%" />--%>
                                    <asp:TemplateField HeaderText="Remark">
                                        <ItemTemplate>
                                            <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                            <asp:Label ID="lblRemarkView" runat="server" Text='<%#Eval("Remark") %>'></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy" ReadOnly="true" />
                                    <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss tt}" ReadOnly="true" ItemStyle-Width="12%" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" ReadOnly="true" Visible="false" />
                                    <asp:BoundField DataField="ApprovedDate" HeaderText="Approved/Hold Date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss tt}" Visible="false" ReadOnly="true" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" ReadOnly="true" ItemStyle-Width="5%" Visible="false" />
                                    <asp:BoundField DataField="Reason" HeaderText="Reason" SortExpression="Reason" ReadOnly="true" ItemStyle-Width="10%" />
                                    <asp:BoundField DataField="Date" HeaderText="Date" ReadOnly="true" Visible="false" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="Location" HeaderText="Location" ReadOnly="true" Visible="false" />
                                    <asp:BoundField DataField="LocationCode" HeaderText="LocationCode" ReadOnly="true" Visible="false" />
                                    <asp:BoundField DataField="Office" HeaderText="Office" ReadOnly="true" Visible="false" />
                                    <asp:BoundField DataField="Babaji Job No" HeaderText="Babaji Job No" ReadOnly="true" Visible="false" />
                                    <asp:BoundField DataField="ACP/NON ACP" HeaderText="ACP/NON ACP" ReadOnly="true" Visible="false" />
                                    <asp:BoundField DataField="Party Name" HeaderText="Party Name" ReadOnly="true" Visible="false" />
                                    <asp:BoundField DataField="IEC No" HeaderText="IEC No" ReadOnly="true" Visible="false" />
                                    <asp:BoundField DataField="BOE No" HeaderText="BOE No" ReadOnly="true" Visible="false" />
                                    <asp:BoundField DataField="BOE Date" HeaderText="BOE Date" ReadOnly="true" Visible="false" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="TR6Challen No" HeaderText="TR6Challen No" ReadOnly="true" Visible="false" />
                                    <asp:BoundField DataField="Duty Amt" HeaderText="Duty Amt" ReadOnly="true" Visible="false" />
                                    <asp:BoundField DataField="Interest Amt" HeaderText="Interest Amt" ReadOnly="true" Visible="false" />
                                    <asp:BoundField DataField="Penalty Amt" HeaderText="Penalty Amt" ReadOnly="true" Visible="false" />
                                    <asp:BoundField DataField="Total" HeaderText="Total" ReadOnly="true" Visible="false" />
                                    <asp:BoundField DataField="Recd Mail From (Sender Name)" HeaderText="Recd Mail From (Sender Name)" ReadOnly="true" Visible="false" />
                                    <asp:BoundField DataField="Approved By" HeaderText="Approved By" ReadOnly="true" Visible="false" />
                                    <asp:BoundField DataField="RD/DUTY/PENALTY" HeaderText="RD/DUTY/PENALTY" ReadOnly="true" Visible="false" />
                                    <asp:BoundField DataField="Advance Details" HeaderText="Advance Details" ReadOnly="true" Visible="false" />
                                    <asp:BoundField DataField="JobStatus" HeaderText="JobStatus" ReadOnly="true" Visible="false" />
                                    <asp:BoundField DataField="Recd Mail From" HeaderText="Recd Mail From" ReadOnly="true" Visible="false" />
                                    <asp:BoundField DataField="Penalty Approval Mail" HeaderText="Penalty Approval Mail" ReadOnly="true" Visible="false" />
                                    <asp:BoundField DataField="Assessable Value" HeaderText="Assessable Value" ReadOnly="true" Visible="false" />
                                    <asp:BoundField DataField="Custom Duty" HeaderText="Custom Duty" ReadOnly="true" Visible="false" />
                                    <asp:BoundField DataField="Assessable Value + Custom Duty" HeaderText="Assessable Value + Custom Duty" ReadOnly="true" Visible="false" />
                                    <asp:BoundField DataField="Stamp Duty" HeaderText="Stamp Duty" ReadOnly="true" Visible="false" />
                                    <asp:BoundField DataField="IGM No" HeaderText="IGM No" ReadOnly="true" Visible="false" />
                                    <asp:BoundField DataField="BL No" HeaderText="BL No" ReadOnly="true" Visible="false" />
                                    <asp:BoundField DataField="BLDate" HeaderText="BLDate" ReadOnly="true" Visible="false" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="GST No" HeaderText="GST No" ReadOnly="true" Visible="false" />
                                    <asp:BoundField DataField="Client Address" HeaderText="Client Address" ReadOnly="true" Visible="false" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <br />                        
                        <%--<div>
                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="AC_GetExpenseDocDetails" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="PaymentId" SessionField="PaymentId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>--%>
                    </div>
                </asp:Panel>
            </div>
            <div>
                <asp:HiddenField ID="hdnJobDetailHistory" runat="server"></asp:HiddenField>
            </div>

            <%--  END   : MODAL POP-UP FOR Details/History EXPENSE  --%>


        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

