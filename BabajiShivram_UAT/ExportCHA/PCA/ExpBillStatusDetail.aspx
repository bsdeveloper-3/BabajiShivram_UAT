<%@ Page Title="Billing Job Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ExpBillStatusDetail.aspx.cs" Inherits="PCA_ExpBillStatusDetail" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="GVPager" %>
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

    <script type="text/javascript">

        function OnPortOfLoadingSelected(source, eventArgs) {
            // alert(eventArgs.get_value());
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=hdnLoadingPortId.ClientID%>').value = results.PortOfLoadingId;
        }
        $addHandler
        (
            $get('ctl00_ContentPlaceHolder1_Tabs_TabPanelJobDetail_FVJobDetail_txtPortOfLoading'), 'keyup',
            function () {
                $get('<%=hdnLoadingPortId.ClientID %>').value = '0';
                alert($get('<%=hdnLoadingPortId.ClientID %>').value)
            }
        );
    </script>

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

            <AjaxToolkit:TabContainer runat="server" ID="Tabs" ActiveTabIndex="0" CssClass="Tab"
                Width="100%" OnClientActiveTabChanged="ActiveTabChanged12" AutoPostBack="false">
                <%--Start Billing--%>
                <AjaxToolkit:TabPanel runat="server" ID="TabBiiling" HeaderText="Billing Details">
                    <ContentTemplate>
                        <asp:FormView ID="FVJobDetail" HeaderStyle-Font-Bold="true" runat="server" DataKeyNames="JobId"
                            Width="100%" OnDataBound="FVJobDetail_DataBound">
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
                                        <td>Consignee
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("Consignee")%>
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
                                    <tr>
                                        <td>Babaji Branch
                                        </td>
                                        <td>
                                            <span>
                                                <asp:Label ID="lblBabajiBranch" runat="server" Text='<%#Eval("BabajiBranch") %>'></asp:Label>
                                            </span>
                                        </td>
                                        <td>Priority
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("Priority")%>
                                            </span>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>

                        </asp:FormView>
                        <fieldset id="fldPaymentHistory" runat="server">
                        <legend>Payment Detail</legend>
                            <asp:GridView ID="gvPaymentHistory" runat="server" AutoGenerateColumns="False"
                            CssClass="table" Width="99%" PagerStyle-CssClass="pgr"
                            DataKeyNames="lId" DataSourceID="DataSourcePaymentHistory" CellPadding="4"
                            AllowPaging="True" AllowSorting="True" PageSize="40">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Expense Type" DataField="ExpenseType" />
                                <asp:BoundField HeaderText="Payment Type" DataField="PaymentType" />
                                <asp:BoundField HeaderText="Amount" DataField="PaidAmount" />
                                <asp:BoundField HeaderText="Instrument No" DataField="InstrumentNo" />
                                <asp:BoundField HeaderText="Instrument Date" DataField="InstrumentDate" DataFormatString="{0:dd/MM/yyyy}"/>
                                <asp:BoundField HeaderText="User" DataField="UserName" />
                                <asp:BoundField HeaderText="Date" DataField="updDate" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" />
                                </Columns>
                        </asp:GridView>
                        </fieldset>
                        <div>
                            <fieldset>
                            <legend>All Document</legend>
                            <asp:GridView ID="gvPCDDocument" runat="server" AutoGenerateColumns="False" CssClass="table"
                                Width="100%" PagerStyle-CssClass="pgr" DataKeyNames="lid" CellPadding="4" AllowPaging="True"
                                AllowSorting="True" PageSize="20" DataSourceID="PCDDocumentSqlDataSource" OnRowDataBound="gvPCDDocument_RowDataBound"
                                OnRowCommand="gvPCDDocument_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DocumentName" HeaderText="All Document" />
                                    <%--<asp:BoundField DataField="PCDToCustomer" HeaderText="PCD To Customer" />
                                    <asp:BoundField DataField="PCDToScrutiny" HeaderText="Scrutiny" Visible="false" />--%>
                                    <asp:BoundField DataField="UserName" HeaderText="Uploaded By" />
                                    <asp:TemplateField HeaderText="Download">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                                CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </fieldset>

                            <fieldset>
                                <legend>Billing Advice Document</legend>
                                <asp:GridView ID="GridViewBillingAdvice" runat="server" AutoGenerateColumns="False"
                                    CssClass="table" Width="100%" PagerStyle-CssClass="pgr" DataKeyNames="lid" CellPadding="4"
                                    AllowPaging="True" AllowSorting="True" PageSize="20" DataSourceID="BillingAdviceSqlDataSource">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DocumentName" HeaderText="Billing Advice" SortExpression="DocumentName" />
                                        <asp:BoundField DataField="IsCopy" HeaderText="Copy" SortExpression="IsCopy" />
                                        <asp:BoundField DataField="IsOriginal" HeaderText="Original" SortExpression="IsOriginal" />
                                        <asp:BoundField DataField="CreatedBy" HeaderText="User Name" SortExpression="CreatedBy" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                            <fieldset>
                                <legend>PCA Document</legend>
                                <asp:GridView ID="GridViewPCADoc" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" PagerStyle-CssClass="pgr" DataKeyNames="lid" CellPadding="4" AllowPaging="True"
                                    AllowSorting="True" PageSize="20" DataSourceID="PCADocSqlDataSource">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DocumentName" HeaderText="PCA" SortExpression="DocumentName" />
                                        <asp:BoundField DataField="IsCopy" HeaderText="Copy" SortExpression="IsCopy" />
                                        <asp:BoundField DataField="IsOriginal" HeaderText="Original" SortExpression="IsOriginal" />
                                        <asp:BoundField DataField="CreatedBy" HeaderText="User Name" SortExpression="CreatedBy" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                            <fieldset id="BillingInstruction" runat="server">
                                <legend>Billing Instruction</legend>
                                <div id="dvResult" runat="server" style="max-height: 550px; overflow: auto; text-align: center;">
                                    <br />
                                    <div align="center">
                                        <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" align="center" style="text-align: left;">
                                            <tr>
                                                <td><b>Job Ref No</b></td>
                                                <td colspan="3">
                                                    <asp:Label ID="lblRefNo" runat="server"></asp:Label></td>
                                            </tr>
                                            <%--<tr>
                                <td>Allied Agency Apply</td>
                                <td colspan="3"><asp:Label ID="lblAgencyApply" runat="server" Text='<%# Bind("AlliedAgencyApply") %>'></asp:Label> </td>
                            </tr>--%>
                                            <tr>
                                                <td><b>Allied Service</b></td>
                                                <td>
                                                    <asp:Label ID="lblAlliedAgencyService" runat="server" Text='<%# Bind("AlliedAgencyService") %>'></asp:Label>
                                                </td>
                                                <td><b>Allied Remark</b></td>
                                                <td>
                                                    <asp:Label ID="lblAlliedAgencyRemark" runat="server" Text='<%# Bind("AlliedAgencyRemark") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>Other Service</b></td>
                                                <td>
                                                    <asp:Label ID="lblOtherService" runat="server"></asp:Label>
                                                </td>
                                                <td><b>Other Service remark</b></td>
                                                <td>
                                                    <asp:Label ID="lblOtherServiceRemark" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>Other Instruction</b></td>
                                                <td>
                                                    <asp:Label ID="lblInstruction" runat="server"></asp:Label></td>
                                                <td><b>Instruction Copy</b></td>
                                                <td>
                                                    <asp:LinkButton ID="lnkInstructionCopy" runat="server" OnClick="lnkInstructionCopy_Click"></asp:LinkButton>
                                                    <asp:HiddenField ID="hdnInstructionCopy" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>Other Instruction</b></td>
                                                <td>
                                                    <asp:Label ID="lblInstruction1" runat="server"></asp:Label></td>
                                                <td><b>Instruction Copy</b></td>
                                                <td>
                                                    <asp:LinkButton ID="lnkInstructionCopy1" runat="server" OnClick="lnkInstructionCopy1_Click"></asp:LinkButton>
                                                    <asp:HiddenField ID="hdnInstructionCopy1" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>Other Instruction</b></td>
                                                <td>
                                                    <asp:Label ID="lblInstruction2" runat="server"></asp:Label></td>
                                                <td><b>Instruction Copy</b></td>
                                                <td>
                                                    <asp:LinkButton ID="lnkInstructionCopy2" runat="server" OnClick="lnkInstructionCopy2_Click"></asp:LinkButton>
                                                    <asp:HiddenField ID="hdnInstructionCopy2" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>Other Instruction</b></td>
                                                <td>
                                                    <asp:Label ID="lblInstruction3" runat="server"></asp:Label></td>
                                                <td><b>Instruction Copy</b></td>
                                                <td>
                                                    <asp:LinkButton ID="lnkInstructionCopy3" runat="server" OnClick="lnkInstructionCopy3_Click"></asp:LinkButton>
                                                    <asp:HiddenField ID="hdnInstructionCopy3" runat="server" />
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
                                    </div>
                                </div>
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

                                <asp:SqlDataSource ID="BillingAdviceSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetPCDDocumentByWorkFlow" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                        <asp:Parameter Name="DocumentForType" DefaultValue="3" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <asp:SqlDataSource ID="DataSourcePaymentHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="AC_GetInvoicePaymentByJobId" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <asp:SqlDataSource ID="PCADocSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetPCDDocumentByWorkFlow" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                        <asp:Parameter Name="DocumentForType" DefaultValue="2" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <asp:SqlDataSource ID="PCDDocumentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetUploadedPCDDocument" SelectCommandType="StoredProcedure" OnSelected="PCDDocumentSqlDataSource_Selected">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                            </div>
                        </div>

                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
                <AjaxToolkit:TabPanel ID="TabDocument" runat="server" HeaderText="Document">
                    <ContentTemplate>
                        
                        <fieldset>
                            <legend>Download Document</legend>
                            <asp:GridView ID="GridViewDocument" runat="server" AutoGenerateColumns="False" CssClass="table"
                                Width="100%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DocumentSqlDataSource"
                                OnRowCommand="GridViewDocument_RowCommand" CellPadding="4" AllowPaging="True"
                                AllowSorting="True" PageSize="20">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="lid" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbllid" runat="server" Text='<%#Eval("lid") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Document Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDocType" runat="server" Text='<%#Eval("DocTypeName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="FileName" HeaderText="Document" />
                                    <%--<asp:BoundField DataField="DocName" HeaderText="Dcument Type" />--%>                                   
                                    <asp:BoundField DataField="UploadedDate" HeaderText="Date" />
                                    <asp:BoundField DataField="sName" HeaderText="Uploaded By" />
                                    <asp:TemplateField HeaderText="Download">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" Text="Download" CommandName="Download"
                                                CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="View">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkView" runat="server" Text="View" CommandName="View" 
                                                CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <br />
                            <br />
                        </fieldset>
                        <fieldset>
                    <legend>Invoice Document</legend>
                    <asp:GridView ID="gvInvoiceDocument" runat="server" CssClass="table" AutoGenerateColumns="false"
                        PagerStyle-CssClass="pgr" DataKeyNames="InvoiceID" AllowPaging="True" AllowSorting="True" PageSize="20"
                        DataSourceID="DataSourcePaymentDoument" OnRowCommand="gvInvoiceDocument_RowCommand" PagerSettings-Position="TopAndBottom" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1%>
                                </ItemTemplate>
                            </asp:TemplateField>                                 
                            <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" SortExpression="InvoiceNo"/>
                            <asp:BoundField DataField="ExpenseType" HeaderText="Type" SortExpression="ExpenseType"/>
                            <asp:BoundField DataField="VendorName" HeaderText="Vendor" SortExpression="VendorName"/>
                            <asp:BoundField DataField="DocumentName" HeaderText="Document" SortExpression="DocumentName"/>
                            <asp:BoundField DataField="InvoiceTypeName" HeaderText="Tax/Proforma" SortExpression="InvoiceTypeName"/>
                            <asp:TemplateField HeaderText="Download">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton21" runat="server" Text="Download" CommandName="Download"
                                        CommandArgument='<%#Eval("lid") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="View">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton3" runat="server" Text="View" CommandName="View" 
                                        CommandArgument='<%#Eval("lid") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView> 
                </fieldset>
                        <div>
                            <asp:SqlDataSource ID="DocumentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="EX_GetJobDocsDetail" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:SqlDataSource ID="DataSourcePaymentDoument" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="AC_GetInvoiceDocumentBYJobId" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </ContentTemplate>
                    </AjaxToolkit:TabPanel>
                <%--end Billing--%>
            </AjaxToolkit:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

