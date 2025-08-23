<%@ Page Title="Job Expense Payment" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="JobExpPayment.aspx.cs" Inherits="AccountExpense_JobExpPayment" EnableEventValidation="false" Culture="en-GB" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" id="ScriptManager1" scriptmode="Release" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingForm13" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPendingForm13" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div class="clear">
            </div>
            <fieldset class="fieldset-AutoWidth">
                <legend>Expense Payment</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:datafilter id="DataFilter1" runat="server" />
                        </div>
                        <div class="fleft" style="margin-left: 2px; padding-top: 3px;">
                            <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                                <asp:Image ID="Image1" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>
                            &nbsp;&nbsp;
                            <asp:Button ID="btnPayProcess" runat="server" OnClick="btnPayProcess_Click" Text="Pay Process" />
                            <asp:HiddenField ID="hdnFilterValue" runat="server" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtpayprocess" runat="server" Width="10px" BackColor="LightGreen"></asp:TextBox>
                            &nbsp; Payment Processed
                        </div>
                    </asp:Panel>
                </div>
                <div class="clear">
                </div>
                <asp:GridView ID="gvJobExpDetail" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr"
                    DataKeyNames="JobId" OnRowCommand="gvJobExpDetail_RowCommand" AllowPaging="True" AllowSorting="True" CssClass="table" PageSize="20"
                    PagerSettings-Position="TopAndBottom" DataSourceID="DataSourceJobExpenseDetails" Style="white-space: normal" OnRowDataBound="gvJobExpDetail_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" ItemStyle-Width="6%">
                            <ItemTemplate>
                                <center>
                                    <asp:ImageButton ID="imgbtnDocument" runat="server" ImageUrl="~/Images/file.gif" Width="15px" Height="18px"
                                        CommandArgument='<%#Eval("lid")%>' CommandName="DownloadDoc" ToolTip="Download Documents." 
                                        Style="padding-right: 0px; margin-right: 3px; padding-left: 1px" />
                                    <asp:ImageButton ID="imgbtnCancelJob" runat="server" ImageUrl="~/Images/cross.png"
                                        CommandArgument='<%#Eval("lid") %>' CommandName="cancel" Width="16px" Height="18px" ToolTip="Cancel Job Expense."></asp:ImageButton>
                                    <%--  <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/cross.png"
                                    CommandArgument='<%#Eval("lid") %>' CommandName="cancel" Width="16px" Height="18px" ToolTip="Cancel Job Expense."
                                    Style="padding-right: 1px; margin-right: 0px; padding-left: 1px"></asp:ImageButton>--%>
                                </center>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsProcess" runat="server" ToolTip="Payment Process" CommandName="PayProcess" /><%-- AutoPostBack="true" OnCheckedChanged="chkIsProcess_CheckedChanged"/>--%>
                                <asp:Label ID="lblIsPaymentChk" runat="server" Text='<%#Eval("IsPaymentProcess") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblPaymentId" runat="server" Text='<%#Eval("lid") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BS Job No" SortExpression="JobRefNo" ItemStyle-Width="12%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkJobRefNo" ToolTip="Add Job Expense Payment Details." CommandName="addpayment" runat="server" Text='<%#Eval("JobRefNo") %>'
                                    CommandArgument='<%# Eval("lid") + "," + Eval("JobId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" SortExpression="JobRefNo" ReadOnly="true" Visible="false" />
                        <asp:BoundField DataField="Consignee" HeaderText="Consignee" SortExpression="Consignee" ReadOnly="true" ItemStyle-Width="11%" />
                        <asp:BoundField DataField="BranchName" HeaderText="Branch" SortExpression="BranchName" ReadOnly="true" ItemStyle-Width="6%" />
                        <asp:BoundField DataField="PaymentTypeName" HeaderText="Payment Type" SortExpression="PaymentTypeName" ReadOnly="true" ItemStyle-Width="2%" />
                        <asp:BoundField DataField="ExpenseTypeName" HeaderText="Expense Type" SortExpression="ExpenseTypeName" ReadOnly="true" />
                        <%--<asp:BoundField DataField="DutyAmount" HeaderText="Duty Amount" SortExpression="DutyAmount" ReadOnly="true" />--%>
                        <asp:BoundField DataField="Total_Amnt" HeaderText="Total Amnt" SortExpression="Total_Amnt" ReadOnly="true" />
                        <asp:BoundField DataField="PaidTo" HeaderText="Paid To" SortExpression="PaidTo" ReadOnly="true" ItemStyle-Width="9%" />
                        <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" ReadOnly="true" ItemStyle-Width="11%" />
                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy" ReadOnly="true" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss tt}" ReadOnly="true" />
                        <%--  <asp:BoundField DataField="ApprovedBy" HeaderText="Approved By" SortExpression="ApprovedBy" ReadOnly="true" />--%>
                        <asp:BoundField DataField="ApprovedDate" HeaderText="Approved Date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss tt}" ReadOnly="true" />
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>

            <%--  START : MODAL POP-UP FOR DOCUMENTS DOWNLOAD  --%>

            <div id="divContractCopy">
                <cc1:modalpopupextender id="mpeContractCopy" runat="server" cachedynamicresults="false"
                    popupcontrolid="pnlContractCopy" cancelcontrolid="imgbtnDoc" targetcontrolid="lnkbtnContractCopy" backgroundcssclass="modalBackground" dropshadow="true">
                </cc1:modalpopupextender>
                <asp:Panel ID="pnlContractCopy" runat="server" CssClass="ModalPopupPanel" Width="350px">
                    <div class="header">
                        <div class="fleft">
                            Download Documents
                        </div>
                        <div class="fright">
                            <asp:ImageButton ID="imgbtnDoc" ImageUrl="../Images/delete.gif" runat="server" ToolTip="Close" />
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

            <div>
                <asp:SqlDataSource ID="DataSourceJobExpenseDetails" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetApprovedPayment" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="lUser" SessionField="UserId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>

            <%--  START : MODAL POP-UP FOR CANCEL EXPENSE  --%>

            <div id="divCancelExpense">
                <cc1:modalpopupextender id="mpeCancelExpense" runat="server" cachedynamicresults="false" popupcontrolid="pnlCancelExpense" 
                    cancelcontrolid="imgbtnCancelExp" targetcontrolid="hdnCancelExp" backgroundcssclass="modalBackground" dropshadow="true">
                </cc1:modalpopupextender>
                <asp:Panel ID="pnlCancelExpense" runat="server" CssClass="ModalPopupPanel" Width="600px">
                    <div class="header">
                        <div class="fleft">
                            <asp:Label ID="lblPopupName" runat="server"></asp:Label>
                            <asp:HiddenField ID="hdnRequestFor" runat="server" />
                        </div>
                        <div class="fright">
                            <asp:ImageButton ID="imgbtnCancelExp" ImageUrl="../Images/delete.gif" runat="server"
                                ToolTip="Close" />
                        </div>
                    </div>
                    <!-- Lists Of All Documents -->
                    <div id="Div1" runat="server" style="max-height: 550px; overflow: auto; padding: 5px">
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
                                        <asp:Button ID="btnCancelJob" runat="server"
                                            Text="Cancel Job" OnClick="btnCancelJob_OnClick" />
                                        <%----%>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div>
                <asp:HiddenField ID="hdnCancelExp" runat="server"></asp:HiddenField>
            </div>

            <%--  END   : MODAL POP-UP FOR Cancel EXPENSE  --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


