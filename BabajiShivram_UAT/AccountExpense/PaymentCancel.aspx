<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PaymentCancel.aspx.cs" 
    Inherits="AccountExpense_PaymentCancel" EnableEventValidation="false" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

      <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="panPaymentCancel" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../Images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="panPaymentCancel" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div class="clear">
            </div>

               <fieldset class="fieldset-AutoWidth">
                <legend>Cancelled Payments</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                        </div>
                        <div class="fleft" style="margin-left: 2px; padding-top: 3px;">
                            <asp:LinkButton ID="lnkexport" runat="server"  OnClick="lnkexport_Click">
                                <asp:Image ID="Image1" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>
                        </div>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                       <%--  <asp:TextBox ID="txtPendingFunds" runat="server" Width="1.5%" BackColor="LightGoldenrodYellow" ReadOnly="true" ></asp:TextBox>
                        Approval Pending  &nbsp;&nbsp;
                           <asp:TextBox ID="txtHold" runat="server" Width="1.5%" BackColor="LightSalmon" ReadOnly="true"></asp:TextBox>
                        Hold  &nbsp;&nbsp;
                                <asp:TextBox ID="txtApprovedReq" runat="server" Width="1.5%" BackColor="LightGreen" ReadOnly="true"></asp:TextBox>
                        Approved--%>
                                <%--<asp:TextBox ID="txtPayment" runat="server" Width="1.5%" BackColor="#FFF" ReadOnly="true"></asp:TextBox>
                        Payment Completed--%>

                    </asp:Panel>
                </div>
                <div class="clear">
                </div>
                <asp:GridView ID="gvJobExpDetail" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr"
                    DataKeyNames="JobId" AllowPaging="True" AllowSorting="True" CssClass="table" PageSize="20"
                    PagerSettings-Position="TopAndBottom" DataSourceID="DataSourceJobExpenseDetails" Style="white-space: normal" >  <%--OnRowDataBound="gvJobExpDetail_RowDataBound"--%>
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                          <asp:TemplateField HeaderText="BS Job No" SortExpression="JobRefNo" ItemStyle-Width="10%">
                            <ItemTemplate>
                               <%-- <asp:LinkButton ID="lnkJobRefNo" ToolTip="Add Job Expense Payment Details." CommandName="addpayment" runat="server" Text='<%#Eval("JobRefNo") %>'
                                    CommandArgument='<%#Eval("lid")%>' />--%>
                                <asp:Label ID="lblJobRefNo" runat="server" Text='<%#Eval("JobRefNo")%>'> </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" ReadOnly="true" ItemStyle-Width="8%" Visible="false"/>
                        <asp:BoundField DataField="Consignee" HeaderText="Consignee" SortExpression="Consignee" ReadOnly="true" ItemStyle-Width="10.3%" />
                        <asp:BoundField DataField="BranchName" HeaderText="Branch" SortExpression="BranchName" ReadOnly="true" ItemStyle-Width="6.3%" />
                        <asp:BoundField DataField="PlanningDate" HeaderText="Planning Date" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" ItemStyle-Width="4.3%" />
                        <asp:BoundField DataField="PaymentTypeName" HeaderText="Payment Type" SortExpression="PaymentTypeName" ReadOnly="true" ItemStyle-Width="4.3%" />
                        <asp:BoundField DataField="ExpenseTypeName" HeaderText="Expense Type" SortExpression="ExpenseTypeName" ReadOnly="true" ItemStyle-Width="5.3%" />
                        <asp:BoundField DataField="AdvanceDetail" HeaderText="Advance Detail" SortExpression="AdvanceDetail" ReadOnly="true" ItemStyle-Width="4.3%" />
                        <asp:BoundField DataField="Total_Amnt" HeaderText="Total Amount" SortExpression="Total_Amnt" ReadOnly="true" ItemStyle-Width="4.3%" />
                        <asp:BoundField DataField="PaidTo" HeaderText="Paid To" SortExpression="PaidTo" ReadOnly="true" ItemStyle-Width="7.3%" />
                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy" ReadOnly="true" ItemStyle-Width="5.3%" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss tt}" ReadOnly="true" ItemStyle-Width="9.3%" />
                        <asp:BoundField DataField="CancelBy" HeaderText="Cancelled By" SortExpression="CancelBy" ReadOnly="true" ItemStyle-Width="11.3%" />
                        <asp:BoundField DataField="CancelDate" HeaderText="Cancelled Date" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" ItemStyle-Width="9.3%" />
                        <asp:BoundField DataField="CancelReason" HeaderText="Cancelled Reason" ReadOnly="true" ItemStyle-Width="15.3%" />
                    <%--<asp:BoundField DataField="CancelDate" HeaderText="Cancel Date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss tt}" ReadOnly="true" ItemStyle-Width="9.3%" />--%>

                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="DataSourceJobExpenseDetails" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetPaymentCancel" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="lUser" SessionField="UserId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

