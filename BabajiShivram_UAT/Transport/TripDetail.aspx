<%@ Page Title="Trip Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TripDetail.aspx.cs"
    Inherits="Transport_TripDetail" Culture="en-GB" EnableEventValidation="false" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />

    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <script type="text/javascript">
        function ActiveTabChanged12() {
            /* Clear Error Message on Tab change Event */
            document.getElementById('<%=lblError.ClientID%>').innerHTML = '';
            document.getElementById('<%=lblError.ClientID%>').className = '';
        }

        function divexpandcollapse(divname) {
            var div = document.getElementById(divname);
            var img = document.getElementById('img' + divname);

            if (div.style.display == "none") {
                div.style.display = "inline";
                img.src = "Images/minus.png";
                img.title = 'Collapse';
            }
            else {
                div.style.display = "none";
                img.src = "Images/plus.png";
                img.title = 'Expand';
            }
        }
    </script>

    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server"></asp:Label>
            </div>
            <ajaxtoolkit:tabcontainer runat="server" id="TabBilling" activetabindex="0" cssclass="Tab"
                width="100%" onclientactivetabchanged="ActiveTabChanged12" autopostback="true">
                <AjaxToolkit:TabPanel runat="server" ID="TabPanelNormalJob" TabIndex="0" HeaderText="Normal Job Detail">
                    <ContentTemplate>
                        <div style="overflow: auto;">
                            <div class="m clear">
                                <div align="center">
                                    <asp:Label ID="lblError_Job" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="m clear">
                                <asp:Panel ID="pnlFilter1" runat="server">
                                    <div class="fleft">
                                        <uc1:DataFilter ID="DataFilter1" runat="server" />
                                    </div>
                                </asp:Panel>
                                <asp:LinkButton ID="lnkJobExport" runat="server" OnClick="lnkJobExport_Click">
                                    <asp:Image ID="imgExcel_JobDetail" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                                </asp:LinkButton>
                            </div>
                            <div class="clear"></div>
                            <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                                Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="TransRateId"
                                DataSourceID="DataSourceVehicle" CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20"
                                OnRowCommand="gvJobDetail_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ref No" SortExpression="TRRefNo">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkSelect" CommandName="select" runat="server" Text='<%# Bind("TRRefNo")%>'
                                                ToolTip="Click To Update Bill Detail"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Ref No" DataField="TRRefNo" ReadOnly="true" Visible="false" />
                                    <asp:BoundField HeaderText="Job No" DataField="JobRefNo" ReadOnly="true" SortExpression="JobRefNo" />
                                    <asp:BoundField HeaderText="Transporter" DataField="Transporter" ReadOnly="true" SortExpression="Transporter" />
                                    <asp:BoundField HeaderText="Customer" DataField="CustName" ReadOnly="true" SortExpression="CustName" />
                                    <asp:BoundField HeaderText="Pkg" DataField="SumPackages" ReadOnly="true" Visible="false" />
                                    <asp:BoundField HeaderText="Vehicle No" DataField="VehicleNo" SortExpression="VehicleNo" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Type" DataField="VehicleType" ReadOnly="true" />
                                    <asp:BoundField HeaderText="From" DataField="DeliveryFrom" ReadOnly="true" SortExpression="DeliveryFrom" />
                                    <asp:BoundField HeaderText="To" DataField="DeliveryTo" ReadOnly="true" SortExpression="DeliveryTo" />
                                    <asp:BoundField HeaderText="Delivery Type" DataField="DeliveryType" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Reporting Date" DataField="ReportingDate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Unloading Date" DataField="UnloadingDate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Container Return Date" DataField="ContReturnDate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Billing Instruction" DataField="Instruction" ReadOnly="true" />
                               </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
                <AjaxToolkit:TabPanel ID="TabPanelConsolidateJob" runat="server" TabIndex="1" HeaderText="Consolidate Job Detail">
                    <ContentTemplate>
                        <div style="overflow: auto;">
                            <div class="m clear">
                                <div align="center">
                                    <asp:Label ID="lblError_Consolidate" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="m clear">
                                <asp:Panel ID="pnlFilter2" runat="server">
                                    <div class="fleft">
                                        <uc1:DataFilter ID="DataFilter2" runat="server" />
                                    </div>
                                </asp:Panel>
                                <asp:LinkButton ID="lnkConsolidateExport" runat="server" OnClick="lnkConsolidateExport_Click">
                                    <asp:Image ID="imgConsolidateExport" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                                </asp:LinkButton>
                            </div>
                            <div class="clear"></div>
                            <asp:GridView ID="gvConsolidateBill" runat="server" AutoGenerateColumns="False" CssClass="table"
                                Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lid"
                                DataSourceID="DataSourceConsolidateVehicle" CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20"
                                OnRowCommand="gvConsolidateBill_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ref No" SortExpression="TRRefNo">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkSelect" CommandName="select" runat="server" Text='<%# Bind("TransRefNo")%>'
                                                ToolTip="Click To Update Bill Detail" CommandArgument='<%#Eval("lid") + ";" + Eval("TransReqId")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Ref No" DataField="TransRefNo" ReadOnly="true" Visible="false" />
                                    <asp:BoundField HeaderText="Transporter" DataField="Transporter" ReadOnly="true" SortExpression="Transporter" />
                                    <asp:BoundField HeaderText="VehicleNo" DataField="VehicleNo" ReadOnly="true" SortExpression="VehicleNo" />
                                    <asp:BoundField HeaderText="JobRefNo" DataField="JobRefNo" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Job Created By" DataField="CreatedBy" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Job Created Date" DataField="CreatedDate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Billing Instruction" DataField="Instruction" ReadOnly="true" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
            </ajaxtoolkit:tabcontainer>
            <div>
                <asp:SqlDataSource ID="DataSourceVehicle" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TR_GetVehiclesForTripDetail" SelectCommandType="StoredProcedure"
			DataSourceMode="DataSet" EnableCaching="true" CacheDuration="300" CacheKeyDependency="MyCacheDependency">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceConsolidateVehicle" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TR_GetConsolVehiclesForTripDetail" SelectCommandType="StoredProcedure"
			DataSourceMode="DataSet" EnableCaching="true" CacheDuration="300" CacheKeyDependency="MyCacheDependency">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


