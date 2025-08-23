<%@ Page Title="Contract Secured" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ContractSecured.aspx.cs"
    Inherits="CRMReports_ContractSecured" Culture="en-GB" %>

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
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlSPWiseLeadStatus" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../Images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upnlSPWiseLeadStatus" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <fieldset>
                <div class="m clear">
                    <div class="fright" style="margin-right: 5px;">
                        &nbsp;
                        <asp:DropDownList ID="ddlUser" runat="server" DataSourceID="DataSourceSalesPerson" DataTextField="sName"
                            DataValueField="lid" AppendDataBoundItems="true" TabIndex="2" Width="250px" AutoPostBack="true" OnSelectedIndexChanged="ddlUser_SelectedIndexChanged">
                            <asp:ListItem Text="- Select Sales Person -" Value="0" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <legend>Contracts Secured</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                        </div>
                        <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
                            <asp:Image ID="imgExcel" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                        </asp:LinkButton>
                    </asp:Panel>
                </div>
                <div class="clear">
                </div>
                <asp:GridView ID="gvLeads" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr" DataSourceID="DataSourceLeads"
                    DataKeyNames="Year" CssClass="table" OnRowCommand="gvLeads_RowCommand" EmptyDataText="No data found!" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Year" HeaderText="Year" ReadOnly="true" />
                        <asp:TemplateField HeaderText="January">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnJanuary" runat="server" Text='<%#Eval("January") %>' Font-Underline="false" CommandName="Month1" CommandArgument='<%#Eval("Year") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="February">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnFebruary" runat="server" Text='<%#Eval("February") %>' Font-Underline="false" CommandName="Month2" CommandArgument='<%#Eval("Year") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="March">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnMarch" runat="server" Text='<%#Eval("March") %>' Font-Underline="false" CommandName="Month3" CommandArgument='<%#Eval("Year") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="April">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnApril" runat="server" Text='<%#Eval("April") %>' Font-Underline="false" CommandName="Month4" CommandArgument='<%#Eval("Year") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="May">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnMay" runat="server" Text='<%#Eval("May") %>' Font-Underline="false" CommandName="Month5" CommandArgument='<%#Eval("Year") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="June">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnJune" runat="server" Text='<%#Eval("June") %>' Font-Underline="false" CommandName="Month6" CommandArgument='<%#Eval("Year") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="July">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnJuly" runat="server" Text='<%#Eval("July") %>' Font-Underline="false" CommandName="Month7" CommandArgument='<%#Eval("Year") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="August">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnAugust" runat="server" Text='<%#Eval("August") %>' Font-Underline="false" CommandName="Month8" CommandArgument='<%#Eval("Year") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="September">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnSeptember" runat="server" Text='<%#Eval("September") %>' Font-Underline="false" CommandName="Month9" CommandArgument='<%#Eval("Year") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="October">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnOctober" runat="server" Text='<%#Eval("October") %>' Font-Underline="false" CommandName="Month10" CommandArgument='<%#Eval("Year") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="November">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnNovember" runat="server" Text='<%#Eval("November") %>' Font-Underline="false" CommandName="Month11" CommandArgument='<%#Eval("Year") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="December">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnDecember" runat="server" Text='<%#Eval("December") %>' Font-Underline="false" CommandName="Month12" CommandArgument='<%#Eval("Year") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="DataSourceLeads" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_rptContractSecured" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlUser" PropertyName="SelectedValue" Name="UserId" DefaultValue="0" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceSalesPerson" runat="server" ConnectionString="<%$ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_GetSalesPerson" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>

            <div>
                <!-- Status of Lead-->
                <cc1:ModalPopupExtender ID="mpeLeadStatus" runat="server" CacheDynamicResults="false"
                    PopupControlID="pnlLeadStatus" TargetControlID="lnkDummy" BackgroundCssClass="modalBackground" DropShadow="true">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pnlLeadStatus" runat="server" CssClass="ModalPopupPanel" Width="1300px">
                    <div class="header">
                        <div class="fleft">
                            <asp:Label ID="lblStatusTitle" runat="server"></asp:Label>
                            &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkExport_LeadDetail" runat="server" OnClick="lnkExport_LeadDetail_Click">
                            <asp:Image ID="imgExpLeadDetail" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                        </asp:LinkButton>
                        </div>
                        <div class="fright">
                            <asp:ImageButton ID="imgClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgClose_Click"
                                ToolTip="Close" />
                        </div>
                    </div>
                    <div class="m">
                    </div>

                    <div id="Div2" runat="server" style="width: 1280px; height: 500px; overflow: auto; padding: 5px">
                        <asp:HiddenField ID="hdnQuotationId" runat="server" />
                        <div align="center">
                            <asp:Label ID="lbError_Popup" runat="server" Visible="true"></asp:Label>
                            <asp:HiddenField ID="hdnMonthId" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnYear" runat="server" Value="0" />
                        </div>
                        <div>
                            <asp:GridView ID="gvLeadDetail" runat="server" CssClass="table" AutoGenerateColumns="false" DataSourceID="DataSourceLeadDetail" Style="white-space: initial">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Customer" HeaderText="Customer" />
                                    <asp:BoundField DataField="ContactPerson" HeaderText="Contact Person" />
                                    <asp:BoundField DataField="Email" HeaderText="Email" />
                                    <asp:BoundField DataField="MobileNo" HeaderText="Mobile No" />
                                    <asp:BoundField DataField="Address" HeaderText="Address" />
                                    <asp:BoundField DataField="CustCode" HeaderText="Cust Code" />
                                    <asp:BoundField DataField="ReferredBy" HeaderText="Referred By" />
                                    <asp:BoundField DataField="IECNo" HeaderText="IEC No" Visible="false" />
                                    <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" ItemStyle-Width="10%" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>
                <div>
                    <asp:LinkButton ID="lnkDummy" runat="server" Text=""></asp:LinkButton>
                    <asp:SqlDataSource ID="DataSourceLeadDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="CRM_rptContractSecuredDetail" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter Name="Year" ControlID="hdnYear" PropertyName="Value" DbType="Int32" DefaultValue="0" />
                            <asp:ControlParameter Name="MonthId" ControlID="hdnMonthId" PropertyName="Value" DbType="Int32" DefaultValue="0" />
                            <asp:ControlParameter ControlID="ddlUser" PropertyName="SelectedValue" Name="UserId" DefaultValue="0" />
                            <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" DbType="Int32" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
                <!-- Status of Lead-->
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

