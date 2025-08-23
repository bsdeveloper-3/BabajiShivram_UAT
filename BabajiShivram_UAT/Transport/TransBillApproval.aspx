<%@ Page Title="Bill Approval Pending" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="TransBillApproval.aspx.cs" Inherits="Transport_TransBillApproval" %>

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
            <cc1:TabContainer runat="server" ID="TabBilling" ActiveTabIndex="0" CssClass="Tab"
                Width="100%" OnClientActiveTabChanged="ActiveTabChanged12" AutoPostBack="true">
                <cc1:TabPanel runat="server" ID="TabPanelNormalJob" TabIndex="0" HeaderText="Normal Job Detail">
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
                            <asp:GridView ID="gvTransportBillApproval" runat="server" AutoGenerateColumns="False" CssClass="table"
                                Width="1650px" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lid"
                                DataSourceID="DataSourceBillApprovalPending" CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20"
                                OnRowCommand="gvTransportBillApproval_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ref No" SortExpression="TRRefNo" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkSelect" CommandName="select" runat="server" Text='<%# Bind("TRRefNo")%>' CommandArgument='<%#Eval("lid") + ";" + Eval("TransReqId") + ";" + Eval("ConsolidateID") + ";" + Eval("TransporterID")%>'
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
                                    <asp:BoundField HeaderText="Toll Charges" DataField="TollCharges" />
                                    <asp:BoundField HeaderText="Other Charges" DataField="OtherCharges" />
                                    <asp:BoundField HeaderText="Total" DataField="TotalAmount" />
                                    <asp:BoundField HeaderText="Billing Person" DataField="BillPersonName" />
                                    <asp:BoundField HeaderText="Created Date" DataField="BillCreatedOn" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField HeaderText="Created By" DataField="BillCreatedBy" />
                                    <asp:BoundField HeaderText="Status" DataField="ApprovalStatus" />
                                    <asp:BoundField HeaderText="Remark" DataField="Remark" ItemStyle-Width="130px" />
                                    <asp:BoundField HeaderText="Approval Date" DataField="ApprovedDate" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField HeaderText="Approved By" DataField="ApprovedBy" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="TabPanelConsolidateJob" runat="server" TabIndex="1" HeaderText="Consolidate Job Detail">
                    <ContentTemplate>
                        <div style="overflow: auto;">
                            <div class="m clear">
                                <div align="center">
                                    <asp:Label ID="lblError_Consolidate" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="m clear">
                                <asp:Panel ID="Panel1" runat="server">
                                    <div class="fleft">
                                        <uc1:DataFilter ID="DataFilter2" runat="server" />
                                    </div>
                                </asp:Panel>
                                <asp:LinkButton ID="lnkConsolidateExport" runat="server" OnClick="lnkConsolidateExport_Click">
                                    <asp:Image ID="imgConsolidateExport" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                                </asp:LinkButton>
                            </div>
                            <div class="clear"></div>
                            <asp:GridView ID="gvConsolidateBillApproval" runat="server" AutoGenerateColumns="False" CssClass="table"
                                Width="1650px" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lid"
                                DataSourceID="DataSourceConsolidateBillApprovalPending" CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20"
                                OnRowCommand="gvConsolidateBillApproval_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ref No" SortExpression="TRRefNo" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkSelect" CommandName="select" runat="server" Text='<%# Bind("TRRefNo")%>' CommandArgument='<%#Eval("lid") + ";" + Eval("TransReqId") + ";" + Eval("TransBillId") + ";" + Eval("TransporterID")%>'
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
                                    <asp:BoundField HeaderText="Toll Charges" DataField="TollCharges" />
                                    <asp:BoundField HeaderText="Other Charges" DataField="OtherCharges" />
                                    <asp:BoundField HeaderText="Total" DataField="TotalAmount" />
                                    <asp:BoundField HeaderText="Billing Person" DataField="BillPersonName" />
                                    <asp:BoundField HeaderText="Created Date" DataField="BillCreatedOn" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField HeaderText="Created By" DataField="BillCreatedBy" />
                                    <asp:BoundField HeaderText="Status" DataField="ApprovalStatus" />
                                    <asp:BoundField HeaderText="Remark" DataField="Remark" ItemStyle-Width="130px" />
                                    <asp:BoundField HeaderText="Approval Date" DataField="ApprovedDate" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField HeaderText="Approved By" DataField="ApprovedBy" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </cc1:TabPanel>
            </cc1:TabContainer>
            <div>
                <asp:SqlDataSource ID="DataSourceBillApprovalPending" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TR_GetBillApprovalPending" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceConsolidateBillApprovalPending" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TR_GetConsolidateApprovalPending" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
