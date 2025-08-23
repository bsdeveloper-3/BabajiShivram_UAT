<%@ Page Title="Payment Request Details" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" EnableEventValidation="false"
    CodeFile="PaymentRequests.aspx.cs" Inherits="AccountExpense_PaymentRequests" Culture="en-GB" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Scriptmanager runat="server" id="ScriptManager1" scriptmode="Release" />
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
                <legend>Payment Request Details</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:datafilter id="DataFilter1" runat="server" />
                        </div>

                        <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click" Visible="true">
                            <asp:Image ID="Image1" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                        </asp:LinkButton>

                        &nbsp; &nbsp;&nbsp;
                                <b>Total Amount : </b>
                        <asp:Label ID="lblTotalAmount" runat="server" Font-Underline="true" Font-Bold="true"></asp:Label>
                         &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;

                                <asp:TextBox ID="txtPendingFunds" runat="server" Width="1.5%" BackColor="LightGoldenrodYellow" ReadOnly="true" ></asp:TextBox>
                        Approval Pending               
                                <asp:TextBox ID="txtHold" runat="server" Width="1.5%" BackColor="#87AFC7" ReadOnly="true"></asp:TextBox>
                        Hold  
                                <asp:TextBox ID="txtApprovedReq" runat="server" Width="1.5%" BackColor="LightGreen" ReadOnly="true"></asp:TextBox>
                        Approved
                                <asp:TextBox ID="txtPayment" runat="server" Width="1.5%" BackColor="#FFBCFF" ReadOnly="true"></asp:TextBox>
                        Payment Completed
                         <asp:TextBox ID="txtRejectJob" runat="server" Width="1.5%" BackColor="#E8ADAA" ReadOnly="true"></asp:TextBox>
                        Rejected Payment

                    </asp:Panel>
                </div>
                <div class="clear">
                </div>
                <%--    <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnDocument" runat="server" ImageUrl="~/Images/file.gif" Width="17px" Height="18px"
                                    CommandArgument='<%#Eval("lid") %>' CommandName="DownloadDoc" ToolTip="Download Documents." Style="padding-right: 0px; margin-right: 0px; padding-left: 1px" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                <asp:GridView ID="gvJobExpDetail" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr"
                    DataKeyNames="JobId" OnRowCommand="gvJobExpDetail_RowCommand" AllowPaging="True" AllowSorting="True" CssClass="table" PageSize="20"
                    PagerSettings-Position="TopAndBottom" DataSourceID="DataSourceJobExpenseDetails" Style="white-space: normal" OnRowDataBound="gvJobExpDetail_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="BS Job No" SortExpression="JobRefNo" ItemStyle-Width="12%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkJobRefNo" ToolTip="Add Job Expense Payment Details." CommandName="addpayment" runat="server" Text='<%#Eval("JobRefNo") %>'
                                    CommandArgument='<%#Eval("lid") + "," + Eval("StatusId")%>'/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" SortExpression="JobRefNo" ReadOnly="true" Visible="false" />
                        <asp:BoundField DataField="ConsigneeName" HeaderText="Consignee Name" SortExpression="ConsigneeName" ReadOnly="true" />
                        <asp:BoundField DataField="BranchName" HeaderText="Branch" SortExpression="BranchName" ReadOnly="true" />
                        <asp:BoundField DataField="PaymentTypeName" HeaderText="Payment Type" SortExpression="PaymentTypeName" ReadOnly="true" />
                        <asp:BoundField DataField="ExpenseTypeName" HeaderText="Expense Type" SortExpression="ExpenseTypeName" ReadOnly="true" />
                        <asp:BoundField DataField="Total_Amnt" HeaderText="Total Amnt" SortExpression="Total_Amnt" ReadOnly="true" />
                        <asp:BoundField DataField="PaidTo" HeaderText="Paid To" SortExpression="PaidTo" ReadOnly="true" ItemStyle-Width="12%" />
                        <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" ReadOnly="true" ItemStyle-Width="10%" />
                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy" ReadOnly="true" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss tt}" ReadOnly="true" ItemStyle-Width="12%" />
                        <asp:BoundField DataField="RefNo" HeaderText="Payment Ref No" SortExpression="RefNo" ReadOnly="true" Visible="true"/>
                        <asp:BoundField DataField="PaymentDate" HeaderText="Payment Date" SortExpression="PaymentDate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" Visible="true" />
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
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>

            <div>
                <%--  <asp:SqlDataSource ID="DataSourceJobExpenseDetails" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetPaymentRequests" SelectCommandType="StoredProcedure"></asp:SqlDataSource>--%>
                <asp:SqlDataSource ID="DataSourceJobExpenseDetails" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetExpensePayment" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="lUser" SessionField="UserId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>

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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
