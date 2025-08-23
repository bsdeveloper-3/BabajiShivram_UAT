<%@ Page Title="Transport Memo" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="TrackTransMemo.aspx.cs" Inherits="AccountTransport_TrackTransMemo" EnableEventValidation="false" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upFillDetails" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        </div>
    <asp:UpdatePanel ID="upFillDetails" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
                
            <div align="center">
                <asp:Label ID="lblMessage" runat="server" CssClass="errorMsg" Font-Size="Large"></asp:Label>
            </div>
            <fieldset><legend>Track Transport Memo</legend>
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
                <div class="fleft" style="margin-left:30px; padding-top:3px;">
                </div>
            </asp:Panel>
            </div>
            <div class="m clear"></div>
            <asp:GridView ID="gvDetail" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="lId"
                AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataSourceID="InvoiceSqlDataSource" 
                AllowPaging="True" AllowSorting="True" PageSize="40" PagerSettings-Position="TopAndBottom"
                Width="100%" OnRowCommand="gvDetail_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Memo Ref No">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkMemoRefNo" CommandName="select" runat="server" Text='<%#Eval("PayMemoRefNo") %>'
                            CommandArgument='<%#Eval("lid") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PayMemoRefNo" HeaderText="Memo Ref No" SortExpression="PayMemoRefNo" Visible="false" />
                <asp:BoundField DataField="VendorName" HeaderText="Transporter" SortExpression="VendorName" />
                <asp:BoundField DataField="MemoAmount" HeaderText="Amount" SortExpression="MemoAmount" />
                <asp:BoundField DataField="ApprovedBy" HeaderText="Approved By" SortExpression="ApprovedBy" />
                <asp:BoundField DataField="ApprovedDate" HeaderText="Approval Date" SortExpression="ApprovedDate" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField DataField="PaymentUser" HeaderText="Pay User" SortExpression="PaymentUser" />
                <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName" />
                <asp:BoundField DataField="PayDate" HeaderText="Pay Date" SortExpression="updDate" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField DataField="dtDate" HeaderText="Memo Date" SortExpression="dtDate" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField DataField="UserName" HeaderText="User" SortExpression="UserName" />
            </Columns>
            <PagerTemplate>
                <asp:GridViewPager runat="server" />
            </PagerTemplate>
        </asp:GridView>
        </fieldset>
        <div>
            <asp:SqlDataSource ID="InvoiceSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="TRS_GetTransMemo" SelectCommandType="StoredProcedure">
            </asp:SqlDataSource>
        </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


