<%@ Page Title="Bill History" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="BillHistory.aspx.cs" Inherits="Transport_BillHistory" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <style type="text/css">
        input[type="text" i]:disabled {
            background-color: rgba(211, 211, 211, 0.72);
            color: black;
        }

        table textarea {
            background-color: rgba(211, 211, 211, 0.72);
            color: black;
        }

        table.table {
            white-space: normal;
        }
    </style>
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
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlBillApprvl" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upnlBillApprvl" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server"></asp:Label>
            </div>
            <cc1:tabcontainer runat="server" id="TabRequestRecd" activetabindex="0" cssclass="Tab"
                width="100%" onclientactivetabchanged="ActiveTabChanged12" autopostback="true">
                <cc1:TabPanel runat="server" ID="TabPanelNormalJob" TabIndex="0" HeaderText="Normal Job History">
                    <ContentTemplate>
                        <div style="overflow: auto;">
                            <div class="m clear">
                                <div align="center">
                                    <asp:Label ID="lblError_Job" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="m clear">
                                <asp:Panel ID="Panel1" runat="server">
                                    <div class="fleft">
                                        <uc1:DataFilter ID="DataFilter1" runat="server" />
                                    </div>
                                </asp:Panel>
                                <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click"><asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" /></asp:LinkButton>
                            </div>
                            <div class="clear"></div>
                            <asp:GridView ID="gvBillHistory" runat="server" AutoGenerateColumns="False" CssClass="table"
                            Width="1500px" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lid" PagerSettings-Position="TopAndBottom"
                            DataSourceID="DataSourceBillHistory" CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20"
                            OnRowCommand="gvBillHistory_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ref No" SortExpression="TRRefNo" ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkSelect" CommandName="select" runat="server" Text='<%# Bind("TRRefNo")%>' CommandArgument='<%#Eval("TransReqId") + ";" + Eval("TransporterID") + ";" + Eval("lid")%>'
                                            ToolTip="Click To Update Bill Detail"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Ref No" DataField="TRRefNo" Visible="false" />
                                <asp:BoundField HeaderText="Job No" DataField="JobRefNo" ItemStyle-Width="120px" />
                                <asp:BoundField HeaderText="Transporter" DataField="Transporter" />
                                <asp:BoundField HeaderText="Bill Number" DataField="BillNumber" />
                                <asp:BoundField HeaderText="Bill Submit Date" DataField="BillSubmitDate" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField HeaderText="Bill Date" DataField="BillDate" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField HeaderText="Bill Amount" DataField="BillAmount" />
                                <asp:BoundField HeaderText="Detention" DataField="DetentionAmount" />
                                <asp:BoundField HeaderText="Varai" DataField="VaraiAmount" />
                                <asp:BoundField HeaderText="Empty Cont Charges" DataField="EmptyContRcptCharges" />
                                <asp:BoundField HeaderText="Total" DataField="TotalAmount" />
                                <asp:BoundField HeaderText="Billing Person" DataField="BillPersonName" />
                                <asp:BoundField HeaderText="Created Date" DataField="BillCreatedOn" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField HeaderText="Created By" DataField="BillCreatedBy" />
                                <asp:BoundField HeaderText="Approval Status" DataField="ApprovalStatus" />
                                <asp:BoundField HeaderText="Payment Status" DataField="ReceivedStatus" />
                            </Columns>
                            <PagerTemplate>
                                <asp:GridViewPager runat="server" />
                            </PagerTemplate>
                        </asp:GridView>
                        </div>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="TabPanelConsolidateJob" runat="server" TabIndex="1" HeaderText="Consolidate Job History">
                    <ContentTemplate>
                        <div style="overflow: auto;">
                            <div class="m clear">
                                <div align="center">
                                    <asp:Label ID="lblError_ConsolidateJob" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="m clear">
                                <asp:Panel ID="pnlFilter2" runat="server">
                                    <div class="fleft">
                                        <uc1:DataFilter ID="DataFilter2" runat="server" />
                                    </div>
                                    <div class="fleft" style="margin-left: 30px; padding-top: 3px;">
                                        <asp:LinkButton ID="lnkExport_Consolidate" runat="server" OnClick="lnkExport_Consolidate_Click">
                                            <asp:Image ID="imgExportConsolidate" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                                        </asp:LinkButton>
                                    </div>
                                </asp:Panel>
                            </div>
                            <div class="m clear"></div>
                           <%-- <asp:GridView ID="gvConsolidateBillHistory" runat="server" AutoGenerateColumns="False" CssClass="table"
                                Width="1500px" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="TransBillId"
                                DataSourceID="DataSourceConsolidateBillHistory" CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20"
                                OnRowCommand="gvConsolidateBillHistory_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ref No" SortExpression="TRRefNo" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkSelect" CommandName="select" runat="server" Text='<%# Bind("TRRefNo")%>' CommandArgument='<%#Eval("TransReqId") + ";" + Eval("ConsolidateID") %>'
                                                ToolTip="Click To Update Bill Detail"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Ref No" DataField="TRRefNo" Visible="false" />
                                    <asp:BoundField HeaderText="Job No" DataField="JobRefNo" ItemStyle-Width="120px" />
                                    <asp:BoundField HeaderText="Transporter" DataField="Transporter" />
                                    <asp:BoundField HeaderText="Bill Number" DataField="BillNumber" />
                                    <asp:BoundField HeaderText="Bill Submit Date" DataField="BillSubmitDate" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField HeaderText="Bill Date" DataField="BillDate" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField HeaderText="Bill Amount" DataField="BillAmount" />
                                    <asp:BoundField HeaderText="Detention" DataField="DetentionAmount" />
                                    <asp:BoundField HeaderText="Varai" DataField="VaraiAmount" />
                                    <asp:BoundField HeaderText="Empty Cont Charges" DataField="EmptyContRcptCharges" />
                                    <asp:BoundField HeaderText="Total" DataField="TotalAmount" />
                                    <asp:BoundField HeaderText="Billing Person" DataField="BillPersonName" />
                                    <asp:BoundField HeaderText="Created Date" DataField="BillCreatedOn" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField HeaderText="Created By" DataField="BillCreatedBy" />
                                    <asp:BoundField HeaderText="Approval Status" DataField="ApprovalStatus" />
                                    <asp:BoundField HeaderText="Payment Status" DataField="ReceivedStatus" />
                                </Columns>
                            </asp:GridView>--%>
                            <%-- DataSourceID="DataSourceConsolidateBillHistory"--%>
                            <asp:GridView ID="gvConsolidateBillHistory" runat="server" AutoGenerateColumns="False" CssClass="table"
                                Width="1500px" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lid" PagerSettings-Position="TopAndBottom"
                                 CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20" DataSourceID="DataSourceConsolidateBillHistory"
                                OnRowCommand="gvConsolidateBillHistory_RowCommand">
                              <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                     </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Ref No" SortExpression="TRRefNo" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkSelect" CommandName="select" runat="server" Text='<%# Bind("TransRefNo")%>' CommandArgument='<%#Eval("TransReqId") + ";" + Eval("lid") %>'
                                                ToolTip="Click To Update Bill Detail"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Ref No" DataField="TransRefNo" ReadOnly="true" Visible="false"  />
                                    <asp:BoundField HeaderText="Transporter" DataField="Transporter" ReadOnly="true" SortExpression="Transporter" />
                                    <asp:BoundField HeaderText="VehicleNo" DataField="VehicleNo" ReadOnly="true" SortExpression="VehicleNo" />
                                    <asp:BoundField HeaderText="JobRefNo" DataField="JobRefNo" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Job Created By" DataField="CreatedBy" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Job Created Date" DataField="CreatedDate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                                </Columns>
                                <PagerTemplate>
                                    <asp:GridViewPager runat="server" />
                                </PagerTemplate>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </cc1:TabPanel>
            </cc1:tabcontainer>
            <div>
                <asp:SqlDataSource ID="DataSourceBillHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TR_GetBillDetail" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <%-- <asp:SqlDataSource ID="DataSourceConsolidateBillHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TR_GetConsolidateBillDetail" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>--%>
                <asp:SqlDataSource ID="DataSourceConsolidateBillHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TR_GetConsolidateBillTracking" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
