<%@ Page Title="Task Completed" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CompletedCustomerTaskList.aspx.cs" Inherits="CustomerTask_CompletedCustomerTaskList" %>

<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />

    <fieldset>
        <legend>Completed Customer Task</legend>
        <!-- Filter Content Start-->
        <div class="fleft">
            <uc1:DataFilter ID="DataFilter1" runat="server" />
        </div>

        <!-- Filter Content END-->
        <br />
        <br />
        <br />
        <div>
            <asp:LinkButton ID="lnkPortXls" runat="server" OnClick="lnkPortXls_Click">
                <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
            </asp:LinkButton>
        </div>
        <asp:GridView ID="GridComplitedExelCustomerTask" PagerStyle-CssClass="pgr" CssClass="table" runat="server"
            AutoGenerateColumns="false" DataSourceID="PendingCustomerTaskDataSource" Visible="false">

            <columns>
                        <asp:TemplateField HeaderText="Sl">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex +1 %>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                 <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" SortExpression="CustomerName" />
                <asp:BoundField DataField="ConsigneeName" HeaderText="Consignee Name" SortExpression="ConsigneeName" />
                <asp:BoundField DataField="AssignTaskBy" HeaderText="Task Created By" SortExpression="AssignTaskBy" />
                <asp:BoundField DataField="BranchName" HeaderText="Branch Name" SortExpression="BranchName" />
                <asp:BoundField DataField="EmpName" HeaderText="Task Assign To" SortExpression="EmpName" />
                <asp:BoundField DataField="Subject" HeaderText="Subject" SortExpression="Subject" />
                 <asp:BoundField DataField="Billable" HeaderText="Billable" SortExpression="Billable" />
                <asp:BoundField DataField="JobRefNo" HeaderText="Job No" SortExpression="JobRefNo" />
                <asp:BoundField DataField="EstimateDate" HeaderText="Estimate Date" SortExpression="EstimateDate" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="StartDate" HeaderText="Start Date" SortExpression="StartDate" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="Priority" HeaderText="Priority" SortExpression="Priority" />
                <asp:BoundField DataField="ActivityType" HeaderText="Activity Type" SortExpression="ActivityType" />
                <asp:BoundField DataField="ActivityDetail" HeaderText="Activity Details" SortExpression="ActivityDetail" />
                <asp:BoundField DataField="FollowupUpdate" HeaderText="Follow Up Update" SortExpression="FollowupUpdate" />
                <asp:BoundField DataField="FollowupDate" HeaderText="Follow Up Date" SortExpression="FollowupUpdate" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="sName" HeaderText="Task Updated Person Name" SortExpression="sName" />
                <asp:BoundField DataField="UpdDate" HeaderText="Task Updated Date" SortExpression="UpdDate" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" NullDisplayText="Pending" />

                            </columns>
        </asp:GridView>

        <asp:GridView ID="GridComplitedMiscellaneousCustomerTask" runat="server" AutoGenerateColumns="False"
            Width="100%" PagerStyle-CssClass="pgr" DataSourceID="PendingMiscellaneousCustomerTaskDataSorce"
            CssClass="table" CellPadding="4" DataKeyNames="LID" OnRowDataBound="GridComplitedMiscellaneousCustomerTask_RowDataBound"
            AllowPaging="True" AllowSorting="True" PageSize="20" OnRowCommand="GridComplitedMiscellaneousCustomerTask_RowCommand">
            <columns>
            <asp:TemplateField HeaderText="Sl">
                <ItemTemplate>
                    <%#Container.DataItemIndex +1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="lnkSelect" runat="server" CommandName="select" Text="Select"
                        CommandArgument='<%#Eval("LID")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" SortExpression="CustomerName" />
            <asp:BoundField DataField="ConsigneeName" HeaderText="Consignee Name" SortExpression="ConsigneeName" />
            <asp:BoundField DataField="BranchName" HeaderText="Branch Name" SortExpression="BranchName" />
            <asp:BoundField DataField="AssignTaskBy" HeaderText="Task Created By" SortExpression="AssignTaskBy" />
            <asp:BoundField DataField="EmpName" HeaderText="Task Assign To" SortExpression="EmpName" />
            <asp:BoundField DataField="Subject" HeaderText="Subject" SortExpression="Subject" />
            <asp:BoundField DataField="JobRefNo" HeaderText="Job No" SortExpression="JobRefNo" />
            <asp:BoundField DataField="StartDate" HeaderText="Start Date"
                SortExpression="StartDate" DataFormatString="{0:dd/MM/yyyy}"  />
             <asp:BoundField DataField="Priority" HeaderText="Priority"
                SortExpression="Priority" />
             <asp:BoundField DataField="ActivityType" HeaderText="Activity Type"
                SortExpression="ActivityType" />
              <asp:BoundField DataField="Status" HeaderText="Status"
                SortExpression="Status" NullDisplayText="Pending" />
          
             <asp:TemplateField HeaderText="Download">
            <ItemTemplate>
                <asp:LinkButton ID="lnkDownload" runat="server" Text="Document"  CommandName="download" 
                    CommandArgument='<%#Eval("CustFilePath") %>' Visible="true">
                </asp:LinkButton>
                
            </ItemTemplate>
        </asp:TemplateField>

        </columns>
        </asp:GridView>
    </fieldset>
    <div>
        <asp:SqlDataSource ID="PendingCustomerTaskDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="BS_GetComplitedCustomerTaskDeatilEXLFile" SelectCommandType="StoredProcedure">
            <selectparameters>
                <asp:SessionParameter Name="UserId" SessionField="UserId" />

            </selectparameters>
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="PendingMiscellaneousCustomerTaskDataSorce" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="GetComplitedMiscellaneousCustomerTaskList" SelectCommandType="StoredProcedure">
            <selectparameters>
                <asp:SessionParameter Name="UserId" SessionField="UserId" />

            </selectparameters>
        </asp:SqlDataSource>
    </div>

</asp:Content>

