<%@ Page Title="Track Issued Instrument" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
 CodeFile="TrackIssueInstrument.aspx.cs" Inherits="AccountExpense_TrackIssueInstrument" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    &nbsp;&nbsp;<asp:Button ID="btnNew" runat="server" Text="Issue New Instrument" OnClick="btnNew_Click" />
    <fieldset><legend>Issue Instrument</legend>
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
        <asp:GridView ID="gvDetail" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="ChequeIssueId"
            AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataSourceID="ChequeSqlDataSource" 
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
                            CommandArgument='<%#Eval("ChequeIssueId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="JobRefNo" HeaderText="Job No" Visible="False" />
                <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                <asp:BoundField DataField="PayTo" HeaderText="PayTo" SortExpression="PayTo" />
                <asp:BoundField DataField="ChequeNo" HeaderText="Cheque No" SortExpression="ChequeNo" />
                <asp:BoundField DataField="ChequeDate" HeaderText="Cheque Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ChequeDate" />
                <asp:BoundField DataField="BankName" HeaderText="Bank Name" SortExpression="BankName" />
                <asp:BoundField DataField="IssuedBy" HeaderText="Issued By" SortExpression="IssuedBy" />
                <asp:BoundField DataField="IssueDate" HeaderText="Issue Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="IssueDate" />
            </Columns>
            <PagerTemplate>
                <asp:GridViewPager runat="server" />
            </PagerTemplate>
        </asp:GridView>
        </fieldset>
        <div>
            <asp:SqlDataSource ID="ChequeSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="AC_FAGetInstrument" SelectCommandType="StoredProcedure">
            </asp:SqlDataSource>
        </div>
</asp:Content>

