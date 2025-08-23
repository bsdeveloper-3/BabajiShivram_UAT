<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BillTracking.aspx.cs"
    Inherits="Transport_BillTracking" %>

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
            <div class="m clear">
                <fieldset class="fieldset-AutoWidth">
                    <legend>Bill Approval Pending</legend>
                    <div class="m clear">
                        <asp:Panel ID="pnlFilter" runat="server">
                            <div class="fleft">
                                <uc1:DataFilter ID="DataFilter1" runat="server" />
                            </div>
                            <div class="fleft" style="margin-left: 5px; padding-top: 3px;">
                                <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
                                    <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                                </asp:LinkButton>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="clear">
                    </div>
                    <div>
                        <asp:GridView ID="gvTransportBillApproval" runat="server" AutoGenerateColumns="False" CssClass="table"
                            Width="1500px" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lid"
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
                                        <asp:LinkButton ID="lnkSelect" CommandName="select" runat="server" Text='<%# Bind("TRRefNo")%>' CommandArgument='<%#Eval("lid") + ";" + Eval("TransReqId") %>'
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
                                <asp:BoundField HeaderText="Status" DataField="ApprovalStatus" />
                                <asp:BoundField HeaderText="Remark" DataField="Remark" ItemStyle-Width="130px" />
                                <asp:BoundField HeaderText="Approval Date" DataField="ApprovedDate" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField HeaderText="Approved By" DataField="ApprovedBy" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </fieldset>
                <div>
                    <asp:SqlDataSource ID="DataSourceBillApprovalPending" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="TR_GetBillApprovalPending" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:SessionParameter Name="UserId" SessionField="UserId" />
                            <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

