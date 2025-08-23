<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FreightDashboard.aspx.cs" Inherits="Freight_FreightDashboard"
    MasterPageFile="~/MasterPage.master" Title="Freight Dashboard" EnableEventValidation="false" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <script src="../JS/jquery-3.1.0.min.js" type="text/javascript"></script>
    <script src="../JS/toastr/toastr.min.js" type="text/javascript"></script>
    <link href="../JS/toastr/toastr.min.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": false,
            "progressBar": false,
            "positionClass": "toast-bottom-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "0",
            "hideDuration": "0",
            "timeOut": "0",
            "extendedTimeOut": "0",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };

//        toastr.info("Agent Information Updation Pending! Please Provide Details", "Information");
    </script>
    <style type="text/css">
        div#ctl00_ContentPlaceHolder1_pnlChatPopup {
            position: relative;
            box-shadow: rgb(0, 0, 0) 5px 5px 5px;
            border: 2px solid #46b8da;
            border-radius: 3px;
        }

        li.media {
            display: block;
        }

        .panel-footer {
            padding: 10px 15px;
            background-color: rgba(213, 213, 213, 0.6);
            border-top: 1px solid #ddd;
            border-bottom-right-radius: 3px;
            border-bottom-left-radius: 3px;
        }

        .input-group {
            position: relative;
            display: table;
            border-collapse: separate;
        }
    </style>
    <div>
        <%--<div id="toast"></div>--%>

        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPanel" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPanel" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div style="float: left; width: 43%;">
                <!--Freight Month Summary -->
                <fieldset>
                    <legend>Freight Summary</legend>
                    <div>
                        <b>Summary Type</b>
                        <asp:DropDownList ID="ddPendingFreight" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddPendingFreight_SelectedIndexChanged">
                            <asp:ListItem Text="Pending Freight" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Overall Summary" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;
                <asp:LinkButton ID="lnkTotalSummaryExport" runat="server" OnClick="lnkTotalSummaryExport_Click">
                    <asp:Image ID="Image3" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
                    </div>
                    <div>

                        <asp:GridView ID="gvSummaryMonth" runat="server" CssClass="table" DataSourceID="DataSourceFreightMonth"
                            Width="99%" AutoGenerateColumns="false" OnRowCommand="gvSummaryMonth_RowCommand">
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
                                        <asp:LinkButton ID="lnkApr" runat="server" Text='<%#Eval("Apr") %>' CommandName="MonthDetail" CommandArgument='<%#Eval("StatusId")+";4" %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="May">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkMay" runat="server" Text='<%#Eval("May") %>' CommandName="MonthDetail" CommandArgument='<%#Eval("StatusId")+";5" %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Jun">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkJun" runat="server" Text='<%#Eval("Jun") %>' CommandName="MonthDetail" CommandArgument='<%#Eval("StatusId")+";6" %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Jul">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkJul" runat="server" Text='<%#Eval("Jul") %>' CommandName="MonthDetail" CommandArgument='<%#Eval("StatusId")+";7" %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Aug">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkAug" runat="server" Text='<%#Eval("Aug") %>' CommandName="MonthDetail" CommandArgument='<%#Eval("StatusId")+";8" %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sep">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkSep" runat="server" Text='<%#Eval("Sep") %>' CommandName="MonthDetail" CommandArgument='<%#Eval("StatusId")+";9" %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Oct">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkOct" runat="server" Text='<%#Eval("Oct") %>' CommandName="MonthDetail" CommandArgument='<%#Eval("StatusId")+";10" %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Nov">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkNov" runat="server" Text='<%#Eval("Nov") %>' CommandName="MonthDetail" CommandArgument='<%#Eval("StatusId")+";11" %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dec">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDec" runat="server" Text='<%#Eval("Dec") %>' CommandName="MonthDetail" CommandArgument='<%#Eval("StatusId")+";12" %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Jan">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkJan" runat="server" Text='<%#Eval("Jan") %>' CommandName="MonthDetail" CommandArgument='<%#Eval("StatusId")+";1" %>'> </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Feb">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkFeb" runat="server" Text='<%#Eval("Feb") %>' CommandName="MonthDetail" CommandArgument='<%#Eval("StatusId")+";2" %>'> </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mar">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkMar" runat="server" Text='<%#Eval("Mar") %>' CommandName="MonthDetail" CommandArgument='<%#Eval("StatusId")+";3" %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </fieldset>

                <div>
                    <asp:SqlDataSource ID="DataSourceFreightMonth" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="FR_DSSummaryMonth" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:SessionParameter Name="UserId" SessionField="UserId" />
                            <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                            <asp:ControlParameter ControlID="ddPendingFreight" Name="ReportType" PropertyName="SelectedValue" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
                <!--Freight Summary -->
                <fieldset>
                    <legend runat="server" id="legendName">Freight User Summary Status</legend>
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
                            <asp:ListItem Text="Freight User" Value="1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Freight Mode" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Freight Type" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Freight Customer" Value="4"></asp:ListItem>
                            <asp:ListItem Text="Sales Representative" Value="5"></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;<asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                            <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                        </asp:LinkButton>
                    </div>
                    <div>
                        <asp:GridView ID="gvSummaryFreight" runat="server" CssClass="table" Width="99%" AutoGenerateColumns="false"
                            AllowSorting="true" AllowPaging="true" PageSize="20" PagerStyle-CssClass="pgr" PagerSettings-Position="TopAndBottom"
                            DataSourceID="DataSourceFreightSummary" OnSorting="gvSummaryFreight_Sorting"
                            OnRowCommand="gvSummaryFreight_RowCommand" OnPageIndexChanging="gvSummaryFreight_PageIndexChanging">
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
                                <asp:TemplateField HeaderText="Enquiry" SortExpression="Enquiry">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEnquiry" runat="server" Text='<%#Eval("Enquiry") %>' CommandName="SummaryDetail" CommandArgument='<%#Eval("lId")+";1" %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quoted" SortExpression="Quoted">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkQuoted" runat="server" Text='<%#Eval("Quoted") %>' CommandName="SummaryDetail" CommandArgument='<%#Eval("lId")+";2" %>'> </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Awarded" SortExpression="Awarded">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkAwarded" runat="server" Text='<%#Eval("Awarded") %>' CommandName="SummaryDetail" CommandArgument='<%#Eval("lId")+";3" %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lost" SortExpression="Lost">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkLost" runat="server" Text='<%#Eval("Lost") %>' CommandName="SummaryDetail" CommandArgument='<%#Eval("lId")+";4" %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Executed" SortExpression="Executed">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkExecuted" runat="server" Text='<%#Eval("Executed") %>' CommandName="SummaryDetail" CommandArgument='<%#Eval("lId")+";20"%>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Budgetary" SortExpression="Budgetary">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBudgetary" runat="server" Text='<%#Eval("Budgetary") %>' CommandName="SummaryDetail" CommandArgument='<%#Eval("lId")+";6"%>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lead" SortExpression="Lead">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkLead" runat="server" Text='<%#Eval("Lead") %>' CommandName="SummaryDetail" CommandArgument='<%#Eval("lId")+";7"%>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </fieldset>
                <div>
                    <asp:SqlDataSource ID="DataSourceFreightSummary" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="FR_DSSummaryFreightUser" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddPendingFreight" Name="ReportType" PropertyName="SelectedValue" />
                            <asp:ControlParameter ControlID="ddMonth" Name="MonthId" PropertyName="SelectedValue" />
                            <asp:SessionParameter Name="UserId" SessionField="UserId" />
                            <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>

                <!--Popup Month-Status Detail  -->

                <div id="divPopupMonthDetail">
                    <cc1:modalpopupextender id="ModalPopupMonthStatus" runat="server" cachedynamicresults="false"
                        dropshadow="False" popupcontrolid="Panel2Month" targetcontrolid="lnkDummyMonth">
                </cc1:modalpopupextender>
                    <asp:Panel ID="Panel2Month" runat="server" CssClass="ModalPopupPanel" Style="display: none">
                        <div class="header">
                            <div class="fleft">
                                &nbsp;<asp:LinkButton ID="lnkExportSummary" runat="server" OnClick="lnkExportSummary_Click">
                                    <asp:Image ID="imgExpSummary" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                                </asp:LinkButton>
                            </div>
                            <div class="fright">
                                <asp:Button ID="btnCancelPopup" runat="server" OnClick="btnCancelMonthPopup_Click" Text="Close"
                                    CausesValidation="false" class="no" ToolTip="Close Window" />
                            </div>
                        </div>
                        <!--Freight Detail Start-->
                        <div id="Div1" runat="server" style="max-height: 600px; max-width: 900px; overflow: auto;">
                            <asp:GridView ID="gvMonthStatusDetail" runat="server" CssClass="table" Width="99%" AutoGenerateColumns="false"
                                DataSourceID="DataSourceMonthDetail" AllowSorting="true" AllowPaging="true" PageSize="20" PagerStyle-CssClass="pgr"
                                OnSorting="gvMonthStatusDetail_Sorting" OnPageIndexChanging="gvMonthStatusDetail_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ENQRefNo" HeaderText="ENQ No" SortExpression="ENQRefNo" />
                                    <asp:BoundField DataField="ENQDate" HeaderText="ENQ Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ENQDate" />
                                    <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName" />
                                    <asp:BoundField DataField="CurrentStatus" HeaderText="CurrentStatus" SortExpression="CurrentStatus" />
                                    <asp:BoundField DataField="StatusDate" HeaderText="Status Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="StatusDate" />
                                    <asp:BoundField DataField="EnquiryUser" HeaderText="Freight SPC" SortExpression="EnquiryUser" />
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
                                    <asp:BoundField DataField="PortOfDischargedName" HeaderText="Port of Discharged" SortExpression="PortOfDischargedName" />
                                    <asp:BoundField DataField="SalesRepName" HeaderText="Sales Rep" SortExpression="SalesRepName" />
                                    <asp:BoundField DataField="StatusRemarks" HeaderText="Remark" SortExpression="StatusRemarks" />
                                    <asp:BoundField DataField="LostStatus" HeaderText="Lost Reason" SortExpression="LostStatus" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>

                    <div>
                        <asp:SqlDataSource ID="DataSourceMonthDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="FR_DSMonthFreightDetail" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddPendingFreight" Name="ReportType" PropertyName="SelectedValue" />
                                <asp:Parameter Name="MonthId" DefaultValue="2" />
                                <asp:Parameter Name="StatusId" DefaultValue="1" />
                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                    <div>
                        <asp:LinkButton ID="lnkDummyMonth" runat="server" Text=""></asp:LinkButton>
                    </div>
                </div>
                <!--Popup Freight Summary Detail  -->

                <div id="divPopupSummaryDetail">
                    <cc1:modalpopupextender id="ModalPopupSummaryDetail" runat="server" cachedynamicresults="false"
                        dropshadow="False" popupcontrolid="Panel2Summary" targetcontrolid="lnkDummySummary">
                </cc1:modalpopupextender>
                    <asp:Panel ID="Panel2Summary" runat="server" CssClass="ModalPopupPanel">
                        <div class="header">
                            <div class="fleft">
                                &nbsp;<asp:Button ID="btnCancelSummaryPopup" runat="server" OnClick="btnCancelSummaryPopup_Click" Text="Close" CausesValidation="false" />
                            </div>
                            &nbsp;&nbsp;<asp:LinkButton ID="lnkExportSummaryDetail" runat="server" OnClick="lnkExportSummaryDetail_Click">
                                <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>
                            <div class="fleft">
                                &nbsp;<asp:Label ID="lblSummaryStatus" Text="" runat="server" />
                            </div>
                            <div class="fright">
                                <asp:ImageButton ID="imgCancelSummaryPopup" ImageUrl="~/Images/delete.gif" OnClick="btnCancelSummaryPopup_Click" runat="server" ToolTip="Close" />
                            </div>
                        </div>
                        <!--Freight Detail Start-->
                        <div id="Div3" runat="server" style="max-height: 550px; max-width: 900px; overflow: auto;">
                            <asp:GridView ID="gvSummaryStatusDetail" runat="server" CssClass="table" Width="98%" AutoGenerateColumns="false"
                                DataSourceID="DataSourceSummaryDetail" AllowSorting="true" AllowPaging="false" PageSize="20" PagerStyle-CssClass="pgr"
                                OnSorting="gvSummaryStatusDetail_Sorting" OnPageIndexChanging="gvSummaryStatusDetail_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ENQRefNo" HeaderText="ENQ No" SortExpression="ENQRefNo" />
                                    <asp:BoundField DataField="FRJobNo" HeaderText="Job No" SortExpression="FRJobNo" />
                                    <asp:BoundField DataField="ENQDate" HeaderText="ENQ Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ENQDate" />
                                    <asp:BoundField DataField="EnquiryUser" HeaderText="Freight SPC" SortExpression="EnquiryUser" />
                                    <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName" />
                                    <asp:BoundField DataField="StatusDate" HeaderText="Status Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="StatusDate" />
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
                                    <asp:BoundField DataField="PortOfDischargedName" HeaderText="Port of Discharged" SortExpression="PortOfDischargedName" />
                                    <asp:BoundField DataField="SalesRepName" HeaderText="Sales Rep" SortExpression="SalesRepName" />
                                    <asp:BoundField DataField="StatusRemarks" HeaderText="Remark" SortExpression="StatusRemarks" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                    <div>
                        <asp:SqlDataSource ID="DataSourceSummaryDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="FR_DSSummaryFreightUserDetail" SelectCommandType="StoredProcedure" OnSelected="DataSourceSummaryDetail_Selected">
                            <SelectParameters>
                                <asp:Parameter Name="lId" DefaultValue="0" />
                                <asp:ControlParameter ControlID="ddPendingFreight" Name="ReportType" PropertyName="SelectedValue" />
                                <asp:ControlParameter ControlID="ddMonth" Name="MonthId" PropertyName="SelectedValue" />
                                <asp:Parameter Name="StatusId" DefaultValue="1" />
                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                    <div>
                        <asp:LinkButton ID="lnkDummySummary" runat="server" Text=""></asp:LinkButton>
                    </div>
                </div>

                <fieldset>
                    <legend>CBM Calculator</legend>
                    <div>
                        <select id="unit" onchange="cal_cbm();" style="width: 100px;">
                            <option value="cm">Centimeter</option>
                            <option value="mm">Millimeter</option>
                            <option value="m">Meter</option>
                            <option value="in">Inch</option>
                            <option value="ft">Foot</option>
                            <option value="yd">Yard</option>
                        </select>
                        <input id="txtPkgs" placeholder="No Of Pkgs" onchange="cal_cbm();" onkeyup="cal_cbm();" value="1">Pkgs
                    <input id="l" placeholder="Length" onchange="cal_cbm();" onkeyup="cal_cbm();">L
                    <input id="w" placeholder="Width" onchange="cal_cbm();" onkeyup="cal_cbm();">W 
                    <input id="h" placeholder="Height" onchange="cal_cbm();" onkeyup="cal_cbm();">H
                    
                    <input id="cbm" placeholder="CBM" readonly>CBM
                    </div>
                    <p id="divMsg"></p>

                </fieldset>
                <fieldset>
                    <legend>Chargeable Weight Calculator</legend>
                    <div>
                        <select id="ddUnitW" onchange="cal_cbm();" style="width: 100px;">
                            <option value="cmW">Centimeter</option>
                            <option value="mmW">Millimeter</option>
                            <option value="mW">Meter</option>
                            <option value="inW">Inch</option>
                            <option value="ftW">Foot</option>
                            <option value="ydW">Yard</option>
                        </select>
                        <input id="txtPkgW" placeholder="No Of Pkgs" onchange="cal_ChargeWeight();" onkeyup="cal_ChargeWeight();" value="1">Pkgs
                    <input id="txtLenW" placeholder="Length" onchange="cal_ChargeWeight();" onkeyup="cal_ChargeWeight();">L
                    <input id="txtWidthW" placeholder="Width" onchange="cal_ChargeWeight();" onkeyup="cal_ChargeWeight();">W 
                    <input id="txtHeighW" placeholder="Height" onchange="cal_ChargeWeight();" onkeyup="cal_ChargeWeight();">H
                    
                    <input id="txtChargeW" placeholder="Charge Weight" readonly>CW
                    </div>
                    <p id="divChargeWeight"></p>
                </fieldset>

                <script type="text/javascript">
                    var ocbm = document.getElementById("cbm");
                    var ol = document.getElementById("l");
                    var ow = document.getElementById("w");
                    var oh = document.getElementById("h");
                    var ounit = document.getElementById("unit");
                    var opkg = document.getElementById("txtPkgs");
                    function cal_cbm() {

                        var l = ol.value.trim();

                        var w = ow.value.trim();
                        var h = oh.value.trim();
                        var unit = ounit.value;
                        var p = opkg.value;
                        if (l.indexOf("/") != -1) { l = Fraction2Decimal(l); }
                        if (w.indexOf("/") != -1) { w = Fraction2Decimal(w); }
                        if (h.indexOf("/") != -1) { h = Fraction2Decimal(h); }

                        if ((l == '') || (w == '') || (h == '') || isNaN(l) || isNaN(w) || isNaN(h)) {
                            ocbm.value = '';

                        } else {
                            if (unit == 'in') { l = l * 2.54; w = w * 2.54; h = h * 2.54; precision = 100; }
                            else if (unit == 'ft') { l = l * 30.48; w = w * 30.48; h = h * 30.48; precision = 10; }
                            else if (unit == 'yd') { l = l * 91.44; w = w * 91.44; h = h * 91.44; precision = 10; }
                            else if (unit == 'mm') { l = l * 0.1; w = w * 0.1; h = h * 0.1; precision = 1000; }
                            else if (unit == 'cm') { precision = 100; }
                            else if (unit == 'm') { l = l * 100; w = w * 100; h = h * 100; precision = 10; }
                            cbm = Math.round(l * w * h / 1000000 * precision) / precision;
                            ocbm.value = cbm * p;

                        }

                    }
                </script>

                <script type="text/javascript">
                    var ocbmW = document.getElementById("txtChargeW");
                    var olW = document.getElementById("txtLenW");
                    var owW = document.getElementById("txtWidthW");
                    var ohW = document.getElementById("txtHeighW");
                    var ounitW = document.getElementById("ddUnitW");
                    var opkgW = document.getElementById("txtPkgW");

                    function cal_ChargeWeight() {

                        var lw = olW.value.trim();
                        var ww = owW.value.trim();
                        var hw = ohW.value.trim();
                        var unitw = ounitW.value;
                        var pw = opkgW.value;

                        if (lw.indexOf("/") != -1) { lw = Fraction2Decimal(lw); }
                        if (ww.indexOf("/") != -1) { ww = Fraction2Decimal(ww); }
                        if (hw.indexOf("/") != -1) { hw = Fraction2Decimal(hw); }

                        if ((lw == '') || (ww == '') || (hw == '') || isNaN(lw) || isNaN(ww) || isNaN(hw)) {

                            ocbmW.value = '';
                        }

                        else {

                            if (unitw == 'inW') { lw = lw * 2.54; ww = ww * 2.54; hw = hw * 2.54; precision = 100; }
                            else if (unitw == 'ftW') { lw = lw * 30.48; ww = ww * 30.48; hw = hw * 30.48; precision = 10; }
                            else if (unitw == 'ydW') { lw = lw * 91.44; ww = ww * 91.44; hw = hw * 91.44; precision = 10; }
                            else if (unitw == 'mmW') { lw = lw * 0.1; ww = ww * 0.1; hw = hw * 0.1; precision = 1000; }
                            else if (unitw == 'cmW') { precision = 100; }
                            else if (unitw == 'mW') { lw = lw * 100; ww = ww * 100; hw = hw * 100; precision = 10; }

                            cbmW = Math.round(lw * ww * hw / 6000 * precision) / precision;

                            ocbmW.value = cbmW * pw;
                        }
                    }
                </script>
            </div>

            <div style="float: left; margin-left: 20px; width: 49%;">
                <fieldset>
                    <legend>Import</legend>
                    <legend>Ongoing Shipment – Current Job</legend>
                    <div>
                    <asp:LinkButton ID="lnkOlShipment" runat="server" OnClick="lnkOlShipment_Click">
                        <asp:Image ID="Image4" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                    </div>
                    <div>
                 
                    <asp:GridView ID="gvOlShipment" runat="server" CssClass="table" DataSourceID="DataSourceOlShipment" AllowPaging="true" PageSize="5"
                        Width="99%" AutoGenerateColumns="false" OnRowCommand="gvSummaryMonth_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FRJobNo" HeaderText="Job No" SortExpression="FRJobNo" />
                            <asp:TemplateField HeaderText="Customer" SortExpression="par_name">
                                <ItemTemplate>
                                    <div style="word-wrap: break-word; width: 120px; white-space:normal;">
                                        <asp:Label ID="lblCust" runat="server" Text='<%#Bind("par_name") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                <ItemTemplate>
                                    <div style="word-wrap: break-word; width: 70px; white-space:normal;">
                                        <asp:Label ID="lblStatus" runat="server" Text='<%#Bind("Status") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Freight SPC" SortExpression="EnquiryUser">
                                <ItemTemplate>
                                    <div style="word-wrap: break-word; width: 50px; white-space:normal;">
                                        <asp:Label ID="lblEnquiryUser" runat="server" Text='<%#Bind("EnquiryUser") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ETA" HeaderText="ETA" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ETA"/>
                            <asp:BoundField DataField="CANDate" HeaderText="CAN Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="CANDate"/>
                            <asp:BoundField DataField="DOCreatedDate" HeaderText="DO Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="DOCreatedDate"/>
                            <asp:BoundField DataField="amount" HeaderText="amount" SortExpression="amount"/>
                        </Columns>
                    </asp:GridView>
                    </div>

                    <legend>Current Job Amount More Than 1 Lac</legend>
                    <div>
                    <asp:LinkButton ID="lnkCurrentJob1Lac" runat="server" OnClick="lnkCurrentJob1Lac_Click">
                        <asp:Image ID="Image5" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                    </div>
                    <div>
                 
                    <asp:GridView ID="gvCurrentJob" runat="server" CssClass="table" DataSourceID="DataSourceOlShipmentAmtGreaterThan1Lac" AllowPaging="true" PageSize="5"
                        Width="99%" AutoGenerateColumns="false" OnRowCommand="gvSummaryMonth_RowCommand">
                        <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="FRJobNo" HeaderText="Job No" SortExpression="FRJobNo" />
                        <asp:TemplateField HeaderText="Customer" SortExpression="par_name">
                            <ItemTemplate>
                                <div style="word-wrap: break-word; width: 120px; white-space:normal;">
                                    <asp:Label ID="lblCust" runat="server" Text='<%#Bind("par_name") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status" SortExpression="Status">
                            <ItemTemplate>
                                <div style="word-wrap: break-word; width: 70px; white-space:normal;">
                                    <asp:Label ID="lblStatus" runat="server" Text='<%#Bind("Status") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Freight SPC" SortExpression="EnquiryUser">
                            <ItemTemplate>
                                <div style="word-wrap: break-word; width: 50px; white-space:normal;">
                                    <asp:Label ID="lblEnquiryUser" runat="server" Text='<%#Bind("EnquiryUser") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ETA" HeaderText="ETA" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ETA"/>
                        <asp:BoundField DataField="CANDate" HeaderText="CAN Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="CANDate"/>
                        <asp:BoundField DataField="DOCreatedDate" HeaderText="DO Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="DOCreatedDate"/>
                        <asp:BoundField DataField="amount" HeaderText="amount" SortExpression="amount"/>
                        </Columns>
                    </asp:GridView>
                    </div>

                    <legend>Shipment Completed – Pending For Billing</legend>
                    <div>
                    <asp:LinkButton ID="lnkShipmentComplete" runat="server" OnClick="lnkShipmentComplete_Click">
                        <asp:Image ID="Image6" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                    </div>
                    <div>
                 
                     <asp:GridView ID="gvShipmentCleared" runat="server" CssClass="table" DataSourceID="DataSourceShipmentCleared" PageSize="5" AllowPaging="true"
                        Width="99%" AutoGenerateColumns="false" OnRowCommand="gvSummaryMonth_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FRJobNo" HeaderText="Job No" SortExpression="FRJobNo" />
                            <asp:TemplateField HeaderText="Customer" SortExpression="par_name">
                                <ItemTemplate>
                                    <div style="word-wrap: break-word; width: 120px; white-space:normal;">
                                        <asp:Label ID="lblCust2" runat="server" Text='<%#Bind("par_name") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                <ItemTemplate>
                                    <div style="word-wrap: break-word; width: 70px; white-space:normal;">
                                        <asp:Label ID="lblStatus2" runat="server" Text='<%#Bind("Status") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Freight SPC" SortExpression="EnquiryUser">
                                <ItemTemplate>
                                    <div style="word-wrap: break-word; width: 50px; white-space:normal;">
                                        <asp:Label ID="lblEnquiryUser2" runat="server" Text='<%#Bind("EnquiryUser") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ETA" HeaderText="ETA" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ETA"/>
                            <asp:BoundField DataField="CANDate" HeaderText="CAN Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="CANDate"/>
                            <asp:BoundField DataField="DOCreatedDate" HeaderText="DO Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="DOCreatedDate"/>
                            <asp:BoundField DataField="amount" HeaderText="amount" SortExpression="amount"/>
                        </Columns>
                    </asp:GridView>
                    </div>

                    <div>
                        <asp:SqlDataSource ID="DataSourceOlShipment" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="FOP_GetOlShipment" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                    <div>
                        <asp:SqlDataSource ID="DataSourceOlShipmentAmtGreaterThan1Lac" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="FOP_GetOlShipmentAmtGreaterThan1Lac" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                    <div>
                         <asp:SqlDataSource ID="DataSourceShipmentCleared" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="FOP_BJVGetShipmentCleared" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </fieldset>

                <fieldset>
                    <legend>Export</legend>
                    <legend>Ongoing Shipment – Current Job</legend>
                    <div>
                    <asp:LinkButton ID="lnkExOlShipment" runat="server" OnClick="lnkExOlShipment_Click">
                        <asp:Image ID="Image7" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                    </div>
                    <div>
                 
                    <asp:GridView ID="gvExOlShipment" runat="server" CssClass="table" DataSourceID="DataSourceOlShipmentForExport" AllowPaging="true" PageSize="5"
                        Width="99%" AutoGenerateColumns="false" OnRowCommand="gvSummaryMonth_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FRJobNo" HeaderText="Job No" SortExpression="FRJobNo" />
                            <asp:TemplateField HeaderText="Customer" SortExpression="par_name">
                                <ItemTemplate>
                                    <div style="word-wrap: break-word; width: 120px; white-space:normal;">
                                        <asp:Label ID="lblCust2" runat="server" Text='<%#Bind("par_name") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                <ItemTemplate>
                                    <div style="word-wrap: break-word; width: 70px; white-space:normal;">
                                        <asp:Label ID="lblStatus2" runat="server" Text='<%#Bind("Status") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SPC Name" SortExpression="EnquiryUser">
                                <ItemTemplate>
                                    <div style="word-wrap: break-word; width: 50px; white-space:normal;">
                                        <asp:Label ID="lblEnquiryUser2" runat="server" Text='<%#Bind("EnquiryUser") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="SBDate" HeaderText="SB Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="SBDate"/>
                            <asp:BoundField DataField="VGMDate" HeaderText="VGM Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="VGMDate"/>
                            <asp:BoundField DataField="MBLDate" HeaderText="MBL Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="MBLDate"/>
                            <asp:BoundField DataField="ShipOnboardDate" HeaderText="Ship Onboard Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ShipOnboardDate"
                                 ItemStyle-Width="30px" HeaderStyle-Width="30px"/>
                            <asp:BoundField DataField="amount" HeaderText="amount" SortExpression="amount"/>
                        </Columns>
                    </asp:GridView>
                    </div>

                    <legend>Current Job Amount More Than 1 Lac</legend>
                    <div>
                    <asp:LinkButton ID="lnkCurrentJob1LacForExport" runat="server" OnClick="lnkCurrentJob1LacForExport_Click">
                        <asp:Image ID="Image8" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                    </div>
                    <div>
                 
                    <asp:GridView ID="gvCurrentJobForExport" runat="server" CssClass="table" DataSourceID="DataSourceOlShipmentAmtGreaterThan1LacForExport" AllowPaging="true" PageSize="5"
                        Width="99%" AutoGenerateColumns="false" >
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FRJobNo" HeaderText="Job No" SortExpression="FRJobNo" />
                            <asp:TemplateField HeaderText="Customer" SortExpression="par_name">
                                <ItemTemplate>
                                    <div style="word-wrap: break-word; width: 120px; white-space:normal;">
                                        <asp:Label ID="lblCust2" runat="server" Text='<%#Bind("par_name") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                <ItemTemplate>
                                    <div style="word-wrap: break-word; width: 70px; white-space:normal;">
                                        <asp:Label ID="lblStatus2" runat="server" Text='<%#Bind("Status") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SPC Name" SortExpression="EnquiryUser">
                                <ItemTemplate>
                                    <div style="word-wrap: break-word; width: 50px; white-space:normal;">
                                        <asp:Label ID="lblEnquiryUser2" runat="server" Text='<%#Bind("EnquiryUser") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="SBDate" HeaderText="SB Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="SBDate"/>
                            <asp:BoundField DataField="VGMDate" HeaderText="VGM Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="VGMDate"/>
                            <asp:BoundField DataField="MBLDate" HeaderText="MBL Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="MBLDate"/>
                            <asp:BoundField DataField="ShipOnboardDate" HeaderText="Ship Onboard Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ShipOnboardDate"
                                 ItemStyle-Width="30px" HeaderStyle-Width="30px"/>
                            <asp:BoundField DataField="amount" HeaderText="amount" SortExpression="amount"/>
                        </Columns>
                    </asp:GridView>
                    </div>

                    <legend>Shipment Completed – Pending For Billing</legend>
                    <div>
                    <asp:LinkButton ID="lnkShipmentCompleteForexport" runat="server" OnClick="lnkShipmentCompleteForexport_Click">
                        <asp:Image ID="Image9" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                    </div>
                    <div>
                 <%-- DataSourceID="DataSourceShipmentClearedForExport" --%>
                     <asp:GridView ID="gvShipmentClearedForExport" runat="server" CssClass="table" PageSize="5" AllowPaging="true"
                        Width="99%" AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FRJobNo" HeaderText="Job No" SortExpression="FRJobNo" />
                            <asp:TemplateField HeaderText="Customer" SortExpression="par_name">
                                <ItemTemplate>
                                    <div style="word-wrap: break-word; width: 120px; white-space:normal;">
                                        <asp:Label ID="lblCust2" runat="server" Text='<%#Bind("par_name") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                <ItemTemplate>
                                    <div style="word-wrap: break-word; width: 70px; white-space:normal;">
                                        <asp:Label ID="lblStatus2" runat="server" Text='<%#Bind("Status") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SPC Name" SortExpression="EnquiryUser">
                                <ItemTemplate>
                                    <div style="word-wrap: break-word; width: 50px; white-space:normal;">
                                        <asp:Label ID="lblEnquiryUser2" runat="server" Text='<%#Bind("EnquiryUser") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="SBDate" HeaderText="SB Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="SBDate"/>
                            <asp:BoundField DataField="VGMDate" HeaderText="VGM Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="VGMDate"/>
                            <asp:BoundField DataField="MBLDate" HeaderText="MBL Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="MBLDate"/>
                            <asp:BoundField DataField="ShipOnboardDate" HeaderText="Ship On board Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ShipOnboardDate"
                                 ItemStyle-Width="30px" HeaderStyle-Width="30px"/>
                            <asp:BoundField DataField="amount" HeaderText="amount" SortExpression="amount"/>
                        </Columns>
                    </asp:GridView>
                    </div>

                    <div>
                        <asp:SqlDataSource ID="DataSourceOlShipmentForExport" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="FOP_GetOlShipmentForExport" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                    <div>
                        <asp:SqlDataSource ID="DataSourceOlShipmentAmtGreaterThan1LacForExport" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="FOP_GetOlShipmentAmtGreaterThan1LacForExport" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                    <div>
                         <asp:SqlDataSource ID="DataSourceShipmentClearedForExport" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="FOP_BJVGetShipmentClearedForExport" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </fieldset>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
