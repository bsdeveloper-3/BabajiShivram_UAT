<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="InvoiceExcel.aspx.cs" 
    Inherits="AccountExpense_InvoiceExcel" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <fieldset><legend>Invoice Booking</legend>
        <div align="center">
            <asp:Label ID="lblMessage" runat="server" CssClass="errorMsg" EnableViewState="false" ></asp:Label>
        </div>
        <asp:DropDownList ID="ddRIM" runat="server" AutoPostBack="true">
            <asp:ListItem Text="RIM" Value="1"></asp:ListItem>
            <asp:ListItem Text="Non-RIM" Value="0"></asp:ListItem>
        </asp:DropDownList>
        <asp:TextBox ID="txtReportDate" AutoPostBack="true" runat="server" MaxLength="20" Width="100px"></asp:TextBox>
        <AjaxToolkit:CalendarExtender ID="calStatusDate" runat="server" EnableViewState="False"
            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtReportDate">
        </AjaxToolkit:CalendarExtender>
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
            AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataSourceID="InvoiceSqlDataSource" 
            AllowPaging="True" AllowSorting="True" PageSize="40" PagerSettings-Position="TopAndBottom"
            Width="100%" >
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="BookType" HeaderText="Book Type"/>
                <asp:BoundField DataField="ExcelType" HeaderText="Excel Type"/>
                <asp:BoundField DataField="InvoiceType" HeaderText="Invoice Type"/>
                <asp:BoundField DataField="BillDate" HeaderText="BillDate"/>
                <asp:BoundField DataField="par_code" HeaderText="par_code"/>
                <asp:BoundField DataField="Ref" HeaderText="Ref"/>
                <asp:BoundField DataField="rAmount" HeaderText="rAmount"/>
                <asp:BoundField DataField="Charge" HeaderText="Charge"/>
                <asp:BoundField DataField="Charge Name" HeaderText="Charge Name"/>
                <asp:BoundField DataField="Paid_To" HeaderText="Paid_To"/>
                <asp:BoundField DataField="rno" HeaderText="rno"/>
                <asp:BoundField DataField="Date" HeaderText="Date"/>
                <asp:BoundField DataField="jvpcode" HeaderText="jvpcode"/>
                <asp:BoundField DataField="BillRecdDate" HeaderText="BillRecdDate"/>
                <asp:BoundField DataField="Narration" HeaderText="Narration"/>
                <asp:BoundField DataField="Chequeno" HeaderText="Chequeno"/>
                <asp:BoundField DataField="chequedt" HeaderText="chequedt"/>
                <asp:BoundField DataField="SER_NO" HeaderText="SER_NO"/>
                <asp:BoundField DataField="vcode" HeaderText="vcode"/>
                <asp:BoundField DataField="gstin" HeaderText="gstin"/>
                <asp:BoundField DataField="gstpos" HeaderText="gstpos"/>
                <asp:BoundField DataField="hsn_sc" HeaderText="hsn_sc"/>
                <asp:BoundField DataField="cgstrt" HeaderText="cgstrt"/>
                <asp:BoundField DataField="cgstamt" HeaderText="cgstamt"/>
                <asp:BoundField DataField="sgstrt" HeaderText="sgstrt"/>
                <asp:BoundField DataField="sgstamt" HeaderText="sgstamt"/>
                <asp:BoundField DataField="igstrt" HeaderText="igstrt"/>
                <asp:BoundField DataField="igstamt" HeaderText="igstamt"/>
                <asp:BoundField DataField="Amount" HeaderText="Amount"/>
                <asp:BoundField DataField="cgstpar_code" HeaderText="cgstpar_code"/>
                <asp:BoundField DataField="sgstpar_code" HeaderText="sgstpar_code"/>
                <asp:BoundField DataField="igstpar_code" HeaderText="igstpar_code"/>
                <asp:BoundField DataField="cstcode1" HeaderText="cstcode1"/>
                <asp:BoundField DataField="cstcode2" HeaderText="cstcode2"/>
                <asp:BoundField DataField="cstcode3" HeaderText="cstcode3"/>
                <asp:BoundField DataField="cstcode4" HeaderText="cstcode4"/>
                <asp:BoundField DataField="TDSJVCode" HeaderText="TDSJVCode"/>
                <asp:BoundField DataField="TDSAmt" HeaderText="TDSAmt"/>
                <asp:BoundField DataField="TDSCode" HeaderText="TDSCode"/>
                <asp:BoundField DataField="TDSGrossAmt" HeaderText="TDSGrossAmt"/>
                <asp:BoundField DataField="TDSRate" HeaderText="TDSRate"/>
                <asp:BoundField DataField="RCMApp" HeaderText="RCMApp"/>
                <asp:BoundField DataField="rcmCGStrt" HeaderText="rcmCGStrt"/>
                <asp:BoundField DataField="RCMCGSTAmt" HeaderText="RCMCGSTAmt"/>
                <asp:BoundField DataField="rcmSGStrt" HeaderText="rcmSGStrt"/>
                <asp:BoundField DataField="RCMSGSTAmt" HeaderText="RCMSGSTAmt"/>
                <asp:BoundField DataField="rcmIGStrt" HeaderText="rcmIGStrt"/>
                <asp:BoundField DataField="RCMIGSTAmt" HeaderText="RCMIGSTAmt"/>
                <asp:BoundField DataField="rcmcgstpar_code" HeaderText="rcmcgstpar_code"/>
                <asp:BoundField DataField="rcmsgstpar_code" HeaderText="rcmsgstpar_code"/>
                <asp:BoundField DataField="rcmIGStpar_code" HeaderText="rcmIGStpar_code"/>
                </Columns>
            <PagerTemplate>
                <asp:GridViewPager runat="server" />
            </PagerTemplate>
        </asp:GridView>
        </fieldset>
        <div>
            <asp:SqlDataSource ID="InvoiceSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="AC_GetInvoiceForExcel" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:ControlParameter ControlID="ddRIM" PropertyName="SelectedValue" Name="IsRIM" DefaultValue="1" ConvertEmptyStringToNull="true" />
                    <asp:ControlParameter Name="ReportDate" ControlID="txtReportDate" PropertyName="Text" Type="datetime" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
</asp:Content>
