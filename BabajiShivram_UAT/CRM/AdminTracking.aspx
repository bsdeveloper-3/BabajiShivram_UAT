<%@ Page Title="Admin Tracking" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AdminTracking.aspx.cs"
    Inherits="CRM_AdminTracking" Culture="en-GB" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" %>

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
    <script type="text/javascript">
        function GridSelectAllColumn(spanChk) {
            var oItem = spanChk.children;
            var theBox = (spanChk.type == "checkbox") ? spanChk : spanChk.children.item[0]; xState = theBox.checked;
            elm = theBox.form.elements;

            for (i = 0; i < elm.length; i++) {
                if (elm[i].type === 'checkbox' && elm[i].checked != xState)
                    elm[i].click();
            }
        }
    </script>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlLeads" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../Images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upnlLeads" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="vsRejectContract" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="vgRejectContract" CssClass="errorMsg" />
            </div>
            <div class="clear">
            </div>
            <div class="clear"></div>
            <fieldset class="fieldset-AutoWidth">
                <legend>Admin Tracking</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                    </asp:Panel>
                    <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
                        <asp:Image ID="imgExcel" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                </div>
                </div>
                <div class="clear">
                </div>
                <asp:GridView ID="gvLeads" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr" DataSourceID="DataSourceLeads"
                    DataKeyNames="lid" OnRowCommand="gvLeads_RowCommand" AllowPaging="True" AllowSorting="True" CssClass="table" PageSize="20"
                    PagerSettings-Position="TopAndBottom" OnRowDataBound="gvLeads_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Lead Ref No">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkLead" runat="server" ToolTip="Click to view lead." Text='<%#Eval("LeadRefNo") %>'
                                    CommandName="viewlead" CommandArgument='<%#Eval("lid")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="QuotationStatus" HeaderText="Quote Status" ReadOnly="true" Visible="false" />
                        <asp:BoundField DataField="LeadRefNo" HeaderText="Lead Ref No" ReadOnly="true" Visible="false" />
                        <asp:BoundField DataField="LeadStageName" HeaderText="Status" SortExpression="LeadStageName" ReadOnly="true" />
                        <asp:BoundField DataField="CompanyName" HeaderText="Company" SortExpression="CompanyName" ReadOnly="true" />
                        <asp:BoundField DataField="Services" HeaderText="Services" SortExpression="Services" ReadOnly="true" />
                        <asp:BoundField DataField="LeadSourceName" HeaderText="Source" SortExpression="LeadSourceName" ReadOnly="true" />
                        <asp:BoundField DataField="ContactName" HeaderText="Contact Name" SortExpression="ContactName" ReadOnly="true" />
                        <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" ReadOnly="true" />
                        <asp:BoundField DataField="MobileNo" HeaderText="Mobile No" SortExpression="MobileNo" ReadOnly="true" />
                        <asp:BoundField DataField="LeadCreatedBy" HeaderText="Lead Owner" SortExpression="LeadCreatedBy" ReadOnly="true" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="CreatedDate" ReadOnly="true" />
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="DataSourceLeads" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_GetLeadForTracking" SelectCommandType="StoredProcedure"><%--CRM_GetLeadTracking--%>
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

