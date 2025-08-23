<%@ Page Title="Track Expense Booking" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="TrackExpenseBooking.aspx.cs" Inherits="AccountExpense_TrackExpenseBooking" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    &nbsp;&nbsp;<asp:Button ID="btnNew" runat="server" Text="New Expense Booking" OnClick="btnNew_Click" />
    <fieldset><legend>Expense Booking</legend>
        <div align="center">
            <asp:Label ID="lblMessage" runat="server" CssClass="errorMsg" EnableViewState="false" ></asp:Label>
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
        <asp:GridView ID="gvDetail" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="BookingId"
            AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataSourceID="BookingSqlDataSource" 
            AllowPaging="True" AllowSorting="True" PageSize="40" PagerSettings-Position="TopAndBottom"
            Width="100%">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Job No" SortExpression="JobRefNo">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkJobRefNo" CommandName="select" runat="server" Text='<%#Eval("JobRefNo") %>'
                            CommandArgument='<%#Eval("BookingId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="JobRefNo" HeaderText="Job No" Visible="False" />
                <asp:BoundField DataField="PaidTo" HeaderText="Paid To" SortExpression="PaidTo" />
                <asp:BoundField DataField="PayModeName" HeaderText="Pay Mode" SortExpression="PayModeName" />
                <asp:BoundField DataField="RIMorAGName" HeaderText="RIM/AG" SortExpression="RIMorAGName" />
                <asp:BoundField DataField="InvoiceTotalAmount" HeaderText="INV Total Amount" SortExpression="InvoiceTotalAmount" />
                <asp:BoundField DataField="InvoiceTaxableAmount" HeaderText="INV Tax Amount" SortExpression="InvoiceTaxableAmount" />
                <asp:BoundField DataField="ApproveStatus" HeaderText="Status" SortExpression="ApproveStatus" />
                <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" />
                <asp:BoundField DataField="CreatedDate" HeaderText="Booking Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="CreatedDate" />
            </Columns>
            <PagerTemplate>
                <asp:GridViewPager runat="server" />
            </PagerTemplate>
        </asp:GridView>
        </fieldset>
        <div>
            <asp:SqlDataSource ID="BookingSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="AC_FAGetExpenseBookingRIM" SelectCommandType="StoredProcedure">
            </asp:SqlDataSource>
        </div>
</asp:Content>

