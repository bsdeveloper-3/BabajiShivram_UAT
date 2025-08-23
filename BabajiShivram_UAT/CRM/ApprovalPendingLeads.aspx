<%@ Page Title="Lead Approval" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ApprovalPendingLeads.aspx.cs"
    Inherits="CRM_PendingLeads" EnableEventValidation="false" Culture="en-GB" %>

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

        .tdLead {
            margin-bottom: 15px;
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
                <asp:ValidationSummary ID="vsLeads" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="vgRequired" CssClass="errorMsg" />
            </div>
            <div class="clear">
            </div>

            <fieldset>
                <legend>Lead Approval</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                        </div>
                        <div class="fleft" style="margin-left: 2px;">
                            <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
                                <asp:Image ID="imgExcel" runat="server" ImageUrl="~/Images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
                <div class="clear">
                </div>

                <asp:GridView ID="gvLeads" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr" DataSourceID="DataSourceLeads"
                    DataKeyNames="LeadId" OnRowCommand="gvLeads_RowCommand" AllowPaging="True" AllowSorting="True" CssClass="table" PageSize="20"
                    PagerSettings-Position="TopAndBottom" OnRowDataBound="gvLeads_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="" SortExpression="">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnDownloadQuote" runat="server" CausesValidation="false" CommandName="DownloadQuote"
                                    ImageAlign="AbsMiddle" ImageUrl="~/Images/pdf2.png" Width="16" Height="16" ToolTip="Download Quotation in PDF Format."
                                    CommandArgument='<%#Eval("QuotePath") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Documents">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnShowDocuments" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Click to view documents."
                                    CommandName="ShowDocs" CommandArgument='<%#Eval("LeadId")%>'/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Lead Ref No" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="left" ItemStyle-CssClass="tdLead">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkLead" runat="server" ToolTip="Click to view lead." Text='<%#Eval("LeadRefNo") %>'
                                    CommandName="select" CommandArgument='<%#Eval("LeadId")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField DataField="LeadId" HeaderText="LeadId" SortExpression="LeadId" ReadOnly="true" ItemStyle-Width="10%" Visible="false" />--%>
                        <asp:BoundField DataField="LeadRefNo" HeaderText="Lead Ref No" SortExpression="LeadRefNo" ReadOnly="true" ItemStyle-Width="10%" Visible="false" />
                        <asp:BoundField DataField="CompanyName" HeaderText="Company" SortExpression="CompanyName" ReadOnly="true" ItemStyle-Width="20%" />
                        <asp:BoundField DataField="LeadStageName" HeaderText="Current Status" SortExpression="LeadStageName" ReadOnly="true" ItemStyle-Width="10%" />
                        <%--<asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" ReadOnly="true" ItemStyle-Width="30%" />--%>
                        <asp:TemplateField HeaderText="Remark">
                            <ItemTemplate>
                                <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                <asp:Label ID="lblRemarkView" runat="server" Text='<%#Eval("Remark") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="LeadSourceName" HeaderText="Source" SortExpression="LeadSourceName" ReadOnly="true" ItemStyle-Width="5%" />
                        <asp:BoundField DataField="ContactName" HeaderText="Contact Name" SortExpression="ContactName" ReadOnly="true" ItemStyle-Width="13%" />
                        <asp:BoundField DataField="MobileNo" HeaderText="Mobile No" SortExpression="MobileNo" ReadOnly="true" ItemStyle-Width="5%" />
                        <asp:BoundField DataField="CreatedBy" HeaderText="Lead Owner" SortExpression="CreatedBy" ReadOnly="true" ItemStyle-Width="5%" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LeadDate" ReadOnly="true" ItemStyle-Width="5%" />
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>

            <asp:SqlDataSource ID="DataSourceLeads" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
               SelectCommand="CRM_GetPendingApprovalLeads" SelectCommandType="StoredProcedure" ><%--TEST_CRMLeadApproval--%>
                <SelectParameters>
                    <asp:SessionParameter Name="UserId" SessionField="UserId" />
                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                </SelectParameters>
            </asp:SqlDataSource>

            <div>
                <asp:HiddenField ID="hdnPopup" runat="server" />
                <asp:HiddenField ID="hdnLeadId" runat="server" Value="0" />
                <cc1:ModalPopupExtender ID="mpeDocument" runat="server" TargetControlID="hdnPopup" BackgroundCssClass="modalBackground" CancelControlID="imgClose"
                    PopupControlID="pnlDocument" DropShadow="true">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pnlDocument" runat="server" CssClass="modalPopup1" BackColor="#F5F5DC" Width="670px" Height="335px" BorderStyle="Solid" BorderWidth="1px">
                    <div id="div2" runat="server">
                        <table width="100%">
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td align="center">Download Documents -
                                    <asp:Label ID="lblLeadRefNo" runat="server"></asp:Label>
                                    <span style="float: right">
                                        <asp:ImageButton ID="imgClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgClose_Click" ToolTip="Close" />
                                    </span>
                                </td>
                            </tr>
                        </table>
                        <div align="center">
                            <asp:Panel ID="pnlDownloadDocs" runat="server" Width="660px" Height="300px" ScrollBars="Auto" BorderStyle="Solid" BorderWidth="1px">
                                <asp:GridView ID="GridViewDocument" runat="server" AutoGenerateColumns="False" CssClass="table" Width="100%" AlternatingRowStyle-CssClass="alt"
                                    PagerStyle-CssClass="pgr" DataKeyNames="lid" DataSourceID="DocumentSqlDataSource" OnRowCommand="GridViewDocument_RowCommand"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DocPath" HeaderText="File Name" />
                                        <asp:TemplateField HeaderText="Download">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                                    CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CREATED_DT" HeaderText="Uploaded Date" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="CREATED_BY" HeaderText="Uploaded By" />
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="DocumentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="CRM_GetUploadedDocument" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="hdnLeadId" Name="LeadId" PropertyName="Value" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </asp:Panel>
                        </div>
                    </div>
                </asp:Panel>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

