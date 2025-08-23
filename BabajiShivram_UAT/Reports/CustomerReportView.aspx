<%@ Page Title="Customer Report" Language="C#" MasterPageFile="~/CustomerMaster.master" AutoEventWireup="true" CodeFile="CustomerReportView.aspx.cs" 
    Inherits="Reports_CustomerReportView" EnableEventValidation="false" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="gvPager" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
                <div class="clear"></div>
                <div>
                    <div class="fleft">
                        <uc1:DataFilter ID="DataFilter1" runat="server" />
                    </div>
                    <div class="fleft" style="margin-left:40px;">
                        <asp:CheckBox ID="chkCustomer" runat="server" AutoPostBack="true" Text="Show Customer Report"  Visible="false" /><%-- Checked="true"  Visible="false"--%>
                    </div>    
                </div>
                <div class="clear"></div>
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
                        <%--<asp:BoundField DataField="ReportType" HeaderText="Report Type" SortExpression="ReportType"/>--%>
                        <%--<asp:BoundField DataField="GroupName" HeaderText="Babaji Group" SortExpression="GroupName" />--%>
                        <asp:BoundField DataField="CustomerName" HeaderText="Customer" SortExpression="CustomerName"/>
                        <asp:TemplateField HeaderText="Downloaded On" SortExpression="LastDownloadOn">
                            <ItemTemplate>
                                <asp:Label ID="lblLastdn" runat="server" Text='<%#Eval("LastDownloadOn","{0:dd/MM/yy hh:mm tt}")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
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
                                <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete"  CommandName="DeleteReport" CommandArgument='<%#Eval("lid") %>'
                                    OnClientClick='return confirm("Are you sure to Delete? ");'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerTemplate>
                        <gvPager:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="ViewReportSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CustGetAdhocReport" SelectCommandType="StoredProcedure"> 
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="CustUserId" />
                        <asp:Parameter Name="IsCustomer" DefaultValue="1" />
                    </SelectParameters>
                    <%--<DeleteParameters>
                        DeleteCommand="delCustAdhocReport" DeleteCommandType="StoredProcedure" OnDeleted="DataSourceReport_Deleted"
                        <asp:ControlParameter ControlID="gvReportMS" Name="lid" PropertyName="SelectedValue" />
                        <asp:SessionParameter Name="lUpdUser" SessionField="UserId" />
                        <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
                    </DeleteParameters>--%>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

