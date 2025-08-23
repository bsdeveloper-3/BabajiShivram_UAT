<%@ Page Title="SB Preparation" Language="C#" MasterPageFile="~/ExportCustomerMaster.master" AutoEventWireup="true"
    CodeFile="CustomerSBPreparation.aspx.cs" Inherits="CustomerExport_CustomerSBPreparation" EnableSessionState="True"
    Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist"
            runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../Images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional"
        RenderMode="Inline">
        <ContentTemplate>
            <div align="center" style="vertical-align: top">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div class="clear">
            </div>
            <div>
                <fieldset class="fieldset-AutoWidth">
                    <legend>Pending SB Preparation</legend>
                    <div id="dvFilter" runat="server">
                        <div class="m clear">
                            <asp:Panel ID="pnlFilter" runat="server">
                                <div class="fleft">
                                    <uc1:DataFilter ID="DataFilter1" runat="server" />
                                </div>
                                <div style="margin-left: 2px; padding-top: 3px;">
                                    <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                                        <asp:Image ID="Image2" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                                    </asp:LinkButton>
                                </div>
                            </asp:Panel>
                        </div>
                        <div class="clear">
                        </div>
                    </div>
                    <div>
                        <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                            PagerStyle-CssClass="pgr" OnPreRender="gvJobDetail_PreRender" OnRowDataBound="gvJobDetail_RowDataBound"
                            DataKeyNames="lid" DataSourceID="JobDetailSqlDataSource" AllowPaging="True" AllowSorting="True"
                            PagerSettings-Position="TopAndBottom" PageSize="20" Width="90%">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="JobRefNo" HeaderText="Job Ref No" SortExpression="JobRefNo"/>
                                <asp:BoundField DataField="CustRefNo" HeaderText="Cust Ref No" SortExpression="CustRefNo" />
                                <asp:BoundField DataField="Shipper" HeaderText="Shipper" SortExpression="Shipper" />
                                <asp:BoundField DataField="ConsigneeName" HeaderText="Consignee" SortExpression="ConsigneeName" />
                                <asp:BoundField DataField="TransMode" HeaderText="Mode" SortExpression="TransMode" />
                                <asp:BoundField DataField="PortOfLoading" HeaderText="Port of Loading" SortExpression="PortOfLOading" />
                                <asp:BoundField DataField="PortOfDischarge" HeaderText="Port of Discharge" SortExpression="PortOfDischarge" />
                                <asp:BoundField DataField="UserName" HeaderText="Job Created By" SortExpression="UserName" />
                                <asp:BoundField DataField="JobCreatedDate" HeaderText="Job Created Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="dtDate" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </fieldset>
            </div>
            <div>
                <asp:SqlDataSource ID="JobDetailSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CC_GetSBPreparationByCustId" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="CustUserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
