<%@ Page Title="Stamp Excel" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
        CodeFile="StampExcel.aspx.cs" Inherits="AccountExpense_StampExcel" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <fieldset><legend>Stamp Duty FA Posting</legend>
        <div align="left">
            <asp:Label ID="lblMessage" runat="server" CssClass="errorMsg" EnableViewState="false" ></asp:Label>
            Date
            <asp:TextBox ID="txtReportDate" AutoPostBack="true" runat="server" MaxLength="20" Width="100px"></asp:TextBox>
            <cc1:CalendarExtender ID="calStatusDate" runat="server" EnableViewState="False"
                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtReportDate">
            </cc1:CalendarExtender>
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
        <div class="m clear"></div>
        <asp:GridView ID="gvDetail" runat="server" AutoGenerateColumns="False" CssClass="table" 
            AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataSourceID="StampSqlDataSource" 
            AllowPaging="True" AllowSorting="True" PageSize="40" PagerSettings-Position="TopAndBottom"
            Width="100%" >
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Customer" HeaderText="Customer"/>
                <%--<asp:BoundField DataField="ConsigneeCode" HeaderText="ConsigneeCode"/>--%>
                <asp:BoundField DataField="charge" HeaderText="charge"/>
                <asp:BoundField DataField="name" HeaderText="name"/>
                <asp:BoundField DataField="paid_to" HeaderText="paid_to"/>
                <asp:BoundField DataField="rno" HeaderText="rno"/>
                <asp:BoundField DataField="DATE" HeaderText="DATE"/>
                <asp:BoundField DataField="vcode" HeaderText="vcode"/>
                <asp:BoundField DataField="GSTIN" HeaderText="GSTIN"/>
                <asp:BoundField DataField="POS" HeaderText="POS"/>
                <asp:BoundField DataField="ramount" HeaderText="ramount"/>
                <asp:BoundField DataField="stampduty" HeaderText="stampduty"/>
                <asp:BoundField DataField="admin" HeaderText="admin"/>
                <asp:BoundField DataField="fhsn_sc" HeaderText="fhsn_sc"/>
                <asp:BoundField DataField="CGSTRT" HeaderText="CGSTRT"/>
                <asp:BoundField DataField="CGSTAMT" HeaderText="CGSTAMT"/>
                <asp:BoundField DataField="SGSTRT" HeaderText="SGSTRT"/>
                <asp:BoundField DataField="SGSTAMT" HeaderText="SGSTAMT"/>
                <asp:BoundField DataField="IGSTRT" HeaderText="IGSTRT"/>
                <asp:BoundField DataField="IGSTAMT" HeaderText="IGSTAMT"/>
                <asp:BoundField DataField="par_code" HeaderText="par_code"/>
                <asp:BoundField DataField="par_name" HeaderText="par_name"/>
                <asp:BoundField DataField="ref" HeaderText="ref"/>
                <asp:BoundField DataField="cstcode1" HeaderText="cstcode1"/>
                <asp:BoundField DataField="cstcode2" HeaderText="cstcode2"/>
                <asp:BoundField DataField="cstcode3" HeaderText="cstcode3"/>
                <asp:BoundField DataField="cstcode4" HeaderText="cstcode4"/>
                <asp:BoundField DataField="Amount" HeaderText="Amount"/>
                <asp:BoundField DataField="cgstpar_code" HeaderText="cgstpar_code"/>
                <asp:BoundField DataField="sgstpar_code" HeaderText="sgstpar_code"/>
                <asp:BoundField DataField="igstpar_code" HeaderText="igstpar_code"/>
                <asp:BoundField DataField="salestype" HeaderText="salestype"/>
                <asp:BoundField DataField="frankingcode" HeaderText="frankingcode"/>
                <asp:BoundField DataField="jbtype" HeaderText="jbtype"/>
                <asp:BoundField DataField="hsn_sc" HeaderText="hsn_sc"/>
                </Columns>
            <PagerTemplate>
                <asp:GridViewPager runat="server" />
            </PagerTemplate>
        </asp:GridView>
        </fieldset>
        <div>
            <asp:SqlDataSource ID="StampSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="INV_GetExcelForStamp" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:ControlParameter Name="ReportDate" ControlID="txtReportDate" PropertyName="Text" Type="datetime" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
</asp:Content>

