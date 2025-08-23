<%@ Page Title="Maintenance Emp" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ReportLabour.aspx.cs" 
    Inherits="Transport_ReportLabour" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
 <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
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
            <fieldset><legend>Mechanic Work Hour/Month Report</legend>
                <div class="clear" style="text-align:center;">
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                </div>
                <div>
                <asp:GridView ID="gvSummaryWork" runat="server" CssClass="table" DataSourceID="DataSourceEmp" AutoGenerateColumns="true"
                    AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" PageSize="20" AllowPaging="True" AllowSorting="True" 
                    PagerSettings-Position="TopAndBottom" OnPreRender="gvSummaryWork_PreRender" >
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                </div>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="DataSourceEmp" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TR_rptLabourHourSummary" SelectCommandType="StoredProcedure">
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
 </asp:Content>

