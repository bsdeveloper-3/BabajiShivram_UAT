<%@ Page Title="Customer Visit Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CustomerVisit.aspx.cs" Inherits="CRM_CustomerVisit" Culture="en-GB" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
    </asp:ScriptManager>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upJobDetail" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upJobDetail" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <asp:Button ID="btnNewVisit" runat="server" Text="New Visit" OnClick="btnNewVisit_Click" TabIndex="1" />
            <asp:Button ID="btnBack" runat="server" Text="Go Back" OnClick="btnBack_Click" TabIndex="2" />
            <fieldset>
                <legend>Customer Visit</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:datafilter id="DataFilter1" runat="server" />
                        </div>
                    </asp:Panel>
                    <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
                        <asp:Image ID="imgExcel" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                </div>
                <div class="clear">
                </div>
                <div>
                    <asp:GridView ID="gvVisitReport" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr" DataSourceID="DataSourceVisitReport"
                        DataKeyNames="lid" AllowPaging="True" AllowSorting="True" CssClass="table" PageSize="20"
                        PagerSettings-Position="TopAndBottom">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Customer" HeaderText="Customer" ReadOnly="true" SortExpression="Customer" />
                            <asp:BoundField DataField="VisitDate" HeaderText="Visit Date" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="CategoryName" HeaderText="Visit Category" ReadOnly="true" />
                            <asp:TemplateField HeaderText="Remark">
                                <ItemTemplate>
                                    <div style="word-wrap: break-word; width: 400px; white-space:normal;">
                                    <asp:Label ID="lblRemarkView" runat="server" Text='<%#Eval("Remark") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CreatedBy" HeaderText="Created By" ReadOnly="true" />
                            <asp:BoundField DataField="CreatedDate" HeaderText="CreatedDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                        </Columns>
                        <PagerTemplate>
                            <asp:GridViewPager runat="server" />
                        </PagerTemplate>
                    </asp:GridView>
                </div>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="DataSourceVisitReport" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CRM_GetCustomerVisitReport" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
