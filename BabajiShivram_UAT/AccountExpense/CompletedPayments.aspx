<%@ Page Title="Completed Payments" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CompletedPayments.aspx.cs"
    Inherits="AccountExpense_CompletedPayments" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingForm13" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="images/processing.gif" />
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
                <legend>Completed Payments</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                        </div>
                        <div class="fleft" style="margin-left: 2px; padding-top: 3px;">
                            <asp:LinkButton ID="lnkexport" runat="server" Text="Detail" OnClick="lnkexport_Click">
                                <asp:Image ID="Image1" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:LinkButton ID="lnkexport1" runat="server" Text="Summary" OnClick="lnkexport1_Click">
                                <asp:Image ID="Image2" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>
                        </div>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                         <asp:TextBox ID="txtPendingFunds" runat="server" Width="1.5%" BackColor="LightGoldenrodYellow" ReadOnly="true"></asp:TextBox>
                        Approval Pending  &nbsp;&nbsp;
                           <asp:TextBox ID="txtHold" runat="server" Width="1.5%" BackColor="LightSalmon" ReadOnly="true"></asp:TextBox>
                        Hold  &nbsp;&nbsp;
                                <asp:TextBox ID="txtApprovedReq" runat="server" Width="1.5%" BackColor="LightGreen" ReadOnly="true"></asp:TextBox>
                        Approved
                                <%--<asp:TextBox ID="txtPayment" runat="server" Width="1.5%" BackColor="#FFF" ReadOnly="true"></asp:TextBox>
                        Payment Completed--%>
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
                        <asp:TemplateField HeaderText="BS Job No" SortExpression="JobRefNo" ItemStyle-Width="12%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkJobRefNo" ToolTip="Add Job Expense Payment Details." CommandName="addpayment" runat="server" Text='<%#Eval("JobRefNo") %>'
                                    CommandArgument='<%#Eval("lid")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" SortExpression="JobRefNo" ReadOnly="true" Visible="false" />
                        <asp:BoundField DataField="ConsigneeName" HeaderText="Consignee Name" SortExpression="ConsigneeName" ReadOnly="true" />
                        <asp:BoundField DataField="BranchName" HeaderText="Branch" SortExpression="BranchName" ReadOnly="true" />
                        <asp:BoundField DataField="CompanyName" HeaderText="Company" SortExpression="CompanyName" ReadOnly="true" />
                        <asp:BoundField DataField="PaymentTypeName" HeaderText="Payment Type" SortExpression="PaymentTypeName" ReadOnly="true" />
                        <asp:BoundField DataField="ExpenseTypeName" HeaderText="Expense Type" SortExpression="ExpenseTypeName" ReadOnly="true" />
                        <asp:BoundField DataField="Total_Amnt" HeaderText="Total Amount" SortExpression="Total_Amnt" ReadOnly="true" />
                        <asp:BoundField DataField="PaidTo" HeaderText="Paid To" SortExpression="PaidTo" ReadOnly="true" ItemStyle-Width="12%" />
                        <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" ReadOnly="true" ItemStyle-Width="10%" />
                        <asp:BoundField DataField="CreatedBy" HeaderText="Payment By" SortExpression="CreatedBy" ReadOnly="true" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="Payment Date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" ReadOnly="true" ItemStyle-Width="12%" />
                        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" ReadOnly="true" Visible="false" />
                        <asp:BoundField DataField="ApprovedDate" HeaderText="Approved/Hold Date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" Visible="false" ReadOnly="true" />
                        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" ReadOnly="true" ItemStyle-Width="5%" Visible="false" />
                        <%--<asp:BoundField DataField="Remark" HeaderText="Reason" SortExpression="Remark" ReadOnly="true" ItemStyle-Width="10%" />--%>
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
                <asp:SqlDataSource ID="DataSourceJobExpenseDetails" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetExpensePayment" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="lUser" SessionField="UserId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

