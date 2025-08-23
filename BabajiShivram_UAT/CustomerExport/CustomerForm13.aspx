<%@ Page Title="Pending Form 13" Language="C#" MasterPageFile="~/ExportCustomerMaster.master" AutoEventWireup="true" CodeFile="CustomerForm13.aspx.cs"
    Inherits="CustomerExport_CustomerForm13" EnableEventValidation="false" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingForm13" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPendingForm13" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="vsAddFilingDetail" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" />
            </div>
            <div class="clear">
            </div>
            <fieldset class="fieldset-AutoWidth">
                <legend>Pending Form 13</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                        </div>
                        <div class="fleft" style="margin-left: 2px; padding-top: 3px;">
                            <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                                <asp:Image ID="Image1" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
                <div class="clear">
                </div>
                <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr"
                    DataKeyNames="JobId" AllowPaging="True" AllowSorting="True" CssClass="table" PageSize="20"
                    PagerSettings-Position="TopAndBottom" DataSourceID="PendingNotingSqlDataSource" OnRowDataBound="gvJobDetail_RowDataBound"
                    OnPreRender="gvJobDetail_PreRender">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No"/>
                        <asp:BoundField DataField="CustRefNo" HeaderText="Cust Ref No" SortExpression="CustRefNo" ReadOnly="true" />
                        <asp:BoundField DataField="Shipper" HeaderText="Shipper" SortExpression="Shipper" ReadOnly="true" />
                        <asp:BoundField DataField="Consignee" HeaderText="Consignee" SortExpression="Consignee" ReadOnly="true" />
                        <asp:BoundField DataField="PortOfDischarge" HeaderText="Port Of Discharge" SortExpression="PortOfDischarge" ReadOnly="true" />
                        <asp:BoundField DataField="SBNo" HeaderText="SB No" SortExpression="SBNo" ReadOnly="true" />
                        <asp:BoundField DataField="SBDate" HeaderText="SB Date" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                        <asp:BoundField DataField="LEODate" HeaderText="Superintendent LEO Date" SortExpression="LEODate" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="Form13Date" HeaderText="Form 13 Date" SortExpression="Form13Date" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="TransHandOverDate" HeaderText="Transporter Hand Over Date" SortExpression="TransHandOverDate" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="ContainerGetInDate" HeaderText="Container Get In Date" SortExpression="ContainerGetInDate" DataFormatString="{0:dd/MM/yyyy}" />
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="PendingNotingSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CC_GetPendingForm13ByCustId" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="CustUserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

