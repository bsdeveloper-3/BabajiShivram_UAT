<%@ Page Title="Volume Analysis" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="VolumeAnalysis.aspx.cs"
    Inherits="CRMReports_VolumeAnalysis" MaintainScrollPositionOnPostback="true" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="GVPager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <AjaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlVolumeAnalysis" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <script type="text/javascript">
        function ActiveTabChanged12() {
            /* Clear Error Message on Tab change Event */
            document.getElementById('<%=lblError.ClientID%>').innerHTML = '';
            document.getElementById('<%=lblError.ClientID%>').className = '';
        }
    </script>
    <asp:UpdatePanel ID="upnlVolumeAnalysis" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="clear">
        <asp:Panel ID="pnlFilter" runat="server">
            <div class="fright" style="margin-right: 5px;">
                <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
                    <asp:Image ID="imgExportExcel" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
                &nbsp;
                <asp:DropDownList ID="ddlMode" runat="server" Width="70px" TabIndex="1" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlMode_SelectedIndexChanged">
                    <asp:ListItem Value="0" Text="Both" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="1" Text="Air"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Sea"></asp:ListItem>
                </asp:DropDownList>
                &nbsp;
                <asp:DropDownList ID="ddlUser" runat="server" DataSourceID="DataSourceSalesPerson" DataTextField="sName"
                    DataValueField="lid" AppendDataBoundItems="true" TabIndex="2" Width="250px" AutoPostBack="true" OnSelectedIndexChanged="ddlUser_SelectedIndexChanged">
                    <asp:ListItem Text="- Select Sales Person -" Value="0" Selected="True"></asp:ListItem>
                </asp:DropDownList>
                &nbsp;
                 <asp:DropDownList ID="ddlMonth" runat="server" TabIndex="1" Width="150px" ToolTip="Filter monthly user data" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                     <asp:ListItem Text="Financial Year" Selected="True" Value="0"></asp:ListItem>
                     <asp:ListItem Text="January" Value="1"></asp:ListItem>
                     <asp:ListItem Text="February" Value="2"></asp:ListItem>
                     <asp:ListItem Text="March" Value="3"></asp:ListItem>
                     <asp:ListItem Text="April" Value="4"></asp:ListItem>
                     <asp:ListItem Text="May" Value="5"></asp:ListItem>
                     <asp:ListItem Text="June" Value="6"></asp:ListItem>
                     <asp:ListItem Text="July" Value="7"></asp:ListItem>
                     <asp:ListItem Text="August" Value="8"></asp:ListItem>
                     <asp:ListItem Text="September" Value="9"></asp:ListItem>
                     <asp:ListItem Text="October" Value="10"></asp:ListItem>
                     <asp:ListItem Text="November" Value="11"></asp:ListItem>
                     <asp:ListItem Text="December" Value="12"></asp:ListItem>
                 </asp:DropDownList>
            </div>
        </asp:Panel>
    </div>
    <AjaxToolkit:TabContainer runat="server" ID="Tabs" ActiveTabIndex="0" CssClass="Tab" Width="100%" OnClientActiveTabChanged="ActiveTabChanged12" AutoPostBack="false">
        <AjaxToolkit:TabPanel runat="server" ID="TabPanelImport" TabIndex="1" HeaderText="Import CHA">
            <ContentTemplate>
                <fieldset>
                    <asp:GridView ID="gvImport" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr" DataSourceID="DataSourceImportCHA"
                        DataKeyNames="CustomerId" CssClass="table" OnPreRender="gvImport_PreRender" EmptyDataText="No Data Found" EmptyDataRowStyle-Font-Bold="true" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Customer" HeaderText="Customer Name" SortExpression="Customer" ReadOnly="true" />
                            <asp:BoundField DataField="NoofJobsAir" HeaderText="No of Jobs" SortExpression="NoofJobsAir" ReadOnly="true" />
                            <asp:BoundField DataField="NOOFJOBSFCL" HeaderText="FCL Jobs" SortExpression="NOOFJOBSFCL" ReadOnly="true" />
                            <asp:BoundField DataField="NOOFJOBSLCL" HeaderText="LCL Jobs" SortExpression="NOOFJOBSLCL" ReadOnly="true" />
                            <asp:BoundField DataField="NOOFCONT20" HeaderText="Cont 20" SortExpression="NOOFCONT20" ReadOnly="true" />
                            <asp:BoundField DataField="NOOFCONT40" HeaderText="Cont 40" SortExpression="NOOFCONT40" ReadOnly="true" />
                            <asp:BoundField DataField="TEU" HeaderText="TEU" SortExpression="TEU" ReadOnly="true" />
                            <asp:BoundField DataField="GrossWeight" HeaderText="Gross WT" SortExpression="GrossWeight" ReadOnly="true" />
                            <asp:BoundField DataField="NoOfPKGS" HeaderText="Total Pkg" SortExpression="NoOfPKGS" ReadOnly="true" />
                            <asp:BoundField DataField="CreatedBy" HeaderText="Referred By" SortExpression="CreatedBy" ReadOnly="true" />
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="DataSourceImportCHA" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                        SelectCommand="CRM_rptVolumeAnalysis_ImportCHA" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlMonth" PropertyName="SelectedValue" Name="MonthId" DefaultValue="0" />
                            <asp:ControlParameter ControlID="ddlMode" PropertyName="SelectedValue" Name="ModeId" DefaultValue="0" />
                            <asp:ControlParameter ControlID="ddlUser" PropertyName="SelectedValue" Name="UserId" DefaultValue="0" />
                            <asp:ControlParameter ControlID="ddlUser" PropertyName="SelectedValue" Name="SalesPersonId" DefaultValue="0" />
                            <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </fieldset>
            </ContentTemplate>
        </AjaxToolkit:TabPanel>
        <AjaxToolkit:TabPanel runat="server" ID="TabPanelExport" TabIndex="2" HeaderText="Export CHA">
            <ContentTemplate>
                <fieldset>
                    <asp:GridView ID="gvExport" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr" DataSourceID="DataSourceExportCHA"
                        DataKeyNames="CustomerId" CssClass="table" OnPreRender="gvExport_PreRender" EmptyDataText="No Data Found" EmptyDataRowStyle-Font-Bold="true" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Customer" HeaderText="Customer Name" SortExpression="Customer" ReadOnly="true" />
                            <asp:BoundField DataField="NoofJobsAir" HeaderText="No of Jobs" SortExpression="NoofJobsAir" ReadOnly="true" />
                            <asp:BoundField DataField="NOOFJOBSFCL" HeaderText="FCL Jobs" SortExpression="NOOFJOBSFCL" ReadOnly="true" />
                            <asp:BoundField DataField="NOOFJOBSLCL" HeaderText="LCL Jobs" SortExpression="NOOFJOBSLCL" ReadOnly="true" />
                            <asp:BoundField DataField="NOOFCONT20" HeaderText="Cont 20" SortExpression="NOOFCONT20" ReadOnly="true" />
                            <asp:BoundField DataField="NOOFCONT40" HeaderText="Cont 40" SortExpression="NOOFCONT40" ReadOnly="true" />
                            <asp:BoundField DataField="TEU" HeaderText="TEU" SortExpression="TEU" ReadOnly="true" />
                            <asp:BoundField DataField="GrossWeight" HeaderText="Gross WT" SortExpression="GrossWeight" ReadOnly="true" />
                            <asp:BoundField DataField="NoOfPKGS" HeaderText="Total Pkg" SortExpression="NoOfPKGS" ReadOnly="true" />
                            <asp:BoundField DataField="CreatedBy" HeaderText="Referred By" SortExpression="CreatedBy" ReadOnly="true" />
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="DataSourceExportCHA" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                        SelectCommand="CRM_rptVolumeAnalysis_ExportCHA" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlMonth" PropertyName="SelectedValue" Name="MonthId" DefaultValue="0" />
                            <asp:ControlParameter ControlID="ddlMode" PropertyName="SelectedValue" Name="ModeId" DefaultValue="0" />
                            <asp:ControlParameter ControlID="ddlUser" PropertyName="SelectedValue" Name="UserId" DefaultValue="0" />
                            <asp:ControlParameter ControlID="ddlUser" PropertyName="SelectedValue" Name="SalesPersonId" DefaultValue="0" />
                            <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </fieldset>
            </ContentTemplate>
        </AjaxToolkit:TabPanel>
        <AjaxToolkit:TabPanel runat="server" ID="TabPanelFreight" TabIndex="3" HeaderText="Freight Forwarding">
            <ContentTemplate>
                <fieldset>
                    <asp:GridView ID="gvFreight" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr" DataSourceID="DataSourceFreight"
                        DataKeyNames="CustomerId" CssClass="table" OnPreRender="gvFreight_PreRender" EmptyDataText="No Data Found" EmptyDataRowStyle-Font-Bold="true" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Customer" HeaderText="Customer Name" SortExpression="Customer" ReadOnly="true" />
                            <asp:BoundField DataField="NoofJobsAir" HeaderText="No of Jobs" SortExpression="NoofJobsAir" ReadOnly="true" />
                            <asp:BoundField DataField="NOOFJOBSFCL" HeaderText="FCL Jobs" SortExpression="NOOFJOBSFCL" ReadOnly="true" />
                            <asp:BoundField DataField="NOOFJOBSLCL" HeaderText="LCL Jobs" SortExpression="NOOFJOBSLCL" ReadOnly="true" />
                            <asp:BoundField DataField="NOOFCONT20" HeaderText="Cont 20" SortExpression="NOOFCONT20" ReadOnly="true" />
                            <asp:BoundField DataField="NOOFCONT40" HeaderText="Cont 40" SortExpression="NOOFCONT40" ReadOnly="true" />
                            <asp:BoundField DataField="TEU" HeaderText="TEU" SortExpression="TEU" ReadOnly="true" />
                            <asp:BoundField DataField="GrossWeight" HeaderText="Gross WT" SortExpression="GrossWeight" ReadOnly="true" />
                            <asp:BoundField DataField="NoOfPKGS" HeaderText="Total Pkg" SortExpression="NoOfPKGS" ReadOnly="true" />
                            <asp:BoundField DataField="CreatedBy" HeaderText="Referred By" SortExpression="CreatedBy" ReadOnly="true" />
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="DataSourceFreight" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                        SelectCommand="CRM_rptVolumeAnalysis_Freight" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlMonth" PropertyName="SelectedValue" Name="MonthId" DefaultValue="0" />
                            <asp:ControlParameter ControlID="ddlMode" PropertyName="SelectedValue" Name="ModeId" DefaultValue="0" />
                            <asp:ControlParameter ControlID="ddlUser" PropertyName="SelectedValue" Name="UserId" DefaultValue="0" />
                            <asp:ControlParameter ControlID="ddlUser" PropertyName="SelectedValue" Name="SalesPersonId" DefaultValue="0" />
                            <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </fieldset>
            </ContentTemplate>
        </AjaxToolkit:TabPanel>
        <AjaxToolkit:TabPanel runat="server" ID="TabPanelTransportation" TabIndex="4" HeaderText="Transportation">
            <ContentTemplate>
                <fieldset>
                    <asp:GridView ID="gvTransportation" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr" DataSourceID="DataSourceTransportation"
                        CssClass="table" OnPreRender="gvTransportation_PreRender" EmptyDataText="No Data Found" EmptyDataRowStyle-Font-Bold="true" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Customer" HeaderText="Customer Name" SortExpression="Customer" ReadOnly="true" />
                            <asp:BoundField DataField="CustVehicles" HeaderText="Customer Vehicles" SortExpression="CustVehicles" ReadOnly="true" />
                            <asp:BoundField DataField="BabajiVehicles" HeaderText="Babaji Vehicles" SortExpression="BabajiVehicles" ReadOnly="true" />
                            <asp:BoundField DataField="NoofJobs" HeaderText="No of Jobs" SortExpression="NoofJobs" ReadOnly="true" />
                            <asp:BoundField DataField="CreatedBy" HeaderText="Referred By" SortExpression="CreatedBy" ReadOnly="true" />
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="DataSourceTransportation" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                        SelectCommand="CRM_rptVolumeAnalysis_Transport" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlMonth" PropertyName="SelectedValue" Name="MonthId" DefaultValue="0" />
                            <asp:ControlParameter ControlID="ddlMode" PropertyName="SelectedValue" Name="ModeId" DefaultValue="0" />
                            <asp:ControlParameter ControlID="ddlUser" PropertyName="SelectedValue" Name="SalesPersonId" DefaultValue="0" />
                            <asp:SessionParameter Name="UserId" SessionField="UserId" />
                            <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </fieldset>
            </ContentTemplate>
        </AjaxToolkit:TabPanel>
    </AjaxToolkit:TabContainer>
    <asp:SqlDataSource ID="DataSourceSalesPerson" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
        SelectCommand="CRM_GetSalesPerson" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:SessionParameter Name="UserId" SessionField="UserId" />
            <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

