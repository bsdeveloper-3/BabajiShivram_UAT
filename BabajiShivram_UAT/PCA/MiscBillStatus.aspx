<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MiscBillStatus.aspx.cs" Inherits="PCA_MiscBillStatus"
    Culture="en-GB" EnableEventValidation="false" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <fieldset>
                <legend>Job Detail</legend>
                <asp:FormView ID="FVJobDetail" HeaderStyle-Font-Bold="true" runat="server" DataKeyNames="JobId"
                    Width="100%" OnDataBound="FVJobDetail_DataBound">
                    <ItemTemplate>
                        <div class="m clear">
                            <asp:Button ID="btnBackButton" runat="server" OnClick="btnBackButton_Click" Text="Back" Visible="false" />
                        </div>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>BS Job No.
                                </td>
                                <td>
                                    <%# Eval("JobRefNo") %>
                                                &nbsp;
                                            <asp:Label ID="lblInbondJobNo" Text="Inbond Job" runat="server" Visible="false"></asp:Label>
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
                                <td>Mode
                                </td>
                                <td>
                                    <span>
                                        <%# Eval("Mode")%>
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
                                <td>Babaji Branch
                                </td>
                                <td>
                                    <span>
                                        <asp:Label ID="lblBabajiBranch" runat="server" Text='<%#Eval("BabajiBranch") %>'></asp:Label>
                                    </span>
                                </td>
                                <td>First Check Required?	
                                </td>
                                <td>
                                    <span>
                                        <%# (Boolean.Parse(Eval("FirstCheckRequired").ToString())) ? "Yes": "No"%>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td>Gross Weight
                                </td>
                                <td>
                                    <span>
                                        <%# Eval("GrossWT")%> <span>Kgs</span>
                                    </span>
                                </td>
                                <td>Packages
                                </td>
                                <td>
                                    <%# Eval("NoOfPackages")%>
                                            &nbsp;
                                            <%#Eval("PackageTypeName")%>
                                </td>
                            </tr>
                            <tr>
                                <td>Container
                                </td>
                                <td>
                                    <span>20" - <b><%# Eval("Count20")%> </b>&nbsp;&nbsp;&nbsp;
                                                40" - <b><%# Eval("Count40")%> </b>&nbsp;&nbsp;&nbsp;
                                                LCL - <b><%# Eval("CountLCL")%> </b>
                                    </span>
                                </td>
                                <td>Total Container
                                </td>
                                <td>
                                    <b><%# Eval("CountTotal")%> </b>
                                </td>
                            </tr>
                            <%--<tr>
                                        <td>Job Type
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("JobType")%>
                                            </span>
                                        </td>
                                        <td>RMS/NonRMS
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("RMSNonRMS")%>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>BoE No
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("BOENo")%>
                                            </span>
                                        </td>
                                        <td>BoE Date
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("BOEDate","{0:dd/MM/yyyy}")%>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>MBL No
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("MAWBNo")%>
                                            </span>
                                        </td>
                                        <td>MBL Date
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("MAWBDate","{0:dd/MM/yyyy}")%>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Delivery Type
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("DeliveryType")%>
                                            </span>
                                        </td>
                                        
                                    </tr>--%>
                            <tr>
                                <td>Transport By
                                </td>
                                <td>
                                    <%# (Convert.ToBoolean(Eval("TransportationByBabaji"))) ? "Babaji": "Customer"%>
                                </td>
                                <td>KAM
                                </td>
                                <td>
                                    <span>
                                        <%# Eval("KAMUser")%>
                                    </span>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:FormView>
            </fieldset>

            <fieldset>
                <legend>Billing Instruction</legend>
                <%--<div id="dvResult" runat="server" style="max-height: 550px; overflow: auto; text-align: center;">--%>
                <br />
                <%--<div align="center">--%>
                <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" align="center" style="text-align: left;">
                    <tr>
                        <td><b>Allied Service</b></td>
                        <td>
                            <div style="word-wrap: break-word; width: 200px; white-space: normal;">
                                <asp:Label ID="lblAlliedAgencyService" runat="server" Text='<%# Bind("AlliedAgencyService") %>'></asp:Label>
                            </div>
                        </td>
                        <td><b>Allied Remark</b></td>
                        <td>
                            <div style="word-wrap: break-word; width: 200px; white-space: normal;">
                                <asp:Label ID="lblAlliedAgencyRemark" runat="server" Text='<%# Bind("AlliedAgencyRemark") %>'></asp:Label>
                            </div>
                        </td>
                        <td>
                            <b>Read By & Date</b>
                        </td>
                        <td>
                            <asp:Label ID="lblAlliedReadBy" runat="server"></asp:Label>
                        </td>
                        <td><b>Charge By & Date</b></td>
                        <td>
                            <asp:Label ID="lblAlliedChargeBy" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Other Service</b></td>
                        <td>
                            <div style="word-wrap: break-word; width: 200px; white-space: normal;">
                                <asp:Label ID="lblOtherService" runat="server"></asp:Label>
                            </div>
                        </td>
                        <td><b>Other Service remark</b></td>
                        <td>
                            <div style="word-wrap: break-word; width: 200px; white-space: normal;">
                                <asp:Label ID="lblOtherServiceRemark" runat="server"></asp:Label>
                            </div>
                        </td>
                        <td>
                            <b>Read By & Date</b>
                        </td>
                        <td>
                            <asp:Label ID="lblOtherServiceReadBy" runat="server"></asp:Label>
                        </td>
                        <td><b>Charge By & Date</b></td>
                        <td>
                            <asp:Label ID="lblOtherServiceChargeBy" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Other Instruction</b></td>
                        <td>
                            <div style="word-wrap: break-word; width: 200px; white-space: normal;">
                                <asp:Label ID="lblInstruction" runat="server"></asp:Label>
                            </div>
                        </td>
                        <td><b>Instruction Copy</b></td>
                        <td>
                            <div style="word-wrap: break-word; width: 200px; white-space: normal;">
                                <asp:LinkButton ID="lnkInstructionCopy" runat="server" OnClick="lnkInstructionCopy_Click"></asp:LinkButton>
                                <asp:HiddenField ID="hdnInstructionCopy" runat="server" />
                            </div>
                        </td>
                        <td>
                            <b>Read By & Date</b>
                        </td>
                        <td>
                            <asp:Label ID="lblReadBy1" runat="server"></asp:Label>
                        </td>
                        <td><b>Charge By & Date</b></td>
                        <td>
                            <asp:Label ID="lblChargeBy1" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Other Instruction</b></td>
                        <td>
                            <div style="word-wrap: break-word; width: 200px; white-space: normal;">
                                <asp:Label ID="lblInstruction1" runat="server"></asp:Label>
                        </td>

                        <td><b>Instruction Copy</b></td>
                        <td>
                            <div style="word-wrap: break-word; width: 200px; white-space: normal;">
                                <asp:LinkButton ID="lnkInstructionCopy1" runat="server" OnClick="lnkInstructionCopy1_Click"></asp:LinkButton>
                                <asp:HiddenField ID="hdnInstructionCopy1" runat="server" />
                            </div>
                        </td>
                        <td>
                            <b>Read By & Date</b>
                        </td>
                        <td>
                            <asp:Label ID="lblReadBy2" runat="server"></asp:Label>
                        </td>
                        <td><b>Charge By & Date</b></td>
                        <td>
                            <asp:Label ID="lblChargeBy2" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Other Instruction</b></td>
                        <td>
                            <div style="word-wrap: break-word; width: 200px; white-space: normal;">
                                <asp:Label ID="lblInstruction2" runat="server"></asp:Label>
                            </div>
                        </td>
                        <td><b>Instruction Copy</b></td>
                        <td>
                            <div style="word-wrap: break-word; width: 200px; white-space: normal;">
                                <asp:LinkButton ID="lnkInstructionCopy2" runat="server" OnClick="lnkInstructionCopy2_Click"></asp:LinkButton>
                                <asp:HiddenField ID="hdnInstructionCopy2" runat="server" />
                            </div>
                        </td>
                        <td>
                            <b>Read By & Date</b>
                        </td>
                        <td>
                            <asp:Label ID="lblReadBy3" runat="server"></asp:Label>
                        </td>
                        <td><b>Charge By & Date</b></td>
                        <td>
                            <asp:Label ID="lblChargeBy3" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Other Instruction</b></td>
                        <td>
                            <div style="word-wrap: break-word; width: 200px; white-space: normal;">
                                <asp:Label ID="lblInstruction3" runat="server"></asp:Label>
                            </div>
                        </td>
                        <td><b>Instruction Copy</b></td>
                        <td>
                            <div style="word-wrap: break-word; width: 200px; white-space: normal;">
                                <asp:LinkButton ID="lnkInstructionCopy3" runat="server" OnClick="lnkInstructionCopy3_Click"></asp:LinkButton>
                                <asp:HiddenField ID="hdnInstructionCopy3" runat="server" />
                            </div>
                        </td>
                        <td>
                            <b>Read By & Date</b>
                        </td>
                        <td>
                            <asp:Label ID="lblReadBy4" runat="server"></asp:Label>
                        </td>
                        <td><b>Charge By & Date</b></td>
                        <td>
                            <asp:Label ID="lblChargeBy4" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>User Name</b></td>
                        <td>
                            <asp:Label ID="lblUserId" runat="server"></asp:Label>
                        </td>
                        <td><b>User Date</b></td>
                        <td>
                            <asp:Label ID="lblUserDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <%--</div>--%>
                <%--</div>--%>
            </fieldset>

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
                    SelectCommand="GetBillingScrutinyById" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="DataSourceDraftinvoice" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetDraftInvoiceById" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="DataSourceDraftCheck" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetDraftCheckById" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="DataSourceFinalTyping" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetFinalTypingById" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="DataSourceFinalCheck" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetFinalCheckById" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="DataSourceBillDispatch" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetBillDispatchById" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="DataSourceBillRejection" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetBillRejectionById" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                    </SelectParameters>
                </asp:SqlDataSource>

            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

