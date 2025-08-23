<%@ Page Title="Pending Delivery" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
        CodeFile="PendingTransDelivery.aspx.cs" Inherits="Transport_PendingTransDelivery" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <script type="text/javascript">
        function ActiveTabChanged12() {
            /* Clear Error Message on Tab change Event */
            document.getElementById('<%=lblMessage.ClientID%>').innerHTML = '';
            document.getElementById('<%=lblMessage.ClientID%>').className = '';
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
    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional"
        RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </div>
            <%--<div align="left">
                <asp:Button ID="btnConsolidate" runat="server" Text="Consolidated Delivery" OnClick="btnConsolidate_Click" />
            </div>--%>
            <div class="clear">
                <ajaxtoolkit:tabcontainer runat="server" id="TabRequestRecd" activetabindex="0" cssclass="Tab"
                    width="100%" onclientactivetabchanged="ActiveTabChanged12" autopostback="true">
                    <AjaxToolkit:TabPanel runat="server" ID="TabPanelNormalJob" TabIndex="0" HeaderText="Job Detail">
                        <ContentTemplate>
                            <div class="clear">
                                <asp:Panel ID="pnlFilter" runat="server">
                                    <div class="fleft">
                                        <uc1:DataFilter ID="DataFilter1" runat="server" />
                                    </div>
                                    <div class="fleft" style="margin-left: 5px; padding-top: 3px;">
                                        <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
                                                <asp:Image ID="imgExcel1" runat="server" ImageUrl="~/Images/Excel.jpg" ToolTip="Export To Excel" />
                                        </asp:LinkButton>
                                    </div>
                                </asp:Panel>
                            </div>
                            <br />
                            <div class="clear">
                            </div>
                            <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                                PagerStyle-CssClass="pgr" DataKeyNames="JobId,CustomerId" AllowPaging="True" AllowSorting="True" Width="100%"
                                PageSize="20" PagerSettings-Position="TopAndBottom" OnRowCommand="gvJobDetail_RowCommand"
                                OnPreRender="gvJobDetail_PreRender" OnRowDataBound="gvJobDetail_RowDataBound" DataSourceID="PendingDeliverySqlDataSource">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BS Job No" SortExpression="JobRefNo">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkJobRefNo" CommandName="select" runat="server" Text='<%#Eval("JobRefNo") %>'
                                                CommandArgument='<%#Eval("JobId") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" Visible="false" />
                                    <asp:BoundField DataField="ConsigneeName" HeaderText="Consignee" SortExpression="ConsigneeName" />
                                    <asp:BoundField DataField="Port" HeaderText="Port" SortExpression="Port" />
                                    <asp:BoundField DataField="CFSName" HeaderText="CFS" SortExpression="CFSName" />
                                    <asp:BoundField DataField="NoOfPackages" HeaderText="Pkgs" SortExpression="NoOfPackages"
                                        ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="GrossWT" HeaderText="Gross Weight" SortExpression="GrossWT"
                                        ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="Con20" HeaderText="Con20" SortExpression="Con20" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Con40" HeaderText="Con40" SortExpression="Con40" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="LCL" HeaderText="LCL" SortExpression="LCL" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="DeliveryTypeName" HeaderText="Delivery Type" SortExpression="DeliveryTypeName" />
                                    <asp:BoundField DataField="ExamineDate" HeaderText="Examine Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ExamineDate" />
                                    <asp:BoundField DataField="OutOfChargeDate" HeaderText="Out of Charge Date" SortExpression="OutOfChargeDate" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" />
                                </Columns>
                                <PagerTemplate>
                                    <asp:GridViewPager runat="server" />
                                </PagerTemplate>
                            </asp:GridView>
                        </ContentTemplate>
                    </AjaxToolkit:TabPanel>
                    <AjaxToolkit:TabPanel ID="TabPanelConsolidateJob" runat="server" TabIndex="1" HeaderText="Consolidate Job Detail">
                        <ContentTemplate>
                            <div class="clear">
                                <asp:Panel ID="pnlFilter2" runat="server">
                                    <div class="fleft">
                                        <uc1:DataFilter ID="DataFilter2" runat="server" />
                                    </div>
                                    <div class="fleft" style="margin-left: 5px; padding-top: 3px;">
                                        <asp:LinkButton ID="lnkExportConsolidate" runat="server" OnClick="lnkExportConsolidate_Click">
                                                <asp:Image ID="imgExcel2" runat="server" ImageUrl="~/Images/Excel.jpg" ToolTip="Export To Excel" />
                                        </asp:LinkButton>
                                    </div>
                                </asp:Panel>
                            </div>
                            <div class="clear">
                            </div>
                            <asp:GridView ID="gvConsolidateJob" runat="server" AutoGenerateColumns="False" CssClass="table"
                                PagerStyle-CssClass="pgr" DataKeyNames="lid" AllowPaging="True" AllowSorting="True" Width="100%"
                                PageSize="20" PagerSettings-Position="TopAndBottom" OnRowCommand="gvConsolidateJob_RowCommand"
                                DataSourceID="DataSourceConsolidateJobs">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="TR Ref No">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnTRRefNo" CommandName="select" runat="server" Text='<%#Eval("TRRefNo") %>'
                                                CommandArgument='<%#Eval("ConsolidateID") + ";" + Eval("lid")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="TRRefNo" HeaderText="Ref No" Visible="false" />
                                    <asp:BoundField DataField="CustName" HeaderText="Customer" SortExpression="CustName" />
                                    <asp:BoundField DataField="RequestType" HeaderText="Type" Visible="false" />
                                    <asp:BoundField DataField="JobRefNo" HeaderText="Job No" />
                                    <asp:BoundField DataField="VehiclesRequired" HeaderText="Vehicles Req" SortExpression="VehiclesRequired" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="LocationFrom" HeaderText="Location" SortExpression="LocationFrom" />
                                    <asp:BoundField DataField="Destination" HeaderText="Destination" SortExpression="Destination" />
                                    <asp:BoundField DataField="NoOfPkgs" HeaderText="Pkgs" SortExpression="NoOfPkgs"
                                        ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="GrossWeight" HeaderText="Weight (Kgs)" SortExpression="GrossWeight"
                                        ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="Count20" HeaderText="20" SortExpression="Count20" />
                                    <asp:BoundField DataField="Count40" HeaderText="40" SortExpression="Count40" />
                                    <asp:BoundField DataField="CountLCL" HeaderText="LCL" SortExpression="CountLCL" />
                                    <asp:BoundField DataField="DeliveryType" HeaderText="Type" SortExpression="DeliveryType" />
                                    <asp:BoundField DataField="RequestDate" HeaderText="Request Date" SortExpression="RequestDate" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="VehiclePlaceDate" HeaderText="Vehicle Place Required" SortExpression="VehiclePlaceDate" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="RequestedBy" HeaderText="Requested By" SortExpression="RequestedBy" />
                                </Columns>
                                <PagerTemplate>
                                    <asp:GridViewPager runat="server" />
                                </PagerTemplate>
                            </asp:GridView>
                        </ContentTemplate>
                    </AjaxToolkit:TabPanel>
                </ajaxtoolkit:tabcontainer>
                <div>
                    <asp:SqlDataSource ID="PendingDeliverySqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="GetPendingDeliveryForUser" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:SessionParameter Name="UserId" SessionField="UserId" />
                            <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="DataSourceConsolidateJobs" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="BS_GetConsolidateRequests" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:SessionParameter Name="UserId" SessionField="UserId" />
                            <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

