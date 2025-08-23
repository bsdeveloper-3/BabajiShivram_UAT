<%@ Page Title="Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FreightReportTrack.aspx.cs" 
    Inherits="Freight_FreightReportTrack" Culture="en-GB" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="gvPager" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" ID="content1">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" runat="server" AssociatedUpdatePanelID="upAdhocReport">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upAdhocReport" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div class="clear">
            </div>
            <fieldset><legend>Report List</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                        </div>
                    </asp:Panel>
                </div>
                <div class="clear">
                </div>
                <asp:GridView ID="gvReportMS" runat="server" AutoGenerateColumns="False" CssClass="table"
                    AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lid"
                    OnRowCommand="gvReportMS_RowCommand" AllowPaging="True" AllowSorting="True" PagerSettings-Position="TopAndBottom"
                    OnPreRender="gvReportMS_PreRender" DataSourceID="ViewReportSqlDataSource">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ReportName" HeaderText="Report Name" SortExpression="ReportName" />
                        <asp:BoundField DataField="UserName" HeaderText="Prepared By" SortExpression="UserName" />
                        <asp:TemplateField HeaderText="Prepared On" SortExpression="dtDate">
                            <ItemTemplate>
                                <asp:Label ID="lblPrepareDate" runat="server" Text='<%#Eval("dtDate","{0:dd/MM/yyyy hh:mm tt}")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="Downloaded On" SortExpression="LastDownloadOn">
                            <ItemTemplate>
                                <asp:Label ID="lblLastdn" runat="server" Text='<%#Eval("LastDownloadOn","{0:dd/MM/yyyy hh:mm tt}")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblid" runat="server" Text='<%#Eval("lid")%>' Visible="false"></asp:Label>
                                <asp:Label ID="lblReportName" runat="server" Text='<%#Eval("ReportName")%>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Generate/Execute">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkGenerate" runat="server" Text="Generate Report" CommandName="GenerateReport"
                                    CommandArgument='<%#Eval("lid") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="25%" HeaderText="Modify">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="Edit" CommandArgument='<%#Eval("lid")%>'></asp:LinkButton>
                                <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="Delete"
                                    OnClientClick='return confirm("Are you sure to Delete? ");'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerTemplate>
                        <gvPager:GridViewPager ID="GridViewPager1" runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="ViewReportSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="FR_GetAdhocReport" SelectCommandType="StoredProcedure" DeleteCommand="FR_delAdHocReport"
                    DeleteCommandType="StoredProcedure" OnDeleted="DataSourceReport_Deleted">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:ControlParameter ControlID="gvReportMS" Name="lid" PropertyName="SelectedValue" />
                        <asp:SessionParameter Name="lUserId" SessionField="UserId" />
                        <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
                    </DeleteParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
