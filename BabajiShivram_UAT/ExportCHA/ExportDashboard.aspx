<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ExportDashboard.aspx.cs" Inherits="ExportCHA_ExportDashboard" EnableEventValidation="false" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <script type="text/javascript">

        function OnCustomerSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');

            $get('<%=hdnCustId.ClientID%>').value = results.ClientId;
        }

        function OnBranchSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');

            $get('<%=hdnBranchId.ClientID%>').value = results.BranchId;
        }

    </script>
    <style type="text/css">
        .heading {
            line-height: 20px;
        }

        .modalBackground {
            background-color: black;
            filter: alpha(opacity=90);
            opacity: 0.5;
        }

        .modalPopup {
            border-radius: 5px;
            background-color: #FFFFFF;
            border-width: 1px;
            border-style: solid;
            border-color: black;
            padding-top: 5px;
            padding-left: 3px;
            width: 300px;
            height: 140px;
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
    <asp:UpdatePanel ID="upCustomerUser" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div id="divSearch" style="width: 60%;">
                <div style="float: left; width: 40%; padding-left: 20px;">
                    <asp:HiddenField ID="hdnCustId" runat="server" Value="0" />
                    <asp:TextBox ID="txtCustomer" Width="100%" runat="server" MaxLength="100" AutoPostBack="true"
                        OnTextChanged="txtSearchText_TextChanged" placeholder="Search Customer Name"></asp:TextBox>
                    <div id="divwidthCust">
                    </div>
                    <AjaxToolkit:AutoCompleteExtender ID="CustomerExtender" runat="server" TargetControlID="txtCustomer"
                        CompletionListElementID="divwidthCust" ServicePath="WebService/CustomerAutoComplete.asmx"
                        ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCust"
                        ContextKey="4317" UseContextKey="True" OnClientItemSelected="OnCustomerSelected"
                        CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                        CompletionListHighlightedItemCssClass="AutoExtenderHighlight" FirstRowSelected="true">
                    </AjaxToolkit:AutoCompleteExtender>
                </div>

                <div style="float: left; width: 40%; padding-left: 20px;">
                    <asp:HiddenField ID="hdnBranchId" runat="server" Value="0" />
                    <asp:TextBox ID="txtBranch" Width="100%" runat="server" MaxLength="100" AutoPostBack="true"
                        OnTextChanged="txtSearchText_TextChanged" placeholder="Search Branch Name"></asp:TextBox>
                    <div id="divwidthBranch">
                    </div>
                    <AjaxToolkit:AutoCompleteExtender ID="branchExtender" runat="server" TargetControlID="txtBranch"
                        CompletionListElementID="divwidthBranch" ServicePath="WebService/BranchAutoComplete.asmx"
                        ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthBranch"
                        ContextKey="4319" UseContextKey="True" OnClientItemSelected="OnBranchSelected"
                        CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                        CompletionListHighlightedItemCssClass="AutoExtenderHighlight" FirstRowSelected="true">
                    </AjaxToolkit:AutoCompleteExtender>
                </div>
            </div>
            <br />
            <div style="float: left; margin-left: 10px; width: 49%;">

                <fieldset>

                    <legend>Priority Shipment</legend>
                    <asp:LinkButton ID="lnkPriorityWiseList" runat="server" OnClick="lnkPriorityWiseList_Click"
                        data-tooltip="&nbsp; &nbsp; &nbsp; Export To Excel">
                        <asp:Image ID="Image3" runat="server" ImageUrl="../Images/Excel.jpg" />
                    </asp:LinkButton>
                    <div style="height: 263px; overflow: scroll">
                        <asp:GridView ID="gvPriorityShipments" runat="server" AutoGenerateColumns="False" DataKeyNames="lid" Style="white-space: normal"
                            Width="99%" CssClass="table" DataSourceID="DataSourcePriorityShipment" OnRowCommand="gvPriorityShipments_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Job No" ItemStyle-Width="25%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkPriorityShipmentJobNo" runat="server" Text='<%#Eval("JobRefNo") %>'
                                            CommandArgument='<%#Eval("lid")+","+ Eval("ActivityStatus") %>' CommandName="PriorityShipmentJob" ForeColor="Black"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CustName" HeaderText="Customer Name"  ItemStyle-Width="35%" />
                                <asp:BoundField DataField="Status" HeaderText="Stage"  ItemStyle-Width="20%" />
                                <asp:BoundField DataField="NoOfDaysPending" HeaderText="No of days pending"  ItemStyle-Width="15%" />
                            </Columns>
                        </asp:GridView>
                        <div>
                            <asp:SqlDataSource ID="DataSourcePriorityShipment" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="EX_GetPriorityWsJobDetails" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                    <asp:SessionParameter Name="FinYearID" SessionField="FinYearId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </div>
                </fieldset>

                <fieldset>
                    <legend>Job Opening Balance</legend>
                    <asp:LinkButton ID="lnkbtnJobOpeningExport" runat="server" OnClick="lnkbtnJobOpeningExport_Click"
                        data-tooltip="&nbsp; &nbsp; &nbsp; Export To Excel">
                        <asp:Image ID="Image6" runat="server" ImageUrl="../Images/Excel.jpg" />
                    </asp:LinkButton>
                    <div id="divOPen">
                        <asp:GridView ID="gvJobOpening" runat="server" AutoGenerateColumns="False" DataKeyNames="StatusID" ShowFooter="true"
                            Width="99%" CssClass="table" OnPreRender="gvJobOpening_PreRender" DataSourceID="SqlDataSourceJobOpening" OnRowDataBound="gvJobOpening_RowDataBound"
                            FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                            <%--OnRowCommand="gvJobOpening_RowCommand">--%>
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="StatusName" HeaderText="Sub Department" />
                                <asp:BoundField DataField="OpenBAL" HeaderText="Opening Balance" />
                                <asp:BoundField DataField="NewFile" HeaderText="New Files" />
                                <asp:BoundField DataField="CompleteToday" HeaderText="Completed Today" />
                                <asp:BoundField DataField="CloseBAL" HeaderText="Closing Balance" />
                            </Columns>
                        </asp:GridView>
                        <div>
                            <asp:SqlDataSource ID="SqlDataSourceJobOpening" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                DataSourceMode="DataSet" OnSelected="DataSourcePortwise_Selected"
                                SelectCommand="EX_GetJobOpeningLists" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                    <asp:ControlParameter ControlID="hdnCustId" PropertyName="Value" Name="CustId" />
                                    <asp:ControlParameter ControlID="hdnBranchId" PropertyName="Value" Name="BranchId" />
                                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </div>
                </fieldset>

                <div>
                    <asp:HiddenField ID="modelPopup12" runat="server" />
                    <AjaxToolkit:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="modelPopup12"
                        PopupControlID="Panel3" BackgroundCssClass="modalBackground" DropShadow="true"
                        CancelControlID="ImageButton2">
                    </AjaxToolkit:ModalPopupExtender>

                    <asp:Panel ID="Panel3" runat="server" CssClass="modalPopup" BackColor="#F5F5DC" Width="650px" Height="300px">
                        <div id="div3" runat="server">
                            <table width="100%">
                                <tr class="heading">
                                    <td align="center" style="background-color: #F5F5DC">
                                        <b>&nbsp;&nbsp;<asp:Label ID="lblTitle" runat="server" Text=""></asp:Label></b>
                                        &nbsp;&nbsp;&nbsp;
                                            <asp:LinkButton ID="lnkStageDetail" runat="server" OnClick="lnkStageDetail_Click" ToolTip="Export To Excel">
                                                <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/Excel.jpg" Style="margin-top: 5px" />
                                            </asp:LinkButton>
                                        <span style="float: right">
                                            <asp:ImageButton ID="ImageButton2" ImageUrl="~/Images/delete.gif" runat="server" OnClick="btnCancelPopup_Click1" ToolTip="Close" />
                                        </span>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="Panel5" runat="server" Width="645px" Height="265px" ScrollBars="Auto" BorderStyle="Solid" BorderWidth="1px">
                                <asp:GridView ID="grvPopupForSagePending" runat="server" AutoGenerateColumns="false" AllowPaging="false"
                                    CssClass="gridview" OnRowCommand="grvPopupForSagePending_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex +1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Job No">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkJobNo" runat="server" Text='<%#Eval("JobRefNo") %>'
                                                    CommandArgument='<%#Eval("lid") + ";" + Eval("ActivityStatus")%>' CommandName="RedirectJob" ForeColor="Black"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CustName" HeaderText="Party Name" SortExpression="CustName" />
                                        <asp:BoundField DataField="CurrentDate" HeaderText="Arrived On Date" SortExpression="CurrentDate" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="NoOfDaysPending" HeaderText="NoOfDaysPending" SortExpression="NoOfDaysPending" />
                                    </Columns>
                                </asp:GridView>

                            </asp:Panel>
                        </div>
                    </asp:Panel>
                </div>

                 <fieldset>
                    <legend>No. of Job Pending - Stage Wise</legend>
                    <asp:LinkButton ID="lnkStageWisePendinglist" runat="server" OnClick="lnkStageWisePendinglist_Click"
                        data-tooltip="&nbsp; &nbsp; &nbsp; Export To Excel">
                        <asp:Image ID="Image1" runat="server" ImageUrl="../Images/Excel.jpg" />
                    </asp:LinkButton>
                    <asp:GridView ID="gvPendingJob" runat="server" RowStyle-Wrap="false" OnRowCommand="gvPendingJob_RowCommand" AutoGenerateColumns="false" Style="height: 100%" Width="99%"
                        DataSourceID="DataSourcePendingDeptWise" CssClass="table" FooterStyle-ForeColor="Black" OnRowDataBound="gvPendingJob_RowDataBound"
                        FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                        <RowStyle Wrap="true" />
                        <Columns>
                            <asp:TemplateField HeaderText="SI">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("StatusID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="StatusName" HeaderText="Stage Name" SortExpression="StatusName" ItemStyle-Width="15%"
                                ItemStyle-Wrap="true" />

                            <asp:TemplateField HeaderText="No Of Jobs">
                                <ItemTemplate>
                                    <asp:Label ID="lblPendingJob" runat="server" Text='<%#Eval("OpenBAL") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Show Detail">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkNoOfStagePending" runat="server" Text="Show Detail" CommandName="show"
                                        CommandArgument='<%#Eval("StatusID") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="DataSourcePendingDeptWise" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="EX_GetJobPendingDeptWise" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:SessionParameter Name="UserId" SessionField="UserId" />
                            <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </fieldset>
                <fieldset>
                    <legend>Total No. Of Files Pending For Billing</legend>
                    <asp:LinkButton ID="lnkShipmentClearancePending" runat="server" OnClick="lnkShipmentClearancePending_Click"
                        data-tooltip="&nbsp; &nbsp; &nbsp; &nbsp; Export To Excel">
                        <asp:Image ID="imgShipmentClearancePending" runat="server" ImageUrl="../Images/Excel.jpg" />
                    </asp:LinkButton>
                    <asp:GridView ID="gvShipmentGetInPending" runat="server" FooterStyle-BackColor="#CCCCFF"
                        OnRowDataBound="gvShipmentGetInPending_RowDataBound" ShowFooter="true" RowStyle-Wrap="false"
                        AutoGenerateColumns="false" Style="height: 100%" Width="99%" DataSourceID="SQlDataSourceForShipmentGetIn"
                        CssClass="table">
                        <RowStyle Wrap="true" />
                        <Columns>
                            <asp:BoundField DataField="Stage" HeaderText="Stage" SortExpression="Stage" />
                            <asp:TemplateField HeaderText="NoOfJobs">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkNoOfJobs" runat="server" Text='<%#Eval("noofjobs") %>' OnClick="lnkNoOfJobs_Click"
                                        CommandArgument='<%#Eval("Stage")%>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Creditamount" HeaderText="Credit Amount" SortExpression="Creditamount"
                                ItemStyle-Width="15%" ItemStyle-Wrap="true" Visible="false" />
                            <asp:BoundField DataField="Debitamount" HeaderText="Debit Amount" SortExpression="Debitamount"
                                ItemStyle-Width="15%" ItemStyle-Wrap="true" />
                        </Columns>
                    </asp:GridView>
                    <asp:HiddenField ID="hdnUserId" runat="server" Value="0" />
                    <asp:SqlDataSource ID="SQlDataSourceForShipmentGetIn" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="EX_GetPendingFilesForBilling" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="hdnCustId" PropertyName="Value" Name="CustWiseId" />
                            <asp:ControlParameter ControlID="hdnBranchId" PropertyName="Value" Name="BranchwiseId" />
                            <asp:SessionParameter Name="UserId" SessionField="UserId" />
                            <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </fieldset>
                <fieldset style="height: 190px">
                    <legend>Summary Of Bill Pending List</legend>
                    <div>
                        <%-- <asp:DropDownList ID="Drpdashboard" runat="server" AutoPostBack="true" Height="20px" Width="225px" BackColor="#FFFFCC"
                            OnSelectedIndexChanged="Drpdashboard_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                            <asp:ListItem Value="3">More Than 3 Days</asp:ListItem>
                            <asp:ListItem Value="6">User Keeps File More Than 1 Days</asp:ListItem>
                        </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;--%>
                        <asp:LinkButton ID="lnkPriorityClearenace" runat="server" OnClick="lnkPriorityClearenace_Click" data-tooltip="&nbsp; Export To Excel">
                            <asp:Image ID="imgPriorityClearenace" ImageAlign="Middle" runat="server" ImageUrl="~/Images/Excel.jpg" />
                        </asp:LinkButton>
                    </div>

                    <asp:Panel ID="panBillSummary" runat="server" ScrollBars="Auto" Height="150px">

                        <table id="tbl1" runat="server" style="float: left; width: 99%;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblDispatchdays" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="gvDispatchdays" AllowPaging="true" AllowSorting="true" Width="100%" PageSize="4" runat="server" OnRowCommand="gvDispatchdays_RowCommand"
                                        AutoGenerateColumns="false" Style="white-space: normal" DataSourceID="sqlDispatchdays" CssClass="table" OnPreRender="gvDispatchdays_PreRender">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Job No" ItemStyle-Width="20%">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkJobNo" runat="server" Text='<%#Eval("JobNo") %>'
                                                        CommandArgument='<%#Eval("lid")%>' CommandName="ShowJob" ForeColor="Black">
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="JobNo" HeaderText="BS Job Number" SortExpression="JobNo" ItemStyle-Width="20%" Visible="false" />
                                            <asp:BoundField DataField="ClientName" HeaderText="Client Name" SortExpression="ClientName" ItemStyle-Width="45%" />
                                            <asp:BoundField DataField="Aging I" HeaderText="Pending Days" SortExpression="Aging I" ItemStyle-Width="15%" />
                                            <asp:BoundField DataField="Stage" HeaderText="Stage" SortExpression="Stage" ItemStyle-Width="20%" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <asp:SqlDataSource ID="sqlDispatchdays" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="EX_GetBillPendingSummary" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                        <%--  <table id="tbl2" runat="server" style="display: none; float: left; width: 50%;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblDispatchWeek" Width="500px" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="gvDispatchWeek" AllowPaging="true" AllowSorting="true" Width="550px"
                                        PageSize="5" runat="server" AutoGenerateColumns="false" DataSourceID="sqlDispatchweek" CssClass="table" OnPreRender="gvDispatchWeek_PreRender">
                                        <Columns>
                                            <asp:BoundField DataField="JobNo" HeaderText="JobNo" SortExpression="JobNo" />
                                            <asp:BoundField DataField="ClientName" HeaderText="Client Name" SortExpression="ClientName" />
                                            <asp:BoundField DataField="Aging I" HeaderText="Pending Days" SortExpression="Aging I" />
                                            <asp:BoundField DataField="Dispatch Week" HeaderText="Dispatch Week" SortExpression="Dispatch Week" />
                                            <asp:BoundField DataField="Stage" HeaderText="Stage" SortExpression="Stage" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <asp:SqlDataSource ID="sqlDispatchweek" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="EX_GetDispatchWeekRpt" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hdnCustId" PropertyName="Value" Name="CustWiseId" />
                                <asp:ControlParameter ControlID="hdnBranchId" PropertyName="Value" Name="BranchwiseId" />
                                <asp:ControlParameter ControlID="hdnUserId" PropertyName="Value" Name="UserwiseId" />
                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                        <table id="tbl3" runat="server" style="display: none; float: left; width: 50%;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblhighcreditdays" Width="500px" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="gvhighcreditdays" AllowPaging="true" AllowSorting="true" Width="550px"
                                        OnPreRender="gvhighcreditdays_PreRender" PageSize="5" runat="server" AutoGenerateColumns="false" DataSourceID="sqlhighcreditdays" CssClass="table">
                                        <Columns>
                                            <asp:BoundField DataField="JobNo" HeaderText="JobNo" SortExpression="JobNo" />
                                            <asp:BoundField DataField="ClientName" HeaderText="Client Name" SortExpression="ClientName" />
                                            <asp:BoundField DataField="Aging I" HeaderText="Pending Days" SortExpression="Aging I" />
                                            <asp:BoundField DataField="High Credit Days" HeaderText="High Credit Days" SortExpression="High Credit Days" />
                                            <asp:BoundField DataField="Stage" HeaderText="Stage" SortExpression="Stage" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <asp:SqlDataSource ID="sqlhighcreditdays" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="EX_GethighcreditdaysRpt" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hdnCustId" PropertyName="Value" Name="CustWiseId" />
                                <asp:ControlParameter ControlID="hdnBranchId" PropertyName="Value" Name="BranchwiseId" />
                                <asp:ControlParameter ControlID="hdnUserId" PropertyName="Value" Name="UserwiseId" />
                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                        <table id="tbl4" runat="server" style="display: none; float: left; width: 50%;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblmorethan3days" Width="500px" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="gvmorethan3days" AllowPaging="true" AllowSorting="true" Width="550px"
                                        OnPreRender="gvmorethan3days_PreRender" PageSize="5" runat="server" AutoGenerateColumns="false" DataSourceID="sqlmorethan3days" CssClass="table">
                                        <Columns>
                                            <asp:BoundField DataField="JobNo" HeaderText="JobNo" SortExpression="JobNo" />
                                            <asp:BoundField DataField="ClientName" HeaderText="Client Name" SortExpression="ClientName" />
                                            <asp:BoundField DataField="Aging I" HeaderText="Pending Days" SortExpression="Aging I" />
                                            <asp:BoundField DataField="Stage" HeaderText="Stage" SortExpression="Stage" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <asp:SqlDataSource ID="sqlmorethan3days" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="EX_Getmorethan3days" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hdnCustId" PropertyName="Value" Name="CustWiseId" />
                                <asp:ControlParameter ControlID="hdnBranchId" PropertyName="Value" Name="BranchwiseId" />
                                <asp:ControlParameter ControlID="hdnUserId" PropertyName="Value" Name="UserwiseId" />
                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                        <table id="tbl7" runat="server" style="display: none; float: left; width: 50%;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblmorethan1days" Width="500px" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="gvuserKeepmorethan1days" AllowPaging="true" AllowSorting="true" Width="550px" PageSize="5" runat="server"
                                        OnPreRender="gvuserKeepmorethan1days_PreRender" AutoGenerateColumns="false" DataSourceID="sqluserkeepfilemorethan1days" CssClass="table">
                                        <Columns>
                                            <asp:BoundField DataField="JobNo" HeaderText="JobNo" SortExpression="JobNo" />
                                            <asp:BoundField DataField="ClientName" HeaderText="Client Name" SortExpression="ClientName" />
                                            <asp:BoundField DataField="Aging I" HeaderText="Pending Days" SortExpression="Aging I" />
                                            <asp:BoundField DataField="Stage" HeaderText="Stage" SortExpression="Stage" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <asp:SqlDataSource ID="sqluserkeepfilemorethan1days" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="EX_Getuserkeepfilemorethan1days" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hdnCustId" PropertyName="Value" Name="CustWiseId" />
                                <asp:ControlParameter ControlID="hdnBranchId" PropertyName="Value" Name="BranchwiseId" />
                                <asp:ControlParameter ControlID="hdnUserId" PropertyName="Value" Name="UserwiseId" />
                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                            </SelectParameters>
                        </asp:SqlDataSource>--%>
                    </asp:Panel>
                </fieldset>
                <table id="tbShipmentGetInDetails" runat="server" style="display: none">
                    <tr>
                        <td>
                            <asp:HiddenField ID="modelPopup" runat="server" />
                            <AjaxToolkit:ModalPopupExtender ID="mpeShipmentGetIn" runat="server" TargetControlID="modelPopup" BackgroundCssClass="modalBackground"
                                PopupControlID="pnlShipmentGetInDetails" DropShadow="true">
                            </AjaxToolkit:ModalPopupExtender>

                            <asp:Panel ID="pnlShipmentGetInDetails" runat="server" CssClass="modalPopup1" BackColor="#F5F5DC" Width="605px" Height="335px">
                                <div id="div1" runat="server">
                                    <table width="100%">
                                        <tr>
                                            <td align="center">
                                                <b>&nbsp;&nbsp;<asp:Label ID="lblbillingpendingFiles" runat="server" Text=""></asp:Label></b>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="lnkExportShipmentDetails" runat="server" OnClick="lnkExportShipmentDetails_Click" ToolTip="Export To Excel">
                                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/images/Excel.jpg" Style="margin-top: 5px" />
                                                </asp:LinkButton>
                                                <span style="float: right">
                                                    <asp:ImageButton ID="imgClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="btnCancelPopup_Click" ToolTip="Close" />
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Panel ID="panScroll" runat="server" Width="600px" Height="300px" ScrollBars="Auto" BorderStyle="Solid" BorderWidth="1px">
                                        <asp:GridView ID="gvShipmentDetails" runat="server" CssClass="gridview" AutoGenerateColumns="false"
                                            AllowPaging="false" OnRowCommand="gvShipmentDetails_RowCommand">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex +1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Job No">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkShipmentJobNo" runat="server" Text='<%#Eval("JobRefNo") %>'
                                                            CommandArgument='<%#Eval("lid")%>' CommandName="ShipmentJob" ForeColor="Black"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="PartyName" HeaderText="Party Name" SortExpression="PartyName" />
                                                <asp:BoundField DataField="Amount" HeaderText="Debit Amount" SortExpression="Amount" />
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </div>
                            </asp:Panel>
                        </td>
                        <td>
                            <div>
                                <asp:HiddenField ID="modelPopup1" runat="server" />
                                <AjaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="modelPopup1" BackgroundCssClass="modalBackground"
                                    PopupControlID="Panel2" DropShadow="true">
                                </AjaxToolkit:ModalPopupExtender>
                                <asp:Panel ID="Panel2" runat="server" CssClass="modalPopup1" BackColor="#F5F5DC" Width="405px" Height="335px">
                                    <div id="div2" runat="server">
                                        <table width="100%">
                                            <tr>
                                                <td align="center">
                                                    <b>&nbsp;&nbsp;<asp:Label ID="lblBillpendList" runat="server" Text=""></asp:Label></b>
                                                    &nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="lnkjobSummarylist" runat="server" OnClick="lnkjobSummarylist_Click" ToolTip="Export To Excel">
                                                        <asp:Image ID="Image4" runat="server" ImageUrl="~/images/Excel.jpg" Style="margin-top: 5px" />
                                                    </asp:LinkButton>
                                                    <span style="float: right">
                                                        <asp:ImageButton ID="imgbtnSummaryShipmentDetails" ImageUrl="~/Images/delete.gif" runat="server" OnClick="btnCancelPopup_Click1" ToolTip="Close" />
                                                    </span>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Panel ID="Panel4" runat="server" Width="400px" Height="300px" ScrollBars="Auto" BorderStyle="Solid" BorderWidth="1px">
                                            <asp:GridView ID="gvsummarylist" runat="server" CssClass="gridview" AutoGenerateColumns="false"
                                                AllowPaging="false" OnRowCommand="gvsummarylist_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sl">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex +1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Job No">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkBillJobNo" runat="server" Text='<%#Eval("JobrefNo") %>'
                                                                CommandArgument='<%#Eval("lid")%>' CommandName="BillJob" ForeColor="Black"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="PartyName" HeaderText="Party Name" SortExpression="PartyName" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Debit Amount" SortExpression="Amount" />
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </div>
                                </asp:Panel>

                                <asp:SqlDataSource ID="DsSummarylist" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="EX_GetBillPendingDetails" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:Parameter Name="Stage" Type="Int16" />
                                        <asp:ControlParameter ControlID="hdnCustId" PropertyName="Value" Name="CustWiseId" />
                                        <asp:ControlParameter ControlID="hdnBranchId" PropertyName="Value" Name="BranchwiseId" />
                                        <asp:ControlParameter ControlID="hdnUserId" PropertyName="Value" Name="UserwiseId" />
                                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </td>
                    </tr>
                </table>
                <asp:SqlDataSource ID="DataSourceShipmentGetIn" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="EX_GetPendingShipmentGetInDetails" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:Parameter Name="Stage" Type="Int16" />
                        <asp:ControlParameter ControlID="hdnCustId" PropertyName="Value" Name="CustWiseId" />
                        <asp:ControlParameter ControlID="hdnBranchId" PropertyName="Value" Name="BranchwiseId" />
                        <asp:ControlParameter ControlID="hdnUserId" PropertyName="Value" Name="UserwiseId" />
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>

            <div style="float: right; margin-left: 2px; margin-right: 10px; width: 49%;">
                <fieldset><legend>Under Clearance – Current Job</legend>
                <div id="divUnderClearance">
                    <asp:LinkButton ID="lnkUnderClearance" runat="server" OnClick="lnkUnderClearance_Click" data-tooltip="&nbsp; &nbsp; &nbsp; Export To Excel">
                        <asp:Image ID="Image9" runat="server" ImageUrl="~/images/Excel.jpg" />
                    </asp:LinkButton>
                    <div align="right">
                         <asp:label ID="lblHeader" runat="server" Text="Shipment Under Clearance and Pending for Shipping Bill, Customs, Leo and Shipped On Board"  ></asp:label>
                     </div>

                    <asp:GridView ID="gvUnderClearance" runat="server" AutoGenerateColumns="False" CssClass="table" 
                        PagerStyle-CssClass="pgr" AllowPaging="True" AllowSorting="True" DataSourceID="SqlDataSourceUnderClearance"
                        OnRowDataBound="gvJobDetail_RowDataBound" PagerSettings-Position="TopAndBottom" PageSize="10" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:BoundField DataField="ref" HeaderText="Job No" SortExpression="ref"/>--%>
                            <asp:TemplateField HeaderText="Job No" SortExpression="ref">
                                <ItemTemplate>
                                    <div style="word-wrap: break-word; width: 80px; white-space:normal;">
                                        <asp:Label ID="lblJobNo1" runat="server" Text='<%#Bind("ref") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Customer" SortExpression="par_name">
                                <ItemTemplate>
                                    <div style="word-wrap: break-word; width: 90px; white-space:normal;">
                                        <asp:Label ID="lblCust" runat="server" Text='<%#Bind("par_name") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                <ItemTemplate>
                                     <div style="word-wrap: break-word; width: 50px; white-space:normal;">
                                    <asp:Label ID="lblStatus" runat="server" DataFormatString="{0:dd/MM/yyyy}"></asp:Label>
                                         </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="SBDate" HeaderText="SB Date" SortExpression="SBDate" DataFormatString="{0:dd/MM/yyyy}"/>
                            <asp:BoundField DataField="LEODate" HeaderText="Leo Date" SortExpression="LEODate" DataFormatString="{0:dd/MM/yyyy}"/>
                            <asp:BoundField DataField="ShippingLineDate" HeaderText="GetIn Date" SortExpression="ShippingLineDate" DataFormatString="{0:dd/MM/yyyy}"/>
                            <asp:TemplateField HeaderText="KAM" SortExpression="KAM">
                                <ItemTemplate>
                                    <div style="word-wrap: break-word; width: 50px; white-space:normal;">
                                        <asp:Label ID="lblKAM" runat="server" ></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount"/>
                        </Columns>
                    </asp:GridView>

                    <div>
                        <asp:SqlDataSource ID="SqlDataSourceUnderClearance" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        DataSourceMode="DataSet" EnableCaching="true" CacheDuration="600"
		                SelectCommand="Ex_BJVGetUnderClearanceReport" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:SessionParameter Name="UserId" SessionField="UserId" />
                            <asp:SessionParameter Name="ModuleID" SessionField="MID" />
                            <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    </div>
                </div>
            </fieldset>
               <fieldset><legend>Current Amount > 1 Lac</legend>
                <div id="divPortWise">
                    <asp:LinkButton ID="lnkCurrentAmtXls" runat="server" OnClick="lnkCurrentAmtXls_Click" data-tooltip="&nbsp; &nbsp; &nbsp; Export To Excel">
                        <asp:Image ID="Image7" runat="server" ImageUrl="~/images/Excel.jpg" />
                    </asp:LinkButton>
                    <div align="right">
                         <asp:label ID="lblHeader1" runat="server" Text="Shipment expenses are more than 1 lac"  ></asp:label>
                     </div>

                    <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table" 
                        PagerStyle-CssClass="pgr" AllowPaging="True" AllowSorting="True" DataSourceID="FASqlDataSource"
                        OnRowDataBound="gvJobDetail_RowDataBound" PagerSettings-Position="TopAndBottom" PageSize="10" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                           <%-- <asp:BoundField DataField="ref" HeaderText="Job No" SortExpression="ref"/>--%>
                            <asp:TemplateField HeaderText="Job No" SortExpression="ref">
                                <ItemTemplate>
                                    <div style="word-wrap: break-word; width: 80px; white-space:normal;">
                                        <asp:Label ID="lblJobNo" runat="server" Text='<%#Bind("ref") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Customer" SortExpression="par_name">
                                <ItemTemplate>
                                    <div style="word-wrap: break-word; width: 90px; white-space:normal;">
                                        <asp:Label ID="lblCust" runat="server" Text='<%#Bind("par_name") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                <ItemTemplate>
                                     <div style="word-wrap: break-word; width: 50px; white-space:normal;">
                                    <asp:Label ID="lblStatus" runat="server" DataFormatString="{0:dd/MM/yyyy}"></asp:Label>
                                         </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="SBDate" HeaderText="SB Date" SortExpression="SBDate" DataFormatString="{0:dd/MM/yyyy}"/>
                            <asp:BoundField DataField="LEODate" HeaderText="Leo Date" SortExpression="LEODate" DataFormatString="{0:dd/MM/yyyy}"/>
                            <asp:BoundField DataField="ShippingLineDate" HeaderText="GetIn Date" SortExpression="ShippingLineDate" DataFormatString="{0:dd/MM/yyyy}"/>
                            <asp:TemplateField HeaderText="KAM" SortExpression="KAM">
                                <ItemTemplate>
                                    <div style="word-wrap: break-word; width: 50px; white-space:normal;">
                                        <asp:Label ID="lblKAM" runat="server" ></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount"/>
                        </Columns>
                    </asp:GridView>

                    <div>
                        <asp:SqlDataSource ID="FASqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        DataSourceMode="DataSet" EnableCaching="true" CacheDuration="600"
		                SelectCommand="BJV_GetCurrAmtGreaterthan1Lac" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:SessionParameter Name="UserId" SessionField="UserId" />
                            <asp:SessionParameter Name="ModuleID" SessionField="MID" />
                            <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    </div>
                </div>
            </fieldset>

            <fieldset><legend>Shipment Cleared – Job Pending to Send for Billing</legend>
                <asp:LinkButton ID="lnkShipmentCleared" runat="server" OnClick="lnkShipmentCleared_Click" data-tooltip="&nbsp; &nbsp; &nbsp; Export To Excel">
                        <asp:Image ID="Image8" runat="server" ImageUrl="~/images/Excel.jpg" />
                    </asp:LinkButton>

                    <div align="right">
                         <asp:label ID="lblHeader2" runat="server" Text="Shipped on Board Completed and File Pending For Billing"  ></asp:label>
                     </div>

                    <asp:GridView ID="gvShipmentCleared" runat="server" RowStyle-Wrap="false"
                        ShowFooter="true" AutoGenerateColumns="false" Style="height: 100%" Width="100%"
                        DataSourceID="SqlDataPendingList" CssClass="table" FooterStyle-ForeColor="Black" PageSize="10" AllowPaging="true"
                        FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                        <RowStyle Wrap="true" />
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Job No" SortExpression="JobRefNo">
                                <ItemTemplate>
                                    <div style="word-wrap: break-word; width: 80px; white-space:normal;">
                                        <asp:Label ID="lblJobNo1" runat="server" Text='<%#Bind("JobRefNo") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:BoundField DataField="JobRefNo" HeaderText="Job No" SortExpression="JobRefNo"/>--%>
                            
                            <asp:TemplateField HeaderText="Customer" SortExpression="par_name">
                                <ItemTemplate>
                                    <div style="word-wrap: break-word; width: 90px; white-space:normal;">
                                        <asp:Label ID="lblPartyName" runat="server" Text='<%#Bind("PartyName") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="KAM" SortExpression="KAM">
                                <ItemTemplate>
                                    <div style="word-wrap: break-word; width: 50px; white-space:normal;">
                                        <asp:Label ID="lblKAM1" runat="server" Text='<%#Bind("KAM") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Stage" SortExpression="stage">
                                <ItemTemplate>
                                    <div style="word-wrap: break-word; width: 50px; white-space:normal;">
                                        <asp:Label ID="lblstage" runat="server" Text='<%#Bind("stage") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                           <asp:BoundField DataField="SBDate" HeaderText="SB Date" SortExpression="SBDate" DataFormatString="{0:dd/MM/yyyy}"/>
                            <asp:BoundField DataField="LEODate" HeaderText="Leo Date" SortExpression="LEODate" DataFormatString="{0:dd/MM/yyyy}"/>
                            <asp:BoundField DataField="ShippingLineDate" HeaderText="GetIn Date" SortExpression="ShippingLineDate" DataFormatString="{0:dd/MM/yyyy}"/>
                           <%-- <asp:BoundField DataField="ShippingLineDate" HeaderText="Shipment Date" SortExpression="ShippingLineDate" DataFormatString="{0:dd/MM/yyyy}"/>
                            <asp:BoundField DataField="DispatchDate" HeaderText="Dispatch" SortExpression="DispatchDate" DataFormatString="{0:dd/MM/yyyy}"/>--%>
                            <asp:BoundField DataField="Transport By" HeaderText="Trans By" SortExpression="Transport By"/>
                             <asp:BoundField DataField="DEBITAMT" HeaderText="Amount" SortExpression="DEBITAMT" />
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataPendingList" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        DataSourceMode="DataSet" EnableCaching="true" CacheDuration="600"
		                SelectCommand="EX_GetShipmentClearancePendingForBillingList" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="hdnCustId" PropertyName="Value" Name="CustWiseId" />
                            <asp:ControlParameter ControlID="hdnBranchId" PropertyName="Value" Name="BranchwiseId" />
                            <asp:ControlParameter ControlID="hdnUserId" PropertyName="Value" Name="UserwiseId" />
                            <asp:SessionParameter Name="UserId" SessionField="UserId" />
                            <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
            </fieldset>

                
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

