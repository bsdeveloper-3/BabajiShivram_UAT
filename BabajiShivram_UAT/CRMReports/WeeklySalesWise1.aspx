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
                        </div>
                    </asp:Panel>
                </div>
            </fieldset>




        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

