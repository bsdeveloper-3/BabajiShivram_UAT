<%@ Page Title="Rejected Leads" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Rejected.aspx.cs"
    Inherits="CRM_Rejected" Culture="en-GB" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" %>

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
                <asp:ValidationSummary ID="vsApproval" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="vgApproval" CssClass="errorMsg" />
            </div>
            <div class="clear">
            </div>
            <div class="clear"></div>
            <fieldset class="fieldset-AutoWidth">
                <legend>Rejected Leads</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:datafilter id="DataFilter1" runat="server" />
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
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkAction" runat="server" ToolTip="Click to view lead." Text="Send Approval"
                                    CommandName="sendapproval" CommandArgument='<%#Eval("lid") + ";" + Eval("LeadRefNo")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Lead Ref No">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkLead" runat="server" ToolTip="Click to view lead." Text='<%#Eval("LeadRefNo") %>'
                                    CommandName="viewlead" CommandArgument='<%#Eval("lid")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="LeadRefNo" HeaderText="Lead Ref No" ReadOnly="true" Visible="false" />
                        <%--<asp:BoundField DataField="QuotationStatus" HeaderText="Quote Status" ReadOnly="true" Visible="false" />--%>
                        <%--<asp:BoundField DataField="LeadStageName" HeaderText="Status" SortExpression="LeadStageName" ReadOnly="true" Visible="false" />--%>
                        <asp:BoundField DataField="CompanyName" HeaderText="Company" SortExpression="CompanyName" ReadOnly="true" />
                        <asp:TemplateField HeaderText="Rejection Remark" SortExpression="">
                            <ItemTemplate>
                                <div style="word-wrap: break-word; width: 200px; white-space:normal; color:red;">
                                    <asp:Label ID="lblRemark" runat="server" Text='<%#Bind("Remark") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Services" HeaderText="Product" SortExpression="Services" ReadOnly="true" />
                        <asp:BoundField DataField="LeadSourceName" HeaderText="Source" SortExpression="LeadSourceName" ReadOnly="true" />
                        <asp:BoundField DataField="ContactName" HeaderText="Contact Name" SortExpression="ContactName" ReadOnly="true" />
                        <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" ReadOnly="true" />
                        <asp:BoundField DataField="MobileNo" HeaderText="Mobile No" SortExpression="MobileNo" ReadOnly="true" />
                        <asp:BoundField DataField="LeadCreatedBy" HeaderText="Lead Owner" SortExpression="LeadCreatedBy" ReadOnly="true" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="CreatedDate" ReadOnly="true" />
                        <%--<asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="RejectionRemark" ReadOnly="true" />--%>
                        
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="DataSourceLeads" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_GetRejectedLeads" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>

            <!-- Update Status of Quotation-->
            <cc1:modalpopupextender id="mpeRemark" runat="server" cachedynamicresults="false"
                popupcontrolid="Panel2Document" targetcontrolid="lnkDummy" backgroundcssclass="modalBackground" dropshadow="true">
            </cc1:modalpopupextender>
            <asp:Panel ID="Panel2Document" runat="server" CssClass="ModalPopupPanel" Width="500px">
                <div class="header">
                    <div class="fleft">
                        Lead Re-approval for
                        <asp:Label ID="lblLeadRefNo" runat="server" Font-Bold="true"></asp:Label>
                    </div>
                    <div class="fright">
                        <asp:ImageButton ID="imgClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgClose_Click" ToolTip="Close" />
                    </div>
                </div>
                <div class="m">
                </div>

                <div id="Div2" runat="server" style="max-height: 200px; overflow: auto; padding: 5px">
                    <asp:HiddenField ID="hdnLeadId" runat="server" Value="0" />
                    <div align="center">
                        <asp:Label ID="lbError_Popup" runat="server" Visible="true"></asp:Label>
                    </div>
                    <div>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>Remark (if any)
                                    <asp:RequiredFieldValidator ID="rfvrem" runat="server" ControlToValidate="txtRemark" SetFocusOnError="true" Display="Dynamic"
                                        ErrorMessage="Enter Remark." Text="*" ValidationGroup="vgApproval"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Rows="3" Width="250px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="btnSendApproval" runat="server" OnClick="btnSendApproval_Click" CausesValidation="true" OnClientClick="return confirm('Are you sure you want to send this lead for approval?')"
                                        ValidationGroup="vgApproval" Text="Send approval" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </asp:Panel>
            <asp:HiddenField ID="lnkDummy" runat="server" />
            <!-- Update Status of Quotation-->

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

