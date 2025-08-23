<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomerJobReport.aspx.cs" EnableEventValidation="false"
Inherits="Reports_CustomerJobReport" MasterPageFile="~/CustomerMaster.master" Title="Job Report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingJob" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
<asp:UpdatePanel ID="upPendingJob" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>
    <div align="center">
        <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
    </div>
    <div>
    <asp:Button ID="btnSubmit" Text="Show Report" runat="server" OnClick="btnSave_Click" />
    <asp:Button ID="btnCancel" Text="Cancel" CssClass="cancel" runat="server" OnClick="btnCancel_Click" CausesValidation="false"  />
    </div>
    <div class="fright">
        <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
            <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
        </asp:LinkButton>
    </div>
      <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
        <tr>
            <td>
                Report Field
            </td>
            <td class="m">
                <asp:CheckBoxList ID="chkField" runat="server" CellPadding="2" CellSpacing="1"  RepeatColumns="5"
                  RepeatLayout="Table" RepeatDirection="horizontal"  DataSourceID="FieldSqlDataSource" DataTextField="sName" DataValueField="QueryName">
                  </asp:CheckBoxList>
            </td>
        </tr>
          
    </table>
                <div class="m clear"></div>
                    <asp:GridView ID="gvReportField" runat="server" AutoGenerateColumns="true" Width="100%" CssClass="table" 
                        PagerStyle-CssClass="pgr" CellPadding="4" AllowPaging="True" PagerSettings-Position="TopAndBottom"
                       DataSourceID="ReportSqlDataSource" PageSize="40" AllowSorting="true" OnSorting="gvReportField_Sorting" OnPreRender="gvReportField_PreRender">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <%#Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerTemplate>
                            <asp:GridViewPager runat="server" />
                        </PagerTemplate>
                    </asp:GridView>
                <div id="divSqlDataSource">
                    <asp:SqlDataSource ID="FieldSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="GetJobQueryField" SelectCommandType="StoredProcedure">
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="ReportSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="rptCustomerQueryField" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:Parameter Name="ColumnList" DbType="String" />
                            <asp:SessionParameter Name="CustId" SessionField="CustId" />
                        </SelectParameters>
                        </asp:SqlDataSource>
                </div>
    </contenttemplate>
</asp:updatepanel>
</asp:Content>
