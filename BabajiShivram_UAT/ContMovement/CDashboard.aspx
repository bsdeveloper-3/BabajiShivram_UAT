<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CDashboard.aspx.cs" Inherits="ContMovement_CDashboard" EnableEventValidation="false" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
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

    <asp:UpdatePanel ID="upnlCMDashboard" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div style="float: left; margin-left: 10px; width: 55%;">
                <fieldset>
                    <legend>No. of Jobs Count
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkbtnNoofJobs" runat="server" OnClick="lnkbtnNoofJobs_Click" data-tooltip="&nbsp; &nbsp; &nbsp; Export To Excel">
                            <asp:Image ID="imgNoofJob" runat="server" ImageUrl="../Images/Excel.jpg" />
                        </asp:LinkButton>
                    </legend>
                    <div>
                        <asp:FormView ID="fvJobDetail" HeaderStyle-Font-Bold="true" runat="server" DataSourceID="DataSourceNoofJobs" Width="100%">
                            <ItemTemplate>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                    <tr style="background-color: #cbcbdc">
                                        <td><b>Sr.No.</b></td>
                                        <td><b>Particular</b></td>
                                        <td><b>Stage</b></td>
                                        <td><b>Number of Jobs Count</b></td>
                                    </tr>
                                    <tr>
                                        <td>1</td>
                                        <td>Newly Created Jobs</td>
                                        <td>Movement Detail</td>
                                        <td>
                                            <asp:LinkButton ID="lnkbtnNewJobs" runat="server" OnClick="lnkbtnNewJobs_Click" Text='<%#Eval("Jobs_CreatedNew") %>'
                                                Font-Bold="true" Font-Underline="true"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>2</td>
                                        <td>ETA within 7 days</td>
                                        <td></td>
                                        <td>
                                            <asp:LinkButton ID="lnkbtnETAValidity" runat="server" OnClick="lnkbtnETAValidity_Click" Text='<%#Eval("Jobs_ETAValidity") %>'
                                                Font-Bold="true" Font-Underline="true"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>3</td>
                                        <td>Movement Completion Pending</td>
                                        <td>Movement Process</td>
                                        <td>
                                            <asp:LinkButton ID="lnkbtnMovementCompPending" runat="server" OnClick="lnkbtnMovementCompPending_Click" Text='<%#Eval("Jobs_MovementCompletionPending") %>'
                                                Font-Bold="true" Font-Underline="true"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>4</td>
                                        <td>Empty Container Return Date Pending</td>
                                        <td>Movement Process</td>
                                        <td>
                                            <asp:LinkButton ID="lnkbtnEmptyContReturnPending" runat="server" OnClick="lnkbtnEmptyContReturnPending_Click" Text='<%#Eval("Jobs_EmptyContReturnPending") %>'
                                                Font-Bold="true" Font-Underline="true"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>5</td>
                                        <td>Container Received At Yard Date Pending</td>
                                        <td>Cont Received</td>
                                        <td>
                                            <asp:LinkButton ID="lnkbtnContCFSDatePending" runat="server" OnClick="lnkbtnContCFSDatePending_Click" Text='<%#Eval("Jobs_ContCFSDatePending") %>'
                                                Font-Bold="true" Font-Underline="true"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>6</td>
                                        <td>Billing Scrutiny Pending</td>
                                        <td>Billing Scrutiny</td>
                                        <td>
                                            <asp:LinkButton ID="lnkbtnScrutinyPending" runat="server" OnClick="lnkbtnScrutinyPending_Click" Text='<%#Eval("Jobs_ScrutinyPending") %>'
                                                Font-Bold="true" Font-Underline="true"></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:FormView>
                        <div>
                            <asp:SqlDataSource ID="DataSourceNoofJobs" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="CM_GetDashboardJobCount" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </div>
                </fieldset>
            </div>
            <div style="float: left; margin-left: 10px; width: 40%;">
                <fieldset>
                    <legend>Notifications
                        <asp:Label ID="lblNotificationCount" runat="server" Width="20px"></asp:Label>
                    </legend>
                    <div style="height: 150px; overflow: auto">
                        <asp:GridView ID="gvNotification" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr"
                            DataKeyNames="JobId" OnRowCommand="gvNotification_RowCommand" AllowPaging="false" AllowSorting="True" CssClass="table" PageSize="20"
                            DataSourceID="SqlDataSourceNotifications" ShowHeader="false">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnNotification" runat="server" Font-Underline="true" CommandName="Select" Text='<%#Eval("Status") %>'
                                            CommandArgument='<%#Eval("JobId") + ";" + Eval("StatusId")%>' ToolTip="Select to view in detail."></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div>
                            <asp:SqlDataSource ID="SqlDataSourceNotifications" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="CM_GetMovementNotifications" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </div>
                </fieldset>
            </div>
            <div style="float: left; margin-left: 10px; width: 90%;">
                <fieldset>
                    <legend>Empty Container Return Date Pending (for more than 3 days)
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkbtnEmptyContPending2" runat="server" OnClick="lnkbtnEmptyContPending2_Click" data-tooltip="&nbsp; &nbsp; &nbsp; Export To Excel">
                            <asp:Image ID="imgEmptyContPending2" runat="server" ImageUrl="../Images/Excel.jpg" />
                        </asp:LinkButton>
                    </legend>
                    <div style="width: 1325px; overflow: auto">
                        <asp:GridView ID="gvEmptyContDatePending" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr"
                            DataKeyNames="lid" OnRowCommand="gvEmptyContDatePending_RowCommand" AllowPaging="True" AllowSorting="True" CssClass="table" PageSize="20"
                            PagerSettings-Position="TopAndBottom" DataSourceID="DataSourceEmptyContPending" OnPreRender="gvEmptyContDatePending_PreRender" OnRowDataBound="gvEmptyContDatePending_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Job Ref No">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnJobRfNo" runat="server" CommandName="Select" Text='<%#Eval("JobRefNo") %>' Font-Bold="true"
                                            CommandArgument='<%#Eval("JobId") %>' Font-Underline="true" ToolTip="Add empty container return date for this job."></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="JobRefNo" HeaderText="JobRefNo" SortExpression="JobRefNo" ReadOnly="true" Visible="false" />
                                <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" ReadOnly="true" />
                                <asp:BoundField DataField="ConsigneeName" HeaderText="Consignee" SortExpression="ConsigneeName" ReadOnly="true" />
                                <asp:BoundField DataField="ETADate" HeaderText="ETA Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ETADate" ReadOnly="true" />
                                <asp:BoundField DataField="LastDispatchDate" HeaderText="Dispatch Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LastDispatchDate" ReadOnly="true" />
                                <asp:BoundField DataField="BranchName" HeaderText="Branch" SortExpression="BranchName" ReadOnly="true" />
                                <asp:BoundField DataField="SumOf20" HeaderText="Sum Of 20" SortExpression="SumOf20" ReadOnly="true" />
                                <asp:BoundField DataField="SumOf40" HeaderText="Sum Of 40" SortExpression="SumOf40" ReadOnly="true" />
                                <asp:BoundField DataField="ContainerType" HeaderText="Cont Type" SortExpression="ContainerType" ReadOnly="true" />
                                <asp:BoundField DataField="JobCreationDate" HeaderText="Job Creation" DataFormatString="{0:dd/MM/yyyy}" SortExpression="JobCreationDate" ReadOnly="true" />
                                <asp:BoundField DataField="JobCreatedBy" HeaderText="Job Created By" DataFormatString="{0:dd/MM/yyyy}" SortExpression="JobCreatedBy" ReadOnly="true" />
                                <asp:BoundField DataField="CFSName" HeaderText="CFS Name" SortExpression="CFSName" ReadOnly="true" />
                                <asp:BoundField DataField="ShippingLineDate" HeaderText="Shipping Line Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ShippingLineDate" ReadOnly="true" />
                                <asp:BoundField DataField="ConfirmedByLineDate" HeaderText="Confirmed By Line Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ConfirmedByLineDate" ReadOnly="true" />
                                <asp:BoundField DataField="MovementCompDate" HeaderText="Movement Complete" DataFormatString="{0:dd/MM/yyyy}" SortExpression="MovementCompDate" ReadOnly="true" />
                                <asp:BoundField DataField="Aging" HeaderText="Aging" SortExpression="Aging" ReadOnly="true" />
                            </Columns>
                            <PagerTemplate>
                                <asp:GridViewPager runat="server" />
                            </PagerTemplate>
                        </asp:GridView>
                        <div>
                            <asp:SqlDataSource ID="DataSourceEmptyContPending" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="CM_GetDashboardEmptyCont" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </div>
                </fieldset>
            </div>
            <div style="float: left; margin-left: 10px; width: 90%;">
                <fieldset>
                    <legend>Container Received At Yard Date Pending (for more than 3 days)
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkbtnContCFSRecdPending" runat="server" OnClick="lnkbtnContCFSRecdPending_Click" data-tooltip="&nbsp; &nbsp; &nbsp; Export To Excel">
                            <asp:Image ID="imgContCFSRecdPending" runat="server" ImageUrl="../Images/Excel.jpg" />
                        </asp:LinkButton>
                    </legend>
                    <div style="width: 1325px; overflow: auto">
                        <asp:GridView ID="gvContCFSRecdPending" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr"
                            DataKeyNames="lid" OnRowCommand="gvContCFSRecdPending_RowCommand" AllowPaging="True" AllowSorting="True" CssClass="table" PageSize="20"
                            PagerSettings-Position="TopAndBottom" DataSourceID="DataSourceContCFSPending" OnPreRender="gvContCFSRecdPending_PreRender" OnRowDataBound="gvContCFSRecdPending_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CFS Updated Cont" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnContRecdCFSDate" CommandName="container" runat="server" Font-Bold="true"
                                            CommandArgument='<%#Eval("JobId") + ";" + Eval("JobRefNo")%>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="CFS Updated Cont" ReadOnly="true" Visible="false" />
                                <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" SortExpression="JobRefNo" ReadOnly="true" />
                                <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" ReadOnly="true" />
                                <asp:BoundField DataField="ConsigneeName" HeaderText="Consignee" SortExpression="ConsigneeName" ReadOnly="true" />
                                <asp:BoundField DataField="ETADate" HeaderText="ETA Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ETADate" ReadOnly="true" />
                                <asp:BoundField DataField="LastDispatchDate" HeaderText="Dispatch Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LastDispatchDate" ReadOnly="true" />
                                <asp:BoundField DataField="BranchName" HeaderText="Branch" SortExpression="BranchName" ReadOnly="true" />
                                <asp:BoundField DataField="SumOf20" HeaderText="Sum Of 20" SortExpression="SumOf20" ReadOnly="true" />
                                <asp:BoundField DataField="SumOf40" HeaderText="Sum Of 40" SortExpression="SumOf40" ReadOnly="true" />
                                <asp:BoundField DataField="ContainerType" HeaderText="Cont Type" SortExpression="ContainerType" ReadOnly="true" />
                                <asp:BoundField DataField="JobCreationDate" HeaderText="Job Creation" DataFormatString="{0:dd/MM/yyyy}" SortExpression="JobCreationDate" ReadOnly="true" />
                                <asp:BoundField DataField="JobCreatedBy" HeaderText="Job Created By" DataFormatString="{0:dd/MM/yyyy}" SortExpression="JobCreatedBy" ReadOnly="true" />
                                <asp:BoundField DataField="CFSName" HeaderText="CFS Name" SortExpression="CFSName" ReadOnly="true" />
                                <asp:BoundField DataField="ShippingLineDate" HeaderText="Shipping Line Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ShippingLineDate" ReadOnly="true" />
                                <asp:BoundField DataField="ConfirmedByLineDate" HeaderText="Confirmed By Line Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ConfirmedByLineDate" ReadOnly="true" />
                                <asp:BoundField DataField="MovementCompDate" HeaderText="Movement Complete" DataFormatString="{0:dd/MM/yyyy}" SortExpression="MovementCompDate" ReadOnly="true" />
                                <asp:BoundField DataField="EmptyContReturnDate" HeaderText="Empty Cont Return Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="EmptyContReturnDate" ReadOnly="true" />
                                <asp:BoundField DataField="Aging" HeaderText="Aging" SortExpression="Aging" ReadOnly="true" />
                            </Columns>
                            <PagerTemplate>
                                <asp:GridViewPager runat="server" />
                            </PagerTemplate>
                        </asp:GridView>
                        <div>
                            <asp:SqlDataSource ID="DataSourceContCFSPending" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="CM_GetDashboardContCFSRecd" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </div>
                </fieldset>
            </div>
            <div>
                <asp:HiddenField ID="hdnParticularId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnJobDetail" runat="server" />
                <cc1:ModalPopupExtender ID="mpeJobDetail" runat="server" TargetControlID="hdnJobDetail" PopupControlID="pnlJobDetail"
                    BackgroundCssClass="modalBackground" DropShadow="true" CancelControlID="imgbtnCancelJobDetail">
                </cc1:ModalPopupExtender>

                <asp:Panel ID="pnlJobDetail" runat="server" CssClass="modalPopup" Width="1290px" Height="400px">
                    <div id="div3" runat="server">
                        <table width="100%">
                            <tr class="heading" style="background-color: white">
                                <td align="center">
                                    <b>&nbsp;&nbsp;<asp:Label ID="lblTitle" runat="server" Text=""></asp:Label></b>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="lnkbtnJobDetail" runat="server" OnClick="lnkbtnJobDetail_Click" ToolTip="Export To Excel">
                                        <asp:Image ID="imgJobDetail" runat="server" ImageUrl="~/Images/Excel.jpg" Style="margin-top: 5px" />
                                    </asp:LinkButton>
                                    <span style="float: right">
                                        <asp:ImageButton ID="imgbtnCancelJobDetail" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgbtnCancelJobDetail_Click" ToolTip="Close" />
                                    </span>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="pnlJobDetail2" runat="server" Style="overflow: auto; width: 1270px; height: 350px; padding: 5px" BorderStyle="Solid" BorderWidth="1px">
                            <br />
                            <div class="clear">
                                <div class="fleft">
                                    <uc1:DataFilter ID="DataFilter1" runat="server" />
                                </div>
                            </div>
                            <div class="clear"></div>
                            <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="false" AllowPaging="false" DataSourceID="DataSourceJobDetail"
                                CssClass="table" OnRowCommand="gvJobDetail_RowCommand" Width="90%" OnRowDataBound="gvJobDetail_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" SortExpression="JobRefNo" ReadOnly="true" />
                                    <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" ReadOnly="true" />
                                    <asp:BoundField DataField="ConsigneeName" HeaderText="Consignee" SortExpression="ConsigneeName" ReadOnly="true" />
                                    <asp:BoundField DataField="ETADate" HeaderText="ETA Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ETADate" ReadOnly="true" />
                                    <asp:BoundField DataField="PortName" HeaderText="Port" SortExpression="PortName" ReadOnly="true" />
                                    <asp:BoundField DataField="LastDispatchDate" HeaderText="Dispatch Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LastDispatchDate" ReadOnly="true" />
                                    <asp:BoundField DataField="BranchName" HeaderText="Branch" SortExpression="BranchName" ReadOnly="true" />
                                    <asp:BoundField DataField="SumOf20" HeaderText="Sum Of 20" SortExpression="SumOf20" ReadOnly="true" />
                                    <asp:BoundField DataField="SumOf40" HeaderText="Sum Of 40" SortExpression="SumOf40" ReadOnly="true" />
                                    <asp:BoundField DataField="ContainerType" HeaderText="Cont Type" SortExpression="ContainerType" ReadOnly="true" />
                                    <asp:BoundField DataField="JobCreationDate" HeaderText="Job Creation" DataFormatString="{0:dd/MM/yyyy}" SortExpression="JobCreationDate" ReadOnly="true" />
                                    <asp:BoundField DataField="JobCreatedBy" HeaderText="Job Created By" DataFormatString="{0:dd/MM/yyyy}" SortExpression="JobCreatedBy" ReadOnly="true" />
                                    <asp:BoundField DataField="CFSName" HeaderText="CFS Name" SortExpression="CFSName" ReadOnly="true" />
                                    <asp:BoundField DataField="ShippingLineDate" HeaderText="Shipping Line Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ShippingLineDate" ReadOnly="true" />
                                    <asp:BoundField DataField="ConfirmedByLineDate" HeaderText="Confirmed By Line Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ConfirmedByLineDate" ReadOnly="true" />
                                    <asp:BoundField DataField="MovementCompDate" HeaderText="Movement Complete" DataFormatString="{0:dd/MM/yyyy}" SortExpression="MovementCompDate" ReadOnly="true" />
                                    <asp:BoundField DataField="EmptyContReturnDate" HeaderText="Empty Cont Return Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="EmptyContReturnDate" ReadOnly="true" />
                                    <asp:BoundField DataField="ContCFSReceivedDate" HeaderText="Container Received At Yard Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="MovementCompDate" ReadOnly="true" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" ReadOnly="true" />
                                </Columns>
                            </asp:GridView>
                            <div>
                                <asp:SqlDataSource ID="DataSourceJobDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="CM_GetDashboardJobDetail" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                                        <asp:ControlParameter ControlID="hdnParticularId" Name="Particular" PropertyName="Value" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </asp:Panel>
                    </div>
                </asp:Panel>
            </div>
            <%-- Popup for Cont Recd updated dates --%>
            <div>
                <asp:LinkButton ID="lnkUpdatedCont" runat="server" Text=""></asp:LinkButton>
                <cc1:ModalPopupExtender ID="mpeUpdatedCont" runat="server" TargetControlID="lnkUpdatedCont" CancelControlID="imgEmailClose"
                    PopupControlID="pnlUpdatedCont" BackgroundCssClass="modalBackground" CacheDynamicResults="false">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pnlUpdatedCont" runat="server" CssClass="ModalPopupPanel" Style="border-radius: 5px; padding: 5px">
                    <div class="header">
                        <div class="fleft">
                            Updated Containers Received At Yard for Job -
                            <asp:Label ID="lblUpdatedContForJob" runat="server" Font-Bold="true" Font-Underline="true"></asp:Label>
                        </div>
                        <div class="fright">
                            <asp:Button ID="imgEmailClose" runat="server" OnClick="imgEmailClose_Click" ToolTip="Close" Text="Close" Style="background: white; color: black" />
                        </div>
                    </div>
                    <div id="DivABC" runat="server" style="height: 400px; width: 600px; overflow: auto; padding: 5px">
                        <fieldset>
                            <legend>Container Detail</legend>
                            <div style="height: 200px; overflow: auto;">
                                <asp:GridView ID="gvContainerDetail" runat="server" CssClass="table" AutoGenerateColumns="false" DataSourceID="DataSourceContReceived"
                                    OnRowDataBound="gvContainerDetail_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="ContainerNo" HeaderText="Container No" SortExpression="ContainerNo" ReadOnly="true" />
                                        <asp:BoundField DataField="ContainerSize" HeaderText="Container Size" SortExpression="ContainerSize" ReadOnly="true" />
                                        <asp:BoundField DataField="ContainerType" HeaderText="Container Type" SortExpression="ContainerType" ReadOnly="true" />
                                        <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" SortExpression="VehicleNo" ReadOnly="true" />
                                        <asp:BoundField DataField="TransporterName" HeaderText="Transporter Name" SortExpression="TransporterName" ReadOnly="true" />
                                        <asp:BoundField DataField="LRNo" HeaderText="LR No" SortExpression="LRNo" ReadOnly="true" />
                                        <asp:BoundField DataField="LRDate" HeaderText="LR Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LRDate" ReadOnly="true" />
                                        <asp:BoundField DataField="BabajiChallanNo" HeaderText="Challan No" SortExpression="BabajiChallanNo" ReadOnly="true" />
                                        <asp:BoundField DataField="DispatchDate" HeaderText="Dispatch Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="DispatchDate" ReadOnly="true" />
                                        <asp:BoundField DataField="ContRecdAtCFSDate" HeaderText="Cont Recd at Yard Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ContRecdAtCFSDate" ReadOnly="true" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div>
                                <asp:SqlDataSource ID="DataSourceContReceived" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="CM_GetContainerDetails" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </fieldset>
                    </div>
                </asp:Panel>
            </div>
            <%-- Popup for Cont Recd updated dates --%>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

