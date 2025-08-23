<%@ Page Title="Bill Received" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
        CodeFile="TransBillReceived.aspx.cs" Inherits="BSCCPLTransport_TransBillReceived" Culture="en-GB"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlTPBill" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upnlTPBill" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="vsReqStatus" runat="server" ShowMessageBox="True"
                ShowSummary="False" ValidationGroup="ReqStatus" CssClass="errorMsg" />
            <div align="center">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </div>
            <fieldset>
            <legend>Bill Received - BSCCPL</legend>
                <div class="m clear">
                <asp:Panel ID="pnlFilter" runat="server">
                    <div class="fleft">
                        <uc1:DataFilter ID="DataFilter1" runat="server" />
                    </div>
                </asp:Panel>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
                        <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" ImageAlign="AbsBottom" /></asp:LinkButton>
                </div>
                <asp:GridView ID="gvReceiveBill" runat="server" AutoGenerateColumns="False" CssClass="table"
                    Width="100%" DataKeyNames="TransBillId" DataSourceID="DataSourceBill" 
                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40"
                    OnRowCommand="gvReceiveBill_RowCommand">
                    <AlternatingRowStyle CssClass="alt" />
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ref No" ItemStyle-Width="80px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkSelect" CommandName="select" runat="server" Text='<%# Bind("TRRefNo")%>' CommandArgument='<%#Eval("TransReqId") + ";" + Eval("ConsolidateID") + ";" + Eval("TransBillId") + ";" + Eval("TransporterID") %>'
                                    ToolTip="Show Transport Bill Detail"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Job No" DataField="JobRefNo" />
                        <asp:BoundField HeaderText="Transporter" DataField="Transporter" SortExpression="Transporter" />
                        <asp:BoundField HeaderText="Bill Number" DataField="BillNumber" SortExpression="BillNumber"/>
                        <asp:BoundField HeaderText="Bill Submit Date" DataField="BillSubmitDate" DataFormatString="{0:dd/MM/yyyy}" SortExpression="BillSubmitDate"/>
                        <asp:BoundField HeaderText="Bill Date" DataField="BillDate" DataFormatString="{0:dd/MM/yyyy}" SortExpression="BillDate"/>
                        <asp:BoundField HeaderText="Bill Amount" DataField="BillAmount" SortExpression="BillAmount"/>
                        <asp:BoundField HeaderText="Detention" DataField="DetentionAmount" SortExpression="DetentionAmount"/>
                        <asp:BoundField HeaderText="Varai" DataField="VaraiAmount" SortExpression="VaraiAmount"/>
                        <asp:BoundField HeaderText="Empty Cont Charges" DataField="EmptyContRcptCharges" SortExpression="EmptyContRcptCharges"/>
                        <asp:BoundField HeaderText="Toll Charges" DataField="TollCharges" SortExpression="TollCharges"/>
                        <asp:BoundField HeaderText="Other Charges" DataField="OtherCharges" SortExpression="OtherCharges"/>
                        <asp:BoundField HeaderText="Total" DataField="TotalAmount" SortExpression="TotalAmount"/>
                        <asp:BoundField HeaderText="Sent User" DataField="SentUser" SortExpression="SentUser"/>
                        <asp:BoundField HeaderText="Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy}" SortExpression="SentDate"/>
                    </Columns>
                    <PagerStyle CssClass="pgr" />
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="DataSourceBill" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TRS_GetBillReceived" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        <asp:Parameter Name="CompanyID" DefaultValue="12" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
