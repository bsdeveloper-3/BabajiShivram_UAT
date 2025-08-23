<%@ Page Title="Maintenance Emp" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ReportMaintenance.aspx.cs" 
    Inherits="Transport_ReportMaintenance" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
 <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPanel" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPanel" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <fieldset><legend>Maintenance Work Summary</legend>
                <div class="clear" style="text-align:center;">
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                </div>
                <div>
                <asp:GridView ID="gvSummaryEmp" runat="server" CssClass="table" DataSourceID="DataSourceEmp" AutoGenerateColumns="false"
                    AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" PageSize="20" AllowPaging="True" AllowSorting="True" 
                    PagerSettings-Position="TopAndBottom" OnPreRender="gvSummaryEmp_PreRender" OnRowDataBound="gvSummaryEmp_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Emp" HeaderText="Emp" SortExpression="Emp"/>
                        <asp:BoundField DataField="WorkDate" HeaderText="Work Date" SortExpression="WorkDate" DataFormatString="{0:MM/dd/yyyy}" />
                        <asp:BoundField DataField="WorkHour" HeaderText="Work Hour" SortExpression="WorkHour" />
                        <asp:BoundField DataField="WorkLocation" HeaderText="Location" SortExpression="WorkLocation" />
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <asp:Label ID="lblExpenseDesc" Text='<%#Eval("ShortDesc") %>' runat="server" ></asp:Label>
                                <asp:LinkButton ID="lnkMore" runat="server" Text="...More" ToolTip='<%#Eval("Description") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                </div>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="DataSourceEmp" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TR_rptMaintenanceEmp" SelectCommandType="StoredProcedure">
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
 </asp:Content>

