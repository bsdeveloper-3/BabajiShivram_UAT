<%@ Page Title="Weekly Visit Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="WeeklyVisit.aspx.cs"
    Inherits="CRMReports_WeeklyVisit" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
    </asp:ScriptManager>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlWeeklyVisit" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../Images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upnlWeeklyVisit" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:HiddenField ID="hdnCustId" runat="server" Value="0" />
            </div>
            <div class="m clear">
            </div>
            <div class="clear" style="text-align: center">
                From
                <cc1:calendarextender id="calStartDate" runat="server" firstdayofweek="Sunday"
                    format="dd/MM/yyyy" popupposition="BottomRight" targetcontrolid="txtStartDate">
                </cc1:calendarextender>
                <cc1:maskededitextender id="meeStartDate" targetcontrolid="txtStartDate" mask="99/99/9999" messagevalidatortip="true"
                    masktype="Date" autocomplete="false" runat="server">
                </cc1:maskededitextender>
                <cc1:maskededitvalidator id="mevStartDate" controlextender="meeStartDate" controltovalidate="txtStartDate" isvalidempty="false"
                    invalidvaluemessage="Start Date is invalid" invalidvalueblurredmessage="Invalid Start Date" setfocusonerror="true"
                    minimumvaluemessage="Invalid Start Date" maximumvaluemessage="Invalid Date" minimumvalue="01/01/2016" maximumvalue="31/12/2025"
                    runat="Server" validationgroup="vgVisitReport"></cc1:maskededitvalidator>
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
                    runat="Server" validationgroup="vgVisitReport"></cc1:maskededitvalidator>
                &nbsp;&nbsp;
                <asp:TextBox ID="txtEndDate" runat="server" Width="80px" placeholder="dd/mm/yyyy" TabIndex="2" ToolTip="Select End Date." AutoPostBack="true" OnTextChanged="txtEndDate_TextChanged"></asp:TextBox>

            </div>
            <fieldset>
                <legend>Weekly Visit</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:datafilter id="DataFilter1" runat="server" />
                        </div>
                        <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
                            <asp:Image ID="imgExcel" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                        </asp:LinkButton>
                        <div class="fright" style="margin-right: 10px">
                            <asp:CheckBox ID="chkIncludeVisitDate" runat="server" Checked="true" AutoPostBack="true" Text="Report Based On Visit Date" OnCheckedChanged="chkIncludeVisitDate_CheckedChanged" />
                        </div>
                    </asp:Panel>
                </div>
                <div class="clear">
                </div>
                <asp:GridView ID="gvVisitReport" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr" DataSourceID="DataSourceVisitReport"
                    DataKeyNames="lid" AllowPaging="True" AllowSorting="True" CssClass="table" PageSize="20" PagerSettings-Position="TopAndBottom" Style="white-space: inherit">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="LeadRefNo" HeaderText="Lead Ref No" ReadOnly="true" ItemStyle-Width="8%" />
                        <asp:BoundField DataField="Company" HeaderText="Company" ReadOnly="true" ItemStyle-Width="10%" />
                        <asp:BoundField DataField="VisitDate" HeaderText="Visit Date" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="5%" />
                        <asp:BoundField DataField="CategoryName" HeaderText="Visit Category" ReadOnly="true" />
                        <asp:BoundField DataField="Remark" HeaderText="Remark" ReadOnly="true" ItemStyle-Width="57%" />
                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" ReadOnly="true" ItemStyle-Width="10%" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="CreatedDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" ItemStyle-Width="10%" />
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
                <asp:SqlDataSource ID="DataSourceVisitReport" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_rptWeeklyVisit" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtStartDate" PropertyName="Text" Name="StartDate" Type="DateTime" />
                        <asp:ControlParameter ControlID="txtEndDate" PropertyName="Text" Name="EndDate" Type="DateTime" />
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        <asp:ControlParameter ControlID="chkIncludeVisitDate" PropertyName="Checked" Name="IsVisitReport" Type="Boolean" DefaultValue="True" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

