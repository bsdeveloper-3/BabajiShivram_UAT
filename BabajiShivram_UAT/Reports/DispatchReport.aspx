<%@ Page Title="Dispatch Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DispatchReport.aspx.cs"
    Inherits="Reports_DispatchReport" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style type="text/css">
        /*Calendar Control CSS*/
        .cal_Theme1 .ajax__calendar_container {
            background-color: #DEF1F4;
            border: solid 1px #77D5F7;
        }

        .cal_Theme1 .ajax__calendar_header {
            background-color: #ffffff;
            margin-bottom: 4px;
        }

        .cal_Theme1 .ajax__calendar_title,
        .cal_Theme1 .ajax__calendar_next,
        .cal_Theme1 .ajax__calendar_prev {
            color: #004080;
            padding-top: 3px;
        }

        .cal_Theme1 .ajax__calendar_body {
            background-color: #ffffff;
            border: solid 1px #77D5F7;
        }

        .cal_Theme1 .ajax__calendar_dayname {
            text-align: center;
            font-weight: bold;
            margin-bottom: 4px;
            margin-top: 2px;
            color: #004080;
        }

        .cal_Theme1 .ajax__calendar_day {
            color: #004080;
            text-align: center;
        }

        .cal_Theme1 .ajax__calendar_hover .ajax__calendar_day,
        .cal_Theme1 .ajax__calendar_hover .ajax__calendar_month,
        .cal_Theme1 .ajax__calendar_hover .ajax__calendar_year,
        .cal_Theme1 .ajax__calendar_active {
            color: #004080;
            font-weight: bold;
            background-color: #DEF1F4;
        }

        .cal_Theme1 .ajax__calendar_today {
            font-weight: bold;
        }

        .cal_Theme1 .ajax__calendar_other,
        .cal_Theme1 .ajax__calendar_hover .ajax__calendar_today,
        .cal_Theme1 .ajax__calendar_hover .ajax__calendar_title {
            color: #bbbbbb;
        }
    </style>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <div align="center">
        <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
    </div>
    <br />

    <div>
        <asp:Panel ID="panSez" runat="server">
            <fieldset class="fieldset">
                <legend>Dispatch Report</legend>

                <table width="70%">
                    <tr>
                        <td><b>Select Dispatch Date  -  </b></td>
                        <td>&nbsp&nbsp;<b>From :</b>&nbsp;&nbsp;<asp:TextBox ID="txtfrom" runat="server" Width="100px" BackColor="#FFFFCC"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtfrom" Format="dd/MM/yyyy" CssClass=" cal_Theme1"></asp:CalendarExtender>
                        </td>
                        <td><b>To :</b>&nbsp;&nbsp;<asp:TextBox ID="txtTo" runat="server" Width="100px" BackColor="#FFFFCC"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtTo" Format="dd/MM/yyyy" CssClass=" cal_Theme1"></asp:CalendarExtender>
                        </td>
                        <td> Today's Report : 
                            <asp:CheckBox ID="chkToday" runat="server" OnCheckedChanged="chkToday_OnCheckedChanged"/>
                        </td>
                        <td></td>
                        <td>
                            <asp:Button ID="btnSummary" runat="server" Text="Summary" OnClick="btnSummary_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btnDetail" runat="server" Text="Detail" OnClick="btnDetail_Click" />
                        </td>
                    </tr>
<%--                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>

                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>--%>
                </table>

                <%--<asp:GridView ID="gridReport" runat="server" ></asp:GridView>--%>
                <div style="width: 100%">
                    <div style="width: 100%; text-align: left">
                        <asp:Label ID="lblGridMessage" runat="server" Font-Bold="true" Font-Underline="true"></asp:Label>&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkDispatchReport" runat="server" OnClick="lnkDispatchReport_Click"
                            Visible="false" data-tooltip="&nbsp; Export To Excel">
                              <asp:Image ID="imgDispatchReport" runat="server" ImageUrl="../Images/Excel.jpg" />
                        </asp:LinkButton>
                        <asp:LinkButton ID="lnkDispatchDetail" runat="server" OnClick="lnkDispatchDetail_Click"
                            Visible="false" data-tooltip="&nbsp; Export To Excel">
                              <asp:Image ID="imglnkDispatchDetail" runat="server" ImageUrl="../Images/Excel.jpg" />
                        </asp:LinkButton>
                    </div>
                    <div id="divTotalCount" runat="server" style="width: 100%; text-align: right">
                        <b>Billing Dept Total : </b>
                        <asp:Label ID="lblBillDeptTot" runat="server" Font-Bold="true" Font-Underline="true"></asp:Label>&nbsp;&nbsp;
                         <b>PCA Total : </b>
                        <asp:Label ID="lblPCATot" runat="server" Font-Bold="true" Font-Underline="true"></asp:Label>&nbsp;&nbsp;
                    </div>
                </div>

                <asp:GridView ID="gridReport" runat="server" AutoGenerateColumns="False" CssClass="table" ShowFooter="true"
                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" OnRowDataBound="gridReport_RowDataBound"
                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20" OnPageIndexChanging="OnPageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Customer Name">
                            <ItemTemplate>
                                <asp:Label ID="lblCustName" runat="server" Text='<%#Eval("CustName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Billing Department">
                            <ItemTemplate>
                                <asp:Label ID="lblBillDept" runat="server" Text='<%#Eval("BillDept") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="PCA">
                            <ItemTemplate>
                                <asp:Label ID="lblPCA" runat="server" Text='<%#Eval("PCA") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <asp:GridView ID="gridDetailReport" runat="server" AutoGenerateColumns="False" CssClass="table"
                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr"
                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20" OnPageIndexChanging="gridDetailReport_OnPageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Job Reference No">
                            <ItemTemplate>
                                <asp:Label ID="lblJobRefNo" runat="server" Text='<%#Eval("JobRefNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Customer Name">
                            <ItemTemplate>
                                <asp:Label ID="lblCustName" runat="server" Text='<%#Eval("CustName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Delivery Type">
                            <ItemTemplate>
                                <asp:Label ID="lblTypeOfDelivery" runat="server" Text='<%#Eval("TypeOfDelivery") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Doc Carry By">
                            <ItemTemplate>
                                <asp:Label ID="lblDocCarry" runat="server" Text='<%#Eval("CarryingPerson") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Delivered Date">
                            <ItemTemplate>
                                <asp:Label ID="lblDeliveredDate" runat="server" Text='<%#Eval("PCDDeliveryDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Received By">
                            <ItemTemplate>
                                <asp:Label ID="lblReceivedBy" runat="server" Text='<%#Eval("ReceivedBy") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Courier Name">
                            <ItemTemplate>
                                <asp:Label ID="lblCourierName" runat="server" Text='<%#Eval("CourierName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Docket No">
                            <ItemTemplate>
                                <asp:Label ID="lblDocketNo" runat="server" Text='<%#Eval("DocketNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="PCD Customer">
                            <ItemTemplate>
                                <asp:Label ID="lblPCDCustomer" runat="server" Text='<%#Eval("PCDCustomer") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Dispatch Date">
                            <ItemTemplate>
                                <asp:Label ID="lblDispatchDate" runat="server" Text='<%#Eval("DispatchDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Dispatch By">
                            <ItemTemplate>
                                <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("sName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

            </fieldset>
        </asp:Panel>
    </div>

</asp:Content>

