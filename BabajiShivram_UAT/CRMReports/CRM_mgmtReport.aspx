<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CRM_mgmtReport.aspx.cs"
    Inherits="CRMReports_CRM_mgmtReport" Culture="en-GB" %>

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
        <asp:UpdateProgress ID="uprogress" runat="server" AssociatedUpdatePanelID="upnlMGMTReport">
            <ProgressTemplate>
                <img alt="progress" src="../Images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upnlMGMTReport" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            </div>

            <fieldset>
                <legend>Management Summary Report</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="m clear">
                            <table>
                                <tr>
                                    <td class="fleft" style="text-align: left; margin-right: 5px;">Sales Person
                                        <asp:DropDownList ID="ddlUser" runat="server" DataSourceID="DataSourceSalesPerson" DataTextField="sName"
                                            DataValueField="lid" AppendDataBoundItems="true" TabIndex="2" Width="250px" AutoPostBack="true"
                                            Style="margin-left: 12px;" OnSelectedIndexChanged="ddlUser_SelectedIndexChanged">
                                            <asp:ListItem Text="- Select Sales Person -" Value="0" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <div class="m clear" style="text-align: left; margin-right: 5px;">
                                            Date Between
                                            <cc1:CalendarExtender ID="calStartDate" runat="server" FirstDayOfWeek="Sunday"
                                                Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtStartDate">
                                            </cc1:CalendarExtender>
                                            <cc1:MaskedEditExtender ID="meeStartDate" TargetControlID="txtStartDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                MaskType="Date" AutoComplete="false" runat="server">
                                            </cc1:MaskedEditExtender>
                                            <cc1:MaskedEditValidator ID="mevStartDate" ControlExtender="meeStartDate" ControlToValidate="txtStartDate" IsValidEmpty="false"
                                                InvalidValueMessage="Start Date is invalid" InvalidValueBlurredMessage="Invalid Start Date" SetFocusOnError="true"
                                                MinimumValueMessage="Invalid Start Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2016" MaximumValue="31/12/2025"
                                                runat="Server" ValidationGroup="vgVisitReport">
                                            </cc1:MaskedEditValidator>
                                            &nbsp;&nbsp;
                                            <asp:TextBox ID="txtStartDate" runat="server" Width="80px" placeholder="dd/mm/yyyy" TabIndex="1" ToolTip="Select Start Date." AutoPostBack="true" OnTextChanged="txtStartDate_TextChanged"></asp:TextBox>
                                                            &nbsp;&nbsp;To
                                            <cc1:CalendarExtender ID="calEndDate" runat="server" FirstDayOfWeek="Sunday"
                                                Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtEndDate">
                                            </cc1:CalendarExtender>
                                                            <cc1:MaskedEditExtender ID="meeEndDate" TargetControlID="txtEndDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                MaskType="Date" AutoComplete="false" runat="server">
                                            </cc1:MaskedEditExtender>
                                            <cc1:MaskedEditValidator ID="mevEndDate" ControlExtender="meeEndDate" ControlToValidate="txtEndDate" IsValidEmpty="false"
                                                InvalidValueMessage="End Date is invalid" InvalidValueBlurredMessage="Invalid End Date" SetFocusOnError="true"
                                                MinimumValueMessage="Invalid End Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2016" MaximumValue="31/12/2025"
                                                runat="Server" ValidationGroup="vgVisitReport">
                                            </cc1:MaskedEditValidator>
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
                    </asp:Panel>
                </div>

                <div id="dvsalePerson" runat="server" class="m clear">
                    <asp:Panel ID="Panel1" runat="server">
                        <legend>Sales Person :  
                            <asp:Label ID="lblSalePerson" runat="server"></asp:Label>
                        </legend>

                        <asp:SqlDataSource ID="DataSourceSalesPerson" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                            SelectCommand="CRM_GetSalesPerson" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                        <div class="m clear"></div>

                        <div id="Div1" runat="server">
                            <asp:Panel ID="lblUserSummary" runat="server">

                                <asp:GridView ID="gvSummaryCount" runat="server" AutoGenerateColumns="false" DataSourceID="DataSourceSummaryCount"
                                    CssClass="table" Width="50" Visible="false">
                                    <Columns>
                                        <asp:BoundField DataField="LEAD_COUNT" HeaderText="LEAD_COUNT" ReadOnly="true" />
                                        <asp:BoundField DataField="LEAD_APPROVED" HeaderText="LEAD_APPROVED" ReadOnly="true" />
                                        <asp:BoundField DataField="PENDING_QUOTE" HeaderText="PENDING_QUOTE" ReadOnly="true" />
                                        <asp:BoundField DataField="CONTRACT_APPOVAL" HeaderText="CONTRACT_APPOVAL" ReadOnly="true" />
                                        <%--<asp:BoundField DataField="" HeaderText="" ReadOnly="true" />--%>
                                    </Columns>
                                </asp:GridView>

                                <asp:SqlDataSource ID="DataSourceSummaryCount" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                                SelectCommand="CRM_SummaryCount" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:ControlParameter Name="SalePersonId" PropertyName="SelectedValue" ControlID="ddlUser" DefaultValue="0" />
                                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </asp:Panel>
                        </div>

                        <div id="dvtable" runat="server">
                            <legend>
                                <b><asp:Label ID="lblLead" runat="server"></asp:Label></b></legend>

                            <asp:Repeater ID="Repeater_Lead" runat="server" Visible="true" OnItemCreated="Repeater_Lead_ItemCreated">
                                <HeaderTemplate>
                                    <table class="table" style="width: 100%">
                                        <tr>
                                            <td style="width: 15px; word-wrap: break-word">Sl</td>

                                            <td style="width: 150px; word-wrap: break-word">Lead Name </td>

                                            <td style="width: 100px; word-wrap: break-word">Service </td>

                                            <td style="width: 100px; word-wrap: break-word">Latest Status </td>

                                            <td style="width: 40px; word-wrap: break-word">Expected Volume</td>

                                            <td style="width: 200px; word-wrap: break-word">Latest Status Remark</td>

                                            <%--<td style="width: 300px; word-wrap: break-word">Activity Details </td>

                                            <td id="lblUName1" style="width: 100px; word-wrap: break-word" runat="server">Sales Person Name</td>--%>
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
                                    <%#DataBinder.Eval(Container,"DataItem.Services")%></asp:Label>
                                        </td>
                                        <td width="100px">
                                            <asp:Label ID="lblVolumExpected" runat="server" Style="word-wrap: break-word; width: 100px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.LeadStatus")%></asp:Label>
                                        </td>
                                        <td width="40px">
                                            <asp:Label ID="lblActivityDt" runat="server" Style="word-wrap: break-word; width: 40px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.Expected_Volume")%></asp:Label>
                                        </td>
                                        <td width="200px">
                                            <asp:Label ID="Label3" runat="server" Style="word-wrap: break-word; width: 100px; white-space: normal;">
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
                                    <asp:Label ID="lblErrMsg1" runat="server"></asp:Label>
                                    <br />
                                </td>
                            </tr>
                            <legend>
                                <b><asp:Label ID="lblLeadApproved" runat="server"></asp:Label></b>
                            </legend>
                            <asp:Repeater ID="Repeater_LeadApproved" runat="server" Visible="true" OnItemCreated="Repeater_LeadApproved_ItemCreated">
                                <HeaderTemplate>
                                    <table class="table" style="width: 100%">
                                        <tr>
                                            <td style="width: 15px; word-wrap: break-word">Sl</td>
                                            <td style="width: 150px; word-wrap: break-word">Lead Name </td>
                                            <td style="width: 100px; word-wrap: break-word">Service </td>
                                            <td style="width: 1px; word-wrap: break-word">Target Month </td>
                                            <td style="width: 100px; word-wrap: break-word">Latest Status </td>
                                            <td style="width: 200px; word-wrap: break-word">Latest Status Remark </td>
                                            <td style="width: 400px; word-wrap: break-word">Last Visit Remark</td>
                                            <td style="width: 5px; word-wrap: break-word">Last Visit Date </td>
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
                                            <asp:Label ID="lblLocation" runat="server" Style="word-wrap: break-word; width: 100px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.Services")%></asp:Label>
                                        </td>
                                        <td width="1px">
                                            <asp:Label ID="lblVolumExpected" runat="server" Style="word-wrap: break-word; width: 100px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.TARGET_MON")%></asp:Label>
                                        </td>
                                        <td width="100px">
                                            <asp:Label ID="Label4" runat="server" Style="word-wrap: break-word; width: 40px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.LeadStatus")%></asp:Label>
                                        </td>
                                        <td width="200px">
                                            <asp:Label ID="lblActivityDt" runat="server" Style="word-wrap: break-word; width: 40px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.Remark")%></asp:Label>
                                        </td>
                                        <td width="400px">
                                            <asp:Label ID="Label1" runat="server" Style="word-wrap: break-word; width: 100px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.VISIT_REMARK")%></asp:Label>
                                        </td>
                                        <td width="5px">
                                            <asp:Label ID="Label2" runat="server" Style="word-wrap: break-word; width: 40px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.VisitDate")%></asp:Label>
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

                            <div class="m clear" ></div>
                            <legend><b><asp:Label ID="lblQuote" runat="server"></asp:Label></b></legend>
                            <asp:Repeater ID="Repeater_Quote" runat="server" Visible="true" OnItemCreated="Repeater_Quote_ItemCreated" >
                                <HeaderTemplate>
                                    <table class="table" style="width: 100%">
                                        <tr>
                                            <td style="width: 15px; word-wrap: break-word">Sl</td>
                                            <td style="width: 150px; word-wrap: break-word">Lead Name </td>
                                            <td style="width: 100px; word-wrap: break-word">Service </td>
                                            <td style="width: 1px; word-wrap: break-word">Quote Date </td>
                                            <td style="width: 100px; word-wrap: break-word">Quote Status </td>
                                            <td style="width: 200px; word-wrap: break-word">Latest Status Remark </td>
                                            <td style="width: 400px; word-wrap: break-word">Last Visit Remark</td>
                                            <td style="width: 5px; word-wrap: break-word">Last Visit Date </td>
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
                                            <asp:Label ID="lblLocation" runat="server" Style="word-wrap: break-word; width: 100px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.SERVICES")%></asp:Label>
                                        </td>
                                        <td width="1px">
                                            <asp:Label ID="lblQuoteDate" runat="server" Style="word-wrap: break-word; width: 100px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.dtDate")%></asp:Label>
                                        </td>
                                        <td width="100px">
                                            <asp:Label ID="lblQuoteStatus" runat="server" Style="word-wrap: break-word; width: 100px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.QuotationStatus")%></asp:Label>
                                        </td>
                                         <td width="200px">
                                            <asp:Label ID="Label5" runat="server" Style="word-wrap: break-word; width: 100px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.Remark")%></asp:Label>
                                        </td>
                                        <td width="400px">
                                            <asp:Label ID="Label1" runat="server" Style="word-wrap: break-word; width: 100px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.VISIT_REMARK")%></asp:Label>
                                        </td>
                                        <td width="5px">
                                            <asp:Label ID="Label2" runat="server" Style="word-wrap: break-word; width: 40px; white-space: normal;">
                                    <%#DataBinder.Eval(Container,"DataItem.VisitDate")%></asp:Label>
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
                        </div>

                    </asp:Panel>
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

