<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ResolveRequest.aspx.cs" Inherits="Service_ResolveRequest" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <asp:ScriptManager runat="server" ID="ScriptManager1" />
    
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
        <div align="center">
            <asp:Label ID="lblError" runat="server"></asp:Label>
        </div>
        <div class="m clear">
        <fieldset><legend>Issue Detail</legend>
        <div class="m clear">
            <asp:Panel ID="pnlFilter" runat="server">
                <div class="fleft">
                    <uc1:DataFilter ID="DataFilter1" runat="server" />
                </div>
            </asp:Panel>
        </div>
        <div class="clear">
        </div>
            <asp:GridView ID="GridViewService" runat="server" AutoGenerateColumns="False" CssClass="table"
                Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                DataSourceID="DataSourceService" CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20" 
                OnRowEditing="GridViewService_RowEditing" OnRowCancelingEdit="GridViewService_RowCancelingEdit"
                OnRowUpdating="GridViewService_RowUpdating">
                <Columns>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" CommandName="Edit" runat="server" Text="Edit" 
                                ToolTip="Click To Update Status"></asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update Detail" runat="server"
                                Text="Update" ValidationGroup="Required"></asp:LinkButton>
                            <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel Detail Update" CausesValidation="false"
                                runat="server" Text="Cancel"></asp:LinkButton>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Emp Name" DataField="EmpName" ReadOnly="true" />
                    <asp:BoundField HeaderText="Dept" DataField="DeptName" ReadOnly="true" />
                    <asp:BoundField HeaderText="Issue Date" DataField="RequestDate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true"/>
                    <asp:BoundField HeaderText="Resolved Date" DataField="ResolvedDate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true"/>
                    <asp:BoundField HeaderText="Resolved By" DataField="ResolvedByName" ReadOnly="true" />
                    <asp:BoundField HeaderText="Status" DataField="StatusName" ReadOnly="true" />
                    <asp:TemplateField HeaderText="Issue">
                        <ItemTemplate>
                            <asp:Label ID="lblIssue" Text='<%# Eval("RequestRemark")%>' runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Remark">
                        <ItemTemplate>
                            <asp:Label ID="lblResolveRemark" Text='<%# BIND("ResolveRemark")%>' runat="server"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtResolveRemark" runat="server" TextMode="MultiLine" Width="200px" MaxLength="800" Text='<%# BIND("ResolveRemark","{0:dd/MM/yyyy}")%>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    
                </Columns>
            </asp:GridView>
        </fieldset>
        <div>
        <asp:SqlDataSource ID="DataSourceService" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="SR_GetPendingDeptIssue" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

