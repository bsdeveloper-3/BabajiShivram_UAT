<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs"
    Inherits="CRM_Dashboard" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <div>
        <asp:UpdateProgress ID="updProgress" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <style type="text/css">
        .WhatsNew_td {
            border: 1px solid navy;
            padding: 10px;
            font-size: 14px;
        }

        .modalBackground {
            background-color: black;
            filter: alpha(opacity=90);
            opacity: 0.5;
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

        .LeadCnt_td {
            background-color: #b6ff00;
            height: 25px;
            border: 2px solid #cccc;
            border-radius: 4px;
            text-align: center;
            font-size: 16px;
        }

        .td-img {
            height: 40px;
            width: 45px;
            border-width: 0px;
            border-radius: 7px;
        }
    </style>
    <script type="text/javascript">
        function OnCustomerSelected(source, eventArgs) {
            // alert(eventArgs.get_value());
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnCustId').value = results.CompanyId;
        }
        $addHandler
       (
           $get('ctl00_ContentPlaceHolder1_txtSearchCompany'), 'keyup',
           function () {
               $get('ctl00_ContentPlaceHolder1_hdnCustId').value = '0';
           }
       );
    </script>
    <asp:UpdatePanel ID="UpdatePanelDashboard" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <asp:HiddenField ID="hdnCustId" runat="server" Value="0" />
            <table border="0" cellspacing="20" width="100%" style="padding-right: 30px">
                <colgroup>
                    <col width="20%" />
                    <col width="20%" />
                    <col width="20%" />
                    <col width="20%" />
                    <col width="20%" />
                </colgroup>
                <tr>
                    <td class="LeadCnt_td" style="background-color: #00bd5d">
                        <table border="0" width="100%" style="padding: 5px">
                            <tr>
                                <td style="text-align: left">
                                    <asp:Image runat="server" CssClass="td-img" ImageUrl="~/Images/Lead.png" />
                                </td>
                                <td style="text-align: right">
                                    <asp:LinkButton ID="lnkbtnLead" runat="server" Font-Bold="true" Font-Underline="false" ForeColor="white" OnClick="lnkbtnLead_Click" Text="Lead"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="LeadCnt_td" style="background-color: #1c98ef">
                        <table border="0" width="100%" style="padding: 5px">
                            <tr>
                                <td style="text-align: left">
                                    <asp:Image runat="server" CssClass="td-img" ImageUrl="~/Images/LeadApproved.jpg" />
                                </td>
                                <td style="text-align: right">
                                    <asp:LinkButton ID="lnkbtnLeadApproved" runat="server" Font-Bold="true" Font-Underline="false" ForeColor="White" OnClick="lnkbtnLeadApproved_Click" Text="Lead Approved"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="LeadCnt_td" style="background-color: #ffb100">
                        <table border="0" width="100%" style="padding: 5px">
                            <tr>
                                <td style="text-align: left">
                                    <asp:Image runat="server" CssClass="td-img" ImageUrl="~/Images/Quote.png" />
                                </td>
                                <td style="text-align: right">
                                    <asp:LinkButton ID="lnkbtnQuote" runat="server" Font-Bold="true" Font-Underline="false" ForeColor="White" OnClick="lnkbtnQuote_Click" Text="Quote"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="LeadCnt_td" style="background-color: #6e7e1ab5">
                        <table border="0" width="100%" style="padding: 5px">
                            <tr>
                                <td style="text-align: left">
                                    <asp:Image runat="server" CssClass="td-img" ImageUrl="~/Images/ContractApproval.png" />
                                </td>
                                <td style="text-align: right">
                                    <asp:LinkButton ID="lnkbtnContractApproval" runat="server" Font-Bold="true" Font-Underline="false" ForeColor="White" OnClick="lnkbtnContractApproval_Click" Text="Contract Approval"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="LeadCnt_td" style="background-color: #ef391c"><%--125d48--%>
                        <table border="0" width="100%" style="padding: 5px">
                            <tr>
                                <td style="text-align: left">
                                    <asp:Image runat="server" CssClass="td-img" ImageUrl="~/Images/LeadRejected.png" />
                                </td>
                                <td style="text-align: right">
                                    <asp:LinkButton ID="lnkbtnRejected" runat="server" Font-Bold="true" Font-Underline="false" ForeColor="White" OnClick="lnkbtnRejected_Click" Text="Rejected"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <div style="text-align: center">
                <span style="color: Navy">-------------------------------------------------------------</span> &nbsp;&nbsp;
                <asp:DropDownList ID="ddlMonth" runat="server" TabIndex="1" Width="150px" ToolTip="Filter monthly user data" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                    <asp:ListItem Text="Financial Year" Selected="True" Value="0"></asp:ListItem>
                    <asp:ListItem Text="January" Value="1"></asp:ListItem>
                    <asp:ListItem Text="February" Value="2"></asp:ListItem>
                    <asp:ListItem Text="March" Value="3"></asp:ListItem>
                    <asp:ListItem Text="April" Value="4"></asp:ListItem>
                    <asp:ListItem Text="May" Value="5"></asp:ListItem>
                    <asp:ListItem Text="June" Value="6"></asp:ListItem>
                    <asp:ListItem Text="July" Value="7"></asp:ListItem>
                    <asp:ListItem Text="August" Value="8"></asp:ListItem>
                    <asp:ListItem Text="September" Value="9"></asp:ListItem>
                    <asp:ListItem Text="October" Value="10"></asp:ListItem>
                    <asp:ListItem Text="November" Value="11"></asp:ListItem>
                    <asp:ListItem Text="December" Value="12"></asp:ListItem>
                </asp:DropDownList>
                &nbsp;&nbsp;
                <asp:TextBox ID="txtSearchCompany" runat="server" Width="300px" ToolTip="Enter Company Name."
                    CssClass="SearchTextbox" placeholder="Search Company Name" TabIndex="2" AutoPostBack="true" OnTextChanged="txtSearchCompany_TextChanged"></asp:TextBox>
                <div id="divwidthCust" runat="server">
                </div>
                <cc1:AutoCompleteExtender ID="CustomerExtender" runat="server" TargetControlID="txtSearchCompany"
                    CompletionListElementID="divwidthCust" ServicePath="~/WebService/LeadAutoCompany.asmx"
                    ServiceMethod="GetCompanyList" MinimumPrefixLength="2" BehaviorID="divwidthCust"
                    ContextKey="1234" UseContextKey="True" OnClientItemSelected="OnCustomerSelected"
                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                </cc1:AutoCompleteExtender>
                &nbsp;&nbsp;
                   <asp:DropDownList ID="ddlSalesPerson" runat="server" DataTextField="sName"
                       DataValueField="lid" AppendDataBoundItems="true" TabIndex="2" Width="250px" AutoPostBack="true" OnSelectedIndexChanged="ddlSalesPerson_SelectedIndexChanged">
                       <asp:ListItem Text="- Select Sales Person -" Value="0" Selected="True"></asp:ListItem>
                   </asp:DropDownList>
                <span style="color: Navy">------------------------------------------------------------------</span>
            </div>
            <table border="1">
                <tr>
                    <td align="left" valign="top" style="width: 50%">
                        <div id="divNewLeads" align="left" runat="server">
                            <asp:UpdatePanel ID="UpdatePanelNewLeads" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                <ContentTemplate>
                                    <fieldset style="width: 700px; height: 225px;">
                                        <legend>Customer On-Board  </legend>
                                        <div style="width: 700px; height: 200px; overflow: auto">
                                            <asp:LinkButton ID="lnkCustOnBoardExport" runat="server" OnClick="lnkCustOnBoardExport_Click" >
                                                <asp:Image ID="imgCustOnBoardExportExcel" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                                            </asp:LinkButton>
                                            <asp:GridView ID="gvCustomerOnBoard" runat="server" AutoGenerateColumns="False" DataSourceID="DataSourceCustomerOnBoard"
                                                DataKeyNames="lid" AllowSorting="True" CssClass="table">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sl">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex +1%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" ReadOnly="true" />
                                                    <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="ReferredBy" ReadOnly="true" />
                                                    <asp:BoundField DataField="CreatedDate" HeaderText="On Board Date" SortExpression="LeadDate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                                                </Columns>
                                            </asp:GridView>
                                            <asp:HiddenField ID="hdnFinYearId" runat="server" />
                                        </div>
                                    </fieldset>
                                    <fieldset style="width: 700px; height: 225px;">
                                        <legend>Total Visits</legend>
                                        <div style="width: 700px; height: 200px; overflow: auto">
                                            <asp:LinkButton ID="lnkTotVisit" runat="server" OnClick="lnkTotVisit_Click">
                                                <asp:Image ID="imgTotVisit" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                                            </asp:LinkButton>
                                            <asp:GridView ID="gvTotalVisits" runat="server" AutoGenerateColumns="False" DataSourceID="DataSourceVisits"
                                                DataKeyNames="lid" AllowSorting="True" CssClass="table" Style="white-space: unset">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sl" ItemStyle-Width="5%">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex +1%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Company" HeaderText="Company" SortExpression="Company" ReadOnly="true" ItemStyle-Width="30%" />
                                                    <asp:BoundField DataField="CategoryName" HeaderText="Visit Category" ReadOnly="true" />
                                                    <asp:BoundField DataField="Remark" HeaderText="Notes" SortExpression="Remark" ReadOnly="true" ItemStyle-Width="50%" />
                                                    <asp:BoundField DataField="VisitDate" HeaderText="Visit Date" SortExpression="VisitDate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" ItemStyle-Width="10%" />
                                                    <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy" ReadOnly="true" ItemStyle-Width="10%" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </fieldset>

                                    <fieldset style="width: 700px; height: 225px;">
                                        <legend>Target Month List of Leads</legend>
                                        <div style="width: 700px; height: 200px; overflow: auto">
                                            <asp:LinkButton ID="lnkTargetMonth" runat="server" OnClick="lnkTargetMonth_Click">
                                                <asp:Image ID="imgTargetMonth" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                                            </asp:LinkButton>
                                            <asp:GridView ID="gvTargetMonth" runat="server" CssClass="table" AutoGenerateColumns="false"
                                                AllowSorting="true" DataSourceID="DataSourceLead_TargetMonth">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sl">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Compony Name">
                                                        <ItemTemplate>
                                                            <div style="word-wrap: break-word; width: 155px; white-space: normal;">
                                                                <asp:Label ID="lblComponyName" runat="server" Text='<%#Bind("sName") %>'></asp:Label>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="LeadRefNo" HeaderText="LeadRefNo" SortExpression="LeadRefNo" ReadOnly="true" />
                                                    <asp:BoundField DataField="Lead Owner" HeaderText="Lead Owner" SortExpression="Lead Owner" ReadOnly="true" />
                                                    <asp:BoundField DataField="Target Months" HeaderText="Target Months" SortExpression="Target Months" ReadOnly="true" />
                                                    <asp:TemplateField HeaderText="Remark">
                                                        <ItemTemplate>
                                                            <div style="word-wrap: break-word; width: 100px; white-space: normal;">
                                                                <asp:Label ID="lblRemark" runat="server" Text='<%#Bind("Remark") %>'></asp:Label>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </fieldset>
                                    <div>
                                       <%-- <asp:SqlDataSource ID="DataSourceLeads" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                            SelectCommand="CRM_GetDashboardStagesCount" SelectCommandType="StoredProcedure">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="ddlMonth" PropertyName="SelectedValue" Name="MonthId" DefaultValue="0" />
                                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                                                <asp:ControlParameter ControlID="hdnCustId" PropertyName="Value" Name="CompanyId" DefaultValue="0" />
                                                <asp:ControlParameter ControlID="ddlSalesPerson" PropertyName="SelectedValue" Name="SalesPersonId" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>--%>
                                        <asp:SqlDataSource ID="DataSourceVisits" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                            SelectCommand="CRM_GetDashboardVisits" SelectCommandType="StoredProcedure">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="ddlMonth" PropertyName="SelectedValue" Name="MonthId" DefaultValue="0" />
                                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                                <%--<asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />--%>
                                                <asp:ControlParameter ControlID="hdnFinYearId" PropertyName="Value" Name="FinYearId" DefaultValue="0" />
                                                <asp:ControlParameter ControlID="hdnCustId" PropertyName="Value" Name="CompanyId" DefaultValue="0" />
                                                <asp:ControlParameter ControlID="ddlSalesPerson" PropertyName="SelectedValue" Name="SalesPersonId" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td align="left" valign="top" style="width: 50%">
                        <div id="divCustomerOnBoard" runat="server">
                            <asp:UpdatePanel ID="UpdatePanelCustomerOnBoard" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                <ContentTemplate>

                                    <%--<fieldset style="width: 750px; height: 95px;">
                                        <legend>What's New Today?</legend>
                                        <div>
                                            <table border="1" width="100%">
                                                <tr>
                                                    <td class="WhatsNew_td" style="background-color: #0095ffc7">
                                                        <asp:LinkButton ID="lnkbtnLeadStatus" runat="server" Text="Lead Status Updated" OnClick="lnkbtnLeadStatus_Click" ForeColor="White" Font-Bold="true"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <%-- <tr>
                                                    <td class="WhatsNew_td" style="background-color: #002c7cb3">
                                                        <asp:LinkButton ID="lnkbtnContractExpiry" runat="server" Text="3 months to go for Contract Expiry" OnClick="lnkbtnContractExpiry_Click" ForeColor="White" Font-Bold="true"></asp:LinkButton>
                                                    </td>
                                                </tr>--%>
                                                <%--<tr>
                                                    <td class="WhatsNew_td" style="background-color: #008004e0">
                                                        <asp:LinkButton ID="lnkbtnUpcomingFollowup" runat="server" Text="New/Upcoming Due Date for Follow ups" OnClick="lnkbtnUpcomingFollowup_Click" ForeColor="White" Font-Bold="true"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </fieldset>--%>

                                    <fieldset style="width: 700px; height: 225px;">
                                        <legend>Customer Onboard Volume Summary</legend>
                                        <div style="width:100%; height:200px; overflow:auto">
                                            <asp:LinkButton ID="lnkCustOnboardVolSummary" runat="server" OnClick="lnkCustOnboardVolSummary_Click">
                                                <asp:Image ID="imgCustOnboardVolSummary" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                                            </asp:LinkButton> <%--DataSourceID="SQLOnBoardSummary"--%>
                                            <asp:GridView ID="gvOnBoardSummary" runat="server" AutoGenerateColumns="false" 
                                                AllowSorting="true" PagerSettings-Position="Bottom" CssClass="table" PagerStyle-CssClass="pgr">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sl">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                                                <asp:Label ID="lblCustName" runat="server" Text='<%#Bind("CustName") %>'></asp:Label>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="No Of Jobs" DataField="NO_OF_JOBS" ReadOnly="true" />
                                                    <asp:BoundField HeaderText="No Of Container" DataField="NO_OF_CONT" ReadOnly="true" />
                                                    <asp:BoundField HeaderText="No Of LCL" DataField="NO_OF_LCL" ReadOnly="true" />
                                                    <asp:BoundField HeaderText="No Of Air Shipment" DataField="NO_OF_AIR_SHIPMENT" ReadOnly="true" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <div>
                                        <asp:SqlDataSource ID="SQLOnBoardSummary" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                            SelectCommand="CRM_GetDashboardSummary" SelectCommandType="StoredProcedure">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="ddlMonth" PropertyName="SelectedValue" Name="MonthId" DefaultValue="0" />
                                                <%--<asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />--%>
                                                <asp:ControlParameter ControlID="hdnFinYearId" PropertyName="Value" Name="FinYearId" DefaultValue="0" />
                                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                                <asp:ControlParameter ControlID="ddlSalesPerson" PropertyName="SelectedValue" Name="SalesPersonId" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                    </fieldset>

                                    <fieldset style="width: 700px; height: 225px;">
                                        <legend>3 months to go for Contract Expiry 
                                        </legend>
                                        <div style="width: 100%; height: 200px; overflow: auto">
                                            <asp:LinkButton ID="lnkContractExpiry" runat="server" OnClick="lnkContractExpiry_Click" >
                                                <asp:Image ID="imgContractExpiryExcel" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                                            </asp:LinkButton>
                                            <asp:GridView ID="gvContractExpiry" runat="server" AutoGenerateColumns="false" DataSourceID="DataSourceWhatsNew_Followup"
                                                PagerSettings-Position="TopAndBottom" AllowSorting="true" CssClass="table" PagerStyle-CssClass="pgr">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sl">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Company Name">
                                                        <ItemTemplate>
                                                            <div style="word-wrap: break-word; width: 155px; white-space: normal;">
                                                                <asp:Label ID="lblRemark" runat="server" Text='<%#Bind("sName") %>'></asp:Label>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="LeadRefNo" HeaderText="Lead Ref No" SortExpression="LeadRefNo" ReadOnly="true" />
                                                    <asp:BoundField DataField="Lead Owner" HeaderText="Lead Owner" SortExpression="Lead Owner" ReadOnly="true" />
                                                    <asp:BoundField DataField="Expiry Date" HeaderText="Expiry Date"  SortExpression="Expiry Date" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                                                    <asp:TemplateField HeaderText="Remark">
                                                        <ItemTemplate>
                                                            <div style="word-wrap: break-word; width: 100px; white-space: normal;">
                                                                <asp:Label ID="lblRemark" runat="server" Text='<%#Bind("Remark") %>'></asp:Label>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerTemplate>
                                                    <asp:GridViewPager runat="server"></asp:GridViewPager>
                                                </PagerTemplate>
                                            </asp:GridView>
                                        </div>
                                    </fieldset>
                                    <fieldset style="width: 700px; height: 225px;">
                                        <legend>Leads marked for follow up date </legend>
                                        <div style="width: 100%; height: 200px; overflow: auto">
                                            <asp:LinkButton ID="lnkFollowupDate" runat="server" OnClick="lnkFollowupDate_Click">
                                                <asp:Image ID="imgFollowup" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                                            </asp:LinkButton>
                                            <asp:GridView ID="gvFollowupLead" runat="server" AutoGenerateColumns="false" DataSourceID="DataSourceLead_Followup"
                                                PagerSettings-Position="TopAndBottom" AllowSorting="true" CssClass="table" PagerStyle-CssClass="pgr">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sl">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Company Name">
                                                        <ItemTemplate>
                                                            <div style="word-wrap: break-word; width: 155px; white-space: normal;">
                                                                <asp:Label ID="lblRemark" runat="server" Text='<%#Bind("sName") %>'></asp:Label>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="LeadRefNo" HeaderText="Lead Ref No" SortExpression="LeadRefNo" ReadOnly="true" />
                                                    <asp:BoundField DataField="Lead Owner" HeaderText="Lead Owner" SortExpression="Lead Owner" ReadOnly="true" />
                                                    <asp:BoundField DataField="Date" HeaderText="Followup Date" SortExpression="Date" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                                                    <asp:BoundField DataField="FollowupBy" HeaderText="Followup By" SortExpression="FollowupBy" ReadOnly="true" />
                                                    <asp:TemplateField HeaderText="Remark">
                                                        <ItemTemplate>
                                                            <div style="word-wrap: break-word; width: 100px; white-space: normal;">
                                                                <asp:Label ID="lblRemark" runat="server" Text='<%#Bind("Remark") %>'></asp:Label>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerTemplate>
                                                    <asp:GridViewPager runat="server"></asp:GridViewPager>
                                                </PagerTemplate>
                                            </asp:GridView>
                                        </div>
                                    </fieldset>



                                    <div>
                                        <asp:SqlDataSource ID="DataSourceCustomerOnBoard" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                            SelectCommand="CRM_GetCustomerOnBoard" SelectCommandType="StoredProcedure">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="ddlMonth" PropertyName="SelectedValue" Name="MonthId" DefaultValue="0" />
                                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                                <%--<asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />--%>
                                                <asp:ControlParameter ControlID="hdnFinYearId" PropertyName="Value" Name="FinYearId" DefaultValue="0" />
                                                <asp:ControlParameter ControlID="hdnCustId" PropertyName="Value" Name="CompanyId" DefaultValue="0" />
                                                <asp:ControlParameter ControlID="ddlSalesPerson" PropertyName="SelectedValue" Name="SalesPersonId" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>


                                    <!-- What's New Log -->
                                    <cc1:ModalPopupExtender ID="mpeWhatsNew" runat="server" CacheDynamicResults="false"
                                        PopupControlID="pnlWhatsNew" TargetControlID="hdnWhatsNew" BackgroundCssClass="modalBackground" DropShadow="true">
                                    </cc1:ModalPopupExtender>
                                    <asp:Panel ID="pnlWhatsNew" runat="server" CssClass="ModalPopupPanel" Width="700px" Height="400px" Style="border-radius: 6px">
                                        <div class="header">
                                            <div class="fleft">
                                                &nbsp;
                                               <asp:Label ID="lblPopupTitle" runat="server" Font-Bold="true" Font-Underline="true"></asp:Label>
                                            </div>
                                            <div class="fright">
                                                <asp:ImageButton ID="imgbtnClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgbtnClose_Click" ToolTip="Close" />
                                            </div>
                                        </div>
                                        <div class="clear">
                                        </div>
                                        <div id="Div3" runat="server" style="max-height: 350px; overflow: auto; padding: 5px">
                                            <asp:GridView ID="gvWhatsNewLog" runat="server" CssClass="table" AutoGenerateColumns="true"
                                                EmptyDataText="No record found!" Width="100%">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sl">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex +1%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <div>
                                                <%--<asp:SqlDataSource ID="DataSourceWhatsNew_LeadStatus" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                                    SelectCommand="CRM_GetWhatsNew_LeadStatus" SelectCommandType="StoredProcedure">
                                                    <SelectParameters>
                                                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                                                        <asp:ControlParameter ControlID="hdnCustId" PropertyName="Value" Name="CompanyId" DefaultValue="0" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>--%>
                                                <asp:SqlDataSource ID="DataSourceWhatsNew_Followup" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                                    SelectCommand="CRM_GetWhatsNew_Followup" SelectCommandType="StoredProcedure">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddlMonth" PropertyName="SelectedValue" Name="MonthId" DefaultValue="0" />
                                                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                                        <%--<asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />--%>
                                                        <asp:ControlParameter ControlID="hdnFinYearId" PropertyName="Value" Name="FinYearId" DefaultValue="0" />
                                                        <asp:ControlParameter ControlID="hdnCustId" PropertyName="Value" Name="CompanyId" DefaultValue="0" />
                                                        <asp:ControlParameter ControlID="ddlSalesPerson" PropertyName="SelectedValue" Name="SalesPersonId" DefaultValue="0" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                                <asp:SqlDataSource ID="DataSourceLead_Followup" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                                    SelectCommand="CRM_GetLead_Followup" SelectCommandType="StoredProcedure">
                                                    <SelectParameters>
                                                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                                        <%--<asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />--%>
                                                        <asp:ControlParameter ControlID="hdnFinYearId" PropertyName="Value" Name="FinYearId" DefaultValue="0" />
                                                        <asp:ControlParameter ControlID="hdnCustId" PropertyName="Value" Name="CompanyId" DefaultValue="0" />
                                                        <asp:ControlParameter ControlID="ddlSalesPerson" PropertyName="SelectedValue" Name="SalesPersonId" DefaultValue="0" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                                <asp:SqlDataSource ID="DataSourceLead_TargetMonth" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                                    SelectCommand="CRM_GetTargetMonthLead" SelectCommandType="StoredProcedure">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddlMonth" PropertyName="SelectedValue" Name="MonthId" DefaultValue="0" />
                                                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                                        <%--<asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />--%>
                                                        <asp:ControlParameter ControlID="hdnFinYearId" PropertyName="Value" Name="FinYearId" DefaultValue="0" />
                                                        <asp:ControlParameter ControlID="hdnCustId" PropertyName="Value" Name="CompanyId" DefaultValue="0" />
                                                        <asp:ControlParameter ControlID="ddlSalesPerson" PropertyName="SelectedValue" Name="SalesPersonId" DefaultValue="0" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <asp:HiddenField ID="hdnWhatsNew" runat="server"></asp:HiddenField>
                                    <!-- What's New Log -->

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
            </table>
            <asp:SqlDataSource ID="DataSourceSalesPerson" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                SelectCommand="CRM_GetSalesPerson" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Name="UserId" DefaultValue="0" />
                    <asp:Parameter Name="FinYearId" DefaultValue="0" />
                </SelectParameters>
            </asp:SqlDataSource>

            <!-- Status of Lead-->
            <cc1:ModalPopupExtender ID="mpeLeadStatus" runat="server" CacheDynamicResults="false"
                PopupControlID="pnlLeadStatus" TargetControlID="lnkDummy" BackgroundCssClass="modalBackground" DropShadow="true">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pnlLeadStatus" runat="server" CssClass="ModalPopupPanel" Width="1300px">
                <div class="header">
                    <div class="fleft">
                        <asp:Label ID="lblStatusTitle" runat="server"></asp:Label>
                        <asp:Label ID="lblSalesPerson" runat="server"></asp:Label>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkExport_LeadDetail" runat="server" OnClick="lnkExport_LeadDetail_Click">
                            <asp:Image ID="imgExpLeadDetail" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                        </asp:LinkButton>
                    </div>
                    <div class="fright">
                        <asp:ImageButton ID="imgClose_LeadDetail" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgClose_LeadDetail_Click"
                            ToolTip="Close" />
                    </div>
                </div>
                <div class="m">
                </div>

                <div id="Div2" runat="server" style="width: 1280px; height: 500px; overflow: auto; padding: 5px">
                    <asp:HiddenField ID="hdnQuotationId" runat="server" />
                    <div align="center">
                        <asp:Label ID="lbError_Popup" runat="server" Visible="true"></asp:Label>
                        <asp:HiddenField ID="hdnSalesPersonId" runat="server" />
                        <asp:HiddenField ID="hdnStageId" runat="server" Value="0" />
                    </div>
                    <div>
                        <asp:GridView ID="gvLeadDetail" runat="server" CssClass="table" AutoGenerateColumns="true" DataSourceID="DataSourceLeadDetail"
                            Style="white-space: initial" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-Font-Bold="true" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataText="No Data Found">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </asp:Panel>
            <div>
                <asp:LinkButton ID="lnkDummy" runat="server" Text=""></asp:LinkButton>
                <asp:SqlDataSource ID="DataSourceLeadDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_GetDashboard" SelectCommandType="StoredProcedure"><%--CRM_GetDashboard_StageDetail--%>
                    <SelectParameters>
                        <asp:ControlParameter Name="StageId" ControlID="hdnStageId" PropertyName="Value" DbType="Int32" DefaultValue="0" />
                        <asp:ControlParameter Name="MonthId" ControlID="ddlMonth" PropertyName="SelectedValue" DbType="Int32" DefaultValue="0" />
                        <asp:ControlParameter Name="CompanyId" ControlID="hdnCustId" PropertyName="Value" DefaultValue="0" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" DbType="Int32" />
                        <%--<asp:ControlParameter Name="SalesPersonId" ControlID="ddlSalesPerson" PropertyName="SelectedValue" DbType="Int32" DefaultValue="0" />--%>
                        <asp:SessionParameter Name="SalesPersonId" SessionField="UserId" DbType="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
            <!-- Status of Lead-->

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

