<%@ Page Title="Additional Expense Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
        CodeFile="AdditionalExpenseReport.aspx.cs" Inherits="Reports_AdditionalExpenseReport" EnableEventValidation="false" Culture="en-GB"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingPost" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPendingPost" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false" ></asp:Label>
            </div>
            <div class="clear"></div>
            <fieldset>
            <legend>Additional Expense</legend>    
            <div class="fleft">
                <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                    <asp:Image ID="Image1" runat="server" ImageAlign="AbsMiddle" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
            </div>
            <div class="clear">
                <uc1:DataFilter ID="DataFilter1" runat="server" />
            </div>
            <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                DataKeyNames="ExpenseID" PageSize="20" AllowPaging="True" AllowSorting="True"
                PagerSettings-Position="TopAndBottom" PagerStyle-CssClass="pgr" 
                DataSourceID="ExpenseSqlDataSource" OnRowCommand="gvDetail_RowCommand">
                <Columns>
                <asp:TemplateField HeaderText="Sl" >
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Job No" SortExpression="JobRefNO">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkJobRefNo" CommandName="select" runat="server" Text='<%#Eval("JobRefNO") %>'
                            CommandArgument='<%#Eval("JobID")+";"+ Eval("ExpenseId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="JobRefNO" HeaderText="Job No" Visible="False" />
                <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                <asp:BoundField DataField="BranchName" HeaderText="Branch" SortExpression="Branch" />
                <asp:BoundField DataField="RMSNonRMS" HeaderText="RMS/NonRMS" SortExpression="RMSNonRMS" />
                <asp:BoundField DataField="HODName" HeaderText="HOD" SortExpression="HODName" />
                <asp:BoundField DataField="HODApprovalDate" HeaderText="HoD Approved Date" DataFormatString="{0:dd/MM/yyyy}"
                    SortExpression="HODApprovalDate" />
                <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName" />
                <asp:BoundField DataField="Billable" HeaderText="Billable ?" SortExpression="Billable" />
                <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />
                <%--<asp:BoundField DataField="MgmtApprovalName" HeaderText="Approved By" SortExpression="MgmtApprovalName" />
                <asp:BoundField DataField="ApprovalDate" HeaderText="Approval Date" DataFormatString="{0:dd/MM/yyyy}"
                    SortExpression="ApprovalDate" />--%>
                <asp:BoundField DataField="ExpenseName" HeaderText="Type" SortExpression="ExpenseName" />
                <%--<asp:BoundField DataField="ApprovalRemark" HeaderText="Approval Remark" SortExpression="ApprovalRemark" />--%>
                <asp:BoundField DataField="ExpenseDate" HeaderText="Expense Date" DataFormatString="{0:dd/MM/yyyy}"
                    SortExpression="ExpenseDate" />
                <asp:TemplateField HeaderText="User Remark" SortExpression="ExpenseRemark">
                    <ItemTemplate>
                        <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                        <asp:Label ID="lblExpenseRemark" runat="server" 
                            Text='<%# Eval("ExpenseRemark")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
                <PagerTemplate>
                    <asp:GridViewPager runat="server" />
                </PagerTemplate>
            </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="ExpenseSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="rtpAdditionalExpense" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


