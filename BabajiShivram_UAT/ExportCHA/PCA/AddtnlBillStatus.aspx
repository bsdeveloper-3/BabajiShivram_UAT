<%@ Page Title="Bill Status Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddtnlBillStatus.aspx.cs"
    Inherits="PCA_AddtnlBillStatus" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="GVPager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style type="text/css">
        .Tab .ajax__tab_header {
            white-space: nowrap !important;
        }
    </style>
    <AjaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upJobDetail" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upJobDetail" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:HiddenField ID="hdnPreAlertId" runat="server" />
                <asp:HiddenField ID="hdnCustId" runat="server" />
                <asp:HiddenField ID="hdnMode" runat="server" />
                <asp:HiddenField ID="hdnBoeTypeId" runat="server" />
                <asp:HiddenField ID="hdnDeliveryType" runat="server" />
                <asp:HiddenField ID="hdnLoadingPortId" runat="server" />
                <asp:HiddenField ID="hdnUploadPath" runat="server" />
            </div>
            <div class="clear"></div>

            <fieldset>
                <%--Start Billing--%>
                <asp:FormView ID="FVJobDetail" HeaderStyle-Font-Bold="true" runat="server" DataKeyNames="JobId" Width="100%" DataSourceID="SqlDataSourceJobDetail">
                    <ItemTemplate>
                        <div class="m clear">
                            <asp:Button ID="btnBackButton" runat="server" OnClick="btnBackButton_Click" Text="Back" />
                        </div>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>BS Job No.
                                </td>
                                <td>
                                    <%# Eval("JobRefNo") %>
                                </td>
                                <td>Cust Ref No.
                                </td>
                                <td>
                                    <span>
                                        <%# Eval("CustRefNo") %>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td>Customer
                                </td>
                                <td>
                                    <span>
                                        <%# Eval("Customer")%>
                                    </span>
                                </td>
                                <td>Consignee
                                </td>
                                <td>
                                    <span>
                                        <%# Eval("ConsigneeName")%>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td>Customer Division
                                </td>
                                <td>
                                    <asp:Label ID="lblDivision" runat="server" Text='<%#Eval("Division") %>'></asp:Label>
                                </td>
                                <td>Customer Plant
                                </td>
                                <td>
                                    <asp:Label ID="lblPlant" runat="server" Text='<%#Eval("Plant") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Mode
                                </td>
                                <td>
                                    <span>
                                        <%# Eval("Mode")%>
                                    </span>
                                </td>
                                <td>Port of Discharge
                                </td>
                                <td>
                                    <span>
                                        <%# Eval("Port")%>
                                    </span>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:FormView>
                <asp:SqlDataSource ID="SqlDataSourceJobDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AD_GetAdditionalJobById" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <div>
                    <fieldset>
                        <legend>Billing Scrutiny</legend>
                        <asp:GridView ID="gvbillingscrutiny" runat="server" AutoGenerateColumns="False"
                            CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceBillingScrutiny"
                            CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Status" DataField="Status" />
                                <asp:BoundField HeaderText="Sent By Billing Advice" DataField="SentBy" />
                                <asp:BoundField HeaderText="Billing Advice Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                <asp:BoundField HeaderText="Received By Billing Scrutiny" DataField="ReceivedBy" />
                                <asp:BoundField HeaderText="Billing Scrutiny Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                <asp:BoundField HeaderText="Scrutiny Completed Date" DataField="ScrutinyDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                <asp:BoundField HeaderText="Scrutiny Completed By" DataField="ScrutinyCompletedBy" />
                            </Columns>
                        </asp:GridView>
                    </fieldset>

                    <fieldset>
                        <legend>Draft Invoice</legend>
                        <asp:GridView ID="gvDraftInvoice" runat="server" AutoGenerateColumns="False"
                            CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceDraftinvoice"
                            CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Status" DataField="Status" />
                                <asp:BoundField HeaderText="Sent By Billing Scrutiny" DataField="SentBy" />
                                <asp:BoundField HeaderText="Billing Scrutiny Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                <asp:BoundField HeaderText="Received By Draft Invoice" DataField="ReceivedBy" />
                                <asp:BoundField HeaderText="Draft Invoice Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                <asp:BoundField HeaderText="Draft Invoice Completed Date" DataField="DraftInvoiceDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                <asp:BoundField HeaderText="Draft Invoice Completed By" DataField="FAUserName" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                            </Columns>
                        </asp:GridView>
                    </fieldset>

                    <fieldset>
                        <legend>Draft Check</legend>
                        <asp:GridView ID="gvDraftcheck" runat="server" AutoGenerateColumns="False"
                            CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceDraftCheck"
                            CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Status" DataField="Status" />
                                <asp:BoundField HeaderText="Sent By Draft Invoice" DataField="SentBy" />
                                <asp:BoundField HeaderText="Draft Invoice Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                <asp:BoundField HeaderText="Received By Draft Check" DataField="ReceivedBy" />
                                <asp:BoundField HeaderText="Draft Check Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                <asp:BoundField HeaderText="Draft Check Completed Date" DataField="DraftCheckDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                            </Columns>
                        </asp:GridView>
                    </fieldset>

                    <fieldset>
                        <legend>Final Invoice Typing</legend>
                        <asp:GridView ID="gvFinaltyping" runat="server" AutoGenerateColumns="False"
                            CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceFinalTyping"
                            CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Status" DataField="Status" />
                                <asp:BoundField HeaderText="Sent By Draft Check" DataField="SentBy" />
                                <asp:BoundField HeaderText="Draft Check Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                <asp:BoundField HeaderText="Received By Final Typing" DataField="ReceivedBy" />
                                <asp:BoundField HeaderText="Final Typing Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                <asp:BoundField HeaderText="Final Typing Completed Date" DataField="FinalTypingDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                <asp:BoundField HeaderText="Final Typing Completed by" DataField="FAUserName" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                <asp:BoundField HeaderText="Comment" DataField="Comment" />
                            </Columns>
                        </asp:GridView>
                    </fieldset>

                    <fieldset>
                        <legend>Final Invoice Check</legend>
                        <asp:GridView ID="gvfinalcheck" runat="server" AutoGenerateColumns="False"
                            CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceFinalCheck"
                            CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Status" DataField="Status" />
                                <asp:BoundField HeaderText="Sent By Final Typing" DataField="SentBy" />
                                <asp:BoundField HeaderText="Final Typing Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                <asp:BoundField HeaderText="Received By Final Check" DataField="ReceivedBy" />
                                <asp:BoundField HeaderText="Final Check Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                <asp:BoundField HeaderText="Final Check Completed Date" DataField="FinalCheckDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                            </Columns>
                        </asp:GridView>
                    </fieldset>

                    <fieldset>
                        <legend>Bill Dispatch</legend>
                        <asp:GridView ID="gvbilldispatch" runat="server" AutoGenerateColumns="False" CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceBillDispatch"
                            CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Status" DataField="Status" />
                                <asp:BoundField HeaderText="Sent By Final Check" DataField="SentBy" />
                                <asp:BoundField HeaderText="Final Check Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                <asp:BoundField HeaderText="Received By Bill Dispatch" DataField="ReceivedBy" />
                                <asp:BoundField HeaderText="Bill Dispatch Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                <asp:BoundField HeaderText="Bill Dispatch Completed Date" DataField="BillDispatchDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                            </Columns>
                        </asp:GridView>
                    </fieldset>

                    <fieldset>
                        <legend>Bill Rejection</legend>
                        <asp:GridView ID="gvBillrejection" runat="server" AutoGenerateColumns="False" CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceBillRejection"
                            CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Stage" DataField="Stage" />
                                <asp:BoundField HeaderText="Rejected by" DataField="RejectedBy" />
                                <asp:BoundField HeaderText="Bill Rejection Date" DataField="RejectedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                <asp:BoundField HeaderText="Reason" DataField="Reason" />
                                <asp:BoundField HeaderText="Remark" DataField="Remark" />
                                <asp:BoundField HeaderText="Followup Date" DataField="FollowupDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                <asp:BoundField HeaderText="Followup Remark" DataField="FollowupRemark" />
                            </Columns>
                        </asp:GridView>
                    </fieldset>

                    <div id="div1">
                        <asp:SqlDataSource ID="DataSourceBillingScrutiny" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="AD_GetBillingScrutinyById" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="JobId" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                        <asp:SqlDataSource ID="DataSourceDraftinvoice" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="AD_GetDraftInvoiceById" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="JobId" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                        <asp:SqlDataSource ID="DataSourceDraftCheck" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="AD_GetDraftCheckById" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="JobId" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                        <asp:SqlDataSource ID="DataSourceFinalTyping" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="AD_GetFinalTypingById" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="JobId" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                        <asp:SqlDataSource ID="DataSourceFinalCheck" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="AD_GetFinalCheckById" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="JobId" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                        <asp:SqlDataSource ID="DataSourceBillDispatch" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="AD_GetBillDispatchById" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="JobId" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                        <asp:SqlDataSource ID="DataSourceBillRejection" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="AD_GetBillRejectionById" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="JobId" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                    </div>
                </div>
                <%--end Billing--%>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

