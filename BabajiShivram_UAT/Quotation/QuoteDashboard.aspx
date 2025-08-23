<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="QuoteDashboard.aspx.cs"
    Inherits="Quotation_QuoteDashboard" Title="Quotation Dashboard"
    EnableEventValidation="false" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <style type="text/css">
        .modalBackground {
            background-color: black;
            filter: alpha(opacity=90);
            opacity: 0.5;
        }
    </style>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPanel" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPanel" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div style="float: left; width: 70%;">
                <!--Freight Month Summary -->
                <fieldset>
                    <legend>Quotation Summary</legend>
                    <div>
                        <b>Summary Type</b>
                        <asp:DropDownList ID="ddPendingQuotation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddPendingQuotation_SelectedIndexChanged">
                            <asp:ListItem Text="Pending Quotation" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Overall Summary" Value="2" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;
                         <asp:LinkButton ID="lnkTotalSummaryExport" runat="server" OnClick="lnkTotalSummaryExport_Click">
                             <asp:Image ID="Image3" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                         </asp:LinkButton>
                    </div>
                    <div>
                        <asp:GridView ID="gvSummaryQuotationMonth" runat="server" CssClass="table" DataSourceID="DataSourceQuotationMonth"
                            Width="99%" AutoGenerateColumns="false" OnRowCommand="gvSummaryQuotationMonth_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lnkStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Apr">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkApr" runat="server" Text='<%#Eval("Apr") %>' CommandName="MonthDetail"
                                            CommandArgument='<%#Eval("StatusId")+";4" %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="May">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkMay" runat="server" Text='<%#Eval("May") %>' CommandName="MonthDetail"
                                            CommandArgument='<%#Eval("StatusId")+";5" %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Jun">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkJun" runat="server" Text='<%#Eval("Jun") %>' CommandName="MonthDetail"
                                            CommandArgument='<%#Eval("StatusId")+";6" %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Jul">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkJul" runat="server" Text='<%#Eval("Jul") %>' CommandName="MonthDetail"
                                            CommandArgument='<%#Eval("StatusId")+";7" %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Aug">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkAug" runat="server" Text='<%#Eval("Aug") %>' CommandName="MonthDetail"
                                            CommandArgument='<%#Eval("StatusId")+";8" %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sep">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkSep" runat="server" Text='<%#Eval("Sep") %>' CommandName="MonthDetail"
                                            CommandArgument='<%#Eval("StatusId")+";9" %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Oct">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkOct" runat="server" Text='<%#Eval("Oct") %>' CommandName="MonthDetail"
                                            CommandArgument='<%#Eval("StatusId")+";10" %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Nov">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkNov" runat="server" Text='<%#Eval("Nov") %>' CommandName="MonthDetail"
                                            CommandArgument='<%#Eval("StatusId")+";11" %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dec">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDec" runat="server" Text='<%#Eval("Dec") %>' CommandName="MonthDetail"
                                            CommandArgument='<%#Eval("StatusId")+";12" %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Jan">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkJan" runat="server" Text='<%#Eval("Jan") %>' CommandName="MonthDetail"
                                            CommandArgument='<%#Eval("StatusId")+";1" %>'> </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Feb">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkFeb" runat="server" Text='<%#Eval("Feb") %>' CommandName="MonthDetail"
                                            CommandArgument='<%#Eval("StatusId")+";2" %>'> </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mar">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkMar" runat="server" Text='<%#Eval("Mar") %>' CommandName="MonthDetail"
                                            CommandArgument='<%#Eval("StatusId")+";3" %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </fieldset>
                <div>
                    <asp:SqlDataSource ID="DataSourceQuotationMonth" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="BS_GetQuotationMonthSummary" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:SessionParameter Name="UserId" SessionField="UserId" />
                            <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                            <asp:ControlParameter ControlID="ddPendingQuotation" Name="ReportType" PropertyName="SelectedValue" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
                <!--Freight Summary -->
                <fieldset>
                    <legend runat="server" id="legendName">Customer Wise Summary</legend>
                    <div>
                        <b>Report Month</b>
                        <asp:DropDownList ID="ddMonth" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddReportType_SelectedIndexChanged">
                            <asp:ListItem Value="0" Text="Financial Year"></asp:ListItem>
                            <asp:ListItem Value="1" Text="January"></asp:ListItem>
                            <asp:ListItem Value="2" Text="February"></asp:ListItem>
                            <asp:ListItem Value="3" Text="March"></asp:ListItem>
                            <asp:ListItem Value="4" Text="April"></asp:ListItem>
                            <asp:ListItem Value="5" Text="May"></asp:ListItem>
                            <asp:ListItem Value="6" Text="June"></asp:ListItem>
                            <asp:ListItem Value="7" Text="July"></asp:ListItem>
                            <asp:ListItem Value="8" Text="Aug"></asp:ListItem>
                            <asp:ListItem Value="9" Text="Sep"></asp:ListItem>
                            <asp:ListItem Value="10" Text="Oct"></asp:ListItem>
                            <asp:ListItem Value="11" Text="Nov"></asp:ListItem>
                            <asp:ListItem Value="12" Text="Dec"></asp:ListItem>
                        </asp:DropDownList>
                        <b>Summary For</b>
                        <asp:DropDownList ID="ddReportType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddReportType_SelectedIndexChanged">
                            <asp:ListItem Text="ALL" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Quoted for Customer" Value="1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Quotation Type" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Division" Value="3"></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;<asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                            <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                        </asp:LinkButton>
                    </div>
                    <div>
                        <asp:GridView ID="gvSummaryQuotation" runat="server" CssClass="table" Width="99%" AutoGenerateColumns="false"
                            AllowSorting="true" AllowPaging="true" PageSize="20" PagerStyle-CssClass="pgr"
                            PagerSettings-Position="TopAndBottom" DataSourceID="DataSourceQuotationSummary"
                            OnSorting="gvSummaryQuotation_Sorting" OnRowCommand="gvSummaryQuotation_RowCommand"
                            OnPageIndexChanging="gvSummaryQuotation_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Name" SortExpression="sName">
                                    <ItemTemplate>
                                        <asp:Label ID="lnksName" runat="server" Text='<%#Eval("sName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Draft Quotation" SortExpression="Draft Quotation">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDraftQuotation" runat="server" Text='<%#Eval("Draft Quotation") %>' CommandName="SummaryDetail"
                                            CommandArgument='<%#Eval("lId")+";1" %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Approval Pending" SortExpression="Approval Pending">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkApprovalPending" runat="server" Text='<%#Eval("Approval Pending") %>' CommandName="SummaryDetail"
                                            CommandArgument='<%#Eval("lId")+";2" %>'> </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Approved" SortExpression="Approved">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkApproved" runat="server" Text='<%#Eval("Approved") %>' CommandName="SummaryDetail"
                                            CommandArgument='<%#Eval("lId")+";3" %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rejected" SortExpression="Rejected">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkRejected" runat="server" Text='<%#Eval("Rejected") %>' CommandName="SummaryDetail"
                                            CommandArgument='<%#Eval("lId")+";4" %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Awarded" SortExpression="Awarded">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkAwarded" runat="server" Text='<%#Eval("Awarded") %>' CommandName="SummaryDetail"
                                            CommandArgument='<%#Eval("lId")+";5"%>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Awaited" SortExpression="Awaited">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkAwaited" runat="server" Text='<%#Eval("Awaited") %>' CommandName="SummaryDetail"
                                            CommandArgument='<%#Eval("lId")+";6"%>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lost" SortExpression="Lost">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkLost" runat="server" Text='<%#Eval("Lost") %>' CommandName="SummaryDetail"
                                            CommandArgument='<%#Eval("lId")+";7"%>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Negotiate" SortExpression="Negotiate">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkNegotiate" runat="server" Text='<%#Eval("Negotiate") %>' CommandName="SummaryDetail"
                                            CommandArgument='<%#Eval("lId")+";8"%>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </fieldset>
                <div>
                    <asp:SqlDataSource ID="DataSourceQuotationSummary" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="BS_QuotedForCustSummary" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddPendingQuotation" Name="ReportType" PropertyName="SelectedValue" />
                            <asp:ControlParameter ControlID="ddMonth" Name="MonthId" PropertyName="SelectedValue" />
                            <asp:SessionParameter Name="UserId" SessionField="UserId" />
                            <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
                <!--Popup Month-Status Detail  -->
                <div id="divPopupMonthDetail">
                    <cc1:ModalPopupExtender ID="ModalPopupMonthStatus" runat="server" CacheDynamicResults="false"
                        DropShadow="true" PopupControlID="Panel2Month" TargetControlID="lnkDummyMonth" BackgroundCssClass="modalBackground">
                    </cc1:ModalPopupExtender>
                    <asp:Panel ID="Panel2Month" runat="server" CssClass="ModalPopupPanel">
                        <div style="text-align: center">
                            <asp:Label ID="lblErrorPopup" runat="server" EnableViewState="false"></asp:Label>
                        </div>
                        <div class="header">
                            <div class="fleft">
                                &nbsp;<asp:Button ID="btnCancelPopup" runat="server" OnClick="btnCancelMonthPopup_Click"
                                    Text="Close" CausesValidation="false" />
                            </div>
                            &nbsp;&nbsp;<asp:LinkButton ID="lnkExportSummary" runat="server" OnClick="lnkExportSummary_Click">
                                <asp:Image ID="imgExpSummary" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>
                            <div class="fright">
                                <asp:ImageButton ID="imgClose" ImageUrl="~/Images/delete.gif" OnClick="btnCancelMonthPopup_Click"
                                    runat="server" ToolTip="Close" />
                            </div>
                        </div>
                        <!--Freight Detail Start-->
                        <div id="Div1" runat="server" style="min-height: 200px;max-height: 600px; max-width: 900px; overflow: auto;">
                            <asp:GridView ID="gvQuotationDetails" runat="server" CssClass="table" AutoGenerateColumns="false"
                                PagerStyle-CssClass="pgr" DataKeyNames="lid" AllowPaging="True" AllowSorting="True" PageSize="20"
                                PagerSettings-Position="TopAndBottom" OnRowCommand="gvQuotationDetails_RowCommand" Width="99%"
                                OnSorting="gvQuotationDetails_Sorting" OnPageIndexChanging="gvQuotationDetails_PageIndexChanging"
                                DataSourceID="DataSourceMonthDetail" OnRowDataBound="gvQuotationDetails_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quotation Ref No" SortExpression="QuoteRefNo">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEditQuotation" CommandName="GetQuote" ToolTip="Edit Quotation" runat="server"
                                                Text='<%#Eval("QuoteRefNo") %>' CommandArgument='<%#Eval("lid") + ";" + Eval("ApprovalStatusId") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="QuoteRefNo" HeaderText="Quotation Ref No" SortExpression="QuoteRefNo" ReadOnly="true" Visible="false" />
                                    <asp:BoundField DataField="CustomerName" HeaderText="Customer" SortExpression="CustomerName" ReadOnly="true" />
                                    <asp:BoundField DataField="QuotedFormat" HeaderText="Quotation Type" SortExpression="QuotedFormat" ReadOnly="true" />
                                    <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category" ReadOnly="true" />
                                    <asp:BoundField DataField="Mode" HeaderText="Mode" SortExpression="Mode" ReadOnly="true" />
                                    <asp:BoundField DataField="CreatedBy" HeaderText="Quoted By" SortExpression="CreatedBy" ReadOnly="true" />
                                    <asp:BoundField DataField="CreatedDate" HeaderText="Quoted On" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                                    <asp:BoundField DataField="ModifiedBy" HeaderText="Modified By" SortExpression="ModifiedBy" ReadOnly="true" />
                                    <asp:BoundField DataField="ModifiedDate" HeaderText="Modified On" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                                    <asp:BoundField DataField="ApprovalStatus" HeaderText="Status" SortExpression="ApprovalStatus" ReadOnly="true" Visible="false" />
                                    <asp:TemplateField HeaderText="Status" SortExpression="ApprovalStatus">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lblStatus" runat="server" Text='<%# Eval("ApprovalStatus") %>' CommandArgument='<%#Eval("lid") %>'
                                                CommandName="getstatus" ToolTip="Update Status For Quotation."></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" SortExpression="">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnDownloadQuote" runat="server" CausesValidation="false" CommandName="DownloadQuote"
                                                ImageAlign="AbsMiddle" ImageUrl="~/Images/pdf2.png" Width="16" Height="16" ToolTip="Download Quotation in PDF Format."
                                                CommandArgument='<%#Eval("QuotePath") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                    <div>
                        <asp:SqlDataSource ID="DataSourceMonthDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="BS_GetQuotationMonthDetail" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddPendingQuotation" Name="ReportType" PropertyName="SelectedValue" />
                                <asp:Parameter Name="MonthId" />
                                <asp:Parameter Name="StatusId" />
                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                    <div>
                        <asp:HiddenField ID="lnkDummyMonth" runat="server"></asp:HiddenField>
                    </div>
                </div>
                <!--Popup Freight Summary Detail  -->
                <div id="divPopupSummaryDetail">
                    <cc1:ModalPopupExtender ID="ModalPopupSummaryDetail" runat="server" CacheDynamicResults="false"
                        DropShadow="true" PopupControlID="Panel2Summary" BackgroundCssClass="modalBackground" TargetControlID="lnkDummySummary">
                    </cc1:ModalPopupExtender>
                    <asp:Panel ID="Panel2Summary" runat="server" CssClass="ModalPopupPanel">
                        <div class="header">
                            <div class="fleft">
                                &nbsp;<asp:Button ID="btnCancelSummaryPopup" runat="server" OnClick="btnCancelSummaryPopup_Click"
                                    Text="Close" CausesValidation="false" />
                            </div>
                            &nbsp;&nbsp;<asp:LinkButton ID="lnkExportSummaryDetail" runat="server" OnClick="lnkExportSummaryDetail_Click">
                                <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>
                            <div class="fleft">
                                &nbsp;<asp:Label ID="lblSummaryStatus" Text="" runat="server" />
                            </div>
                            <div class="fright">
                                <asp:ImageButton ID="imgCancelSummaryPopup" ImageUrl="~/Images/delete.gif" OnClick="btnCancelSummaryPopup_Click"
                                    runat="server" ToolTip="Close" />
                            </div>
                        </div>
                        <!--Freight Detail Start-->
                        <div id="Div3" runat="server" style="min-height: 200px;max-height: 550px; max-width: 900px; overflow: auto;">
                            <%--<asp:GridView ID="gvSummaryStatusDetail" runat="server" CssClass="table" Width="98%"
                                AutoGenerateColumns="false" DataSourceID="DataSourceSummaryDetail" AllowSorting="true"
                                AllowPaging="false" PageSize="20" PagerStyle-CssClass="pgr" OnSorting="gvSummaryStatusDetail_Sorting"
                                OnPageIndexChanging="gvSummaryStatusDetail_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ENQRefNo" HeaderText="ENQ No" SortExpression="ENQRefNo" />
                                    <asp:BoundField DataField="ENQDate" HeaderText="ENQ Date" DataFormatString="{0:dd/MM/yyyy}"
                                        SortExpression="ENQDate" />
                                    <asp:BoundField DataField="EnquiryUser" HeaderText="Freight SPC" SortExpression="EnquiryUser" />
                                    <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName" />
                                    <asp:BoundField DataField="StatusDate" HeaderText="Status Date" DataFormatString="{0:dd/MM/yyyy}"
                                        SortExpression="StatusDate" />
                                    <asp:BoundField DataField="CustRefNo" HeaderText="Cust Ref#" SortExpression="CustRefNo" />
                                    <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                                    <asp:BoundField DataField="Shipper" HeaderText="Shipper" SortExpression="Shipper" />
                                    <asp:BoundField DataField="Consignee" HeaderText="Consignee" SortExpression="Consignee" />
                                    <asp:BoundField DataField="CountOf20" HeaderText="20" SortExpression="CountOf20" />
                                    <asp:BoundField DataField="CountOf40" HeaderText="40" SortExpression="CountOf40" />
                                    <asp:BoundField DataField="LCLVolume" HeaderText="LCL(CBM)" SortExpression="LCLVolume" />
                                    <asp:BoundField DataField="NoOfPackages" HeaderText="Pkgs" SortExpression="NoOfPackages" />
                                    <asp:BoundField DataField="GrossWeight" HeaderText="Gross WT" SortExpression="GrossWeight" />
                                    <asp:BoundField DataField="ChargeableWeight" HeaderText="Charge WT" SortExpression="ChargeableWeight" />
                                    <asp:BoundField DataField="ModeName" HeaderText="Mode" SortExpression="ModeName" />
                                    <asp:BoundField DataField="TermsName" HeaderText="Terms" SortExpression="TermsName" />
                                    <asp:BoundField DataField="TypeName" HeaderText="Type" SortExpression="TypeName" />
                                    <asp:BoundField DataField="CountryName" HeaderText="Country" SortExpression="CountryName" />
                                    <asp:BoundField DataField="LoadingPortName" HeaderText="Loading Port" SortExpression="LoadingPortName" />
                                    <asp:BoundField DataField="PortOfDischargedName" HeaderText="Port of Discharged"
                                        SortExpression="PortOfDischargedName" />
                                    <asp:BoundField DataField="SalesRepName" HeaderText="Sales Rep" SortExpression="SalesRepName" />
                                    <asp:BoundField DataField="StatusRemarks" HeaderText="Remark" SortExpression="StatusRemarks" />
                                </Columns>
                            </asp:GridView>--%>
                            <asp:GridView ID="gvSummaryStatusDetail" runat="server" CssClass="table" AutoGenerateColumns="false"
                                PagerStyle-CssClass="pgr" DataKeyNames="lid" AllowPaging="True" AllowSorting="True" PageSize="20"
                                PagerSettings-Position="TopAndBottom" OnRowCommand="gvSummaryStatusDetail_RowCommand" Width="99%"
                                OnSorting="gvSummaryStatusDetail_Sorting" OnPageIndexChanging="gvSummaryStatusDetail_PageIndexChanging"
                                DataSourceID="DataSourceSummaryDetail" OnRowDataBound="gvSummaryStatusDetail_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quotation Ref No" SortExpression="QuoteRefNo">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEditQuotation" CommandName="GetQuote" ToolTip="Edit Quotation" runat="server"
                                                Text='<%#Eval("QuoteRefNo") %>' CommandArgument='<%#Eval("lid") + ";" + Eval("ApprovalStatusId") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="QuoteRefNo" HeaderText="Quotation Ref No" SortExpression="QuoteRefNo" ReadOnly="true" Visible="false" />
                                    <asp:BoundField DataField="CustomerName" HeaderText="Customer" SortExpression="CustomerName" ReadOnly="true" />
                                    <asp:BoundField DataField="QuotedFormat" HeaderText="Quotation Type" SortExpression="QuotedFormat" ReadOnly="true" />
                                    <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category" ReadOnly="true" />
                                    <asp:BoundField DataField="Mode" HeaderText="Mode" SortExpression="Mode" ReadOnly="true" />
                                    <asp:BoundField DataField="CreatedBy" HeaderText="Quoted By" SortExpression="CreatedBy" ReadOnly="true" />
                                    <asp:BoundField DataField="CreatedDate" HeaderText="Quoted On" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                                    <asp:BoundField DataField="ModifiedBy" HeaderText="Modified By" SortExpression="ModifiedBy" ReadOnly="true" />
                                    <asp:BoundField DataField="ModifiedDate" HeaderText="Modified On" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                                    <asp:BoundField DataField="ApprovalStatus" HeaderText="Status" SortExpression="ApprovalStatus" ReadOnly="true" Visible="false" />
                                    <asp:TemplateField HeaderText="Status" SortExpression="ApprovalStatus">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lblStatus" runat="server" Text='<%# Eval("ApprovalStatus") %>' CommandArgument='<%#Eval("lid") %>'
                                                CommandName="getstatus" ToolTip="Update Status For Quotation."></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" SortExpression="">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnDownloadQuote" runat="server" CausesValidation="false" CommandName="DownloadQuote"
                                                ImageAlign="AbsMiddle" ImageUrl="~/Images/pdf2.png" Width="16" Height="16" ToolTip="Download Quotation in PDF Format."
                                                CommandArgument='<%#Eval("QuotePath") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                    <div>
                        <asp:SqlDataSource ID="DataSourceSummaryDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="FR_DSSummaryFreightUserDetail" SelectCommandType="StoredProcedure"
                            OnSelected="DataSourceSummaryDetail_Selected">
                            <SelectParameters>
                                <asp:Parameter Name="lId" DefaultValue="0" />
                                <asp:ControlParameter ControlID="ddPendingQuotation" Name="ReportType" PropertyName="SelectedValue" />
                                <asp:ControlParameter ControlID="ddMonth" Name="MonthId" PropertyName="SelectedValue" />
                                <asp:Parameter Name="StatusId" DefaultValue="1" />
                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                    <div>
                        <asp:HiddenField ID="lnkDummySummary" runat="server"></asp:HiddenField>
                    </div>
                </div>
            </div>
            <!--Document for Doc Upload-->
            <div id="divDocument">
                <cc1:ModalPopupExtender ID="ModalPopupDocument" runat="server" CacheDynamicResults="false"
                    PopupControlID="Panel2Document" TargetControlID="lnkDummy" BackgroundCssClass="modalBackground" DropShadow="true">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="Panel2Document" runat="server" CssClass="ModalPopupPanel" Width="500px">
                    <div class="header">
                        <div class="fleft">
                            Update Status of Quotation
                        </div>
                        <div class="fright">
                            <asp:ImageButton ID="ImageButton1" ImageUrl="~/Images/delete.gif" runat="server" OnClick="btnCancelPopup_Click"
                                ToolTip="Close" />
                        </div>
                    </div>
                    <div class="m">
                    </div>

                    <div id="Div2" runat="server" style="max-height: 200px; overflow: auto; padding: 5px">
                        <asp:HiddenField ID="hdnQuotationId" runat="server" />
                        <div align="center">
                            <asp:Label ID="lbError_Popup" runat="server" Visible="true"></asp:Label>
                        </div>
                        <div>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>Select Status
                                      <asp:RequiredFieldValidator ID="rfvstatus" runat="server" ControlToValidate="ddlStatus" Display="Dynamic" SetFocusOnError="true"
                                          ErrorMessage="Please Select Status." Text="*" ValidationGroup="vgAddStatus" InitialValue="0" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlStatus" runat="server" AppendDataBoundItems="true" DataSourceID="DataSourceStatus"
                                            DataTextField="sName" DataValueField="lid" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="0" Text="-Select-"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Remark (if any)
                                      <asp:RequiredFieldValidator ID="rfvrem" runat="server" ControlToValidate="txtRemark" SetFocusOnError="true" Display="Dynamic"
                                          ErrorMessage="Enter Remark." Text="*" ValidationGroup="vgAddStatus"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Rows="3" Width="250px">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_OnClick" CausesValidation="true" ValidationGroup="vgAddStatus"
                                            Text="Save Status" />
                                    </td>
                                </tr>
                            </table>
                            <asp:GridView ID="gvStatusHistory" runat="server" CssClass="table" PagerStyle-CssClass="pgr"
                                AllowSorting="true" AutoGenerateColumns="false" AllowPaging="true" DataSourceID="DataSourceStatusHistory"
                                PageSize="30" PagerSettings-Position="TopAndBottom" Style="white-space: normal"
                                DataKeyNames="lid" Width="98%">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Status" DataField="StatusName" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Remark" DataField="Remark" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Modified By" DataField="ModifiedBy" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Modified Date" DataField="ModifiedDate" ReadOnly="true" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div>
                <asp:HiddenField ID="lnkDummy" runat="server"></asp:HiddenField>
                <asp:SqlDataSource ID="DataSourceStatusHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="BS_GetQuotationStatusHistory" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="QuotationId" SessionField="QuotationId" DbType="Int32" />
                        <asp:SessionParameter Name="UserId" SessionField="UserId" DbType="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceStatus" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="BS_GetQuotationStatus" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </div>
            <!--Document for Doc Upload - END -->
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

