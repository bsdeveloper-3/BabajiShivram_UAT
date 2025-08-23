<%@ Page Title="Pending Job Expense" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PendingJobExpense.aspx.cs"
    Inherits="ExportCHA_PendingJobExpense" Culture="en-GB"
    EnableEventValidation="false" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional"
        RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div class="clear"></div>
            <fieldset>
                <legend>Job Detail</legend>
                <div class="clear">
                    <uc1:DataFilter ID="DataFilter1" runat="server" />
                </div>
                <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                    DataKeyNames="JobId" PageSize="20" AllowPaging="True" AllowSorting="True"
                    PagerSettings-Position="TopAndBottom" PagerStyle-CssClass="pgr" OnRowCommand="gvJobDetail_RowCommand"
                    OnPreRender="gvJobDetail_PreRender" DataSourceID="PendingExpenseDetails">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="BS Job No" SortExpression="JobRefNo">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkJobRefNo" CommandName="select" runat="server" Text='<%#Eval("JobRefNo") %>'
                                    CommandArgument='<%#Eval("JobId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="JobRefNo" HeaderText="Job No" SortExpression="JobRefNo" Visible="False" />
                        <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                        <asp:BoundField DataField="CustRefNo" HeaderText="Cust Ref No" SortExpression="CustRefNo" />
                        <asp:BoundField DataField="Shipper" HeaderText="Shipper" SortExpression="Shipper" />
                        <asp:BoundField DataField="ConsigneeName" HeaderText="Consignee" SortExpression="ConsigneeName" />
                        <asp:BoundField DataField="TransMode" HeaderText="Mode" SortExpression="TransMode" />
                        <asp:BoundField DataField="PortOfLoading" HeaderText="Port of Loading" SortExpression="PortOfLOading" />
                        <asp:BoundField DataField="PortOfDischarge" HeaderText="Port of Discharge" SortExpression="PortOfDischarge" />
                        <asp:BoundField DataField="UserName" HeaderText="Job Created By" SortExpression="UserName" />
                        <asp:BoundField DataField="JobCreatedDate" HeaderText="Job Created Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="dtDate" />
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="PendingExpenseDetails" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="EX_GetJobDetailForExpense" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

