<%@ Page Title="Weekly sales wise" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" EnableEventValidation="false"
    CodeFile="WeeklySalesWise.aspx.cs" Inherits="CRMReports_WeeklySalesWise" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
    </asp:ScriptManager>
    <style type="text/css">
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
    </style>
    <div>
        <asp:UpdateProgress ID="uprogress" runat="server" AssociatedUpdatePanelID="upnlWeeklySalesWise">
            <ProgressTemplate>
                <img alt="progress" src="../Images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upnlWeeklySalesWise" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            </div>


            <fieldset>
                <legend>Weekly Sales Report</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="m clear">
                            <table>
                                <tr>
                                    <td>
                                        <div class="fleft" style="text-align: left; margin-right: 5px;">
                                            Sales Person
                                            <asp:DropDownList ID="ddlUser" runat="server" DataSourceID="DataSourceSalesPerson" DataTextField="sName"
                                                DataValueField="lid" AppendDataBoundItems="true" TabIndex="2" Width="250px" AutoPostBack="true"
                                                Style="margin-left: 12px;" OnSelectedIndexChanged="ddlUser_SelectedIndexChanged">
                                                <asp:ListItem Text="- Select Sales Person -" Value="0" ></asp:ListItem>
                                            </asp:DropDownList>
                                            &nbsp;
                                        </div>
                                    </td>
                                    <td>
                                        <div class="m clear" style="text-align: left; margin-right: 5px;">
                                            Date Between
                            <cc1:calendarextender id="calStartDate" runat="server" firstdayofweek="Sunday"
                                format="dd/MM/yyyy" popupposition="BottomRight" targetcontrolid="txtStartDate">
                            </cc1:calendarextender>
                                            <cc1:maskededitextender id="meeStartDate" targetcontrolid="txtStartDate" mask="99/99/9999" messagevalidatortip="true"
                                                masktype="Date" autocomplete="false" runat="server">
                            </cc1:maskededitextender>
                                            <cc1:maskededitvalidator id="mevStartDate" controlextender="meeStartDate" controltovalidate="txtStartDate" isvalidempty="false"
                                                invalidvaluemessage="Start Date is invalid" invalidvalueblurredmessage="Invalid Start Date" setfocusonerror="true"
                                                minimumvaluemessage="Invalid Start Date" maximumvaluemessage="Invalid Date" minimumvalue="01/01/2016" maximumvalue="31/12/2025"
                                                runat="Server" validationgroup="vgVisitReport">
                                            </cc1:maskededitvalidator>
                                            &nbsp;&nbsp;
                                            <asp:TextBox ID="txtStartDate" runat="server" Width="80px" placeholder="dd/mm/yyyy" TabIndex="1" ToolTip="Select Start Date." AutoPostBack="true" OnTextChanged="txtStartDate_TextChanged"></asp:TextBox>
                                                            &nbsp;&nbsp;To
                                            <cc1:calendarextender id="calEndDate" runat="server" firstdayofweek="Sunday"
                                                format="dd/MM/yyyy" popupposition="BottomRight" targetcontrolid="txtEndDate">
                                            </cc1:calendarextender>
                                                            <cc1:maskededitextender id="meeEndDate" targetcontrolid="txtEndDate" mask="99/99/9999" messagevalidatortip="true"
                                                                masktype="Date" autocomplete="false" runat="server">
                                            </cc1:maskededitextender>
                                            <cc1:maskededitvalidator id="mevEndDate" controlextender="meeEndDate" controltovalidate="txtEndDate" isvalidempty="false"
                                                invalidvaluemessage="End Date is invalid" invalidvalueblurredmessage="Invalid End Date" setfocusonerror="true"
                                                minimumvaluemessage="Invalid End Date" maximumvaluemessage="Invalid Date" minimumvalue="01/01/2016" maximumvalue="31/12/2025"
                                                runat="Server" validationgroup="vgVisitReport">
                                            </cc1:maskededitvalidator>
                                                            &nbsp;&nbsp;
                                            <asp:TextBox ID="txtEndDate" runat="server" Width="80px" placeholder="dd/mm/yyyy" TabIndex="2" ToolTip="Select End Date." AutoPostBack="true" OnTextChanged="txtEndDate_TextChanged"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnExportToPDF" runat="server" Text="Export To PDF" OnClick="btnExportToPDF_Click" CommandName="Select" />
                                       <%-- <asp:Button ID="btnExportToExcel" runat="server" Text="Export To Excel" OnClick="btnExportToExcel_Click" CommandName="Excel" />--%>
                                    </td>
                                </tr>
                            </table>

                        </div>
                        <%--<div class="m clear"></div>--%>
                    </asp:Panel>
                </div>
                <div id="dvsalePerson" runat="server" class="m clear">
                    <asp:Panel ID="Panel1" runat="server">
                        <legend>Sales Person :  
                            <asp:Label ID="lblSalePerson" runat="server"></asp:Label>
                        </legend>


                        <asp:SqlDataSource ID="DataSourceSalesPerson" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                            SelectCommand="CRM_GetSalesPersonList" SelectCommandType="StoredProcedure"><%--CRM_GetSalesPerson--%>
                            <SelectParameters>
                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                        <div class="m clear"></div>

                        <div id="dvtable" runat="server">

                            <legend>
                                <b>Current Status :
                        <asp:Label ID="lblCurrentStatus1" runat="server"></asp:Label></b></legend>

                            <asp:Repeater ID="Repeater_rptWeeklySaleWise1" runat="server" Visible="true" OnItemCreated="Repeater_rptWeeklySaleWise1_ItemCreated">
                                <HeaderTemplate>
                                    <table class="table" style="width: 100%">
                                        <tr>
                                            <td style="width: 15px; word-wrap: break-word">Sl</td>

                                            <td style="width: 150px; word-wrap: break-word">Lead Name </td>

                                            <td style="width: 100px; word-wrap: break-word">Location </td>

                                            <td style="width: 100px; word-wrap: break-word">Expected Volume </td>

                                            <td style="width: 40px; word-wrap: break-word">Activity Date </td>

                                            <td style="width: 300px; word-wrap: break-word">Activity Details </td>

                                            <td id="lblUName1" style="width: 100px; word-wrap: break-word" runat="server">Sales Person Name</td>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td width="15px">
                                            <%# Container.ItemIndex+1 %>
                                        </td>
                                        <td width="150px">
                                            <asp:Label ID="lblLeadName" runat="server" Style="word-wrap: break-word; width: 150px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.Lead_Name")%></asp:Label>
                                        </td>
                                        <td width="100px">
                                            <asp:Label ID="lblLocation" runat="server" Style="word-wrap: break-word; width: 100px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.Location")%></asp:Label>
                                        </td>
                                        <td width="100px">
                                            <asp:Label ID="lblVolumExpected" runat="server" Style="word-wrap: break-word; width: 100px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.VolumeExpected")%></asp:Label>
                                        </td>
                                        <td width="40px">
                                            <asp:Label ID="lblActivityDt" runat="server" Style="word-wrap: break-word; width: 40px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.ACTIVITY_DATE")%></asp:Label>
                                        </td>
                                        <td width="300px">
                                            <asp:Label ID="lblActivity" runat="server" Style="word-wrap: break-word; width: 300px; white-space: normal;">
                                <%#DataBinder.Eval(Container,"DataItem.ACTIVITY_DETAILS")%></asp:Label>
                                        </td>
                                        <td width="100px" id="lblName1" runat="server">
                                            <asp:Label ID="lblUser" runat="server" Style="word-wrap: break-word; width: 300px; white-space: normal;">
                                            <%#DataBinder.Eval(Container,"DataItem.sName")%></asp:Label>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <tr>
                                <td>
                                    <asp:Label ID="lblErrMsg1" runat="server"></asp:Label>
                                </td>
                            </tr>



                            <div class="m clear"></div>
                            <legend>
                                <b>Current Status :
                        <asp:Label ID="lblCurrentStatus2" runat="server"></asp:Label></b>
                            </legend>
                            <asp:Repeater ID="Repeater_rptWeeklySaleWise2" runat="server" Visible="true" OnItemCreated="Repeater_rptWeeklySaleWise2_ItemCreated">
                                <HeaderTemplate>
                                    <table class="table" style="width: 100%">
                                        <tr>
                                            <td style="width: 15px; word-wrap: break-word">Sl</td>

                                            <td style="width: 150px; word-wrap: break-word">Lead Name </td>

                                            <td style="width: 100px; word-wrap: break-word">Location </td>

                                            <td style="width: 100px; word-wrap: break-word">Expected Volume </td>

                                            <td style="width: 40px; word-wrap: break-word">Activity Date </td>

                                            <td style="width: 300px; word-wrap: break-word">Activity Details </td>

                                            <td id="lblUName2" style="width: 100px; word-wrap: break-word" runat="server">Sales Person Name</td>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td width="15px">
                                            <%# Container.ItemIndex+1 %>
                                        </td>
                                        <td width="150px">
                                            <asp:Label ID="lblLeadName2" runat="server" Style="word-wrap: break-word; width: 150px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.Lead_Name")%></asp:Label>
                                        </td>
                                        <td width="100px">
                                            <asp:Label ID="lblLocation2" runat="server" Style="word-wrap: break-word; width: 100px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.Location")%></asp:Label>
                                        </td>
                                        <td width="100px">
                                            <asp:Label ID="lblVolumExpected2" runat="server" Style="word-wrap: break-word; width: 100px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.VolumeExpected")%></asp:Label>
                                        </td>
                                        <td width="40px">
                                            <asp:Label ID="lblActivityDt2" runat="server" Style="word-wrap: break-word; width: 40px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.ACTIVITY_DATE")%></asp:Label>
                                        </td>
                                        <td width="300px">
                                            <asp:Label ID="lblActivity2" runat="server" Style="word-wrap: break-word; width: 300px; white-space: normal;">
                                <%#DataBinder.Eval(Container,"DataItem.ACTIVITY_DETAILS")%></asp:Label>
                                        </td>
                                        <td width="100px" id="lblName2" runat="server">
                                            <asp:Label ID="lblUser2" runat="server" Style="word-wrap: break-word; width: 300px; white-space: normal;">
                                            <%#DataBinder.Eval(Container,"DataItem.sName")%></asp:Label>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <tr>
                                <td>
                                    <asp:Label ID="lblErrMsg2" runat="server"></asp:Label>
                                </td>
                            </tr>

                            <div class="m clear"></div>
                            <legend><b>Current Status :
                    <asp:Label ID="lblCurrentStatus3" runat="server"></asp:Label></b></legend>
                            <asp:Repeater ID="Repeater_rptWeeklySaleWise3" runat="server" Visible="true" OnItemCreated="Repeater_rptWeeklySaleWise3_ItemCreated">
                                <HeaderTemplate>
                                    <table class="table" style="width: 100%">
                                        <tr>
                                            <td style="width: 15px; word-wrap: break-word">Sl</td>

                                            <td style="width: 150px; word-wrap: break-word">Lead Name </td>

                                            <td style="width: 100px; word-wrap: break-word">Location </td>

                                            <td style="width: 100px; word-wrap: break-word">Expected Volume </td>

                                            <td style="width: 40px; word-wrap: break-word">Activity Date </td>

                                            <td style="width: 300px; word-wrap: break-word">Activity Details </td>

                                            <td style="width: 100px; word-wrap: break-word" id="lblUName3" runat="server">Sales Person Name</td>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td width="15px">
                                            <%# Container.ItemIndex+1 %>
                                        </td>
                                        <td width="150px">
                                            <asp:Label ID="lblLeadName3" runat="server" Style="word-wrap: break-word; width: 150px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.Lead_Name")%></asp:Label>
                                        </td>
                                        <td width="100px">
                                            <asp:Label ID="lblLocation3" runat="server" Style="word-wrap: break-word; width: 100px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.Location")%></asp:Label>
                                        </td>
                                        <td width="100px">
                                            <asp:Label ID="lblVolumExpected3" runat="server" Style="word-wrap: break-word; width: 100px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.VolumeExpected")%></asp:Label>
                                        </td>
                                        <td width="40px">
                                            <asp:Label ID="lblActivityDt3" runat="server" Style="word-wrap: break-word; width: 40px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.ACTIVITY_DATE")%></asp:Label>
                                        </td>
                                        <td width="300px">
                                            <asp:Label ID="lblActivity3" runat="server" Style="word-wrap: break-word; width: 300px; white-space: normal;">
                                <%#DataBinder.Eval(Container,"DataItem.ACTIVITY_DETAILS")%></asp:Label>
                                        </td>
                                        <td width="100px" id="lblName3" runat="server">
                                            <asp:Label ID="lblUser3" runat="server" Style="word-wrap: break-word; width: 300px; white-space: normal;">
                                            <%#DataBinder.Eval(Container,"DataItem.sName")%></asp:Label>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <tr>
                                <td>
                                    <asp:Label ID="lblErrMsg3" runat="server"></asp:Label>
                                </td>
                            </tr>

                            <div class="m clear"></div>
                            <legend><b>Current Status :
                    <asp:Label ID="lblCurrentStatus4" runat="server"></asp:Label></b></legend>
                            <asp:Repeater ID="Repeater_rptWeeklySaleWise4" runat="server" Visible="true" OnItemCreated="Repeater_rptWeeklySaleWise4_ItemCreated">
                                <HeaderTemplate>
                                    <table class="table" style="width: 100%">
                                        <tr>
                                            <td style="width: 15px; word-wrap: break-word">Sl</td>

                                            <td style="width: 150px; word-wrap: break-word">Lead Name </td>

                                            <td style="width: 100px; word-wrap: break-word">Location </td>

                                            <td style="width: 100px; word-wrap: break-word">Expected Volume </td>

                                            <td style="width: 40px; word-wrap: break-word">Activity Date </td>

                                            <td style="width: 300px; word-wrap: break-word">Activity Details </td>

                                            <td style="width: 100px; word-wrap: break-word" id="lblUName4" runat="server">Sales Person Name</td>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td width="15px">
                                            <%# Container.ItemIndex+1 %>
                                        </td>
                                        <td width="150px">
                                            <asp:Label ID="lblLeadName4" runat="server" Style="word-wrap: break-word; width: 150px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.Lead_Name")%></asp:Label>
                                        </td>
                                        <td width="100px">
                                            <asp:Label ID="lblLocation4" runat="server" Style="word-wrap: break-word; width: 100px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.Location")%></asp:Label>
                                        </td>
                                        <td width="100px">
                                            <asp:Label ID="lblVolumExpected4" runat="server" Style="word-wrap: break-word; width: 100px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.VolumeExpected")%></asp:Label>
                                        </td>
                                        <td width="40px">
                                            <asp:Label ID="lblActivityDt4" runat="server" Style="word-wrap: break-word; width: 40px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.ACTIVITY_DATE")%></asp:Label>
                                        </td>
                                        <td width="300px">
                                            <asp:Label ID="lblActivity4" runat="server" Style="word-wrap: break-word; width: 300px; white-space: normal;">
                                <%#DataBinder.Eval(Container,"DataItem.ACTIVITY_DETAILS")%></asp:Label>
                                        </td>
                                        <td width="100px" id="lblName4" runat="server">
                                            <asp:Label ID="lblUser4" runat="server" Style="word-wrap: break-word; width: 300px; white-space: normal;">
                                            <%#DataBinder.Eval(Container,"DataItem.sName")%></asp:Label>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <tr>
                                <td>
                                    <asp:Label ID="lblErrMsg4" runat="server"></asp:Label>
                                </td>
                            </tr>

                            <div class="m clear"></div>
                            <legend><b>Current Status :
                    <asp:Label ID="lblCurrentStatus5" runat="server"></asp:Label></b></legend>
                            <asp:Repeater ID="Repeater_rptWeeklySaleWise5" runat="server" Visible="true" OnItemCreated="Repeater_rptWeeklySaleWise5_ItemCreated">
                                <HeaderTemplate>
                                    <table class="table" style="width: 100%">
                                        <tr>
                                            <td style="width: 15px; word-wrap:break-word">Sl</td>

                                            <td style="width: 150px; word-wrap: break-word">Lead Name </td>

                                            <td style="width: 100px; word-wrap: break-word">Location </td>

                                            <td style="width: 100px; word-wrap: break-word">Expected Volume </td>

                                            <td style="width: 40px; word-wrap: break-word">Activity Date </td>

                                            <td style="width: 300px; word-wrap: break-word">Activity Details </td>

                                            <td style="width: 100px; word-wrap: break-word" id="lblUName5" runat="server">Sales Person Name</td>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td width="15px">
                                            <%# Container.ItemIndex+1 %>
                                        </td>
                                        <td width="150px">
                                            <asp:Label ID="lblLeadName5" runat="server" Style="word-wrap: break-word; width: 150px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.Lead_Name")%></asp:Label>
                                        </td>
                                        <td width="100px">
                                            <asp:Label ID="lblLocation5" runat="server" Style="word-wrap: break-word; width: 100px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.Location")%></asp:Label>
                                        </td>
                                        <td width="100px">
                                            <asp:Label ID="lblVolumExpected5" runat="server" Style="word-wrap: break-word; width: 100px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.VolumeExpected")%></asp:Label>
                                        </td>
                                        <td width="40px">
                                            <asp:Label ID="lblActivityDt5" runat="server" Style="word-wrap: break-word; width: 40px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.ACTIVITY_DATE")%></asp:Label>
                                        </td>
                                        <td width="300px">
                                            <asp:Label ID="lblActivity5" runat="server" Style="word-wrap: break-word; width: 300px; white-space: normal;">
                                <%#DataBinder.Eval(Container,"DataItem.ACTIVITY_DETAILS")%></asp:Label>
                                        </td>
                                        <td width="100px" id="lblName5" runat="server">
                                            <asp:Label ID="lblUser5" runat="server" Style="word-wrap: break-word; width: 300px; white-space: normal;">
                                            <%#DataBinder.Eval(Container,"DataItem.sName")%></asp:Label>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <tr>
                                <td>
                                    <asp:Label ID="lblErrMsg5" runat="server"></asp:Label>
                                </td>
                            </tr>

                            <div class="m clear"></div>
                            <legend><b>Weekly Visit Summary </b></legend>
                    <asp:Label ID="lblCurrentStatus6" runat="server"></asp:Label>
                            <asp:Repeater ID="Repeater_rptWeeklySaleWise6" runat="server" Visible="true">
                                <HeaderTemplate>
                                    <table class="table" style="width: 100%">
                                        <tr>
                                            <td style="width: 100px; word-wrap: break-word">Lead Ref No </td>

                                            <td style="width: 150px; word-wrap: break-word">Company </td>

                                            <td style="width: 100px; word-wrap: break-word">Visit Date </td>

                                            <td style="width: 100px; word-wrap: break-word">Visit Category </td>

                                            <td style="width: 400px; word-wrap: break-word">Remark </td>

                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td width="100px">
                                            <asp:Label ID="lblLeadName5" runat="server" Style="word-wrap: break-word; width: 100px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.LeadRefNo")%></asp:Label>
                                        </td>
                                        <td width="150px">
                                            <asp:Label ID="lblLocation5" runat="server" Style="word-wrap: break-word; width: 150px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.Company")%></asp:Label>
                                        </td>
                                        <td width="100px">
                                            <asp:Label ID="lblVolumExpected5" runat="server" Style="word-wrap: break-word; width: 100px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.VisitDate")%></asp:Label>
                                        </td>
                                        <td width="100px">
                                            <asp:Label ID="lblActivityDt5" runat="server" Style="word-wrap: break-word; width: 100px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.CategoryName")%></asp:Label>
                                        </td>
                                        <td width="400px">
                                            <asp:Label ID="lblActivity5" runat="server" Style="word-wrap: break-word; width: 300px; white-space: normal;">
                                <%#DataBinder.Eval(Container,"DataItem.Remark")%></asp:Label>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <tr>
                                <td>
                                    <asp:Label ID="lblErrMsg6" runat="server"></asp:Label>
                                </td>
                            </tr>

                            <div class="m clear"></div>
                            <legend><b>No Updates Since last 1 Month </b></legend>
                    <asp:Label ID="lblCurrentStatus7" runat="server"></asp:Label>
                            <asp:Repeater ID="Repeater_rptWeeklySaleWise7" runat="server" Visible="true">
                                <HeaderTemplate>
                                    <table class="table" style="width: 100%">
                                        <tr>
                                            <td style="width: 100px; word-wrap: break-word">Lead Ref No </td>

                                            <td style="width: 100px; word-wrap: break-word">Quote Status </td>

                                            <td style="width: 150px; word-wrap: break-word">Company </td>

                                            <td style="width: 150px; word-wrap: break-word">Services </td>

                                            <td style="width: 100px; word-wrap: break-word">Created Date </td>

                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td width="100px">
                                            <asp:Label ID="lblLeadName5" runat="server" Style="word-wrap: break-word; width: 100px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.LeadRefNo")%></asp:Label>
                                        </td>
                                        <td width="100px">
                                            <asp:Label ID="lblLocation5" runat="server" Style="word-wrap: break-word; width: 100px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.QuotationStatus")%></asp:Label>
                                        </td>
                                        <td width="150px">
                                            <asp:Label ID="lblVolumExpected5" runat="server" Style="word-wrap: break-word; width: 150px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.CompanyName")%></asp:Label>
                                        </td>
                                        <td width="150px">
                                            <asp:Label ID="lblActivityDt5" runat="server" Style="word-wrap: break-word; width: 150px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.Services")%></asp:Label>
                                        </td>
                                        <td width="100px">
                                            <asp:Label ID="lblActivity5" runat="server" Style="word-wrap: break-word; width: 100px; white-space: normal;">
                                <%#DataBinder.Eval(Container,"DataItem.CreatedDate")%></asp:Label>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <tr>
                                <td>
                                    <asp:Label ID="lblErrMsg7" runat="server"></asp:Label>
                                </td>
                            </tr>

                            <div class="m clear"></div>
                            <legend><b>Onboarded Customer </b></legend>
                            <asp:Label ID="lblCurrentStatus8" runat="server"></asp:Label>
                            <asp:GridView ID="gvCustomerOnBoard" runat="server" AutoGenerateColumns="False" 
                                DataKeyNames="lid" AllowSorting="True" CssClass="table">
                                <Columns>
                                    <asp:BoundField DataField="LeadRefNo" HeaderText="LeadRefNo" SortExpression="Customer" ReadOnly="true" />
                                    <asp:BoundField DataField="Customer" HeaderText="Company" SortExpression="Customer" ReadOnly="true" />
                                    <asp:BoundField DataField="Services" HeaderText="Services" SortExpression="ReferredBy" ReadOnly="true" />
                                    <asp:BoundField DataField="VolumeExpected" HeaderText="Volume Expected" SortExpression="" ReadOnly="true" />
                                    <asp:BoundField DataField="CreatedDate" HeaderText="On Board Date" SortExpression="LeadDate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                                    <asp:BoundField DataField="KYCDate" HeaderText="KYCDate" SortExpression="" ReadOnly="true" />

                                </Columns>
                            </asp:GridView>

                            <div class="m clear"></div>
                            <legend><b>Freight Summary </b></legend>   <%--DataSourceID="DataSourceFreightSummary"--%>
                            <asp:Label ID="lblCurrentStatus13" runat="server"></asp:Label>
                            <asp:GridView ID="gvSummaryFreight" runat="server" CssClass="table" Width="99%" AutoGenerateColumns="false"
                            AllowSorting="true" AllowPaging="true" PageSize="20" PagerStyle-CssClass="pgr" PagerSettings-Position="TopAndBottom">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="sName" HeaderText="Name" SortExpression="" ReadOnly="true" />
                                <asp:BoundField DataField="Enquiry" HeaderText="Enquiry" SortExpression="" ReadOnly="true" />
                                <asp:BoundField DataField="Quoted" HeaderText="Quoted" SortExpression="" ReadOnly="true" />
                                <asp:BoundField DataField="Awarded" HeaderText="Awarded" SortExpression="" ReadOnly="true" />
                                <asp:BoundField DataField="Lost" HeaderText="Lost" SortExpression="" ReadOnly="true" />
                                <asp:BoundField DataField="Executed" HeaderText="Executed" SortExpression="" ReadOnly="true" />
                                <asp:BoundField DataField="Budgetary" HeaderText="Budgetary" SortExpression="" ReadOnly="true" />
                                <asp:BoundField DataField="Lead" HeaderText="Lead" SortExpression="" ReadOnly="true" />
                            </Columns>
                        </asp:GridView>

                        <div>
                            <asp:SqlDataSource ID="DataSourceFreightSummary" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="FRSummaryForCRMReport" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                    <asp:ControlParameter ControlID="txtStartDate" Name="StartDate" PropertyName="Text" Type="String"  />
                                    <asp:ControlParameter ControlID="txtEndDate" Name="EndDate" PropertyName="Text" Type="String"  />
                                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>


                            <div class="m clear"></div>
                            <legend><b>Business Volume Analysis <br />
                                Import CHA </b></legend>
                            <asp:Label ID="lblCurrentStatus9" runat="server"></asp:Label>
                            <asp:GridView ID="gvImport" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr" 
                                CssClass="table" EmptyDataRowStyle-Font-Bold="true" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Customer" HeaderText="Customer Name" SortExpression="Customer" ReadOnly="true" />
                                    <asp:BoundField DataField="NoofJobsAir" HeaderText="No of Jobs" SortExpression="NoofJobsAir" ReadOnly="true" />
                                    <asp:BoundField DataField="NOOFJOBSFCL" HeaderText="FCL Jobs" SortExpression="NOOFJOBSFCL" ReadOnly="true" />
                                    <asp:BoundField DataField="NOOFJOBSLCL" HeaderText="LCL Jobs" SortExpression="NOOFJOBSLCL" ReadOnly="true" />
                                    <asp:BoundField DataField="NOOFCONT20" HeaderText="Cont 20" SortExpression="NOOFCONT20" ReadOnly="true" />
                                    <asp:BoundField DataField="NOOFCONT40" HeaderText="Cont 40" SortExpression="NOOFCONT40" ReadOnly="true" />
                                    <asp:BoundField DataField="TEU" HeaderText="TEU" SortExpression="TEU" ReadOnly="true" />
                                    <asp:BoundField DataField="GrossWeight" HeaderText="Gross WT" SortExpression="GrossWeight" ReadOnly="true" />
                                    <asp:BoundField DataField="NoOfPKGS" HeaderText="Total Pkg" SortExpression="NoOfPKGS" ReadOnly="true" />
                                    <asp:BoundField DataField="CreatedBy" HeaderText="Referred By" SortExpression="CreatedBy" ReadOnly="true" />
                                </Columns>
                            </asp:GridView>
                            
                            <div class="m clear"></div>
                            <legend><b>Export CHA </b></legend>
                            <asp:Label ID="lblCurrentStatus12" runat="server"></asp:Label>
                            <asp:GridView ID="gvExport" runat="server" AutoGenerateColumns="False" Width="100%" 
                                DataKeyNames="CustomerId" CssClass="table" EmptyDataText="No Data Found">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Customer" HeaderText="Customer Name" SortExpression="Customer" ReadOnly="True" />
                                    <asp:BoundField DataField="NoofJobsAir" HeaderText="No of Jobs" SortExpression="NoofJobsAir" ReadOnly="True" />
                                    <asp:BoundField DataField="NOOFJOBSFCL" HeaderText="FCL Jobs" SortExpression="NOOFJOBSFCL" ReadOnly="True" />
                                    <asp:BoundField DataField="NOOFJOBSLCL" HeaderText="LCL Jobs" SortExpression="NOOFJOBSLCL" ReadOnly="True" />
                                    <asp:BoundField DataField="NOOFCONT20" HeaderText="Cont 20" SortExpression="NOOFCONT20" ReadOnly="True" />
                                    <asp:BoundField DataField="NOOFCONT40" HeaderText="Cont 40" SortExpression="NOOFCONT40" ReadOnly="True" />
                                    <asp:BoundField DataField="TEU" HeaderText="TEU" SortExpression="TEU" ReadOnly="True" />
                                    <asp:BoundField DataField="GrossWeight" HeaderText="Gross WT" SortExpression="GrossWeight" ReadOnly="True" />
                                    <asp:BoundField DataField="NoOfPKGS" HeaderText="Total Pkg" SortExpression="NoOfPKGS" ReadOnly="True" />
                                    <asp:BoundField DataField="CreatedBy" HeaderText="Referred By" SortExpression="CreatedBy" ReadOnly="True" />
                                </Columns>
                                <EmptyDataRowStyle Font-Bold="True" ForeColor="Red" HorizontalAlign="Center" />
                                <PagerStyle CssClass="pgr" />
                            </asp:GridView>


                            <div class="m clear"></div>
                            <legend><b>Freight Forwarding </b></legend>
                            <asp:Label ID="lblCurrentStatus10" runat="server"></asp:Label>
                            <asp:GridView ID="gvFreight" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table" >
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Customer" HeaderText="Customer Name" SortExpression="Customer" ReadOnly="True" />
                                    <asp:BoundField DataField="NoofJobsAir" HeaderText="No of Jobs" SortExpression="NoofJobsAir" ReadOnly="True" />
                                    <asp:BoundField DataField="NOOFJOBSFCL" HeaderText="FCL Jobs" SortExpression="NOOFJOBSFCL" ReadOnly="True" />
                                    <asp:BoundField DataField="NOOFJOBSLCL" HeaderText="LCL Jobs" SortExpression="NOOFJOBSLCL" ReadOnly="True" />
                                    <asp:BoundField DataField="NOOFCONT20" HeaderText="Cont 20" SortExpression="NOOFCONT20" ReadOnly="True" />
                                    <asp:BoundField DataField="NOOFCONT40" HeaderText="Cont 40" SortExpression="NOOFCONT40" ReadOnly="True" />
                                    <asp:BoundField DataField="TEU" HeaderText="TEU" SortExpression="TEU" ReadOnly="True" />
                                    <asp:BoundField DataField="GrossWeight" HeaderText="Gross WT" SortExpression="GrossWeight" ReadOnly="True" />
                                    <asp:BoundField DataField="NoOfPKGS" HeaderText="Total Pkg" SortExpression="NoOfPKGS" ReadOnly="True" />
                                    <asp:BoundField DataField="CreatedBy" HeaderText="Referred By" SortExpression="CreatedBy" ReadOnly="True" />
                                </Columns>
                                <EmptyDataRowStyle Font-Bold="True" ForeColor="Red" HorizontalAlign="Center" />
                                <PagerStyle CssClass="pgr" />
                            </asp:GridView>

                            <div class="m clear"></div>
                            <legend><b>Loss Business – No Operation since last 2 months </b></legend>
                            <asp:Label ID="lblCurrentStatus11" runat="server"></asp:Label>
                            <asp:Repeater ID="Repeater_rptWeeklySaleWise8" runat="server" Visible="true">
                                <HeaderTemplate>
                                    <table class="table" style="width: 100%">
                                        <tr>
                                            <td style="width: 100px; word-wrap: break-word">CompanyName </td>

                                            <td style="width: 100px; word-wrap: break-word">Month Of Last Operation</td>

                                            <td style="width: 150px; word-wrap: break-word">Module </td>

                                            <td style="width: 100px; word-wrap: break-word">Package </td>

                                            <td style="width: 100px; word-wrap: break-word">TEU </td>

                                            <td style="width: 100px; word-wrap: break-word">Gross Weight </td>

                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td width="100px">
                                            <asp:Label ID="lblLeadName5" runat="server" Style="word-wrap: break-word; width: 100px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.CUSTNAME")%></asp:Label>
                                        </td>
                                        <td width="100px">
                                            <asp:Label ID="lblLocation5" runat="server" Style="word-wrap: break-word; width: 150px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.MonthOfLastOperation")%></asp:Label>
                                        </td>
                                        <td width="150px">
                                            <asp:Label ID="lblVolumExpected5" runat="server" Style="word-wrap: break-word; width: 150px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.ModuleType")%></asp:Label>
                                        </td>
                                         <td width="100px">
                                            <asp:Label ID="lblPackage" runat="server" Style="word-wrap: break-word; width: 100px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.Package")%></asp:Label>
                                        </td>
                                         <td width="100px">
                                            <asp:Label ID="lblTEU" runat="server" Style="word-wrap: break-word; width: 100px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.TEU")%></asp:Label>
                                        </td>
                                        <td width="100px">
                                            <asp:Label ID="Label2" runat="server" Style="word-wrap: break-word; width: 100px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.GrossWT")%></asp:Label>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </div>
                    </asp:Panel>
                </div>
            </fieldset>
            <%--<div>
                <asp:SqlDataSource ID="DataSourceCustomerOnBoard" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRMGetCustomerOnBoard" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        <asp:ControlParameter ControlID="ddlUser" PropertyName="SelectedValue" Name="SalesPersonId" DefaultValue="0" />
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <%--<asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        <asp:ControlParameter ControlID="hdnCustId" PropertyName="Value" Name="CompanyId" DefaultValue="0" />--%
                        
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>--%>



        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

