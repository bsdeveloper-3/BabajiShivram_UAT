<%@ Page Title="Customer Transportation" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="WarehouseCustomer.aspx.cs" 
    Inherits="ExportCHA_WarehouseCustomer" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <style type="text/css">
        .modalBackground {
            background-color: black;
            filter: alpha(opacity=90);
            opacity: 0.5;
        }

        .modalPopup1 {
            border-radius: 5px;
            background-color: #FFFFFF;
            border-width: 1px;
            border-style: solid;
            border-color: black;
            padding-top: 5px;
            padding-left: 3px;
            width: 600px;
            height: 300px;
        }
    </style>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upJobDetail" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); opacity: .8;">
                    <img alt="progress" src="../Images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>

        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upJobDetail" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>

            <div align="center">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" />
            </div>

            <fieldset>
                <legend>Pending Customer Transport</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                        </div>
                        <div class="fleft" style="margin-left: 30px; padding-top: 3px;">
                            <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                                <asp:Image ID="Image2" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>

                <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                    PagerStyle-CssClass="pgr" DataKeyNames="JobId" AllowPaging="True" AllowSorting="True" PageSize="20"
                    PagerSettings-Position="TopAndBottom" OnRowCommand="gvJobDetail_RowCommand" Width="100%"
                    DataSourceID="PendingJobForTransoprt" OnPreRender="gvJobDetail_PreRender" >
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="BS Job No" SortExpression="JobRefNo">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkSelect" CommandName="Select" ToolTip="Customer Transport Detail" runat="server"
                                    Text='<%#Eval("JobRefNo") %>' CommandArgument='<%#Eval("JobId") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" Visible="false" />
                        <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer"/>
                        <asp:BoundField DataField="CustRefNo" HeaderText="Cust Ref No" SortExpression="CustRefNo" />
                        <asp:BoundField DataField="Shipper" HeaderText="Shipper" SortExpression="Shipper"/>
                        <asp:BoundField DataField="Consignee" HeaderText="Consignee" SortExpression="Consignee" />
                        <asp:BoundField DataField="TransMode" HeaderText="Mode" SortExpression="TransMode" />
                        <asp:BoundField DataField="PortOfDischarge" HeaderText="Port Of Discharge" SortExpression="PortOfDischarge" />
                        <asp:BoundField DataField="RequestDate" HeaderText="Request Date" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="RequestBy" HeaderText="Request By" SortExpression="RequestBy" />
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>

            </fieldset>
            <div>
                <asp:SqlDataSource ID="PendingJobForTransoprt" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="EX_GetPendingTransport" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
